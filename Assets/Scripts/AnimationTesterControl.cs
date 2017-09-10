using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]
public class AnimationTesterControl : MonoBehaviour {

	private float Gravity; 
	public float JumpHeight = 10f;
	public float TimeToJumpApex = 0.4f;
	public float MoveSpeed = 15f;
	public float accelerationTimeAirborne = 0.2f; 
	public float accelerationTimeGrounded = 0.1f; 

	private float JumpVelocity;
	public Vector3 Velocity;
	private float _velocityXSmoothing;

	private float airTime;
	private bool OnBoostBlock;


	Controller2D _controller;
	Rigidbody2D _rigidbody;
	Animator _animator;

	void Start () {
		gameObject.tag = "Player";
		gameObject.layer = LayerMask.NameToLayer("Player");
		_controller = GetComponent<Controller2D> ();
		_animator = gameObject.GetComponent<Animator>();
		Gravity = -(2 * JumpHeight) / Mathf.Pow (TimeToJumpApex, 2);
		JumpVelocity = Mathf.Abs(Gravity) * TimeToJumpApex;
		SetupRigidbody ();
		if (_animator != null)
			CheckForAnimationControllerAndShit ();
		else
			print ("no animator");
	}

	void CheckForAnimationControllerAndShit()
	{
		if (_animator.runtimeAnimatorController == null)
			print ("Missing Animation Controller: Create one and drag the shit there");
	}

	void SetupRigidbody(){
		_rigidbody = gameObject.AddComponent<Rigidbody2D>();
		_rigidbody.gravityScale = 0.1f;
	}

	void ProcessMovement ()
	{
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			if (airTime > 0) 
				Velocity.y = JumpVelocity / 2; 
			if (_controller.collisions.below)
				Velocity.y = JumpVelocity;
		}
	
		float targetVelocityX = input.x * MoveSpeed;
		Velocity.x = Mathf.SmoothDamp (Velocity.x, targetVelocityX, ref _velocityXSmoothing,
			(_controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		Velocity.y += Gravity * Time.deltaTime;
		OnBoostBlock = false;
		_controller.Move (Velocity * Time.deltaTime);


	}

	void CollisionCheck ()
	{
		if (_controller.collisions.above || _controller.collisions.below) 
			Velocity.y = 0;
		
		if (!_controller.collisions.below) 
		{
			airTime += Time.deltaTime;
			if (_controller.collisions.below) 
			{
				airTime = 0;
			}
		}
	}

	void Update ()
	{
		CollisionCheck ();
		ProcessMovement ();
	}
}