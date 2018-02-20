using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class FazIlha : MonoBehaviour {
	public const int 	SPAWN = -1;
	public const int	GRAMA = 0;
	public const int	AGUA = 1;
	public const int	FINAL = 2;
	public const int	BOSS = 3;


	public GameObject prefabGrama, prefabAgua,prefabAreaFinal,prefabBoss;


	public float percentAguaInicial = 40;
	public int largura,altura;
	public int iteracoes = 5;
	public int iteracoesDif = 5;

	public float percentInicialBoss=35;
	public int larguraBoss, alturaBoss;
	public int iteracoesBoss = 1;
	public int tamMinimoBoss = 15;

	ConstrutorMapa mapaIlha, mapaTesouroFinal;
	Config.Point pontoBoss, pontoSpawn;
	Area[,] matrizFinal;



	/*** Secao de interface ***/

	public Area[,]  ConstroiIlha() {	
		mapaIlha = new ConstrutorMapa (largura, altura, iteracoes, iteracoesDif, percentAguaInicial);
		mapaIlha.matrizMapa = ColocaAreaFinal (mapaIlha.matrizMapa);

		EscolheSpawnJogador (mapaIlha.matrizMapa);
		DesenhaMapa (mapaIlha);

		matrizFinal = mapaIlha.matrizMapa;
		return matrizFinal;
		//DesenhaMapa(mapaIlha.matrizMapa);

	}


	public Config.Point GetPontoSpawn(){
		return pontoSpawn;
	}

	public Config.Point GetPontoBoss(){
		return pontoBoss;
	}



	/****************************/

	/****** Secao de construcao dos mapas automata ****/


	// Use this for initialization




	Area[,] FazMapaTesouroFinal(){
		mapaTesouroFinal = new ConstrutorMapa (larguraBoss, alturaBoss, iteracoesBoss, 1, percentInicialBoss);
		return mapaTesouroFinal.matrizMapa;
	}


	Area[,] ColocaAreaFinal(Area[,] mapa1){
		Area[,] mapaRetorno = new Area[mapa1.GetLength (0), mapa1.GetLength (1)];
		Area[,] mapa2 = FazMapaTesouroFinal ();

		Vector2 ponto = PegaPontoInicial (mapa1);
		int xCentro = (int)ponto.x;
		int yCentro = (int)ponto.y;

		int tamanhoX = mapa2.GetLength (0)/2;
		int tamanhoY = mapa2.GetLength (1)/2;


		int zeroX = xCentro - tamanhoX;
		int zeroY = yCentro - tamanhoY;

		int contagem2 = 0;


		while (contagem2 < tamMinimoBoss) {
			contagem2 = 0;
			for (int l, c = 0; c < mapaRetorno.GetLength (0); c++) {
				for (l = 0; l < mapaRetorno.GetLength (1); l++) {
					mapaRetorno [c, l] = mapa1 [c, l];
					if (c >= zeroX && c < xCentro + tamanhoX) {
						if (l >= zeroY && l < yCentro + tamanhoY) {
							int difX = c - zeroX;
							int difY = l - zeroY;
							if (mapa1 [c, l].tipo == GRAMA && mapa2 [difX, difY].tipo == GRAMA) {
								mapaRetorno [c, l].tipo = FINAL;
								mapaRetorno [c, l].dificuldade = 5;
								contagem2++;
							}
							if (difX == tamanhoX && difY == tamanhoY) {
								pontoBoss = new Config.Point (c, l);
								mapaRetorno [c, l].tipo = BOSS;
								mapaRetorno [c, l].dificuldade = 6;
							}

						}
					}
				}
			}
			if (contagem2 < tamMinimoBoss) {
				contagem2 = 0;
				mapa2 = FazMapaTesouroFinal ();
			}
		}

		return mapaRetorno;

	}

	Vector2 PegaPontoInicial(Area[,] mapa){
		bool achou = false;
		Vector2 v = new Vector2 (0, 0);
		int x = 0, y = 0;

		while (achou == false) {
			x = UnityEngine.Random.Range (0, mapa.GetLength (0));
			y = UnityEngine.Random.Range (0, mapa.GetLength (1));

			achou = (mapa [x, y].tipo == GRAMA && mapa[x,y].dificuldade > 1);
		}

		v.x = x;
		v.y = y;

		return v;

	}

	Config.Point LinhaSpawn(Area[,] mapa, int quadrante, int colunas, int linhas){
		bool horizontal = (UnityEngine.Random.Range (0, 101) >= 50);
		if (horizontal) {
			int linhaInicial = (quadrante == 1 || quadrante == 2) ? UnityEngine.Random.Range (linhas / 2, linhas - 1) : UnityEngine.Random.Range (1, (linhas / 2) - 1);
			int colunaInicial = (quadrante == 1 || quadrante == 4) ? 1 : colunas - 1;
			int c = colunaInicial;
			int l = linhaInicial;


			while (mapa [c, l].tipo == AGUA) {
				//mapa [c, l].tipo = SPAWN;
				if (quadrante == 1 || quadrante == 4) {
					c++;
				} else {
					c--;
				}

				if (c >= colunas - 1 || c <= 1) {
					c = colunaInicial;
					l++;
				}
				if (l >= linhas) {
					l = 1;
				}
			}

			return new Config.Point (c, l);


		} else {
			
			int linhaInicial = (quadrante == 1 || quadrante == 2) ? linhas-1 : 1;
			int colunaInicial = (quadrante == 1 || quadrante == 4) ? UnityEngine.Random.Range (1, colunas / 2) : UnityEngine.Random.Range(colunas/2+1, colunas-1);
			int c = colunaInicial;
			int l = linhaInicial;


			while (mapa [c, l].tipo == AGUA) {
				//mapa [c, l].tipo = SPAWN;
				if (quadrante == 1 || quadrante == 2) {
					l--;
				} else {
					l++;
				}

				if (l >= linhas || l <= 0) {
					l = linhaInicial;
					c++;
				}
				if (c >= colunas) {
					c = 1;
				}
			}
			return new Config.Point (c, l);
			
		}



	}

	void EscolheSpawnJogador(Area[,] mapa){
		Config.Point ponto = new Config.Point(0,0);


		//Descobre em qual "quadrante" ta o boss

		//Primeiro quadrante - superior esquerdo
		if (pontoBoss.x < mapa.GetLength (0) / 2 && pontoBoss.y >= mapa.GetLength(1)/2) {
			ponto = LinhaSpawn (mapa, 3, mapa.GetLength (0), mapa.GetLength (1));
		}

		//Segundo quadrante - superior direito
		if (pontoBoss.x >= mapa.GetLength (0) / 2 && pontoBoss.y >= mapa.GetLength(1)/2) {
			ponto = LinhaSpawn (mapa, 4, mapa.GetLength (0), mapa.GetLength (1));
		}

		//Terceiro quadrante - inferior direito
		if (pontoBoss.x >= mapa.GetLength (0) / 2 && pontoBoss.y < mapa.GetLength(1)/2) {
			ponto = LinhaSpawn (mapa, 1, mapa.GetLength (0), mapa.GetLength (1));
		}

		//Quarto quadrante - inferior esquerdo
		if (pontoBoss.x < mapa.GetLength (0) / 2 && pontoBoss.y < mapa.GetLength(1)/2) {
			ponto = LinhaSpawn (mapa, 2, mapa.GetLength (0), mapa.GetLength (1));
		}

		pontoSpawn = ponto;
		mapa [ponto.x, ponto.y].tipo = SPAWN;

	}

	
	void DesenhaMapa(ConstrutorMapa construtorMapa){
		GameObject g;
		Area[,] matrizMapa = construtorMapa.matrizMapa;
		for (int linha, coluna = 0; coluna < largura; coluna++) {
			for (linha = 0; linha < altura; linha++) {
				matrizMapa [coluna, linha].vizinhosDificuldade = construtorMapa.VizinhosDificuldade (coluna, linha);
				matrizMapa [coluna, linha].posicao = new Config.Point (coluna, linha);
				if (matrizMapa [coluna, linha].tipo == GRAMA) {
					g = Instantiate (prefabGrama, new Vector3 (coluna, linha), Quaternion.identity) as GameObject;
					matrizMapa [coluna, linha].objetoCena = g;
					//Pinta o nivel de vermelho do tile de acordo com a dificuldade.
					//1 = 0.2f 5 = 1
					Color c = g.GetComponent<SpriteRenderer> ().color;
					c.r = 0.2f * matrizMapa [coluna, linha].dificuldade;
					g.GetComponent<SpriteRenderer> ().color = c;

					/*
					if (matrizMapa [coluna, linha].dificuldade == 2 && matrizMapa [coluna, linha].vizinhosDificuldade [1] >= 2) {
						matrizMapa [coluna, linha].ColocaObjeto (prefabMatoAlto);
					}
					*/

				} else if (matrizMapa [coluna, linha].tipo == AGUA) {
					g = Instantiate (prefabAgua, new Vector3 (coluna, linha), Quaternion.identity) as GameObject;
					matrizMapa [coluna, linha].objetoCena = g;

				} else if (matrizMapa [coluna, linha].tipo == FINAL) {

					g = Instantiate (prefabAreaFinal, new Vector3 (coluna, linha), Quaternion.identity) as GameObject;
					matrizMapa [coluna, linha].objetoCena = g;

				} else {
					
					g = Instantiate (prefabBoss, new Vector3 (coluna, linha), Quaternion.identity) as GameObject;
					matrizMapa [coluna, linha].objetoCena = g;

				}
				matrizMapa [coluna, linha].SetSprite( g.GetComponent<SpriteRenderer>());

			}

		}

	}


}

