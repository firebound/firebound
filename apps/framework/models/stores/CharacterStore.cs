using Godot;
using System;
using System.Linq;

using DiceRolling.Locations;
using DiceRolling.Characters;

namespace DiceRolling.Stores;

/// <summary>
/// Armazena dados dos personagens em coleções e facilita a manipulação desses personagens.
/// </summary>
[Tool]
[GlobalClass]
public partial class CharacterStore : Resource {
    private static CharacterStore? _instance;
    public static CharacterStore Instance {
        get {
            _instance ??= new CharacterStore();
            return _instance;
        }
    }

    [Export]
    public Godot.Collections.Array<CharacterType> Characters { get; private set; } = [];

    public void AddCharacter(CharacterType character) {
        character.InitializeAttributes();
        Characters.Add(character);
    }

    public void RemoveCharacter(string characterID) {
        Characters = [.. Characters.Where(c => c.Id != characterID)];
    }

    public Godot.Collections.Array<string> GetAllCharacterIds() {
        return [.. Characters.Select(c => c.Id)];
    }

    public CharacterType GetCharacterById(string characterID) {
        var character = Characters.FirstOrDefault(c => c.Id == characterID) ?? throw new Exception($"Character with ID {characterID} not found");
        character.InitializeAttributes();
        return character;
    }

    public void SetLocationType(string characterID, LocationType location, int slotIndex) {
        var character = GetCharacterById(characterID);
        character.Location = location;
        character.SlotIndex = slotIndex;
    }

    public (LocationType? location, int slotIndex) GetLocationType(string characterID) {
        var character = GetCharacterById(characterID);
        return (character.Location, character.SlotIndex);
    }

    public void UpdateCharacter(string characterID, Godot.Collections.Dictionary<string, Variant> updatedFields) {
        var character = GetCharacterById(characterID);
        foreach (var field in updatedFields) {
            var property = character.GetType().GetProperty(field.Key);
            property?.SetValue(character, field.Value);
        }
        character.InitializeAttributes();
    }
    public CharacterStore() { }
}