﻿#define left

using UnityEngine;
using System.Collections;

public class InstructionsMenu : Menu {
	
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
		int_difficulty = PlayerPrefs.GetInt("difficulty",0);
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
		if(Input.GetKeyUp(KeyCode.Minus) || Input.GetKeyUp (KeyCode.KeypadMinus) || Input.GetKeyUp (KeyCode.Backslash) || Input.GetKeyUp (KeyCode.Underscore)){
			easier();
		}
		
		if(Input.GetKeyUp(KeyCode.Plus) || Input.GetKeyUp(KeyCode.KeypadPlus) || Input.GetKeyUp (KeyCode.Slash) || Input.GetKeyUp(KeyCode.Equals)){
			harder ();
		}

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
	
	


	//Sets the gui colors to black
	void setBlack(){
		black = true;
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
			xJustification =25;
			#else
			xJustification = Screen.width.2;
			#endif
			//Initialize the labels
			//Make the title label bigger
			GUI.Label(new Rect(Screen.width/2-250, 0, 500, 100), "Instructions",largeFont);

			GUI.Label(new Rect(xJustification, 150, 100, 100), "Enemies will come at you from all directions. Type the word above their head and hit space \nto fire a " +
			          "projetile at them. After the projectile hits them either they will get a new word or they will die.\n" + 
			          "Sometimes when a zombie dies it will drop a health box. Destroy this by typing its words nad hitting\n" +
			          "space to get extra points and extra health. Each hit you get will help to increase your combo.\n"+
			          "Hitting backspace, hitting delete, or taking damage will hurt your combo. If a zombie hits your fort\n" +
			          "you will take damage until it is killed.",smallFont);

			GUI.skin.button.fontSize = 25;
			//initialize the buttons
			/*if(GUI.Button(new Rect(xJustification, Screen.height -180, 240, 40), "Instructions")){
				//Clear Upgrades
				Debug.Log ("Instructions Clicked");
				Application.LoadLevel("Instuctions");
			}*/

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
			if(GUI.Button(new Rect(xJustification+500, 100, 200, 40), "Harder (+)")){
				//Clear Upgrades
				harder();
			}
			if(GUI.Button(new Rect(xJustification+700, 100, 200, 40), "Easier (-)")){
				//Clear Upgrades
				easier();
			}


			if(GUI.Button(new Rect(xJustification, Screen.height - 140, 240, 40), "startnewgame")){
				PlayerPrefs.SetInt("difficulty",int_difficulty);
				//Clear Upgrades
				Debug.Log ("Start New Game Clicked");
				GameObject_upgrades.SendMessage("ClearUpgrades");
				GameObject_upgrades.GetComponent<Upgrades>().DifficultySetting = int_difficulty;
				Application.LoadLevel ("Basic");
			}

			if(GUI.Button(new Rect(xJustification, Screen.height - 100, 240, 40), "mainmenu")){
				//Clear Upgrades
				Debug.Log ("Main Menu Clicked");
				Application.LoadLevel ("MainMenu");
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
