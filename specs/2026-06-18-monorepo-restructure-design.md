# Design — Reestruturação para Monorepo (Firebound)

<!-- markdownlint-disable MD013 -->

> **Data:** 2026-06-18
> **Status:** aprovado para implementação (abordagem A)
> **Autor:** brainstorming assistido

## 1. Objetivo

Estabelecer explicitamente o padrão de monorepo do repositório `firebound`, alinhado às convenções usadas em outros projetos do autor (ex.: `firebound/proscenio`):

- `./AGENTS.md` — porta de entrada para agentes de IA.
- `./.ai/` — casa de skills e convenções do projeto.
- `./apps/` — apps do monorepo (framework Godot + site de documentação).
- `./docs/` — **conteúdo** de documentação (legível no GitHub e consumido pelo Docusaurus).
- `./packages/` — placeholder para pacotes/bridges futuros (não necessário ainda).
- `./scripts/` — scripts do repositório.
- `./tools/` — ferramentas (já existe: `github-manager`).

Maior valor: organização de `apps/` e `docs/`, e o scaffold de `.ai/` + `AGENTS.md`.

**Não-objetivos:** nenhuma mudança de comportamento do jogo, nenhuma refatoração de código C#, nenhuma correção dos problemas listados em `ARCHITECTURE-ANALYSIS.md`. Só estrutura, configs e scaffold.

## 2. Estrutura-alvo

```text
./
├── AGENTS.md
├── .ai/
│   ├── README.md
│   ├── conventions/
│   │   ├── README.md
│   │   ├── monorepo.md
│   │   ├── code-style.md
│   │   ├── commits.md
│   │   └── godot.md
│   └── skills/
│       └── README.md
├── apps/
│   ├── framework/            # ← src/ (projeto Godot inteiro)
│   └── docs/                 # ← app Docusaurus + pipeline DocFX
├── docs/                     # conteúdo .md (à mão)
│   ├── architecture/
│   ├── game_design/
│   └── tutorials/
├── specs/                    # specs de design + análises de arquitetura
│   ├── 2026-06-18-monorepo-restructure-design.md
│   ├── ARCHITECTURE-ANALYSIS.md
│   └── ARCHITECTURE-SUMMARY.md
├── packages/
│   └── README.md
├── scripts/
│   └── README.md
├── tools/                    # inalterado
├── .github/workflows/        # sonarqube.yml + pages.yml ajustados
└── (.markdownlint-cli2.jsonc, LICENSE, CONTRIBUTING.md, etc. permanecem na raiz)
```

**Princípios:**

1. Todo movimento via `git mv` (preserva histórico).
2. O projeto Godot move como bloco único → refs internas (`res://`, `.sln`, `.csproj`, `.tscn`, `.tres`) permanecem válidas.
3. `docs/` raiz contém **apenas conteúdo escrito à mão**. A documentação de API gerada (DocFX) fica interna ao app de docs e é gitignored — não polui o `docs/` legível no GitHub.

## 3. Mapa de movimentos

### 3.1 Framework Godot → `apps/framework/`

`git mv src apps/framework`. Move todo o conteúdo de `src/` (incluindo `addons/`, `assets/`, `controllers/`, `events/`, `helpers/`, `models/`, `renderers/`, `resources/`, `tests/`, `project.godot`, `dice-rolling.sln`, `dice-rolling.csproj`, `dice-rolling.xsd`, `.runsettings`, `README.md`, `.gitignore`, `.gitattributes`).

- Refs internas do projeto Godot são relativas à raiz do projeto (`res://`) → continuam corretas.
- `.godot/` (build cache) é regenerado pela engine; se versionado parcialmente, mover junto é inócuo.
- **Não** renomear `dice-rolling.sln`/`.csproj` nesta atividade (rename `DiceRolling`→`Firebound` é item separado da dívida técnica).

### 3.2 App Docusaurus → `apps/docs/`

`git mv` dos artefatos de app (não do conteúdo):