public class ConstrutorMapa  {
	

	float percentAguaInicial;
	int largura,altura;
	int iteracoes;
	int iteracoesDif;
	int dificuldadeMediaInicial = 1;
	float chanceMutacao = 1f;
	int difMin = 1;
	int difMax = 5;


	public Area[,] matrizMapa;

	public ConstrutorMapa(int _largura, int _altura, int _iteracoes, int _iteracoesDif, float _percentInicial){
		largura = _largura;
		altura = _altura;
		percentAguaInicial = _percentInicial;
		iteracoes = _iteracoes;
		iteracoesDif = _iteracoesDif;

		ConstruirMapaNovo ();
	}

	public void ConstruirMapaNovo(){
		MapaInicial ();
		for(int i = 0; i<Mathf.Max(iteracoes,iteracoesDif); i++){
			if (i < Mathf.Min(iteracoes,iteracoesDif)) {
				GeraTudo ();
			} else {
				if (iteracoes > iteracoesDif) {
					GeraMapa ();
				} else {
					GeraDificuldades ();
				}
			}

		}



		/*
		for(int i = 0; i<iteracoes; i++){
			GeraMapa();
		}
		ZeraAguas ();

		for(int i = 0; i<iteracoesDif; i++){
			GeraDificuldades();
		}
		*/




		TiraDesconectadas();

	}

