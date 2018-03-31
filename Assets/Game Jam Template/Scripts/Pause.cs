﻿using UnityEngine;

public class Pause : MonoBehaviour {
	
	private ShowPanels _showPanels;						//Reference to the ShowPanels script used to hide and show UI panels
	private bool _isPaused;								//Boolean to check if the game is paused or not
	private StartOptions _startScript;					//Reference to the StartButton script

	void Awake()
	{
		_showPanels = GetComponent<ShowPanels> ();
		_startScript = GetComponent<StartOptions> ();
	}
	
	private void Update () {
		if (Input.GetButtonDown ("Cancel") && !_isPaused && !_startScript.inMainMenu) 
		{
			DoPause();
			_showPanels.ShowPausePanel();
		} 
		else if (Input.GetButtonDown ("Cancel") && _isPaused && !_startScript.inMainMenu) 
		{
			UnPause ();
			_showPanels.HidePausePanel ();
		}
	
	}


	public void DoPause()
	{
		_isPaused = true;
		Time.timeScale = 0;
	}


	public void UnPause()
	{
		_isPaused = false;
		Time.timeScale = 1;
	}


}
