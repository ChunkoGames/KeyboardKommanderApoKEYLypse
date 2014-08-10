/// <summary>
/// High score. This class is inherited by the upgrades class. It's used to track currency and the player.
/// </summary>
using UnityEngine;
using System.Collections;

public class HighScore : MonoBehaviour {

	private int difficultySetting;
	public int highScore;		//The highest score a player has ever gotten
	public int currentScore;	//represents total number of letters collected
	public int letters;			//how many letters you currently have. Letters act as a type of in game currency

	public GUIStyle LargeFont;

	//Supports combo system to get more points/letters
	public int currentCombo;	//the player's current combo
	public int maximumCombo;	//the largest combo ever achieved by the player
	public int comboIncrease;	//when comboIncrease is greater than currentCombo, currentCombo is incremented
	//TODO: include methods to read in a high score from playerprefs and check
	//TODO: When the player's score exceeds it

	void Awake(){
		LargeFont = new GUIStyle();
		LargeFont.fontSize = 50;
		LargeFont.normal.textColor = Color.blue;
	}

	public int DifficultySetting
	{
		get
		{
			return difficultySetting;
		}
		set
		{
			difficultySetting = value;
		}
	}

	// Use this for initialization
	void Start () {
		Messenger.AddListener("Game Over", OnDeath);
		readScore();
		//Combo's are set to 1 because having a combo of 0 doesn't really make sense
		currentCombo = 1;
		maximumCombo = 1;
		
		//All other values are initialized to 0 because that's all you need
		currentScore = 0;
		letters = 0;
		highScore = 0;
		comboIncrease = 0;
	}

	/// <summary>
	/// Attempt to read highscore, currentScore and letters. If any values are missing
	/// set them all to 0
	/// </summary>
	protected void readScore(){
	}

	/// <summary>
	/// After the player dies check to see if the new score is higher than the old
	/// score, if it is update the score
	/// </summary>
	protected void OnDeath(){
		if(currentScore>highScore)
			highScore = currentScore;

		letters = 0;
		currentScore = 0;
	}

	/// <summary>
	/// Adds to the letters when letters are earned
	/// </summary>
	/// <param name="lettersEarned">Letters earned.</param>
	public void addLetters(int lettersEarned){
		letters += lettersEarned;
		currentScore += lettersEarned;
	}

	/// <summary>
	/// Resets the combo. Called when fort takes damage
	/// </summary>
	public void resetCombo(){
		currentCombo = 1;
		comboIncrease = 0;
	}

	/// <summary>
	/// Grant points/lettes to the player for getting a hit on a zombie.
	/// </summary>
	/// <param name="Difficulty">Difficulty level of the zombie</param>
	public void hit(float Difficulty){
		Difficulty = Difficulty + 1.0f;
		letters += (int)Difficulty*currentCombo;
		currentScore += (int)Difficulty*currentCombo*difficultySetting*difficultySetting;
		comboIncrease++;

		if(comboIncrease > currentCombo){
			currentCombo++;
			comboIncrease = 0;
		}
	}

	/// <summary>
	/// Grant the player points for killing a zombie with a specified difficulty. Maximum comvo is used to give the player a rebound mechanic. After losing their combo
	/// it's not as hard to get it back because their score is increasing twice as fast.
	/// </summary>
	/// <param name="Difficulty">Difficulty level of the zombie killed</param>
	public void kill(float Difficulty){
		if(currentCombo < maximumCombo){
			hit (Difficulty*2);
		}
		else{
			maximumCombo = currentCombo;
			hit (Difficulty);
		}
	}

	void OnGUI(){
		if(Application.loadedLevelName == "Basic"){
			LargeFont.fontSize = 30;
			GUI.Label(new Rect(20, 70, 100, 100), "Current Score: " + currentScore,LargeFont);
			GUI.Label(new Rect(20, 100, 100, 100), "Combo: " + currentCombo,LargeFont);
		}
	}

	/// <summary>
	/// Spends the letters.
	/// </summary>
	/// <param name="lettersSpent">Letters spent.</param>
	void spendLetters(int lettersSpent){
		letters -= lettersSpent;
	}
}
