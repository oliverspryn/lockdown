using UnityEngine;
using System.Collections;

public class WallRiser : MonoBehaviour {
	public GameObject Maze;

	public void OnTriggerEnter(Collider c) {
		LOneMaze m = Maze.GetComponent<LOneMaze>();
		m.EnableSliding = true;
	}
}