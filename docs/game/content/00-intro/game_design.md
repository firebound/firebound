# Firebound – GDD

---

## Introdução

Título: Firebound  
**Subtítulo**: RPG Tático de Fantasia baseado em mitologias ameríndias
Desenvolvedor: Space Wizard Studios (Independente, Brasília-DF)

### Ficha Técnica

**Gênero Principal:** RPG Tático / Roguelike (runs independentes com meta‑progressão)  
**Subgêneros / Influências:** Estratégia em turnos, exploração segmentada procedural, construção situacional de equipe  
**Perspectiva:** Top-down 2.5D (personagens e efeitos estilizados sobre mapas 3D)
**Plataformas Alvo Inicial:** PC (Windows / Steam)
**Distribuição:** Digital (modelo premium – compra única, possibilidade futura de DLCs de conteúdo)  
**Modo de Jogo:** Single-player (co-op em estudo para futuro)  
**Sessão Média:** 30–60 minutos por run  
**Ciclo Médio de Aprendizado:** 2–3 runs para compreensão das mecânicas principais
**Público-Alvo Primário:** 16–35 anos, jogadores que apreciam *theorycraft* e combate tático
**Classificação Indicativa Estimada:** 12+ (temas de combate e elementos espirituais abstratos)  
**Engine / Stack:** Godot 4 + Framework em C#  
**Estado Atual:** Criação de personagens, inimigos e através da framework, protótipo de combate, interações básicas de combate
**Pilares Técnicos:** Modularidade de sistemas, conteúdo data-driven, edição e adição de conteúdos de forma facilitada pela Framework.

### Resumo

Você monta uma equipe, entra em uma região viva que reage às suas escolhas e enfrenta encontros táticos em turnos. Se cair, volta mais sábio: cada run falha devolve conhecimento para desbloquear novas opções, rotas e sinergias de **Essências**.

### Visão Geral

Firebound é um RPG tático roguelike estruturado em expedições sucessivas: você monta uma equipe, parte em uma incursão, enfrenta tensão crescente, decide quando arriscar ou recuar e retorna à base para tratar feridas, recuperar a estabilidade da equipe e desbloquear novas camadas de possibilidades — um ciclo de pressão, recuperação e preparação deliberada.

A mecânica principal é o combate tático em turnos sobre um terreno vivo, onde o posicionamento, controle de recursos e manipulação do ambiente são tão cruciais quanto as habilidades dos personagens. O terreno reage às ações do jogador e inimigos, criando oportunidades e desafios dinâmicos.

Para utilizar suas habilidades e interagir com esse ambiente, os personagens dependem de dados de **Essências** - recursos que representam forças primordiais do mundo fictício. Gerenciar quando e como usar esses dados adiciona uma camada estratégica profunda, influenciando tanto o combate quanto a exploração.

A exploração acontece em regiões de um mundo fictício, onde cada região é segmentada em áreas para se explorar. Nessas áreas, um mapa de rotas ramificadas é composto por "nós" em que cada nó representa um tipo de encontro (combate, evento ritual, anomalias, pontos de descanso, batalhas contra monstros elite e chefes intermediários). As rotas parcialmente previsíveis permitem planejar curvas de dificuldade, modular o risco versus a recompensa e preparar sinergias antes de confrontos decisivos. Muitos nós oferecem escolhas de tributo ou risco: sacrificar recursos imediatos para obter novos recursos e adquirir poderes temporários ou permanentes.

Na narrativa, cada expedição é uma travessia espiritual por regiões sagradas dentro de uma cosmologia sincrética inspirada em mitologias ameríndias. A espiritualidade funciona como moldura temática, onde os rituais e símbolos explicam por que o jogador adquire, transforma e combina dados de **Essências**. Cada dado representa um fragmento condensado dessas forças (Vida, Mundo, Sonhos) e a gestão desse conjunto (quais dados obter, quando rolar, quais rolar novamente ou quais **Essências** usar) cria urgência estratégica: atrasar uma decisão pode significar chegar a um nó crítico com faces inadequadas ou desperdício de potencial.

### Fantasia Central

O jogador assume o papel de explorar uma crise em curso onde irá viajar por regiões em declínio, decifrar sinais de rituais enigmáticos, compreender os acontecimentos que levaram áreas à ruína e escassez e compreender os ecos de vozes espirituais.

Nesse cenário, o jogador irá intervir realizando escolhas que revelam causas ocultas e alteram o destino local. A história não é despejada - ela é insinuada em fragmentos poéticos, objetos, inscrições e consequências sistêmicas após cada decisão.

A fantasia central combina três eixos:

1. Descoberta: cada região, área ou nó revelam pistas que conectam uma narrativa maior (profecias, corrupções, disputas de poder).

2. Intervenção: o jogador decide através de suas ações e escolhas se estabiliza, purifica, negocia ou explora ao limite para extrair mais **Essências** antes do colapso.

