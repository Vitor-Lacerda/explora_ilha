using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulaIlha : MonoBehaviour {

	public Transform playerTransform;
	public float chanceTesouro = 10f;
	public float chanceInimigo = 0.8f;
	public Interactable prefabTesouro;
	public Personagem prefabOnca;
	public Personagem prefabAtirador;
	public Personagem prefabBoss;
	public Interactable prefabMatoAlto, prefabParede;

	List<Area> areasGrama, areasMato, areasRuinas, areasBoss;

	// Use this for initialization
	void Start () {
		areasGrama = new List<Area> ();
		areasMato = new List<Area> ();
		areasRuinas = new List<Area> ();
		areasBoss = new List<Area> ();
	}
	
	void Update(){
		
	}

	public void Popula(Area[,] mapa, Config.Point spawn){
		playerTransform.position = new Vector2 (spawn.x, spawn.y);
		List<Area> listaMapa = mapa.Cast<Area> ().ToList ();
		areasGrama = listaMapa.Where (i => i.dificuldade == 1 && i.tipo == FazIlha.GRAMA).ToList();
		areasMato = listaMapa.Where (i => i.dificuldade == 2).ToList();
		areasRuinas = listaMapa.Where (i => i.dificuldade >= 3 && i.dificuldade <= 4).ToList();
		areasBoss = listaMapa.Where (i => i.dificuldade >= 5).ToList();


		for (int linha, coluna = 0; coluna < mapa.GetLength(0); coluna++) {
			for (linha = 0; linha < mapa.GetLength(1); linha++) {
				ColocaTesouro (mapa, coluna, linha);
			}
		}
		PopulaBoss (mapa);
		PopulaGramas (mapa);
		PopulaMatos (mapa);

	}

	bool ColocaTesouro(Area[,] mapa, int coluna, int linha){
		if (mapa[coluna,linha].tipo!= 1 && Random.Range (0f, 101f) <= chanceTesouro) {
			mapa [coluna, linha].ColocaObjeto (prefabTesouro);
			return true;
		}
		return false;
	}

	bool ColocaInimigos(Area a, Personagem p){
		if (Random.Range (0f, 101f) <= chanceInimigo) {
			a.ColocaInimigo (p);
			return true;
		}
		return false;
	}

	public void ColocaDragao(Area[,] mapa, Config.Point pontoBoss){
		GameObject go = GameObject.Instantiate (prefabBoss.gameObject, new Vector3 (pontoBoss.x, pontoBoss.y, 0), Quaternion.identity);
		Personagem p = go.GetComponent<Personagem> ();
		for (int i = 0; i < 2; i++) {
			for (int j = 0; j < 2; j++) {
				mapa [pontoBoss.x + i, pontoBoss.y + j].EntrarNaArea (p);
			}
		}

	}

	public void PopulaGramas(Area[,] mapa){
		foreach (Area a in areasGrama) {
			if(a.vizinhosDificuldade[4] + a.vizinhosDificuldade[5] <= 6) {
				a.ColocaObjeto (prefabParede);
			}
		}
	}

	public void PopulaMatos(Area[,] mapa){
		foreach (Area a in areasMato) {
			ColocaInimigos (a, prefabOnca);
			if (a.vizinhosDificuldade [1] >= 2 || a.vizinhosDificuldade[0]>= 2) {
				a.ColocaObjeto (prefabMatoAlto);
			}
		}
	}

	public void PopulaBoss(Area[,] mapa){
		int aberto = 0;
		foreach (Area a in areasBoss) {
			if(a.vizinhosDificuldade[1] >=2 && Random.Range(0f,101f)<=100-chanceTesouro) {
				if (aberto <= 20) {
					aberto++;
				} else {
					a.ColocaObjeto (prefabParede);
				}
			}
		}
	}
}
