/// <summary>
/// Dictionary. This class is responsible for reading in and containing the 60,000+ words which will appear in this game.
/// This class has functionality to assign words for the many zombies in this game.
/// </summary>
using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class Dictionary : MonoBehaviour {
	/// sourceFile and reader are needed for accessing the classes we need to read in the data
	protected FileInfo sourceFile = null;
	protected StreamReader reader = null;
	protected string text = " ";

	//we have 69903 words
	//todo: set pagelengths to private
	private int[] pageLengths; //used to store the number of words on each page
	private ArrayList[] pages; //we want 11 pages so 0-10 0 is 2 characters, 10 is 12 or more
	private int difficultySetting = -1;

	// Use this for initialization
	void Start () {
		//Initialize 10 difficulty levels
		pages = new ArrayList[10];//initialize array of 11 arrraylits
		pageLengths = new int[10];

		for(int i = 0; i < 10; i++){
			pages[i] = new ArrayList();
			pageLengths[i] = -1; //set to -1 so we don't have to subtract 1 in pickword
		}


		sourceFile = new FileInfo("KeyboardKommanderApokeylypse_Data\\Dictionary.txt");//new FileInfo("Dictionary.txt");
		reader = sourceFile.OpenText ();

		//In order to initialize each letter in the alphabet we iterate through a string containing the entire alphabet
		//and insert each indivicual element into the pages array at an index correlating to the difficulty (determined by length)
		//of the string
		string alphabet = "abcdefghijklmnopqrstuvwxyz";

		for(int i =0; i <alphabet.Length-2; i++){
			pages[0].Add(alphabet.Substring(i,1));
			pageLengths[0]+=1;
		}

		//add the contents from the dictionary
		while (text != null){
			int difficulty;
			text = reader.ReadLine(); //read the line
			difficulty = calculateDifficulty(text); //find out the difficulty of this string (determined by length)
			pages[difficulty].Add(text); //add the element to an arraylist in the pages array
			pageLengths[difficulty] += 1; //increase the length of this entry

		} 
		
		//after everything is loaded print out one item frome ach page as well as the length of each page
#if printContents
		for(int i = 0; i < 10; i++){
			Debug.Log ("Length of page " + i + " :" + pageLengths[i]); //set to -1 so we don't have to subtract 1 in pickword
		}
#endif


	}

	/// <summary>
	/// Calculates the difficulty of this string.
	/// </summary>
	/// <returns>The difficulty.</returns> an integer between 0 and 9
	/// <param name="text">Text.</param>
	public int calculateDifficulty(string text){
		if(text == null || text == ""){
			text = "ashfbewnnxcyviufhg";
		}
		int _difficulty = text.Length - 1;
		if(_difficulty > 9){ //don't let the difficulty be larger than 9 (prevent out of bounds exception)
			_difficulty = 9;
		}
		return _difficulty;
	}

	/// <summary>
	/// Picks the word. Return one word from a specific difficulty level
	/// </summary>
	/// <returns>The word.</returns> a word of that difficulty level randomly selected
	/// <param name="difficulty">Difficulty.</param> How long the word should be 
	public string pickWord(int difficulty){
		try{
			int index;
			int maxLength = pageLengths[difficulty];
			index = (int) UnityEngine.Random.Range (0, maxLength);
			return (string)(pages[difficulty])[index];
		}
		catch{
			Debug.LogWarning("Dictionary line 69 difficulty:" + difficulty);
			return "pickWordFailed";
		}
	}

	/// <summary>
	/// Picks the words. Chooses several based off of how many you ask for
	/// The words are randomly chosen to be slightly more or less difficult
	/// than the difficulty level passed
	/// </summary>
	/// <returns>The words.</returns>
	/// <param name="difficulty">Difficulty.</param>
	/// <param name="howMany">How many.</param>
	public Stack pickWords(int difficulty, int howMany){
		if(difficultySetting == -1){
			difficultySetting = GameObject.FindGameObjectWithTag("Upgrades").GetComponent<Upgrades>().DifficultySetting;
		}
		Stack pickWordsList = new Stack(); //initializeStack
		for(int i = 0; i < howMany; i++){
			int rDifficulty = UnityEngine.Random.Range (difficulty-i,difficulty+ i); //randomized difficulty. Words become increasingly random towards the end of the stack


			rDifficulty += difficultySetting;
			//Make sure the difficulty is within the range of pages, if it's too low set it to 0, if it's too high set it to 9
			if(rDifficulty <0){
				rDifficulty = difficultySetting;
			}
			else if (rDifficulty>9){
				rDifficulty = 9;
			}
			pickWordsList.Push (pickWord(rDifficulty));//return 
		}
		return pickWordsList;
	}
}
