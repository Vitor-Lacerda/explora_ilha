using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControleJogador : Personagem {


	public UIController controleUI;
	public int raioDeVisao = 5;

	protected int vidaMaxima;


	protected Vector2 moveAnterior;



	// Use this for initialization
	protected override void Start () {
		base.Start ();
		vidaMaxima = vidaInicial;
		moveAnterior = new Vector2(0,0);
		controleUI.MostraCoracoes (vidaAtual);
		AtualizaVisao ();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		move.x = Input.GetAxisRaw ("Horizontal");
		move.y = Input.GetAxisRaw ("Vertical");

		if ((move != Vector2.zero && moveAnterior == Vector2.zero && !moving)|| stepTimer >= stepDelay) {
			Step ();

		}

		moveAnterior = move;
	}

	protected override bool Step(){
		

		Area proximaArea = matrizIlha [(int)transform.position.x + (int)move.x, (int)transform.position.y + (int)move.y];
		bool entrou = base.Step ();

		proximaArea.Interact (entrou);
		AtualizaVisao ();

		return entrou;
	}

	public override void TomarDano(int valor){
		base.TomarDano (valor);
		controleUI.MostraCoracoes (vidaAtual);
	}

	public virtual void AtualizaVisao(){
		Config.Point pos = new Config.Point((int)transform.position.x, (int)transform.position.y);
		int startC = pos.x - raioDeVisao;
		int startL = pos.y - raioDeVisao;
		int fimC = pos.x + raioDeVisao;
		int fimL = pos.y + raioDeVisao;

		int x = startC;
		int y = startL;

		for (x = startC; x <= fimC; x++) {
			for (y = startL; y <= fimL; y++) {
				if (controleMapa.DentroMapa (x, y)) {
					int raio = raioDeVisao - matrizIlha [x, y].bloqueioVisao;
					if (Mathf.Pow((x - pos.x),2) + Mathf.Pow((y - pos.y),2) <= raio * raio) {
						matrizIlha [x, y].Mostra ();
					}
				}
			}

		}
	}
}
