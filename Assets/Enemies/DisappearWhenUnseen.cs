using UnityEngine;
using System.Collections;

public class DisappearWhenUnseen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DestroyObject (this.gameObject, 10f);
	}

	// Update is called once per frame
	void Update () {
	}
}
