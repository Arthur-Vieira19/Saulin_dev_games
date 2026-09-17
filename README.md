# Saulin Dev Games

Dois jogos feitos em **Unity 2021.3.20f1**, a partir dos protótipos 3 e 5 do curso
*Create with Code* (base original: [gmarnold/unity-create-with-code](https://github.com/gmarnold/unity-create-with-code)).
Os dois foram repaginados: paleta nova, nomes novos, HUD própria e mudanças de mecânica —
não são cópias do repositório de origem.

> As pastas geradas pelo Unity (`Library/`, `Temp/`, `obj/`, `Logs/`, `.vs/`, `.sln`, `.csproj`)
> **não** fazem parte do repositório. O Unity recria todas elas ao abrir o projeto.

## Como abrir

1. Unity Hub → **Add** → selecione a pasta do jogo (`Jogo 3 - Fuga Neon` ou `Jogo 5 - Toque Citrico`).
2. Abra com a versão **2021.3.20f1** (outras versões próximas devem funcionar, com upgrade automático).
3. A cena principal já está registrada em *Build Settings*:
   `Assets/Scenes/Fuga Neon.unity` e `Assets/Scenes/Toque Citrico.unity`.

---

## Jogo 3 — Fuga Neon

Corrida lateral: o corredor foge por uma pista neon e precisa saltar os obstáculos.

**Controles**

| Tecla | Ação |
|---|---|
| `Espaço` / `↑` | Pular (pulo duplo no ar) |
| `Shift` (ou `Z`) | Turbo — pista mais rápida e pontos em dobro |
| `R` | Recomeçar depois do game over |

**O que mudou em relação ao original**

- **Visual neon**: fundo roxo profundo, luz direcional violeta, sprite do cenário tingido de
  violeta e chão com material próprio (`Assets/Materials/Neon_Ground.mat`, com emissão magenta).
- **`NeonMood.cs`** (novo, na Main Camera): o fundo pulsa entre dois tons, esfria para ciano
  durante o turbo e pisca em vermelho quando o jogador bate.
- **HUD própria** desenhada pelo `PlayerController`: pontos, recorde salvo em `PlayerPrefs`
  e aviso dos controles — o original não tinha placar nenhum.
- **Dificuldade progressiva**: o `SpawnManager` diminui o intervalo entre obstáculos a cada
  spawn (de 2,1s até 0,95s) e sorteia a escala de cada obstáculo. Antes era `InvokeRepeating`
  com intervalo fixo.
- **Pontuação num lugar só**: antes cada objeto de cenário com `MoveLeft` somava pontos por
  quadro (o placar dependia de quantos objetos estavam na tela). Agora quem pontua é o
  `PlayerController`.
- **Gravidade corrigida**: `Physics.gravity` passou a ser atribuída de forma absoluta, então o
  valor não se acumula a cada reinício da cena.
- Ajustes de sensação: pulo 500 → 560, gravidade 1,5 → 1,75, velocidade base 10 → 12,5,
  turbo 1,5× → 1,8×.

## Jogo 5 — Toque Cítrico

Clique nas frutas que sobem da parte de baixo da tela e desvie das estragadas.

**Controles**: mouse. Escolha a intensidade na tela de título — **Suave**, **Médio** ou **Ácido**.

**O que mudou em relação ao original**

- **Paleta cítrica**: fundo verde-petróleo escuro, botões em lima com destaque laranja,
  textos em amarelo cítrico, borda verde-folha e grade em creme.
- **Combo**: acertos seguidos multiplicam os pontos (até 5×) e mudam a cor do placar
  (verde → âmbar → laranja). Tocar numa fruta estragada zera o combo.
- **Sistema de vidas**: deixar uma fruta boa cair custa um "suco" (3 no total) em vez de
  encerrar a partida na hora.
- **Ritmo crescente**: o intervalo de spawn encurta sozinho durante a partida
  (até o limite de 0,3s), além do divisor de dificuldade escolhido.
- **Recorde** salvo em `PlayerPrefs` e mostrado na tela de fim de jogo.
- **Frutas variadas**: cada uma sai com cor cítrica sorteada, escala e torque aleatórios,
  e o estouro de partículas sai na mesma cor da fruta tocada.
- **Botões coloridos por intensidade** (`DifficultyButton` pinta o próprio `ColorBlock`).
- Limpeza: o `using UnityEditor;` do `GameManager` foi removido — ele quebrava a compilação
  de builds fora do editor.

---

Assets de arte e som vêm da *Course Library* do curso Create with Code, da Unity.
