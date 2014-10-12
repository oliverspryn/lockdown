﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// This class will calculate several points of interest on a 
/// <c>Wall</c> object which resides on the side of a <c>Cell</c>
/// object. The charts below show each of the points this class
/// will calculate. Note that the points naer the border of the
/// <c>Wall</c> obbject are actually pressed up against the edge
/// of the object, without any sort of margin.
/// 
/// With labels:
/// 
///     +=========================+
///     | X          X          X |
///     |   NW2        N2   NE2   |
///     |      X     X     X      |
///     |   NW1        N1   NE1   |
///     | X    X     X     X    X |
///     |   W2   W1   C  E1  E2   |
///     |      X     X     X      |
///     |   SW1        S1   SE1   |
///     | X          X          X |
///     |   Sw2        S2   SE2   |
///     +=========================+
///     
///  Without labels:
///  
///		+=========================+
///     | X          X          X |
///     |                         |
///     |      X     X     X      |
///     |                         |
///     | X    X     X     X    X |
///     |                         |
///     |      X     X     X      |
///     |                         |
///     | X          X          X |
///     +=========================+
/// 
/// </summary>
public class POI3D {
	#region Fields

/// <summary>
/// Calculate the center point.
/// </summary>
	public Vector3 C {
		get {
			switch(Direction) {
				case Compass.Ceiling:
					return new Vector3(POIX[Point3D.Center], POIY[Point3D.UUp] - Abutted[Compass.Ceiling], POIZ[Point3D.Center]);

				case Compass.East:
					return new Vector3(POIX[Point3D.LLeft] + Abutted[Compass.East], POIY[Point3D.Center], POIZ[Point3D.Center]);

				case Compass.Floor:
					return new Vector3(POIX[Point3D.Center], POIY[Point3D.DDown] + Abutted[Compass.Floor], POIZ[Point3D.Center]);

				case Compass.North:
					return new Vector3(POIX[Point3D.Center], POIY[Point3D.Center], POIZ[Point3D.FForward] + Abutted[Compass.North]);

				case Compass.South:
					return new Vector3(POIX[Point3D.Center], POIY[Point3D.Center], POIZ[Point3D.BBackward] + Abutted[Compass.South]);

				case Compass.West:
					return new Vector3(POIX[Point3D.RRight] - Abutted[Compass.West], POIY[Point3D.Center], POIZ[Point3D.Center]);
			}

			return new Vector3();
		}
	}

/// <summary>
/// Calculate the north-center most point.
/// </summary>
	public Vector3 N1 {
		get {
			switch(Direction) {
				case Compass.Ceiling:
					return new Vector3(POIX[Point3D.Center], POIY[Point3D.UUp] - Abutted[Compass.Ceiling], POIZ[Point3D.FForward] - Abutted[Compass.North]);

				case Compass.East:
					return new Vector3(POIX[Point3D.LLeft] - Abutted[Compass.East], POIY[Point3D.UUp] - Abutted[Compass.Ceiling], POIZ[Point3D.Center]);

				case Compass.Floor:
					return new Vector3(POIX[Point3D.Center], POIY[Point3D.DDown] + Abutted[Compass.Floor], POIZ[Point3D.FForward] - Abutted[Compass.North]);

				case Compass.North:
					return new Vector3(POIX[Point3D.Center], POIY[Point3D.UUp] - Abutted[Compass.Ceiling], POIZ[Point3D.FForward] - Abutted[Compass.North]);

				case Compass.South:
					return new Vector3(POIX[Point3D.Center], POIY[Point3D.UUp] - Abutted[Compass.Ceiling], POIZ[Point3D.BBackward] + Abutted[Compass.South]);

				case Compass.West:
					return new Vector3(POIX[Point3D.LLeft] + Abutted[Compass.West], POIY[Point3D.UUp] - Abutted[Compass.Ceiling], POIZ[Point3D.Center]);
			}

			return new Vector3();
		}
	}

	#endregion

	#region Private Members

/// <summary>
/// A listing of space values which an abutted, intruding object will occupy 
/// within a <c>Cell</c>. For instance, how much space is the width of a 
/// <c>Wall</c> taking in the current <c>Cell</c>?
/// </summary>
	private Dictionary<Compass, float> Abutted;

/// <summary>
/// The Cell object which contains boundaries, such as walls and a
/// floor, to measure.
/// </summary>
	private Cell Cell;

/// <summary>
/// The direction of the wall with respect to its associated <c>Cell</c>.
/// </summary>
	private Compass Direction;

/// <summary>
/// A dictionary of pre-calculated POI on the object which span in the X
/// direction.
/// </summary>
	private Dictionary<Point3D, float> POIX;

/// <summary>
/// A dictionary of pre-calculated POI on the object which span in the Y
/// direction.
/// </summary>
	private Dictionary<Point3D, float> POIY;

/// <summary>
/// A dictionary of pre-calculated POI on the object which span in the Z
/// direction.
/// </summary>
	private Dictionary<Point3D, float> POIZ;

/// <summary>
/// The <c>Wall</c> object which to measure.
/// </summary>
	private Walls Wall;

	#endregion