	void GeraMapa(){
		for (int linha, coluna = 0; coluna < largura; coluna++) {
			for (linha = 0; linha < altura; linha++) {
				matrizMapa [coluna, linha].tipo = RegraAutomataTipo (coluna, linha);
			}
		}
	}

	void GeraDificuldades(){
		for (int linha, coluna = 0; coluna < largura; coluna++) {
			for (linha = 0; linha < altura; linha++) {
				matrizMapa [coluna, linha].dificuldade = RegraAutomataDificuldade (coluna, linha);
			}
		}
	}

	void GeraTudo(){
		for (int linha, coluna = 0; coluna < largura; coluna++) {
			for (linha = 0; linha < altura; linha++) {
				matrizMapa [coluna, linha].tipo = RegraAutomataTipo (coluna, linha);
				matrizMapa [coluna, linha].dificuldade = RegraAutomataDificuldade (coluna, linha);
			}
		}
	}
	void ZeraAguas(){
		for (int linha, coluna = 0; coluna < largura; coluna++) {
			for (linha = 0; linha < altura; linha++) {
				if (matrizMapa [coluna, linha].tipo == 1 || matrizMapa [coluna, linha].vizinhosAgua >=2) {
					matrizMapa [coluna, linha].dificuldade = difMin;
				}
			}
		}
	}


