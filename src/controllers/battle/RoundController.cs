using Godot;
using System.Linq;
using DiceRolling.Stores;

namespace DiceRolling.Controllers;

/// <summary>
/// Gerencia o fluxo de rounds da batalha.
/// </summary>
/// <remarks>
/// O <c>RoundController</c> é responsável por coordenar a progressão da batalha, garantindo que cada round siga a sequência correta de fases.
///     <list type="table">
///         <listheader>
///             <term>Phase 2: Gestão dos rounds</term>
///             <description>Controle do início, progresso e término dos rounds</description>
///         </listheader>
///         <item>- Inicia um novo round</item>
///         <item>- Inicia a fase de declaração de ações (<c>ActionsController</c>)</item>
///         <item>- Inicia a fase de resolução de turnos (<c>TurnController</c>)</item>
///         <item>- Finaliza o round</item>
///         <item>- Verifica o estado da batalha para decidir se deve continuar ou terminar</item>
///     </list>
/// </remarks>
[GlobalClass]
public partial class RoundController : Node {
    private ActionsController? _actionsController;
    private TurnController? _turnController;
    private RoundState _currentRoundState = RoundState.RoundStart;
    private int _currentRound = 0;
    public RoundState CurrentRoundState => _currentRoundState;
    public int CurrentRound => _currentRound;

    public override void _Ready() {
        // Defer getting references until all controllers are initialized
        CallDeferred(nameof(InitializeReferences));
        ConnectEvents();
    }

    private void InitializeReferences() {
        // Get references to other controllers from sibling nodes
        _actionsController = GetParent()?.GetNode<ActionsController>("ActionsController");
        _turnController = GetParent()?.GetNode<TurnController>("TurnController");
    }

    public override void _ExitTree() {
        DisconnectEvents();
    }

    private void ConnectEvents() {
        DisconnectEvents();
        BattleEvents.Instance.TransitionedToRounds += OnTransitionedToRounds;
        BattleEvents.Instance.ActionsDeclared += OnActionsDeclared;
        BattleEvents.Instance.TurnsResolved += OnTurnsResolved;
    }

    private void DisconnectEvents() {
        if (BattleEvents.Instance != null) {
            BattleEvents.Instance.TransitionedToRounds -= OnTransitionedToRounds;
            BattleEvents.Instance.ActionsDeclared -= OnActionsDeclared;
            BattleEvents.Instance.TurnsResolved -= OnTurnsResolved;
        }
    }

    private void SetRoundState(RoundState newState) {
        GD.PrintRich($"[color=violet][RoundController] Round state changing: {_currentRoundState} -> {newState}.[/color]");
        _currentRoundState = newState;
    }

    private void AdvanceRound() {
        _currentRound++;
        GD.PrintRich($"[color=violet][RoundController] Advanced to round {_currentRound}.[/color]");
    }

    public void StartRound() {
        // Wait for initialization if controllers are not ready yet
        if (_actionsController == null || _turnController == null) {
            GD.PrintRich("[color=violet][RoundController] Controllers not initialized yet, deferring StartRound...[/color]");
            CallDeferred(nameof(StartRound));
            return;
        }

        GD.PrintRich("[color=violet][RoundController] Starting a new round...[/color]");

        SetRoundState(RoundState.RoundStart);

        AdvanceRound();

        BattleEvents.Instance.EmitRoundStarted(_currentRound);

        StartActionsDeclarationPhase();
    }

    private void StartActionsDeclarationPhase() {
        if (_actionsController == null) {
            GD.PrintErr("[RoundController] ActionsController not initialized!");
            return;
        }

        GD.PrintRich("[color=violet][RoundController] Starting actions declaration phase...[/color]");

        SetRoundState(RoundState.ActionsDeclaration);

        var playerTeam = BattleController.Instance.GetPlayerTeam();
        var enemyTeam = BattleController.Instance.GetEnemyTeam();
        _actionsController.StartActionsDeclaration(playerTeam, enemyTeam);
    }

