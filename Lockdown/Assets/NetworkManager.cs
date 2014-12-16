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
	
	// Handles to network-synchronized game objects within the hierarchy (not initialized by NetworkManager)
	public GameObject maze;
	public GameObject[] Blockades;
	
	// Prefabs for network-synchronized objects initialized by the NetworkManager
	public GameObject P1Controller, P2Controller, P3Controller, P4Controller;
	public GameObject BlockadeManager;
	
	// Handles to placeholders in the hierarchy for objects instantiated by the NetworkManager
	public GameObject P1Placeholder, P2Placeholder, P3Placeholder, P4Placeholder;
	
	// Objects that the manager has instantiated at runtime - handles for later runtime use
	private GameObject player1, player2, player3, player4;
	private BlockadeManager blockadeMgr;

	void Awake()
	{
		// See comments in MouseLook.cs or FPSInputController.js to explain
		// this utter bizarreness
		/*GameObject netOnOffFoobarThing = (GameObject)Object.Instantiate(new GameObject("Network OnOff Foobar Thing"));
		if(networkingOn)
			netOnOffFoobarThing.SetActive (true);
		else
			netOnOffFoobarThing.SetActive (false);*/
	}
	
	// Use this for initialization
	public void Init () {
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
		
		if(Network.isServer)
		{
			//if(Application.loadedLevel == 0) // Level 1
			//{
				// Initialize maze
			if(maze != null) {
				LOneMazeManager mazeScript = maze.GetComponent<LOneMazeManager>();
				mazeScript.Init();
			}

				// Initialize blockade manager with blockades from maze + hallway
				GameObject blockadeMgrObj = (GameObject)Network.Instantiate (BlockadeManager, Vector3.zero, Quaternion.identity, 0);
				blockadeMgr = blockadeMgrObj.GetComponent<BlockadeManager>();
				blockadeMgr.Blockades = Blockades;
				blockadeMgr.MazeManager = maze;
				blockadeMgr.Init();
			//}
			
			// Since this is the server, we will have control of players 1 and 2.
			player1 = (GameObject)Network.Instantiate(P1Controller, P1Placeholder.gameObject.transform.position, P1Placeholder.gameObject.transform.rotation, 0);
			player2 = (GameObject)Network.Instantiate(P2Controller, P2Placeholder.gameObject.transform.position, P2Placeholder.gameObject.transform.rotation, 0);
			// Activate P1 and P2's cameras
			player1.gameObject.transform.Find("Main Camera").gameObject.SetActive(true);
			player2.gameObject.transform.Find("Main Camera").gameObject.SetActive(true);
			
		}
		else // we are the client
		{
			// Since this is the client, we will have control of players 3 and 4.
			player3 = (GameObject)Network.Instantiate(P3Controller, P3Placeholder.gameObject.transform.position, P3Placeholder.gameObject.transform.rotation, 0);
			//player4 = (GameObject)Network.Instantiate(P4Controller, P4Placeholder.gameObject.transform.position, P4Placeholder.gameObject.transform.rotation, 0);
			// Activate P3 and P4's cameras
			player3.gameObject.transform.Find("Main Camera").gameObject.SetActive(true);
			//player4.gameObject.transform.Find("Main Camera").gameObject.SetActive(true);
		}
	}
	
	// Offline version of the above - spawns objects that would be networked if we were in online mode
	void offlineSpawnNetworkedObjects()
	{
		//if(Application.loadedLevel == 0) // Level 1
		//{
			// In networked mode, this happens in:
			//		Server: spawnNetworkedObjects()
			// 		Client: InitMaze(), which is RPC called by OnPlayerConnected()
		if(maze != null) {
			LOneMazeManager mazeScript = maze.GetComponent<LOneMazeManager>();
			mazeScript.Init();
		}

			// Initialize blockade manager with blockades from maze + hallway
			GameObject blockadeMgrObj = (GameObject)Object.Instantiate (BlockadeManager, Vector3.zero, Quaternion.identity);
			blockadeMgr = blockadeMgrObj.GetComponent<BlockadeManager>();
			blockadeMgr.Blockades = Blockades;
			blockadeMgr.MazeManager = maze;
			blockadeMgr.Init();
		//}
		
		// Spawn only players 1 and 2 in offline mode (later, we'll be using 4-way split screen)
		player1 = (GameObject)Object.Instantiate(P1Controller, P1Placeholder.gameObject.transform.position, P1Placeholder.gameObject.transform.rotation);
		player2 = (GameObject)Object.Instantiate(P2Controller, P2Placeholder.gameObject.transform.position, P2Placeholder.gameObject.transform.rotation);
		// Activate P1 and P2's cameras
		player1.gameObject.transform.Find("Main Camera").gameObject.SetActive(true);
		player2.gameObject.transform.Find("Main Camera").gameObject.SetActive(true);
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
		//if(Application.loadedLevel == 0) // Level 1
		//{
			// Get the maze seed we (the server) generated locally
		if(maze != null) {
			LOneMazeManager mazeScript = maze.GetComponent<LOneMazeManager>();
			networkView.RPC("InitMaze", player, mazeScript.Seed);
		} else {
			networkView.RPC("InitBlockadeDoorThings", player);
		}
		//}
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

	[RPC]
	void InitBlockadeDoorThings() {
		GameObject blockadeMgrObj = GameObject.FindGameObjectWithTag("BlockadeManager");
		blockadeMgr = blockadeMgrObj.GetComponent<BlockadeManager>();
		blockadeMgr.Blockades = Blockades;
		blockadeMgr.MazeManager = maze;
		blockadeMgr.Init();
	}

	// To be called by on the clients by the server when they connect -
	// this will build the client's maze using the server's seed.
	[RPC]
	void InitMaze(int seed)
	{
		// Initialize the maze using the seed sent by the server
		if(maze != null) {
			LOneMazeManager mazeScript = maze.GetComponent<LOneMazeManager>();
			mazeScript.Init(seed);
		}

		// Initialize blockade manager with blockades from maze + hallway
		GameObject blockadeMgrObj = GameObject.FindGameObjectWithTag ("BlockadeManager");
		blockadeMgr = blockadeMgrObj.GetComponent<BlockadeManager>();
		blockadeMgr.Blockades = Blockades;
		blockadeMgr.MazeManager = maze;
		blockadeMgr.Init();
	}
}
