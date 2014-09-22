using UnityEngine;

/// <summary>
/// A <c>Cell</c> object functions more like a C-style struct, and is
/// merely used to hold information about a particular <c>Cell</c>,
/// such as which walls are up, its position in the grid, and whether
/// or not this <c>Cell</c> has been visited by the generation algorithm.
/// </summary>

public class Cell {
	#region Constructors

/// <summary>
/// Create a new <c>Cell</c> object, with all of the walls intact
/// and set the cell as unvisited by the generation algorithm.
/// </summary>
	public Cell(IVector2 position) {
		Light = null;
		Position = position;
		Tangent = new Direction<Cell>();
		Visited = false;
		Walls = new Direction<Walls>();
	}

	#endregion

	#region Fields

/// <summary>
/// A reference to the <c>GameObject</c> which represents the ceiling
/// of a particular <c>Cell</c>.
/// </summary>
	public GameObject Ceiling { get; set; }

/// <summary>
/// A reference to the <c>GameObject</c> which represents the floor
/// of a particular <c>Cell</c>.
/// </summary>
	public GameObject Floor { get; set; }

/// <summary>
/// A reference to the <c>GameObject</c> which represents the light
/// within a particular <c>Cell</c>.
/// </summary>
	public GameObject Light { get; set; }

/// <summary>
/// Obtain the 3D location and sizing parameters of this <c>Cell</c>
/// object.
/// </summary>
	public Parameters Parameters {
		get {
			Parameters p = new Parameters();

		//Calculate the location of the object
			p.Center2D = new Vector2(Floor.transform.position.x, Floor.transform.position.z);
			p.Center3D = new Vector3(
				Floor.transform.position.x,
				(Ceiling.transform.position.y - Floor.transform.position.y) / 2.0f,
				Floor.transform.position.z
			);
			
		//Calculate the spacial parameters inside of the cell
			p.InnerHeight = Ceiling.transform.localScale.y - (Floor.transform.localScale.y / 2.0f) - (Ceiling.transform.localScale.y / 2.0f);
			p.OuterHeight = Ceiling.transform.localScale.y;

			p.InnerLength = Ceiling.transform.localScale.z - (Walls.North == null ? 0 : (Walls.North.Wall.transform.localScale.z / 2.0f)) - (Walls.South == null ? 0 : (Walls.South.Wall.transform.localScale.z / 2.0f));
			p.OuterLength = Ceiling.transform.localScale.z;

			p.InnerWidth = Ceiling.transform.localScale.x - (Walls.East == null ? 0 : (Walls.East.Wall.transform.localScale.z / 2.0f)) - (Walls.West == null ? 0 : (Walls.West.Wall.transform.localScale.z / 2.0f));
			p.OuterWidth = Ceiling.transform.localScale.x;

			return p;
		}
	}

/// <summary>
/// The position of the <c>Cell</c> within the maze.
/// </summary>
	public IVector2 Position { get; set; }

/// <summary>
/// A collection of <c>Cell</c> objects to the North, South, East, and
/// West of the current <c>Cell</c>.
/// </summary>
	public Direction<Cell> Tangent { get; set; }

/// <summary>
/// Whether or not this <c>Cell</c> has been visited by the generaion
/// algorithm.
/// </summary>
	public bool Visited { get; set; }

/// <summary>
/// A collection of <c>GameObject</c> objects to the North, South, East,
/// and West of the current <c>Cell</c> which represent the walls which
/// divide each of the maze cells.
/// </summary>
	public Direction<Walls> Walls { get; set; }

	#endregion
}