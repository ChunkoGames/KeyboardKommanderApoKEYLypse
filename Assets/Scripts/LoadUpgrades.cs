using UnityEngine;
using System.Collections;
/// <summary>
/// If there is no upgrades score prefab load it on awake. Do the same for Dictionary
/// </summary>
public class LoadUpgrades : MonoBehaviour {

	/// <summary>
	/// Awake this instance. On awakening load an upgrades score prefab if there isn't one there already.
	/// </summary>
	void Awake(){
		Debug.Log ("Awake");
		if((GameObject.FindGameObjectsWithTag("Upgrades")).Length == 0){//If no gameobjects with the tag upgrades can be found then we need to create one
			Debug.LogWarning("Loading Upgrades");
			Resources.Load ("Upgrades-Score");
		}
	}
}
