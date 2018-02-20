using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area  {

	public Config.Point posicao;
	public int tipo;
	public int dificuldade;
	public int vizinhosAgua;
	public int[] vizinhosDificuldade;
	public int custoAndar;
	public Personagem ocupante;
	public GameObject objetoCena;
	public int bloqueioVisao;

	SpriteRenderer sprite;
	Color corInicial;


	public List<Interactable> _interactables;
	public List<Interactable> interactables
	{
		get{
			if (_interactables == null) {
				_interactables = new List<Interactable> ();
			}
			return _interactables;
		}

		set{
			if (_interactables == null) {
				_interactables = new List<Interactable> ();
			}
			_interactables = value;
		}

	}

	public List<Interactable> _toRemove;
	public List<Interactable> toRemove
	{
		get{
			if (_toRemove == null) {
				_toRemove = new List<Interactable> ();
			}
			return _toRemove;
		}

		set{
			if (_toRemove == null) {
				_toRemove = new List<Interactable> ();
			}
			_toRemove = value;
		}

	}
		




	public Area(int t, int d){
		tipo = t;
		dificuldade = d;
		vizinhosAgua = 0;
		vizinhosDificuldade = new int[4];
		if(tipo == 1){
			custoAndar = int.MaxValue;
		}
		else{
			custoAndar = 0;
		}
			

	}

	public void ColocaObjeto(Interactable obs){
		Vector3 pos = new Vector3 (posicao.x, posicao.y);
		GameObject o = GameObject.Instantiate (obs.gameObject, pos, Quaternion.identity) as GameObject;
		interactables.Add (o.GetComponent<Interactable>());
		custoAndar = Mathf.Max (custoAndar, obs.custoAndar);
		o.transform.parent = objetoCena.transform;
		bloqueioVisao = Mathf.Max (bloqueioVisao, obs.bloqueioVisao);
	}

	public void ColocaInimigo(Personagem inimigo){
		Vector3 pos = new Vector3 (posicao.x, posicao.y);
		GameObject inim = GameObject.Instantiate (inimigo.gameObject, pos, Quaternion.identity) as GameObject;
		EntrarNaArea (inim.GetComponent<Personagem> ());
	}

	public void Interact(bool entrando){
		if (interactables.Count > 0) {
			foreach (Interactable i in interactables) {
				i.Interact (this, entrando);
			}
		}
		if (toRemove.Count > 0) {
			foreach (Interactable i in toRemove) {
				interactables.Remove (i);
			}
			toRemove.Clear ();
		}

	}
	public void RemoveObstaculo(Interactable obs){
		toRemove.Add (obs);
	}

	public void EntrarNaArea(Personagem personagem){
		ocupante = personagem;
		personagem.transform.parent = objetoCena.transform;
	}

	public void SairDaArea(Personagem personagem){
		ocupante = null;
	}

	public void SetSprite(SpriteRenderer sr){
		sprite = sr;
		corInicial = sr.color;
	}

	public void Highlight(bool b){
		if (b) {
			sprite.color = Color.cyan;
		} else {
			sprite.color = corInicial;
		}
	}

	public void Mostra(){

		for (int i = 0; i < objetoCena.transform.childCount; i++) {
			if (i == 0) {
				objetoCena.transform.GetChild (i).gameObject.GetComponent<SpriteRenderer>().enabled = false;
			} else {
				objetoCena.transform.GetChild (i).gameObject.GetComponent<SpriteRenderer>().enabled = true;
			}
		}
	}

}
