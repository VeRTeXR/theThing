using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuSelection : MonoBehaviour
{
	public GameObject[] _button;
	public GameObject _menuSelector;
	private bool _inputAllowed;

	public StartMenuSelection(GameObject[] button)
	{
		_button = button;
	}

	void Start ()
	{
		_button = new GameObject[transform.childCount];
		for (var i = 0; i < transform.childCount; i++)
		{
			_button[i] = transform.GetChild(i).gameObject;
		}
		_button[0].transform.GetChild(1).gameObject.SetActive(true);
		_inputAllowed = true;
	}

	void Update()
	{
		if (_inputAllowed)
		{
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
			{
				_inputAllowed = false;
				StartCoroutine(SwitchActiveButtonPosition());
			}
		}

		if (Input.GetKey(KeyCode.Return))
		{
			if (ActiveButton() == 0)
			{
				Manager.instance.StartMenu.GetComponent<StartOptions>().StartButtonClicked();
			}
			if (ActiveButton() == 1)
			{
				Manager.instance.StartMenu.GetComponent<QuitApplication>().Quit();
			}
		}
	}

	private int ActiveButton()
	{
		if (_button[0].transform.GetChild(1).gameObject.activeSelf)
		{
			return 0;
		}
		if (_button[1].transform.GetChild(1).gameObject.activeSelf)
			return 1;
		return 1;
	}
	
	private IEnumerator SwitchActiveButtonPosition()
	{
		if (_button[0].transform.GetChild(1).gameObject.activeSelf)
		{
			_button[0].transform.GetChild(1).gameObject.SetActive(false);
			_button[1].transform.GetChild(1).gameObject.SetActive(true);
			yield return new WaitForSecondsRealtime(0.5f);
			_inputAllowed = true;
		}
		else
		{
			_button[1].transform.GetChild(1).gameObject.SetActive(false);
			_button[0].transform.GetChild(1).gameObject.SetActive(true);
			yield return new WaitForSecondsRealtime(0.5f);
			_inputAllowed = true;
		}
	}
}
