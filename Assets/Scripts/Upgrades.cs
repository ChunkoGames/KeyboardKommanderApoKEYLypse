using UnityEngine;
using System.Collections;

/// <summary>
/// Upgrades.
/// This class inherits from highscore to gain access to letters
/// Since this inherits from highscore we do not need a highscore object
/// </summary>
public class Upgrades : HighScore {

	public int healthUpgrades =0;
	public Upgrades() {

	}

	void Awake(){
		Messenger.AddListener("Game Over", OnDeath);
		readScore();
		//Combo's are set to 1 because having a combo of 0 doesn't really make sense
		currentCombo = 1;
		maximumCombo = 1;
		
		//All other values are initialized to 0 because that's all you need
		currentScore = 0;
		letters = 0;
		highScore = PlayerPrefs.GetInt ("HighScore",0);
		comboIncrease = 0;
		Messenger.AddListener ("ReadUpgrades",ReadUpgrades);
		Messenger.AddListener ("ClearUpgrades",ClearUpgrades);

	}

	/// <summary>
	/// Read in the upgrades data from the player preferences 
	/// </summary>
	void Start () {

	}

	/// <summary>
	/// Calculates the max health using the number of healthUpgrades.
	/// </summary>
	/// <returns>The max health.</returns>
	public float calculateMaxHealth(){
		return 100.0f + 10.0f*((float)healthUpgrades);
	}

	/// <summary>
	/// Read upgrades from playerPrefs, if they are missing reinitialize
	/// </summary>
	void ReadUpgrades(){
		Debug.LogError("Read Upgrades Has Been Called");
		highScore = PlayerPrefs.GetInt ("HighScore",0);
	}
	
	/// <summary>
	/// If this is upgraded record changes and save them to the system
	/// </summary>
	void Update () {
	
	}
	/// <summary>
	/// Clear this instance. Reset all values
	/// </summary>
	public void ClearUpgrades(){

	}

	/// <summary>
	/// Saves the score. If the currentScore is higher than currentscore save it in playerPrefs
	/// </summary>
	public void SaveScore(){
		if(currentScore > highScore){
			PlayerPrefs.SetInt("HighScore",currentScore);
		}
	}
}