3. Resolução: a soma das pistas encontradas e escolhas efetuadas gera um estado final para aquele local (purificado, estabilizado, convertido, corrompido, esvaziado, etc). Esse estado produz efeitos persistentes: ajusta a tabela de encontros futuros, libera ou fecha a possibilidade de nós, altera a **Prevalência** de determinadas **Essências**, concede ou queima faces nos dados de rolagem e registram um fragmento narrativo que podem recontextualizar as regiões seguintes. A consequência não é aleatória - ela materializa a trajetória do jogador ali.

O jogador avança não só vencendo batalhas, mas decifrando padrões que transformam tributos e decisões de risco em acesso a novos dados de **Essências**, ampliando o repertório de respostas. O “progresso narrativo” é sentir o mundo reagir: rotas liberadas, tipos de encontro modificados, faces especiais adicionadas ao conjunto de possibilidades e mudanças sutis em descrições futuras.

### Loop Central da Jogabilidade

1. Recrutamento: obter ou resgatar personagens na cidade ou durante incursões para ampliar opções.
2. Preparação: montar equipe, configurar dados de iniciais e selecionar a região/área para a incursão.
3. Exploração Procedural: avançar pelos nós gerados (combate, evento, tributo, descanso, elite, etc.) escolhendo risco vs. estabilidade.
4. Escalada e Confronto: dificuldade aumenta conforme decisões (rotas, tributos, uso de recursos, etc.) até o encontro com o chefe da área.
5. Resolução e Retorno: resultado (vitória, retirada ou queda) gera efeitos persistentes, libera ajustes meta e reconfigura a próxima preparação.

Ciclo resumido: Recrutar -> Preparar -> Explorar -> Enfrentar Chefe -> Resolver e Reconfigurar.

### Diferenciais

- Ambiente vivo e modular: regiões, áreas e nós evoluem entre os encontros, não apenas durante um combate.
- Dados significativos: cada face tem agência (não são apenas números, mas gatilhos e modificadores situacionais).
- Resolução persistente de áreas: decisões selam estados que reconfiguram rotas e influenciam nas probabilidades futuras, mas sem ser aleatório ou causar impedimentos permanentes.
- Ritmo tensionado, mas controlado: o jogador escolhe quando escalonar risco ou consolidar ganhos antes da resolução do local.
- Sinergias flexíveis: combinações de **Essências** e personagens criam estilos variados, não builds fixas.
- Narrativa integrada nas mecânicas: fantasia mítica explica cada recurso e a transformação mecânica.
- Meta progressão curada: desbloqueios expandem opções sem invalidar runs iniciais.
- Leitura tática clara: a resolução do combate é feita de forma que as escolhas reduzam a carga cognitiva, sem perder a profundidade estratégica.

---

## Jornada e Gameplay

### Regiões

Macro-espaços temáticos que combinam bioma e a tensão narrativa. Cada Região possui um Chefe único a ser derrotado. Ele só é enfrentado quando os critérios de desbloqueio da região são atingidos (ex: completar um conjunto mínimo de Áreas ou cumprir missões-chave). A derrota ou resolução desse Chefe conclui a Região e libera a próxima.

Cada região contém: tabela de eventos própria, famílias de inimigos, alterações nos ambientes / cenários e curva de dificuldade condizente com o momento da campanha.

Ao ser resolvida, propaga efeitos amplos sobre todas as Áreas daquela Região (ajuste de frequências de nós raros, alteração de pesos de **Essências**, alterações nos ambientes de acordo com a resolução, abertura/fechamento de tipos de evento) e insere modificadores globais na próxima visita.

#### Áreas

Conjuntos fixos (missões / localidades) dentro de uma Região. Cada Área tem identidade própria (fantasia, objetivo narrativo, perfil de risco) e um conjunto de possibilidades delimitado de tipos de nó.

Características chave:

- Não são geradas proceduralmente em layout macro (existem como missões definidas). A variabilidade vem de: (1) quais nós elegíveis aparecem, (2) ramificações e ordem emergente, (3) modificadores herdados de resoluções anteriores (da própria Área ou da Região), (4) estado narrativo persistente.

Elites Nomeadas: algumas Áreas podem conter 0–N elites que funcionam como picos opcionais de risco e fonte de recompensas especializadas. Eles nunca substituem o Chefe regional, apenas modulam a curva interna da missão.

Resolver uma Área gera efeitos localizados (ou leves efeitos cruzados em outra Área da mesma Região): revelar fragmentos narrativos, alterar leve peso de um tipo de nó, desbloquear um evento específico ou ajustar faces potenciais em uma rolagem futura ou registrar. Esses efeitos são menores que a resolução da Região, mas criam sensação de impacto e continuidade.

##### Nós

Pontos de decisão interligados formando a rota dentro da Área. Tipos principais:

