# Convenção — Commits

Usar **Conventional Commits** (já em uso no histórico do projeto).

```text
<tipo>(<escopo opcional>): <descrição curta>

<corpo opcional explicando o porquê>
```

## Tipos comuns

- `feat` — nova funcionalidade.
- `fix` — correção de bug.
- `refactor` — mudança de código sem alterar comportamento.
- `docs` — documentação.
- `chore` — manutenção, build, estrutura (ex.: esta reestruturação de monorepo).
- `test` — testes.

## Escopo

Use o app/pasta como escopo quando ajudar: `feat(framework): ...`,
`docs(apps/docs): ...`, `chore(repo): ...`.

## Notas

- Assunto curto (≤ ~50 chars), imperativo.
- Corpo só quando o "porquê" não for óbvio.
