using System.Collections;
using UnityEngine;


public class StartOptions : MonoBehaviour {
	public bool inMainMenu = true;
	public Animator AnimColorFade;
	public Animator AnimMenuAlpha;
	public AnimationClip FadeColorAnimationClip;
	public AnimationClip FadeAlphaAnimationClip;
	public Pause PauseScript;
	public bool ChangeMusicOnStart;

	private PlayMusic _playMusic;
	private float fastFadeIn = .01f;
	private ShowPanels _showPanels;


	public void Awake()
	{
		InitializeMenuAndPauseGameplay();
	}

	private void InitializeMenuAndPauseGameplay()
	{
		_showPanels = GetComponent<ShowPanels>();
		PauseScript = GetComponent<Pause>();
		_playMusic = GetComponent<PlayMusic>();
		
		SetUnscaleUiAnimatorUpdateMode();

		PauseScript.DoPause();
	}

	private void SetUnscaleUiAnimatorUpdateMode()
	{
		AnimMenuAlpha.updateMode = AnimatorUpdateMode.UnscaledTime;
		AnimColorFade.updateMode = AnimatorUpdateMode.UnscaledTime;
	}


	public void StartButtonClicked()
	{
		FadeOutMusicOnStartIfAppropriate();
		StartGameInScene();
	}

	private void FadeOutMusicOnStartIfAppropriate()
	{
		if (ChangeMusicOnStart)
		{
			_playMusic.FadeDown(FadeColorAnimationClip.length);
		}
	}


	public IEnumerator HideDelayed()
	{
		yield return new WaitForSecondsRealtime(FadeAlphaAnimationClip.length);
		_showPanels.HideMenu();
	}

	public void StartGameInScene()
	{
		inMainMenu = false;
		ChangeMusicOnStartIfAppropriate();
		FadeAndDisableMenuPanel();
		StartCoroutine("UnpauseGameAfterMenuFaded");

	}

	private void FadeAndDisableMenuPanel()
	{
		AnimMenuAlpha.SetTrigger("fade");
		StartCoroutine("HideDelayed");
	}

	private void ChangeMusicOnStartIfAppropriate()
	{
		if (ChangeMusicOnStart)
			Invoke("PlayNewMusic", FadeAlphaAnimationClip.length);
	}

	public IEnumerator UnpauseGameAfterMenuFaded()
	{
		yield return new WaitUntil(() => !_showPanels.MenuPanel.gameObject.activeSelf);
		PauseScript.UnPause();
	}

	public void PlayNewMusic()
	{
		_playMusic.FadeUp (fastFadeIn);
		_playMusic.PlaySelectedMusic (1);
	}
}
