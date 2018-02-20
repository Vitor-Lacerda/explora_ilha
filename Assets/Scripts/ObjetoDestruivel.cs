using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoDestruivel : Interactable {

	public int ataques = 1;
	int contAtaques;

	protected override void Start ()
	{
		base.Start ();
		contAtaques = 0;
	}

	public override void Interact (Area parentArea, bool entrando)
	{
		base.Interact (parentArea, entrando);
		contAtaques++;
		if (contAtaques >= ataques) {
			parentArea.custoAndar = 0;
			parentArea.RemoveObstaculo (this);
			gameObject.SetActive (false);
		}


	}


}
