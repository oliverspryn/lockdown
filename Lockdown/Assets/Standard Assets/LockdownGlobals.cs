using UnityEngine;
using System.Collections;

// A class for various stuff that needs to be accessible game-wide
public class LockdownGlobals : Singleton<LockdownGlobals>
{
	protected LockdownGlobals() { } // this is a Singleton - make sure the constructor is never called
	
	public bool networkingOn = false;
	public bool isServer = false;
}