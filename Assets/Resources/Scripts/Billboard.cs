using UnityEngine;
using System.Collections;

    

public class Billboard : MonoBehaviour {


	private Animator _animator;
	private GameObject _startMenuUi;
	private StartOptions _startOptions;

	void Start () {
		_animator = GetComponent<Animator>();
		_startMenuUi = GameObject.FindGameObjectWithTag("StartMenu");
		_startOptions = _startMenuUi.GetComponent<StartOptions>();

	}
	void Update () {
		if (CheckIfPausedAndReturn()) return;

		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		//transform.LookAt(Vector3.zero);
		transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));
		_animator.SetFloat("vertSpeed", Mathf.Abs(input.y));
		_animator.SetFloat("speed", Mathf.Abs(input.x));

		if (input.x > 0)
		{
			transform.eulerAngles = new Vector3(0, 180, 0);
			//transform.rotation = Quaternion.identity;
		}

		if (input.x < 0)
		{
			transform.eulerAngles = new Vector3(0, 0, 0);
		}

		if (Input.GetAxisRaw("Vertical") > 0)
		{
			transform.eulerAngles = new Vector3(0, 180, 0);
		}
		if (Input.GetAxisRaw("Vertical") < 0)
		{
			transform.eulerAngles = new Vector3(0, 0, 0);
		}

	}

	private bool CheckIfPausedAndReturn()
	{
		if (_startOptions)
		{
			if (_startOptions.PauseScript._isPaused)
				return true;
		}
		return false;
	}
}
