/// <summary>
/// Destroyable object. Any object that can be destroyed by getting it's health down to Health Crates and Zombies extend this object
/// </summary>
using UnityEngine;
using System.Collections;

public class DestroyableObject : MonoBehaviour {
	protected Transform tZombie; //the zombie's transform
	protected float TEXT_PADDING_Y = 1.0f;
	protected float TEXT_PADDING_X = .1f;
	
	protected bool sized = false; //If this collider has been sized yet
	public float health = 1.0f; //How much damage it will take to kill this zombie. A health of 1 means that it will take 1 hit with a standard pistol to kil this zombie
	public float _difficulty = 1.0f; //used to calculate score for hits and stats for this zombie
	public BoxCollider2D boxCollider;
	public GameObject gofort; //the fort
	public Transform tFort;	//the fort's transform
	protected bool killed = false;
	
	public string currentWord = "";
	public TextReader tr; //check to see if the current word is this zombies word
	public TextMesh zombieTextMesh;
	protected Stack words;
	public Upgrades upgrades;

	/// <summary>
	/// Start this instance. 
	/// Scale all of the zombies stats to match the difficulty level we are currently on
	/// </summary>
	void Start () {
		//cache the fort location
		gofort = GameObject.Find ("Fort");
		tFort = gofort.transform; //get the fort's transform
		
		upgrades = GameObject.FindGameObjectWithTag("Upgrades").GetComponent<Upgrades>();
		
		tZombie = transform;
		tr = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TextReader>();
		boxCollider = gameObject.GetComponentInChildren<BoxCollider2D>();
		
	}
	/// <summary>
	/// Sizes the box collider.
	/// </summary>
	public void SizeCollider()
	{   
		Renderer textRenderer = gameObject.GetComponentInChildren<Renderer>();
		int chars = (gameObject.GetComponentInChildren<TextMesh>().text).Length;
		boxCollider.center = new Vector2 (textRenderer.bounds.extents.x - textRenderer.bounds.size.x / 2, textRenderer.bounds.extents.y - textRenderer.bounds.size.y / 2);
		boxCollider.size = new Vector2 (textRenderer.bounds.size.x + (chars+1), textRenderer.bounds.size.y + TEXT_PADDING_Y);
	}
	/// <summary>
	/// Hit this instance. Overridden by children.
	/// </summary>
	public virtual void hit(float hitPower, float stunChance){

	}

	/// <summary>
	/// Splash the specified splashDamage.
	/// </summary>
	/// <param name="splashDamage">Splash damage.</param>
	public virtual void splash(float splashDamage, float speedModifier, float knockback){
		
	}

	/// <summary>
	/// Sets the stats.
	/// </summary>
	/// <param name="difficulty">Difficulty.</param>
	/// <param name="h">The height.</param>
	public virtual void setStats(int difficulty, int h){

	}

	/// <summary>
	/// Kills me. This class is overridden by children
	/// </summary>
	public virtual void killMe(){
	}
	/// <summary>
	/// Sets the mesh word. pops string from words stack
	/// </summary>
	public void setMeshWord(){
		if(words.Count != 0){ //if there is more than one word left
			//pop the first element
			currentWord = (string)words.Pop ();
			try{
				zombieTextMesh.text = currentWord; //set gui to current word
			}
			catch{
				Debug.LogWarning("Unity is being stupid");
			}
		}
		else{
			//we're out of words, kill the zombie
			die();
		}
		sized = false;
	}
	
	void die(){
		Destroy(this.gameObject);
	}
	
	/// <summary>
	/// Sets the words arrayList to the passed array List.
	/// </summary>
	/// <param name="wList">Words list.</param>
	public void setWords(Stack wList){
		words = wList;
		setMeshWord();
	}
	
	/// <summary>
	/// Adds one word to the zombies arrayList of words
	/// </summary>
	public void addWord(string w){
		try{
			words.Push (w);
		}
		catch{
			words = new Stack();
			words.Push (w);
		}
		setMeshWord();
	}
	
	/// <summary>
	/// Takes the damage. Called when a word is completed. Subtract health from the zombie and change the state.
	/// </summary>
	public void takeDamage(float damage){
		setMeshWord();
		health -= damage;
		if(health <= 0.0f){
			if(!killed){
				killed = true;
				killMe();
			}
		}
		
		upgrades.hit (_difficulty);
		
		//if the zombie is still alive calculate chance to stun based off of weapon, and zombie strength
	}
	
	/// <summary>
	/// Check the specified text. If the text matches up with this zombies word then we need to kill this zombie and return true. Otherwise return false.
	/// </summary>
	/// <param name="text">Text.</param>
	public bool check(string text){
		if(text == currentWord){
			return true;
		}
		else{
			return false;
		}
	}
}