- `docs/docusaurus.config.ts`, `docs/sidebars.ts`, `docs/package.json`, `docs/package-lock.json`, `docs/postcss.config.js`, `docs/tsconfig.json`, `docs/processApiFiles.js`
- `docs/src/` (tema, plugins, páginas, css)
- `docs/public/`
- `docs/.npmrc`, `docs/.prettierrc`, `docs/.gitattributes`, `docs/.gitignore`
- Artefatos de build (`docs/.docusaurus/`, `docs/build/`) — não versionados; ignorados via `.gitignore` do app.

### 3.3 Conteúdo → `docs/` (raiz)

- `git mv docs/content/architecture docs/architecture`
- `git mv docs/content/game_design docs/game_design`
- `git mv docs/content/tutorials docs/tutorials`
- `ARCHITECTURE-ANALYSIS.md` e `ARCHITECTURE-SUMMARY.md` vão para `specs/` (não `docs/`), junto deste design.
- `docs/content/framework/api/` (conteúdo de API processado) era gerado/untracked; **não** migra. Será regenerado dentro de `apps/docs/generated/` (ver §4).
- A pasta `docs/content/` deixa de existir ao final.

### 3.4 DocFX → `apps/docs/`

- `git mv docfx.json apps/docs/docfx.json`
- `git mv filterConfig.yml apps/docs/filterConfig.yml` (se presente na raiz)

## 4. Pipeline de documentação (abordagem A — split limpo)

Objetivo: conteúdo à mão em `docs/` raiz; API gerada interna ao app e gitignored.

### 4.1 `apps/docs/docfx.json`

- `metadata[].src[].src`: `"src"` → `"../framework"`
- `metadata[].output`: `"docs/api"` → `"generated/api-raw"` (relativo a `apps/docs/`)
- `filter`: `"filterConfig.yml"` (agora ao lado, em `apps/docs/`)

### 4.2 `apps/docs/processApiFiles.js`

- `apiDir`: lê de `generated/api-raw`
- `docsApiDir`: escreve em `generated/api` (md processado)
- `processedTocFilePath`: `generated/api/toc_processed.json`

### 4.3 `apps/docs/docusaurus.config.ts`

- Preset `classic` → `docs.path`: `'./content'` → `'../../docs'` (conteúdo à mão).
- Adicionar **segunda instância** de `@docusaurus/plugin-content-docs` para a API:
  - `id: 'api'`, `path: './generated/api'`, `routeBasePath: 'api'`, `sidebarPath: './sidebars-api.ts'`.
- Navbar: item de API condicionado à existência de `generated/api/toc_processed.json`.
- Atualizar links GitHub desatualizados (`Space-Wizard-Studios/sw-game-dice-rolling` → `firebound/firebound`).

### 4.4 Sidebars

- `sidebars.ts`: manter `architectureSidebar`, `gameDesignSidebar`, `tutorialsSidebar` (autogenerated, dirName relativo a `../../docs`). Remover `apiSidebar` daqui.
- Novo `sidebars-api.ts`: lê `generated/api/toc_processed.json` (lógica migrada do `sidebars.ts` atual).

### 4.5 `.gitignore` do app

- Adicionar `generated/` ao `apps/docs/.gitignore` (além de `.docusaurus/`, `build/`, `node_modules/`).

## 5. Ajustes de CI

### 5.1 `.github/workflows/sonarqube.yml`

- `paths`: `"src/**"` → `"apps/framework/**"`
- `sonar.sources="src"` → `"apps/framework"`
- `sonar.tests="src/tests"` → `"apps/framework/tests"`
- `sonar.exclusions` `src/addons/**` → `apps/framework/addons/**`
- `dotnet build src/dice-rolling.sln` → `dotnet build apps/framework/dice-rolling.sln`

### 5.2 `.github/workflows/pages.yml`

- `paths`: incluir `apps/docs/**`, `docs/**`, `apps/framework/**` (DocFX depende do código).
- `docfx` → rodar com `working-directory: apps/docs` (ou `docfx apps/docs/docfx.json`).
- `npm install` / `npm run process-api` / `npm run build`: `working-directory: docs` → `apps/docs`.
- Upload artifact: `./docs/build` → `./apps/docs/build`.
- **Nota:** o deploy continua disparando só na branch `documentation`. Esta mudança em `main` não publica até ser mergeada em `documentation` (fora do escopo desta atividade).

