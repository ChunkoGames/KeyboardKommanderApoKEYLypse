using UnityEngine;
using System.Collections;

public class HealthCrate : DestroyableObject {
	private float time = 10.0f; //how long this crate will stay here before it disapears
	private float timePassed = 0; //How much time has passed;
	/// <summary>
	/// Hit this instance. Check for chance of stun as well as take damage
	/// </summary>
	public override void hit(float hitPower, float stunChance){
		takeDamage(hitPower);
		time += 1.0f;
	}

	/// <summary>
	/// Splash the specified splashDamage.
	/// </summary>
	/// <param name="splashDamage">Splash damage.</param>
	public override void splash(float splashDamage, float speedModifier, float knockback){
		health -= splashDamage;
		if(health <= 0.0f){
			if(!killed){
				killed = true;
				killMe();
			}
		}
	}

	/// <summary>
	/// Sets the stats for this zombie. including speed attackdistance, attackDamage, etc.
	/// </summary>
	public override void setStats(int difficulty, int h){
		//Set the stats
		_difficulty = difficulty;
		health = h +1;
		time = h * (_difficulty + 1)*5.0f + 5.0f;
	}

	/// <summary>
	/// Sets the postion. Set to a zombies position so it's like the zombie is dropping it.
	/// </summary>
	/// <param name="pos">Position.</param>
	public void setPostion(Vector2 pos){
		transform.position = pos;
	}

	/// <summary>
	/// Kills the zombie. Plays death animation/particle effects and removes the gameobject from play
	/// </summary>
	public override void killMe(){
		//give the player letters for getting a kill
		upgrades.kill(_difficulty*10);
		//animations
		tFort.BroadcastMessage("healDamage", (10 + _difficulty));
		
		//remove gameobject
		Destroy(this.gameObject);
	}

	public void Update(){
		timePassed += Time.deltaTime;
		if(timePassed > time){
			Destroy (this.gameObject);
		}
	}
}
;