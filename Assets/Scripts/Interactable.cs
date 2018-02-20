using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

	public int custoAndar = 1;
	public int bloqueioVisao = 0;

	protected virtual void Start(){
		//Inicializar efeitos
	}

	public virtual void Interact(Area parentArea, bool entrando){
		//Tocar efeitos visuais e sonoros de interacao
	}
}
