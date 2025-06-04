using Godot;
using System.Linq;
using System.Collections.Generic;
using DiceRolling.Characters;
using DiceRolling.Stores;
using DiceRolling.Actions;
using DiceRolling.Services;

namespace DiceRolling.Controllers;

/// <summary>
/// Gerencia a resolução dos turnos dos personagens durante a batalha.
/// </summary>
/// <remarks>
/// O <c>TurnController</c> é responsável por controlar a execução das ações dos personagens durante a batalha.
///     <list type="table">
///         <listheader>
///             <term>Resolução de turnos</term>
///             <description>Controle do fluxo de ações dentro de um turno</description>
///         </listheader>
///         <item>- Inicia o turno de um personagem</item>
///         <item>- Gerencia a execução de ações do personagem</item>
///         <item>- Finaliza o turno de um personagem</item>
///     </list>
/// </remarks>
[GlobalClass]
public partial class TurnController : Node {
    private QueueController? _queueController;
    private IReadOnlyDictionary<CharacterType, DeclaredActionInfo>? _declaredActions; // Store the declared actions

    public override void _Ready() {
        // Defer getting references until all controllers are initialized
        CallDeferred(nameof(InitializeReferences));
        ConnectEvents();
    }

    private void InitializeReferences() {
        // Get reference to QueueController from sibling nodes
        _queueController = GetParent()?.GetNode<QueueController>("QueueController");
    }

    public override void _ExitTree() {
        DisconnectEvents();
    }

    private void ConnectEvents() {
        DisconnectEvents();
    }

    private void DisconnectEvents() {
    }

    public void StartTurnsResolution() {
        // Wait for initialization if QueueController is not ready yet
        if (_queueController == null) {
            GD.PrintRich("[color=pink]TurnController: QueueController not initialized yet, deferring StartTurnsResolution...[/color]");
            CallDeferred(nameof(StartTurnsResolution));
            return;
        }

        GD.PrintRich("[color=pink]TurnController: Starting turns resolution.[/color]");
        _declaredActions = BattleController.Instance.ActionsController?.DeclaredActions;

        if (_declaredActions == null) {
            // Log a more severe error as this indicates a potential flow issue
            GD.PrintErr("TurnController: CRITICAL ERROR - Declared actions not available! Cannot resolve turns.");
            // TODO: Implement robust error handling here. Options include:
            BattleEvents.Instance.EmitTurnsResolved(); // Emit TurnsResolved even on error for now
            return;
        }
        ProcessNextCharacterTurn();
    }

    // Process the turn for the next character in the queue
    private void ProcessNextCharacterTurn() {
        if (_queueController == null) {
            GD.PrintErr("TurnController: QueueController is null.");
            BattleEvents.Instance.EmitTurnsResolved(); // End phase if no queue
            return;
        }

        // Check win/loss conditions before getting the next character
        if (!ShouldContinueBattle()) {
            GD.PrintRich("[color=pink]TurnController: Battle ending condition met. Stopping turn resolution.[/color]");
            _declaredActions = null; // Clear reference
            BattleEvents.Instance.EmitTurnsResolved(); // Signal that resolution phase is done (battle might end)
            return;
        }

        CharacterType? nextCharacter = _queueController.GetNextCharacter(); // Use GetNextCharacter which likely dequeues

        if (nextCharacter != null) {
            StartCharacterTurn(nextCharacter);
        }
        else {
            GD.PrintRich("[color=pink]TurnController: Initiative queue is empty. All turns resolved for this round.[/color]");
            _declaredActions = null; // Clear reference
            BattleEvents.Instance.EmitTurnsResolved(); // Signal that all turns for this round are done
        }
    }

    // Start a character's turn
    private void StartCharacterTurn(CharacterType character) {
        GD.PrintRich($"[color=pink]TurnController: Starting turn for {character.Name}.[/color]");
        BattleEvents.Instance.EmitTurnStarted(character);

        // Immediately execute the declared action
        ExecuteCharacterAction(character);
    }

