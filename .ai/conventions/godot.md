# Convenção — Godot

Notas específicas da engine e deste projeto.

## Onde fica

- Projeto Godot: `apps/framework/project.godot`. Abrir essa pasta como projeto na
  Godot 4 (mono / .NET).
- Solução .NET: `apps/framework/dice-rolling.sln`.

## Cache `.godot/`

- `apps/framework/.godot/` é cache regenerável e **gitignored**. A engine
  recria ao abrir o projeto. Não versionar, não depender do conteúdo.

## Padrões de cena/código

- Entidades (`renderers/entities/`) são contêineres de dados (`Entity3D.Data`) que
  emitem `EntityUpdated`; componentes-filho escutam e se redesenham.
- **Atenção:** esse padrão tem fragilidades conhecidas (acoplamento por
  `GetParent<T>`, `NodePath` exportados, race em `_Ready()`). Ver diagnóstico em
  `specs/ARCHITECTURE-ANALYSIS.md` (§6) antes de criar/alterar entidades.

## Editor

- VSCode: `dotnet.defaultSolution` aponta para `apps/framework/dice-rolling.sln`.
- Mover o projeto Godot = mover a pasta inteira (refs `res://` são relativas à
  raiz do projeto e sobrevivem).
