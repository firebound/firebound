using System;
using Godot;

namespace DiceRolling.Services;

public class IdService {
    private static IdService? _instance;
    public static IdService Instance => _instance ??= new IdService();

    private IdService() { }

    public string GenerateNewId() {
        return Guid.NewGuid().ToString();
    }

    public void EnsureValidId(ref string id) {
        if (!ValidationService.ValidateId(id)) {
            GD.PrintErr("Id is null or empty, generating new Id");
            id = GenerateNewId();
        }
    }
}