    public void StartTurnsResolutionPhase() {
        if (_turnController == null) {
            GD.PrintErr("[RoundController] TurnController not initialized!");
            return;
        }

        GD.PrintRich("[color=violet][RoundController] Starting turns resolution phase...[/color].");

        SetRoundState(RoundState.TurnsResolution);

        _turnController.StartTurnsResolution();
    }

    // Finaliza o round atual
    public void EndRound() {
        GD.PrintRich("[color=violet][RoundController] Ending the current round...[/color]");

        SetRoundState(RoundState.RoundEnd);

        BattleEvents.Instance.EmitRoundEnded(_currentRound);

        CheckBattleState();
    }

    // ? Deve ser feito aqui, no TurnController ou em outro lugar?
    // Verifica o estado da batalha para decidir se deve continuar ou terminar
    // Verifica se um novo turno deve começar ou se a rodada deve terminar
    // Verifica se uma nova rodada deve começar ou se a batalha deve terminar
    private void CheckBattleState() {
        GD.PrintRich("[color=violet][RoundController] Checking battle state...[/color]");

        // Use the same logic as TurnController.ShouldContinueBattle for now
        // TODO: Refactor this check into BattleResultsController or a shared service
        var battleController = BattleController.Instance;
        if (battleController == null) {
            GD.PrintErr("[RoundController] BattleController instance is null. Cannot check battle state.");
            // Decide how to handle this - maybe end battle?
            // BattleEvents.Instance.EmitBattleEnded(BattleResult.Error); // Example
            return;
        }

        var playerTeam = battleController.GetPlayerTeam();
        var enemyTeam = battleController.GetEnemyTeam();

        var attributesStore = AttributesStore.Instance;
        var healthAttribute = attributesStore.GetAttributeByName("Health");

        if (healthAttribute == null) {
            GD.PrintErr("[RoundController] Health attribute not found. Cannot determine battle end.");
            // Decide how to handle this - maybe end battle?
            // BattleEvents.Instance.EmitBattleEnded(BattleResult.Error); // Example
            return;
        }

        bool hasPlayerAlive = playerTeam.Any(p => p.GetAttributeCurrentValue(healthAttribute) > 0);
        bool hasEnemyAlive = enemyTeam.Any(e => e.GetAttributeCurrentValue(healthAttribute) > 0);

        // If battle should continue (both teams have members alive)
        if (hasPlayerAlive && hasEnemyAlive) {
            GD.PrintRich("[color=violet][RoundController] Battle continues. Starting next round.[/color]");
            // If the battle should continue, initiate the next round.
            // Use CallDeferred to avoid recursion issues
            CallDeferred(nameof(StartRound));
        }
        // If battle should end
        else {
            GD.PrintRich($"[color=violet][RoundController] Battle ended. Players alive: {hasPlayerAlive}, Enemies alive: {hasEnemyAlive}.[/color]");
            // TODO: Delegate to BattleResultsController or emit BattleEnded event
            // For now, just stop the loop.
            // Example: BattleEvents.Instance.EmitBattleEnded(hasPlayerAlive ? BattleResult.Victory : BattleResult.Defeat);
            SetRoundState(RoundState.RoundEnd); // Ensure state is final
            GD.PrintRich("[color=violet][RoundController] Battle finished. No new round started.[/color]");
        }
    }
    // Eventos
    private void OnTransitionedToRounds(int number) {
        GD.PrintRich("[color=violet]Event TransitionedToRounds fired on RoundController.[/color]");
        StartRound();
    }

    private void OnActionsDeclared() {
        GD.PrintRich("[color=violet]Event ActionsDeclared fired on RoundController.[/color]");
        if (_currentRoundState == RoundState.ActionsDeclaration) {
            StartTurnsResolutionPhase();
        }
        else {
            GD.PrintErr($"RoundController received ActionsDeclared signal in unexpected state: {_currentRoundState}");
        }
    }

    private void OnTurnsResolved() {
        GD.PrintRich("[color=violet]Event TurnsResolved fired on RoundController.[/color]");
        EndRound();
    }
}