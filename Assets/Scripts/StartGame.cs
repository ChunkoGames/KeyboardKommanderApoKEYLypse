using UnityEngine;
//using System.Collections;
using System;

/// <summary>
/// Startgame. Checks to see if the string is equal to any key strings. If it is jump to the appropriate scene
/// </summary>
public class StartGame : MonoBehaviour {
	public TextReader tr; //reference to the textreader, used to check the input
	public Menu menu;
	public Upgrades upgrades;
	public bool instructions = false;
	// Each frame check to see if the current string is one of the key words. If it is perform
	// the appropriate action

	/// <summary>
	/// Start this instance. Find and cache our TextReader object
	/// </summary>
	void Start(){
		tr = GameObject.Find ("Main Camera").GetComponent<TextReader>();
		menu = GameObject.Find ("Main Camera").GetComponent<Menu>();
		upgrades = GameObject.FindGameObjectWithTag("Upgrades").GetComponent<Upgrades>();
	}

	/// <summary>
	/// Update this instance. Find out if our currentString matches any of our commands
	/// </summary>
	void Update () {
		string input = (tr.currentString).ToLower();
		if(instructions == false){
			switch(input){
			case "startnewgame": //If the user has typed start new game, load the first level with no upgrades
				upgrades.ClearUpgrades();//GameObject.Find("Upgrades-Score").SendMessage("ClearUpgrades");
				upgrades.DifficultySetting = menu.Int_difficulty;
				Application.LoadLevel ("Basic");
				break;
			case "continue": //If the user has typed continue, continue where he/she last left off
				//load the player preferences and then start the game
				upgrades.ClearUpgrades();//GameObject.Find("Upgrades-Score").SendMessage("ReadUpgrades");
				upgrades.DifficultySetting = menu.Int_difficulty;
				Application.LoadLevel ("Basic");
				break;
			case "mainmenu": //If the user has typed continue, continue where he/she last left off
				//load the player preferences and then start the game
				upgrades.ClearUpgrades();
				Application.LoadLevel ("MainMenu");
				break;
			case "instructions": //If the user has typed instructions, load the instructions page
				Application.LoadLevel ("Instructions");
				break;
			}
		}
		else if(instructions == true){
			switch(input){
			case "startnewgame": //If the user has typed start new game, load the first level with no upgrades
				GameObject.Find("Upgrades-Score").SendMessage("ClearUpgrades");
				upgrades.DifficultySetting = menu.Int_difficulty;
				Application.LoadLevel ("Basic");
				break;
			case "continue": //If the user has typed continue, continue where he/she last left off
				//load the player preferences and then start the game
				upgrades.ClearUpgrades();
				upgrades.DifficultySetting = menu.Int_difficulty;
				Application.LoadLevel ("Basic");
				break;
			case "instructions": //If the user has typed instructions, load the instructions page
				Application.LoadLevel ("Instructions");
				break;
			}
		}
	}
}
