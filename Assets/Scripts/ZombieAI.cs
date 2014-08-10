/// <summary>
/// Zombie AI. THis class controls a single zombie's movement. They will move, somewhat erratically towards the fort in the center of the map
/// </summary>
using UnityEngine;
using System.Collections;

public class ZombieAI : DestroyableObject {
	public enum ZombieState {
		Stunned, //stop moving until unstunned
		Crippled, //only able to move slowly
		Enraged, //move faster
		Attacking, //Currently damaging the fort, do not move
		Active //standard state
	}

	//Determines what type of zombie this is
	//and what the strongest characteristic is
	public enum ZombieType{
		Tank=0, //slow moving high health, high damage
		Sprinter=1, //High speed, low stun time
		Rager=2, //Can't be stunned, automatically rages after being attacked
		Standard=3 //Weaker than the other types, now distinguishing characteristic
	}


	public float speed = 1.0f;	//How fast this zombie moves
	private float attackDistance = 7.0f;	//distance from which an enemy can attack
	public float attackDamage = 1.0f;//how much damage this zombie does when it attacks
	private float stunTime = 5.0f; //how long the zombie stays stunned for
	private int rageChance = 2;
	public float timeSinceStun = 0.0f;

	public int stunResistance = 25; //how difficult it is to stun this zombie

	public int unstunThreshold = 1; //determines how difficult it is for this zombie to b eunstunned 10 is the lowest 
	public ZombieState zombieState;
	public ZombieType zombieType;
	


	private float distance = 999.0f; //Distance from this zombie to the fort



	/// <summary>
	/// Kills the zombie. Plays death animation/particle effects and removes the gameobject from play
	/// </summary>
	public override void killMe(){
		//give the player letters for getting a kill
		upgrades.kill(_difficulty);
		//animations

		//5% chance to spawn a health crate
		if(Random.Range (0,100)<=10){
			transform.parent.GetComponentInParent<ZombieManager>().addHealthCrate(this.gameObject,(int)_difficulty);//see if we don't need to go two levels up
		}

		//remove gameobject
		Destroy(this.gameObject);
	}


	/// <summary>
	/// Sets the stats for this zombie. including speed attackdistance, attackDamage, etc.
	/// </summary>
	public override void setStats(int difficulty, int h){
		_difficulty = (float)difficulty;

		int int_zombieType = UnityEngine.Random.Range (0, 6);


		switch(int_zombieType){
		case 0:
			zombieType = ZombieType.Rager;
			health = (float)UnityEngine.Random.Range (h,2*h);
			speed = (float)UnityEngine.Random.Range (0.25f,0.5f + Mathf.Sqrt(_difficulty)*0.1f);
			attackDamage = (float)UnityEngine.Random.Range (0.1f,0.1f + Mathf.Sqrt(_difficulty)*0.1f);
			stunTime = (float)UnityEngine.Random.Range (1.0f, 5.0f);
			stunResistance = 25 + (int)UnityEngine.Random.Range (0,_difficulty);
			unstunThreshold = 1 + (int)_difficulty;
			rageChance = 10;
			break;
		case 1:
			zombieType = ZombieType.Sprinter;
			health = (float)UnityEngine.Random.Range (h,2*h);
			speed = (float)UnityEngine.Random.Range (1.5f,3.5f + Mathf.Sqrt(_difficulty)*0.4f);
			attackDamage = (float)UnityEngine.Random.Range (0.25f,0.25f + Mathf.Sqrt(_difficulty)*0.1f);
			stunTime = (float)(UnityEngine.Random.Range (3.0f, 8.0f)/(1 + Mathf.Sqrt(_difficulty)*0.1f));
			stunResistance = 25 + (int)UnityEngine.Random.Range (0.0f,Mathf.Sqrt(_difficulty));
			unstunThreshold = 1 + (int)_difficulty;
			rageChance = (int)UnityEngine.Random.Range (1,4);
			break;
		case 2:
			zombieType = ZombieType.Standard;
			health = (float)UnityEngine.Random.Range (h,2*h);
			speed = (float)UnityEngine.Random.Range (0.5f,2.0f + Mathf.Sqrt(_difficulty)*0.1f);
			attackDamage = (float)UnityEngine.Random.Range (0.5f,2.0f + Mathf.Sqrt(_difficulty)*0.1f);
			stunTime = (float)(UnityEngine.Random.Range (3.0f, 8.0f)/(1 + Mathf.Sqrt(_difficulty)*0.1f));
			stunResistance = 25 + (int)UnityEngine.Random.Range (0.0f,Mathf.Sqrt(_difficulty));
			unstunThreshold = 1 + (int)_difficulty;
			rageChance = (int)UnityEngine.Random.Range (2,8);
			break;
		case 3:
			zombieType = ZombieType.Tank;
			health = (float)UnityEngine.Random.Range (2*h,6*h);
			speed = (float)UnityEngine.Random.Range (0.02f,0.04f + Mathf.Sqrt(_difficulty)*0.02f);
			attackDamage = (float)UnityEngine.Random.Range (0.01f,0.1f + _difficulty*0.02f);
			stunTime = (float)UnityEngine.Random.Range (1.0f, 2.0f);
			stunResistance = 25 + (int)_difficulty;
			unstunThreshold = 1 + (int)_difficulty;
			rageChance = 0;
			break;
		default:
			zombieType = ZombieType.Standard;
			health = (float)UnityEngine.Random.Range (h,2*h);
			speed = (float)UnityEngine.Random.Range (0.5f,2.0f + Mathf.Sqrt(_difficulty)*0.1f);
			attackDamage = (float)UnityEngine.Random.Range (0.5f,2.0f + Mathf.Sqrt(_difficulty)*0.1f);
			stunTime = (float)(UnityEngine.Random.Range (3.0f, 8.0f)/(1 + Mathf.Sqrt(_difficulty)*0.1f));
			stunResistance = 25 + (int)UnityEngine.Random.Range (0.0f,Mathf.Sqrt(_difficulty));
			unstunThreshold = 1 + (int)_difficulty;
			rageChance = (int)UnityEngine.Random.Range (2,8);
			break;
		}
		
		zombieState = ZombieAI.ZombieState.Active;	//Start out in active mode
	}


