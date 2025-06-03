using Godot;
using System.Linq;
using System.Collections.Generic;

using DiceRolling.Characters;

namespace DiceRolling.Controllers;

/// <summary>
/// Gerencia a fila de iniciativa dos personagens durante a batalha.
/// </summary>
/// <remarks>
/// O <c>QueueController</c> é responsável por controlar a ordem em que os personagens irão agir durante a batalha.
///     <list type="table">
///         <listheader>
///             <term>Gestão da iniciativa</term>
///             <description>Calcula, atualiza e mantém a ordem de ação dos personagens</description>
///         </listheader>
///         <item>- Calcula a ordem de iniciativa inicial com base nos atributos dos personagens</item>
///         <item>- Gerencia modificadores de iniciativa que podem alterar a ordem durante a batalha</item>
///         <item>- Atualiza a fila de iniciativa quando personagens entram ou saem da batalha</item>
///         <item>- Fornece informações sobre os próximos personagens a agir</item>
///     </list>
/// </remarks>
public partial class QueueController : RefCounted {
    // The initiative queue (order of characters' turns)
    private Queue<CharacterType> _initiativeQueue = new Queue<CharacterType>();
    public IEnumerable<CharacterType> InitiativeQueue => _initiativeQueue;

    public QueueController() {
        ConnectEvents();
    }

    private void ConnectEvents() {
        DisconnectEvents();
        BattleEvents.Instance.CharactersPositioned += OnCharactersPositioned;
        BattleEvents.Instance.CharacterAddedToQueue += OnCharacterAddedToQueue;
        BattleEvents.Instance.CharacterRemovedFromQueue += OnCharacterRemovedFromQueue;
        BattleEvents.Instance.CharacterInitiativeModified += OnCharacterInitiativeModified;

    }

    private void DisconnectEvents() {
        if (BattleEvents.Instance != null) {
            BattleEvents.Instance.CharactersPositioned -= OnCharactersPositioned;
            BattleEvents.Instance.CharacterAddedToQueue -= OnCharacterAddedToQueue;
            BattleEvents.Instance.CharacterRemovedFromQueue -= OnCharacterRemovedFromQueue;
            BattleEvents.Instance.CharacterInitiativeModified -= OnCharacterInitiativeModified;
        }
    }

    private void CalculateInitialOrder(Godot.Collections.Array characters) {
        _initiativeQueue.Clear();

        // Add all characters to the queue
        foreach (CharacterType character in characters.Cast<Godot.Variant>().Select(v => v.As<CharacterType>())) {
            _initiativeQueue.Enqueue(character);
        }

        // Sort by initiative (descending)
        SortQueue();

        // Notify that the queue has been created
        BattleEvents.Instance.EmitInitiativeQueueCreated(new Godot.Collections.Array(_initiativeQueue.ToArray()));

    }

    // Sort the initiative queue based on character initiative values
    private void SortQueue() {
        // Convert queue to list, sort it, then rebuild the queue
        var sortedCharacters = _initiativeQueue.OrderByDescending(c => GetCharacterInitiative(c)).ToList();
        _initiativeQueue = new Queue<CharacterType>(sortedCharacters);

        // Print queue information
        GD.PrintRich("[color=pink]=== INITIATIVE QUEUE ===[/color]");
        int i = 1;
        foreach (var character in _initiativeQueue) {
            int initiative = GetCharacterInitiative(character);

            // Determine team based on location
            string team = "Unknown";
            if (character.Location == BattleController.Instance.PlayerSquadLocation) {
                team = "Player";
            }
            else if (character.Location == BattleController.Instance.EnemySquadLocation) {
                team = "Enemy";
            }

            GD.PrintRich($"[color=pink]{i++}. {character.Name} (Team: {team}) - Initiative: {initiative}.[/color]");
        }
        GD.PrintRich("[color=pink]======================[/color]");
    }

    private static int GetCharacterInitiative(CharacterType character) {
        var speedAttribute = character.Attributes.Cast<CharacterAttribute>()
            .FirstOrDefault(attr => attr.Type?.Name == "Speed");

        int baseSpeed = speedAttribute != null ? speedAttribute.CurrentValue : 0;

        // If no Speed attribute found, log a warning
        if (speedAttribute == null) {
            GD.PrintRich($"[color=pink]Character {character.Name} doesn't have a Speed attribute.[/color]");
        }

        // TODO Consider any active initiative modifiers
        // int initiativeModifiers = 0;
        // if (character.ActiveEffects != null) {
        //     foreach (var effect in character.ActiveEffects) {
        //         if (effect.AffectsInitiative) {
        //             initiativeModifiers += effect.InitiativeModifierValue;
        //         }
        //     }
        // }

        return baseSpeed;
        // return baseSpeed + initiativeModifiers;
    }

    public void AddCharacterToQueue(CharacterType character) {
        GD.PrintRich($"[color=pink]QueueController: Adding character {character.Name} to initiative queue.[/color]");
        if (!_initiativeQueue.Contains(character)) {
            _initiativeQueue.Enqueue(character);
            SortQueue();
            BattleEvents.Instance.EmitCharacterAddedToQueue(character);
        }
    }

    // Removes a character from the initiative queue
    public void RemoveCharacterFromQueue(CharacterType character) {
        GD.PrintRich($"[color=pink]QueueController: Removing character {character.Name} from initiative queue.[/color]");

        if (_initiativeQueue.Contains(character)) {
            // Create a temporary queue to hold characters
            var tempQueue = new Queue<CharacterType>();

            // Dequeue until we find the character to remove
            while (_initiativeQueue.Count > 0) {
                var currentCharacter = _initiativeQueue.Dequeue();
                if (currentCharacter != character) {
                    tempQueue.Enqueue(currentCharacter);
                }
            }

            // Restore the queue with the remaining characters
            _initiativeQueue = tempQueue;
        }

        BattleEvents.Instance.EmitCharacterRemovedFromQueue(character);
    }

    public void MoveCharacterToEndOfQueue(CharacterType character) {
        GD.PrintRich($"[color=pink]QueueController: Moving character {character.Name} to end of initiative queue.[/color]");

        // Remove the character and add them back to the end of the queue
        RemoveCharacterFromQueue(character);
        AddCharacterToQueue(character);

        BattleEvents.Instance.EmitCharacterMovedToEndOfQueue(character);
    }

    public CharacterType? GetNextCharacter() {
        return _initiativeQueue.Count > 0 ? _initiativeQueue.Peek() : null;
    }

    public bool IsQueueEmpty() {
        return _initiativeQueue.Count == 0;
    }

    // Event handlers
    private void OnCharactersPositioned(Godot.Collections.Array characters) {
        GD.PrintRich("[color=pink]Event CharactersPositioned fired on QueueController, calculating initial order.[/color]");
        CalculateInitialOrder(characters);
    }

    private void OnCharacterAddedToQueue(CharacterType character) {
        GD.PrintRich("[color=pink]Event CharacterAddedToQueue fired on QueueController, re-sorting queue.[/color]");
        SortQueue();
    }

    private void OnCharacterRemovedFromQueue(CharacterType character) {
        GD.PrintRich("[color=pink]Event CharacterRemovedFromQueue fired on QueueController, re-sorting queue.[/color]");
        SortQueue();
    }

    private void OnCharacterInitiativeModified(CharacterType character) {
        GD.PrintRich("[color=pink]Event CharacterInitiativeModified fired on QueueController, re-sorting queue.[/color]");
        SortQueue();
    }
}