using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	protected int int_difficulty = 0;

	public int Int_difficulty{
		get
		{
			return int_difficulty;
		}
		set{
			int_difficulty = value;
		}
	}
}
