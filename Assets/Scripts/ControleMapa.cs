using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleMapa : MonoBehaviour {

	public FazIlha fazIlha;
	public PopulaIlha populaIlha;

	Area[,] matrizFinal;

	// Use this for initialization
	void Awake () {
		matrizFinal = fazIlha.ConstroiIlha ();
		populaIlha.ColocaDragao (matrizFinal, fazIlha.GetPontoBoss ());
		populaIlha.Popula (matrizFinal, fazIlha.GetPontoSpawn());
	}

	public Area[,] GetMatrizIlha(){
		return matrizFinal;
	}

	//Retorna se o ponto e dentro do mapa
	public bool DentroMapa(int c, int l){
		if (c < 0 || l < 0) {
			return false;
		}
		if (c > matrizFinal.GetLength(0) - 1 || l > matrizFinal.GetLength(1) - 1) {
			return false;
		}
		return true;
	}
}