    // Execute the action declared by the character
    private void ExecuteCharacterAction(CharacterType character) {
        if (_declaredActions == null || !_declaredActions.TryGetValue(character, out DeclaredActionInfo? declaredInfo) || declaredInfo == null) {
            GD.PrintErr($"TurnController: No declared action found for {character.Name}. Skipping turn execution.");
            // Proceed to end turn even if action is missing
            BattleEvents.Instance.EmitTurnEnded(character);
            // QueueController should handle moving character on TurnEnded event
            ProcessNextCharacterTurn(); // Move to the next character
            return;
        }

        if (declaredInfo.IsPass()) {
            GD.PrintRich($"[color=pink]TurnController: {character.Name} passes the turn.[/color]");
        }
        else if (declaredInfo.Action != null) {
            GD.PrintRich($"[color=pink]TurnController: Executing action '{declaredInfo.Action.Name}' for {character.Name} targeting {declaredInfo.Target?.Name ?? "None"}.[/color]");

            // Create context
            var context = new ActionContext(character, declaredInfo.Target);

            // Execute the action
            bool success = declaredInfo.Action.Do(context); // Or use CharacterAction.Resolve if appropriate

            if (success) {
                // Consume the energy AFTER successful execution
                ActionService.ConsumeEnergy(character, declaredInfo.Action);
                GD.PrintRich($"[color=pink]TurnController: Action '{declaredInfo.Action.Name}' performed successfully by {character.Name}.[/color]");
                BattleEvents.Instance.EmitActionPerformed(character); // Signal action success
            }
            else {
                GD.PrintRich($"[color=orange]TurnController: Action '{declaredInfo.Action.Name}' failed for {character.Name}.[/color]");
                // Handle action failure if needed (e.g., emit a different event)
            }
        }

        // End the turn
        BattleEvents.Instance.EmitTurnEnded(character);
        // QueueController should listen to TurnEnded and move the character to the end of the queue or remove if dead

        // Process the next character
        // Add a slight delay or wait for animations if needed before processing next turn
        // CallDeferred(nameof(ProcessNextCharacterTurn)); // Example using CallDeferred
        ProcessNextCharacterTurn(); // Call directly for now
    }

    private bool ShouldContinueBattle() {
        // Check if both teams still have active characters
        var battleController = BattleController.Instance;
        if (battleController == null) {
            GD.PrintErr("TurnController: BattleController instance is null. Cannot check battle state.");
            return false; // Cannot determine, assume end
        }

        // Get alive characters from each team
        var playerTeam = battleController.GetPlayerTeam();
        var enemyTeam = battleController.GetEnemyTeam();

        // Get attributes store for health check - Consider making AttributesStore a singleton or injecting it
        var attributesStore = AttributesStore.Instance; // Use singleton instance
        var healthAttribute = attributesStore.GetAttributeByName("Health"); // Use method from store

        if (healthAttribute == null) {
            GD.PrintErr("TurnController: Health attribute not found in AttributesStore.");
            return true; // Continue battle if we can't check health properly
        }

        // Check if there are alive characters in both teams
        bool hasPlayerAlive = playerTeam.Any(p => p.GetAttributeCurrentValue(healthAttribute) > 0);
        bool hasEnemyAlive = enemyTeam.Any(e => e.GetAttributeCurrentValue(healthAttribute) > 0);

        // If either team has no living characters, battle should end
        if (!hasPlayerAlive || !hasEnemyAlive) {
            GD.PrintRich($"[color=pink]TurnController: Battle ending condition met - Players alive: {hasPlayerAlive}, Enemies alive: {hasEnemyAlive}.[/color]");
            return false;
        }

        GD.PrintRich("[color=pink]TurnController: Battle continuing - both teams have active characters.[/color]");
        return true;
    }
}