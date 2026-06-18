# AGENTS.md

Porta de entrada para agentes de IA neste repositório. Leia isto antes de agir.

## O que é

**Firebound** é um framework + jogo roguelike de batalha por turnos, em **C# na Godot 4**.
O repositório é um **monorepo**: o framework/jogo (Godot) e o site de documentação
(Docusaurus) convivem como apps separados, com conteúdo e ferramentas ao redor.

## Layout do monorepo

| Pasta | O que é |
| --- | --- |
| `apps/framework/` | Projeto Godot 4 / C# (a framework + protótipo do jogo). Abrir `apps/framework/project.godot`. |
| `apps/docs/` | App Docusaurus (site de documentação) + pipeline DocFX para API. |
| `docs/` | **Conteúdo** de documentação em Markdown — legível no GitHub e consumido por `apps/docs`. |
| `packages/` | Reservado para pacotes/bridges entre apps. Vazio por ora. |
| `scripts/` | Scripts do repositório (build, manutenção, automações). |
| `tools/` | Scripts e ferramentas do repositório. |
| `specs/` | Specs de design e análises de arquitetura. |
| `.ai/` | Convenções e skills do projeto para agentes. Comece por `.ai/README.md`. |

## Como rodar / testar

**Framework (Godot):**

- Abrir o projeto no Godot 4: `apps/framework/project.godot`.
- Build/test C#: `dotnet build apps/framework/dice-rolling.sln` e `dotnet test` (config em `apps/framework/.runsettings`).

**Docs (Docusaurus):**

- `cd apps/docs && npm install && npm run dev` (dev server).
- `npm run build` gera o site. A API é gerada via DocFX (`npm run process-api`) e fica em `apps/docs/generated/` (gitignored).

## Convenções

- Convenções de código, commits, monorepo e Godot estão em [`.ai/conventions/`](.ai/conventions/).
- Skills específicas do projeto: [`.ai/skills/`](.ai/skills/).

## Estado da arquitetura

Há um diagnóstico arquitetural recente (pontos fortes/fracos, dívida técnica) em
[`specs/ARCHITECTURE-ANALYSIS.md`](specs/ARCHITECTURE-ANALYSIS.md) (resumo em
[`specs/ARCHITECTURE-SUMMARY.md`](specs/ARCHITECTURE-SUMMARY.md)). Leia antes de
propor mudanças estruturais em `renderers`, `events` ou `controllers`.
