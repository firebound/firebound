using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

using DiceRolling.Characters;
using DiceRolling.Grids;
using DiceRolling.Stores;
using DiceRolling.Locations;
using DiceRolling.Helpers;

using DiceRolling.Entities;

namespace DiceRolling.Controllers;

/// <summary>
/// Executa comandos de alto nível durante a batalha.
/// </summary>
/// <remarks>
/// O <c>BattleController</c> é responsável por gerenciar o fluxo geral da batalha, incluindo a inicialização, transições de estado e configuração.
///     <list type="table">
///         <listheader>
///             <term>Transições de estados</term>
///             <description>Principais transições de estado da batalha</description>
///         </listheader>
///         <item>- Início da batalha</item>
///         <item>- Pausa a batalha</item>
///         <item>- Continua a batalha</item>
///         <item>- Fim da batalha</item>
///     </list>
///     <list type="table">
///         <listheader>
///             <term>Setup da batalha</term>
///             <description>Configuração inicial da batalha</description>
///         </listheader>
///         <item>- Geração de inimigos</item>
///         <item>- Posicionamento de personagens</item>
///         <item>- Inicialização da fila de iniciativa (<c>InitiativeController</c>)</item>
///         <item>- Transição para a fase de rounds (<c>RoundController</c>)</item>
///     </list>
/// </remarks>
[Tool]
[GlobalClass]
[Icon("res://assets/editor/controller.svg")]
public partial class BattleController : Node {
    private static BattleController? _instance;
    public static BattleController Instance {
        get {
            _instance ??= Engine.GetMainLoop() is SceneTree tree
                ? tree.Root.FindNodeOfType<BattleController>()
                : new BattleController();

            if (_instance == null) {
                GD.PrintRich("[BattleController] Instance not found or created. Creating a new instance. This should not happen in a running game.");
                _instance = new BattleController();
            }

            return _instance;
        }
    }

    [Export] public CharacterStore? PlayerCharacterStore { get; set; }
    [Export] public CharacterStore? EnemyCharacterStore { get; set; }
    [Export] public LocationType? PlayerSquadLocation { get; set; }
    [Export] public LocationType? EnemySquadLocation { get; set; }
    [Export] public PackedScene? GridEntityScene { get; set; }
    [Export(PropertyHint.Range, "-10,10,1")] public Vector3 PlayerGridPosition { get; set; } = new Vector3(-3, 0, 0);
    [Export(PropertyHint.Range, "-10,10,1")] public Vector3 EnemyGridPosition { get; set; } = new Vector3(3, 0, 0);
    private GridEntity? _playerGridEntity;
    private GridEntity? _enemyGridEntity;
    [ExportToolButton("Recreate Grids")]
    public Callable RecreateGrids => Callable.From(() => {
        // Clean up existing grids
        if (_playerGridEntity != null) {
            _playerGridEntity.QueueFree();
            _playerGridEntity = null;
        }

        if (_enemyGridEntity != null) {
            _enemyGridEntity.QueueFree();
            _enemyGridEntity = null;
        }

        // Create new grids
        CreateBattleGrids();
        GD.PrintRich("[color=gold][Battle Controller] Battle grids recreated.[/color]");
    });

    // State management
    private BattleState _currentState = BattleState.Start;
    public BattleState CurrentState => _currentState;
    // Battle data
    private int _currentRound = 0;
    public int CurrentRound => _currentRound;
    private Godot.Collections.Array _playerTeam = [];
    private Godot.Collections.Array _enemyTeam = [];

    // Controllers
    private QueueController? _queueController;
    private TurnController? _turnController;
    private RoundController? _roundController;
    private ActionsController? _actionsController;
    private BattleResultsController? _battleResultsController;
    private PostBattleController? _postBattleController;

    public ActionsController? ActionsController => _actionsController;
    public RoundState CurrentRoundState => _roundController?.CurrentRoundState ?? RoundState.RoundEnd;

    public override void _Ready() {
        _instance = this;
        InitializeControllers();

        // Initialize with characters from stores if available
        if (PlayerCharacterStore?.Characters != null && PlayerSquadLocation != null) {
            var playerChars = PlayerCharacterStore.Characters.Where(c =>
                c != null && c.Location == PlayerSquadLocation).ToArray();
            _playerTeam = new Godot.Collections.Array(playerChars);
            GD.PrintRich($"[color=gold][Battle Controller] Player team initialized with {playerChars.Length} characters.[/color]");
        }

        if (EnemyCharacterStore?.Characters != null && EnemySquadLocation != null) {
            var enemyChars = EnemyCharacterStore.Characters.Where(c =>
                c != null && c.Location == EnemySquadLocation).ToArray();
            _enemyTeam = new Godot.Collections.Array(enemyChars);
            GD.PrintRich($"[color=gold][Battle Controller] Enemy team initialized with {enemyChars.Length} characters.[/color]");
        }

        // ! TODO - FOR TESTING PURPOSES
        if (Engine.IsEditorHint()) {
            return;
        }

        StartBattle();
    }

