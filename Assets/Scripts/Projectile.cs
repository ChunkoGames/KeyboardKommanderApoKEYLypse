using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public Transform _target;
	private string _word; //compare against colliding target to see if the target takes damage
	private Weapon _weapon;
	private bool _enabled;
	private float _speed;
	private float _splashDamage;
	private float _speedModifier; //how much to slow down splash victims by
	private float _splashrange;
	private float _range; //max distance of an enemy which can be hit
	private float _knockback; //how much to move the enemies back
	private Transform _transform;//the projectile's transform

	private float _timePassed; //when 100 seconds have passed destroy this projectile

	// Use this for initialization
	void Start () {
	
	}
	/// <summary>
	/// Sets the stats.
	/// </summary>
	/// <param name="target">Target. transform this gameobjct pursues</param>
	/// <param name="word">Word. Can only hit gameobject with this word</param>
	/// <param name="speed">Speed. Controls how fast this projectile moves</param>
	/// <param name="power">Power. How much damage this projectile does</param>
	/// <param name="knockback">Knockback. How much this projectile pushes back enemies</param>
	/// <param name="splashDamage">Splash damage. Controls distance and damage for splash damage</param>
	/// <param name="penetrating">If set to <c>true</c> penetrating. Whether or not this projectile can go throuhg multiple enemies</param>
	public void setStats(Transform target, string word, Weapon w){
		_target = target;
		_word = word;
		_weapon = w;
		_speed = _weapon.getSpeed();
		_timePassed = 0;
		_transform = transform;
		_splashDamage = w.getSplashDamage();
		_knockback = w.KnockBack;
		_speedModifier = 0.9f - (0.1f * Mathf.Sqrt(_splashDamage));
		if(_speedModifier < 0.1f){
			_speedModifier = 0.1f;
		}
		_range = Mathf.Sqrt(_splashDamage) + 3.0f;



		//GetComponentInChildren<SplashDamage>().setup(_weapon.getSplashDamage());	 
		 


		_enabled = true;
	}

	void OnCollisionEnter(Collision coll){
		if(coll.transform.tag == "Zombie"){
			DestroyableObject ZAI = coll.gameObject.GetComponent<DestroyableObject>();
			if(ZAI.check (_word)){ //if the words match up then we have hit our target
				//Do damage to this enemy
				ZAI.hit(_weapon.getHitPower(),_weapon.getStunChance());
				
				
				//Debug.Log ("Checking for splash damage position.x:" + transform.position.x + " position.y:" + transform.position.y);
				//Do splash damage
				GameObject [] allZombies = GameObject.FindGameObjectsWithTag("Zombie");
				foreach(GameObject go in allZombies){
					if(Vector2.Distance (go.transform.position,transform.position) < _range){
						//Debug.Log ("Splashing Object:" + go.name);
						go.GetComponent<DestroyableObject>().splash(_splashDamage,_speedModifier, _knockback);
					}
				}
				
			}
			else if(_weapon.Penetrating){
				ZAI.hit(_weapon.getHitPower(),_weapon.getStunChance());
			}
		}
		
		if(!_weapon.Penetrating){
			Destroy (gameObject);
		}
	}

	/// <summary>
	/// Raises the collision enter2 d event. If the enemy we've collided with is our target do damage and destroy the projectile. If the enemy we've collided with is not our target
	/// then if penetrating is enabled damage the target and keep moving. Else destroy the projectile
	/// </summary>
	/// <param name="coll">Coll.</param>
	void OnCollisionEnter2D(Collision2D coll){
		Debug.Log ("Collision with:" + coll.gameObject.name);
		if(coll.transform.tag == "Zombie"){
			DestroyableObject ZAI = coll.gameObject.GetComponent<DestroyableObject>();
			if(ZAI.check (_word)){ //if the words match up then we have hit our target
				//Do damage to this enemy
				ZAI.hit(_weapon.getHitPower(),_weapon.getStunChance());


				//Debug.Log ("Checking for splash damage position.x:" + transform.position.x + " position.y:" + transform.position.y);
				//Do splash damage
				GameObject [] allZombies = GameObject.FindGameObjectsWithTag("Zombie");
				foreach(GameObject go in allZombies){
					if(Vector2.Distance (go.transform.position,transform.position) < _range){
						//Debug.Log ("Splashing Object:" + go.name);
						go.GetComponent<DestroyableObject>().splash(_splashDamage,_speedModifier, _knockback);
					}
				}
				Destroy (gameObject);

			}
			else if(_weapon.Penetrating){
				ZAI.hit(_weapon.getHitPower(),_weapon.getStunChance());
			}
		}

		if(!_weapon.Penetrating){
			Destroy (gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		_timePassed +=Time.deltaTime;
		if(_timePassed > 10.0f){
			Destroy(this.gameObject);
		}
		if(_enabled){
			if(_target == null){
				Destroy (this.gameObject);
			}
			else{
				_transform.position = Vector2.MoveTowards(_transform.position, _target.position, Time.smoothDeltaTime*_speed*10);

			}
		}
	}
}