- Combate: encontro tático padrão (chance de faces e XP).
- Elite: combate modificado com mutação regional e recompensa reforçada.
- Subchefe: variante de pico (mecânica distinta) dentro da Área com loot estruturante.
- Evento: escolhas narrativo-mecânicas (tributo, transmutação de faces, pistas de resolução).
- Anomalia: efeito ambiental único que altera regras temporariamente.
- Descanso: janela de estabilização parcial (curar/status/ajustar dados) com custo de tempo/escalada.
- Encerramento da Missão: conclusão do objetivo da Área (pode envolver subchefe final local, prova ritual, cadeia de eventos). Gera o pacote de resolução da Área.

##### Expedições

Uma expedição = uma run em UMA única Área (missão). Você seleciona a Área, percorre seu mapa de nós até cumprir o objetivo ou falhar. Ao completar a Área, aplica-se o efeito de resolução local e acumula-se progresso para enfrentar (ou re-enfrentar) o Chefe regional. Completar o conjunto pretendido de Áreas (ou requisitos narrativos associados) habilita o encontro com o Chefe único daquela Região.

##### Eventos

Micro-narrativas e rituais que oferecem risco controlado ou conversão de recursos. Funcionam como:

- Informativos: pistas, lores e previsão tática na run atual;
- Transformadores: alteram faces dos dados e a prevalência deles na run atual;
- Sacrifício: troca com custo imediato por ganho diferido;
- Limiar: forçam uma escolha, rota alternativa ou travamento parcial de recurso na run atual;

### Cosmologia e Essências

A cosmologia de Firebound é inspirada em mitologias ameríndias, reinterpretando conceitos centrais para criar uma estrutura simbólica que fundamenta as mecânicas de jogo. O mundo é sustentado por três **Essências Primordiais** — Vida, Mundo e Sonhos. Elas são narrativa e sistema: condensam-se em dados roláveis, modulam encontros e sustentam resoluções que impactam diretamente na gameplay e na história.

O **Aspectos Derivados** representam manifestações focadas dessas forças, oferecendo especializações que alteram faces de dados, habilitam modificadores situacionais e criam ganchos de sinergia entre arquétipos.

#### Essência Vital – Ka’yaru, Deus da Vida

Abrange a cura e a morte, a regeneração e a degradação. Representa a constituição, o fim dos ciclos naturais e a manipulação dos atributos físicos e vitais.

Aspectos: Ayvu (cura e o renascimento), Karai (dissolução e o término).  

Impacto na gameplay: permite uso de habilidade para restauração, ressurreição, tratamento ou criação de penalidades e de degradação temporárias (veneno, enfraquecimento, feridas abertas, etc.).

#### Essência do Mundo – Mauyaq, o Renovador

Liga-se à transformação material e dinâmica do mundo (fogo/ar/água/terra). Representa a manipulação e mudança do ambiente e dos atributos elementares.

Aspectos: Ayratá (influxo do fogo e do ar - entrada catalítica), Ymarã (efluxo da água e da terra - liberação dissipativa).

Impacto na gameplay: permite alterar estados do terreno e em personagens (ignição / queimaduras, lama / lentidão, etc.), criar efeitos bônus ou blindagens temporárias (fortitude, resistência a elementos, etc.).

#### Essência dos Sonhos – Xelcan, o Observador

Trata da temporalidade e das percepções subjetivas, da memória. Representa a manipulação do tempo, ilusões e a influência psíquica sobre o ambiente e seres sencientes.

Aspectos: Ayvukéra (eco psíquico e do espirito - ego), Arakéra (eco do tempo e do cosmos - unidade).

Impacto na gameplay: permite antecipar ou retardar iniciativas, fixar faces úteis para próxima rolagem, projetar ilusões, ecoar parte de ação anterior ou adiar resolução de turnos

---

## Personagens

Personagens jogáveis e inimigos são definidos por arquétipos que determinam função tática, mecânicas e habilidades. A morte não é permanente — personagens incapacitados sofrem punições mas retornam entre expedições, mantendo a jornada cíclica.

### Arquétipos

Arquétipos definem identidade e função de personagens — tanto jogáveis quanto inimigos. Cada arquétipo combina função tática (Tank, Healer, DPS, Controle, Suporte), passivas únicas e habilidades temáticas.

**Personagens Jogáveis** ganham experiência durante expedições e desbloqueiam escolhas de habilidades nos níveis 5, 10 e 15 — o jogador seleciona entre 2–3 opções do pool do arquétipo, criando especialização gradual e builds distintas a partir da mesma base. Alguns arquétipos iniciam disponíveis, outros desbloqueiam ao cumprir condições narrativas.

**Inimigos** usam arquétipos fixos que determinam seu comportamento tático, mas não evoluem por nível — sua força escala via mutações de Elite e modificadores regionais.
O sistema prioriza **especialização temática sem rigidez**: dois arquétipos podem compartilhar uma Essência mas explorá-la diferentemente — Essência Vital sustenta ou corrói, Essência do Mundo protege ou queima, Essência dos Sonhos antecipa ou confunde.

### Arquétipos Jogáveis Iniciais

