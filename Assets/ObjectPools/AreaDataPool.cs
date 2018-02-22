using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AreaDataPool : ScriptableObject {
	public float chanceInimigo = 0.8f;
	public float chanceTesouro = 10f;

	public List<Inimigo> inimigos;

	public List<Interactable> paredes;

	public List<Interactable> tesouros;

	public Inimigo inimigoAleatorio(){
		return inimigos[Random.Range(0, inimigos.Count)];
	}
	public Interactable paredeAleatoria(){
		return paredes[Random.Range(0, paredes.Count)];
	}
	public Interactable tesouroAleatorio(){
		return tesouros[Random.Range(0, tesouros.Count)];
	}


}
