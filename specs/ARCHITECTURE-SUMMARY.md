# Firebound — Resumo da Análise Arquitetural

<!-- markdownlint-disable MD013 -->

> **Data:** 2026-06-18 · Resumo em prosa do diagnóstico completo em [ARCHITECTURE-ANALYSIS.md](ARCHITECTURE-ANALYSIS.md).

## Onde o projeto está, em palavras simples

Você construiu duas coisas ao mesmo tempo sem perceber: um **motor de regras de jogo** (a parte `models/`) e uma **forma de mostrar e controlar esse jogo na tela** (as camadas `renderers/`, `controllers/` e `events/`). A boa notícia é que o motor de regras está sólido — é a parte mais madura, mais "real" e mais bem pensada de tudo. A má notícia é que tudo que envolve *montar o jogo em cena e fazer as peças conversarem* é onde a complexidade saiu do controle. E o seu instinto estava certo: os três pontos que te incomodavam são exatamente os três pontos mais problemáticos do código.

## O coração funciona bem

A pasta `models/` (os dados como personagem, dado, ação, atributo, papel; mais os serviços e os "stores" que guardam estado) está praticamente toda implementada de verdade, com lógica real, não mockada. A modelagem em cima dos `Resource` da Godot foi uma boa escolha: você ganha edição visual, salvamento e recarga automática de graça. O fluxo "papel é o molde, personagem é a instância" é coerente. Isso é base para continuar, não para jogar fora.

O único exagero aqui é **interface demais**. Você criou famílias inteiras de interfaces pequenas (`ICharacterInformationSheet`, `ICharacterAssetSheet`, `IAttributeValues`, etc.) que somam centenas de linhas, mas como cada uma só tem uma única classe que a implementa, elas não te dão nenhum ganho prático — só cerimônia para manter. É o tipo de abstração que parece "arquitetura limpa" mas só adiciona trabalho.

## A camada visual é a dor de verdade

Aqui está o motivo de "montar objetos na cena dar tanto bug". O padrão que você montou funciona assim: uma *Entity* é uma casca vazia que só segura um dado; quando o dado muda, ela dispara um sinal; e os *Components* filhos escutam esse sinal e se redesenham. Em teoria é elegante e desacoplado. Na prática, entre o dado e o que aparece na tela existem **cinco ou mais camadas de indireção**, e cada uma é um lugar onde algo pode quebrar silenciosamente.

Os problemas concretos:

- Os componentes assumem que são **filhos diretos** de uma entidade (usam `GetParent`). Basta você inserir um nó intermediário na cena para tudo virar `null` sem erro visível.
- Existe uma **corrida de tempo**: quando uma entidade é criada dinamicamente (um grid cria células, que criam personagens, que criam modelos), o dado às vezes é injetado *antes* dos componentes-filho estarem prontos para escutar — então a primeira atualização se perde e a coisa aparece vazia ou errada.
- Os componentes dependem de **caminhos de nó fixos** (`NodePath`). Renomear ou mover um nó na cena quebra o código em tempo de execução, sem aviso no editor.
- Já houve um **loop infinito** nessa engrenagem — e ele foi "resolvido" comentando o trecho, não consertando a causa. Isso é um sinal claro de que o padrão está lutando contra você.

Resumindo: esse desenho aguenta cenas simples e estáticas, mas desmorona em composição dinâmica e aninhada — que é justamente o que um jogo de batalha precisa o tempo todo.

## Os eventos estão meio quebrados

Você tem **dois sistemas de eventos** vivendo lado a lado (`EventBus` e `BattleEvents`), com nomes de namespace inconsistentes e só um deles realmente registrado como global. Mais grave: **mais da metade dos eventos definidos não está em uso** — uns são disparados mas ninguém escuta, outros têm quem escute mas nunca são disparados. O sistema *parece* completo, mas boa parte é fachada.

O que faz a batalha funcionar mesmo assim é que, por baixo dos eventos, os controllers se chamam **diretamente** uns aos outros. Ou seja, existem dois canais de comunicação competindo, e o canal de eventos está em grande parte morto. Isso explica por que é tão difícil entender o fluxo só lendo o código.

## Os controllers ficaram confusos — mas a causa é clara

A máquina de estados da batalha em si (rodadas, fases, fila de iniciativa) é **boa e bem feita**. A confusão vem de três coisas:

1. O fluxo é **invisível**: para entender o que acontece depois de "começar batalha", você precisa pular de evento em evento, de handler em handler, reconstruindo a sequência de cabeça. Não dá para ler de cima para baixo.
2. Tem **código de teste rodando como se fosse produção** — o "input do jogador" hoje é simulado automaticamente, não vem de uma interface real. Então quem lê acha que algo é automático quando deveria ser uma decisão do jogador.
3. O `BattleController` faz **coisas demais** (estado, criação de grid, inicialização de personagem, montagem dos sub-controllers, debug) — responsabilidades que pertenceriam aos serviços.

A tela de pós-batalha (vitória/derrota/recompensa) e os gerenciadores de menu estão **vazios**, só esqueletos.

## A história do "framework" ainda não é verdade

O projeto se apresenta como um framework reutilizável + um jogo, mas no código **não existe separação física** entre os dois: tudo está no mesmo namespace (`DiceRolling`, apesar do rename para "Firebound" no README), no mesmo assembly, sem pontos de extensão. Um desenvolvedor externo não conseguiria usar o framework sem editar o miolo. O rename para Firebound foi só de fachada — o código não migrou. E a documentação de API está desatualizada: aponta para pastas que não existem mais e descreve classes que foram renomeadas.

## O que isso significa para a retomada

Se eu tivesse que te dizer em uma frase: **a fundação (dados e regras) está boa e merece ficar; o que precisa de redesenho é a camada que junta tudo — como as peças aparecem na tela e como elas conversam entre si.** Os três maiores investimentos, em ordem, seriam:

1. Repensar o padrão entity/component dos renderers.
2. Unificar e limpar os eventos (decidindo o que é evento de verdade e o que é chamada direta).
3. Antes de crescer mais, decidir se "framework separável" continua sendo meta — porque essa decisão muda a estrutura de tudo.

Há também uma correção de premissa importante: você lembrava de "tudo muito mockado". Não é bem isso. O que está mockado é o **input do jogador** e a **UI de fim de batalha**. Os dados, serviços e stores estão reais — o problema deles nunca foi maturidade, foi acoplamento e excesso de abstração.
