using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	//singleton
	public static GameController instance;



	//Managers
	public ControleMapa controleMapa;
	public UIController uiController;
	


	int ouroAtual;


	void Awake(){
		if (instance == null) {
			instance = this;
		}
	}

	// Use this for initialization
	void Start () {
		ouroAtual = 0;
	}

	public void GanhaOuro(int valor){
		ouroAtual += valor;
		uiController.ValorOuro (ouroAtual);
	}

	public bool GastaOuro(int valor){
		if (valor > ouroAtual) {
			return false;
		}
		ouroAtual -= valor;
		uiController.ValorOuro (ouroAtual);
		return true;
	}
	

}
