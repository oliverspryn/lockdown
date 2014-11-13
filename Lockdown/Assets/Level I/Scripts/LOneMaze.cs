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
/// An array of blockades to put in the players' path.
/// </summary>
	public GameObject[] Blockades;

/// <summary>
/// An array of objects to disperse throughout the maze which a player can
/// pick up.
/// </summary>
	public GameObject[] Collectables;

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

	#endregion

	#region Private Memebers

/// <summary>
/// Whether or not the alarms have already been sounded once.
/// </summary>
	private bool AlarmsSounded = false;
	
	#endregion

	#region Constructors

/// <summary>
/// A constructor which will call the super constructor to build the maze,
/// and this method will add additional objects to the maze which are 
/// specific to level one.
/// </summary>
	public void Start() {
		PlaceAlarms();
		PlaceLights();
	}

	#endregion

	#region Public Methods

/// <summary>
/// Generate the maze and place objects throughout the maze.
/// </summary>
///
/// <param name="seed">An optional seed value which can be used to predictably generate a maze</param>
	public new void Init(int seed = -1) {
		base.Init(seed);
		DrawGraffiti();
		PlaceAlarms();
		PlaceBlockades();
		PlaceCollectables();
	}

/// <summary>
/// Activate all of the alarms which have have been placed within
/// the maze. This can only be called once.
/// </summary>
	public void SoundAlarm() {
	//Have the alarms already been sounded once?
		if(AlarmsSounded)
			return;

	//Sound the alarms
		AlarmsSounded = true;
		bool soundEnabled = true;

		for(int i = 0; i < X; ++i) {
			for(int j = 0; j < Y; ++j) {
				if(Cells[i, j].Alarm != null) {
					Cells[i, j].Alarm.GetComponent<Alarm>().Activate(soundEnabled);
					soundEnabled = false; //Sound only one of the alarm
				}
			}
		}
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

			//Does this cell not have any bounding walls? Try again, then.
				if(wall == null) {
					--i;
					continue;
				}

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
		LOneCell cell;
		LOneCell[] targets = { Cells[0, 0], Cells[0, Y - 1], Cells[X - 1, 0], Cells[X - 1, Y - 1] };

		for(int i = 0; i < targets.Length; ++i) {
			cell = targets[i];
			cell.Alarm = Instantiate(Alarm) as GameObject;

			if(cell.Position.Y == 0) {
				cell.Alarm.transform.position = cell.GetPOI(Compass.South).N1;
			} else if(cell.Position.Y == Y - 1) {
				cell.Alarm.transform.position = cell.GetPOI(Compass.North).N1;
				cell.Alarm.transform.Rotate(0.0f, 180.0f, 0.0f);
			}
		}
	}

/// <summary>
/// Randomly place blockades throughout the maze to block the player from
/// having free access throughout the maze.
/// </summary>
	private void PlaceBlockades() {
		LOneCell cell;
		Vector3 pos;

		for(int i = 0; i < Blockades.Length; ++i) {
			cell = Cells[Random.Next(X - 1), Random.Next(Y - 1)];

			if(Random.Next(1) == 0 && !cell.Walls.North.Enabled && cell.Blockade == null) {
			//Create a position the blockade
				cell.Blockade = Instantiate(Blockades[i]) as GameObject;
				Blockades[i] = cell.Blockade;

				pos = cell.GetPOI(Compass.North).C;
				pos.x += 2.7f;
				pos.y -= cell.Blockade.GetComponent<Blockade>().Height + 7.03f;
				pos.z += 5.27f;

				cell.Blockade.transform.position = pos;
				cell.Blockade.transform.Rotate(0.0f, 90.0f, 0.0f);

			//Replace the wall that was there
				Destroy(cell.Walls.North.Wall);

				cell.Walls.North.Enabled = true;
				cell.Walls.North.Wall = cell.Blockade;
				cell.Tangent.North.Walls.South.Enabled = true;
				cell.Tangent.North.Walls.South.Wall = cell.Blockade;
			} else if(!cell.Walls.East.Enabled && cell.Blockade == null) {
			//Create a position the blockade
				cell.Blockade = Instantiate(Blockades[i]) as GameObject;
				Blockades[i] = cell.Blockade;

				pos = cell.GetPOI(Compass.East).C;
				pos.x -= 5.03f;
				pos.y -= cell.Blockade.GetComponent<Blockade>().Height + 7.03f;
				pos.z += 2.69f;

				cell.Blockade.transform.position = pos;

			//Replace the wall that was there
				Destroy(cell.Walls.East.Wall);

				cell.Walls.East.Enabled = true;
				cell.Walls.East.Wall = cell.Blockade;
				cell.Tangent.East.Walls.West.Enabled = true;
				cell.Tangent.East.Walls.West.Wall = cell.Blockade;
			} else {
				--i;
				continue;
			}

			Blockades[i].GetComponent<Blockade>().OpenOnStart = false;
		}
	}

/// <summary>
/// Place objects a player can pick up randomly throughout the maze.
/// </summary>
	private void PlaceCollectables() {
		LOneCell cell;
		Vector3 pos;

		for(int i = 0; i < Collectables.Length; ++i) {
			cell = Cells[Random.Next(X), Random.Next(Y)];

			if(cell.Collectable == null) {
				cell.Collectable = Instantiate(Collectables[i]) as GameObject;
				pos = cell.GetPOI(Compass.Floor).C;
				pos.y = cell.GetPOI(Compass.North).C.y;

				cell.Collectable.transform.position = pos;
			} else {
				--i;
			}
		}
	}

/// <summary>
/// Place the Light prefab object randomly throughout the maze.
/// </summary>
	private void PlaceLights() {
		Vector3 pos;
		Cell[] targets = { Cells[0, 0], Cells[0, Y - 1], Cells[X - 1, 0], Cells[X - 1, Y - 1] };
		int x, y;

		for(int i = 0; i < targets.Length; ++i) {
			GameObject light = Instantiate(Light) as GameObject;
			x = targets[i].Position.X;
			y = targets[i].Position.Y;

			pos = Cells[x, y].Parameters.Center3D;
			pos.x -= light.transform.localScale.x;
			pos.y = -0.5f + MazeLocation.y;
			pos.z += light.transform.localScale.z / 2.0f;

			Cells[x, y].Light = light;
			light.transform.position = pos;
		}
	}

	#endregion
}