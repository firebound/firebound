# Personagens

Esta seção apresenta as principais mecânicas que definem as personagens do jogo.

A ideia é fornecer uma visão geral das mecânicas que regem as personagens, incluindo atributos, habilidades, progressão e interações em combate, mas não entrar em detalhes específicos de como essas mecânicas são implementadas no jogo, como na narrativa, gráficos, ou aspectos de balanceamento e etc.

Nesta página, você encontrará uma visão geral resumida de cada mecânica, com links para seções mais detalhadas onde você pode explorar cada aspecto em profundidade.

---

## Progressão

As personagens evoluem ao longo do jogo, adquirindo experiência, aumentando atributos e desbloqueando novas habilidades.

Para mais detalhes, consulte a seção de [Progressão](progression/progression.md).

---

## Papéis e Arquétipos (Roles)

Cada personagem pertence a um arquétipo que define suas características básicas, habilidades, estilos de jogo e funções em combate.

> Exemplos:
>
> - **Guardião** é um arquétipo de combate corpo a corpo, com alta força e defesa, ideal para proteger aliados e causar dano direto.
> - **Xamã** é um arquétipo de suporte, com habilidades de cura e controle, capaz de manipular elementos naturais para ajudar a equipe.
> - **Arqueiro** é um arquétipo de ataque à distância, com alta agilidade e precisão, focado em causar dano a inimigos distantes.

Para mais detalhes, consulte a seção de [Papéis e Arquétipos](roles/roles.md).

---

## Atributos

As personagens possuem **Atributos Básicos** que determinam seu desempenho em combate. Os valores iniciais variam conforme os papéis e arquétipos (role) das personagens.

> Exemplo: Um curandeiro pode começar com 30-40 de vida, enquanto um guerreiro tribal começa com 50-60.

As personagens também possuem **Resistências Elementais**, que influenciam seus ataques e defesas dos elementos como fogo, água, terra e vento.

> Exemplo: Uma xamã pode ter alta afinidade com o elemento água, aumentando o dano de seus ataques aquáticos.

E, além disso, as personagens têm **Resistências Físicas**, que reduzem o dano recebido de ataques físicos cortantes, perfurantes ou contundentes.

> Exemplo: Um guerreiro com armadura de couro pode ter alta resistência a ataques cortantes, mas baixa resistência a ataques perfurantes.

Para mais detalhes, consulte a seção de [Atributos](attributes/attributes.md).

---

## Energia

As personagens utilizam Energia para realizar a maioria de suas habilidades em combate.

Para mais detalhes, consulte a seção de [Energia](energy/energy.md).

---

## Passivas

Habilidades passivas não requerem ação direta para serem ativadas, mas influenciam o desempenho da personagem de forma contínua ou sob certas condições.

Para mais detalhes, consulte a seção de [Passivas](passives/passives.md).

---

## Ações

Cada personagem pode realizar uma ação por turno em combate.

As **Ações Básicas** são comuns a todas as personagens e incluem mover, defender e atacar com sua arma (ou ataque básico do **Papel**).

As **Habilidades** são determinadas pelo arquétipo e papel da personagem, podendo ser ações ofensivas, defensivas ou de suporte.

> Exemplo: Mover para um bloco adjacente, defender para aumentar a defesa temporariamente, ou usar uma habilidade inspirada em rituais ancestrais do clã.

Para mais detalhes, consulte a seção de [Ações](actions/actions.md).

---

## Efeitos de Status

Durante o combate, as personagens podem ser afetadas por **efeitos temporários** que modificam seus atributos, ações ou comportamento.

Esses efeitos podem ser positivos (buffs) ou negativos (debuffs) e são aplicados por habilidades, itens ou condições ambientais.

> Exemplos: envenenamento, atordoamento, fortalecimento, regeneração.

Para mais detalhes, consulte a seção de [Efeitos de Status](status-effects/status-effects.md).

---

## Equipamentos

Personagens possuem slots para equipamentos, que modificam atributos, concedem novas ações ou habilidades e influenciam a estratégia de combate.

> Exemplo: Um cajado cerimonial permite ataques à distância, enquanto uma lança tribal define o tipo de ataque básico disponível.

Para mais detalhes, consulte a seção de [Equipamentos](equipments/equipments.md).

---

## Morte

Quando uma personagem chega a 0 de vida, ele é considerado morto e não pode mais ser usado em combate. Personagens mortos podem ser revividos por meio de itens ou locais específicos do jogo.

> Exemplo: a personagem morto se transforma em um item "Restos mortais", que pode ser usado para reviver a personagem na "enfermaria" da aldeia.
