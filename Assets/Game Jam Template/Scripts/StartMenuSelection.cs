using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuSelection : MonoBehaviour
{
	public GameObject[] _button;
	public GameObject _menuSelector;

	public StartMenuSelection(GameObject[] button)
	{
		_button = button;
	}

	void Start ()
	{
		Debug.LogError(transform.childCount);
		_button = new GameObject[transform.childCount];
		for (var i = 0; i < transform.childCount; i++)
		{
			_button[i] = transform.GetChild(i).gameObject;
			Debug.LogError(transform.GetChild(i).gameObject.name);
		}
		_button[0].transform.GetChild(1).gameObject.SetActive(true);
	}

	void Update()
	{
		if (Input.GetAxis("Vertical")>=1)
		{
			SetSelectorAtOption();
		}
		else
		{
			
		}
	}

	private void SetSelectorAtOption()
	{
		
	}
}
