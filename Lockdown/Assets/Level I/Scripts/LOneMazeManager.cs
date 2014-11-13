using UnityEngine;
using System;

public class LOneMazeManager : MonoBehaviour {
	#region Fields

/// <summary>
/// A reference to the destroyer which will destroy level one, if the game
/// does not end within a particular amount of time.
/// </summary>
	public GameObject Destroyer = null;

/// <summary>
/// Get the length of the maze (Z-direction), including the length of all
/// the cells and the length of all the walls which construct the outer
/// boundary of the maze.
/// </summary>
	public float Length {
		get {
			return Maze.GetComponent<LOneMaze>().Length;
		}
	}

/// <summary>
/// A pre-configured maze prefab to copy into a super maze.
/// </summary>
	public GameObject Maze;

/// <summary>
/// A prefab which the player will run into to trigger the rising walls in
/// the next sub-maze.
/// </summary>
	public GameObject RiserTrigger;

/// <summary>
/// A seed value used to replcate multiple instances of a maze across multiple
/// locations or machines.
/// </summary>
	public int Seed = -1;

/// <summary>
/// Get the width of the maze (X-direction), including the width of all
/// the cells and the width of all the walls which construct the outer
/// boundary of the maze.
/// </summary>
	public float Width {
		get {
			return Maze.GetComponent<LOneMaze>().Width * 3.0f;
		}
	}

	#endregion

	#region Private Members

/// <summary>
/// The maze which is at the center of the super maze.
/// </summary>
	private MazeAccessories Center;

/// <summary>
/// The maze which is at the left of the super maze.
/// </summary>
	private MazeAccessories Left;

/// <summary>
/// The maze which is at the right of the super maze.
/// </summary>
	private MazeAccessories Right;

	#endregion

	#region Constructors

/// <summary>
/// Create and position the child mazes within the super maze.
/// </summary>
	public void Awake() {
		Seed = Seed == -1 ? (int)(DateTime.Now - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds : Seed;

	//Position the child mazes
		Center = new MazeAccessories();
		Center.Maze = Instantiate(Maze) as GameObject;
		Center.Script = Center.Maze.GetComponent<LOneMaze>();

		Maze.GetComponent<LOneMaze>().MazeLocation.x -= Center.Script.Width;
		Left = new MazeAccessories();
		Left.Maze = Instantiate(Maze) as GameObject;
		Left.Script = Left.Maze.GetComponent<LOneMaze>();

		Maze.GetComponent<LOneMaze>().MazeLocation.x += 2.0f * Center.Script.Width;
		Right = new MazeAccessories();
		Right.Maze = Instantiate(Maze) as GameObject;
		Right.Script = Right.Maze.GetComponent<LOneMaze>();

	//Position the triggers
		Left.Trigger = Instantiate(RiserTrigger) as GameObject;
		Left.Trigger.transform.position = Left.Script.Cells[Left.Script.X - 1, 0].GetPOI(Compass.East).C;
		Left.Trigger.GetComponent<WallRiser>().Maze = Left.Maze;

		Center.Trigger = Instantiate(RiserTrigger) as GameObject;
		Center.Trigger.transform.position = Center.Script.Cells[Center.Script.X - 1, Center.Script.Y - 1].GetPOI(Compass.East).C;
		Center.Trigger.GetComponent<WallRiser>().Maze = Center.Maze;

		Right.Trigger = Instantiate(RiserTrigger) as GameObject;
		Right.Trigger.transform.position = Right.Script.Cells[Right.Script.X - 1, 0].GetPOI(Compass.East).C;
		Right.Trigger.GetComponent<WallRiser>().Maze = Right.Maze;

		Right.Trigger.GetComponent<WallRiser>().Destroyer = Destroyer;

	//Reset the position of the flipping prefab, somehow, we just modified it
		Maze.GetComponent<LOneMaze>().MazeLocation.x -= Center.Script.Width;
	}

/// <summary>
/// Create passageways between the child mazes.
/// </summary>
	public void Start() {
		Left.Script.DestroyWall(Left.Script.X - 1, 0, Compass.East, true);
		Left.Script.DestroyWall(0, Left.Script.Y - 1, Compass.West, true);

		Center.Script.DestroyWall(Center.Script.X - 1, Center.Script.Y - 1, Compass.East, true);
		Center.Script.DestroyWall(0, 0, Compass.West, true);

		Right.Script.DestroyWall(Right.Script.X - 1, 0, Compass.East, true);
		Right.Script.DestroyWall(0, Right.Script.Y - 1, Compass.West, true);
	}

	#endregion

	#region Public Methods

/// <summary>
/// Generate each of the child mazes.
/// </summary>
/// 
/// <param name="seed">An optional seed value which can be used to predictably generate a maze</param>
	public void Init(int seed = -1) {
		seed = (seed == -1) ? Seed : seed;

		Left.Script.Init(seed);
		Center.Script.Init(seed + 42);
		Right.Script.Init(seed + 84);
	}

	#endregion
}