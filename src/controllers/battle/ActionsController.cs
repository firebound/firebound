using Godot;
using DiceRolling.Characters;
using System.Collections.Generic;
using DiceRolling.Services;

namespace DiceRolling.Controllers;

/// <summary>
/// Gerencia a declaração de ações dos personagens durante a batalha.
/// </summary>
/// <remarks>
/// O <c>ActionsController</c> é responsável por coordenar o momento em que personagens e inimigos escolhem suas ações antes da execução dos turnos.
///     <list type="table">
///         <listheader>
///             <term>Declaração de ações</term>
///             <description>Processo de escolha e preparação das ações</description>
///         </listheader>
///         <item>- Gerencia a declaração das ações dos inimigos</item>
///         <item>- Gerencia a rolagem de dados para coleta de energia dos personagens</item>
///         <item>- Recebe comandos de declaração de ações dos personagens dos jogadores</item>
///     </list>
/// </remarks>
public partial class ActionsController : RefCounted {
    private readonly HashSet<CharacterType> _charactersDeclaredActions = [];
    private readonly Dictionary<CharacterType, DeclaredActionInfo> _declaredActions = new();
    private List<CharacterType> _playerTeam = [];
    private List<CharacterType> _enemyTeam = [];

    public IReadOnlyDictionary<CharacterType, DeclaredActionInfo> DeclaredActions => _declaredActions;

    public ActionsController() {
        ConnectEvents();
    }

    private void ConnectEvents() {
        DisconnectEvents();
        BattleEvents.Instance.PlayerActionDeclared += OnPlayerActionDeclared;
        BattleEvents.Instance.PlayerTargetSelected += OnPlayerTargetSelected;
        BattleEvents.Instance.PlayerActionCancelled += OnPlayerActionCancelled;
        BattleEvents.Instance.EnemyActionDeclared += OnEnemyActionDeclared;
    }

    private void DisconnectEvents() {
        if (BattleEvents.Instance != null) {
            BattleEvents.Instance.PlayerActionDeclared -= OnPlayerActionDeclared;
            BattleEvents.Instance.PlayerTargetSelected -= OnPlayerTargetSelected;
            BattleEvents.Instance.PlayerActionCancelled -= OnPlayerActionCancelled;
            BattleEvents.Instance.EnemyActionDeclared -= OnEnemyActionDeclared;
        }
    }

    public void StartActionsDeclaration(List<CharacterType> playerTeam, List<CharacterType> enemyTeam) {
        GD.PrintRich("[color=cyan]ActionsController: Starting actions declaration phase...[/color]");

        _playerTeam = playerTeam;
        _enemyTeam = enemyTeam;
        _charactersDeclaredActions.Clear();
        _declaredActions.Clear();

        GD.PrintRich($"[color=cyan]ActionsController: Using teams - Players: {_playerTeam.Count}, Enemies: {_enemyTeam.Count}.[/color]");

        GD.PrintRich("[color=cyan]ActionsController: Rolling dice for player characters...[/color]");

        foreach (var playerCharacter in _playerTeam) {
            playerCharacter.RollEquippedDiceForEnergy();
        }

        GD.PrintRich("[color=cyan]ActionsController: Player dice rolling complete.[/color]");

        DeclareEnemyActionsForTesting();
        DeclarePlayerActionsForTesting();
    }

    private void DeclareEnemyActionsForTesting() {
        GD.PrintRich("[color=cyan]ActionsController: Declaring enemy actions (with validation)...[/color]");

        foreach (var enemy in _enemyTeam) {
            bool actionDeclared = false;
            if (enemy.Actions == null || enemy.Actions.Count == 0 || _playerTeam.Count == 0) {
                GD.PrintRich($"[color=cyan]Enemy {enemy.Name} has no actions or no player targets.[/color]");
                // Still need to emit declared signal even if no action is possible
                BattleEvents.Instance.EmitEnemyActionDeclared(enemy);
                continue;
            }

            // Shuffle actions for variety if desired
            var shuffledActions = new Godot.Collections.Array<CharacterAction>(enemy.Actions);
            shuffledActions.Shuffle();

            foreach (CharacterAction charAction in shuffledActions) {
                var actionType = charAction.Type;
                if (actionType == null) continue;

                // Check Energy
                if (!ActionService.CanAffordAction(enemy, actionType)) {
                    GD.Print($"Enemy {enemy.Name} cannot afford {actionType.Name}");
                    continue;
                }

                // Check Targets
                List<CharacterType> validTargets = ActionService.GetValidTargets(enemy, actionType, _playerTeam);

                if (validTargets.Count > 0) {
                    var target = validTargets[GD.RandRange(0, validTargets.Count - 1)];

                    _declaredActions[enemy] = new DeclaredActionInfo(actionType, target);

                    BattleEvents.Instance.EmitEnemyActionDeclared(enemy);
                    GD.PrintRich($"[color=cyan]Enemy {enemy.Name} declared action {actionType.Name} targeting {target.Name}. (VALIDATED)[/color]");
                    actionDeclared = true;
                    break;
                }
                else { GD.Print($"Enemy {enemy.Name} found no valid targets for {actionType.Name}"); }
            }

            if (!actionDeclared) {
                // Store a "Pass" action if none was valid
                _declaredActions[enemy] = new DeclaredActionInfo(); // Pass action
                GD.PrintRich($"[color=cyan]Enemy {enemy.Name} could not find any valid action to declare (Pass).[/color]");
                BattleEvents.Instance.EmitEnemyActionDeclared(enemy);
            }
        }
    }

