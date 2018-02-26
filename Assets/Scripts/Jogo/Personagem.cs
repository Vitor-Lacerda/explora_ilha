using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personagem : MonoBehaviour {

	public ControleMapa controleMapa;

	public int stepSize = 1;
	public float stepDelay = 0.1f;
	public float stepTotalTime = 0.1f;
	protected float stepTimer;
	public int poderPraAndar = 1;
	public int vidaInicial = 3;
	protected int vidaAtual;

	public int poderDeDano = 1;

	protected Vector2 move;

	protected Area[,] matrizIlha;
	protected Area areaAtual;

	protected bool moving;



	// Use this for initialization
	protected virtual void Start () {
		vidaAtual = vidaInicial;
		stepTimer = 0f;
		move = new Vector2(0,0);
		matrizIlha = controleMapa.GetMatrizIlha ();
		areaAtual = matrizIlha [(int)transform.position.x, (int)transform.position.y];
		moving = false;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		if (!moving) {
			stepTimer += Time.deltaTime;
		}
	}

	protected virtual bool Step(){
		if (moving) {
			return false;
		}

		stepTimer = 0;

		bool entrou = false;

		Area proximaArea = matrizIlha [(int)transform.position.x + (int)move.x, (int)transform.position.y + (int)move.y];
		if (move.x == 0 && move.y == 0) {
			return false; //Nem faz nada se move tiver zerado;
		}
		if (move.x != 0 && move.y != 0) {
			return false; //So pode mover cardinalmente
		}

		if (proximaArea.ocupante == null) {
			if (proximaArea.custoAndar < poderPraAndar) {
				Vector3 temp = transform.position;
				temp += (Vector3)move;
				entrou = true;
				StartCoroutine (StepRoutine (temp, proximaArea));
			}
		} else {
			//Interagir com o ocupante.
			proximaArea.ocupante.TomarDano(poderDeDano);
		}

		return entrou;
	}

	protected virtual IEnumerator StepRoutine(Vector3 targetPos, Area proximaArea){
		float currentLerpTime = 0f;
		moving = true;
		while (currentLerpTime <= stepTotalTime) {
			currentLerpTime += Time.deltaTime;
			transform.position = Vector3.Lerp (transform.position, targetPos, currentLerpTime / stepTotalTime);
			yield return null;
		}
		areaAtual.SairDaArea (this);
		proximaArea.EntrarNaArea (this);
		areaAtual = proximaArea;

		moving = false;
		yield return null;
	}

	public virtual void TomarDano(int valor){
		vidaAtual -= valor;
		if (vidaAtual <= 0) {
			gameObject.SetActive (false);
			areaAtual.SairDaArea (this);
		}
	}
}
