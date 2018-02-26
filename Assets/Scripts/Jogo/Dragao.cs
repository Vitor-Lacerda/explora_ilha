using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragao : Inimigo {

	public int alcanceFogo = 4;
	public int larguraInicialFogo = 2;
	public int danoFogo = 2;

	public int larguraExtraAsa = 1;
	public int alcanceAsas = 2;

	public int larguraExtraRabo = 1;
	public int alcanceRabo = 3;

	protected List<Area> espacosAtaque;
	protected int danoDoAtaque;


	protected override void Start ()
	{
		base.Start ();
		espacosAtaque = new List<Area> ();
		raioVisao = alcanceFogo;
		spriteRenderer = this.GetComponent<SpriteRenderer> ();
		danoDoAtaque = poderDeDano;
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
		EscolheAtaque ();
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
			if (espacosAtaque [i].ocupante != null) {
				espacosAtaque [i].ocupante.TomarDano (danoDoAtaque);
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
		spriteRenderer.color = Color.white;
	}

	protected void EscolheAtaque(){
		
		Config.Point pontoAlvo = new Config.Point((int)alvo.transform.position.x, (int)alvo.transform.position.y);
		Config.Point pontoBoss = new Config.Point ((int)transform.position.x, (int)transform.position.y);

		if (pontoAlvo.x == pontoBoss.x || pontoAlvo.x == pontoBoss.x + 1) {
			if (pontoAlvo.y < pontoBoss.y) {
				Fogo ();
			} else if (pontoAlvo.y > pontoBoss.y + 1) {
				Rabo ();
			}
		} 
		else {
			if (pontoAlvo.y == pontoBoss.y || pontoAlvo.y == pontoBoss.y) {
				Asas ();
			}
			else if (pontoAlvo.y < pontoBoss.y) {
				if (pontoBoss.y - pontoAlvo.y <= larguraExtraAsa) {
					Asas ();
				} else {
					Fogo ();
				}

			} else if (pontoAlvo.y > pontoBoss.y + 1) {
				if (pontoAlvo.y - (pontoAlvo.y + 1) <= larguraExtraAsa) {
					Asas ();
				} else {
					Rabo ();
				}
			}
		}

	}

	protected void MarcaEspaco(int x, int y){
		if (controleMapa.DentroMapa (x, y)) {
			Area a = matrizIlha [x, y];
			MarcaEspaco (a);
		}
	}

	protected void MarcaEspaco(Area a){
		espacosAtaque.Add (a);
		a.Highlight (true);
	}

	protected void Fogo(){
		danoDoAtaque = danoFogo;
		int xInicial = (int)transform.position.x;
		int yInicial = (int)transform.position.y;
		for (int i = 0; i < alcanceFogo; i++) {
			for (int c = xInicial - i; c < xInicial + larguraInicialFogo + i; c++) {
				MarcaEspaco (c, yInicial - i - 1);
			}
		}
	}

	protected void Asas(){
		danoDoAtaque = poderDeDano;
		for (int y = -larguraExtraAsa; y <= 1 + larguraExtraAsa; y++) {
			for (int x = 1; x <= alcanceAsas; x++) {
				MarcaEspaco ((int)transform.position.x - x, (int)transform.position.y + y);
				MarcaEspaco ((int)transform.position.x + 1 + x,(int)transform.position.y + y);
			}
		}

	}

	protected void Rabo(){
		danoDoAtaque = poderDeDano;
		for (int x = -larguraExtraRabo; x <= 1 + larguraExtraRabo; x++) {
			for(int y = 1; y<= alcanceRabo;y++){
				MarcaEspaco ((int)transform.position.x + x, (int)transform.position.y + 1 + y);
			}
		}
	}

}
