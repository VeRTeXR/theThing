using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour {
	
	public int width;
	public int height;
	
	public string seed;
	public bool useRandomSeed;
	public GameObject[] tileArray;
	public GameObject[] altTileArray;
	public GameObject tileChoice;
	public int randIndex;
	public int playerCount = 0;
	public GameObject bg;
	public GameObject spawnTile;
	public int spawnLocationX;
	public int spawnLocationY;
	public bool spawnPointCreated = false;
	public int smoothCycle;


	[Range(0,100)]
	public int randomFillPercent;

	private Transform boardHolder;
	private Transform backHolder;

	public int[,] map;
	
	void Start() {
		boardHolder = new GameObject ("Board").transform;
		backHolder = new GameObject ("backboardHolder").transform; 

		randomFillPercent = Random.Range (40,50);
		GenerateMap();
		//map is your grid. put a* sys into it. if 0 then path is free 1 means something is in the way (another random tile)
		OnDraw ();


	}

	void GenerateMap() {
		map = new int[width,height];
		RandomFillMap();
		while (smoothCycle != 0) {
			SmoothMap();
			Debug.Log("smoot"+smoothCycle);
			smoothCycle--;
		}
		flipSpawnValue();

	}

	void DestroyMap(){
		GameObject c = GameObject.FindWithTag("Background");
		if (c != null) {
						Destroy (c);
				}
	}
	
	void RandomFillMap() {
		if (useRandomSeed) {
			seed = Time.time.ToString();
		}
		
		System.Random pseudoRandom = new System.Random(seed.GetHashCode());
		
		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				if (x == 0 || x == width-1 || y == 0 || y == height -1) {
					map[x,y] = 1;
				}
				else {
					map[x,y] = (pseudoRandom.Next(0,100) < randomFillPercent)? 1: 0; //this choose if the shit will be 0 or 1 from fill percent 
				}
			}
		}
	}
	
	void SmoothMap() {
		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				int neighbourWallTiles = GetSurroundingWallCount(x,y);
				//adding shit here in case of spawn tile and etcetc
				//spawn tile within the 1 part and limit the exit and spawn point to 1
				if (neighbourWallTiles > 4)
				{
					map[x,y] = 1; //if neighbour is alive > 4 therefore this tile is alive
				}
				if (neighbourWallTiles < 4) 
				{
					map[x,y] = 0; // else dead
				}

				
			}
		}
	}


	void flipSpawnValue() {
		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				int neighbourWallTiles = GetSurroundingWallCount(x,y);
				if (neighbourWallTiles < 4) 
				{
					if(!spawnPointCreated)
					{
						Debug.Log("value flip");
						map[x,y] = 2; // created spawn at 2
						spawnPointCreated = true;
					}
				}	
			}
		}
	}
	
	int GetSurroundingWallCount(int gridX, int gridY) {
		int wallCount = 0;
		for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX ++) 
		{
			for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY ++) 
			{
				if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) 
				{
					if (neighbourX != gridX || neighbourY != gridY) 
					{
						wallCount += map[neighbourX,neighbourY];
					}
				}
				else 
				{
					wallCount ++;
				}
			}
		}
		
		return wallCount;
	}



	// chrck this or use existing ondraw();
	void SpawnPlayer() { // spawn Player at first open space, kinda sucks but it works 
		for (int x = 0; x < width; x++) {
			spawnLocationX = x; 
			for(int y = 0; y < height; y++) {
				spawnLocationY = y;
				if(map [x,y] == 0){
					if(playerCount == 0) {
						//Instantiate (Player) 
						Debug.Log ("Spawn");
						playerCount = 1;
						return;
					}
					if(playerCount == 1) {
						//Player.pos = spawnlocx, spawnlocy
						Debug.Log("Move Player to Spawn");
						return;
					}
				}
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////



	void OnDraw() 
	{
		if (map != null) 
		{
			for (int x = 0; x < width; x ++) 
			{
				for (int y = 0; y < height; y ++) 
				{
					Vector3 pos = new Vector3(-width/2 + x + .5f,-height/2 + y+.5f, 1);
					if (map [x,y] == 2) 
					{
						Debug.Log("fuckfuckfuk");
						Instantiate (spawnTile, pos, Quaternion.identity);
					}
					if (map [x, y] == 1) 
					{
						randIndex = Random.Range (0, tileArray.Length);
						tileChoice = tileArray [randIndex];
						GameObject instance = Instantiate (tileChoice, pos, Quaternion.identity) as GameObject;
						instance.transform.SetParent (boardHolder);
						GameObject filler = Instantiate (bg, pos, Quaternion.identity) as GameObject ;
						filler.transform.SetParent (backHolder);
					} 
					if (map [x,y] == 0)
					{
						Debug.Log("fuckfuckfssssuk");
						randIndex = Random.Range (0, altTileArray.Length);
						tileChoice = altTileArray [randIndex];
						GameObject instance = Instantiate (tileChoice, pos, Quaternion.identity) as GameObject;
						instance.transform.SetParent (boardHolder);
					}	
				}
			}
		}
	}
	
}