## 6. Scaffold `.ai/` + `AGENTS.md`

### 6.1 `AGENTS.md` (raiz)

Conciso. Conteúdo:

- Uma linha sobre o projeto (framework + jogo roguelike turn-based, C#/Godot 4).
- Mapa do monorepo (apps/docs/packages/scripts/tools, `.ai/`).
- Como rodar/testar cada app: `apps/framework` (abrir `project.godot` no Godot; `dotnet build`/`dotnet test` no `.sln`); `apps/docs` (`npm install && npm run dev`).
- Ponteiro para `.ai/conventions/` e `.ai/skills/`.
- Ponteiro para `docs/ARCHITECTURE-ANALYSIS.md` como visão da arquitetura.

### 6.2 `.ai/README.md`

Explica o propósito de `.ai/` (casa de skills e convenções consumíveis por agentes) e a relação com `AGENTS.md`.

### 6.3 `.ai/conventions/`

- `README.md` — índice das convenções.
- `monorepo.md` — a estrutura desta spec, resumida (o que vai em cada pasta).
- `code-style.md` — convenções C#/Godot observadas (namespaces `DiceRolling.*` por enquanto, padrão Resource-based, services/stores singleton, `[Tool]`/`[Export]`/`[Signal]`).
- `commits.md` — Conventional Commits (já em uso no histórico).
- `godot.md` — notas específicas de Godot (onde fica o projeto, como abrir, padrão entity/component — com link ao diagnóstico).

### 6.4 `.ai/skills/README.md`

Placeholder explicando que skills específicas do projeto vivem aqui (vazio por ora).

## 7. Placeholders `packages/` e `scripts/`

- `packages/README.md` — "Reservado para pacotes/bridges entre apps. Vazio por ora."
- `scripts/README.md` — "Scripts do repositório (build, manutenção, automações)."

## 8. Outros toques

- `README.md` (raiz): atualizar a seção "Estrutura de arquivos" para refletir o monorepo.
- `apps/framework/README.md` (era `src/README.md`): ajustar caminho de instalação (abrir `apps/framework/project.godot`).
- `.github/copilot-instructions.md`: `/src` → `apps/framework`.
- `.vscode/settings.json`: revisar referências a `src/` (já marcado como modificado no working tree).
- Links GitHub `sw-game-dice-rolling`/`Space-Wizard-Studios` desatualizados → `firebound/firebound` onde tocados (escopo mínimo: arquivos já editados nesta atividade).

## 9. Riscos

- **DocFX/Godot build não verificável 100% localmente** (precisa .NET tool + build do csproj). Mitigação: verificar build do Docusaurus localmente com `generated/` ausente (API condicional); validar paths de CI por inspeção.
- **Branch `documentation`** ficará divergente da `main` até receber estes commits; o deploy de Pages só reflete após merge nela.
- **`.godot/` cache**: se houver paths absolutos cacheados, a engine regenera ao reabrir o projeto.
- **Movimento grande**: muitos `git mv`; revisar `git status` antes de commitar.

## 10. Verificação (Definition of Done)

1. `git status` mostra renomeações (R), não delete+add, para os arquivos movidos.
2. `apps/docs`: `npm install` + `npm run build` conclui (com conteúdo à mão; API opcional).
3. `apps/framework/project.godot` abre no Godot sem erro de path (verificação manual do autor).
4. `dotnet build apps/framework/dice-rolling.sln` compila.
5. Workflows `sonarqube.yml` e `pages.yml` referenciam apenas caminhos novos (inspeção).
6. `AGENTS.md` e `.ai/` presentes e coerentes.
7. `markdownlint-cli2` limpo nos `.md` novos.
8. Nenhum arquivo órfão em `docs/content/` (pasta removida).

## 11. Out of scope (follow-ups)

- Rename `DiceRolling`→`Firebound` (namespaces, `.sln`/`.csproj`, `project.godot`).
- Mover commits para a branch `documentation` / disparar deploy.
- Criar `apps/game` ou `apps/prototype`.
- Qualquer item de dívida técnica do `ARCHITECTURE-ANALYSIS.md`.
