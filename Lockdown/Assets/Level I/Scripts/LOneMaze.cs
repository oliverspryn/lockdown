using UnityEngine;
using System;

/// <summary>
/// Generate and populate a maze which is specific to level one
/// for this game.
/// </summary>
public class LOneMaze : Maze<LOneCell> {
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
	public int GraffitiTotal = 20;

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
	public void Start() {
		PlaceLights();
	}

	#endregion

	#region Public Methods

/// <summary>
/// Generate the maze and place graffiti on the walls.
/// </summary>
///
/// <param name="seed">An optional seed value which can be used to predictably generate a maze</param>
	public void Init(int seed = -1) {
		base.Init();
		DrawGraffiti();
	}
	
/// <summary>
/// Slide the graffiti into place whenever the walls are sliding
/// into place.s
/// </summary>
	public new void Update() {
		base.Update();

		if(!EnableSliding)
			return;

	//Slide up the graffiti
		for(int i = 0; i < X; ++i) {
			for(int j = 0; j < Y; ++j) {
				if(Cells[i, j].Graffiti != null) {
					Vector3 A = Cells[i, j].Graffiti.transform.position;
					Vector3 B = A;
					B.y = DestLocation - (Size.y / 4.0f);

					Cells[i, j].Graffiti.transform.position = Vector3.Lerp(A, B, Time.deltaTime / 3.0f);
				}
			}
		}
	}

	#endregion

	#region Helper Methods

/// <summary>
/// Draw graffiti randomly throughout the maze.
/// </summary>
	private void DrawGraffiti() {
		LOneCell cell;
		GameObject graffiti;
		Vector3 size;
		RandomWall wall;

		for(int i = 0; i < GraffitiTotal; ++i) {
			cell = Cells[Random.Next(0, X), Random.Next(0, Y)];

			if(cell.Graffiti == null) {
				graffiti = Instantiate(Graffiti[Random.Next(0, Graffiti.Length)]) as GameObject;
				wall = GetRandomWall(cell);
				cell.Graffiti = graffiti;

			//Position the graffiti
				size = cell.GetPOI(wall.Direction).S1;

				if(cell.Position.X == 0       && wall.Direction == Compass.West  || 
				   cell.Position.X == (X - 1) && wall.Direction == Compass.East  ||
				   cell.Position.Y == 0       && wall.Direction == Compass.South ||
				   cell.Position.Y == (Y - 1) && wall.Direction == Compass.North) {
					//This is an outer wall, keep the graffiti above the ground
				} else {
					size.y -= wall.Wall.Wall.transform.localScale.y;
				}

				size.y += MazeLocation.y;
				graffiti.transform.position = size;

			//Rotate the graffiti
				switch(wall.Direction) {
					case Compass.East:
						graffiti.transform.Rotate(0.0f, 90.0f, 0.0f);
						break;

					case Compass.South:
						graffiti.transform.Rotate(0.0f, 180.0f, 0.0f);
						break;

					case Compass.West:
						graffiti.transform.Rotate(0.0f, 270.0f, 0.0f);
						break;
				}
			} else {
				--i;
			}
		}
	}

/// <summary>
/// Place break out alarms in pre-defined locations throughout the maze.
/// </summary>
	private void PlaceAlarms() {
		GameObject alarm = Instantiate(Alarm) as GameObject;
		alarm.GetComponent<Alarm>().Activated = true;
		alarm.transform.position = Cells[0, 2].GetPOI(Compass.North).N1;
	}

/// <summary>
/// Place the Light prefab object randomly throughout the maze.
/// </summary>
	private void PlaceLights() {
		Vector3 pos;
		int x, y;

		for(int i = 0; i < LightCount; ++i) {
			x = Random.Next(X);
			y = Random.Next(Y);

			//Prevent two lights from being placed within the same cell
			if(Cells[x, y].Light == null) {
				GameObject light = Instantiate(Light) as GameObject;

				pos = Cells[x, y].Parameters.Center3D;
				pos.x -= light.transform.localScale.x;
				pos.y = -0.5f + MazeLocation.y;
				pos.z += light.transform.localScale.z / 2.0f;

				Cells[x, y].Light = light;
				light.transform.position = pos; //+ MazeLocation;
			} else {
				--i;
			}
		}
	}

	#endregion
}