/// <summary>
/// An indicator to indicate the depth of measurement at which a 
/// calculation should occur when measuring the parameters of a 
/// <c>Wall</c> object. Namely, should the calculations take into
/// consideration the presence of an orthogonal <c>Wall</c> object
/// up against this one (inner) when performing the calculations,
/// or should the presence of such a wall be ignored (outer)?
/// </summary>
/// 
public enum Depth {
	Inner = 0,
	Outer
}