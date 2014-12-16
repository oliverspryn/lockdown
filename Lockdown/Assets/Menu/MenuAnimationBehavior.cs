using UnityEngine;
using System.Collections;

public class MenuAnimationBehavior : MonoBehaviour {
	public Animator Model1;
	public Animator Model2;
	public Animator Model3;
	
	public int Controller;
	public int model;

	public int isClient;//0 for local, 1 for host, 2 for client

	Animator[] Models;
	string playerInputSuffix;

	// Use this for initialization
	void Start () {
		model = 3;
		//do stuff
		Models = new Animator[4];
		Models [0] = Model1;
		Models [1] = Model2;
		Models [2] = Model3;
		Models [3] = null;
		playerInputSuffix = " P" + Controller;
	}

	// Update is called once per frame
	void Update () {

		if((isClient == 1 && Controller == 3) || (isClient == 2 && Controller != 3))
		{
			return;
		}
		if(isClient == 2)
			playerInputSuffix = " P" + (Controller - 2);
		else 
			playerInputSuffix = " P" + Controller;
		/*
		if(model == 3)
		{
			Models[0].SetBool ("Selected", false);
			Models[1].SetBool ("Selected", false);
			Models[2].SetBool ("Selected", false);
		}
		*/
		if (Input.GetButtonDown ("LB" + playerInputSuffix))
		{
			//if(model != 3)
			//	Models[model].SetBool ("Selected", false);
			model--;
		}
		else if (Input.GetButtonDown ("RB" + playerInputSuffix))
		{
			//if(model != 3)
			//	Models[model].SetBool ("Selected", false);
			model++;
		}
		model = (model + 4) % 4;
		//if(model != 3)
			//Models[model].SetBool ("Selected", true);
		setModel (model);

		if(Network.connections.Length > 0)
			networkView.RPC ("setModel", RPCMode.OthersBuffered, model);
	}

	[RPC]
	void setModel(int mod)
	{
		model = mod;
		Models[0].SetBool ("Selected", false);
		Models[1].SetBool ("Selected", false);
		Models[2].SetBool ("Selected", false);
		if (model != 3)
			Models[model].SetBool ("Selected", true);

	}
}
