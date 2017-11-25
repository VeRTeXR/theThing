using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {

	[Serializable]
	public class Count {
		public int minimum;
		public int maximum;

		public Count (int min, int max) {
			minimum = min;
			maximum = max;
		}

	}

	public int columns = 30;
	public int rows = 30;
	public Count wallCount = new Count (5,5);
	public GameObject player;
	public GameObject exit;
	public GameObject spawn;
	public GameObject[] floorTiles;
	public GameObject[] environmentTiles;
	public GameObject[] enemyTiles;
	public GameObject[] outerWallTiles;
	public GameObject[] itemTiles;
	public int minEnemyCount;
	public int maxEnemyCount;
	public int minItemCount;
	public int maxItemCount;
	public int minEnvironmentCount;
	public int maxEnvironmentCount;

	private Transform boardHolder;
	private List <Vector3> gridPositions = new List <Vector3>();


	void InitialiseList () {
		gridPositions.Clear();
		for (int x=1; x<columns-1; x++) {
			for (int y=1; y<rows-1; y++) {
				gridPositions.Add(new Vector3(x,y,0f));

			}
		}
	}

	void BoardSetup ()
	{
		//Instantiate Board and set boardHolder to its transform.
		boardHolder = new GameObject ("BackgroundBoard").transform;
		
		//Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
		for(int x = -1; x < columns + 1; x++)
		{
			//Loop along y axis, starting from -1 to place floor or outerwall tiles.
			for(int y = -1; y < rows + 1; y++)
			{
				//Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
				GameObject toInstantiate = floorTiles[Random.Range (0,floorTiles.Length)];


				if(x == -1 || x == columns || y == -1 || y == rows)
					toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];

				
				//Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
				GameObject instance =
					Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
				
				//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
				instance.transform.SetParent (boardHolder);
			}
		}
	}
	

	
	Vector3 RandomPos() {
		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex];
		gridPositions.RemoveAt (randomIndex);
		return randomPosition;
	}
	
	void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum) {
		int objectCount = Random.Range (minimum, maximum + 1); //3-7
		Debug.Log (objectCount);

		if(objectCount == 0) {
			
		}

		for (int i = 0; i < objectCount; i++) {
			Vector3 randompPos = RandomPos();
			GameObject tileChoice = tileArray [Random.Range(0, tileArray.Length-1)];
			Instantiate (tileChoice, randompPos, Quaternion.identity);
		}
	}

	public void SetupScene(int level) {

		InitialiseList ();
		//BoardSetup ();

		minEnemyCount = 1 + (int)Mathf.Log (level, 2f) * 2;
		maxEnemyCount = 1 + (int)Mathf.Log (level, 2f) * 5;
		minItemCount = (int)Mathf.Log (level, 2f) * 2;
		maxItemCount = (int)Mathf.Log (level, 2f) * 5;
		minEnvironmentCount = (int)Mathf.Log (level, 2f) * 5;
		maxEnvironmentCount = (int)Mathf.Log (level, 2f) * 5; 

		//Instantiate a random number of enemies based on minimum and maximum, at randomized positions.

		LayoutObjectAtRandom (enemyTiles, minEnemyCount, maxEnemyCount);
		LayoutObjectAtRandom (environmentTiles, minEnvironmentCount, maxEnvironmentCount);
		LayoutObjectAtRandom (itemTiles, minItemCount, maxItemCount);


		//Instantiate the exit tile in the upper right hand corner of our game board
		//Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);
		
	}
	
	
}
