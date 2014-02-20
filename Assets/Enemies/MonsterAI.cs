﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class MonsterAI : MonoBehaviour {
	CharacterController _controller;
	Transform _transform;
	Animator animator;
	Transform _player;
	PlayerManager _playerManager;

	Transform _eyes;
	Transform _playerMarker;

	float speed;
	float gravity;
	Vector3 moveDirection;

	Vector3 target;
	float maxRotSpeed = 200.0f;
	float minTime = 0.1f;
	float velocity;

	float wanderSpeed = 4f;
	float chaseSpeed = 7f;
	bool chasing;
	
	int health;

	bool change;
	float range;
	float attackRange = 9f;
	float squareRange = 160f;

	int attackDelay = 33;
	int attackTimer;
	int attackDamage = 2;

	// Use this for initialization
	void Start () {
		_controller = GetComponent<CharacterController>();
		_transform = GetComponent<Transform> ();
		animator = GetComponent<Animator>();
		_player = GameObject.Find ("Player").GetComponent<Transform> ();
		_playerManager = GameObject.Find ("Player").GetComponent<PlayerManager> ();
		_eyes = _transform.Find ("Eyes");
		_playerMarker = _player.Find ("PlayerMarker");
		chasing = false;
		speed = wanderSpeed;
		attackTimer = attackDelay - 11;
		gravity = 20f;
		animator.SetFloat ("speed", speed);
		animator.SetBool ("isgrounded", true);
		range = 2f;
		target = GetTarget ();
		InvokeRepeating ("NewTarget", 0.01f, 1.0f);
		health = 100;
	}

	// Update is called once per frame
	void Update () {
		if (_transform.position.y < -5) {
			DestroyImmediate (this.gameObject);
		}
		
		if (chasing) {
			if ((_player.position - _transform.position).sqrMagnitude < attackRange) {
				animator.SetBool ("dojump", true);
				if(attackTimer > attackDelay){
					_playerManager.health -= attackDamage;
					attackTimer = 0;
				}
				else{
					attackTimer++;
				}
			}
			else{
				animator.SetBool ("dojump", false);
				if(Vector3.Distance (_transform.position, _player.position) > 3f){
					moveDirection = _transform.forward;
					moveDirection *= speed;
					moveDirection.y -= gravity;
					_controller.Move (moveDirection * Time.deltaTime);
					var newRotation = Quaternion.LookRotation (_player.position - _transform.position).eulerAngles;
					var angles = _transform.rotation.eulerAngles;
					_transform.rotation = Quaternion.Euler (angles.x, Mathf.SmoothDampAngle (angles.y, newRotation.y, ref velocity, minTime, maxRotSpeed), angles.z);
					animator.SetFloat ("speed", speed);
				}
				else{
					animator.SetFloat ("speed", 0f);
				}
			}
		}
		else {
			if((_player.position - _transform.position).sqrMagnitude < squareRange && (Vector3.Dot ((_player.position - _transform.position).normalized, _transform.forward) > 0)){
				if(!Physics.Linecast (_eyes.position, _playerMarker.position)){
					chasing = true;
					speed = chaseSpeed;
				}
			}
			if (change) {
				target = GetTarget ();
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
			else {
				animator.SetFloat ("speed", 0f);
			}
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
	
	public void damage(int damage) {
		health -= damage;
		if (health <= 0)
			Destroy(transform.gameObject);
	}
}
