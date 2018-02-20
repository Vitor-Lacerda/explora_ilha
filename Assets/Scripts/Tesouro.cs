using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tesouro : Interactable {

	public override void Interact (Area parentArea, bool entrando)
	{
		if (entrando) {
			base.Interact (parentArea, entrando);
			int valor = Config.TESOURO_BASE  + (Config.SOMA_TESOURO * (parentArea.dificuldade-1));
			GameController.instance.GanhaOuro (valor);
			gameObject.SetActive (false);
			parentArea.RemoveObstaculo (this);

		}

	}
}
