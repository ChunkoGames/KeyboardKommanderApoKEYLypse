using UnityEngine;
using System.Collections;

/// <summary>
/// Zombie. Controls one single zombie. Used by Zombie Manager. Various monsters will
/// extend this class. This holds an arraylist of words if any of them are typed the zombie
/// will take damage
/// </summary>
public class Zombie: MonoBehaviour {
	// Use this for initialization
	void Start () {
	}

	/// <summary>
	/// Check to see if this zombie is still "alive", if not delete it.
	/// </summary>
	/// <returns>True - still alive, false - dead</returns>
	public bool StatusCheck(){
		return true;
	}





}
