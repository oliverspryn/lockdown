﻿using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	private bool Transable = false;
	private string Next = string.Empty;
	public bool AutoStart = false;

	void Awake() {
		// Make sure the level manager itself survives all level transitions
		//DontDestroyOnLoad(this.transform.gameObject);
	}

/// <summary>
/// Initialize the Network Manager on the first level.
/// </summary>
	public void Start() {
		GameObject netMgr = GameObject.FindGameObjectWithTag ("Network Manager");
		if(AutoStart && netMgr != null) RestoreNetMgrState(netMgr);

	}

	public void Update() {
		if(Transable) {
			TransitionLevel(Next);
			Transable = false;
		}
	}

	[RPC]
	public void Trans(string level) {
		Transable = true;
		Next = level;
	}

	// Use this function for any level transitions in the game.
	// It presents the same interface as Application.LoadLevel() (pass the name of the level to be loaded),
	// but performs any necessary setup/transition tasks needed to ensure game continuity.
	public void TransitionLevel(string newLevelName)
	{
		// Basic format for switch cases:
		// 		*Clean up old level (scoring, deal with stranded players, close elevator doors, etc.)
		//		*Call Application.LoadLevel() to perform the actual switch (removes all non-persistent
		//			objects, loads all objects from new level)
		//		*Restore state (NetworkManager, etc.)
		//		*Set up new level

		if(!Transable) {
			if(Network.isServer) {
				networkView.RPC("Trans", RPCMode.OthersBuffered, newLevelName);
			} else {
				Next = newLevelName;
				return;
			}
		}

		switch(newLevelName)
		{
		case "Level 0":
			Application.LoadLevel ("Level I");
			break;

		case "Level I":
		case "Level 1": // old typos never die...but we can patch over them! :-D
			Application.LoadLevel ("Level I");
			break;

		case "Level 2":
			Application.LoadLevel ("Level 2");
			break;

		case "Level 3":
			Application.LoadLevel ("Level 3");
			break;

		case "FinalScene":
			Application.LoadLevel ("FinalScene");
			break;

		default:
			throw new Lockdown_LevelNotFoundException(
				string.Format ("Level '{0}' not recognized by the LevelManager. If you added a new level, did you forget to add it to LevelManager.cs?", newLevelName));
		}

		Transable = false;
	}

	void RestoreNetMgrState(GameObject newNetMgrGameObj) {
		NetworkManager newNetMgr = newNetMgrGameObj.GetComponent<NetworkManager>();

		newNetMgr.AWS_URL = LockdownGlobals.Instance.AWSServer;
		newNetMgr.gameName = LockdownGlobals.Instance.GameName;
		newNetMgr.isServer = LockdownGlobals.Instance.Host == Host.Server;
		newNetMgr.networkingOn = LockdownGlobals.Instance.NetworkingEnabled;
		newNetMgr.useAWSserver = LockdownGlobals.Instance.AWSServerEnabled;
		newNetMgr.Init();
	}

	public void JoinGame(string GameName) {
		LockdownGlobals.Instance.GameName = GameName;
		LockdownGlobals.Instance.Host = Host.Client;

		GameObject netMgr = GameObject.FindGameObjectWithTag("Network Manager");
		if(netMgr != null)
			RestoreNetMgrState(netMgr);
	}

	public void HostGame(string GameName) {
		LockdownGlobals.Instance.GameName = GameName;
		LockdownGlobals.Instance.Host = Host.Server;

		GameObject netMgr = GameObject.FindGameObjectWithTag("Network Manager");
		if(netMgr != null)
			RestoreNetMgrState(netMgr);
	}

	public void StartGame(string LevelName) {
		TransitionLevel(LevelName);
	}

	public void OnLevelWasLoaded() {
		GameObject netMgr = GameObject.FindGameObjectWithTag ("Network Manager");
		RestoreNetMgrState(netMgr);
	}

	#region Exception Types

	public class Lockdown_LevelNotFoundException : System.Exception
	{
		// Constructor - pass the error message associated with this exception
		public Lockdown_LevelNotFoundException(string msg)
		{
			Message = msg;
		}

		public new string Message
		{ get; private set; }
	}

	#endregion
}