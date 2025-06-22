# Actions

**Actions** são entidades que representam as ações que podem ser realizadas pelos personagens no jogo.

Para mais detalhes, veja a [Referência de API](../../api/DiceRolling.Actions.md).

## Visão Geral

As ações no jogo são utilizadas para definir o que os personagens podem fazer. Cada ação possui uma categoria (`Category`), energia necessária (`DiceEnergy`), efeitos (`Effects`) e um alvo (`TargetBoard`).

```mermaid
flowchart TD
    Types["ActionType"]

    subgraph Interfaces
        IAction
    end

    subgraph Properties["Properties"]
        Other["..."]
        Category
        DiceEnergy
        Effects
        TargetBoard
    end

    subgraph External
        CategoryFeature["Category"]
        DiceFeature["DiceEnergy"]
        EffectsFeature["EffectType"]
        TargetBoardFeature["TargetBoardType"]
    end

    Types-->|implementa|Interfaces

    Interfaces-->|define|Properties

    Category-->|resource|CategoryFeature
    DiceEnergy-->|resource|DiceFeature
    Effects-->|resource|EffectsFeature
    TargetBoard-->|resource|TargetBoardFeature

    style Types fill:#d74242,stroke:#8a0d26,stroke-width:2px;
    style Interfaces fill:#1da2d3,stroke:#1c74d5,stroke-width:2px;
```

:::warning Atenção

Os tipos de Resources irão alterar conforme o projeto evoluir. Para mais detalhes, veja sobre os [Resources](../00-intro/overview.md).

:::

---

## Interfaces

- **IAction**: define as entidades de ações que são realizadas por personagens do jogo e agrega as interfaces:
  - **IIdentifiable**: define uma ID única.
  - **IActionInformation**: informações gerais de uma ação.
  - **IActionAssets**: recursos visuais de uma ação.
  - **IActionBehavior**: comportamento de uma ação.
  - **IActionContext**: contexto de uma ação.
  - **IActionResult**: resultado de uma ação.

### Enumerators

N/A

---

## Types (Resources)

- **ActionType**: Representa um tipo de ação no jogo e inclui suas informações, comportamento, categoria, contexto e efeitos.

### Types externos

- **Category**: categoria da ação.
- **DiceEnergy**: energia necessária para realizar a ação.
- **Effects**: efeitos da ação.
- **TargetBoard**: configuração do alvo da ação.

---

## Services

N/A

---

## Stores

N/A
