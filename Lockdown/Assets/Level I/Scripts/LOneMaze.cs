using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Generate and populate a maze which is specific to level one
/// for this game.
/// </summary>
public class LOneMaze : Maze {
	#region Fields

/// <summary>
/// A reference to an alarm prefab.
/// </summary>
	public GameObject Alarm;

/// <summary>
/// An array of prefabs which will be used as graffiti through out the maze.
/// </summary>
	public GameObject[] Graffiti;

/// <summary>
/// The number of graffiti markings to display throughout the maze.
/// </summary>
	public int GraffitiTotal = 40;

/// <summary>
/// A reference to a light prefab.
/// </summary>
	public GameObject Light;

/// <summary>
/// The number of lights to place randomly throughout the maze.
/// </summary>
	public int LightCount = 20;

	#endregion

	#region Constructors

/// <summary>
/// A constructor which will call the super constructor to build the maze,
/// and this method will add additional objects to the maze which are 
/// specific to level one.
/// </summary>
	public new void Awake() {
		base.Awake();
		PlaceLights();
		DrawGraffiti();

	//Punch out some walls to access and exit the maze
		DestroyWall(Cells[X - 1, 0], Compass.East);
		DestroyWall(Cells[X - 1, Y - 1], Compass.East);
		DestroyWall(Cells[0, (int)Math.Floor(Y / 2.0f)], Compass.West);
	}

	#endregion

	#region Helper Methods

/// <summary>
/// Draw graffiti randomly throughout the maze.
/// </summary>
	private void DrawGraffiti() {
		GameObject current;
		System.Random rand = new System.Random(Seed + 300);
		POI3D size;

		for(int i = 0; i < GraffitiTotal; ++i) {
			current = Instantiate(Graffiti[rand.Next(0, Graffiti.Length)]) as GameObject;
			size = Cells[rand.Next(0, X), rand.Next(0, Y)].POI;

			current.transform.position = size.N1;
		}
	}

/// <summary>
/// Place break out alarms in pre-defined locations throughout the maze.
/// </summary>
	private void PlaceAlarms() {
		GameObject alarm = Instantiate(Alarm) as GameObject;
		alarm.GetComponent<Alarm>().Activated = true;
		alarm.transform.position = Cells[0, 2].POI.N1;
	}

/// <summary>
/// Place the Light prefab object randomly throughout the maze.
/// </summary>
	private void PlaceLights() {
		Vector3 pos;
		System.Random rand = new System.Random(Seed + 400);
		int x, y;

		for(int i = 0; i < LightCount; ++i) {
			x = rand.Next(X);
			y = rand.Next(Y);

			//Prevent two lights from being placed within the same cell
			if(Cells[x, y].Light == null) {
				GameObject light = Instantiate(Light) as GameObject;

				pos = Cells[x, y].Parameters.Center3D;
				pos.x -= light.transform.localScale.x;
				pos.y = -0.5f;
				pos.z += light.transform.localScale.z / 2.0f;

				Cells[x, y].Light = light;
				light.transform.position = pos;
			} else {
				--i;
			}
		}
	}

	#endregion
}