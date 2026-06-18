# FIREBOUND GAME DEVELOPMENT

## Sumário

- [FIREBOUND GAME DEVELOPMENT](#firebound-game-development)
  - [Sumário](#sumário)
    - [Tecnologias do Jogo](#tecnologias-do-jogo)
    - [Estrutura da framework](#estrutura-da-framework)
  - [Ambiente de Desenvolvimento](#ambiente-de-desenvolvimento)
    - [Pré-requisitos](#pré-requisitos)
    - [Instalação do projeto](#instalação-do-projeto)

### Tecnologias do Jogo

- **Godot 4+**: Engine utilizada para desenvolver o projeto;
- **C#**: Linguagem de programação utilizada para a lógica do jogo.

### Estrutura da framework

```bash
.
└── apps/framework
    ├── addons          # bibliotecas third party
    │   └── @spacewiz   # plugins desenvolvidos para o projeto
    │
    ├── assets          # assets como sprites, sons, texturas e seus arquivos de configuração
    │
    ├── controllers     # comunicação entre a view e o model, interatividade e controle de eventos
    │                   # lógica de alto nível, como o controle do estado do jogo, cenas e transições
    │
    ├── events          # comunicação por signals entre os models, views e controllers
    │                   # TODO: bus, handlers, types
    │
    ├── helpers         # classes utilitárias
    │
    ├── models
    │   ├── data        # estruturas dos dados
    │   ├── services    # manipulação de dados e regras de negócio
    │   └── stores      # estado em tempo de execução (cache e gerenciamento)
    │
    ├── renderers           # visualização e renderização do jogo
    │   ├── components  # componentes reutilizáveis
    │   ├── entities    # entidades do jogo
    │   ├── scenes      # cenas do jogo
    │   └── ui          # elementos de interface do usuário
    │
    ├── resources       # arquivos de configuração estáticos do jogo
    │
    └── tests           # testes unitários e cenas específicas para testes
```

## Ambiente de Desenvolvimento

Para começar a desenvolver na framework, siga as instruções abaixo:

### Pré-requisitos

1. **Godot Engine 4+**: Você pode baixar a versão mais recente do Godot [aqui](https://godotengine.org/download).

2. **.NET SDK**: O projeto utiliza C#, então você precisará do .NET SDK instalado. Você pode baixar o .NET SDK [aqui](https://dotnet.microsoft.com/download).

### Instalação do projeto

1. Abra o projeto no Godot Engine:

   - Inicie o Godot Engine.
   - Clique em "Import" e navegue até o diretório onde você clonou o repositório.
   - Selecione o arquivo `project.godot` e clique em "Open".

2. Certifique-se de que o Godot está configurado para usar o .NET SDK:

   - Vá para `Editor` > `Editor Settings`.
   - No painel esquerdo, expanda `Mono` e selecione `Editor`.
   - Verifique se o caminho do `Mono Build` está apontando para o local correto do .NET SDK.

3. Execute o projeto:
   - Com o projeto aberto no Godot, clique no botão de "Play" (ícone de triângulo) na barra superior para executar o jogo.

Agora você deve estar pronto para começar a desenvolver e testar o jogo localmente.