	/// <summary>
	/// Hit this instance. Check for chance of stun as well as take damage
	/// </summary>
	public override void hit(float hitPower, float stunChance){
		takeDamage(hitPower);
		//stun the zombie if a high enough roll is achieved. stunResistance is used as the lower bound and is added to the upper bound
		if(zombieState == ZombieState.Enraged || UnityEngine.Random.Range(stunResistance,stunResistance + 100) < stunChance){
			zombieState = ZombieState.Stunned;
		}
	}

	/// <summary>
	/// Splash the specified splashDamage.
	/// </summary>
	/// <param name="splashDamage">Splash damage.</param>
	public override void splash(float splashDamage, float speedModifier, float knockback){
//		Debug.Log ("Splash Damage:" + splashDamage + ":" + speedModifier);
		health -= splashDamage;
		speed *= speedModifier;
		Vector2.MoveTowards(tZombie.position, tFort.position, -knockback*Time.smoothDeltaTime*5.0f);
		if(health <= 0.0f){
			if(!killed){
				killed = true;
				killMe();
			}
		}
	}

	/// <summary>
	/// Move this zombie.
	/// 1. Check the state of the zombie
	/// 		if stunned attempt to unstun
	/// 2. Face the fort (use randomNess and speed)
	/// 3. Move toward the fort use only speed
	/// </summary>
	void Update(){
		if(sized == false){
			SizeCollider();
			sized = true;
		}
		/*try{
			if(tr.Check(currentWord)){
				//give the player points for getting a hit
				upgrades.hit (_difficulty);
				//take damage and pick new word
				takeDamage();
			}
		}
		catch{
			Debug.LogError("string could not be checked currentWord:"+currentWord);
		}*/
		//Check to see if we're dead
		switch(zombieState){
		case ZombieState.Stunned://if stunned try to become unstunned
			timeSinceStun += Time.deltaTime;
			if(timeSinceStun > stunTime){
				if(Random.Range(0,250) < 10 + unstunThreshold){
					if(Random.Range (0,10) < rageChance){
						zombieState = ZombieState.Crippled;
					}
					else
					{
						zombieState = ZombieState.Enraged;
					}
					timeSinceStun = 0;
				}
			}
			break;
		case ZombieState.Crippled://if crippled move 1/4 as fast
			tZombie.position = Vector2.MoveTowards(tZombie.position, tFort.position, Time.smoothDeltaTime/4.0f);
			distance = Vector2.Distance (tZombie.position,tFort.position);
			if(distance <= attackDistance){
				zombieState = ZombieState.Attacking;
			}
			break;
		case ZombieState.Active: //move normally
			tZombie.position = Vector2.MoveTowards(tZombie.position, tFort.position, Time.smoothDeltaTime);
			distance = Vector2.Distance (tZombie.position,tFort.position);
			if(distance <= attackDistance){
				zombieState = ZombieState.Attacking;
			}
			break;
		case ZombieState.Enraged://move 4x as fast
			if(zombieType == ZombieType.Rager){
				tZombie.position = Vector2.MoveTowards(tZombie.position, tFort.position, Time.smoothDeltaTime*10.0f);
				distance = Vector2.Distance (tZombie.position,tFort.position);
				if(distance <= attackDistance){
					zombieState = ZombieState.Attacking;
				}
			}
			else{
				tZombie.position = Vector2.MoveTowards(tZombie.position, tFort.position, Time.smoothDeltaTime*4.0f);
				distance = Vector2.Distance (tZombie.position,tFort.position);
				if(distance <= attackDistance){
					zombieState = ZombieState.Attacking;
				}
			}
			break;
		case ZombieState.Attacking:
			distance = Vector2.Distance (tZombie.position,tFort.position);//calculate the distance
			//Attack the fort
			if(distance <= attackDistance){ //if we've been pushed out of range start running again
				gofort.BroadcastMessage ("takeDamage",attackDamage);
			}
			else{
				zombieState = ZombieState.Enraged;
			}
			break;
		}

	}

	/// <summary>
	/// Raises the collision enter2 d event. If we collide with the player start attacking and say we can attack from this distance
	/// </summary>
	/// <param name="coll">Coll.</param>
	void OnCollisionEnter2D(Collision2D coll) {
		if(coll.gameObject.tag == "Player"){
			distance = Vector2.Distance (tZombie.position,tFort.position);//calculate the distance
			attackDistance = distance + .2f;//since we collided make sure we can attack from this distance

			gofort.BroadcastMessage ("takeDamage",attackDamage);
			zombieState = ZombieState.Attacking;
		}
	}
}
