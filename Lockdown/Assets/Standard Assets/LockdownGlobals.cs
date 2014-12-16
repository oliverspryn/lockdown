using UnityEngine;
using System.Collections;

/// <summary>
/// Whether or not this player is the client or server.
/// </summary>
public enum Host {
	Server = 0,
	Client
}

/// <summary>
/// A class for various stuff that needs to be accessible game-wide.
/// </summary>
public class LockdownGlobals : Singleton<LockdownGlobals> {
/// <summary>
/// Do not instantiate.
/// </summary>
	protected LockdownGlobals() { }

/// <summary>
/// The URL or IP address of the associated AWS server.
/// </summary>
	public string AWSServer = "54.69.3.209";

/// <summary>
/// Whether or not to use the AWS server.
/// </summary>
	public bool AWSServerEnabled = true;

/// <summary>
/// Whether or not this person is the client or the host.
/// </summary>
	public Host Host = Host.Server;

/// <summary>
/// The name of the game as registered on the networking server.
/// </summary>
	public string GameName = "Lockdown Alpha";

/// <summary>
/// Whether or not the networking is enabled.
/// </summary>
	public bool NetworkingEnabled = true;

/// <summary>
/// Indicates which characters have been selected by each player
/// </summary>
	public int[] characters = new int[3];

/// <summary>
/// Indicates which players are located on which machines:
/// 	0 for server, 1 for first client, 2 for second client (only in three-way networked mode)
/// Note that right now we're only supporting 3-way local and 2-player server/1-player server modes,
/// but this array has planned ahead. :-)
/// </summary>
	public int[] playerMachines = new int[3];
}