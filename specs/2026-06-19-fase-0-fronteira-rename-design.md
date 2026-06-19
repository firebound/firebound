# Design — Fase 0: Fronteira Framework-vs-Jogo + Rename DiceRolling→Firebound

<!-- markdownlint-disable MD013 -->

> **Data:** 2026-06-19
> **Status:** aprovado para implementação (abordagem A — módulo por composição)
> **Autor:** brainstorming assistido
> **Relacionado:** `specs/ARCHITECTURE-ANALYSIS.md` §7 (fronteira framework-vs-jogo), §3; roadmap de fases pós-monorepo.

## 1. Objetivo

Executar a **Fase 0** do roadmap: as decisões baratas que destravam os refactors profundos (eventos, renderers, controllers). Duas frentes acopladas:

1. **Rename `DiceRolling`→`Firebound`** — o nome `DiceRolling` é _stale_ (era o nome do protótipo); o produto é a framework **Firebound**.
2. **Fronteira framework-vs-jogo + modularidade** — estabelecer, em **nível de princípio**, (a) onde o conteúdo de jogo se separará no futuro e (b) como mecânicas opcionais (a rolagem de dados sendo a primeira) plugam na framework.

**Decisões que enquadram esta spec** (do brainstorming):

- **Firebound = framework reusável de verdade.** Outros jogos poderão ser construídos sobre ela; o que existe hoje será publicado normalmente como framework.
- **Não existe código exclusivo de jogo hoje.** Tudo é placeholder; o jogo em si está só documentado (superficialmente). Logo, **não há nada de jogo para separar agora** — o split de conteúdo é futuro.
- **Nome do jogo ainda não decidido.** Por isso o rename tem alvo único `Firebound` (sem namespace de jogo ainda).
- **A mecânica de dados é o maior diferencial de gameplay**, mas será **opcional**: o código shippa junto com a framework, e será preciso poder ligar/desligar (nem todo jogo usará dados). O _design detalhado_ desse liga/desliga é **fora de escopo** aqui (spec própria, depois do redesign de eventos).

**Não-objetivos:** nenhum redesign de eventos/renderers/controllers; nenhum split físico de assembly; nenhuma movimentação de conteúdo; nenhuma implementação concreta do liga/desliga do dado. Esta fase é rename + princípios.

## 2. Contexto atual (aterrado no código)

- **Assembly única:** `apps/framework/dice-rolling.csproj`, `RootNamespace=DiceRolling`, `assembly_name="dice-rolling"`. Testes (gdUnit4) na **mesma** assembly.
- **Namespace `DiceRolling.*`** em todo o código de primeira parte: **305 ocorrências em 132 arquivos** (`.cs`, `.csproj`, `.sln`).
- **Recursos seguros:** `0` ocorrências de `DiceRolling`/`dice-rolling` em arquivos `.tres`/`.tscn`. Cenas e resources referenciam scripts por **caminho** (`res://...`) e `script_class` por **nome simples** (não namespaced) → o rename de namespace **não** os toca.
- **Addons:** os de **primeira parte** `addons/@spacewiz/*` (Arc3DEditorPlugin, TargetBoardEditorPlugin) usam `DiceRolling` → entram no rename. Os **vendored** (`gdUnit4`, `shaker`, `imrp`, `SignalVisualizer`) **não** usam e ficam intactos.
- **Autoloads** (`project.godot`): só `EventBus` é C# (registrado por caminho `res://events/EventBus.cs` → imune a namespace); `Shaker` e `SignalDebugger` são `.gd`. Stores/services **não** são autoloads.
- **Toolchain:** já atualizada para Godot **4.6** (`Godot.NET.Sdk/4.6.3`, `config/features=("4.6",...)`, addon gdUnit4 6.1.3, `gdUnit4.api 5.0.0`, `gdUnit4.test.adapter 3.0.0`, `Microsoft.NET.Test.Sdk 18.6.0`), casando com o editor instalado **4.6.3.stable.mono**. `TargetFramework` segue `net8.0` (travado pelo Godot 4.6). `dotnet build` usa o SDK do NuGet 10.0.x (independe do editor). Feito antes do rename como pré-requisito (ver `chore(deps): upgrade Godot 4.4→4.6`).

## 3. Rename `DiceRolling` → `Firebound`

Princípio: **swap do token raiz**, preservando a sub-estrutura de namespaces (ex.: `DiceRolling.Models.Data.Dice` → `Firebound.Models.Data.Dice`). **Não** relocar o módulo de dados nesta fase (isso é da spec do dice-toggle). Só primeira parte; vendored addons intactos.

