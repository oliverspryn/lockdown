using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	// If false, we're in offline mode (no networking) - local multiplayer only
	public bool networkingOn = false;
	public bool isServer = false;

	private string gameTypeName = "edu.gcc.Lockdown.Lockdown";
	public string gameName = "Lockdown-NetworkTest1";
	public string gameComment = "Change gameName if you don't want to join the same game as other team members.";

	public bool useAWSserver = false;
	public string AWS_URL; // set in GUI

	private bool refreshingHostList = false;
	private HostData[] hostData;

	// Handles to network-synchronized game objects (drag and drop in GUI)
	public GameObject maze;

	// Use this for initialization
	void Start () {
		if(useAWSserver)
		{
			MasterServer.ipAddress = AWS_URL;
			MasterServer.port = 23466;
			Network.natFacilitatorIP = AWS_URL;
			Network.natFacilitatorPort = 50005;
		}

		if(networkingOn) // networked mode
		{
			if(isServer)
			{
				Debug.Log ("*** SERVER MODE ***: Starting Server.");
				startServer();
			}
			else
			{
				Debug.Log ("*** CLIENT MODE ***: Looking for a host...");
				refreshHostList();
			}
		}
		else // offline mode
			offlineSpawnNetworkedObjects();
	}

	// Host a server and register it to the Master Server
	void startServer()
	{
		Network.InitializeServer(2, 25001, !Network.HavePublicAddress());
		MasterServer.RegisterHost(gameTypeName, gameName, gameComment);
	}

	// Get the list of hosts from the Master Server
	void refreshHostList()
	{
		MasterServer.RequestHostList(gameTypeName);
		refreshingHostList = true;
		Debug.Log ("Getting host list...");
	}

	// Spawn all network-synchronized objects
	void spawnNetworkedObjects()
	{
		// TODO: this is where we instantiate and/or set up network-synchronized objects in the scene
		// (for now, just the maze)
		// Note: if an object has a NetworkView attached directly to it, it needs to be initialized at
		// runtime, using Network.Initialize(), here.

		if(Network.isServer)
		{
			LOneMazeManager mazeScript = maze.GetComponent<LOneMazeManager>();
			mazeScript.Init();
		}
	}

	// Offline version of the above - spawns objects that would be networked if we were in online mode
	void offlineSpawnNetworkedObjects()
	{
		// In networked mode, this happens in:
		//		Server: spawnNetworkedObjects()
		// 		Client: InitMaze(), which is RPC called by OnPlayerConnected()
		LOneMazeManager mazeScript = maze.GetComponent<LOneMazeManager>();
		mazeScript.Init();
	}

	// *** Networking messages ***
	void OnServerInitialized()
	{
		Debug.Log ("Server initialized.");
		spawnNetworkedObjects();
	}

	void OnConnectedToServer()
	{
		Debug.Log ("Connected to server.");
		spawnNetworkedObjects();
	}

	void OnMasterServerEvent(MasterServerEvent mse)
	{
		if(mse == MasterServerEvent.RegistrationSucceeded)
			Debug.Log ("Server registered.");
	}

	// for RPCs that the server needs to make on the client when it connects
	void OnPlayerConnected(NetworkPlayer player)
	{
		// Get the maze seed we (the server) generated locally
		LOneMazeManager mazeScript = maze.GetComponent<LOneMazeManager>();

		networkView.RPC ("InitMaze", player, mazeScript.Seed);
	}

	// ** Clean up networked game objects (maybe these are only for "directly observed" objects? not sure) **
	void OnPlayerDisconnected(NetworkPlayer player)
	{
		Network.RemoveRPCs (player);
		Network.DestroyPlayerObjects(player);
	}

	void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		Network.RemoveRPCs (Network.player);
		Network.DestroyPlayerObjects(Network.player);
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

				// TODO: implement a better way to choose a host from the list of available games
				// (right now, it's hardcoded to connect to the first available game)
				Network.Connect(hostData[0]);
			}
		}
	}

	// To be called by on the clients by the server when they connect -
	// this will build the client's maze using the server's seed.
	[RPC]
	void InitMaze(int seed)
	{
		LOneMazeManager mazeScript = maze.GetComponent<LOneMazeManager>();
		mazeScript.Init (seed);
	}
}
