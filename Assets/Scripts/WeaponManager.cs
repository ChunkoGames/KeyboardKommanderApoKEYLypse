//KKA stands for Keyboard Kommander Apokeylypse
#define KKA

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponManager : ScriptableObject{
	private List<Weapon> weaponList;
	private int currentindex; //index of weapon currently being used

	private Weapon _currentWeapon;
	// Use this for initialization
	public WeaponManager () {
		weaponList = new List<Weapon>();
#if KKA
		Weapon w = ScriptableObject.CreateInstance<Weapon>(); //The player's only weapon for this version of the game
		w.Penetrating = true;
		weaponList.Add(w); //add the only weapon
#else
		ReadeWeapons();
#endif
		currentindex = 0;
		_currentWeapon = weaponList[currentindex];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Gets the weapon.
	/// </summary>
	/// <returns>The weapon.</returns>
	public Weapon getWeapon(){
		return _currentWeapon;
	}

	/// <summary>
	/// Sets the weapon. Use the index to determine which weapon is needed
	/// </summary>
	/// <param name="index">Index.</param>
	public void setWeapon(int index){
		currentindex =index;
		_currentWeapon = weaponList[currentindex];
	}

	/// <summary>
	/// Adds the weapon.
	/// </summary>
	public void addWeapon(){

	}

	/// <summary>
	/// Reads the weapons. Reads all the weapons from player preferences
	/// </summary>
	void ReadWeapons(){

	}
}
