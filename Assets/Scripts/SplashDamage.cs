using UnityEngine;
using System.Collections;

public class SplashDamage : MonoBehaviour {

	/*private float _range;
	private float _damage;
	private bool _enabled;
	private ArrayList splashVictims; //List of gameobjects which will take splash damage

	public void setup(float splashDamage){
		_range = Mathf.Sqrt(splashDamage) + 2.0f;
		Debug.Log ("_range is:" + _range);
		_damage = splashDamage;
		GetComponent<CircleCollider2D>().radius = _range;
		splashVictims = new ArrayList();
		_enabled = true;
	}*/

	/*void OnCollisionEnter2D(Collision2D coll) {
		Debug.Log ("Splash Damage: collision enter");
		if (_enabled && coll.gameObject.tag == "Zombie"){
			Debug.Log ("Zombie Added");
			splashVictims.Add (coll.gameObject.GetComponent<ZombieAI>());
		}
		
	}

	void OnCollisionExit2D(Collision2D coll){
		if (_enabled && coll.gameObject.tag == "Zombie"){
			Debug.Log ("Zombie Removed");
			splashVictims.Remove (coll.gameObject.GetComponent<ZombieAI>());
		}
	}*/
}
