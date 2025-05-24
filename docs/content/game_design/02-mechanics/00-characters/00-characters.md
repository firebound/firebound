# Personagens

## Composição

- Possuem:
  - [Atributos](./attributes/attributes.md)
  - [Papéis jogáveis](../05-game_content/roles/playable_roles.md) (players) ou [não jogáveis](../05-game_content/roles/unplayable_roles.md) (inimigos)
  - Slots para [equipamentos](./equipments)
- Realizam [ações](./actions) em combate

## Atributos

Os personagens herdam os atributos iniciais de acordo com o seu papel.

Cada papel possui um range inicial de valores para cada atributo.

Exemplo:

```txt
Mage:
Vida 30 - 40

Warrior
Voda 50 - 60
```

## Ações

Os personagens podem realizar apenas uma ação por turno.

Os personagens possuem ações básicas que podem ser utilizadas em combate:

- Mover;
- Defender;
- Ataque básico.

Os personagens herdam as habilidades iniciais de acordo com o seu papel e a cada 5 níveis ganham uma nova habilidade a ser escolhida entre 2 opções.

### Mover

- Sem custo;
- Move o personagem para um bloco adjacente.
  - Se o bloco está ocupado, pode trocar de lugar com o personagem que o ocupa.

### Defender

- Sem custo;
- Aumenta a defesa do personagem em 50% por 1 turno.

### Ataque básico

- Sem custo;
- Realiza a ação básica da arma equipada.

### Habilidades

- Possuem custo para serem utilizadas;
- Podem ser ações de papeis, equipamentos e talismãs.

### Consumíveis

- Usam itens no inventário para serem utilizadas.

## Morte

Quando um personagem chega a 0 de vida, ele é considerado morto e não pode mais ser utilizado em combate.

Ao morrer, o personagem é "transformado" em um item chamado "Restos mortais" que pode ser utilizado para reviver o personagem na [enfermaria](./locations/buildings#enfermaria) da cidade.
