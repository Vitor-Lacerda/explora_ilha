using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulaIlha : MonoBehaviour {

	public Transform playerTransform;


	[Space(10)]
	[Header("Pools das areas")]
	public AreaDataPool poolGrama;
	public AreaDataPool poolMato;
	public AreaDataPool poolAreasSeparadas;
	public AreaDataPool poolBoss;


	List<Area> areasGrama, areasMato, areasRuinas, areasBoss;

	// Use this for initialization
	void Start () {
		areasGrama = new List<Area> ();
		areasMato = new List<Area> ();
		areasRuinas = new List<Area> ();
		areasBoss = new List<Area> ();
	}


	public void Popula(Area[,] mapa, Config.Point spawn){
		playerTransform.position = new Vector2 (spawn.x, spawn.y);
		List<Area> listaMapa = mapa.Cast<Area> ().ToList ();
		areasGrama = listaMapa.Where (i => i.dificuldade == 1 && i.tipo == FazIlha.GRAMA).ToList();
		areasMato = listaMapa.Where (i => i.dificuldade == 2).ToList();
		areasRuinas = listaMapa.Where (i => i.dificuldade >= 3 && i.dificuldade <= 4).ToList();
		areasBoss = listaMapa.Where (i => i.dificuldade >= 5).ToList();


		PopulaGramas (mapa);
		PopulaMatos (mapa);
		PopulaSeparadas (mapa);

	}

	/***Metodos pra popular cada area***/

	public void PopulaGramas(Area[,] mapa){
		foreach (Area a in areasGrama) {
			if (a.vizinhosDificuldade [Config.DIFICULDADE_MINIMA] + a.vizinhosDificuldade [Config.DIFICULDADE_MATOALTO] <= 7) {
				ColocaParedes (a, poolGrama);
			} else {
				ColocaTesouro (a, poolGrama);
			}
		}
	}

	public void PopulaMatos(Area[,] mapa){
		foreach (Area a in areasMato) {
			//ColocaInimigos (a, prefabOnca);
			ColocaTesouro(a, poolMato);
			ColocaInimigos (a, poolMato);
			if (a.vizinhosDificuldade [Config.DIFICULDADE_MENORES] >= 2 || a.vizinhosDificuldade[Config.DIFICULDADE_MAIORES]>= 2) {
				ColocaParedes (a, poolMato);
			}
		}
	}

	public void PopulaSeparadas(Area[,] mapa){
		foreach (Area a in areasRuinas) {
			ColocaTesouro(a, poolAreasSeparadas);
			ColocaInimigos (a, poolAreasSeparadas);
		}
	}




	/***********************************/


	/****Metodos que colocam as coisas ***/

	void ColocaParedes(Area a, AreaDataPool pool){
		Interactable parede = pool.paredeAleatoria();
		a.ColocaObjeto (parede);
	}

	bool ColocaTesouro(Area a, AreaDataPool pool){
		if (a.tipo!= 1 && Random.Range (0f, 101f) <= pool.chanceTesouro) {
			a.ColocaObjeto (pool.tesouroAleatorio());
			return true;
		}
		return false;
	}

	bool ColocaInimigos(Area a, AreaDataPool pool){
		if (Random.Range (0f, 101f) <= pool.chanceInimigo) {
			a.ColocaInimigo (pool.inimigoAleatorio());
			return true;
		}
		return false;
	}

	/*
	public void ColocaDragao(Area[,] mapa, Config.Point pontoBoss){
		GameObject go = GameObject.Instantiate (prefabBoss.gameObject, new Vector3 (pontoBoss.x, pontoBoss.y, 0), Quaternion.identity);
		Personagem p = go.GetComponent<Personagem> ();
		for (int i = 0; i < 2; i++) {
			for (int j = 0; j < 2; j++) {
				mapa [pontoBoss.x + i, pontoBoss.y + j].EntrarNaArea (p);
			}
		}

	}
	*/





}
