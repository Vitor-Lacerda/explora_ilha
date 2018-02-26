using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuledTile : MonoBehaviour {

	[SerializeField]
	protected TileRules regras;
	[SerializeField]
	protected SpriteRenderer spriteRenderer;

	public virtual void SpritePadrao(){
		spriteRenderer.sprite = regras.RandomDefault ();
	}

	public virtual void MudaSprite(bool[] vizinhos){
		spriteRenderer.sprite = regras.SpriteVizinhos (vizinhos);
	}

}
