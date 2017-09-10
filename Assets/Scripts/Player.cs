using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {

	private float jumpHeight = 10;
	private float timeToJumpApex = 0.4f;
	float accelerationTimeAirborne = 0.2f; 		
	float accelerationTimeGrounded = 0.1f; 	
	private float moveSpeed = 15;
	private float restartLevelDelay = 0.2f;
	public bool OnBoostBlock;
	public float BoostSpeed;
	private float _buttoncooldown = 0.5f;
	private int _buttoncount = 0;
	private int _dashcount = 0;
	private float _dashForce = 10;
	private float _bulletFeedbackForce = 50.0f; // for bullet feedback
	private float _playerHp;
	private int _score;
	private float _playerMaxEnegy;
	private float _playerCurrentEnegy;
	private float boosterCost;
	private float rechargeRate;

	private float gravity;
	public float jumpVelocity;
	public Vector3 velocity;
	private float _velocityXSmoothing;
	private float airTime = 0f;

	private bool _isHoldingWeapon;
	private float _playerWeaponPickupRange = 2.0f;
	public Transform Hold;
	RaycastHit2D _hit;
	private float _playerWeaponThrowForce;
	private LayerMask _pickableObjectLayerMask;

	Controller2D _controller;
	Rigidbody2D _rigidbody;
	Animator _animator;

	IEnumerator Attack () 
	{
		yield return new WaitForSeconds (0.5f);
		_controller.Shot(transform);
		StopCoroutine("Attack");
	}
	
	void Start() {
		_playerCurrentEnegy = _playerMaxEnegy;
		_controller = GetComponent<Controller2D> ();
		_rigidbody = GetComponent<Rigidbody2D> ();
		_animator = GetComponent<Animator> ();
		_playerHp = Manager.instance.HP;
        _score = Manager.instance.score;
		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		print ("Gravity: " + gravity + "  Jump Velocity: " + jumpVelocity);
	}

	void CollisionCheck() 
	{
		if (_controller.collisions.above || _controller.collisions.below) 
			velocity.y = 0;
		if (OnBoostBlock) 
			velocity.y += velocity.y + BoostSpeed;
		if(!_controller.collisions.below)
		{
			airTime += Time.deltaTime;
			if(_controller.collisions.below) 
				airTime = 0;
		}
	}


	void PickupItem()
	{
		// Interact
		/*
			Bring up the Dialogue, Pick up guns, Trigger etc...
			*/
		if(!_isHoldingWeapon) 
		{
			Physics2D.queriesStartInColliders = false;
			_hit = Physics2D.Raycast(transform.position, Vector2.right*transform.localScale.x, _playerWeaponPickupRange);
			if(_hit.collider!= null && _hit.collider.CompareTag ("Gun")) {
				_isHoldingWeapon = true;
			}
		}
		else if(Physics2D.OverlapPoint(Hold.position)) 
		{
			_isHoldingWeapon = false;
			if (_hit.collider.gameObject.GetComponent<Rigidbody2D>() != null){
				_hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 (transform.localScale.x,1)*_playerWeaponThrowForce;
			}
		}
	}

	void Jump () 
	{
		if (airTime > 0 && _playerCurrentEnegy > boosterCost) {
			_playerCurrentEnegy = _playerCurrentEnegy - boosterCost;
			velocity.y += jumpVelocity / 2; // kinda hovers now if u repeatedly tap on it
		} 
		if (_controller.collisions.below) {
			velocity.y += jumpVelocity;
		}
		//maybe change to hold?????
		//hold for hover, should be time out for dropping
	}

	void Update() 
	{
		CollisionCheck ();

		if(_isHoldingWeapon) {
			_hit.collider.gameObject.transform.position = Hold.position;
		}

		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

		//Control Code
		///
		////
		/////
		//////

		if (!_controller.collisions.below)
		{
			airTime += Time.deltaTime;
			if (_controller.collisions.below)
			{
				airTime = 0;
			}
		}

		if(_playerCurrentEnegy < _playerMaxEnegy) 
		{
			_playerCurrentEnegy += rechargeRate * Time.deltaTime;
		}

		if (Input.GetKeyDown(KeyCode.E)){
			PickupItem ();
		}

		if (Input.GetKeyDown(KeyCode.Q)) {
			//TODO:: Throw the shit
		}

		if (Input.GetKeyDown(KeyCode.Tab)) {
			//TODO:: Inventory shit
		}

		if(Input.GetKeyDown(KeyCode.LeftControl)) {
			//TODO:: hiding & disable player attack shit
		}

		if (Input.GetKeyDown(KeyCode.LeftShift)) 
		{
			if(_buttoncooldown > 0 && _buttoncount == 1) {
					_dashcount++;
					_controller.Dash(input, _dashForce);
			}
			else {
				_buttoncooldown = 0.5f;
				_buttoncount += 1;
			}
		}

		if	(_buttoncooldown > 0) {
			_buttoncooldown -= 1 * Time.deltaTime;
		}
			else {
				_buttoncount = 0;
			}
	

		if (Input.GetKeyDown (KeyCode.Space)) {
			Jump();
		}

		if (Input.GetKeyDown (KeyCode.Z)) {
			Restart();
		}
		
		/////
		////
		///
		//


		if(_playerHp <= 0) {
				Dead ();
		}

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref _velocityXSmoothing, (_controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
		_controller.Move (velocity * Time.deltaTime);
		OnBoostBlock = false;
	}

	void Dead () 
	{
		Invoke("Restart", restartLevelDelay);
	}

	void OnCollisionEnter2D (Collision2D c)
 	{
 		
 		if (c.gameObject.CompareTag("Exit"))
		 {
			Manager.instance.HP = _playerHp;
			Manager.instance.score = _score;
			//Manager.instance.gun = gun; passing onhold gun to next level also
			 Invoke("Restart", restartLevelDelay);
       		// on scene transition sync values to manager {
        	// hp sync | (holding obj.) gun prefab | level state??? | story stage?? 	
        	//}
		}

		 if (c.gameObject.CompareTag("EnemyBullet"))
		 {
			 _playerHp -= 1;
		 }
		 
		if (c.gameObject.CompareTag("BoostBlock")) {
				OnBoostBlock = true;
				//velocity.y = velocity.y + _boostSpeed; 
		}	
 	}

	private void Restart () {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

}
