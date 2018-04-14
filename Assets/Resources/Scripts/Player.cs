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
	private float restartLevelDelay = 20f;
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
	private float _boosterCost;
	private float _rechargeRate;

	private float _gravity;
	public float JumpVelocity;
	public Vector3 Velocity;
	private float _velocityXSmoothing;
	private float _airTime = 0f;
	public GameObject Explosion;

	private bool _isHoldingWeapon;
	private float _playerWeaponPickupRange = 2.0f;
	public Transform Hold;
	RaycastHit2D _hit;
	private float _playerWeaponThrowForce;
	private LayerMask _pickableObjectLayerMask;

	Controller2D _controller2D;
	Rigidbody2D _rigidbody;
	Animator _animator;

	IEnumerator Attack () 
	{
		yield return new WaitForSeconds (0.5f);
		_controller2D.Shot(transform);
		StopCoroutine("Attack");
	}

	IEnumerator FreezeFrame(float pauseDuration)
	{
		float pauseEndTime = Time.realtimeSinceStartup + pauseDuration;
		while (Time.realtimeSinceStartup < pauseEndTime)
		{
			Time.timeScale = 0f;
			yield return 0;
		}
		Time.timeScale = 1;	
	}

	IEnumerator IsAttacked()
	{
		gameObject.GetComponentInChildren<Renderer>().material.SetFloat("_FlashAmount", 1);
		yield return new WaitForSeconds(0.025f);
		gameObject.GetComponentInChildren<Renderer>().material.SetFloat("_FlashAmount", 0);
	}
	
	void Start() {
		_playerCurrentEnegy = _playerMaxEnegy;
		_controller2D = GetComponent<Controller2D> ();
		_rigidbody = GetComponent<Rigidbody2D> ();
		_animator = GetComponent<Animator> ();
		_playerHp = Manager.instance.HP;
        _score = Manager.instance.score;
		_gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		JumpVelocity = Mathf.Abs(_gravity) * timeToJumpApex;
		print ("Gravity: " + _gravity + "  Jump Velocity: " + JumpVelocity);
	}

	void CollisionCheck() 
	{
		if (_controller2D.collisions.above || _controller2D.collisions.below) 
			Velocity.y = 0;
		if (OnBoostBlock) 
			Velocity.y += Velocity.y + BoostSpeed;
		if(!_controller2D.collisions.below)
		{
			_airTime += Time.deltaTime;
			if(_controller2D.collisions.below) 
				_airTime = 0;
		}
	}


	void PickupItem()
	{
		if (!_isHoldingWeapon)
		{
			Physics2D.queriesStartInColliders = false;
			_hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, _playerWeaponPickupRange);
			if (_hit.collider != null && _hit.collider.CompareTag("Gun"))
			{
				_isHoldingWeapon = true;
			}
		}
		else if (Physics2D.OverlapPoint(Hold.position))
		{
			_isHoldingWeapon = false;
			if (_hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
			{
				_hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity =
					new Vector2(transform.localScale.x, 1) * _playerWeaponThrowForce;
			}
		}
	}

	void Jump () 
	{
		if (_airTime > 0 && _playerCurrentEnegy > _boosterCost)
		{
			_playerCurrentEnegy = _playerCurrentEnegy - _boosterCost;
			Velocity.y += JumpVelocity / 2;
		}
		if (_controller2D.collisions.below)
		{
			Velocity.y += JumpVelocity;
		}
		//maybe change to hold?????
		//hold for hover, should be time out for dropping
	}

	void Update() 
	{
		CollisionCheck ();

		if (_isHoldingWeapon)
		{
			_hit.collider.gameObject.transform.position = Hold.position;
		}

		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

		if (!_controller2D.collisions.below)
		{
			_airTime += Time.deltaTime;
			if (_controller2D.collisions.below)
			{
				_airTime = 0;
			}
		}

		if(_playerCurrentEnegy < _playerMaxEnegy) 
		{
			_playerCurrentEnegy += _rechargeRate * Time.deltaTime;
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
					_controller2D.Dash(input, _dashForce);
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

		if(_playerHp <= 0) {
				Dead ();
		}

		float targetVelocityX = input.x * moveSpeed;
		Velocity.x = Mathf.SmoothDamp (Velocity.x, targetVelocityX, ref _velocityXSmoothing, (_controller2D.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		Velocity.y += _gravity * Time.deltaTime;
		_controller2D.Move (Velocity * Time.deltaTime);
		OnBoostBlock = false;
	}

	void Dead () 
	{
		
		var Explosion = Resources.Load("Prefabs/giantExplosion");
		var exp = (GameObject) Instantiate(Explosion);
		exp.transform.localPosition = gameObject.transform.localPosition + new Vector3(0 , 2f,0);
		Invoke("Restart", restartLevelDelay);
		gameObject.SetActive(false);
	}

	void Damaged(float dmg)
	{
		_playerHp -= dmg;
	}
	
	void OnCollisionEnter2D (Collision2D c)
 	{
		 if (c.gameObject.CompareTag("Enemy"))
		 {
			 StartCoroutine(FreezeFrame(0.15f));
			 StartCoroutine("IsAttacked");
//			 _animator.SetTrigger();
			 _playerHp -= 1;
		 }
 		
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
			 StartCoroutine(FreezeFrame(0.15f));
			 StartCoroutine("IsAttacked");
			 SendMessage("Damaged",c.gameObject.GetComponent<Bullet>().Dmg);
		 }
		 
		if (c.gameObject.CompareTag("BoostBlock")) {
				OnBoostBlock = true;
		}	
		 
//		 if (c.gameObject.CompareTag("Obstacle") && (Controller2D.collisions.left || Controller2D.collisions.right))
//		 {
//			 //TODO::On wall collision, decrease velocity i.e. lerp the shit	
//		 }
 	}

	private void Restart () {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

}
