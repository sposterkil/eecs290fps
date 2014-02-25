using UnityEngine;
using System.Collections;

public class DisappearWhenUnseen : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if(!gameObject.renderer.isVisible){
			Object.Destroy(gameObject);
		}
	}
}
