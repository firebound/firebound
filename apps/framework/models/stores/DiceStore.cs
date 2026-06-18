using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

using DiceRolling.Dice;

namespace DiceRolling.Stores;

[Tool]
[GlobalClass]
public partial class DiceStore : Node {
    private static DiceStore? _instance;
    public static DiceStore Instance {
        get {
            _instance ??= new DiceStore();
            return _instance;
        }
    }

    public List<DiceType> DiceSet { get; private set; } = [];

    private DiceStore() { }

    public void AddDice(DiceType dice) {
        DiceSet.Add(dice);
    }

    public DiceType GetDiceByID(string diceId) {
        var dice = DiceSet.FirstOrDefault(d => d.Id == diceId) ?? throw new Exception($"Dice with ID {diceId} not found");
        return dice;
    }

    public void UpdateDiceByID(string diceId, Action<DiceType> updateFn) {
        var dice = GetDiceByID(diceId);
        updateFn(dice);
    }

    public void UpdateDiceName(string diceId, string newName) {
        UpdateDiceByID(diceId, dice => dice.Name = newName);
    }

    public void UpdateDiceLocation(string diceId, DiceLocation newLocation) {
        UpdateDiceByID(diceId, dice => dice.Location = newLocation);
    }

    public void RemoveDice(string diceId) {
        DiceSet = DiceSet.Where(d => d.Id != diceId).ToList();
    }
}