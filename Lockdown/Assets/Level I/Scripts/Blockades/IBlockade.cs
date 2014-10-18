/// <summary>
/// An interface which all blockade prefabs must implement in order
/// to open, or grant passage through, the blockade.
/// </summary>
public interface IBlockade {
	#region Public Methods

/// <summary>
/// Open, or grant passage through, the blockade.
/// </summary>
	public void Open();

	#endregion
}