	int RegraAutomataTipo(int c, int l){
		int vizinhos = AguasVizinhas (c, l);

		matrizMapa [c, l].vizinhosAgua = vizinhos;

		//Se for agua e tiver pelo menos 4 aguas em volta, vira agua.
		if (matrizMapa [c, l].tipo == 1) {
			if (vizinhos >= 4) {
				matrizMapa [c, l].custoAndar = int.MaxValue;
				return 1;
			}
			if (vizinhos < 2) {
				//Se tiver poucos vizinhos de agua, vira uma grama
				matrizMapa [c, l].custoAndar = 0;

				return 0;
			}
		} 
		//Se for grama e tiver 5 vizinhos de agua, vira agua
		else {
			if (vizinhos >= 5) {
				matrizMapa [c, l].custoAndar = int.MaxValue;

				return 1;
			} 

		} 


		//Se nao tiver vizinhos agua suficiente, vira grama.
		matrizMapa [c, l].custoAndar = 0;
		return 0;
	}

	int RegraAutomataDificuldade(int c, int l){
		int[] vizinhosDif = VizinhosDificuldade (c, l);
		int atual = matrizMapa [c, l].dificuldade;
		matrizMapa [c, l].vizinhosDificuldade = vizinhosDif;
		//Se for agua nao mexe.
		if (matrizMapa [c, l].tipo == 1 || matrizMapa[c,l].vizinhosAgua >= 3) {
			return difMin;
		}

		//Se nao for agua, muda a dificuldade.
		if (atual == difMin) {
			//"Faceis" eu trato como a agua.
			if (vizinhosDif [1] + vizinhosDif [2] >= 4) {
				return SetaDificuldade (atual);
			} else {
				return SetaDificuldade (atual + 1);
			}

		}
		else {
			if (vizinhosDif [1] >= 7) {
				int d = Mathf.Clamp (atual - 1, difMin+1, difMax);
				return SetaDificuldade (d);
			}
			if(vizinhosDif[0] >= 4) {
				return SetaDificuldade (atual + 1);
			}
			return SetaDificuldade (atual);
		}





	}

	int SetaDificuldade(int d){
		int f = d;

		f = Mathf.Clamp (f, difMin, difMax);
		return f;
	}

	int AguasVizinhas (int c, int l){
		int startC = c - 1;
		int startL = l - 1;
		int fimC = c + 1;
		int fimL = l + 1;

		int x = startC;
		int y = startL;

		int vizinhos = 0;

		for (x = startC; x <= fimC ; x++) {
			for (y = startL; y <= fimL; y++) {
				if (!(x==c && y==l)) {
					if (EAgua (x, y)) {
						vizinhos++;
					}
				}
			}
		}

		return vizinhos;
	}

	//Retorna um vetor. [0] = mais dificeis, [1] = mais faceis, [2] = iguais, [3] = media
	// [4] = dificuldade 1, [5] =  dificuldade 2, [6] = dificuldade 3, [7] = dificuldade 4, [8] = dificuldade 5, [9] = dificuldade 6
	public int[] VizinhosDificuldade(int c, int l){
		
		int[] vizinhos = new int[10];

		for (int i = 0; i < vizinhos.Length; i++) {
			vizinhos [i] = 0;
		}

		int startC = c - 1;
		int startL = l - 1;
		int fimC = c + 1;
		int fimL = l + 1;

		int x = startC;
		int y = startL;

		int cont = 0;

		float dif = matrizMapa [c, l].dificuldade;

		for (x = startC; x <= fimC ; x++) {
			for (y = startL; y <= fimL; y++) {
				if (!(x==c && y==l)) {
					int vd;
					if (EAgua (x, y)) {
						vd = difMin;
					} else {
						vd = matrizMapa [x, y].dificuldade;

					}
						vizinhos [3 + vd]++;
						if (vd > dif) {
							vizinhos [0]++;
						} else if (vd < dif) {
							vizinhos [1]++;
						} else {
							vizinhos [2]++;
						}
						vizinhos [3]+=vd;
						cont++;
				}
			}
		}

		vizinhos [3] = Mathf.RoundToInt ((float)vizinhos [3] / cont);

		return vizinhos;
	}

