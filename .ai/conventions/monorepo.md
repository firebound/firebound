# Convenção — Monorepo

Estrutura e o que pertence a cada pasta.

```text
./
├── AGENTS.md          # entrada para agentes
├── .ai/               # convenções + skills do projeto
├── apps/              # apps do monorepo
│   ├── framework/     # projeto Godot 4 / C# (framework + protótipo)
│   └── docs/          # app Docusaurus + pipeline DocFX
├── docs/              # CONTEÚDO de documentação (.md), legível no GitHub
├── packages/          # pacotes/bridges entre apps (vazio por ora)
├── scripts/           # scripts do repositório
├── tools/             # scripts e ferramentas do repositório
└── specs/             # specs de design e análises de arquitetura
```

## Regras

- **`apps/`** contém aplicações executáveis/buildáveis. Cada app é autocontido
  (seu próprio `package.json`, `.csproj`, etc.).
- **`docs/`** é só **conteúdo** escrito à mão. Nada de configuração de app, nada
  de artefato gerado. Deve ler bem direto no GitHub.
- **Documentação de API gerada** (DocFX) é interna ao `apps/docs` (em
  `apps/docs/generated/`, gitignored) — nunca commitada em `docs/`.
- **`packages/`** só recebe código quando houver algo compartilhado entre apps.
  Não criar pacotes especulativos (YAGNI).
- **`specs/`** guarda design docs (`YYYY-MM-DD-<tema>-design.md`) e análises.
- Mover arquivos entre pastas: usar `git mv` para preservar histórico.

## Caminhos sensíveis (não esquecer ao mover coisas)

- `apps/framework/dice-rolling.sln` — solução .NET (CI e VSCode apontam para cá).
- `apps/docs/docfx.json` — lê o código de `../framework` e gera em `generated/api-raw`.
- `.github/workflows/sonarqube.yml` — analisa `apps/framework/**`.
- `.github/workflows/pages.yml` — builda `apps/docs` (deploy só na branch `documentation`).
