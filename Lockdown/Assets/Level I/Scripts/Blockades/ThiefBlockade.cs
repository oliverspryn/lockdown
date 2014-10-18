using UnityEngine;

public class ThiefBlockade : MonoBehaviour, IBlockade {
	#region Fields

/// <summary>
/// An array of objects to destroy when the blockade is opening.
/// </summary>
	public GameObject[] Dest;

/// <summary>
/// The door to open to allow passage.
/// </summary>
	public GameObject Door;

	#endregion

	public void Open() {
	//Open the door
		Door.transform.position = new Vector3(5.554642f, -7.199772f, -1.363853f);
		Door.transform.Rotate(0.0f, 180.0f, 0.0f);

	//Destory stuff
		for(int i = 0; i < Dest.Length; ++i) {
			Destroy(Dest[i]);
		}
	}
}