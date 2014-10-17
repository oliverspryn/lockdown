using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class Maze<T> : MonoBehaviour where T : Cell, new() {
	#region Fields

/// <summary>
/// A reference to a ceiling prefab.
/// </summary>
	public GameObject Ceiling;

/// <summary>
/// Whether or not the walls should slide into place whenever they are out
/// of place, of if they should wait until this value is true.
/// </summary>
	public bool EnableSliding = false;

/// <summary>
/// When a wall is unused and slid into the ground, how far above the floor
/// should it project in order to indicate that a sliding wall is present?
/// </summary>
	public float HiddenWallDelta = 0.1f;

/// <summary>
/// A reference to a floor prefab.
/// </summary>
	public GameObject Floor;

/// <summary>
/// Get the length of the maze (Z-direction), including the length of all
/// the cells and the length of all the walls which construct the outer
/// boundary of the maze.
/// </summary>
	public float Length {
		get {
			return Size.x * Y + OuterWall.transform.localScale.z;
		}
	}

/// <summary>
/// A reference to an outer wall prefab, which is seperate from the standard,
/// inner wall prefab.
/// </summary>
	public GameObject OuterWall;

/// <summary>
/// A seed value used to replcate multiple instances of a maze across multiple
/// locations or machines.
/// </summary>
	public int Seed = -1;

/// <summary>
/// A reference to a wall prefab.
/// </summary>
	public GameObject Wall;

/// <summary>
/// Get the width of the maze (X-direction), including the width of all
/// the cells and the width of all the walls which construct the outer
/// boundary of the maze.
/// </summary>
	public float Width {
		get {
			return Size.x * X + OuterWall.transform.localScale.z;
		}
	}

/// <summary>
/// The number of cells in the maze, in the X-direction, the width.
/// </summary>
	public int X = 10;

/// <summary>
/// The number of cells in the maze, in the Y-direction, the height.
/// </summary>
	public int Y = 10;

	#endregion

	#region Private Members

/// <summary>
/// An array of all of the cells within the maze.
/// </summary>
	protected T[,] Cells;

/// <summary>
/// A random generator instance used throughout the maze generation 
/// process.
/// </summary>
	protected System.Random Random;

/// <summary>
/// The dimensions of the prefab walls which is used to construct the
/// inner walls of the maze.
/// </summary>
	protected Vector3 Size;

	#endregion

	#region Constructors

/// <summary>
/// This method will bootstrap all of the functionality which is necessary
/// to create perfect 3D maze using the Recusrive Backtracking Algorithm.
/// The walls, floor, and ceiling prefabs which are supplied by Unity will
/// be used to create the 3D environment.
/// </summary>
	public void Awake() {
		Seed = Seed == -1 ? (int)(DateTime.Now - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds : Seed;
		Random = new System.Random(Seed);

	//Measure the size of the prefab walls
		GameObject basis = Instantiate(Wall) as GameObject;
		Size = basis.transform.localScale;
		Destroy(basis);

	//Create the maze
		CreateCells();
		ConstructWalls();
	}

	#endregion

	#region Public Methods

/// <summary>
/// Destroy a <c>Wall</c> which borders a particular <c>Cell</c>.
/// </summary>
/// 
/// <param name="cell">The targeted Cell</param>
/// <param name="compass">The wall, ceiling, or floor which to delete</param>
/// <param name="nuke">Whether the wall (if selected) should slide under the floor or really be destroyed</param>
	public void DestroyWall(T cell, Compass compass, bool nuke = false) {
		Vector3 position;
		GameObject wall = null;

		switch(compass) {
			case Compass.Ceiling:
				Destroy(cell.Ceiling);
				return;

			case Compass.East:
				cell.Walls.East.Enabled = false;
				wall = cell.Walls.East.Wall;

				if(nuke) {
					Destroy(wall);
					return;
				}

				break;

			case Compass.Floor:
				Destroy(cell.Floor);
				return;

			case Compass.North:
				cell.Walls.North.Enabled = false;
				wall = cell.Walls.North.Wall;

				if(nuke) {
					Destroy(wall);
					return;
				}

				break;

			case Compass.South:
				cell.Walls.South.Enabled = false;
				wall = cell.Walls.South.Wall;

				if(nuke) {
					Destroy(wall);
					return;
				}

				break;

			case Compass.West:
				cell.Walls.West.Enabled = false;
				wall = cell.Walls.West.Wall;

				if(nuke) {
					Destroy(wall);
					return;
				}

				break;
		}

	//If the wall was simply supposted to move under the floor, then do that here
		position = wall.transform.position;
		position.y = -1 * (Size.y / 2.0f) + HiddenWallDelta;

		wall.transform.position = position;
	}

/// <summary>
/// Create a maze out of the grid of cells.
/// </summary>
/// 
/// <param name="seed">An optional seed value which can be used to predictably generate a maze</param>
	public void Init(int seed = -1) {
		if(seed != -1) {
			Seed = seed;
			Random = new System.Random(Seed);
		}

		CreateMaze();
	}

/// <summary>
/// Animate the walls sliding out of the floor, if the Walls are
/// configured to do so.
/// </summary>
	public void Update() {
		if(!EnableSliding)
			return;

		for(int i = 0; i < X; ++i) {
			for(int j = 0; j < Y; ++j) {
				if(Cells[i, j].Walls.East.Enabled) {
					Vector3 A = Cells[i, j].Walls.East.Wall.transform.position;
					Vector3 B = A;
					B.y = (Size.y / 2.0f);

					Cells[i, j].Walls.East.Wall.transform.position = Vector3.Lerp(A, B, Time.deltaTime / 3.0f);
				}

				if(Cells[i, j].Walls.North.Enabled) {
					Vector3 A = Cells[i, j].Walls.North.Wall.transform.position;
					Vector3 B = A;
					B.y = (Size.y / 2.0f);

					Cells[i, j].Walls.North.Wall.transform.position = Vector3.Lerp(A, B, Time.deltaTime / 3.0f);
				}
			}
		}
	}

	#endregion

	#region Helper Methods

/// <summary>
/// Once the algorithm has generated the data structure which represents 
/// the maze, use the wall prefabs to construct the physical boundaries
/// of the maze, including the floor and ceiling.
/// </summary>
	protected void ConstructWalls() {
		Vector3 position = new Vector3(0.0f, Size.y / 2.0f, 0.0f);

	//Precalculate the locations of each row and column of walls
		int halfX = (int)Math.Floor((X - 1) / 2.0f);
		int halfY = (int)Math.Floor((Y - 1) / 2.0f);

		List<float> XPos = new List<float>();
		List<float> YPos = new List<float>();

		if(X % 2 == 0) { //Even
			for(int i = -1 * halfX; i <= halfX; ++i) {
				if(i == 0) {
					XPos.Add((-1 * Size.x) / 2.0f);
					XPos.Add(Size.x / 2.0f);
				} else {
					XPos.Add((i * Size.x) + (((i < 0 ? -1 : 1) * Size.x) / 2.0f));
				}
			}
		} else {
			for(int i = -1 * halfX; i <= halfX; ++i) {
				XPos.Add(i * Size.x);
			}
		}

		if(Y % 2 == 0) { //Even
			for(int i = -1 * halfY; i <= halfY; ++i) {
				if(i == 0) {
					YPos.Add((-1 * Size.x) / 2.0f);
					YPos.Add(Size.x / 2.0f);
				} else {
					YPos.Add((i * Size.x) + (((i < 0 ? -1 : 1) * Size.x) / 2.0f));
				}
			}
		} else {
			for(int i = -1 * halfY; i <= halfY; ++i) {
				YPos.Add(i * Size.x);
			}
		}

	//Create the walls, ceilings, and floors
		Vector3 scale = new Vector3(Size.x, 0.1f, Size.x);

		for(int i = 0; i < X; ++i) {
			for(int j = 0; j < Y; ++j) {
				position.y = Size.y / 2.0f;

			//Nothern walls
				GameObject wall = Instantiate(Cells[i, j].Walls.North.Wall) as GameObject;
				position.x = XPos[i];
				position.y = (j < Y - 1) ? -1 * (Size.y / 2.0f) + HiddenWallDelta : Size.y / 2.0f;
				position.z = YPos[j] + (Size.x / 2.0f);// - (Size.x / 2.0f); //+ (Size.x / 2.0f);

				wall.transform.position = position;
				Cells[i, j].Walls.North.Wall = wall;

			//Southern walls, run only for the bottom row
				if(j == 0) {
					wall = Instantiate(Cells[i, j].Walls.South.Wall) as GameObject;
					position.x = XPos[i];
					position.y = Size.y / 2.0f;
					position.z = YPos[j] - (Size.x / 2.0f);

					wall.transform.position = position;
					Cells[i, j].Walls.South.Wall = wall;
				}

			//Eastern walls
				wall = Instantiate(Cells[i, j].Walls.East.Wall) as GameObject;
				position.x = XPos[i] + (Size.x / 2.0f);//- (Size.x / 2.0f);// + (Size.x / 2.0f);
				position.y = (i < X - 1) ? -1 * (Size.y / 2.0f) + HiddenWallDelta : Size.y / 2.0f;
				position.z = YPos[j];

				wall.transform.position = position;
				wall.transform.Rotate(wall.transform.rotation.x, 90.0f, wall.transform.rotation.z);
				Cells[i, j].Walls.East.Wall = wall;

			//Western walls
				if(i == 0) {
					wall = Instantiate(Cells[i, j].Walls.West.Wall) as GameObject;
					position.x = XPos[i] - (Size.x / 2.0f);
					position.y = Size.y / 2.0f;
					position.z = YPos[j];

					wall.transform.position = position;
					wall.transform.Rotate(wall.transform.rotation.x, 90.0f, wall.transform.rotation.z);
					Cells[i, j].Walls.West.Wall = wall;
				}

			//Floor
				GameObject floor = Instantiate(Floor) as GameObject;
				position.x = XPos[i];
				position.y = 0.0f;
				position.z = YPos[j];

				floor.transform.localScale = scale;
				floor.transform.position = position;
				Cells[i, j].Floor = floor;

			//Ceiling
				GameObject ceiling = Instantiate(Ceiling) as GameObject;
				position.y = Size.y;

				ceiling.transform.localScale = scale;
				ceiling.transform.position = position;
				ceiling.transform.Rotate(0.0f, 90.0f * Random.Next(4), 0.0f);
				Cells[i, j].Ceiling = ceiling;
			}
		}
	}

/// <summary>
/// Creates a grid of <c>Cell</c> objects for the maze generation algorithm
/// to modify when creating a maze.
/// </summary>
	protected void CreateCells() {
	//Create the grid of cells
		Cells = new T[X, Y];

		for(int i = 0; i < X; ++i) {
			for(int j = 0; j < Y; ++j) {
				Cells[i, j] = new T();
				Cells[i, j].Position = new IVector2(i, j);
			}
		}

	//Now join the tangent cells with each other and add in temporary "walls" for the grid
		for(int i = 0; i < X; ++i) {
			for(int j = 0; j < Y; ++j) {
			//Join the cells
				Cells[i, j].Tangent.North = (j < Y - 1) ? Cells[i, j + 1] : null;
				Cells[i, j].Tangent.South = (j > 0)     ? Cells[i, j - 1] : null;
				Cells[i, j].Tangent.East  = (i < X - 1) ? Cells[i + 1, j] : null;
				Cells[i, j].Tangent.West  = (i > 0)     ? Cells[i - 1, j] : null;

			//Add the walls
				Cells[i, j].Walls.North = (j != Y - 1) ? new Walls(Wall)             : new Walls(OuterWall);
				Cells[i, j].Walls.South = (j > 0)      ? Cells[i, j - 1].Walls.North : new Walls(OuterWall);
				Cells[i, j].Walls.East  = (i != X - 1) ? new Walls(Wall)             : new Walls(OuterWall);
				Cells[i, j].Walls.West  = (i > 0)      ? Cells[i - 1, j].Walls.East  : new Walls(OuterWall);
			}
		}
	}

/// <summary>
/// Utilize a pre-established grid of <c>Cell</c> objects to generate a 
/// maze.
/// </summary>
	protected void CreateMaze() {
	//Select a random cell
		T current = Cells[Random.Next(X), Random.Next(Y)];

	//Initialize the stack and total number of cells
		Stack<T> stack = new Stack<T>();
		int total = X * Y;
		int visited = 1;

	//Build out the cells
		List<T> unvisited;
		T random;

		while(visited < total) {
			unvisited = GetUnvisitedNeighbors(ref current);

			if(unvisited.Count > 0) {
				random = unvisited[Random.Next(unvisited.Count)];
				DestroyWalls(ref current, ref random);

				stack.Push(current);
				current = random;
				++visited;
			} else {
				current = stack.Pop();
			}
		}
	}

/// <summary>
/// By default, the maze generator will begin with a grid of cells, all
/// of which have a North, South, East, and West wall. The algorithm will
/// begin by "knocking" down the walls within the grid to create a maze.
/// This function will take two <c>Cell</c> objects, and will determine 
/// which wall to "knock down" in order to create a passage between them.
/// </summary>
/// 
/// <param name="cellOne">A <c>Cell</c> object within the grid</param>
/// <param name="cellTwo">Another <c>Cell</c> object between which to create a passage</param>
	protected void DestroyWalls(ref T cellOne, ref T cellTwo) {
		IVector2 N = new IVector2(cellOne.Position.X, cellOne.Position.Y);
		N.Y++;
		IVector2 S = new IVector2(cellOne.Position.X, cellOne.Position.Y);
		S.Y--;
		IVector2 E = new IVector2(cellOne.Position.X, cellOne.Position.Y);
		E.X++;
		IVector2 W = new IVector2(cellOne.Position.X, cellOne.Position.Y);
		W.X--;

	//CellTwo is North of CellOne
		if(cellTwo.Position == N)
			cellOne.Walls.North.Enabled = cellTwo.Walls.South.Enabled = false;

	//CellTwo is South of CellOne
		if(cellTwo.Position == S)
			cellOne.Walls.South.Enabled = cellTwo.Walls.North.Enabled = false;

	//CellTwo is East of CellOne
		if(cellTwo.Position == E)
			cellOne.Walls.East.Enabled = cellTwo.Walls.West.Enabled = false;

	//CellTwo is West of CellOne
		if(cellTwo.Position == W)
			cellOne.Walls.West.Enabled = cellTwo.Walls.East.Enabled = false;

		cellOne.Visited = cellTwo.Visited = true;
	}

/// <summary>
/// Select a random, enabled wall which is tanget to a particular <c>Cell</c>.
/// </summary>
/// 
/// <param name="cell">The Cell whose wall should be fetched</param>
/// <returns>A random Wall object which is tangent to the given Cell object</returns>
	protected RandomWall GetRandomWall(T cell) {
		List<Walls> walls = new List<Walls>() {
			cell.Walls.East,
			cell.Walls.North,
			cell.Walls.South,
			cell.Walls.West
		};
		
	//Is the randomly selected wall actually up?
		int chosenOne = Random.Next(4);

		for(int i = chosenOne; true; i = (i + 1) % 4) {
			if(walls[i].Enabled) {
				RandomWall r = new RandomWall();
				r.Wall = walls[i];

			//Determine the direction of the wall
				switch(i) {
					case 0:
						r.Direction = Compass.East;
						return r;

					case 1:
						r.Direction = Compass.North;
						return r;

					case 2:
						r.Direction = Compass.South;
						return r;

					case 3:
						r.Direction = Compass.West;
						return r;
				}
			}
		}
	}

/// <summary>
/// Obtain a listing of neighboring cells which have not yet been visited
/// by the maze generation algorithm.
/// </summary>
/// 
/// <param name="current">A <c>Cell</c> object whose neighbors should be analyzed</param>
/// <returns>A list of <c>Cell</c> objects with neighbors which have not been visited</returns>
	protected List<T> GetUnvisitedNeighbors(ref T current) {
		List<T> ret = new List<T>();

		if(current.Tangent.North != null && !current.Tangent.North.Visited)
			ret.Add(current.Tangent.North as T);

		if(current.Tangent.South != null && !current.Tangent.South.Visited)
			ret.Add(current.Tangent.South as T);

		if(current.Tangent.East != null && !current.Tangent.East.Visited)
			ret.Add(current.Tangent.East as T);

		if(current.Tangent.West != null && !current.Tangent.West.Visited)
			ret.Add(current.Tangent.West as T);

		return ret;
	}

	#endregion
}