### 3.1 Código (C#)

- Substituir palavra-inteira `DiceRolling` → `Firebound` em todos os `.cs` de primeira parte (`namespace DiceRolling…`, `using DiceRolling…`, refs qualificadas). Inclui `apps/framework/addons/@spacewiz/**`. Inclui `apps/framework/tests/**`.
- **Não** tocar `apps/framework/addons/{gdUnit4,shaker,imrp,SignalVisualizer}/**` nem `apps/framework/.godot/**` (regenerado).

### 3.2 Projeto / build

- `RootNamespace`: `DiceRolling` → `Firebound` (no `.csproj`).
- Renomear arquivos: `dice-rolling.csproj` → `firebound.csproj`; `dice-rolling.sln` → `firebound.sln`; `dice-rolling.xsd` → `firebound.xsd`. Usar `git mv` (preserva histórico).
- `firebound.csproj`: `xsi:noNamespaceSchemaLocation="dice-rolling.xsd"` → `"firebound.xsd"`.
- `firebound.sln`: atualizar a entrada do projeto (caminho/nome `dice-rolling.csproj` → `firebound.csproj`) e GUIDs permanecem.
- `.runsettings`: verificar e atualizar qualquer referência ao nome da assembly/solução.

### 3.3 Godot (`project.godot`)

- `[application] config/name="dice-rolling"` → `"firebound"`.
- `[dotnet] project/assembly_name="dice-rolling"` → `"firebound"`. **Consequência:** a DLL compilada muda de nome — como cenas referenciam scripts por caminho (não por assembly), o impacto é nulo nas cenas; verificar via build + import.
- `main_scene`/`icon` são UIDs (`uid://…`) → não mudam.

### 3.4 CI, docs e configs

- `.github/workflows/sonarqube.yml`: `dotnet build apps/framework/dice-rolling.sln` → `…/firebound.sln`.
- `.vscode/settings.json`: refs a `dice-rolling.sln`/assembly.
- `apps/docs/docfx.json` e `apps/docs/processApiFiles.js`: refs a `DiceRolling`/`dice-rolling` (API docs estão **desligadas**, mas manter coerente).
- `apps/docs/src/components/OpenGithubIssueButton/index.tsx`: ref textual.
- Conteúdo `.md`: `docs/framework/architecture/02-models/*.md` (exemplos com `DiceRolling.*`), `CONTRIBUTING.md`, `AGENTS.md`, `.ai/conventions/{monorepo,code-style,godot}.md`, `apps/framework/README.md`.
- **Specs/análises** (`specs/ARCHITECTURE-ANALYSIS.md`, `ARCHITECTURE-SUMMARY.md`, `2026-06-18-monorepo-restructure-design.md`): são registros históricos. Atualizar **apenas** menções que viram instrução ativa (ex.: caminho `dice-rolling.sln` em convenção); deixar o relato histórico como está, com nota se necessário.

### 3.5 Verificação do rename

1. `git grep -n "DiceRolling"` e `git grep -n "dice-rolling"` retornam só ocorrências históricas intencionais (specs) e vendored intocados — zero em código/build/config ativos de primeira parte.
2. `dotnet build apps/framework/firebound.sln` compila.
3. Import headless do Godot sem erro novo de script (ver §7 caveat de versão): `godot_console --headless --import` no diretório do projeto.
4. `.tres`/`.tscn` inalterados no diff (confirma que recursos não dependiam de namespace).

## 4. Convenção de fronteira de conteúdo (futura)

Hoje **não há conteúdo de jogo** para mover — registramos só o _seam_ para quando houver.

**Princípio:** a framework provê **tipos e sistemas**; o jogo provê **instâncias e assets**.

- **Framework (Firebound):** classes `Resource` (ex.: `CharacterType`, `DiceType`, `AttributeType`), serviços, stores, controllers, renderers, eventos. Ou seja, o **código** e os **contratos**.
- **Jogo (futuro):** as **instâncias** desses tipos (arquivos `.tres` em `resources/`) e os **assets** (`assets/`) específicos do jogo.

Implicações registradas (sem ação agora):

- Quando um jogo concreto existir, `resources/` + `assets/` de jogo migram para um local de jogo (ex.: futuro `apps/<game>/` ou pasta de conteúdo), deixando a framework com placeholders/exemplos mínimos.
- Tipos `Resource` **nunca** migram (são contrato da framework).
- O termo "modelos de dados" do jogo = **instâncias** (`.tres`), não as classes.

A spec dedicada de split de conteúdo resolverá o local exato e o mecanismo de carregamento. YAGNI até existir jogo.

