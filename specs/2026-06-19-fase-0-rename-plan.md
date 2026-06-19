# Fase 0 — Rename DiceRolling→Firebound + Princípios de Fronteira — Implementation Plan

<!-- markdownlint-disable MD013 MD031 MD032 -->

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Renomear toda a primeira parte de `DiceRolling`/`dice-rolling` para `Firebound`/`firebound` (código, build, configs, docs) e codificar os princípios de fronteira framework-vs-jogo + módulo opcional em `.ai/conventions/`, sem mudar comportamento.

**Architecture:** Refatoração mecânica de rename, sequenciada em tarefas independentemente verificáveis (build / grep / import headless), cada uma com commit próprio. Sem TDD de comportamento (nada muda em runtime); o "teste" de cada tarefa é a verificação de build/grep/import.

**Tech Stack:** Godot 4.6.3 (mono), C# / net8.0, Godot.NET.Sdk 4.6.3, gdUnit4 6.1.3 + api 5.0.0, PowerShell 7 (pwsh).

**Spec de referência:** `specs/2026-06-19-fase-0-fronteira-rename-design.md`.

## Global Constraints

- Token PascalCase **`DiceRolling` → `Firebound`** (namespaces, `using`, refs C#, RootNamespace, exemplos em docs).
- Token kebab **`dice-rolling` → `firebound`** (nomes de arquivo, `assembly_name`, `config/name`, refs em CI/config).
- **Só primeira parte.** NÃO tocar addons vendored: `apps/framework/addons/{gdUnit4,shaker,imrp,SignalVisualizer}/**`. NÃO tocar `apps/framework/.godot/**` (cache regenerado). Os addons `apps/framework/addons/@spacewiz/**` SÃO primeira parte → renomeiam.
- **NÃO tocar `.tres`/`.tscn`** (0 refs; usam caminho `res://` e `script_class` por nome simples).
- **Specs históricas** (`specs/ARCHITECTURE-*.md`, `specs/2026-06-18-*.md`): preservar narrativa histórica; só atualizar referências que viram instrução ativa (ex.: caminho `dice-rolling.sln` numa convenção). O próprio design e este plano podem citar `DiceRolling` como termo histórico.
- **Toolchain já em 4.6 / net8.0** — não alterar versões.
- Verificação primária de build: `dotnet build apps/framework/<sln-ou-csproj>` → **0 erros** (warnings CS1591 de doc-comment são pré-existentes e toleráveis).
- Commits: Conventional Commits; rodapé `Co-Authored-By: Claude Opus 4.8 <noreply@anthropic.com>`.

---

### Task 1: Branch + rename de namespaces C# (primeira parte)

**Files:**
- Modify: todos os `.cs` em `apps/framework/**` exceto vendored addons e `.godot/` (≈132 arquivos)
- Modify: `apps/framework/dice-rolling.csproj` (só `<RootNamespace>`)

**Interfaces:**
- Consumes: nada.
- Produces: namespace raiz `Firebound.*` em todo o código (Task 2+ assumem que o código já compila sob `Firebound`).

- [ ] **Step 1: Criar a branch de trabalho**

Run:
```bash
git switch -c feat/fase-0-rename
```

- [ ] **Step 2: Substituir `DiceRolling`→`Firebound` nos `.cs` de primeira parte**

Run (pwsh):
```powershell
$files = Get-ChildItem apps/framework -Recurse -File -Filter *.cs |
  Where-Object {
    $_.FullName -notmatch '\\addons\\(gdUnit4|shaker|imrp|SignalVisualizer)\\' -and
    $_.FullName -notmatch '\\\.godot\\'
  }
$count = 0
foreach ($f in $files) {
  $c = Get-Content -LiteralPath $f.FullName -Raw
  $n = $c -replace 'DiceRolling','Firebound'
  if ($n -ne $c) { Set-Content -LiteralPath $f.FullName -Value $n -NoNewline -Encoding utf8; $count++ }
}
"changed $count files"
```
Expected: `changed 132 files` (ordem de grandeza; 130–134 OK).

- [ ] **Step 3: Atualizar `<RootNamespace>` no csproj**

Edit `apps/framework/dice-rolling.csproj`:
```xml
<RootNamespace>Firebound</RootNamespace>
```
(troca `DiceRolling` por `Firebound` nessa linha).

- [ ] **Step 4: Confirmar que não sobrou `DiceRolling` em `.cs` de primeira parte**

Run (pwsh):
```powershell
git grep -n 'DiceRolling' -- 'apps/framework/**/*.cs' ':(exclude)apps/framework/addons/gdUnit4/**' ':(exclude)apps/framework/addons/shaker/**' ':(exclude)apps/framework/addons/imrp/**' ':(exclude)apps/framework/addons/SignalVisualizer/**'
```
Expected: **sem saída** (zero ocorrências). Se aparecer algo em `addons/@spacewiz` deveria já ter sido trocado — investigar.

- [ ] **Step 5: Build para verificar compilação**

Run:
```powershell
dotnet build apps/framework/dice-rolling.csproj -v minimal 2>&1 | Select-Object -Last 6
```
Expected: `0 Erro(s)` / `0 Error(s)`.

- [ ] **Step 6: Commit**

```bash
git add apps/framework
git commit -m "refactor: rename C# namespace DiceRolling to Firebound

Co-Authored-By: Claude Opus 4.8 <noreply@anthropic.com>"
```

---

### Task 2: Rename dos arquivos de build (csproj / sln / xsd)

**Files:**
- Rename: `apps/framework/dice-rolling.csproj` → `apps/framework/firebound.csproj`
- Rename: `apps/framework/dice-rolling.sln` → `apps/framework/firebound.sln`
- Rename: `apps/framework/dice-rolling.xsd` → `apps/framework/firebound.xsd`
- Modify: `firebound.csproj` (ref ao `.xsd`), `firebound.sln` (caminho do projeto)

**Interfaces:**
- Consumes: código já em `Firebound` (Task 1).
- Produces: solução buildável em `apps/framework/firebound.sln` (Task 4 aponta o CI para cá).

- [ ] **Step 1: Renomear os três arquivos com `git mv`**

Run:
```bash
git mv apps/framework/dice-rolling.csproj apps/framework/firebound.csproj
git mv apps/framework/dice-rolling.sln  apps/framework/firebound.sln
git mv apps/framework/dice-rolling.xsd  apps/framework/firebound.xsd
```

- [ ] **Step 2: Atualizar a ref ao `.xsd` dentro do csproj**

Edit `apps/framework/firebound.csproj`, primeira linha:
```xml
<Project xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:noNamespaceSchemaLocation="firebound.xsd" Sdk="Godot.NET.Sdk/4.6.3">
```
(troca `dice-rolling.xsd` por `firebound.xsd`).

- [ ] **Step 3: Atualizar o caminho do projeto no `.sln`**

Run (pwsh) para trocar todas as refs `dice-rolling.csproj`→`firebound.csproj` no sln:
```powershell
$p = 'apps/framework/firebound.sln'
(Get-Content -LiteralPath $p -Raw) -replace 'dice-rolling\.csproj','firebound.csproj' -replace 'dice-rolling','firebound' | Set-Content -LiteralPath $p -NoNewline -Encoding utf8
git grep -n 'dice-rolling' -- apps/framework/firebound.sln
```
Expected: grep **sem saída**.

- [ ] **Step 4: Build pela nova solução**

Run:
```powershell
dotnet build apps/framework/firebound.sln -v minimal 2>&1 | Select-Object -Last 6
```
Expected: `0 Erro(s)`.

- [ ] **Step 5: Confirmar renomeações como R (rename) no git**

Run:
```bash
git status --short
```
Expected: linhas `R  apps/framework/dice-rolling.csproj -> apps/framework/firebound.csproj` (e .sln/.xsd). Não deve haver delete+add.

- [ ] **Step 6: Commit**

```bash
git add apps/framework
git commit -m "refactor: rename build files dice-rolling.* to firebound.*

Co-Authored-By: Claude Opus 4.8 <noreply@anthropic.com>"
```

---

### Task 3: project.godot (config/name + assembly_name)

**Files:**
- Modify: `apps/framework/project.godot`

**Interfaces:**
- Consumes: build em `firebound.*` (Tasks 1–2).
- Produces: projeto Godot nomeado `firebound`, DLL `firebound.dll`.

- [ ] **Step 1: Editar `config/name`**

Edit `apps/framework/project.godot`, seção `[application]`:
```text
config/name="firebound"
```

- [ ] **Step 2: Editar `assembly_name`**

Edit `apps/framework/project.godot`, seção `[dotnet]`:
```text
project/assembly_name="firebound"
```

- [ ] **Step 3: Limpar cache e rebuildar (DLL muda de nome)**

Run (pwsh):
```powershell
Remove-Item apps/framework/.godot -Recurse -Force -ErrorAction SilentlyContinue
dotnet build apps/framework/firebound.sln -v quiet -nologo 2>&1 | Select-String 'Erro|Error|êxito|succeeded' | Select-Object -First 3
```
Expected: `Compilação com êxito` / `0 Erro(s)`.

- [ ] **Step 4: Import headless para verificar que o projeto abre em 4.6**

Run (pwsh):
```powershell
$out = & godot_console --headless --editor --quit 2>&1
"exit: $LASTEXITCODE"
$out | Select-String -Pattern 'SCRIPT ERROR|error CS|Parse Error|Failed to load'
```
Expected: `exit: 0` e **sem** linhas de SCRIPT ERROR / error CS / Parse Error / Failed to load. (Um log isolado `.NET: Assemblies not found` no primeiro open é benigno.)

- [ ] **Step 5: Confirmar a DLL renomeada**

Run (pwsh):
```powershell
Test-Path apps/framework/.godot/mono/temp/bin/Debug/firebound.dll
```
Expected: `True`.

- [ ] **Step 6: Commit**

```bash
git add apps/framework/project.godot
git commit -m "refactor: rename Godot project and assembly to firebound

Co-Authored-By: Claude Opus 4.8 <noreply@anthropic.com>"
```

---

### Task 4: CI + tooling de dev

**Files:**
- Modify: `.github/workflows/sonarqube.yml`
- Modify: `.vscode/settings.json`
- Modify: `apps/framework/.runsettings` (se referenciar nome de assembly/solução)

**Interfaces:**
- Consumes: `firebound.sln` (Task 2).
- Produces: CI e VSCode apontando para `firebound.sln`.

- [ ] **Step 1: Inspecionar as refs atuais**

Run (pwsh):
```powershell
git grep -n 'dice-rolling\|DiceRolling' -- .github/workflows/sonarqube.yml .vscode/settings.json apps/framework/.runsettings
```
Anote cada ocorrência para troca.

- [ ] **Step 2: Trocar nas três (kebab e Pascal conforme aparecer)**

Run (pwsh):
```powershell
foreach ($p in @('.github/workflows/sonarqube.yml','.vscode/settings.json','apps/framework/.runsettings')) {
  if (Test-Path $p) {
    (Get-Content -LiteralPath $p -Raw) -replace 'dice-rolling','firebound' -replace 'DiceRolling','Firebound' |
      Set-Content -LiteralPath $p -NoNewline -Encoding utf8
  }
}
```

- [ ] **Step 3: Verificar que não sobrou nada nesses arquivos**

Run (pwsh):
```powershell
git grep -n 'dice-rolling\|DiceRolling' -- .github/workflows/sonarqube.yml .vscode/settings.json apps/framework/.runsettings
```
Expected: **sem saída**.

- [ ] **Step 4: Sanidade do YAML do Sonar (sln existe, caminho certo)**

Run (pwsh):
```powershell
Select-String -Path .github/workflows/sonarqube.yml -Pattern 'firebound.sln'
Test-Path apps/framework/firebound.sln
```
Expected: a linha do `dotnet build ... firebound.sln` aparece; `True`.

- [ ] **Step 5: Commit**

```bash
git add .github/workflows/sonarqube.yml .vscode/settings.json apps/framework/.runsettings
git commit -m "ci: point Sonar and dev tooling at firebound.sln

Co-Authored-By: Claude Opus 4.8 <noreply@anthropic.com>"
```

---

### Task 5: Docs, scaffold e convenções (+ codificar princípios)

**Files:**
- Modify: `docs/framework/architecture/02-models/*.md` (exemplos `DiceRolling.*`)
- Modify: `AGENTS.md`, `CONTRIBUTING.md`, `apps/framework/README.md`, `README.md` (raiz, se citar)
- Modify: `.ai/conventions/code-style.md`, `.ai/conventions/godot.md`, `.ai/conventions/monorepo.md`
- Modify: `apps/docs/docfx.json`, `apps/docs/processApiFiles.js`, `apps/docs/src/components/OpenGithubIssueButton/index.tsx`
- Create: `.ai/conventions/modularity.md`
- Modify: `.ai/conventions/README.md` (índice)

**Interfaces:**
- Consumes: nomes novos (`Firebound`, `firebound.sln`).
- Produces: convenções vivas dos princípios de fronteira/modularidade.

- [ ] **Step 1: Trocar refs em docs/scaffold (exceto specs históricas e vendored)**

Run (pwsh):
```powershell
$targets = @(
  'docs/framework/architecture/02-models',
  'AGENTS.md','CONTRIBUTING.md','README.md',
  'apps/framework/README.md',
  '.ai/conventions/code-style.md','.ai/conventions/godot.md','.ai/conventions/monorepo.md',
  'apps/docs/docfx.json','apps/docs/processApiFiles.js',
  'apps/docs/src/components/OpenGithubIssueButton/index.tsx'
)
$files = foreach ($t in $targets) { if (Test-Path $t -PathType Container) { Get-ChildItem $t -Recurse -File } elseif (Test-Path $t) { Get-Item $t } }
foreach ($f in $files) {
  (Get-Content -LiteralPath $f.FullName -Raw) -replace 'dice-rolling','firebound' -replace 'DiceRolling','Firebound' |
    Set-Content -LiteralPath $f.FullName -NoNewline -Encoding utf8
}
"updated $($files.Count) files"
```

- [ ] **Step 2: Atualizar a linha de namespace em `code-style.md`**

Em `.ai/conventions/code-style.md`, garantir que a menção a namespaces diga `Firebound.*` (não "`DiceRolling.*` por enquanto"). Se o texto ainda disser "por enquanto", trocar para:
```markdown
Namespaces sob `Firebound.*` (raiz do framework).
```

- [ ] **Step 3: Criar `.ai/conventions/modularity.md`** (princípios do spec §4/§5)

Create `.ai/conventions/modularity.md`:
```markdown
# Convenção — Fronteira Framework-vs-Jogo e Modularidade

Firebound é um **framework reusável**. Estas regras mantêm a separabilidade.

## Fronteira de conteúdo

- **Framework provê tipos e sistemas**: classes `Resource` (ex.: `CharacterType`,
  `DiceType`), serviços, stores, controllers, renderers, eventos.
- **Jogo provê instâncias e assets**: arquivos `.tres` (instâncias dos tipos) e
  assets específicos. "Modelos de dados" do jogo = instâncias, não as classes.
- Tipos `Resource` nunca migram para o jogo (são contrato do framework).
- Hoje não há jogo concreto; tudo é placeholder. Quando houver, `resources/` +
  `assets/` de jogo migram para um local de jogo. YAGNI até lá.

## Mecânicas opcionais (módulos) — abordagem por composição

Mecânicas opcionais (a rolagem de dados é a primeira) são módulos plugáveis.

- **Regra de dependência:** o core nunca referencia um módulo; o módulo
  referencia o core. Wiring core↔módulo via eventos/registro, não chamada
  direta do core para o módulo.
- Desligar/remover um módulo não pode quebrar a compilação do core.
- O módulo é desenhado para "graduar" para addon Godot
  (`addons/firebound.<mod>/`) e/ou assembly própria sem tocar o core.

> O design detalhado do liga/desliga da mecânica de dados é uma spec própria,
> a ser feita após o redesign de eventos.
```

- [ ] **Step 4: Indexar a nova convenção**

Em `.ai/conventions/README.md`, adicionar à lista:
```markdown
- [modularity.md](modularity.md) — fronteira framework-vs-jogo e módulos opcionais.
```

- [ ] **Step 5: Verificar grep global (só specs históricas + vendored podem sobrar)**

Run (pwsh):
```powershell
git grep -n 'dice-rolling\|DiceRolling' -- . ':(exclude)specs/**' ':(exclude)apps/framework/addons/gdUnit4/**' ':(exclude)apps/framework/addons/shaker/**' ':(exclude)apps/framework/addons/imrp/**' ':(exclude)apps/framework/addons/SignalVisualizer/**'
```
Expected: **sem saída**. (Qualquer sobra fora de specs/vendored deve ser tratada aqui.)

- [ ] **Step 6: Markdownlint dos `.md` tocados**

Run (pwsh):
```powershell
npx --yes markdownlint-cli2 ".ai/conventions/*.md" "docs/framework/architecture/02-models/*.md" "AGENTS.md" 2>&1 | Select-Object -Last 5
```
Expected: `0 error(s)`. Corrigir o que aparecer.

- [ ] **Step 7: Commit**

```bash
git add .
git commit -m "docs: rename to Firebound and codify boundary/modularity conventions

Co-Authored-By: Claude Opus 4.8 <noreply@anthropic.com>"
```

---

### Task 6: Verificação final + PR

**Files:** nenhum (verificação + PR).

- [ ] **Step 1: Sweep final de `DiceRolling`/`dice-rolling`**

Run (pwsh):
```powershell
git grep -n 'dice-rolling\|DiceRolling' -- . ':(exclude)specs/**' ':(exclude)apps/framework/addons/gdUnit4/**' ':(exclude)apps/framework/addons/shaker/**' ':(exclude)apps/framework/addons/imrp/**' ':(exclude)apps/framework/addons/SignalVisualizer/**'
```
Expected: **sem saída**.

- [ ] **Step 2: Confirmar `.tres`/`.tscn` intactos**

Run (pwsh):
```powershell
git diff --name-only main...HEAD -- '*.tres' '*.tscn'
```
Expected: **sem saída** (nenhum recurso/cena alterado).

- [ ] **Step 3: Build + import finais**

Run (pwsh):
```powershell
dotnet build apps/framework/firebound.sln -v minimal 2>&1 | Select-Object -Last 4
$o = & godot_console --headless --editor --quit 2>&1; "import exit: $LASTEXITCODE"
$o | Select-String 'SCRIPT ERROR|error CS|Parse Error'
```
Expected: `0 Erro(s)`; `import exit: 0`; sem erros de script.

- [ ] **Step 4: Push + PR**

```bash
git push -u origin feat/fase-0-rename
gh pr create --title "Fase 0: rename DiceRolling→Firebound + princípios de fronteira" --body "$(cat <<'EOF'
Executa a spec specs/2026-06-19-fase-0-fronteira-rename-design.md.

- Rename DiceRolling→Firebound (código, build, project.godot, CI, docs)
- Convenções de fronteira framework-vs-jogo + modularidade em .ai/conventions/modularity.md
- Sem mudança de comportamento; .tres/.tscn intactos

🤖 Generated with [Claude Code](https://claude.com/claude-code)
EOF
)"
```

---

## Self-Review

**Spec coverage:**
- §3.1 código C# → Task 1. §3.2 build files → Task 2. §3.3 project.godot → Task 3. §3.4 CI/docs/configs → Tasks 4–5. §3.5 verificação → Steps de verificação + Task 6. §4 fronteira de conteúdo + §5 modularidade → Task 5 (`.ai/conventions/modularity.md`). §7 DoD → Task 6. ✔ Sem lacunas.

**Placeholder scan:** sem TBD/TODO; todos os comandos e conteúdos completos. ✔

**Type/nome consistency:** `firebound.sln`/`firebound.csproj`/`firebound.xsd`, `Firebound` (ns), `firebound` (config/name, assembly_name) usados de forma consistente entre tarefas. Branch `feat/fase-0-rename` consistente em Task 1 e Task 6. ✔
