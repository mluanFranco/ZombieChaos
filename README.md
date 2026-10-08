# 🧟 Zombie Chaos

FPS de sobrevivência pós-apocalíptico desenvolvido em **Unity 6 (HDRP)** com **C#**, inspirado em *Left 4 Dead*.

## 🎮 Sobre o jogo

Você está preso em uma antiga zona industrial e precisa sobreviver a ondas de inimigos que ficam cada vez maiores e mais fortes. O jogo não tem fim definido: o objetivo é descobrir até qual onda você consegue chegar.

- **Ondas progressivas:** a cada nova onda, a quantidade de inimigos aumenta, com subondas de spawn ao longo da partida.
- **5 tipos de inimigos:** Zumbis, Zumbis Mutantes, Aranhas, Lobisomens e o Migo, uma criatura voadora que dispara bolas de ácido e sobrevoa obstáculos do mapa.
- **Sistema de dano por região:** os acertos causam dano baixo, médio ou alto, conforme a parte do corpo atingida.
- **Pontuação e loja de armas:** cada baixa rende pontos, que podem ser trocados por novas armas e munição na caixa de armas do mapa.
- **Inventário com 2 armas:** a pistola inicial e uma arma secundária comprada (Shotgun, SMG, AK-47, M4A1 ou LMG).
- **Coletáveis:** kits médicos surgem periodicamente pelo mapa, e a vida é restaurada ao final de cada onda.
- **Qualidade gráfica ajustável:** opções Baixo, Médio e Alto, para rodar bem em máquinas mais simples.

## 🕹️ Controles

| Tecla | Ação |
|---|---|
| `W` `A` `S` `D` | Movimentação |
| `Shift` | Correr |
| `Espaço` | Pular |
| `Mouse Esquerdo` / `Direito` | Atirar / Mirar |
| `R` | Recarregar |
| `1` / `2` | Trocar de arma |
| `F` | Interagir (loja de armas) |
| `Esc` | Pausar |

## 🛠️ Tecnologias

- **Unity 6.4** (6000.4.9f1) com pipeline **HDRP**
- **C#** para toda a lógica de gameplay
- **NavMesh / NavMeshAgent** para a movimentação dos inimigos
- **Animator** com eventos de animação
- **Cinemachine** para a câmera em primeira pessoa

## 📦 Créditos

Os modelos 3D, as animações, os áudios e os elementos de interface são assets de terceiros, obtidos na Unity Asset Store, no Sketchfab e no itch.io.

Modelos de Armas 3D
- Pistola: https://sketchfab.com/3d-models/animated-pistol-bd896167e7ca44f19597d3afe6a8d83f
- Shotgun: https://sketchfab.com/3d-models/shotgun-animated-226237e0f49c4f69a9fc5a78df6a236c
- SMG: https://sketchfab.com/3d-models/fps-animated-smg-ea3dad7478624495a5a46f40127b0579
- AK-47: https://sketchfab.com/3d-models/fps-ak-animated-b0b1bae40337449189483f6187cf7af7
- M4A1: https://sketchfab.com/3d-models/fps-animated-carbine-62977bb4c53047a185b9f3a0cdf56b87
- LMG: https://sketchfab.com/3d-models/lmg-animated-c02dc6663f344008a502148e751c9974

Pacote de Armas – UI (Loja de Compra de Armas):
https://assetstore.unity.com/packages/2d/gui/icons/gun-icons-package-287096

Pacote Zombies (Inimigos)
- https://assetstore.unity.com/packages/3d/characters/humanoids/free-shirtless-zombie-276762
- https://drive.google.com/drive/folders/1cwf4KocO0PCgXmEDX95cWhQqPGtIsKm5
- https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/zombie-monster-animations-free259680

Pacote Zombie Mutante (Inimigo):
https://assetstore.unity.com/packages/3d/characters/creatures/morbid-creatures-mutant-1-289874

Pacote Aranha (Inimigo):
https://assetstore.unity.com/packages/3d/characters/creatures/free-fantasy-spider-10104

Pacote Lobisomem (Inimigo):
https://assetstore.unity.com/packages/3d/characters/creatures/creep-horror-creature-244853

Pacote Migo (Inimigo):
https://assetstore.unity.com/packages/3d/characters/creatures/mi-go-254165

Bola Ácida (Utilizado junto ao Inimigo Migo):
https://drive.google.com/drive/folders/1grg10ZOsDohWz_1JGQM5VQYQj4081kIB

Mapa de Jogo:
https://assetstore.unity.com/packages/3d/environments/industrial/rpg-fps-game-assets-for-pc-mobile-industrial-setv3-0-101429

Poste:
https://assetstore.unity.com/packages/3d/props/exterior/street-lights-pack-31644

Destroços de Construção:
https://assetstore.unity.com/packages/3d/props/exterior/rubble-and-debris-modular-set-free-sample-118763

Pacote de Áudio (Passos):
https://assetstore.unity.com/packages/audio/sound-fx/foley/footsteps-essentials-189879

Feedback de Dano Áudio (Jogador)
- https://assetstore.unity.com/packages/audio/sound-fx/horror-elements-112021
- https://assetstore.unity.com/packages/audio/sound-fx/voices/damage-sounds-male-npc-player-audio-pack285385

Pacote de Áudio Armas
- https://f8studios.itch.io/snakes-authentic-gun-sounds
- https://f8studios.itch.io/snakes-second-authentic-gun-sounds-pack
- https://lmglolo.itch.io/free-fps-sfx

Som de Sangue:
https://assetstore.unity.com/packages/audio/sound-fx/horror-game-essentials-153417

Som de Zombie:
https://assetstore.unity.com/packages/audio/sound-fx/zombie-sound-pack-free-version-124430

Som de Migo:
https://assetstore.unity.com/packages/audio/sound-fx/free-deadly-kombat-228835

Fundo Musical:
https://assetstore.unity.com/packages/audio/sound-fx/horror-elements-112021

Coletáveis – Boxes Pack:
https://assetstore.unity.com/packages/3d/props/furniture/boxes-pack-32717
