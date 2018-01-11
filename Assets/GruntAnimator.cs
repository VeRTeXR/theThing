using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntAnimator : MonoBehaviour {
	private Animator _animator;
	private float _idleTimer; 
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
	}

	void Engage()
	{
		_animator.SetTrigger("Engage");
	}
}