Místico das Marés — Seguidor de Umasit (divindade das águas), manipula correntes vitais como marés: cura e sustenta com Essência Vital (Ayvu), mas também controla fluxos aquáticos via Essência dos Sonhos (Ayvukéra), afogando ameaças ou antecipando dano. Healer preventivo que mantém grupo vivo em combates prolongados.

Guardião de Murikaan — Sentinela de terra e pedra. Usa Essência do Mundo (Ymarã) para moldar terreno em barreiras e cristalizar próprio corpo em fortaleza. Tank estático que absorve impactos, provoca inimigos e cria zonas seguras enquanto regenera defensivamente.

Caminhante dos Ventos — Emissária de Saikiliq (espírito dos ventos). Combina Essência do Mundo (Ayratá — ar acelerado) com Essência dos Sonhos (Arakéra — manipulação temporal) para executar alvos antes que reajam. DPS ágil que constrói momentum letal: cada golpe alimenta o próximo, eliminando prioridades rapidamente.

Arauto do Fogo Eterno — Canalizador de Ayratá (fogo/ar). Transforma campo em fornalha viva via ignição persistente que espalha e escala. DPS de área que nega terreno, força reposicionamento constante e pune agrupamentos — quanto mais queima, mais forte fica.

Tecelão de Sonhos — Manipulador puro de Essência dos Sonhos (Ayvukéra + Arakéra). Não enfrenta o presente, reescreve-o: dobra tempo para aliados agirem duas vezes, faz inimigos esquecerem ações, fixa faces de dados. Controlador que subverte ordem natural do combate e elimina aleatoriedade crítica.

Portador da Peste — Canal de Karai (dissolução da Essência Vital). Onde toca, a vida esfarela. Não mata instantaneamente — garante morte inevitável via venenos, feridas abertas e enfraquecimento acumulado. Debuffer/DoT que prepara elites para burst coordenado, criando zonas de contágio que ignoram defesas tradicionais.

### Inimigos

Cada região abriga **famílias temáticas** que refletem sua identidade narrativa: bestas adaptadas ao bioma, facções humanoides em conflito, abominações nascidas de corrupção e colossais que encarnam ameaças regionais. Inimigos reagem ao estado do mundo — alta **Prevalência** de uma **Essência** os fortalece ou corrompe, criando mutações táticas que espelham as escolhas do jogador.

#### Categorias

**Bestas** — Fauna regional adaptada ao bioma. Agressão direta, padrões previsíveis, fraquezas elementais claras. Introduzem mecânicas regionais (linha de visão, posicionamento, etc.) em contexto de baixo risco.

**Humanoides** — Facções hostis com motivações próprias: bandidos, amaldiçoados, exilados. Coordenam taticamente, usam habilidades especiais e se adaptam. Podem ter sub-arquétipos (healer, tank, disruptor), criando *mini puzzles* de priorização. Forçam gerenciamento de múltiplas ameaças simultâneas.

**Abominações** — Corrupções nascidas do desequilíbrio das essências. Violam regras padrão: agem fora de turno, persistem após morte, distorcem terreno. Desafiam familiaridade do jogador com sistemas base, exigindo adaptação e leitura cuidadosa de telegrafias.

**Colossais** — Mini chefes com mecânicas únicas e combate em fases. Presença estruturante que define o ritmo do encontro. Testam sinergia de grupo e gestão de múltiplos vetores de ameaça, preparando para os Chefes regionais.

#### Identidade e Progressão de dificuldade

Cada região define um conjunto de famílias, com estéticas unificadas e fraquezas coerentes.

A dificuldade escala dinamicamente: profundidade dos nós, escolhas de risco (tributos, rotas longas, etc.), **Prevalência** acumulada e histórico de vitórias determinam quando inimigos padrão se tornam **Mutações** e quando ocorre a aparição de **Elites Nomeadas**.

#### Mutações

As mutações refletem a **Prevalência** das **Essências**, causando disrupturas nos atributos e habilidades em inimigos padrão:

- **Essência Vital**: inimigos possuem mais regeneração passiva e efeitos de cura mas também causam envenenamento e doenças que corroem ao longo do tempo.
- **Essência do Mundo**: mais efeitos de controle de terreno (fogo, lentidão, etc.) e habilidades que alteram atributos físicos.
- **Essência dos Sonhos**: mais manipulação de turnos, ilusões e efeitos que confundem ou reposicionam.

#### Elites Nomeadas

As elites nomeadas possuem identidade persistente: nome único, mutações fixas, possibilidade de retorno em runs futuras se não eliminadas. Deixam recursos específicos (fragmentos narrativos) e influenciam eventos de nós adjacentes ou de runs futuras.

#### Chefes

Cada Região possui UM Chefe único que encarna sua identidade narrativa e mecânica. Combate estruturado em 3 fases:

**Fase 1 (Apresentação)** — Mecânica assinatura em versão controlada, padrão de ataque previsível, terreno relativamente neutro. Jogador testa estratégia.

