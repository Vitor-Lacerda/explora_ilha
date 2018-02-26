using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public Text textOuro;

	public Image[] coracoes;

	public void ValorOuro(int valor){
		textOuro.text = valor.ToString ("D3");
	}

	public void MostraCoracoes(int count){
		if (count > coracoes.Length) {
			count = coracoes.Length;
		}

		for (int i = 0; i < coracoes.Length; i++) {
			coracoes [i].gameObject.SetActive (i < count);
		}

	}

}
