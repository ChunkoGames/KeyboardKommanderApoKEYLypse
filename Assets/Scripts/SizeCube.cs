using UnityEngine;
using System.Collections;

public class SizeCube : MonoBehaviour {
	GameObject parent3DText;
	// Use this for initialization
	void Start () {
		parent3DText = this.transform.parent.gameObject;
	}

	void Update(){
		Vector3 parentDimensions = parent3DText.renderer.bounds.size;
		this.transform.localScale = new Vector3(parentDimensions.x,0.0f,parentDimensions.y);
	}
}