**Fase 2 (Escalada)** — Mecânica intensifica (ex: cristalização captura personagens), convoca monstros adicionais ou transforma terreno. Teste de gestão de recursos e priorização.

**Fase 3 (Resolução)** — Mecânica atinge máxima intensidade mas revela fraqueza explorável. Janela de decisão: burst agressivo vs. manipular mecânica para resolução alternativa.

**Resoluções Múltiplas** — Não é binário (matar vs. falhar). Jogador pode: eliminar (agressivo), selar/purificar (ritual com tributo), negociar rendição (diplomático com condições). Cada resolução gera efeito regional distinto.

**Prevalência** — Chefe reage à **Prevalência**: ganha resistência àquela Essência, ou mecânica pode ser subvertida (ex: Sonhos 3 "congela" uma fase por 2 turnos, consumindo toda prevalência).

**Recompensas** — Resolução regional (propaga mudanças em todas Áreas), unlock de próxima Região, Fragmento de Chefe (item único equipável), Vestígios massivos.

---

## Mecânicas e Jogabilidade

### Combate Tático

Combate em turnos sobre **dois campos de batalha** divididos em células — um para cada equipe. Vitória depende de posicionamento estratégico, timing de recursos (dados de Essências) e manipulação de terreno, não apenas atributos brutos.

**Estrutura de Rodadas:** Dados rolados no início da fase de ações geram energia. Jogadores e inimigos declaram ações (jogador visualiza efeitos prováveis antes de confirmar). Turnos executam na ordem de iniciativa (velocidade). Personagens agem via três tipos de ações: **básicas** (mover para célula adjacente, defender, ataque básico com arma equipada), **habilidades básicas** (estilo de combate do arquétipo, disponíveis desde nível 1), e **habilidades especiais** (desbloqueadas por progressão, custo maior, efeitos poderosos).

**Ações e Alvos:** Ações são categorizadas como ofensivas, defensivas, movimento, buffs (fortalecer aliados) e debuffs (enfraquecer inimigos). Cada ação define requisitos de posicionamento (linhas/colunas específicas do campo próprio) e alvos válidos (auto, aliados, inimigos, células específicas). Alvos podem ser únicos, em área (círculo, linha reta, adjacentes) ou condicionais (ex: linha frontal inimiga, retaguarda aliada). Custo em energia varia conforme complexidade.

**Terreno Dinâmico:** Estados de célula (ignição, lama, cristalização, necrose, eco temporal) aplicados por ações ou Prevalência criam combos emergentes (ignição + lama = vapor cegante). Prevalência ≥2 aplica estados automaticamente; nível 3 amplifica área/duração. Personagens recebem buffs/debuffs com duração em turnos; reaplicar empilha duração. Resistências mitigam dano.

### Recursos

**Dados de Essências** — Recurso primário rolado no início de cada rodada. Geram energia para ativar habilidades. Obtidos via eventos/combates. Faces avançadas desbloqueáveis oferecem efeitos híbridos. Rerrolagens são limitadas por turno.

**Energia** — Gerada por rolagens, gasta para ativar habilidades. São temporárias (use ou perca) em uma batalha, mas acumulam quando não são gastas. Gestão tática: gastar agora vs. guardar para reação.

**Tributos** — Moedas recebidas em nós e usadas nos eventos (não carrega entre runs). Gastam para: desbloquear rotas, transmutar dados, negociar com NPCs, antecipar resolução de Área, etc.

**Vestígios** — Meta-moeda obtida ao completar/falhar Áreas. Gastam na Cidade para: desbloquear arquétipos, expandir conjunto de habilidades, comprar faces permanentes, ativar pactos, melhorar atributos base. Sistema generoso: falha dá 30–50% dos Vestígios de vitória.

**Equipamentos** — Itens que definem estilo e capacidades dos personagens. Dividem-se em: **Armas** (definem ataque básico e estilo de combate — lança, arco, borduna), **Armaduras** (proteção e resistências), **Botas** (mobilidade e vantagens em terrenos), **Adornos** (amuletos/colares com bônus especiais e proteção espiritual). Obtidos via combates, eventos e recompensas de Área. Inspirados em artefatos de culturas indígenas, reforçam identidade temática.

### Prevalência

Sistema de escala dinâmica que rastreia uso concentrado de **Essências** durante **Expedição**. Quanto mais ações de uma **Essência** específica são usadas, maior sua **Prevalência** (níveis 0–3). Efeitos escalonam progressivamente:

**Nível 1:** Permite conversões entre sub-essências relacionadas (ex: Ayvu → Karai se **Prevalência** de Essência Vital ≥1). Modificadores leves em eventos.

**Nível 2:** Ações dessa Essência aplicam estados de terreno automaticamente (fogo deixa ignição, cura espalha regeneração em adjacentes). Amplia efeitos narrativos em nós.

