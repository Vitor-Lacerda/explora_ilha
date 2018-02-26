using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : Personagem {

	public float tempoAtaque = 1f;
	protected float timerAtaque;

	protected int raioVisao;
	protected SpriteRenderer spriteRenderer;
	protected Personagem alvo;


	protected int estado = 0; //0 = PARADO / 1 = ESPERANDO PRA ATACAR / 2 = ATACANDO
	//Maquina --- > 0 --> 1 --> 2 --> 0

	protected override void Start ()
	{
		controleMapa = GameObject.FindObjectOfType<ControleMapa> ();
		base.Start ();
	}

	protected override void Update ()
	{
		base.Update ();
		if (estado == 0 && stepTimer >= stepDelay) {
			OlhaAoRedor ();
			stepTimer = 0;
		} else if (estado == 1) {
			PreparaAtaque ();
		}
	}

	protected virtual void OlhaAoRedor(){
		int x = (int)transform.position.x;
		int y = (int)transform.position.y;
		for(int linha, coluna = x-raioVisao; coluna < x+raioVisao;coluna++){
			for (linha = y - raioVisao; linha < y + raioVisao; linha++) {
				if (!(coluna == x && linha == y)) {
					if(controleMapa.DentroMapa(coluna, linha)){
						if (matrizIlha [coluna, linha].ocupante != null && !(matrizIlha[coluna,linha].ocupante is Inimigo)) {
							FicaAtento ( matrizIlha [coluna, linha].ocupante);
							return;
						}
					}
				}
			}
		}
	}

	protected virtual void PreparaAtaque(){
		timerAtaque += Time.deltaTime;
	}

	protected virtual void FicaAtento(Personagem a){
		estado = 1;
		alvo = a;
	}

	protected virtual void FicaDeBoa(){
		alvo = null;
		estado = 0;
	}

	protected virtual IEnumerator Ataque(){
		estado = 2;
		alvo.TomarDano (poderDeDano);
		FicaDeBoa ();
		yield return null;
	}
	

}
