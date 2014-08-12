/// <summary>
/// Text reader - reads in strings from the user to be used for other scripts
/// valid strings are upper case and lower case levels as well as punctuation
/// </summary>
using UnityEngine;
using System.Collections;

public class TextReader : MonoBehaviour {

	//for formatting the buttons and labels:
	GUIStyle smallFont;
	GUIStyle largeFont;
	public string currentString; //this is the string the user has currently entered
	public bool capslock = false;
	private float timeSinceBackSpace = 0.0f;
	public WeaponManager wm;
	public Weapon cw; //current weapon

	public Upgrades upgrades; //reference to the upgrades class used for resetting the combo on hitting backspace and delete

	public bool black = false;	//controls the color of the font

	private float stringSize;

	/// <summary>
	/// Awake this instance. Intialize small and large font right off the bat
	/// </summary>
	void Awake(){
		//Initialize fonts
		currentString = "";
		smallFont = new GUIStyle();
		largeFont = new GUIStyle();
	}

	/// <summary>
	/// Gets the string. This function returns the string that the user has typed
	/// </summary>
	/// <returns>The string.</returns>
	public string getString(){
		return currentString;
	}

	/// <summary>
	/// Check to see if the word passed matches with the users word.
	/// </summary>
	/// <param name="checkWord">The word to check against the input.</param>
	public bool Check(string checkWord){
		if(checkWord ==currentString){
			currentString = "";
			return true;

		}
		return false;
	}
	// Use this for initialization
	void Start () {
		//Add listeners
		Messenger.AddListener("setBlack", setBlack);
		WeaponManager wm = ScriptableObject.CreateInstance<WeaponManager>();
		cw = wm.getWeapon();//Get the current Weapon
	}
 
	/// <summary>
	/// Checks for kills. If the space bar is pressed check all zombies to see if any of them die
	/// </summary>
	private void checkForKills(){
		GameObject []allZombies;
		allZombies= GameObject.FindGameObjectsWithTag("Zombie"); //get a list of all the zombies
		bool hit = false;//if this is false after checking all the zombies then we didn't get a single hit. If this happens
		//reset the combo to 1
		if(allZombies.Length == 0){
			try{
				upgrades.resetCombo();
			}
			catch{
				upgrades =GameObject.FindGameObjectWithTag("Upgrades").GetComponent<Upgrades>();
				upgrades.resetCombo();
			}
			return;
		}
		foreach( GameObject z in allZombies){
			DestroyableObject ZAI = z.GetComponent<DestroyableObject>();
			if(ZAI != null && ZAI.check (currentString)){ //if it's a hit
				Debug.Log ("attempting to load projectile");
				GameObject go_projectile = (GameObject)Instantiate (Resources.Load ("Projectile"));
				go_projectile.transform.position = transform.position;
				go_projectile.GetComponent<Projectile>().setStats(z.transform,currentString,cw);
				//ZAI.hit(cw.getHitPower(),cw.getStunChance()); //Hit the zombie, causes the zombie to take damage, and be subject to other effects such as stun time
				hit = true;
			}
		}

		if(hit == false){ //we didn't get a single hit, reset the combo
			try{
				upgrades.resetCombo();
			}
			catch{
				upgrades =GameObject.FindGameObjectWithTag("Upgrades").GetComponent<Upgrades>();
				upgrades.resetCombo();
			}
		}
	}

