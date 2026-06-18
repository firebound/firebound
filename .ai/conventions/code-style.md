# Convenção — Código (C# / Godot)

Padrões observados no `apps/framework`. Seguir o que já existe ao redor.

## Namespaces

- Namespace raiz atual: `DiceRolling.*` (legado; rename para `Firebound.*` é
  dívida técnica conhecida — ver `specs/ARCHITECTURE-ANALYSIS.md`).
- Namespace segue o domínio, não a pasta física (ex.: `DiceRolling.Characters`,
  `DiceRolling.Services`).

## Padrões de modelagem

- Dados de domínio herdam de `Resource` / `IdentifiableResource` (serialização e
  edição no editor da Godot).
- `services/` = regras/lógica; `stores/` = estado em runtime (singletons via
  `Instance` estática); `data/` = estruturas.
- Atributos Godot: `[Tool]`, `[GlobalClass]`, `[Export]`, `[Signal]`.

## Formatação

- Indentação: 4 espaços (ver `.vscode/settings.json`, `editor.formatOnSave` para C#).
- Formatter C#: `ms-dotnettools.csharp`.

## Cuidado / anti-padrões já mapeados

Evitar ampliar problemas conhecidos (detalhe em `specs/ARCHITECTURE-ANALYSIS.md`):

- Indireção excessiva no padrão entity/component (`renderers/`).
- Dois hubs de eventos concorrentes + signals órfãos (`events/`).
- Services dependendo de controllers (quebra de camadas).
- Excesso de interfaces `I*Sheet` sem ganho polimórfico.
