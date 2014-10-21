using UnityEngine;

/// <summary>
/// This class will trigger the walls of a <c>Maze</c> to rise out of
/// the ground and the lasers to shut off when the associated trigger
/// collides with a player.
/// </summary>
public class WallRiser : MonoBehaviour {
	#region Fields

/// <summary>
/// A reference to the destroyer which will destroy level one, if the game
/// does not end within a particular amount of time.
/// </summary>
	public GameObject Destroyer = null;

/// <summary>
/// The visual indication of the trigger.
/// </summary>
	public GameObject Lasers;

/// <summary>
/// A refernece to the <c>Maze</c> whose walls will be rising out of the
/// ground.
/// </summary>
	public GameObject Maze;

	#endregion

	#region Public Methods

/// <summary>
/// When a player collides with the riser, trigger the <c>Maze</c> walls to
/// rise out of the ground and turn off the lasers.
/// </summary>
/// 
/// <param name="c">The object which collided with the trigger</param>
	public void OnTriggerEnter(Collider c) {
		Destroy(Lasers);

	//Sound the alarm and activate the maze
		LOneMaze m = Maze.GetComponent<LOneMaze>();
		m.EnableSliding = true;
		m.SoundAlarm();

	//Activate the destroyer
		if(Destroyer != null) {
			Destroyer.GetComponent<Destroyer>().Activate = true;
		}
	}

	#endregion
}