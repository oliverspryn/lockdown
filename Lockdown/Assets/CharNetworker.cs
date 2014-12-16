using UnityEngine;
using System.Collections;

public class CharNetworker : MonoBehaviour {
	public Camera c1;
	public Camera c2;
	public Camera c3;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void callSetModel (int mod)
	{
		networkView.RPC ("setModel", RPCMode.AllBuffered, mod);
	}

	[RPC]
	void setModel(int mod)
	{
		/*
		if(Controller != mod / 4)
			return;
		*/
		int k = mod/4;
		if(k == 1)
			c1.GetComponent<MenuAnimationBehavior>().setTheModel(mod%4);
		if(k == 2)
			c2.GetComponent<MenuAnimationBehavior>().setTheModel(mod%4);
		if(k == 3)
			c3.GetComponent<MenuAnimationBehavior>().setTheModel(mod%4);

		//model = mod % 4;

		
	}
}
