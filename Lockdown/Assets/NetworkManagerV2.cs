using UnityEngine;
using System.Collections;

public class NetworkManagerV2 : MonoBehaviour {

	LockdownGlobals globals = LockdownGlobals.Instance;

	public bool useAWSserver = false;
	public string AWS_URL; // set in GUI

	private string gameTypeName = "edu.gcc.Lockdown.Lockdown";
	private string gameComment = "I can make no comment at this time.";
	
	private bool refreshingHostList = false;
	private HostData[] hostData;

	void Awake() {
		// The network manager will be instantiated at the beginning of the game and persist through all levels
		DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start () {
		if(useAWSserver)
		{
			MasterServer.ipAddress = AWS_URL;
			MasterServer.port = 23466;
			Network.natFacilitatorIP = AWS_URL;
			Network.natFacilitatorPort = 50005;
		}
	}

	// This will be called from the menu code when the player chooses to start a server.
	// 	Parameter: string gameName - will correspond to a sequence of Square/Triangle/Circle/X buttons
	void startServer(string gameName)
	{
		Debug.Log ("*** SERVER MODE ***: Starting Server.");

		Network.InitializeServer(2, 25001, !Network.HavePublicAddress());
		MasterServer.RegisterHost(gameTypeName, gameName, gameComment);
	}

	// This will be called from the menu code when the player chooses to join an existing game.
	// 	Parameter: string gameName - will correspond to a sequence of Square/Triangle/Circle/X buttons
	// This function will keep looping until it succeeds in connecting.
	void connectToServer(string gameName)
	{
		Debug.Log ("*** CLIENT MODE ***: Looking for a host...");
		MasterServer.RequestHostList(gameTypeName);
		refreshingHostList = true;
		Debug.Log ("Getting host list...");
	}

	// Update is called once per frame
	void Update () {
		// If we've started to look for a server (host), look every frame until we have one.
		if(refreshingHostList)
		{
			if(MasterServer.PollHostList().Length > 0) // We have a host!
			{
				refreshingHostList = false;
				hostData = MasterServer.PollHostList();
				
				// Connect to the first available server (the game name is chosen so that there should
				// only ever be one)
				Network.Connect(hostData[0]);
			}
		}
	}
}