**Nível 3 (Limiar Crítico):** Máxima potência — estados se espalham +1 célula, duração dobrada, efeitos intensificados. **Porém, equilíbrio quebra:** inimigos na região sofrem mutações alinhadas àquela Essência (Essência Vital = regeneração/venenos agressivos, Essência do Mundo = controle de terreno hostil, Essência dos Sonhos = manipulação temporal/ilusões). Risco vs. recompensa: poder extremo atrai corrupção proporcional.

**Gestão:** Jogador escolhe especializar (explorar **Prevalência** 3 aceitando mutações) ou diversificar (manter equilíbrio, evitar threshold). Algumas resoluções de Área/Chefe exigem ou recompensam **Prevalência** específica.

### Progressão

**Durante a Expedição:** Personagens ganham XP por nó. Níveis 5/10/15 oferecem escolha de habilidades (2–3 opções do pool). Nós específicos concedem/transmutam faces de dados. Tributos aplicam buffs duradouros. Decisões em nós Ecológicos/Rituais modulam **Prevalência**. Cada run constrói "build temporária" customizada pela trajetória.

**Entre as Expedições (Meta):** Vestígios (obtidos em vitória/falha) desbloqueiam: arquétipos, habilidades extras ao pool, faces permanentes, ajustes de atributos, pactos narrativos. Pactos alteram regras (ex: começar com **Prevalência** 1 mas -1 slot). Memória Persistente recontextualiza eventos futuros.

**Gating:** Complexidade gradual. Primeiras 2–3 runs limitam escolhas; depois expandem. **Prevalência**/Resolução Local surgem após primeiro Chefe. Curva generosa: habilidosos vencem Região 1 em 3–5 runs, médios em 8–12.

### Falha

Falha é esperada e produtiva — recompensa tentativa e aprendizado. Sem penalidade permanente: personagens retornam íntegros (narrativa: "volta espiritual"). Vestígios reduzidos (30–50% da vitória) garantem progresso mesmo em derrota. Fragmentos narrativos registram pistas. **Prevalência** "Fantasma": falhar com **Prevalência** 3 inicia próxima run com **Prevalência** 1 daquela Essência.

**Recomeço Informado:** Estatísticas (dano, habilidades, causa), sugestões de ajuste, Vestígios desbloqueados, opção de reconfigurar equipe.

**Modos:** Eliminação total (equipe morre), Retirada estratégica (desistir em Descanso = 70% Vestígios), Corrupção irreversível (múltiplas Prevalências 3 = colapso do mundo + bônus narrativo).

### Exploração

**Cidade (Hub):** Centro de preparação entre expedições com as funcionalidades:

- **Recrutamento e Gestão de Tropa** de até 15 personagens, permitindo visualizar atributos e histórias;
- **Montar Equipe** de 6 slots organizados em 3 colunas × 2 linhas para a próxima expedição;
- **Equipar Personagens** trocar equipamentos (armas/armaduras/botas/adornos);
- **Gastar Vestígios** para desbloquear arquétipos disponíveis, habilidades, manipular runs e garantir melhorias permanentes;
- **Estruturas**: enfermaria, treinamento, loja, mural de missões - aprimoráveis ao longo do jogo);
- **Seleção de Região/Área** para escolher próxima expedição.

**Camadas da Expedição:** Região → Área → Nós. Cada camada com escolhas significativas.

- **Região:** Perfil de dificuldade, Essência dominante (**Prevalência** base), recompensas temáticas. Revisitável após resolução (altera encontros mantendo dificuldade ajustada).

- **Área:** Missão fixa com objetivo narrativo, dificuldade estimada, recompensas previstas, estado de resolução (afeta próxima incursão). Ordem não-linear (exceto tutorial).

- **Nós:** Mapa parcialmente revelado (3–5 à frente). Tipo visível (Combate, Elite, Evento, Descanso, Anomalia, Subchefe, Encerramento), recompensa vaga, riscos adicionais telegrafados. Bifurcações periódicas forçam escolhas (recursos vs. risco vs. narrativa). Previsibilidade tática (tipo conhecido) + surpresa interna (configuração variável).

---

## Produção e Estado do Projeto

### Estado Atual

Framework modular em desenvolvimento ativo: sistemas de combate tático (grid, turnos, iniciativa), atributos, ações e Essências em fase de integração. Ferramentas de criação de personagens e inimigos data-driven operacionais. Pipelines de arte (concept, sprites, animações) e áudio (efeitos, trilha, ambiência) em produção inicial.

### Escopo da Demo (12 meses)

**Slice vertical completo do loop:** Cidade (hub funcional) → Seleção de Área → Expedição (nós variáveis) → Combates táticos → Resolução de Área → Retorno com Vestígios → Meta-progressão.

**Conteúdo:**

- 1 Região jogável: Tundra de Sal com biomas cristalinos, terrosos e arenosos, identidade mecânica de cristalização e reflexos;

- 3 Arquétipos jogáveis completos: cada um com 4 opções habilidades básicas (níveis 5 e 10) + 2 opções de especiais (nível 15)
  - Místico das Marés
  - Guardião de Murikaan
  - Caminhante dos Ventos