	#region update
	// Update is called once per frame
	/// <summary>
	/// Update this instance. Primary function is updating the currentString variable
	/// I am only allowing the player to enter one of the three following keys at a time: 
	/// "Space, backspace, delete", however they may enter as many letter keys as they want
	/// in a given frame.
	/// </summary>
	void Update (){
		//check for Space, Backspace and delete. Else if is used to minimize computations

		/*if(Input.GetKeyUp (KeyCode.CapsLock)){
			Debug.Log("Caps lock pressed");
			if(capslock){capslock = false;}
			else{capslock = true;}
		}*/
		if(Input.GetKey (KeyCode.Backspace)){ //if pressed cut the combo in half
			timeSinceBackSpace +=Time.deltaTime;
			if(timeSinceBackSpace > .1f || Input.GetKeyUp(KeyCode.Backspace)){
				timeSinceBackSpace = 0.0f;
				try{
					upgrades.currentCombo--;
					upgrades.comboIncrease = 0;
					if(currentString.Length > 1)
						currentString = currentString.Substring(0,currentString.Length-1);
					else
						currentString = "";
					try{
						if (upgrades.currentCombo <1) //Don't let upgrades be less than 1
							upgrades.currentCombo = 1;
							upgrades.comboIncrease = 0;
						}
					catch{
						upgrades = GameObject.FindGameObjectWithTag("Upgrades").GetComponent<Upgrades>();
						if (upgrades.currentCombo <1) //Don't let upgrades be less than 1
						{
							upgrades.currentCombo = 1;
							upgrades.comboIncrease = 0;
						}
					}

					return;
				}
				catch{
					upgrades = GameObject.FindGameObjectWithTag("Upgrades").GetComponent<Upgrades>();
					upgrades.currentCombo--;

					try{
						if (upgrades.currentCombo <1) //Don't let upgrades be less than 1
						{
							upgrades.currentCombo = 1;
							upgrades.comboIncrease = 0;
						}
					}
					catch{
						upgrades = GameObject.FindGameObjectWithTag("Upgrades").GetComponent<Upgrades>();
						if (upgrades.currentCombo <1) //Don't let upgrades be less than 1
						{
							upgrades.currentCombo = 1;
							upgrades.comboIncrease = 0;
						}
					}

					if(currentString.Length > 1)
						currentString = currentString.Substring(0,currentString.Length-1);
					else
						currentString = "";
					return;
				}
			}
		}
		else if(Input.GetKeyUp (KeyCode.Delete)){ //if pressed reset the combo
			try{
				upgrades.currentCombo = upgrades.currentCombo - 2;
				currentString = "";
				if (upgrades.currentCombo <1) //Don't let upgrades be less than 1
				{
					upgrades.currentCombo = 1;
					upgrades.comboIncrease = 0;
				}
				return;
			}
			catch{
				upgrades = GameObject.FindGameObjectWithTag("Upgrades").GetComponent<Upgrades>();
				upgrades.currentCombo--;
				if(currentString.Length > 1)
					currentString = currentString.Substring(0,currentString.Length-1);
				else
					currentString = "";
				return;
			}
		}
		else{
			/*if(Input.GetKey(KeyCode.LeftShift)||Input.GetKey(KeyCode.RightShift)|| capslock){
				if(Input.GetKeyUp(KeyCode.A)){
					currentString += "A";
				}
				else if(Input.GetKeyUp(KeyCode.B)){
					currentString += "B";
				}
				else if(Input.GetKeyUp(KeyCode.C)){
					currentString += "C";
				}
				else if(Input.GetKeyUp(KeyCode.D)){
					currentString += "D";
				}
				else if(Input.GetKeyUp(KeyCode.E)){
					currentString += "E";
				}
				else if(Input.GetKeyUp(KeyCode.F)){
					currentString += "F";
				}
				else if(Input.GetKeyUp(KeyCode.G)){
					currentString += "G";
				}
				else if(Input.GetKeyUp(KeyCode.H)){
					currentString += "H";
				}
				else if(Input.GetKeyUp(KeyCode.I)){
					currentString += "I";
				}
				else if(Input.GetKeyUp(KeyCode.J)){
					currentString += "J";
				}
				else if(Input.GetKeyUp(KeyCode.K)){
					currentString += "K";
				}
				else if(Input.GetKeyUp(KeyCode.L)){
					currentString += "L";
				}
				else if(Input.GetKeyUp(KeyCode.M)){
					currentString += "M";
				}
				else if(Input.GetKeyUp(KeyCode.N)){
					currentString += "N";
				}
				else if(Input.GetKeyUp(KeyCode.O)){
					currentString += "O";
				}
				else if(Input.GetKeyUp(KeyCode.P)){
					currentString += "P";
				}
				else if(Input.GetKeyUp(KeyCode.Q)){
					currentString += "Q";
				}
				else if(Input.GetKeyUp(KeyCode.R)){
					currentString += "R";
				}
				else if(Input.GetKeyUp(KeyCode.S)){
					currentString += "S";
				}
				else if(Input.GetKeyUp(KeyCode.T)){
					currentString += "T";
				}
				else if(Input.GetKeyUp(KeyCode.U)){
					currentString += "U";
				}
				else if(Input.GetKeyUp(KeyCode.V)){
					currentString += "V";
				}
				else if(Input.GetKeyUp(KeyCode.W)){
					currentString += "W";
				}
				else if(Input.GetKeyUp(KeyCode.X)){
					currentString += "X";
				}
				else if(Input.GetKeyUp(KeyCode.Y)){
					currentString += "Y";
				}
				else if(Input.GetKeyUp(KeyCode.Z)){
					currentString += "Z";
				}
			}
			else{*/
				if(Input.GetKeyUp(KeyCode.A)){
					currentString += "a";
				}
				else if(Input.GetKeyUp(KeyCode.B)){
					currentString += "b";
				}
				else if(Input.GetKeyUp(KeyCode.C)){
					currentString += "c";
				}
				else if(Input.GetKeyUp(KeyCode.D)){
					currentString += "d";
				}
				else if(Input.GetKeyUp(KeyCode.E)){
					currentString += "e";
				}
				else if(Input.GetKeyUp(KeyCode.F)){
					currentString += "f";
				}
				else if(Input.GetKeyUp(KeyCode.G)){
					currentString += "g";
				}
				else if(Input.GetKeyUp(KeyCode.H)){
					currentString += "h";
				}
				else if(Input.GetKeyUp(KeyCode.I)){
					currentString += "i";
				}
				else if(Input.GetKeyUp(KeyCode.J)){
					currentString += "j";
				}
				else if(Input.GetKeyUp(KeyCode.K)){
					currentString += "k";
				}
				else if(Input.GetKeyUp(KeyCode.L)){
					currentString += "l";
				}
				else if(Input.GetKeyUp(KeyCode.M)){
					currentString += "m";
				}
				else if(Input.GetKeyUp(KeyCode.N)){
					currentString += "n";
				}
				else if(Input.GetKeyUp(KeyCode.O)){
					currentString += "o";
				}
				else if(Input.GetKeyUp(KeyCode.P)){
					currentString += "p";
				}
				else if(Input.GetKeyUp(KeyCode.Q)){
					currentString += "q";
				}
				else if(Input.GetKeyUp(KeyCode.R)){
					currentString += "r";
				}
				else if(Input.GetKeyUp(KeyCode.S)){
					currentString += "s";
				}
				else if(Input.GetKeyUp(KeyCode.T)){
					currentString += "t";
				}
				else if(Input.GetKeyUp(KeyCode.U)){
					currentString += "u";
				}
				else if(Input.GetKeyUp(KeyCode.V)){
					currentString += "v";
				}
				else if(Input.GetKeyUp(KeyCode.W)){
					currentString += "w";
				}
				else if(Input.GetKeyUp(KeyCode.X)){
					currentString += "x";
				}
				else if(Input.GetKeyUp(KeyCode.Y)){
					currentString += "y";
				}
				else if(Input.GetKeyUp(KeyCode.Z)){
					currentString += "z";
				}
			//}
		}

		if(Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp (KeyCode.Return)){
			checkForKills();
			currentString = "";
			return;
		}
	}
	#endregion

	void setBlack(){
		black = true;
	}

	/// <summary>
	/// Make a small label displaying currentString, for the main level this will appear over the fort
	/// </summary>
	void OnGUI(){
		largeFont.fontSize = 30;

		if(black){
			smallFont.normal.textColor = Color.black;
			largeFont.normal.textColor = Color.black;
		}
		else{
			smallFont.normal.textColor = Color.white;
			largeFont.normal.textColor = Color.white;

		}

		stringSize = GUI.skin.GetStyle("label").CalcSize(new GUIContent(currentString)).x;
		GUI.Label(new Rect(Screen.width/2-stringSize*1.2f, Screen.height-100, stringSize*2, 100), currentString,largeFont);
	}
}