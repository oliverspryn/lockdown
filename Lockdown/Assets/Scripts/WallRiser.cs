using UnityEngine;
using System.Collections;

public class WallRiser : MonoBehaviour {
	public GameObject Maze;

	public void OnTriggerEnter(Collider c) {
		Maze m = Maze.GetComponent<Maze>();
		m.EnableSliding = true;
	}
}