# Posicionamento e Alvos

As ações são executadas por personagens e exigem que estejam em **posições específicas** no campo de batalha.

Além disso, as ações afetam **alvos definidos**, que podem ser o próprio ator, seus aliados, inimigos ou **células específicas** do campo (ex: para efeitos persistentes).

Para mais detalhes sobre a estrutura do campo, consulte a seção de [Campo de Batalha](../../02-battle/index.md#campos-de-batalha).

---

## Condições Gerais

As condições gerais definem **linhas ou colunas** no campo de batalha de uma equipe. Elas são usadas tanto para **determinar o posicionamento do ator** quanto para **filtrar os alvos de uma ação**.

Essas condições ajudam a expressar o papel posicional da ação (ex: frente, retaguarda, flanco).

- **Qualquer linha ou coluna**: qualquer linha ou coluna do campo da equipe.

- **Linhas**: condições que se aplicam a linhas inteiras do campo da equipe.
  - **Qualquer linha**: qualquer linha do campo da equipe.
  - **Linha superior**: apenas a linha mais acima do campo.
  - **Linha inferior**: apenas a linha mais abaixo do campo.
  - **Linha `N`**: apenas a linha `N` do campo.

- **Colunas**: condições que se aplicam a colunas inteiras do campo da equipe.
  - **Qualquer coluna**: qualquer coluna do campo da equipe.
  - **Coluna esquerda**: apenas a coluna mais à esquerda do campo da equipe.
  - **Coluna direita**: apenas a coluna mais à direita do campo da equipe.
  - **Coluna `N`**: apenas a coluna `N` do campo da equipe.

---

## Posicionamento

Antes de usar uma ação, o ator precisa atender às **condições de posicionamento** - ou seja, deve estar em uma célula válida no campo de sua equipe.

Essas condições podem ser definidas de forma [geral](#condições-gerais) ou [específica](#posições-específicas).

### Posições Específicas

Algumas ações exigem que o ator esteja **em células específicas** do campo, permitindo o design de habilidades táticas e posicionais.

> Exemplos:
>
> - `Posições([x,y], [x,y])`: a ação só pode ser usada se a personagem estiver em uma das células listadas.
> - `Em célula especial`: a célula precisa ter uma propriedade específica (ex: altar, terreno inflamável).

---

## Alvos

As ações afetam **alvos posicionados no campo de batalha**, cujas posições e naturezas são determinadas pela própria ação.

Assim como o posicionamento, os critérios de alvo podem ser definidos de forma [geral](#condições-gerais) ou [específica](#alvos-específicos).

### Afiliação

Cada ação define **qual grupo** pode ser afetado:

- **Auto**: afeta a própria personagem.
- **Aliados**: personagens da mesma equipe.
- **Inimigos**: personagens da equipe adversária.
- **Células específicas**: células do campo, com ou sem unidades (ex: para criar armadilhas ou invocar terreno).

### Alvos Específicos

Ações podem definir **formas complexas ou táticas** de seleção de alvo. Isso permite desde habilidades de alvo único até áreas condicionais dinâmicas.

> Exemplos:
>
> - `Posições([x,y], [x,y])`: afeta apenas as unidades nas células indicadas.
> - `Em linha reta`: afeta todas as unidades em linha reta a partir da posição do ator.
> - `Círculo Inimigo`: afeta todas as unidades em raio definido em torno de um ponto no campo inimigo.
> - `Círculo Aliado`: o mesmo, mas centrado no campo da própria equipe.
> - `Aliados próximos`: afeta todas as unidades em células adjacentes ao ator.
