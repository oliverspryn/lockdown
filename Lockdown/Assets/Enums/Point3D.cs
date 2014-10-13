/// <summary>
/// An indicator which will be used in a <c>Dictionary</c> to map a pre-
/// calculated grid points in 3D to a specific 3D point.
/// </summary>

public enum Point3D {
//X direction
	LLeft = 0,
	Left,
	Center,
	Right,
	RRight,

//Y direction
	UUp,
	Up,
	//Center, already there
	Down,
	DDown,

//Z direction
	FForward,
	Forward,
	//Center, already there
	Backward,
	BBackward
}