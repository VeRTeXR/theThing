using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


namespace Completed
{
public class Loader : MonoBehaviour {

	public GameObject manager;
	//public GameObject playerManager;
	//public GameObject musicManager;

	public bool gameOver = false;
	// Use this for initialization
	void Awake () {

	
		if (Manager.instance==null) {
			Instantiate(manager);
		/*	if(MusicController.instance==null) 
				{
				Instantiate(musicManager);
				}

			if (PlayerManager.instance==null) 
				{
					Instantiate(playerManager);
				}
				*/
		}
	}

	public void GameOver () {
			// When the game ends, show the title.
			FindObjectOfType<Score> ().Save ();
			gameOver = true;
			
		}

	void Update() {
			if (gameOver) {
				if (Input.GetKeyDown (KeyCode.R)) {
					//Application.LoadLevel(Application.loadedLevel);
					string sceneName = SceneManager.GetActiveScene().name;
					SceneManager.LoadScene(sceneName,LoadSceneMode.Single);
				}
			}
		}

}
}