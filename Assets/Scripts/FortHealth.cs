using UnityEngine;
using System.Collections;

/// <summary>
/// Fort health. Keeps track fo how much health the fort has, and displays it to the gui
/// </summary>
public class FortHealth : MonoBehaviour {
	public float health; //how much health the player currently has
	public float maxHealth; //the maximum amount of health the player can have

	public GameObject GameObject_upgrades; //reference to the upgrades gameobject
	public Upgrades Upgrades_upgrades; //reference to the upgrades class

	//The following variables are for the healthbar
	public float barDisplay = 0.0f;
	public Vector2 pos = new Vector2(0,0);
	public Vector2 size = new Vector2(2000,2000);
	public Texture2D emptyTex;
	public Texture2D fullTex;
	

	// Use this for initialization
	void Start () {
		//Set the position and size for the healht bar
		size.x = Screen.width/2.0f;
		size.y = Screen.height/20.0f;

		//Set the maximum health
		GameObject_upgrades = GameObject.FindGameObjectWithTag("Upgrades");
		Upgrades_upgrades = GameObject_upgrades.GetComponent<Upgrades>();
		maxHealth = Upgrades_upgrades.calculateMaxHealth();
		health = maxHealth;
		Messenger<float>.AddListener ("takeDamage", takeDamage);
		Messenger<float>.AddListener ("healDamage", healDamage);
	}

	/// <summary>
	/// Takes the damage.
	/// </summary>
	/// <param name="dam">Dam.</param> how much damage to take
	void takeDamage(float dam){
		health  -= dam/10.0f;//subtract the damage\
		
		if (health <= 0.0f){
			//Game over
			Application.LoadLevel ("GameOver");
		}
		barDisplay = (health/maxHealth);

		//get rid of the combo
		Upgrades_upgrades.resetCombo();

		//Save the score in playerprefs
		Upgrades_upgrades.SaveScore();
	}

	/// <summary>
	/// Heals the damage.
	/// </summary>
	/// <param name="dam">Dam.</param> how much damage to heal
	void healDamage(float dam){
		health  += dam;//subtract the damage\
		if(health >= maxHealth){
			health = maxHealth;
		}


		barDisplay = (health/maxHealth);
	}

	/// <summary>
	/// Raises the GU event. Display the health bar
	/// </summary>
	void OnGUI(){
		//Background
		GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
		GUI.Box(new Rect(0,0, size.x, size.y), emptyTex);

		//Filled in part
		GUI.BeginGroup(new Rect(0,0, size.x * barDisplay, size.y));
		GUI.Box(new Rect(0,0, size.x, size.y), fullTex);
		GUI.EndGroup();
		GUI.EndGroup();
	}


}