	#region Constructors

/// <summary>
/// Build an object which can calculate several points of interest
/// on a wall, ceiling, or floor object which borders the outside
/// of a <c>Cell</c> object.
/// </summary>
/// 
/// <param name="cell">The Cell object which contains boundaries, such as walls and a floor, to measure</param>
/// <param name="direction">The direction of the wall with respect to its associated Cell</param>
	public POI3D(Cell cell, Compass direction) {
		Cell = cell;
		Direction = direction;

	//Precalculate a grid of POI locations
		Vector3 center = cell.Parameters.Center3D;
		POIX = new Dictionary<Point3D, float>();
		POIY = new Dictionary<Point3D, float>();
		POIZ = new Dictionary<Point3D, float>();

		float fifthX = cell.Floor.transform.localScale.x / 5.0f;
		POIX[Point3D.LLeft]  = center.x - (fifthX * 2.0f);
		POIX[Point3D.Left]   = center.x - (fifthX * 1.0f);
		POIX[Point3D.Center] = center.x + (fifthX * 0.0f);
		POIX[Point3D.Right]  = center.x + (fifthX * 1.0f);
		POIX[Point3D.RRight] = center.x + (fifthX * 2.0f);

		float fifthY = (cell.Ceiling.transform.position.y - cell.Floor.transform.position.y) / 5.0f;
		POIY[Point3D.UUp]    = center.y + (fifthY * 2.0f);
		POIY[Point3D.Up]     = center.y + (fifthY * 1.0f);
		POIY[Point3D.Center] = center.y + (fifthY * 0.0f);
		POIY[Point3D.Down]   = center.y - (fifthY * 1.0f);
		POIY[Point3D.DDown]  = center.y - (fifthY * 2.0f);

		float fifthZ = fifthX;
		POIZ[Point3D.FForward]  = center.z + (fifthZ * 2.0f);
		POIZ[Point3D.Forward]   = center.z + (fifthZ * 1.0f);
		POIZ[Point3D.Center]    = center.z + (fifthZ * 0.0f);
		POIZ[Point3D.Backward]  = center.z - (fifthZ * 1.0f);
		POIZ[Point3D.BBackward] = center.z - (fifthZ * 2.0f);

	//Precalculate a list of minor offsets which will be required a surface is abutted against another
		Abutted = new Dictionary<Compass, float>();
		Abutted[Compass.Ceiling] = Cell.Ceiling.transform.localScale.y / 2.0f;
		Abutted[Compass.East]    = Cell.Walls.East.Enabled  ? Cell.Walls.East.Wall.transform.localScale.z / 2.0f : 0.0f;
		Abutted[Compass.Floor]   = Cell.Floor.transform.localScale.y / 2.0f;
		Abutted[Compass.North]   = Cell.Walls.North.Enabled ? Cell.Walls.North.Wall.transform.localScale.z / 2.0f : 0.0f;
		Abutted[Compass.South]   = Cell.Walls.South.Enabled ? Cell.Walls.South.Wall.transform.localScale.z / 2.0f : 0.0f;
		Abutted[Compass.West]    = Cell.Walls.West.Enabled  ? Cell.Walls.West.Wall.transform.localScale.z / 2.0f : 0.0f;

	//Precalculate if these objects are abutted with another
		/*
		Abutted = new Dictionary<Compass, bool>();

		switch(direction) {
			case Compass.Ceiling:
				Abutted.Add(Compass.Ceiling, false);
				Abutted.Add(Compass.East,    Cell.Walls.East.Enabled);
				Abutted.Add(Compass.Floor,   false);
				Abutted.Add(Compass.North,   Cell.Walls.North.Enabled);
				Abutted.Add(Compass.South,   Cell.Walls.South.Enabled);
				Abutted.Add(Compass.West,    Cell.Walls.West.Enabled);
				break;

			case Compass.East:
				Abutted.Add(Compass.Ceiling, true);
				Abutted.Add(Compass.East,    false);
				Abutted.Add(Compass.Floor,   true);
				Abutted.Add(Compass.North,   Cell.Walls.North.Enabled);
				Abutted.Add(Compass.South,   Cell.Walls.South.Enabled);
				Abutted.Add(Compass.West,    false);
				break;

			case Compass.Floor:
				Abutted.Add(Compass.Ceiling, false);
				Abutted.Add(Compass.East,    Cell.Walls.East.Enabled);
				Abutted.Add(Compass.Floor,   false);
				Abutted.Add(Compass.North,   Cell.Walls.North.Enabled);
				Abutted.Add(Compass.South,   Cell.Walls.South.Enabled);
				Abutted.Add(Compass.West,    Cell.Walls.West.Enabled);
				break;

			case Compass.North:
				Abutted.Add(Compass.Ceiling, true);
				Abutted.Add(Compass.East,    Cell.Walls.East.Enabled);
				Abutted.Add(Compass.Floor,   true);
				Abutted.Add(Compass.North,   false);
				Abutted.Add(Compass.South,   false);
				Abutted.Add(Compass.West,    Cell.Walls.West.Enabled);
				break;

			case Compass.South:
				Abutted.Add(Compass.Ceiling, true);
				Abutted.Add(Compass.East,    Cell.Walls.East.Enabled);
				Abutted.Add(Compass.Floor,   true);
				Abutted.Add(Compass.North,   false);
				Abutted.Add(Compass.South,   false);
				Abutted.Add(Compass.West,    Cell.Walls.West.Enabled);
				break;

			case Compass.West:
				Abutted.Add(Compass.Ceiling, true);
				Abutted.Add(Compass.East,    false);
				Abutted.Add(Compass.Floor,   true);
				Abutted.Add(Compass.North,   Cell.Walls.North.Enabled);
				Abutted.Add(Compass.South,   Cell.Walls.South.Enabled);
				Abutted.Add(Compass.West,    false);
				break;
		}
		*/
	}

	#endregion
}