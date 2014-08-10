#define gui
using UnityEngine;
using System.Collections;
/// <summary>
/// Game interface. This class is responsible for displaying the score and combo to the user.
/// </summary>
public class GameInterface : MonoBehaviour {

	//The GUISKin for the menu
	public GUISkin customSkin;
	
	//Access to the upgrades class to get high score and progress stats:
	public GameObject GameObject_upgrades;
	public Upgrades Upgrades_upgrades;
	public bool whiteFont = true;

	//Used for getting the current health of the fort
	public GameObject fort; //reference to the fort
	public FortHealth FortHealth_fortHealth; //reference to the fort healht script

	private bool guiReady = false;
	private bool black = false;
	
	//for formatting the buttons and labels:
	GUIStyle smallFont;
	GUIStyle largeFont;
	
	float startY = 0;
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
		//Add listeners
		Messenger.AddListener("setBlack", setBlack);
		
		
		smallFont.fontSize = 25;
		largeFont.fontSize = 40;
		
		//White font will stick out on our black background
		smallFont.normal.textColor = Color.white;
		largeFont.normal.textColor = Color.white;
		
		startY = -Screen.height/2 + 120;

		//initialize GameObject_upgrades and Upgrades_upgrades
		GameObject_upgrades = GameObject.Find ("Upgrades-Score");

		//Initialize fortHealth
		fort = GameObject.Find ("Fort"); //get the fort
		FortHealth_fortHealth = fort.GetComponent<FortHealth>();

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
#if gui
			#if left
			xJustification =0;
			#else
			xJustification = Screen.width -2;
			#endif
			//Initialize the labels
			//Make the title label bigger
			GUI.Label(new Rect(0, Screen.height-40, 100, 100), "Current Score: " + Upgrades_upgrades.currentScore,smallFont);

			//Health displayed in fortHealth class

			//print out the currentCombo
			GUI.Label(new Rect(0, Screen.height-80, 100, 100), "Combo: " + Upgrades_upgrades.currentCombo,smallFont);
#endif

		}
	}
}