## 5. Modelo de modularidade — abordagem A (módulo por composição)

Mecânicas opcionais (a rolagem de dados sendo a primeira e principal) são **módulos** plugáveis. Esta fase fixa os **princípios**; a implementação do liga/desliga do dado é spec própria.

### 5.1 Regra de dependência

- **O core nunca referencia um módulo. O módulo referencia o core.**
- Wiring entre core e módulo via **eventos/registro**, não chamada direta do core para o módulo. (Casa com o futuro redesign de eventos — Fase 1.)
- Resultado: remover/desligar um módulo não quebra a compilação do core.

### 5.2 O que é "o módulo de dados"

Superfície atual identificada (fica onde está nesta fase, só renomeada para `Firebound.*`):

- `models/data/Dice/*` (DiceType, DiceSide, DiceEnergy, DiceFactory, DiceIcon, DiceLocation, …, e interfaces `IDice*`).
- `models/stores/{DiceStore,DiceEnergyStore,DiceIconStore}.cs`.
- `helpers/DiceEnergyHelper.cs`.
- Pontos de acoplamento a investigar na spec do dado: `ActionService`/sistema de energia, e o que em `controllers/battle/*` assume dados.

Esta spec **não** move nem re-namespaca esses arquivos para um `Firebound.Modules.Dice.*` — apenas os **nomeia** como o módulo exemplar e fixa a regra de dependência que a spec do dado vai aplicar.

### 5.3 Caminho de graduação

O seam é desenhado para o módulo poder, no futuro e **sem tocar o core**, virar:

- um addon Godot (`addons/firebound.dice/`), e/ou
- uma assembly própria (`Firebound.Dice.csproj` com project reference ao core).

Não fazemos isso agora (abordagens B/flags e C/addon-assembly foram descartadas por, respectivamente, ferir separabilidade e ser prematuras). Abordagem A entrega a separabilidade exigida por "framework reusável de verdade" ao menor custo presente.

## 6. Riscos

- **Rename amplo** (132 arquivos / 305 refs): risco de tocar vendored por engano. Mitigação: restringir a primeira parte (`@spacewiz` sim; `gdUnit4/shaker/imrp/SignalVisualizer` não); revisar `git status`/diff antes de commitar.
- **`assembly_name` muda o nome da DLL.** Mitigação: cenas referenciam por caminho; validar com build + import headless.
- **Versão do editor:** resolvido — projeto e editor ambos em 4.6.3 (upgrade feito antes do rename). `dotnet build` segue como verificação primária (independe do editor).
- **gdUnit4 precisa do runtime Godot:** `dotnet test` puro falha; a verificação de testes é via build + (opcional) runner gdUnit no editor. Não bloquear a fase em testes de runtime.
- **Ocorrências históricas em specs:** não "corrigir" relato histórico cegamente; só instruções ativas.

## 7. Verificação (Definition of Done)

1. `dotnet build apps/framework/firebound.sln` compila sem erro.
2. `godot_console --headless --editor --quit` (em `apps/framework`, com `.godot` limpa) conclui com exit 0 e sem erro de script/namespace.
3. `git grep "DiceRolling"` / `git grep "dice-rolling"` → só specs históricas + vendored intactos; zero em código/build/config ativos.
4. `git status` mostra renomeações (R) para `firebound.{sln,csproj,xsd}`, não delete+add.
5. `.tres`/`.tscn` sem alterações no diff.
6. CI `sonarqube.yml` aponta `firebound.sln`.
7. `markdownlint-cli2` limpo nos `.md` tocados.
8. `AGENTS.md` / `.ai/conventions/*` coerentes com `Firebound`/`firebound.sln`.

## 8. Fora de escopo (follow-ups)

- **Design detalhado do liga/desliga do dado** (spec própria, após redesign de eventos — Fase 1). Inclui re-namespace/relocação eventual do módulo (`Firebound.Modules.Dice` ou addon/assembly).
- **Split físico de conteúdo de jogo** (instâncias `.tres` + assets) — quando existir jogo concreto.
- **Nome do jogo** e criação de `apps/<game>`.
- **Split físico em assemblies** (core vs módulos vs jogo).
- Qualquer item de dívida do `ARCHITECTURE-ANALYSIS.md` (eventos, renderers, controllers).
- ~~Migração da versão do projeto Godot 4.4 → 4.6~~ — **feito** como pré-requisito (commit `chore(deps): upgrade Godot 4.4→4.6`).
- Atualizar addons de terceiros `shaker`/`imrp` (fonte canônica indefinida; já importam OK em 4.6). `SignalVisualizer` já está na última versão (1.7.0).
