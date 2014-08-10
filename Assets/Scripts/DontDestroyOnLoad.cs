/// <summary>
/// Prevents this gameobject from being destroyed when a new scene is loaded
/// </summary>
using UnityEngine;
using System.Collections;

public class DontDestroyOnLoad : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(transform.gameObject);
	}
}
