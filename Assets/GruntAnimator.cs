using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntAnimator : MonoBehaviour {
	private Animator _animator;
	private float _idleTimer;
	private float _idleLimit = 2f;

	private bool _EngageRight;

	// Use this for initialization
	void Start ()
	{
		_animator = GetComponent<Animator>();
	}
	
	void Update () 
	{
		if (_animator.GetCurrentAnimatorStateInfo(0).IsName("soilderIdle"))
		{
			_idleTimer += Time.fixedDeltaTime;
			_animator.SetFloat("idleTime", _idleTimer);
		}

		if (_idleTimer > _idleLimit)
		{
			_idleTimer = 0;
		}
	}

	public void Engage(GameObject Player)
	{
		_animator.SetTrigger("Engage");
		var EngageDirection = Player.transform.position.x - transform.position.x;

		if (EngageDirection > 0)
		{ 
//			_turretBarrel.transform.eulerAngles = new Vector3(0, 0, -90);
			_EngageRight = true;
		}
		else
		{
//			_turretBarrel.transform.eulerAngles = new Vector3(0, 0, 90);
			_EngageRight = false;
		}
		if (_EngageRight)
		{
			GetComponent<SpriteRenderer>().flipX = true;
		}
		else
		{
			GetComponent<SpriteRenderer>().flipX = false;
		}
	}

	public void Melee()
	{
		_animator.SetTrigger("Melee");
	}

	public void ResetAnimationState()
	{
	}
}
