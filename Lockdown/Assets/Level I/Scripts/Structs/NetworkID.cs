/// <summary>
/// This class holds a reference to a network object ID.
/// <summary>
public class NetworkID {
	#region Fields

/// <summary>
/// The ID of the network object.
/// </summary>
	public int ID { get; private set; }

	#endregion

	#region Constructors

/// <summary>
/// Hold a reference to the network object ID.
/// </summary>
/// 
/// <param name="ID">The network object ID</param>
	public NetworkID(int ID) {
		this.ID = ID;
	}

	#endregion
}