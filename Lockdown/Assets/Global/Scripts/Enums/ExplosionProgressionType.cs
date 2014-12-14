/// <summary>
/// Defines the type of movement through which the explosions
/// will progress through the 3D exploding region:
///  - Expanding: The boundary plane will move through the 3D
///    exploding region, creating a larger and larger space
///    in which a random explosion may occur.
///  - Linear: All explosions will occur on the boundary plane
///    as it linearlly moves through the 3D exploding region.
/// </summary>
public enum ExplosionProgressionType {
	Expanding = 0,
	Linear
}