- Ações básicas: Mover, Defender, Ataque Básico (varia por tipo  da arma equipada)

- Equipamentos:
  - Armas: 1 arma por arquétipo, com possíveis variações (ex: lança longa / lança curta)
  - Armaduras, Botas, Adornos: 2 variações de cada (ex: armadura leve/pesada, botas de couro/metálicas, adorno de proteção/força)

- Inimigos:
  - 10 inimigos comuns (Bestas, Humanoides e Abominações) adaptadas à Tundra com 3 variações de mutação (1 por Essência Vital/Mundo/Sonhos - simulando efeitos de Prevalência 3)
  - 2 Elites Nomeadas
  - 1 Chefe

- Eventos: Pelo menos 1 de cada tipo (Informativo, Transformador, Sacrifício, Limiar) - total de 6 Eventos

- Sistema de Prevalência funcional (rastreamento de uso, efeitos em níveis 1/2/3, mutações inimigas)

- Cidade (hub): Tropa (gestão 15 personagens), Montagem de Equipe (6 slots), Equipamentos, Gasto de Vestígios, Seleção de Área

- Tutorial integrado nas primeiras 2 expedições (gating progressivo de complexidade)

**Objetivo da demo:** Provar loop completo, demonstrar a progressão de dificuldade, validar sistemas de Essências/Prevalência, testar balanceamento inicial, gerar material para marketing (trailer, screenshots, wishlist Steam).

### Roadmap da Demo (12 meses)

**Trimestre 1 (Meses 1–3):** Consolidação da framework (combate grid funcional, sistema de turnos/iniciativa, ações básicas/especiais, Essências, atributos). Pipelines de arte/áudio estabelecidos. Protótipo vertical de 1 expedição curta (3 nós → combate simples).

**Trimestre 2 (Meses 4–6):** Criação de conteúdo core: 6 arquétipos completos (habilidades, passivas, animações), 10 inimigos comuns (behaviors, stats, sprites), sistema de **Prevalência** funcional (rastreamento, efeitos 1/2/3). Cidade (hub básico: tropa, equipe, vestígios). Eventos iniciais (4–6). Primeira play test interna.

**Trimestre 3 (Meses 7–9):** Chefe com 3 variações de mutação, 2 Elites Nomeadas, 60–70 equipamentos (4 slots × 6 arquétipos). Resolução de Área (efeitos persistentes, feedback narrativo). Tutorial integrado (gating progressivo). Estados de terreno avançados (combos emergentes). Polimento UX inicial (UI clara, feedback visual de estados).

**Trimestre 4 (Meses 10–12):** Balanceamento (simulação de padrões de turno, ajuste de curvas). Testes externos (alpha fechado, 20–30 jogadores). Polimento final (animações, efeitos, trilha, SFX). Preparação de marketing (trailer, press kit, página Steam). Lançamento da demo pública.

**Marco crítico (Mês 6):** Play test interno completo de loop funcional — se não atingido, redução de escopo (menos arquétipos ou inimigos).  

### Pilares de Design

- Leitura Clara, Profundidade Real.  
- Mundo Como Sistema Vivo, Não Cenário.  
- Falha Ensina, Não Pune Secamente.  
- Sinergias Emergentes Mais Que Builds Fixas.  
- Fantasia Mítica Integrada a Mecânicas.

### Riscos e Mitigação (Demo)

- **Escopo vs. Tempo:** Escopo agressivo para 12 meses com equipe 20h/semana. Mitigação: Marco crítico no Mês 6 (playtest funcional); plano B = reduzir para 4 arquétipos ou 6 inimigos.

- **Complexidade de Essências/Prevalência:** Sistema ambíguo pode confundir jogadores. Mitigação: Tutorial gated (expor mecânicas aos poucos), feedback visual claro (UI, ícones, cores), testes com usuários naive no Mês 9.

- **Balanceamento:** Curvas de poder e mutações desbalanceadas quebram experiência. Mitigação: Ferramenta de simulação de turnos (Mês 7), playtests frequentes (internos Mês 6, externos Mês 10).

- **Leitura Visual:** Estados múltiplos (terreno, buffs, debuffs, Prevalência) sobrecarregam tela. Mitigação: Camadas de informação progressivas (hover, click), paleta de cores consistente, iteração contínua de UI.

### Métricas de Sucesso (Demo)

- **Retenção:** Taxa de conclusão de tutorial > 70%, taxa de segunda expedição > 50%.
- **Sessão:** Duração média 40–60 min (1 expedição completa), taxa de retorno em 7 dias > 40%.
- **Engajamento:** Distribuição de arquétipos razoavelmente equilibrada (nenhum < 8%, nenhum > 25%), taxa de experimentação de Prevalência 3 > 30%.
- **Feedback qualitativo:** Clareza de sistemas (escala 1–5 > 3.5), "sinto que minhas escolhas importam" > 4.0.

