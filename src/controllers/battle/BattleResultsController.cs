using System.Linq;
using DiceRolling.Helpers;
using DiceRolling.Stores;
using Godot;

namespace DiceRolling.Controllers;

/// <summary>
/// Gerencia as condições de vitória, derrota e recompensas da batalha.
/// </summary>
/// <remarks>
/// O <c>BattleResultsController</c> é responsável por determinar o desfecho da batalha, verificando se as condições de vitória ou derrota foram atendidas e iniciando a transição para a tela de resultados.
///     <list type="table">
///         <listheader>
///             <term>Avaliação do resultado da batalha</term>
///             <description>Determinação do fim da batalha e transição para os resultados</description>
///         </listheader>
///         <item>- Verifica condições de vitória</item>
///         <item>- Verifica condições de derrota</item>
///         <item>- Faz a transição para a tela de resultados (<c>PostBattleController</c>)</item>
///     </list>
/// </remarks>
[GlobalClass]
public partial class BattleResultsController : Node {
    public override void _Ready() {
        // Conecta-se aos eventos relevantes
        BattleEvents.Instance.RoundEnded += OnRoundEnded;
    }

    public override void _ExitTree() {
        if (BattleEvents.Instance != null) {
            BattleEvents.Instance.RoundEnded -= OnRoundEnded;
        }
    }

    // Verifica se houve vitória ou derrota
    public void CheckBattleResult() {
        GD.Print("BattleResultsController: Checking battle result...");

        bool victory = CheckVictoryCondition();
        bool defeat = CheckDefeatCondition();

        if (victory || defeat) {
            // Notifica o resultado da batalha
            BattleEvents.Instance.EmitBattleResultChecked(victory);
            GD.Print($"BattleResultsController: Battle ended with result - Victory: {victory}");

            // Transição para o pós-batalha
            TransitionToPostBattle(victory);
        }
        else {
            GD.Print("BattleResultsController: Battle continues - both teams have active characters");
        }
    }

    // Verifica condições de vitória (todos inimigos derrotados)
    private bool CheckVictoryCondition() {
        var enemyTeam = BattleController.Instance.GetEnemyTeam();
        // TODO - achar um jeito melhor de acessar a coleção de atributos do jogo
        var attributesStore = GD.Load<AttributesStore>("res://resources/Attributes/AttributesStore.tres");
        var healthAttribute = AttributesHelper.GetAttributeType(attributesStore, "Health");

        if (healthAttribute == null) {
            GD.PrintErr("BattleResultsController: Health attribute not found");
            return false;
        }

        // Verifica se não existem inimigos ou se todos os inimigos estão derrotados
        bool noEnemiesLeft = enemyTeam.Count == 0 ||
            !enemyTeam.Any(e => e != null && e.GetAttributeCurrentValue(healthAttribute) > 0);

        if (noEnemiesLeft) {
            GD.Print("BattleResultsController: Victory condition met - all enemies defeated");
        }

        return noEnemiesLeft;
    }

    // Verifica condições de derrota (todos personagens do jogador derrotados)
    private bool CheckDefeatCondition() {
        var playerTeam = BattleController.Instance.GetPlayerTeam();
        // TODO - achar um jeito melhor de acessar a coleção de atributos do jogo
        var attributesStore = GD.Load<AttributesStore>("res://resources/Attributes/AttributesStore.tres");
        var healthAttribute = AttributesHelper.GetAttributeType(attributesStore, "Health");

        if (healthAttribute == null) {
            GD.PrintErr("BattleResultsController: Health attribute not found");
            return false;
        }

        // Verifica se não existem jogadores ou se todos os jogadores estão derrotados
        bool noPlayersLeft = playerTeam.Count == 0 ||
            !playerTeam.Any(p => p != null && p.GetAttributeCurrentValue(healthAttribute) > 0);

        if (noPlayersLeft) {
            GD.Print("BattleResultsController: Defeat condition met - all player characters defeated");
        }

        return noPlayersLeft;
    }

    // Transição para a fase de pós-batalha
    private void TransitionToPostBattle(bool victory) {
        // Atualiza o estado da batalha para finalizada
        BattleController.Instance.SetBattleState(BattleState.End);

        // Notifica o fim da batalha
        BattleEvents.Instance.EmitBattleEnded(victory);
    }
    // Eventos
    private void OnRoundEnded(int roundNumber) {
        GD.Print($"Event RoundEnded fired on BattleResultsController - Round {roundNumber} ended");
        CheckBattleResult();
    }
}