    private void DeclarePlayerActionsForTesting() {
        GD.PrintRich("[color=cyan]ActionsController: Declaring player actions (with validation)...[/color]");

        foreach (var player in _playerTeam) {
            bool actionDeclared = false;
            if (player.Actions == null || player.Actions.Count == 0 || _enemyTeam.Count == 0) {
                GD.PrintRich($"[color=cyan]Player {player.Name} has no actions or no enemy targets.[/color]");
                // Still need to emit declared signal even if no action is possible
                BattleEvents.Instance.EmitPlayerActionDeclared(player);
                continue;
            }

            var shuffledActions = new Godot.Collections.Array<CharacterAction>(player.Actions);
            shuffledActions.Shuffle();

            foreach (CharacterAction charAction in shuffledActions) {
                var actionType = charAction.Type;
                if (actionType == null) continue;

                // Check Energy
                if (!ActionService.CanAffordAction(player, actionType)) {
                    GD.Print($"Player {player.Name} cannot afford {actionType.Name}");
                    continue;
                }

                // Check Targets
                List<CharacterType> validTargets = ActionService.GetValidTargets(player, actionType, _enemyTeam);

                if (validTargets.Count > 0) {
                    var target = validTargets[GD.RandRange(0, validTargets.Count - 1)];

                    // Store the declared action and target
                    _declaredActions[player] = new DeclaredActionInfo(actionType, target);

                    BattleEvents.Instance.EmitPlayerActionDeclared(player);
                    GD.PrintRich($"[color=cyan]Player {player.Name} declared action {actionType.Name} targeting {target.Name}. (VALIDATED)[/color]");
                    actionDeclared = true;
                    break;
                }
                else { GD.Print($"Player {player.Name} found no valid targets for {actionType.Name}"); }
            }

            if (!actionDeclared) {
                // Store a "Pass" action if none was valid
                _declaredActions[player] = new DeclaredActionInfo(); // Pass action
                GD.PrintRich($"[color=cyan]Player {player.Name} could not find any valid action to declare (Pass).[/color]");
                BattleEvents.Instance.EmitPlayerActionDeclared(player);
            }
        }
    }

    private bool AllActionsAreDeclared() {
        GD.PrintRich("[color=cyan]ActionsController: Checking if all actions are declared...[/color]");

        int totalCharacters = _playerTeam.Count + _enemyTeam.Count;

        GD.PrintRich($"[color=cyan]ActionsController: {_charactersDeclaredActions.Count}/{totalCharacters} actions declared.[/color]");

        return _charactersDeclaredActions.Count >= totalCharacters;
    }

    private static void CompleteActionsDeclaration() {
        GD.PrintRich("[color=cyan]ActionsController: Completing actions declaration phase...[/color]");
        BattleEvents.Instance.EmitActionsDeclared();
    }

    private void OnEnemyActionDeclared(CharacterType character) {
        GD.PrintRich($"[color=cyan]Event EnemyActionDeclared fired on ActionsController.[/color]");
        _charactersDeclaredActions.Add(character);
        CheckIfAllActionsDeclared();
    }

    private void OnPlayerActionDeclared(CharacterType character) {
        GD.PrintRich("[color=cyan]Event PlayerActionDeclared fired on ActionsController.[/color]");
        _charactersDeclaredActions.Add(character);
        CheckIfAllActionsDeclared();
    }

    private void OnPlayerTargetSelected(CharacterType character, CharacterType target) {
        GD.PrintRich("[color=cyan]Event PlayerTargetSelected fired on ActionsController.[/color]");
        CheckIfAllActionsDeclared();
    }

    private void OnPlayerActionCancelled(CharacterType character) {
        GD.PrintRich("[color=cyan]Event PlayerActionCancelled fired on ActionsController.[/color]");
        _charactersDeclaredActions.Remove(character);
        CheckIfAllActionsDeclared();
    }

    private void CheckIfAllActionsDeclared() {
        if (AllActionsAreDeclared()) {
            CallDeferred(nameof(CompleteActionsDeclaration));
        }
    }
}