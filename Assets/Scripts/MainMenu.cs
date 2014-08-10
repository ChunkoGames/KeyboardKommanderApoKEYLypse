#define left

using UnityEngine;
using System.Collections;
/// <summary>
/// Main menu. The class for the main menu. The purpose of this script is to display several buttons which allow the user to be redirected to particular scenes.
/// </summary>
public class MainMenu : Menu {

	//The GUISKin for the menu

	//Access to the upgrades class to get high score and progress stats:
	public GameObject GameObject_upgrades;
	public Upgrades Upgrades_upgrades;
	public bool whiteFont = true;

	private bool guiReady = false;
	private bool black = false;

	//for formatting the buttons and labels:
	GUIStyle smallFont;
	GUIStyle largeFont;

	float startY = 300;
	float spaceY=40;

	float xJustification;

	int currentLevel; //Track the users progress with this variable

	/// <summary>
	/// Awake this instance. Intialize small and large font right off the bat
	/// </summary>
	void Awake(){
		//Initialize fonts
		smallFont = new GUIStyle();
		largeFont = new GUIStyle();
	}

	/// <summary>
	/// Setup the menu
	/// </summary>
	void Start () {
		
		int_difficulty = PlayerPrefs.GetInt("difficulty",2);
		//Add listeners
		Messenger.AddListener("setBlack", setBlack);


		smallFont.fontSize = 25;
		largeFont.fontSize = 40;

		//White font will stick out on our black background
		smallFont.normal.textColor = Color.white;
		largeFont.normal.textColor = Color.white;

		//format the button so that it's visible


		startY = -Screen.height/2 + 120;
		//initialize GameObject_upgrades and Upgrades_upgrades
		GameObject_upgrades = GameObject.FindGameObjectWithTag("Upgrades");

		//if we have now upgrades object load the prefab for upgrades onto the scene
		if(GameObject_upgrades == null){
			Debug.LogWarning ("Warning: Upgrades GameObject could not be found");
			GameObject_upgrades = Instantiate (Resources.Load ("Upgrades-Score")) as GameObject;
		}

		Upgrades_upgrades = GameObject_upgrades.GetComponent<Upgrades>();

		//no matter what we have the upgrades object so we're good to load everything
		guiReady = true;
	}
	
	// Update is called once per frame
	void Update () {
		startY = -Screen.height/2+120;
		if(Input.GetKeyUp(KeyCode.Minus) || Input.GetKeyUp (KeyCode.KeypadMinus) || Input.GetKeyUp (KeyCode.Backslash)){
			easier();
		}

		if(Input.GetKeyUp(KeyCode.Plus) || Input.GetKeyUp(KeyCode.KeypadPlus) || Input.GetKeyUp (KeyCode.Slash)){
			harder ();
		}


	}

	//Sets the gui colors to black
	void setBlack(){
		black = true;
	}

	public int getDifficulty(){
		return int_difficulty;
	}

	/// <summary>
	/// Harder this instance.
	/// </summary>
	public void harder(){
		int_difficulty++;
		if(int_difficulty > 4)
		{
			int_difficulty = 4;
		}
	}

	/// <summary>
	/// Easier this instance.
	/// </summary>
	public void easier(){
		int_difficulty--;
		if(int_difficulty < 0)
		{
			int_difficulty = 0;
		}
	}


	/// <summary>
	/// Make the buttons and print out the labels
	/// </summary>
	void OnGUI(){

		if(black){
			smallFont.normal.textColor = Color.black;
			largeFont.normal.textColor = Color.black;
		}

		if(guiReady){
#if left
			xJustification =0;
#else
			xJustification = Screen.width.2;
#endif
			//Initialize the labels
			//Make the title label bigger
			GUI.Label(new Rect(Screen.width/2-250, 0, 500, 100), "Keyboard Kommander Apo-KEY-lypse",largeFont);

			GUI.Label(new Rect(xJustification, 50, 100, 100), "High Score: " + Upgrades_upgrades.highScore,smallFont);

			string string_difficulty = "Keyboard Kadet";

			switch(int_difficulty){
			case 0:	//beginner
				string_difficulty = "Keyboard Kadet";
				break;
			case 1: //easy
				string_difficulty = "Keyboard Komrade";
				break;
			case 2:	//normal
				string_difficulty = "Keyboard Kaptain";
				break;
			case 3: //hard
				string_difficulty = "Keyboard Kolonel";
				break;
			case 4: //uber hard
				string_difficulty = "Keyboard Kommander";
				break;
			default:
				break;
			}
			
			GUI.Label(new Rect(xJustification, 100, 100, 100), "Difficulty: " + string_difficulty,smallFont);
			//TODO: add buttons to increment and decrement difficulty
			if(GUI.Button(new Rect(xJustification+500, 100, 200, 40), "Harder (\\)")){
				//Clear Upgrades
				harder();
			}
			if(GUI.Button(new Rect(xJustification+700, 100, 200, 40), "Easier (/)")){
				//Clear Upgrades
				easier();
			}

			//GUI.Label(new Rect(xJustification, 100, 100, 100), "Current Score: " + Upgrades_upgrades.currentScore,smallFont);

			GUI.Label(new Rect(xJustification, 150, 100, 100), "Type or click a command to get started",smallFont);
			GUI.skin.button.fontSize = 25;
			//initialize the buttons
			/*if(GUI.Button(new Rect(xJustification, Screen.height -180, 240, 40), "Instructions")){
				//Clear Upgrades
				Debug.Log ("Instructions Clicked");
				Application.LoadLevel("Instuctions");
			}*/
			if(GUI.Button(new Rect(xJustification, Screen.height - 140, 240, 40), "startnewgame")){
				PlayerPrefs.SetInt("difficulty",int_difficulty);
				//Clear Upgrades
				Debug.Log ("Start New Game Clicked");
				GameObject_upgrades.SendMessage("ClearUpgrades");
				GameObject_upgrades.GetComponent<Upgrades>().DifficultySetting = int_difficulty;
				Application.LoadLevel ("Basic");
			}

			if(GUI.Button(new Rect(xJustification, Screen.height - 180, 240, 40), "instructions")){
				//Clear Upgrades
				Debug.Log ("Instructions Clicked");
				Application.LoadLevel ("Instructions");
			}
			/*if(GUI.Button(new Rect(xJustification, Screen.height -100, 240, 40), "Continue")){
				//Clear Upgrades
				Debug.Log ("Continue Clicked");
				GameObject.Find("Upgrades-Score").SendMessage("ReadUpgrades");
				Application.LoadLevel ("Basic");
			}*/
		}
	}
}