	bool EAgua(int c, int l){
		//Fora do mapa conta como agua
		if (!DentroMapa (c, l)) {
			return true;
		}
		return matrizMapa [c, l].tipo == 1;
	}

	//Retorna se o ponto e dentro do mapa
	bool DentroMapa(int c, int l){
		if (c < 0 || l < 0) {
			return false;
		}
		if (c > largura - 1 || l > altura - 1) {
			return false;
		}
		return true;
	}

	void MapaInicial(){
		matrizMapa = new Area[largura, altura];
		int meioMapa = (int)altura / 2;
		for (int linha, coluna = 0; coluna < largura; coluna++) {
			for (linha = 0; linha< altura; linha++) {
				//Se for borda é agua.
				if (coluna == 0 || linha == 0) {
					matrizMapa [coluna, linha] = new Area (FazIlha.AGUA, difMin);
				} else if (coluna == largura - 1 || linha == altura - 1) {
					matrizMapa [coluna, linha] = new Area (FazIlha.AGUA, difMin);
				}
				//Se nao, é agua aleatoriamente.
				else {
					if (linha == meioMapa) {
						matrizMapa [coluna, linha] = new Area (FazIlha.GRAMA, DificuldadeAleatoria());
					} else {
						matrizMapa [coluna, linha] = AguaAleatoria ();
					}
				}

			}
		}

	}

	int DificuldadeAleatoria(){
		/*
		int f = UnityEngine.Random.Range(difMin, difMax);
		f = Mathf.Clamp(f, difMin, difMax);
		return f;*/

		if (UnityEngine.Random.Range (0, 101) <= percentAguaInicial) {
			return difMin;
		}
		return UnityEngine.Random.Range(difMin+1, difMax);
	}

	Area AguaAleatoria(){
		if (UnityEngine.Random.Range (0, 101) <= percentAguaInicial) {
			return new Area(FazIlha.AGUA, difMin);
		}
		return new Area(FazIlha.GRAMA, DificuldadeAleatoria());
	}



	void TiraDesconectadas(){
		Config.Point pontoInicial = new Config.Point (largura / 2, altura / 2);
		List<Config.Point> lista = FloodFill (pontoInicial);
		if (lista.Count == 0) {
			return;
		}
		Config.Point np = new Config.Point (0, 0);
		for(int l,c = 0; c < largura;c++){
			for (l = 0; l < altura; l++) {
				np.x = c;
				np.y = l;
				if (!lista.Contains (np)) {
					matrizMapa [c, l].tipo = 1;
					matrizMapa [c, l].dificuldade = difMin;
				}

			}
		}
	}



	List<Config.Point> FloodFill(Config.Point startP){
		if (matrizMapa [startP.x, startP.y].tipo == 1)
			return new List<Config.Point> ();

		Queue<Config.Point> fila = new Queue<Config.Point> ();
		List<Config.Point> pontosArea = new List<Config.Point> ();
		fila.Enqueue (startP);
		pontosArea.Add (startP);
		while (fila.Count > 0) {
			Config.Point p = fila.Dequeue ();
			Config.Point np = new Config.Point (0, 0);
			if (!EAgua (p.x - 1, p.y)) {
				np.x = p.x - 1;
				np.y = p.y;
				if (!pontosArea.Contains (np)) {
					fila.Enqueue (np);
					pontosArea.Add (np);

				}
			}

			if (!EAgua (p.x + 1, p.y)) {
				np.x = p.x + 1;
				np.y = p.y;
				if (!pontosArea.Contains (np)) {
					fila.Enqueue (np);
					pontosArea.Add (np);

				}
			}

			if (!EAgua (p.x, p.y + 1)) {
				np.x = p.x;
				np.y = p.y + 1;
				if (!pontosArea.Contains (np)) {
					fila.Enqueue (np);
					pontosArea.Add (np);

				}
			}
			if (!EAgua (p.x, p.y - 1)) {
				np.x = p.x;
				np.y = p.y - 1;
				if (!pontosArea.Contains (np)) {
					fila.Enqueue (np);
					pontosArea.Add (np);

				}
			}




		}

		return pontosArea;
	}


}

