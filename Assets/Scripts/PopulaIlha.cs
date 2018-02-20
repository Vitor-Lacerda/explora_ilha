using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulaIlha : MonoBehaviour {

	public Transform playerTransform;
	public float chanceTesouro = 10f;
	public float chanceInimigo = 0.8f;
	public Interactable prefabTesouro;
	public Personagem prefabInimigo;
	public Personagem prefabBoss;

	// Use this for initialization
	void Start () {
		
	}
	
	void Update(){
		
	}

	public void Popula(Area[,] mapa, Config.Point spawn){
		playerTransform.position = new Vector2 (spawn.x, spawn.y);

		for (int linha, coluna = 0; coluna < mapa.GetLength(0); coluna++) {
			for (linha = 0; linha < mapa.GetLength(1); linha++) {
				if (!ColocaTesouro (mapa, coluna, linha)) {
					ColocaInimigos (mapa, coluna, linha);
				}
			}
		}

	}

	bool ColocaTesouro(Area[,] mapa, int coluna, int linha){
		if (mapa[coluna,linha].tipo!= 1 && Random.Range (0f, 101f) <= chanceTesouro) {
			mapa [coluna, linha].ColocaObjeto (prefabTesouro);
			return true;
		}
		return false;
	}

	bool ColocaInimigos(Area[,] mapa, int coluna, int linha){
		if (mapa[coluna,linha].tipo!= 1 && Random.Range (0f, 101f) <= chanceInimigo) {
			mapa [coluna, linha].ColocaInimigo (prefabInimigo);
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
}