    private void InitializeControllers() {
        // Create controllers as child nodes
        _queueController = new QueueController {
            Name = "QueueController"
        };
        AddChild(_queueController);

        _turnController = new TurnController {
            Name = "TurnController"
        };
        AddChild(_turnController);

        _actionsController = new ActionsController {
            Name = "ActionsController"
        };
        AddChild(_actionsController);

        _roundController = new RoundController {
            Name = "RoundController"
        };
        AddChild(_roundController);

        _battleResultsController = new BattleResultsController {
            Name = "BattleResultsController"
        };
        AddChild(_battleResultsController);

        _postBattleController = new PostBattleController {
            Name = "PostBattleController"
        };
        AddChild(_postBattleController);
    }

    // Starts a new battle with the specified teams
    public void StartBattle(Godot.Collections.Array? playerTeam = null, Godot.Collections.Array? enemyTeam = null) {
        GD.PrintRich("[color=gold][Battle Controller] Starting battle...[/color]");

        // Setup battle data using provided teams or fall back to exported characters
        _playerTeam = playerTeam ?? _playerTeam;
        _enemyTeam = enemyTeam ?? _enemyTeam;
        _currentRound = 0;

        BattleEvents.Instance.EmitBattleStarted(_playerTeam, _enemyTeam);
        SetBattleState(BattleState.InProgress);

        BattleSetup();
    }

    public static void PauseBattle() {
        BattleEvents.Instance.EmitBattlePaused();
    }

    public static void ResumeBattle() {
        BattleEvents.Instance.EmitBattleResumed();
    }

    public void SetBattleState(BattleState newState) {
        GD.PrintRich($"[color=gold][Battle Controller] Battle state changing: {_currentState} -> {newState}.[/color]");
        _currentState = newState;
    }

    public void AdvanceRound() {
        _currentRound++;
    }

    private void BattleSetup() {
        GenerateEnemies();
        PositionCharacters();
    }

    private void GenerateEnemies() {
        // TODO: Implement enemy generation logic
        // This would be based on dungeon level, player team strength, etc.
        BattleEvents.Instance.EmitEnemiesGenerated(_enemyTeam);
    }

    private void PositionCharacters() {
        GD.PrintRich("[color=gold][Battle Controller] Positioning characters...[/color]");

        CreateBattleGrids();

        // Ensure all characters have initialized attributes and actions before positioning
        InitializeCharacterAttributesIfNeeded(_playerTeam);
        InitializeCharacterAttributesIfNeeded(_enemyTeam);
        InitializeCharacterActionsIfNeeded(_playerTeam);
        InitializeCharacterActionsIfNeeded(_enemyTeam);

        _playerGridEntity?.GridData?.AssignCharacters();
        _enemyGridEntity?.GridData?.AssignCharacters();

        // Combine both teams for initiative
        var allCharacters = new Godot.Collections.Array();
        allCharacters.AddRange(_playerTeam);
        allCharacters.AddRange(_enemyTeam);

        BattleEvents.Instance.EmitCharactersPositioned(allCharacters);

        TransitionToRounds();
    }

    // TODO mover para o service de grids (data layer)
    private void CreateBattleGrids() {
        if (GridEntityScene == null) return;

        _playerGridEntity = CreateGrid(
            "P",
            PlayerGridPosition,
            PlayerCharacterStore
        );

        _enemyGridEntity = CreateGrid(
            "E",
            EnemyGridPosition,
            EnemyCharacterStore
        );
    }

    // TODO mover para o service de grids (data layer)
    private GridEntity CreateGrid(string prefix, Vector3 position, CharacterStore? characterStore) {
        if (GridEntityScene == null)
            throw new InvalidOperationException("[color=gold]GridEntityScene is not assigned.[/color]");

        var gridEntity = GridEntityScene.Instantiate<GridEntity>();
        AddChild(gridEntity);
        gridEntity.Position = position;

        // TODO - implement grid sizing somehow 
        var gridData = new GridType(2, 3, prefix) {
            CharacterStore = characterStore
        };

        gridEntity.Data = gridData;

        return gridEntity;
    }

    // TODO mover para o service de characters (data layer)
    private static void InitializeCharacterActionsIfNeeded(Godot.Collections.Array characterTeam) {
        foreach (var character in characterTeam) {
            // Use proper Godot type conversion
            if (character.Obj is CharacterType characterType) {
                // Check if actions are initialized (no actions or empty actions collection)
                if (characterType.Actions == null || characterType.Actions.Count == 0) {
                    GD.PrintRich($"[color=gold]Initializing actions for character: {characterType.Name}.[/color]");
                    characterType.InitializeActions();
                }
            }
        }
    }

