using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;

public class Maze : MonoBehaviour {
	#region Fields

/// <summary>
/// A reference to a ceiling prefab.
/// </summary>
	public GameObject Ceiling;

/// <summary>
/// An array of all of the cells within the maze.
/// </summary>
	private Cell[,] Cells;

/// <summary>
/// A default <c>GameObject</c> to assign as the "inner walls" of the master
/// grid until appropriate walls can be knocked down and new ones added 
/// where they should be.
/// </summary>
	private GameObject DefaultGO;

/// <summary>
/// A reference to a floor prefab.
/// </summary>
	public GameObject Floor;

/// <summary>
/// Get the height of the maze, including the height of all the cells and
/// the length of all the walls which construct the overall height of the
/// maze.
/// </summary>
	public float Height {
		get {
			return Size.y * Y + Size.z * (Y - 1);
		}
	}

/// <summary>
/// The dimensions of the prefab walls which is used to construct the
/// walls of the maze.
/// </summary>
	private Vector3 Size;

/// <summary>
/// A reference to a wall prefab.
/// </summary>
	public GameObject Wall;

/// <summary>
/// Get the width of the maze, including the width of all the cells and
/// the width of all the walls which construct the overall width of the
/// maze.
/// </summary>
	public float Width {
		get {
			return Size.x * X + Size.z * (X - 1);
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

	#region Constructors

/// <summary>
/// This method will bootstrap all of the functionality which is necessary
/// to create perfect 3D maze using the Recusrive Backtracking Algorithm.
/// The walls, floor, and ceiling prefabs which are supplied by Unity will
/// be used to create the 3D environment.
/// </summary>
	public void Start() {
		DefaultGO = new GameObject();

	//Measure the size of the prefab walls
		GameObject basis = Instantiate(Wall) as GameObject;
		Size = basis.transform.localScale;
		Destroy(basis);

	//Create the maze
		CreateCells();
		CreateMaze();
		ConstructWalls();
	}

	#endregion

	#region Helper Methods

/// <summary>
/// Creates a grid of <c>Cell</c> objects for the maze generation algorithm
/// to modify when creating a maze.
/// </summary>
	private void CreateCells() {
	//Create the grid of cells
		Cells = new Cell[X, Y];

		for(int i = 0; i < X; ++i) {
			for(int j = 0; j < Y; ++j) {
				Cells[i, j] = new Cell(new IVector2(i, j));
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
				Cells[i, j].Walls.North = (j < Y - 1) ? DefaultGO : null;
				Cells[i, j].Walls.South = (j > 0)     ? DefaultGO : null;
				Cells[i, j].Walls.East  = (i < X - 1) ? DefaultGO : null;
				Cells[i, j].Walls.West  = (i > 0)     ? DefaultGO : null;
			}
		}
	}

/// <summary>
/// Utilize a pre-established grid of <c>Cell</c> objects to generate a 
/// maze.
/// </summary>
	private void CreateMaze() {
	//Select a random cell
		System.Random rand = new System.Random();
		Cell current = Cells[rand.Next(X), rand.Next(Y)];

	//Initialize the stack and total number of cells
		Stack<Cell> stack = new Stack<Cell>();
		int total = X * Y;
		int visited = 1;

	//Build out the cells
		List<Cell> unvisited;
		Cell random;

		while(visited < total) {
			unvisited = GetUnvisitedNeighbors(ref current);

			if(unvisited.Count > 0) {
				random = unvisited.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
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
/// Once the algorithm has generated the data structure which represents 
/// the maze, use the wall prefabs to construct the physical boundaries
/// of the maze, including the floor and ceiling.
/// </summary>
	private void ConstructWalls() {
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
		int merryXMax = (X % 2 == 0) ? X : (X);
		int merryYMax = (Y % 2 == 0) ? Y : (Y);
		Vector3 scale = new Vector3(Size.x, 0.1f, Size.x);

		for(int i = 0; i < merryXMax; ++i) {
			for(int j = 0; j < merryYMax; ++j) {
				position.y = Size.y / 2.0f;

			//Nothern walls
				if(Cells[i, j].Walls.North != null || j == Y - 1) {
				//Build the wall
					GameObject wall = Instantiate(Wall) as GameObject;
					position.x = XPos[i];
					position.z = YPos[j] + (Size.x / 2.0f);// - (Size.x / 2.0f); //+ (Size.x / 2.0f);

					wall.transform.position = position;

				//Assign this wall to the cell
					Cells[i, j].Walls.North = wall;

					if(j < Y - 1)
						Cells[i, j + 1].Walls.South = wall;
				}

			//Southern walls, run only for the bottom row
				if(j == 0) {
				//Build the wall
					GameObject wall = Instantiate(Wall) as GameObject;
					position.x = XPos[i];
					position.z = YPos[j] - (Size.x / 2.0f);

					wall.transform.position = position;

				//Assign this wall to the cell
					Cells[i, j].Walls.South = wall;
				}

			//Eastern walls
				if(Cells[i, j].Walls.East != null || i == X - 1) {
				//Build the wall
					GameObject wall = Instantiate(Wall) as GameObject;
					position.x = XPos[i] + (Size.x / 2.0f);//- (Size.x / 2.0f);// + (Size.x / 2.0f);
					position.z = YPos[j];

					wall.transform.position = position;
					wall.transform.Rotate(wall.transform.rotation.x, 90.0f, wall.transform.rotation.z);

				//Assign this wall to the cell
					Cells[i, j].Walls.East = wall;

					if(i < X - 1)
						Cells[i, j].Walls.West = wall;
				}

			//Western walls
				if(i == 0) {
				//Build the wall
					GameObject wall = Instantiate(Wall) as GameObject;
					position.x = XPos[i] - (Size.x / 2.0f);
					position.z = YPos[j];

					wall.transform.position = position;
					wall.transform.Rotate(wall.transform.rotation.x, 90.0f, wall.transform.rotation.z);

				//Assign this wall to the cell
					Cells[i, j].Walls.West = wall;
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
				Cells[i, j].Ceiling = ceiling;
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
	private void DestroyWalls(ref Cell cellOne, ref Cell cellTwo) {
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
			cellOne.Walls.North = cellTwo.Walls.South = null;

	//CellTwo is South of CellOne
		if(cellTwo.Position == S)
			cellOne.Walls.South = cellTwo.Walls.North = null;

	//CellTwo is East of CellOne
		if(cellTwo.Position == E)
			cellOne.Walls.East = cellTwo.Walls.West = null;

	//CellTwo is West of CellOne
		if(cellTwo.Position == W)
			cellOne.Walls.West = cellTwo.Walls.East = null;

		cellOne.Visited = cellTwo.Visited = true;
	}

/// <summary>
/// Obtain a listing of neighboring cells which have not yet been visited
/// by the maze generation algorithm.
/// </summary>
/// 
/// <param name="current">A <c>Cell</c> object whose neighbors should be analyzed</param>
/// <returns>A list of <c>Cell</c> objects with neighbors which have not been visited</returns>
	private List<Cell> GetUnvisitedNeighbors(ref Cell current) {
		List<Cell> ret = new List<Cell>();
		
		if(current.Tangent.North != null && !current.Tangent.North.Visited)
			ret.Add(current.Tangent.North);

		if(current.Tangent.South != null && !current.Tangent.South.Visited)
			ret.Add(current.Tangent.South);

		if(current.Tangent.East != null && !current.Tangent.East.Visited)
			ret.Add(current.Tangent.East);

		if(current.Tangent.West != null && !current.Tangent.West.Visited)
			ret.Add(current.Tangent.West);

		return ret;
	}

	#endregion
}