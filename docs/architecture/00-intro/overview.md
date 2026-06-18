# Visão Geral

---

## Resumo da Arquitetura

A arquitetura desta framework é projetada para oferecer **flexibilidade e modularidade**, permitindo modificações e expansões sem impacto direto em outras partes do sistema.

A ideia é ter uma arquitetura baseada em **Modelos, Renderizadores, Controladores e Eventos**, que são organizados em camadas para facilitar a manutenção e a expansão do código:

1. Os **Modelos** armazenam e gerenciam os dados do jogo, estruturados em três camadas: Data, Stores e Services;
2. Os **Renderizadores** representam os dados em elementos visuais;
3. Os **Controladores** centralizam a lógica do jogo, gerenciando estados e cenas;
4. Os **Eventos** permitem a comunicação entre os sistemas, possibilitando a interação entre Modelos, Renderizadores e Controladores.

---

## Modelos de dados

Os Modelos são estruturados em `Resources` do Godot, possibilitando expansão como a criação de novos tipos de personagens, atributos, classes, ações e efeitos com o mínimo de alterações no código.

São estruturados em três camadas:

- **Data**: definem **como** os `Resources` são estruturados, incluindo suas propriedades;
- **Stores**: Armazenam os dados em tempo de execução, mantendo o estado global do jogo e sincronizando as mudanças entre diferentes partes do sistema.
- **Services**: Manipulam os dados das `Stores` e implementam regras de negócio, garantindo consistência na lógica da aplicação.

### Resources

Atualmente, os tipos de `Resources` ainda estão sendo definidos e, por enquanto, existe apenas um para cada modelo.

Por isso, o acesso e manipulação desses tipos está sendo feito diretamente.

```mermaid
flowchart
    subgraph Interfaces
        ICharacter
    end

    Types["CharacterType"]

    CharacterService

    CharacterStore

    Types-->|implementa|Interfaces

    Interfaces-->|define|Properties

    CharacterService-->|manipula|Types
    CharacterService-->|acessa|CharacterStore
    CharacterStore-->|armazena|Types

    style Types fill:#d74242,stroke:#8a0d26,stroke-width:2px;
    style Interfaces fill:#1da2d3,stroke:#1c74d5,stroke-width:2px;
```

:::info Explicação

- `CharacterType` é um tipo de **Resource** que implementa a interface `ICharacter`.
- `CharacterService` manipula `CharacterType`.
- `CharacterStore` armazena `CharacterType`.

:::

:::tip Dica

Para entender melhor o conceito de **Resource**, veja o [tutorial](https://docs.godotengine.org/en/stable/tutorials/scripting/resources.html) e a [documentação oficial](https://docs.godotengine.org/en/stable/classes/class_resource.html) do Godot.

:::

#### Abstração dos Resources

Quando novos tipos de **Resources** forem criados para uma mesma _feature_ (por exemplo, `CharacterTypeA` e `CharacterTypeB`), será necessário que essas classes herdem de uma classe base como `CharacterBase`.

```mermaid
flowchart
    subgraph Interfaces
        ICharacter
    end

    subgraph Types
        CharacterTypeA
        CharacterTypeB
    end

    CharacterService

    CharacterStore

    subgraph Properties
        Other["..."]
        CharacterAction["Actions[]<br>(CharacterAction)"]
        CharacterAttribute["Attributes[]<br>(CharacterAttribute)"]
        Category["Category<br>(Category)"]
        Role["Role<br>(Role)"]
        Location["Location<br>(Location)"]
    end

    subgraph Features
        subgraph ActionFeature
            direction TB
            ActionBase-->|Abstrai|IAction
        end
        subgraph AttributeFeature
            direction TB
            AttributeBase-->|Abstrai|IAttribute
        end
        subgraph CategoryFeature
            direction TB
            CategoryBase-->|Abstrai|ICategory
        end
        subgraph RoleFeature
            direction TB
            RoleBase-->|Abstrai|IRole
        end
        subgraph LocationFeature
            direction TB
            LocationBase-->|Abstrai|ILocation
        end
    end

    CharacterBase-->|abstrai|Interfaces
    Types-->|implementam|Interfaces
    Types-->|herdam|CharacterBase

    Interfaces-->|define|Properties

    CharacterService-->|manipula|CharacterBase
    CharacterService-->|acessa|CharacterStore
    CharacterStore-->|armazena|CharacterBase

    CharacterAction-->|resource|ActionFeature
    CharacterAttribute-->|resource|AttributeFeature
    Category-->|resource|CategoryFeature
    Role-->|resource|RoleFeature
    Location-->|resource|LocationFeature

    style Types fill:#d74242,stroke:#8a0d26,stroke-width:2px;
    style Interfaces fill:#1da2d3,stroke:#1c74d5,stroke-width:2px;
    style CharacterBase fill:#8a1fd1,stroke:#8a1fd1,stroke-width:2px;
    style ActionBase fill:#8a1fd1,stroke:#8a1fd1,stroke-width:2px;
    style AttributeBase fill:#8a1fd1,stroke:#8a1fd1,stroke-width:2px;
    style CategoryBase fill:#8a1fd1,stroke:#8a1fd1,stroke-width:2px;
    style RoleBase fill:#8a1fd1,stroke:#8a1fd1,stroke-width:2px;
    style LocationBase fill:#8a1fd1,stroke:#8a1fd1,stroke-width:2px;
```

:::info Explicação

- `CharacterTypeA` e `CharacterTypeB` são **Resources** que herdam `CharacterBase`.
- `CharacterBase` é a classe base que implementa `ICharacter`.
- `CharacterService` manipula dados do tipo `CharacterBase`.
- `CharacterStore` armazena dados do tipo `CharacterBase`.

:::

---

## Renderizadores

Os **Renderizadores** são responsáveis pela exibição gráfica dos elementos do jogo, usando os dados dos `Models` para criar representações visuais (incluindo animações e efeitos) através de Componentes modulares.

### Componentes

Os **Componentes** compõem os **Renderizadores**, encapsulando funcionalidades específicas, como animações e interações visuais. Eles promovem modularidade e flexibilidade no desenvolvimento.

```mermaid
flowchart BT
    DataModel["Data Model"]

    subgraph Renderers["Renderers"]
        direction BT
        Entity3D
        CharacterEntity
    end

    subgraph Components["Componentes/Framework"]
        direction BT
        AnimatedSpriteComponent
        SelectableComponent

        subgraph Godot["Componentes/Godot"]
            Sprite3D
            Sprite2D
            Area3D
        end
    end

    Renderers-->|referencia|DataModel

    Components -->|compõe|Renderers

    Sprite3D-->|compõe|AnimatedSpriteComponent
    Sprite2D-->|compõe|SelectableComponent
    Area3D-->|compõe|SelectableComponent

    CharacterEntity-->|herda|Entity3D
```

---

## Controladores

Os **Controladores** centralizam a lógica do jogo, gerenciando estados, cenas e eventos.

Coordenam as ações dos jogadores e interações entre personagens, garantindo que a progressão e os sistemas do jogo funcionem corretamente.
