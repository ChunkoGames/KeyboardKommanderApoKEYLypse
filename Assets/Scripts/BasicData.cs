using UnityEngine;
using System.Collections;
/// <summary>
/// Basic data. Manages all the properties specific to this level including
/// Difficulty and GUI elements
/// </summary>
public class BasicData : MonoBehaviour {
	public GameObject mainCamera;
	// Use this for initialization
	void Start () {
		//Get the mainCamera
		mainCamera = GameObject.Find ("Main Camera");
		//Set the gui colors to black
		mainCamera.SendMessage("setBlack");
	}
}
