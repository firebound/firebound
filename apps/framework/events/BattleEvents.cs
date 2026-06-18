using Godot;
using DiceRolling.Characters;

namespace DiceRolling.Controllers;

public partial class BattleEvents : Node {
    private static BattleEvents? _instance;

    public static BattleEvents Instance {
        get {
            if (_instance == null) {
                // Try to get from the scene tree first
                if (Engine.GetMainLoop() is SceneTree tree) {
                    _instance = tree.Root.GetNodeOrNull<BattleEvents>("/root/BattleEvents");
                }

                // If not found, create a new instance
                _instance ??= new BattleEvents();
            }
            return _instance;
        }
    }

    // Battle State Events
    //  - Battle started
    //  - Battle paused
    //  - Battle resumed
    //  - Battle ended
    [Signal] public delegate void BattleStartedEventHandler(Godot.Collections.Array playerTeam, Godot.Collections.Array enemyTeam);
    [Signal] public delegate void BattlePausedEventHandler();
    [Signal] public delegate void BattleResumedEventHandler();
    [Signal] public delegate void BattleEndedEventHandler(bool victory);

    // Battle State Signals
    public void EmitBattleStarted(Godot.Collections.Array playerTeam, Godot.Collections.Array enemyTeam) => EmitSignal(nameof(BattleStarted), playerTeam, enemyTeam);
    public void EmitBattlePaused() => EmitSignal(nameof(BattlePaused));
    public void EmitBattleResumed() => EmitSignal(nameof(BattleResumed));
    public void EmitBattleEnded(bool victory) => EmitSignal(nameof(BattleEnded), victory);

    // Phase 1: Battle setup Events
    //  - Step 1: Enemies generation
    //  - Step 2: Characters position
    //  - Step 3: Initiative queue setup
    //  - Step 4: Transition to Rounds Phase
    [Signal] public delegate void EnemiesGeneratedEventHandler(Godot.Collections.Array enemies);
    [Signal] public delegate void CharactersPositionedEventHandler(Godot.Collections.Array characters);
    [Signal] public delegate void InitiativeQueueSetupEventHandler(Godot.Collections.Array queue);
    [Signal] public delegate void TransitionedToRoundsEventHandler(int roundNumber);

    // Battle setup Signals
    public void EmitEnemiesGenerated(Godot.Collections.Array enemies) => EmitSignal(nameof(EnemiesGenerated), enemies);
    public void EmitCharactersPositioned(Godot.Collections.Array characters) => EmitSignal(nameof(CharactersPositioned), characters);
    public void EmitInitiativeQueueCreated(Godot.Collections.Array queue) => EmitSignal(nameof(InitiativeQueueSetup), queue);
    public void EmitTransitionedToRounds(int roundNumber) => EmitSignal(nameof(TransitionedToRounds), roundNumber);

    // Initiative Events
    //  - Update queue when a character is added
    //  - Update queue when a character is removed
    //  - Update queue when a character's initiative is modified
    [Signal] public delegate void CharacterAddedToQueueEventHandler(CharacterType character);
    [Signal] public delegate void CharacterRemovedFromQueueEventHandler(CharacterType character);
    [Signal] public delegate void CharacterInitiativeModifiedEventHandler(CharacterType character);

    // Initiative Signals
    public void EmitCharacterAddedToQueue(CharacterType character) => EmitSignal(nameof(CharacterAddedToQueue), character);
    public void EmitCharacterRemovedFromQueue(CharacterType character) => EmitSignal(nameof(CharacterRemovedFromQueue), character);
    public void EmitCharacterInitiativeModified(CharacterType character) => EmitSignal(nameof(CharacterInitiativeModified), character);

    // Phase 2: Battle Round Events
    //  - Step 1: Round start
    //  - Step 2: Actions declaration
    //  - Step 3: Turns resolution
    //  - Step 4: Round end
    [Signal] public delegate void RoundStartedEventHandler(int roundNumber);
    [Signal] public delegate void ActionsDeclaredEventHandler();
    [Signal] public delegate void TurnsResolvedEventHandler();
    [Signal] public delegate void RoundEndedEventHandler(int roundNumber);

