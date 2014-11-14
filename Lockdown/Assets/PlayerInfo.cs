using UnityEngine;
using System.Collections;

public class PlayerInfo : MonoBehaviour {

	public int items = 0;

	GameObject itemsDisplay;
	string layerString;
	// Use this for initialization
	void Start () {
		
		if(gameObject.tag == "Player 1")
			itemsDisplay = Instantiate(Resources.Load ("KeyPicP") as GameObject) as GameObject;
		else if(gameObject.tag == "Player 2")
			itemsDisplay = Instantiate(Resources.Load ("HammerPicP") as GameObject) as GameObject;
		else if(gameObject.tag == "Player 3")
			itemsDisplay = Instantiate(Resources.Load ("FloppyPicP") as GameObject) as GameObject;

		itemsDisplay.guiText.text = "" + items;
			
	}
	
	// Update is called once per frame
	void Update () {
		itemsDisplay.guiText.text = "" + items;
	}
}