    // TODO mover para o service de characters (data layer)
    private static void InitializeCharacterAttributesIfNeeded(Godot.Collections.Array characterTeam) {
        foreach (var character in characterTeam) {
            // Use proper Godot type conversion
            if (character.Obj is CharacterType characterType) {
                // Check if attributes are initialized (no attributes or empty attributes collection)
                if (characterType.Attributes == null || characterType.Attributes.Count == 0) {
                    GD.PrintRich($"[color=gold]Initializing attributes for character: {characterType.Name}.[/color]");
                    characterType.InitializeAttributes();
                }
            }
        }
    }

    public void TransitionToRounds() {
        GD.PrintRich("[color=gold][Battle Controller] Transitioning to battle rounds phase...[/color]");
        BattleEvents.Instance.EmitTransitionedToRounds(CurrentRound);
    }

    public Godot.Collections.Array GetAllCharacters() {
        var allCharacters = new Godot.Collections.Array();
        allCharacters.AddRange(_playerTeam);
        allCharacters.AddRange(_enemyTeam);
        return allCharacters;
    }

    public List<CharacterType> GetPlayerTeam() {
        List<CharacterType> result = [];

        GD.PrintRich($"[color=gold]_playerTeam has {_playerTeam.Count} characters.[/color]");
        foreach (var item in _playerTeam) {
            if (item.Obj is CharacterType characterType) {
                result.Add(characterType);
            }
            else {
                GD.PrintRich($"[color=gold]Item is not a CharacterType: {item.Obj}.[/color]");
            }
        }

        GD.PrintRich($"[color=gold]GetPlayerTeam called: {result.Count} characters.[/color]");
        return result;
    }

    public List<CharacterType> GetEnemyTeam() {
        List<CharacterType> result = [];

        GD.PrintRich($"[color=gold]_enemyTeam has {_enemyTeam.Count} characters.[/color]");
        foreach (var item in _enemyTeam) {
            if (item.Obj is CharacterType characterType) {
                result.Add(characterType);
            }
            else {
                GD.PrintRich($"[color=gold]Item is not a CharacterType: {item.Obj}.[/color]");
            }
        }

        GD.PrintRich($"[color=gold]GetEnemyTeam called: {result.Count} characters.[/color]");
        return result;
    }

    // Debugging methods

    public void LogLocationInfo(string? context) {
        GD.PrintRich($"[color=gold][BattleController] Logging location info for context: {context}[/color]");

        // Log the reference locations
        GD.PrintRich($"[color=gold][BattleController] PlayerSquadLocation: {PlayerSquadLocation} (Hash: {PlayerSquadLocation?.GetHashCode()})[/color]");

        GD.PrintRich($"[color=gold][BattleController] PlayerSquadLocation: {EnemySquadLocation} (Hash: {EnemySquadLocation?.GetHashCode()})[/color]");

        // Log each character's location
        GD.PrintRich("\n[color=gold]Player Team Characters:[/color]");
        foreach (var character in _playerTeam) {
            if (character.Obj is CharacterType characterType) {
                GD.PrintRich($"[color=gold]  Character: {characterType.Name}, Location: {characterType.Location} (Hash: {characterType.Location?.GetHashCode()})[/color]");
                GD.PrintRich($"[color=gold]  Is Player Location? {ReferenceEquals(characterType.Location, PlayerSquadLocation)}[/color]");
                GD.PrintRich($"[color=gold]  Is Enemy Location? {ReferenceEquals(characterType.Location, EnemySquadLocation)}[/color]");
            }
        }

        GD.PrintRich("\n[color=gold]Enemy Team Characters:");
        foreach (var character in _enemyTeam) {
            if (character.Obj is CharacterType characterType) {
                GD.PrintRich($"[color=gold]  Character: {characterType.Name}, Location: {characterType.Location} (Hash: {characterType.Location?.GetHashCode()})[/color]");
                GD.PrintRich($"[color=gold]  Is Player Location? {ReferenceEquals(characterType.Location, PlayerSquadLocation)}[/color]");
                GD.PrintRich($"[color=gold]  Is Enemy Location? {ReferenceEquals(characterType.Location, EnemySquadLocation)}[/color]");
            }
        }
    }

    public void LogTeamInfo(string context) {
        GD.PrintRich($"[color=gold][BattleController] Logging team info for context: {context}[/color]");
        GD.PrintRich($"[color=gold]_playerTeam: {_playerTeam.Count} characters[/color]");
        GD.PrintRich($"[color=gold]_enemyTeam: {_enemyTeam.Count} characters[/color]");
    }
}
