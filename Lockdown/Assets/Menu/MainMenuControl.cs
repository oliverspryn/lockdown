using UnityEngine;
using System.Collections;

public class MainMenuControl : MonoBehaviour {

	public GameObject optionsObj;
	public Camera C1;
	public Camera C2;
	public Camera C3;

	TextMesh[] options;
	int num;
	int cur;
	// Use this for initialization
	void Start () {
		cur = 0;
		num = optionsObj.transform.childCount;
		options = new TextMesh[num];
		foreach (Transform child in optionsObj.transform)
			options[cur++] = child.GetComponent<TextMesh>();
		cur = 0;
		options [cur].color = Color.red;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("LB P1"))
		{
			options[cur].color = Color.white;
			cur--;
		}
		else if (Input.GetButtonDown ("RB P1"))
		{
			options[cur].color = Color.white;
			cur++;
		}
		cur = (cur + num) % num;
		options [cur].color = Color.red;

		if (Input.GetButtonDown ("X P1"))
		{
			//TODO: fill out below.
			//perform actions associated with selected option
			if(options[cur].name == "New Local Game"){
				;
			} else if(options[cur].name == "Load Level 2"){
				;
			} else if(options[cur].name == "Load Level 3"){
				;
			} else if(options[cur].name == "Host Game 1"){
				;
			} else if(options[cur].name == "Host Game 2"){
				;
			} else if(options[cur].name == "Join Game 1"){
				;
			} else if(options[cur].name == "Join Game 2"){
				;
			}

		}

	}
}