### Pós-Demo (Visão de Longo Prazo)

Demo valida viabilidade do conceito e gera wishlist Steam. Plano pós-demo (sujeito a funding/receita):

- **Fase 2 (6–9 meses):** +2 Regiões, +4 arquétipos, meta-progressão expandida (pactos narrativos, memória persistente), polimento profundo.
- **Acesso Antecipado (12–18 meses pós-demo):** 3 Regiões completas, 10 arquétipos, 40+ inimigos, 60+ eventos, telemetria ativa, suporte a modding.
- **Versão 1.0 (24–30 meses pós-demo):** Conteúdo narrativo completo, 5 Regiões, modo cooperativo assíncrono experimental.

Decisão de continuar após demo baseada em: recepção crítica (métricas acima), viabilidade financeira (wishlist > 5k, possibilidade de funding), saúde da equipe (sustentabilidade de ritmo).

### Equipe de Desenvolvimento

**Danilo Nobre Nunes – Coordenador e Diretor Técnico**  
Responsabilidades: Gestão do projeto; desenvolvimento da framework e do jogo (programação e game design).  
Dedicação: 20h/semana.  
Mini-bio: Programador full-stack com experiência em aplicações web, XR e jogos. Cofundador da Space Wizard Studios, atua em planejamento, integração técnica-criativa e entrega para parceiros. Também desenvolvedor web no TSE em tecnologias educacionais.

LinkedIn: [danilo-nobre](https://www.linkedin.com/in/danilo-nobre/)

**Gustavo Arante Hugo – Diretor de Arte**  
Responsabilidades: Direção de arte, criação de personagens, inimigos, cenários e animações; preparação de assets para editor e modding; coordenação de testes de usabilidade e documentação de pipelines artísticos.  
Dedicação: 20h/semana.  
Mini-bio: Full-stack Developer no Banco do Brasil com mais de 7 anos como Concept Artist e Diretor de Arte para jogos e mídia digital. Experiência com clientes como Modus Games, Seven Galaxies TCG e Summon Studios; atuação adicional em branding e design editorial.

LinkedIn: [gustav-arantes](https://pt.linkedin.com/in/gustav-arantes)

**William Bernardes Magalhães – Diretor de Programação**  
Responsabilidades: MVP técnico da framework; editor visual de personagens; sistemas de combate, exploração e gerenciamento de equipes; testes automatizados e documentação; coordenação da versão alfa.  
Dedicação: 20h/semana.  
Mini-bio: Desenvolvedor full-stack focado em soluções escaláveis (APIs, microserviços e front-end). Atua na Greenole; interessado em otimização, novas linguagens e metodologias ágeis.

LinkedIn: [wbmagalhaes](https://www.linkedin.com/in/wbmagalhaes/)

**Alexandre Canto Corrêa Barbosa – Diretor de Som**  
Responsabilidades: Pipeline sonora da framework; produção de trilhas, efeitos e ambiências; direção de voz; materiais didáticos para workshops e tutoriais; áudio do jogo alfa.  
Dedicação: 20h/semana.  
Mini-bio: Profissional de áudio há mais de 10 anos em pós-produção para mídias digitais. Experiência em edição, mixagem, composição musical, direção de voz e Foley; participação em projetos como Heartstopper, Hilda, Power Rangers: Fúria Cósmica, Reacher, Invincible e Gen V.

LinkedIn: [alexandre-barbosa](https://www.linkedin.com/in/alexandre-barbosa-083b83b4/)

### Visão Técnica Resumida

- Engine: Godot 4 (C#) – escolha por licença permissiva, pipeline leve e integração ágil com ferramentas customizadas.  
- Linguagem Principal: C# (facilita testes unitários, organização modular e futura contribuição externa).  
- Arquitetura: Modular orientada a componentes + sistemas de dados dirigidos (Data-Driven) para ações, atributos e eventos; foco em serialização clara para modding.  
- Framework Interna: Camada de abstração para: Ações táticas, Estado de Terreno, Essências (sistema de modificadores reativos), Attr Store e Persistência entre runs.  
- Ferramentas Editor: Painéis customizados para criação de personagens, tabelas de eventos e tuning de Essências; exportação de configs em formato leve (provavelmente JSON/YAML) validado por schema.  
- Testes Automatizados: Cobertura priorizada em regras de turno, resolução de ações, integridade de modificadores e geração procedural determinística (seeds).  
- Dados Procedurais: Geração de segmentos regionais baseada em pesos condicionais (Essência dominante, histórico de decisões do jogador).  
- Persistência: Sistema de Legado grava desbloqueios e métricas de run (para Insights) em camada separada do balance runtime.  
- Escalabilidade de Conteúdo: Adição de novas Essências e arquétipos via configuração declarativa minimizando alterações em código núcleo.  
- Telemetria (planejada): Eventos de churn de tutorial, taxa de uso de arquétipos, padrões de falha em chefes para ajuste iterativo.
