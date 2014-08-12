using UnityEngine;
using System.Collections;

/// <summary>
/// Zombies. This is a list of all the zombies in game. The zombies can be killed and added from this list.
/// </summary>
public class ZombieManager : MonoBehaviour {
	//Gameobject references used to 
	public GameObject Fort;
	public GameObject mainCamera;
	public GameObject enemyManager;

	public Dictionary dictionary; //reference to the dictionary for generatig words
	private Difficulty Difficulty_difficulty;//reference to the difficulty calss

	public Vector3 fortPosition; //tracks the position of the fort. This information is passed to the zombies who use this as a target.

	public float waitTime = .1f;	//time to wait
	public float timePassed = -99999.0f; 	//Time since control zombies was last called
	public float spawnRate = 1.50f; //determines whether or not to spawn a zombie
	private int int_difficulty = 0; //our cached version of the difficulty.

	private int difficultySetting = -1; //cached value for difficulty setting

	private int maxZombiesOnScreen = 10; //how many zombies we can have on screen at a given time

	private int numWords = 1;//how many words a zombie gets when the difficulty value changes we need to add reset the numWords variable

	GameObject[] es;	//Stores the list of enemyManagers



	// Use this for initialization
	void Start () {
		//get the different objects we will need for this class
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera"); //get the mainCamera
		dictionary = GameObject.FindGameObjectWithTag("Upgrades").GetComponent<Dictionary>(); //get the dictionaryComponent from upgrades

		Difficulty_difficulty = gameObject.GetComponent<Difficulty>();

		enemyManager = gameObject;//GameObject.Find ("EnemyManager"); //Get the enemy manager gameObject

		//get the fort's information so you can pass it on to the enemies
		Fort = GameObject.Find("Fort");
		fortPosition = Fort.transform.position; 

		//Move remaining zombies
	}
	

	// Update is called once per frame
	//1:See if any of the zombies died if so remove them
	//2:Create the zombies
	//Tell each of the zombies to move
	void Update () {
		timePassed += Time.deltaTime;
		if( GameObject.FindGameObjectsWithTag("Zombie").Length < Difficulty_difficulty.MaxZombies && timePassed > waitTime && Random.Range (0.0f,10.0f)< spawnRate){
			//Randomly determine whether or not to generate a zombie
			addZombie();
			timePassed = 0.0f;
			if(difficultySetting <0){
				difficultySetting = Difficulty_difficulty.DifficultySetting;
			}
			maxZombiesOnScreen = 7 + (int)(Mathf.Sqrt(int_difficulty)) + difficultySetting;
		}

	}

	/// <summary>
	/// Adds the health crate.
	/// </summary>
	public void addHealthCrate(GameObject zombieSpawner, int _difficulty){
		int newDifficulty = _difficulty; //what the difficulty currently is

		
		GameObject goZ = (GameObject)Instantiate (Resources.Load ("HealthBox")); //Instantiate the zombie prefab from resources
		//Assign to an enemyManager
		//1:Randomly choose an enemyManager
		es = GameObject.FindGameObjectsWithTag("Spawner"); //Es now contains all of the enemy spawners
		int emi = (int)Random.Range (0,es.Length); //stands for enemy manager index
		
		//2:Set parent child relationship
		Transform pT = es[emi].transform; //pt stands for parent transform
		goZ.transform.parent = pT;
		Vector3 spawnPos = zombieSpawner.transform.position;
		
		///goZ.transform.position.x += es[emi].transform.localScale.x;
		///goZ.transform.position.y += es[emi].transform.localScale.y;
		goZ.transform.position = spawnPos;
		
		DestroyableObject ZAI = goZ.GetComponent<DestroyableObject>();
		//get words for this zombie. Upgrades knows the difficulty settting so it can effectively determine
		//how many words to give
		numWords = Difficulty_difficulty.getNumWords();
		ZAI.setWords(dictionary.pickWords(int_difficulty,numWords*10));
		ZAI.setStats(int_difficulty,numWords);
		ZAI.setMeshWord();
	}

	/// <summary>
	/// Generate a zombie and add him to the list
	/// </summary>
	void addZombie(){
		int newDifficulty = Difficulty_difficulty.onSpawn(); //what the difficulty currently is
		if(int_difficulty != newDifficulty) //if the difficulty has changed
		{
			int_difficulty = newDifficulty;
			numWords = 1;
		}

		GameObject goZ = (GameObject)Instantiate (Resources.Load ("CommonZombie")); //Instantiate the zombie prefab from resources
		//Assign to an enemyManager
			//1:Randomly choose an enemyManager
		es = GameObject.FindGameObjectsWithTag("Spawner"); //Es now contains all of the enemy spawners
		int emi = (int)Random.Range (0,es.Length); //stands for enemy manager index

			//2:Set parent child relationship
		Transform pT = es[emi].transform; //pt stands for parent transform
		goZ.transform.parent = pT;
		Vector3 spawnPos = pT.position;

		spawnPos.x += Random.Range (-pT.lossyScale.x/2,pT.lossyScale.x/2);
		spawnPos.y += Random.Range (-pT.lossyScale.y/2,pT.lossyScale.y/2);

		///goZ.transform.position.x += es[emi].transform.localScale.x;
		///goZ.transform.position.y += es[emi].transform.localScale.y;
		goZ.transform.position = spawnPos;

		ZombieAI ZAI = goZ.GetComponent<ZombieAI>();
		//get words for this zombie. Upgrades knows the difficulty settting so it can effectively determine
		//how many words to give
			///numWords = Difficulty_difficulty.getNumWords();
		ZAI.setWords(dictionary.pickWords(Difficulty_difficulty.WordLength,Difficulty_difficulty.NumWords*10));
		ZAI.setStats(int_difficulty,numWords);

	}

}