using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atirador : Inimigo {


	public int alcanceTiro = 3;
	public bool atiraDiagonal = false;
	public bool acertaTodos = false;
	protected List<Area> espacosAtaque;

	protected override void Start ()
	{
		base.Start ();
		espacosAtaque = new List<Area> ();
		raioVisao = alcanceTiro;
		spriteRenderer = this.GetComponent<SpriteRenderer> ();
	}

	protected override void PreparaAtaque ()
	{
		base.PreparaAtaque ();
		if (timerAtaque >= tempoAtaque) {
			StartCoroutine (Ataque ());
			timerAtaque = 0;
		}
	}

	protected override IEnumerator Ataque ()
	{
		estado = 2;
		stepTimer = 0;
		spriteRenderer.color = Color.black;
		bool acertou = false;
		for (int i = 0; i < espacosAtaque.Count; i++) {
			espacosAtaque [i].Highlight (false);
			yield return null;
			if ((!acertou || acertaTodos) && espacosAtaque [i].ocupante != null) {
				espacosAtaque [i].ocupante.TomarDano (poderDeDano);
				acertou = true;

			}
		}

		FicaDeBoa ();

		yield return null;
	}

	protected override void FicaDeBoa ()
	{
		base.FicaDeBoa ();
		espacosAtaque.Clear ();
		spriteRenderer.color = Color.red;
	}

	public override void TomarDano (int valor)
	{
		base.TomarDano (valor);
		if (vidaAtual <= 0) {
			foreach (Area a in espacosAtaque) {
				a.Highlight (false);
			}
			espacosAtaque.Clear ();
		}
	}

	protected override void FicaAtento (Personagem a)
	{
		base.FicaAtento (a);
		Vector2 dif = new Vector2 ();
		dif.x = a.transform.position.x - transform.position.x;
		dif.y = a.transform.position.y - transform.position.y;

		if (atiraDiagonal && dif.x != 0 && dif.y != 0) {
			LinhaTiro (transform.position, (int)Mathf.Sign (dif.x), (int)Mathf.Sign (dif.y));
		} else {
			if (Mathf.Abs (dif.x) >= Mathf.Abs (dif.y)) {
				LinhaTiro (transform.position, (int)Mathf.Sign (dif.x), 0);
			} else {
				LinhaTiro (transform.position, 0, (int)Mathf.Sign (dif.y));
			}
		}


			
	}

	protected void LinhaTiro(Vector2 posInicial, int sinalx, int sinaly){
		Area a;
		for (int i = 1; i <= alcanceTiro; i++) {
			if (controleMapa.DentroMapa ((int)posInicial.x + i * sinalx, (int)posInicial.y + i * sinaly)) {
				a = matrizIlha [(int)posInicial.x + i * sinalx, (int)posInicial.y + i * sinaly];
				espacosAtaque.Add (a);
				a.Highlight (true);
			}
		}
	}




}
