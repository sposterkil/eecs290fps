using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class MonsterAI : MonoBehaviour {
	//The Monster's CharacterController.
	CharacterController _controller;
	//The Monster's Transform.
	Transform _transform;
	//The Monster's animator
	Animator animator;
	//The Player's transform.
	Transform _player;
	//The PlayerManager of the Player, used to damage the player.
	PlayerManager _playerManager;
	//The ragdoll spawned when the Monster is killed.
	public GameObject ragdoll;

	//The sound a monster makes when aggroed.
	public AudioSource growl;
	//The sound a monster makes when attacking.
	public AudioSource hit;

	//The empty game object in front of the Monster's face, used to raycast from.
	Transform _eyes;
	//The empty game object just above the Player's head, used to raycast to.
	Transform _playerMarker;

	//The Monster's movement speed
	float speed;
	//The pull of gravity on the Monster.
	float gravity;
	//The direction the Monster is moving in.
	Vector3 moveDirection;

	//The Monster's current spot in the maze it is attempting to path to.
	Vector3 target;
	//The maximum speed a Monster can rotate at.
	float maxRotSpeed = 200.0f;
	//Used to determine rotation speed for smoothdampangle used when turning.
	float minTime = 0.1f;
	//Current velocity, used for smoothdampangle.
	float velocity;

	//The speed a Monster meanders through the maze at.
	float wanderSpeed = 4f;
	//The speed a Monster will pursue the player at.
	float chaseSpeed = 8f;
	//Whether or not the Monster is pursuing the player.
	bool chasing;

	//The Monster's health.
	int health;

	//Whether or not the Monster should try to path to a new location in the Maze.
	bool change;
	//The distance a Monster will approach its current pathing point before stopping.
	float range;
	//Range that a Monster can attack at.
	float attackRange = 9f;
	//Range of the Monster's eyesight, squared.
	float squareRange = 160f;

	//The frame delay before damage occurs when the monster is attacking. Used to sync damage with the animation.
	int attackDelay = 33;
	//The time until the next attack. Used to prevent the monsters from doing their damage every frame, rapidly draining the player's health.
	int attackTimer;
	//Damage monsters do to player Health.
	int attackDamage = 4;
	//Amount of battery drained by Monster attacks.
	int batteryDamage = 2;

	/**
	 * On initialization, sets the monster's various variables to their defaults and acquires transforms used in the Update method. 
	 */
	void Start () {
		_controller = GetComponent<CharacterController>();
		_transform = GetComponent<Transform> ();
		_player = GameObject.Find ("Player").GetComponent<Transform> ();
		_playerManager = GameObject.Find ("Player").GetComponent<PlayerManager> ();
		_eyes = _transform.Find ("Eyes");
		_playerMarker = _player.Find ("PlayerMarker");
		animator = GetComponent<Animator>();
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
		if (health < 100 && !chasing) {
			chasing = true;
			growl.Play();
		}
		if (_transform.position.y < -5) {
			DestroyImmediate (this.gameObject);
		}

		if (chasing && _transform != null) {
			if ((_player.position - _transform.position).sqrMagnitude < attackRange) {
				animator.SetBool ("dojump", true);
				if(attackTimer > attackDelay){
					_playerManager.health -= attackDamage;
					hit.Play();
					if(_playerManager.battery >= batteryDamage){
						_playerManager.battery -= batteryDamage;
					}
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
					growl.Play();
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
		if (health <= 0) {
			DestroyObject (this.gameObject);
			Debug.Log (ragdoll + " " + ragdoll.name);
			Instantiate (ragdoll, _transform.localPosition, Quaternion.identity);
		}
	}
}