    // Battle Round Signals
    public void EmitRoundStarted(int roundNumber) => EmitSignal(nameof(RoundStarted), roundNumber);
    public void EmitActionsDeclared() => EmitSignal(nameof(ActionsDeclared));
    public void EmitTurnsResolved() => EmitSignal(nameof(TurnsResolved));
    public void EmitRoundEnded(int roundNumber) => EmitSignal(nameof(RoundEnded), roundNumber);

    // Enemy Character Events
    //  - Enemy declares an action
    [Signal] public delegate void EnemyActionDeclaredEventHandler(CharacterType character);

    // Enemy Character Signals
    public void EmitEnemyActionDeclared(CharacterType character) => EmitSignal(nameof(EnemyActionDeclared), character);

    // Player Characters Events
    //  - Character rolls dice for energy
    //  - Player declares an action for a character
    //  - Player selects a target for an action
    //  - Player cancels an action for a character
    [Signal] public delegate void PlayerEnergyRolledEventHandler(CharacterType character);
    [Signal] public delegate void PlayerActionDeclaredEventHandler(CharacterType character);
    [Signal] public delegate void PlayerTargetSelectedEventHandler(CharacterType character, CharacterType target);
    [Signal] public delegate void PlayerActionCancelledEventHandler(CharacterType character);

    // Player Characters Signals
    public void EmitPlayerEnergyRolled(CharacterType character) => EmitSignal(nameof(PlayerEnergyRolled), character);
    public void EmitPlayerActionDeclared(CharacterType character) => EmitSignal(nameof(PlayerActionDeclared), character);
    public void EmitPlayerTargetSelected(CharacterType character, CharacterType target) => EmitSignal(nameof(PlayerTargetSelected), character, target);
    public void EmitPlayerActionCancelled(CharacterType character) => EmitSignal(nameof(PlayerActionCancelled), character);

    // Turns Events
    //  - Character starts it's turn
    //  - Character performs it's action
    //  - Character is moved to the end of initiative queue
    //  - Character ends it's turn
    [Signal] public delegate void TurnStartedEventHandler(CharacterType character);
    [Signal] public delegate void ActionPerformedEventHandler(CharacterType character);
    [Signal] public delegate void CharacterMovedToEndOfQueueEventHandler(CharacterType character);
    [Signal] public delegate void TurnEndedEventHandler(CharacterType character);

    // Turns Signals
    public void EmitTurnStarted(CharacterType character) => EmitSignal(nameof(TurnStarted), character);
    public void EmitActionPerformed(CharacterType character) => EmitSignal(nameof(ActionPerformed), character);
    public void EmitCharacterMovedToEndOfQueue(CharacterType character) => EmitSignal(nameof(CharacterMovedToEndOfQueue), character);
    public void EmitTurnEnded(CharacterType character) => EmitSignal(nameof(TurnEnded), character);

    // Turn Check Events
    // - Check if the next turn is from a player character
    [Signal] public delegate void CheckNextTurnEventHandler();
    // Check if a new round starts or the battle ends
    [Signal] public delegate void CheckNextRoundEventHandler();

    // Turn Check Signals
    public void EmitCheckNextTurn() => EmitSignal(nameof(CheckNextTurn));
    public void EmitCheckNextRound() => EmitSignal(nameof(CheckNextRound));

    // Phase 3: Battle Result Events
    //  - Step 1: Result checking
    //  - Step 2: Post Battle transition
    [Signal] public delegate void BattleResultCheckedEventHandler(bool victory);
    [Signal] public delegate void SceneTransitionedEventHandler();

    // Battle Result Signals
    public void EmitBattleResultChecked(bool victory) => EmitSignal(nameof(BattleResultChecked), victory);
    public void EmitSceneTransitioned() => EmitSignal(nameof(SceneTransitioned));


    // Phase 4: Post Battle Events
    //  - Rewards distribution
    // or
    //  - Game Over event
    [Signal] public delegate void RewardsDistributedEventHandler();
    [Signal] public delegate void GameOverEventHandler();

    // Post Battle Signals
    public void EmitRewardsDistributed() => EmitSignal(nameof(RewardsDistributed));
    public void EmitGameOver() => EmitSignal(nameof(GameOver));

}