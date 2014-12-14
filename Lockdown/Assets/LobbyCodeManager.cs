using UnityEngine;
using System.Collections;

public class LobbyCodeManager : MonoBehaviour {

	public Camera parentMenu;
	public GameObject optionsObj;
	public Camera charSelect1;
	public Camera charSelect2;
	public Camera charSelect3;

	private GameObject[] buttons;
	private GameObject[] challenge;
	private int pos;
	private bool entering;
	private string code;
	private string layerString;

	float lastDir;
	TextMesh[] options;
	int num;
	int cur;
	// Use this for initialization
	void Start () {
		lastDir = 0f;
		cur = 0;
		num = optionsObj.transform.childCount;
		options = new TextMesh[num];
		foreach (Transform child in optionsObj.transform)
			options[cur++] = child.GetComponent<TextMesh>();
		cur = 0;
		if(options.Length > 0)
			options [cur].color = Color.red;
	}

	// Use this for initialization
	void Awake () {
		entering = false;
		challenge = new GameObject[0];
		buttons = new GameObject[4];
		buttons[0] = Resources.Load ("Triangle") as GameObject;
		buttons[1] = Resources.Load ("Square") as GameObject;
		buttons[2] = Resources.Load ("Circle") as GameObject;
		buttons[3] = Resources.Load ("Ex") as GameObject;
		layerString = "Lobby GUI";
		pos = -1;
	}

	public void showStaticSequence (int presses)
	{
		code = "";
		int rand;
		for(int i = 0; i < challenge.Length; ++i)
			Destroy(challenge[i], 1.0f); 
		challenge = new GameObject[presses];
		for (int i = 0; i < presses; ++i) 
		{
			rand = Random.Range(0,4);
			code += "" + rand;
			challenge[i] = Instantiate (buttons[rand]) as GameObject;
			challenge[i].transform.position = new Vector3(0.1f+i*.075f,.4f,0f);
			challenge[i].transform.localScale = new Vector3(-.05f,-.05f,1f);
			challenge[i].layer = LayerMask.NameToLayer(layerString);
			challenge[i].guiTexture.color = new Color(.0f, .7f, .8f);
			challenge[i].SetActive(true);
		}
		// create a server with code
		// make sure to check for no collisions
		//code;
	}
	
	public void enterStaticSequence (int presses)
	{
		entering = true;
		pos = 0;
		code = "";
		for(int i = 0; i < challenge.Length; ++i)
			Destroy(challenge[i], 1.0f); 
		challenge = new GameObject[presses];
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Start P1"))
		{
			for(int i = 0; i < challenge.Length; ++i)
				Destroy(challenge[i], 1.0f); 
			parentMenu.gameObject.SetActive(true);
			gameObject.SetActive(false);
		}
		if(options.Length > 0)//host game or local game (not client)
		{
			if (Input.GetAxisRaw("Vertical P1") > 0 && lastDir <= 0)//Input.GetButtonDown ("LB P1"))
			{
				options[cur].color = Color.white;
				cur--;
			}
			else if (Input.GetAxisRaw("Vertical P1") < 0 && lastDir >= 0)//Input.GetButtonDown ("RB P1"))
			{
				options[cur].color = Color.white;
				cur++;
			}
			
			lastDir = Input.GetAxisRaw ("Vertical P1");
			cur = (cur + num) % num;
			options [cur].color = Color.red;

			if (Input.GetButtonDown ("X P1"))
			{
				int num1 = charSelect1.GetComponent<MenuAnimationBehavior>().model+1;
				int num2 = charSelect2.GetComponent<MenuAnimationBehavior>().model+1;
				int num3 = charSelect3.GetComponent<MenuAnimationBehavior>().model+1;

				//verify the characters are appropriately selected
				if(num1*num2*num3 == 6)//a 1, 2, and 3
				{
					//TODO: perform actions associated with selected option

					if(options[cur].name == "New Game"){

					} else if(options[cur].name == "Load Level 2"){

					} else if(options[cur].name == "Load Level 3"){

					}else if(options[cur].name == "New Local Game"){
						
					} else if(options[cur].name == "Load Local Level 2"){
						
					} else if(options[cur].name == "Load Local Level 3"){
						
					}
					gameObject.SetActive(false);
				}
				
			}
		}
		if(!entering)
		{
			//code about game selection
			return;
		}
		if(pos >= challenge.Length)
		{
			//try to find the server with code

			//if it fails
			for(int i = 0; i < challenge.Length; ++i)
				Destroy(challenge[i], 1.0f); 
			pos = 0;
		}
		if (Input.GetButtonDown ("X P1"))
		{
			challenge[pos++] = Instantiate (buttons[3]) as GameObject;
			code += "3";
		}
		else if (Input.GetButtonDown ("O P1"))
		{
			challenge[pos++] = Instantiate (buttons[2]) as GameObject;
			code += "2";
		}
		else if (Input.GetButtonDown ("T P1"))
		{
			challenge[pos++] = Instantiate (buttons[0]) as GameObject;
			code += "0";
		}
		else if (Input.GetButtonDown ("S P1"))
		{
			challenge[pos++] = Instantiate (buttons[1]) as GameObject;
			code += "1";
		}
		else
			return;
		challenge[pos-1].transform.position = new Vector3(0.1f+(pos-1)*.075f,.4f,0f);
		challenge[pos-1].transform.localScale = new Vector3(-.05f,-.05f,1f);
		challenge[pos-1].layer = LayerMask.NameToLayer(layerString);
		challenge[pos-1].guiTexture.color = new Color(.0f, .7f, .8f);
		challenge[pos-1].SetActive(true);
		
	}
}
