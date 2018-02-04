using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntAnimator : MonoBehaviour {
	private Animator _animator;
	private float _idleTimer;
	private float _idleLimit = 2f;
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

	public void Engage()
	{
		_animator.SetTrigger("Engage");
	}

	public void Melee()
	{
		_animator.SetTrigger("Melee");
	}

	public void ResetAnimationState()
	{
	}
}
