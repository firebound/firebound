# Characters

**Characters** são entidades que representam os personagens **jogáveis e não-jogáveis** no jogo.

Para mais detalhes, veja a [Referência de API](../../api/DiceRolling.Characters.md).

## Visão Geral

Os personagens no jogo possuem categoria (`Category`), papel (`Role`) e localização (`Location`). Cada personagem pode realizar ações (`CharacterAction`) e possui atributos (`CharacterAttribute`) específicos.

```mermaid
flowchart
    Types["CharacterType"]

    CharacterService

    CharacterStore

    subgraph Interfaces
        ICharacter
    end

    subgraph Properties
        Other["..."]
        CharacterAction["Actions[]<br>(CharacterAction)"]
        CharacterAttribute["Attributes[]<br>(CharacterAttribute)"]
        Category["Category"]
        Role["Role"]
        Location["Location"]
    end

    subgraph External
        ActionFeature[ActionType]
        AttributeFeature[AttributeType]
        CategoryFeature[Category]
        RoleFeature[RoleType]
        LocationFeature[LocationType]
    end

    Types-->|implementa|Interfaces

    Interfaces-->|define|Properties

    CharacterService-->|manipula|Types
    CharacterService-->|acessa|CharacterStore
    CharacterStore-->|armazena|Types

    CharacterAction-->|resource|ActionFeature
    CharacterAttribute-->|resource|AttributeFeature
    Category-->|resource|CategoryFeature
    Role-->|resource|RoleFeature
    Location-->|resource|LocationFeature

    style Types fill:#d74242,stroke:#8a0d26,stroke-width:2px;
    style Interfaces fill:#1da2d3,stroke:#1c74d5,stroke-width:2px;
```

:::warning Atenção

Os tipos de Resources irão alterar conforme o projeto evoluir. Para mais detalhes, veja sobre os [Resources](../00-intro/overview.md).

:::

---

## Interfaces

- **ICharacter**: define um personagem no jogo e agrega as interfaces:
  - **IIdentifiable**: define uma ID única.
  - **ICharacterInformationSheet**: informações gerais de um personagem e categoria.
  - **ICharacterAssetSheet**: recursos visuais de um personagem.
  - **ICharacterRoleSheet**: role de um personagem.
  - **ICharacterActionSheet**: ações de um personagem.
  - **ICharacterAttributeSheet**: atributos de um personagem.
  - **ICharacterPlacementSheet**: localização de um personagem.

### Enumerators

N/A

---

## Types (Resources)

- **CharacterType**: Representa um tipo de personagem no jogo e inclui suas informações, atributos, ações, recursos visuais, localização e papel.

![CharacterType model](../../../public/architecture/02-features/characters/CharacterType.png)

### Types externos

- **Category**: Categoria do personagem.
- **Role**: Role do personagem.
- **CharacterAction**: Ação do personagem.
- **CharacterAttribute**: Atributo do personagem.
- **Location**: Localização do personagem.

---

## Services

- **CharacterService**: Fornece métodos para manipulação dos dados de personagens.

---

## Stores

- **CharacterStore**: Armazena dados dos personagens em coleções e facilita a manipulação desses personagens.
