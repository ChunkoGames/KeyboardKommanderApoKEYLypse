/// <summary>
/// Difficulty. controlss the difficulty algorithms for the game
/// </summary>
using UnityEngine;
using System.Collections;

public class Difficulty : MonoBehaviour {
	private int difficultyLevel = 1;	//This is selected by the player. This number is multiplied by the number of levels and then divided by 100 to calculate the actual difficulty
	public int letters = 0;
	private float totalDifficulty = 0.0f; //The difficulty which is returned by this function
	public Upgrades u; //our reference to the Upgrades Class
	public int difficulty; //controls the difficulty of the game which in turn controls the difficulty of the zombies which are spawned
	private int difficultyThreshold = 10; //how many spawns until the difficulty level is increased. Once the difficulty is incremented this value is increased by 50%
	private float incrementation = 0.75f; //controlls the incrementation of the difficulty level over time. gradually increases each time.
	private float incrementationFactor = 0.5f; //controls the rate of change of incrementation. We want the rate at which incrementation is increasing to slow down over time
	private int spawned = 0; //tracks the number of enemies which have been spawned
	private int difficultySetting = 0; //How difficulty the user wants this game to be value ranges from 0 to 4

	public int DifficultySetting{
		get{
			return difficultySetting;
		}
		set{
			difficultySetting = value;
		}
	}

	/// <summary>
	/// Start this instance. Get a reference to the Upgrades script on the Main Camera.
	/// </summary>
	void Start(){
		u = GameObject.FindGameObjectWithTag("Upgrades").GetComponent<Upgrades>();
		difficultySetting = u.DifficultySetting;
	}

	/// <summary>
	/// Gets the difficulty. Every five seconds update the number of letters that we have
	/// </summary>
	public int getDifficulty(){
		return difficulty;
	}

	/// <summary>
	/// Gets the number words. Calculated by finding the progress to the next difficulty level (spawned/difficultyThreshold)*4. The vlau
	/// </summary>
	/// <returns>The number words.</returns>
	public int getNumWords(){
		int numWords = (int)(4*(spawned/difficultyThreshold)/(5-difficultySetting));
		return  1 + numWords+ (int)(Mathf.Sqrt(UnityEngine.Random.Range (0,6))) + (int)(difficulty/4); //add one to insure at least one word is spawned
	}

	/// <summary>
	/// Whenever an enemy is spawned adjust the difficulty settings accordingly
	/// Returns the new difficulty level.
	/// </summary>
	public int onSpawn(){
		spawned++;
		if(spawned > difficultyThreshold){ //if enough enemies have been spawned increase the difficulty level
			difficultyThreshold *= (int)(1.00f + incrementation)*(5-difficultySetting);
			difficultyThreshold++;
			incrementation = incrementation * (1.00f + incrementationFactor);
			incrementationFactor *= .9f;
			difficulty++;
		}
		return difficulty;
	}
}
