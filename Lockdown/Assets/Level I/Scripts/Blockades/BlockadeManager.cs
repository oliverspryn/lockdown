using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This class provides and interface to manage all of the
/// <c>Blockade</c> objects which may exist in a level, and send
/// their open or closed state across a network.
/// </summary>
public class BlockadeManager : MonoBehaviour {
	#region Fields

/// <summary>
/// An array of <c>Blockade</c> objects.
/// </summary>
	public GameObject[] Blockades;

/// <summary>
/// A Maze Manager object which will contain a series of <c>Maze</c> objects
/// which will contain a list of 
/// </summary>
	public GameObject MazeManager = null;

	#endregion

	#region Private Members

/// <summary>
/// The master array of <c>Blockade</c> objects which were either manually
/// passed in by the user, or fetched from the maze.
/// </summary>
	private List<Blockade> Manager;

	#endregion

	#region Constructors

/// <summary>
/// Collect all of the <c>Blockade</c> objects into a centralized, master
/// array.
/// </summary>
	public void Init () {
		Manager = new List<Blockade>();

	//Put all of the manually defined blockades into the master array
		foreach(GameObject b in Blockades) {
			Manager.Add(b.GetComponent<Blockade>());
		}

	//Now, go search the maze
		if(MazeManager != null) {
			LOneMazeManager maze = MazeManager.GetComponent<LOneMazeManager>();

			GameObject[][] mazeBlockades = {
				maze.Left.Script.Blockades,
				maze.Center.Script.Blockades,
				maze.Right.Script.Blockades
			};

			foreach(GameObject[] mb in mazeBlockades) {
				foreach(GameObject b in mb) {
					Manager.Add(b.GetComponent<Blockade>());
				}
			}		
		}

	//Now assign a network ID to each blockade
		for(int i = 0; i < Manager.Count; ++i) {
			Manager[i].NetID = i;
			Manager[i].Opened += BlockadeManager_Opened;
		}
	}

	#endregion

	#region Public Methods

/// <summary>
/// Open a <c>Blockade</c> based on its network ID.
/// </summary>
/// 
/// <param name="netID">The network ID of the blockade to open</param>
	[RPC]
	public void Open(int netID) {
		Manager[netID].Open();
	}

	public void BlockadeManager_Opened(int netID) {
		if(networkView != null) networkView.RPC("Open", RPCMode.OthersBuffered, netID);
	}

	#endregion
}