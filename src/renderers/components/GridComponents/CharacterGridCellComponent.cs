using Godot;
using DiceRolling.Entities;
using DiceRolling.Grids;
using DiceRolling.Helpers;

namespace DiceRolling.Components.Grids;

/// <summary>
///  Component that displays a character entity on a grid cell.
/// </summary>
/// <remarks>
/// This component listens for updates to the parent GridCellEntity's GridCellType and displays a character entity if the cell is occupied.
/// </remarks>
[Tool]
[GlobalClass]
[Icon("res://assets/editor/component-3d.svg")]
public partial class CharacterGridCellComponent : Node3D {
    private GridCellEntity? _parent;
    private GridCellType? _cellData;
    private CharacterEntity? _characterEntity;

    [Export] public PackedScene? CharacterEntityScene { get; set; }
    [Export] public Vector3 CharacterOffset { get; set; } = new Vector3(0, 0, 0);

    public override void _Ready() {
        _parent = GetParent<GridCellEntity>();

        if (_parent != null) {
            // Connect to entity updates
            SignalHelper.ConnectSignal(_parent, nameof(Entity3D.EntityUpdated), this, nameof(OnEntityUpdated));

            // Get initial data
            _cellData = _parent.CellData;

            if (_cellData != null) {
                // Connect to cell data changes
                SignalHelper.ConnectSignal(_cellData, nameof(GridCellType.CellChanged), this, nameof(OnCellDataChanged));

                // Initial update
                UpdateCharacter();
            }
        }
    }

    public override void _ExitTree() {
        if (_parent != null) {
            SignalHelper.DisconnectSignal(_parent, nameof(Entity3D.EntityUpdated), this, nameof(OnEntityUpdated));
        }

        if (_cellData != null) {
            SignalHelper.DisconnectSignal(_cellData, nameof(GridCellType.CellChanged), this, nameof(OnCellDataChanged));
        }

        DestroyCharacterEntity();
    }

    private void OnEntityUpdated() {
        if (_parent != null) {
            var newCellData = _parent.CellData;

            // If cell data reference changed
            if (_cellData != newCellData) {
                if (_cellData != null) {
                    SignalHelper.DisconnectSignal(_cellData, nameof(GridCellType.CellChanged), this, nameof(OnCellDataChanged));
                }

                _cellData = newCellData;

                if (_cellData != null) {
                    SignalHelper.ConnectSignal(_cellData, nameof(GridCellType.CellChanged), this, nameof(OnCellDataChanged));
                }

                UpdateCharacter();
            }
        }
    }

    private void OnCellDataChanged() {
        UpdateCharacter();
    }

    private void UpdateCharacter() {
        if (_cellData == null) {
            // GD.Print("CharacterGridCellComponent: _cellData é null, destruindo CharacterEntity");
            DestroyCharacterEntity();
            return;
        }

        // GD.Print($"UpdateCharacter: Cell '{_cellData.Label}', IsOccupied: {_cellData.IsOccupied}, Has Character: {_cellData.Character != null}");

        // REMOVED: This block caused the infinite loop
        // if (_cellData.Character != null && !_cellData.IsOccupied) {
        //     _cellData.IsOccupied = true;
        //     _cellData.NotifyChanged();
        // }

        // Determine if a character should be displayed based on the cell data
        bool hasCharacter = _cellData.IsOccupied && _cellData.Character != null;

        if (hasCharacter) {
            if (_characterEntity == null) {
                // GD.Print("CharacterGridCellComponent: Criando nova CharacterEntity");
                CreateCharacterEntity(); // This sets CharacterData internally
            }
            else if (_characterEntity.Data != _cellData.Character) {
                // GD.Print($"CharacterGridCellComponent: Atualizando CharacterData para {_cellData.Character.Name}");
                _characterEntity.Data = _cellData.Character; // This might trigger EntityUpdated
            }

            // Ensure visibility is correct
            if (_characterEntity != null && !_characterEntity.Visible) {
                _characterEntity.Visible = true;
            }
        }
        else {
            // If no character should be displayed
            if (_characterEntity != null) {
                // GD.Print("CharacterGridCellComponent: Escondendo ou destruindo CharacterEntity existente");
                // Option 1: Just hide
                _characterEntity.Visible = false;
                // Option 2: Destroy (if preferred)
                // DestroyCharacterEntity();
            }
        }
    }

    private void CreateCharacterEntity() {
        if (CharacterEntityScene == null) {
            GD.PrintErr("Cannot create character entity: CharacterEntityScene is null");
            return;
        }

        DestroyCharacterEntity(); // Clean up any existing entity

        _characterEntity = CharacterEntityScene.Instantiate<CharacterEntity>();
        _characterEntity.Position = CharacterOffset;
        AddChild(_characterEntity);

        // Set the character data when creating
        if (_cellData?.Character != null) {
            _characterEntity.Data = _cellData.Character; // This might trigger EntityUpdated
        }
        else {
            // Should not happen if called from UpdateCharacter's hasCharacter check, but good practice
            _characterEntity.Visible = false;
        }
    }

    private void DestroyCharacterEntity() {
        if (_characterEntity != null) {
            // Check if the node is still valid before trying to free it
            if (IsInstanceValid(_characterEntity)) {
                _characterEntity.QueueFree();
            }
            _characterEntity = null;
        }
    }
}