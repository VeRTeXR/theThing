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

	public void Engage(GameObject Player, float speed)
	{
		ResetAnimationState();
		_animator.SetFloat("Engage", speed);
		var EngageDirection = Player.transform.position.x - transform.position.x;

		if (EngageDirection > 0)
			GetComponent<SpriteRenderer>().flipX = true;
		else
			GetComponent<SpriteRenderer>().flipX = false;
	}

	public void Disengage()
	{
		ResetAnimationState();
		_animator.SetTrigger("Disengage");
	} 
	
	public void Melee()
	{
		_animator.SetTrigger("Melee");
	}

	public void ResetAnimationState()
	{
		_animator.SetFloat("idleTime", 0);
		_animator.SetFloat("Engage",0);
		_animator.ResetTrigger("Disengage");
		_animator.ResetTrigger("Melee");
	}
}
