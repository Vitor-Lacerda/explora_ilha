using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraJogo : MonoBehaviour {

	public Transform target;

	Vector3 offset;

	void Start(){
		offset = new Vector3 (0.5f, 0.5f, -10);
	}

	// Update is called once per frame
	void Update () {
		this.transform.position = target.position + offset;
	}
}
