/// <summary>
/// Weapon. Keeps track of a single weapon. The player will eventually be able to have multiple weapons which they can switch between
/// </summary>
using UnityEngine;
using System.Collections;

public class Weapon : ScriptableObject{
	private int stunChance; //How likely this weapon is to stun 1 - 100, 100 translates to 100% stun chance. This value can be over 100 so it can do damage to zombies with extreme resistance
	private float maxDurability;
	private float durability; //The current state of the weapon. The more it is used, the less effective it is. Ticks down from 100. Durability can be improved but not by much
	private int healthSteal; //how much health is restored to the fort when a zombie is killed
	private float hitPower;	//how much damage the weapon does (if a zombie runs out of words they will also die, but a high damage means you kill them faster)
	private float speed; //how long it takes for a projectile from this weapon to hit a zombie
	private bool penetrating; //whether or not this weapon fires projectiles that can penetrate enemies. If penetration is not on the projectile stops cold. If it is on it hurts the enemy
	private float splashDamage;
	private float knockBack;
	//it passes through and then keeps on going

	public enum type{
		Melee = 0, //Only useful when the zombie is really close to the fort
		Catapult = 1, //High damage, but takes a while to hit
		Cannon = 2, //Run of the mill in every regard
		Laser = 3 //Low damage rapid fire
	}
	// Use this for initialization
	public Weapon () {
		Debug.Log ("Weapon Constructor called");
		stunChance = 50; //50% stun chance
		hitPower = 1.0f;
		speed = 1.0f;
		knockBack = 5.0f;
		splashDamage = 1.0f;
		penetrating = false;

		maxDurability = 100.0f;
		durability = maxDurability;
	}

	public float KnockBack{
		get{
			return knockBack;
		}
		set{
			knockBack = value;
		}
	}

	public bool Penetrating{
		get{
			return penetrating;
		}
		set{
			penetrating = value;
		}
	}

	public void takeDamage(float durabilityDamage){
		durability -= durabilityDamage;
	}

	/// <summary>
	/// Gets the hit power.
	/// </summary>
	/// <returns>The hit power.</returns>
	public float getHitPower(){
		return hitPower;
	}

	/// <summary>
	/// Gets the speed.
	/// </summary>
	/// <returns>The speed.</returns>
	public float getSpeed(){
		return speed;
	}

	/// <summary>
	/// Gets the stun chance.
	/// </summary>
	/// <returns>The stun chance.</returns>
	public float getStunChance(){
		return stunChance;
	}

	/// <summary>
	/// Gets the splash damage.
	/// </summary>
	/// <returns>The splash damage.</returns>
	public float getSplashDamage(){
		return splashDamage;
	}
}
