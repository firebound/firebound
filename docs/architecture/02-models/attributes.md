# Attributes

**Attributes** são as características que definem os atributos dos personagens no jogo.

Para mais detalhes, veja a [Referência de API](../../api/DiceRolling.Attributes.md).

## Visão Geral

Os atributos no jogo definem as características dos personagens.

```mermaid
flowchart LR
    subgraph Interfaces
        IAttribute
    end

    Types["AttributeType"]

    AttributeStore

    subgraph Properties
        Other["..."]
    end

    Types-->|implementa|Interfaces

    Interfaces-->|define|Properties

    AttributeStore-->|armazena|Types

    style Types fill:#d74242,stroke:#8a0d26,stroke-width:2px;
    style Interfaces fill:#1da2d3,stroke:#1c74d5,stroke-width:2px;
```

:::warning Atenção

Os tipos de Resources irão alterar conforme o projeto evoluir. Para mais detalhes, veja sobre os [Resources](../00-intro/overview.md).

:::

---

## Interfaces

- **IAttribute**: define um atributo no jogo e agrega as interfaces:
  - **IIdentifiable**: define uma ID única.
  - **IAttributeInformation**: informações básicas de um atributo.
  - **IAttributeAssets**: recursos visuais de um atributo.
  - **IAttributeValues**: valores de um atributo.

### Enumerators

N/A

---

## Models

- **AttributeType**: Representa um tipo de atributo no jogo e inclui suas informações e valores.

---

## Services

N/A

---

## Stores

- **AttributeStore**: Armazena dados dos atributos em coleções e facilita a manipulação desses atributos.
