using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onca : Inimigo {



	// Use this for initialization
	protected override void Start () {
		base.Start ();
		raioVisao = stepSize;
		spriteRenderer = this.GetComponent<SpriteRenderer> ();
		timerAtaque = 0;
		estado = 0;
	}





	protected override void PreparaAtaque(){
		base.PreparaAtaque ();
		move.x = alvo.transform.position.x - transform.position.x;
		move.y = alvo.transform.position.y - transform.position.y;
		//Alvo saiu do alcance -> fica de boa
		if (move.x > raioVisao || move.y > raioVisao) {
			FicaDeBoa ();
			return;
		}
		//Alvo ta em linha reta -> ataca na posicao
		if (move.x == 0 || move.y == 0) {
			StartCoroutine(Ataque());
			return;
		}
		//Ficou tempo demais preparando -> ataca o mais perto que puder
		if (timerAtaque >= tempoAtaque) {
			timerAtaque = 0f;
			if (Mathf.Abs (move.x) < Mathf.Abs (move.y)) {
				move.x = 0;
			} else {
				move.y = 0;
			}
			StartCoroutine(Ataque());
			return;
		}




	}

	protected override bool Step ()
	{
		move.Normalize ();
		return base.Step ();
	}

	protected override IEnumerator Ataque(){
		if (stepTimer < stepDelay) {
			yield return null;
		} else {
			estado = 2;
			for (int i = 0; i < stepSize; i++) {
				bool b = Step ();
				//Espera terminar o passo
				while (moving) {
					yield return null;
				}
				if (!b) {
					break;
				}
			}
			FicaDeBoa ();
		}
	}

	protected override void FicaDeBoa(){
		base.FicaDeBoa ();
		spriteRenderer.color = Color.white;
	}

	protected override void FicaAtento(Personagem a){
		base.FicaAtento (a);
		spriteRenderer.color = Color.red;
		stepTimer = 0;


	}

	 




}
