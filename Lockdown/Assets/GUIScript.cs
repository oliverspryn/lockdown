using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour {

	private string playerInputSuffix;
	private GameObject obstacle;
	private string layerString;
	private float speed;
	private GameObject[] buttons;
	private GameObject[] challenge;
	private int pos;

	// Use this for initialization
	void Start () {
		challenge = new GameObject[0];
		buttons = new GameObject[4];
		buttons[0] = Resources.Load ("Triangle") as GameObject;
		buttons[1] = Resources.Load ("Square") as GameObject;
		buttons[2] = Resources.Load ("Circle") as GameObject;
		buttons[3] = Resources.Load ("Ex") as GameObject;
		layerString = gameObject.tag + " GUI";
		pos = -1;
		//k.SetActive (true);
		//k.layer = LayerMask.NameToLayer(layerString);
		//k.transform.position = new Vector3(.95f,.2f,0f);
		//GameObject clone = Instantiate (k) as GameObject;
		//k.transform.position = new Vector3(.875f,.2f,0f);
		//Instantiate (k);
		//Destroy (clone, 1.0f);
		//k.layer = 9; //"Player2GUI";
		//k.layer = 10;//"Player3GUI";
		//beginSequence(10, .1f);

	}
	public void beginSequence(int presses, float sSpeed, GameObject Sobstacle)
	{
		obstacle = Sobstacle;
		pos = 0;
		speed = sSpeed;
		for(int i = 0; i < challenge.Length; ++i)
			Destroy(challenge[i], 1.0f); 
		challenge = new GameObject[presses];
		for (int i = 0; i < presses; ++i) 
		{
			challenge[i] = Instantiate (buttons[Random.Range(0,4)]) as GameObject;
			challenge[i].transform.position = new Vector3(1.05f+i*.075f,.2f,0f);
			challenge[i].layer = LayerMask.NameToLayer(layerString);
			challenge[i].guiTexture.color = Color.gray;
			challenge[i].SetActive(true);
		}
		playerInputSuffix = " P1"; // default to player 1
		if(gameObject.tag == "Player 1")
			playerInputSuffix = " P1";
		else if(gameObject.tag == "Player 2")
			playerInputSuffix = " P2";
		else if(gameObject.tag == "Player 3")
			playerInputSuffix = " P3";
	}
	// Update is called once per frame
	void Update () {
		if(pos < 0)
			return;
		if (pos >= challenge.Length)
		{
			processSuccess();
			for(int i = 0; i < challenge.Length; ++i)
				Destroy(challenge[i], 1.0f); 
			challenge = new GameObject[0];
			return;
		}
		else if(challenge[pos].transform.position.x <= 0)
			processFailure();
		if (Input.GetButtonDown ("X" + playerInputSuffix))
		    if("X" == challenge[pos].tag)
				challenge [pos++].guiTexture.color = Color.green;
			else
				processFailure();
		if (Input.GetButtonDown ("O" + playerInputSuffix))
			if("O" == challenge[pos].tag)
				challenge [pos++].guiTexture.color = Color.green;
			else
				processFailure();
		if (Input.GetButtonDown ("T" + playerInputSuffix))
			if("T" == challenge[pos].tag)
				challenge [pos++].guiTexture.color = Color.green;
			else
				processFailure();
		if (Input.GetButtonDown ("S" + playerInputSuffix))
			if("S" == challenge[pos].tag)
				challenge [pos++].guiTexture.color = Color.green;
			else
				processFailure();

		for(int i = 0; i < challenge.Length; ++i)
			challenge[i].transform.position = 
				new Vector3(challenge[i].transform.position.x - Time.deltaTime * speed,.2f,0f);
	}
	void processFailure()
	{
		pos = -1;
		for(int i = 0; i < challenge.Length; ++i)
			challenge[i].guiTexture.color = Color.red; 
		for(int i = 0; i < challenge.Length; ++i)
			Destroy(challenge[i], 1.0f); 
	}
	void processSuccess()
	{
		pos = -1;
		doDoorOpen(obstacle);
		networkView.RPC ("doDoorOpen", RPCMode.OthersBuffered, obstacle);
	}

	[RPC]
	void doDoorOpen(GameObject obstacle)
	{
		switch(obstacle.tag)
		{
		case "Thief":
			obstacle.GetComponent<ThiefBlockade>().Open ();
			break;
		case "Hacker":
			obstacle.GetComponent<HackerBlockade>().Open();
			break;
		case "Brute":
			obstacle.GetComponent<BruteBlockade>().Open ();
			break;
		}

		GetComponent<PlayerInfo>().items--;
		Destroy(obstacle.GetComponent<BoxCollider>());
	}
}
