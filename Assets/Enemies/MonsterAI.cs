using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class MonsterAI : MonoBehaviour {
	CharacterController _controller;
	Transform _transform;
	Animator animator;

	float speed;
	float gravity;
	Vector3 moveDirection;

	Vector3 target;
	float maxRotSpeed = 200.0f;
	float minTime = 0.1f;
	float velocity;

	bool change;
	float range;

	// Use this for initialization
	void Start () {
		_controller = GetComponent<CharacterController>();
		_transform = GetComponent<Transform> ();
		animator = GetComponent<Animator>();
		speed = 4f;
		gravity = 20f;
		animator.SetFloat ("speed", speed);
		range = 2f;
		target = GetTarget ();
		InvokeRepeating ("NewTarget", 0.01f, 1.0f);
	}

	// Update is called once per frame
	void Update () {
		if (_transform.position.y < -5) {
			DestroyImmediate(this.gameObject);
		}
		if (change) {
			target = GetTarget();
		}
		if (Vector3.Distance (_transform.position, target) > range) {
			moveDirection = _transform.forward;
			moveDirection *= speed;
			moveDirection.y -= gravity;
			_controller.Move (moveDirection * Time.deltaTime);
			var newRotation = Quaternion.LookRotation (target - _transform.position).eulerAngles;
			var angles = _transform.rotation.eulerAngles;
			_transform.rotation = Quaternion.Euler (angles.x, Mathf.SmoothDampAngle (angles.y, newRotation.y, ref velocity, minTime, maxRotSpeed), angles.z);
			animator.SetFloat ("speed", speed);
		}
		else{
			animator.SetFloat("speed", 0f);
		}
	}

	Vector3 GetTarget(){
		return new Vector3 (Random.Range (0, GridCreator.dimensions), 0, Random.Range (0, GridCreator.dimensions));
	}

	void NewTarget(){
		int choice = Random.Range (0, 3);
		switch (choice) {
		case 0:
			change = true;
			break;
		case 1:
			change = false;
			break;
		case 3:
			target = _transform.position;
			break;
		}
	}
}
