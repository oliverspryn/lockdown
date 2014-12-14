using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

#region Delegates

/// <summary>
/// A delegate which is used whenever the exploding region
/// boundary moves.
/// </summary>
public delegate void BoundaryMoved();

/// <summary>
/// A delegate which is used when the time for the explosions
/// has expired.
/// </summary>
public delegate void ExplosionsComplete();

#endregion

/// <summary>
/// When activated, this class will manage all of the random 
/// <c>Explosion</c> objects, which will create fireballs
/// through out the scene.
/// </summary>
public class ExplosionManager : ScriptableObject {
	#region Events

/// <summary>
/// An event to dispatch when the exploding boundary moves.
/// </summary>
	public event BoundaryMoved BoundaryMoved;

/// <summary>
/// An event to dispatch when the time for the explosions has
/// expired.
/// </summary>
	public event ExplosionsComplete Completed;

	#endregion

	#region Private Members

/// <summary>
/// Whether or not to creaate a new explosion.
/// </summary>
	private bool CreateExplosion = false;

/// <summary>
/// The index of the currently exploding fireball, as indicated in
/// the <c>ExplodingTimes</c> array.
/// </summary>
	private int ExplodingIndex = 0;

/// <summary>
/// The 3D space in which explosions may occur.
/// </summary>
	private ExplodingRegion ExplodingRegion;

/// <summary>
/// A pre-calculated list of times at which an explosion will occur.
/// </summary>
	private List<float> ExplodingTimes;

/// <summary>
/// A reference to an object which will display an exploding 
/// fireball.
/// </summary>
	private GameObject Explosion;

/// <summary>
/// The maximum number of explosions which may occur at any one time.
/// </summary>
	private int ExplosionCount;

/// <summary>
/// The overall duration, in seconds, of the set of explosions.
/// </summary>
	private int ExplosionDuration = 10;

/// <summary>
/// Hold the list of <c>GameObjects</c> which represent the exploding
/// fireballs.
/// </summary>
	private LinkedList<GameObject> ExplosionList;

/// <summary>
/// How should the overall movement of the explosions progress as
/// time progresses?
/// </summary>
	private ExplosionProgression ExplosionProgression = ExplosionProgression.Everywhere;

/// <summary>
/// How long, in seconds, is an individual fireball visible? This
/// setting is used for several calculations, and must be calibrated
/// to properly represent the amount of time over which this occurs.
/// </summary>
	public float FireballVisibleDuration = 7.0f;

/// <summary>
/// An object used by C# to prevent multiple threads from modifying
/// an operation-critical data structure.
/// </summary>
	private System.Object MutexLocker = new System.Object();

/// <summary>
/// How the location of the explosions should occur when they move
/// through space.
/// </summary>
	private ExplosionProgressionType ProgressionType;

/// <summary>
/// A Random class which is used for generating much of the random
/// functionality which is created by this class.
/// </summary>
	private System.Random Random = new System.Random();

/// <summary>
/// A point, which will be adjusted over time, indicating the plane
/// along which the explosions will stop appearing.
/// </summary>
	private float RegionCurrent;

/// <summary>
/// The rate, in meters per second, at which the explosion progression
/// will move through the scene.
/// </summary>
	private float RegionMovementRate;

/// <summary>
/// A point, indicating the plane along which the explosions will
/// start appearing.
/// </summary>
	private float RegionStart;

/// <summary>
/// The number of seconds for which the explosions have been ocurring.
/// </summary>
	private int SecondsProgressed = 0;

/// <summary>
/// A <c>Timer</c> which is used to progress the explosion's plane
/// of movement through the 3D explosion region.
/// </summary>
	private Timer TimerMovement = null;

/// <summary>
/// A <c>Timer</c> which is used to set off the explosions, once
/// they are triggered.
/// </summary>
	private Timer TimerRate;

/// <summary>
/// Have the explosions been triggered?
/// </summary>
	private bool Triggered = false;

	#endregion

	#region Constructors

/// <summary>
/// Creates the <c>ExplosionManager</c>, with all <c>Explosion</c>
/// objects inactive.
/// </summary>
/// 
/// <param name="Explosion">A reference to an object which will display an exploding fireball</param>
/// <param name="ExplosionCount">The maximum number of explosions which may occur at any one time</param>
/// <param name="ExplosionDuration">The overall duration, in seconds, of the set of explosions</param>
/// <param name="ExplosionProgression">How should the overall movement of the explosions progress as time progresses?</param>
/// <param name="ProgressionType">How the location of the explosions should occur when they move through space</param>
/// <param name="ExplodingRegion">The 3D space in which explosions may occur</param>
	public ExplosionManager(GameObject Explosion, int ExplosionCount, int ExplosionDuration, ExplosionProgression ExplosionProgression, ExplosionProgressionType ProgressionType, ExplodingRegion ExplodingRegion) {
	//Retain the programmer preferences
		ExplodingIndex = 0;
		ExplodingTimes = new List<float>();
		this.ExplodingRegion = ExplodingRegion;

		this.Explosion = Explosion;
		this.ExplosionCount = ExplosionCount;
		this.ExplosionDuration = ExplosionDuration;
		this.ExplosionProgression = ExplosionProgression;
		this.ProgressionType = ProgressionType;

	//Initialize the exploding times array
		for(int i = 0; i < ExplosionCount; ++i) {
			ExplodingTimes.Add(0.0f);
		}

	//Initialize the explosions
		ExplosionList = new LinkedList<GameObject>();
		GameObject fireball;

		for(int i = 0; i < ExplosionCount; ++i) {
			fireball = Instantiate(Explosion) as GameObject;
			ExplosionList.AddFirst(fireball);
		}

	//Calculate the explosion's rate of movement in m/s
		switch(ExplosionProgression) {
			case ExplosionProgression.BackToFront:
				RegionMovementRate = MS(ExplodingRegion.Forward, ExplodingRegion.Backward);
				RegionStart = ExplodingRegion.Backward;
				RegionCurrent = RegionStart;
				break;

			case ExplosionProgression.BottomToTop:
				RegionMovementRate = MS(ExplodingRegion.Up, ExplodingRegion.Down);
				RegionStart = ExplodingRegion.Down;
				RegionCurrent = RegionStart;
				break;

			case ExplosionProgression.Everywhere:
				RegionMovementRate = -1.0f;
				RegionStart = ExplodingRegion.Left;
				RegionCurrent = ExplodingRegion.Right;
				break;

			case ExplosionProgression.FrontToBack:
				RegionMovementRate = MS(ExplodingRegion.Forward, ExplodingRegion.Backward);
				RegionStart = ExplodingRegion.Forward;
				RegionCurrent = RegionStart;
				break;

			case ExplosionProgression.LeftToRight:
				RegionMovementRate = MS(ExplodingRegion.Left, ExplodingRegion.Right);
				RegionStart = ExplodingRegion.Left;
				RegionCurrent = RegionStart;
				break;

			case ExplosionProgression.RightToLeft:
				RegionMovementRate = MS(ExplodingRegion.Left, ExplodingRegion.Right);
				RegionStart = ExplodingRegion.Right;
				RegionCurrent = RegionStart;
				break;

			case ExplosionProgression.TopToBottom:
				RegionMovementRate = MS(ExplodingRegion.Up, ExplodingRegion.Down);
				RegionStart = ExplodingRegion.Up;
				RegionCurrent = RegionStart;
				break;
		}
	}

	#endregion

	#region Public Methods

/// <summary>
/// Begin the explosions! (Let them rip!!!)
/// </summary>
	public void Trigger() {
		if(!Triggered && SecondsProgressed < ExplosionDuration) {
			GenerateTimes();
			Triggered = true;

		//Start the explosion progression timer
			TimerMovement = new Timer(1000);
			TimerMovement.Elapsed += MoveBoundary;
			TimerMovement.Enabled = true;
			TimerMovement.Start();

		//Start the explosion rate timer
			TimerRate = new Timer(ExplodingTimes[ExplodingIndex] * 1000);
			TimerRate.Elapsed += Explode;
			TimerRate.Enabled = true;
			TimerRate.Start();
		}
	}

/// <summary>
/// Probe the list of available explosions, and trigger one, if 
/// one is avaiable.
/// </summary>
	public void Update() {
		if(!CreateExplosion)
			return;

		GameObject explosion = ExplosionList.First.Value;
		Explosion script = explosion.GetComponent<Explosion>();

	//Probe the list for an available explosion
		if(!script.Exploding) {
		//Put it on the back of the queue
			lock(MutexLocker) {
				LinkedListNode<GameObject> ex = ExplosionList.First;
				ExplosionList.RemoveFirst();
				ExplosionList.AddLast(ex.Value);

				CreateExplosion = false;
			}

		//Place the fireball
			explosion.transform.position = RandomLocation();

		//Explode the fireball
			script.Trigger();

		//Does a new batch of times need generated?
			if(ExplodingIndex >= ExplosionCount) {
				ExplodingIndex = 0;
				GenerateTimes();
			}
		}
	}

	#endregion

	#region Private Methods

/// <summary>
/// Create a new explosion when the Timer expires.
/// </summary>
/// 
/// <param name="sender">The Timer which sent the event</param>
/// <param name="e">The event arguments</param>
	private void Explode(object sender, ElapsedEventArgs e) {
		lock(MutexLocker) {
			CreateExplosion = true;

			TimerRate.Enabled = false;
			TimerRate.Stop();
			TimerRate.Enabled = true;
			TimerRate.Interval = ExplodingTimes[++ExplodingIndex] * 1000;
			TimerRate.Start();
		}
	}

/// <summary>
/// Generate the listing of times at which an explosion will occur.
/// </summary>
	private void GenerateTimes() {
		float difference = 0.0f;
		float partition = FireballVisibleDuration / (float)ExplosionCount;
		float rand = 0.0f;

		for(int i = 0; i < ExplosionCount; ++i) {
			rand = (float)Random.NextDouble() * partition;
			ExplodingTimes[i] = difference + (rand * partition);
			difference = partition - rand;
		}
	}

/// <summary>
/// After each second, move the boundary plane for the 3D 
/// explosion region to create a larger explosion region.
/// </summary>
/// 
/// <param name="sender">The Timer which sent the event</param>
/// <param name="e">The event arguments</param>
	private void MoveBoundary(object sender, ElapsedEventArgs e) {
	//Move the boundary
		switch(ExplosionProgression) {
			case ExplosionProgression.BackToFront:
			case ExplosionProgression.BottomToTop:
			case ExplosionProgression.LeftToRight:
				RegionCurrent += RegionMovementRate;
				break;

			case ExplosionProgression.FrontToBack:
			case ExplosionProgression.RightToLeft:
			case ExplosionProgression.TopToBottom:
				RegionCurrent -= RegionMovementRate;
				break;
		}

	//Dispatch a boundary moved event
		if(BoundaryMoved != null)
			BoundaryMoved();
		
	//Has the given duration of time already elapsed?
		if(SecondsProgressed++ >= ExplosionDuration) {
		//Stop the timers
			TimerMovement.Enabled = false;
			TimerMovement.Stop();
			TimerMovement.Dispose();

			TimerRate.Enabled = false;
			TimerRate.Stop();
			TimerRate.Dispose();

		//Raise the completion event
			if(Completed != null)
				Completed();
		}
	}

/// <summary>
/// Calculate the rate of the explosion progression through the 
/// scene in meters per second.
/// </summary>
/// 
/// <param name="side1">One side of the 3D explosion region</param>
/// <param name="side2">Another side of the 3D explosion region</param>
/// <returns>The rate of the explosion progression, in meters per second</returns>
	private float MS(float side1, float side2) {
	//For this calculation, we need side1 to have the greater value
		if(side1 < side2) {
			float temp = side1;
			side1 = side2;
			side2 = temp;
		}

	//Calculations will vary based on the sign of the numbers
		if((side1 > 0.0f && side2 > 0.0f) || (side1 < 0.0f && side2 < 0.0f)) {
			return Math.Abs(side1 - side2) / ExplosionDuration;
		} else {
			return (Math.Abs(side1) + Math.Abs(side2)) / ExplosionDuration;
		}
	}

/// <summary>
/// Generate a random location in which to place the fireball within
/// the 3D explosion region.
/// </summary>
/// 
/// <returns>A random location within the 3D explosion region</returns>
	private Vector3 RandomLocation() {
		Vector3 v = new Vector3();

		switch(ExplosionProgression) {
			case ExplosionProgression.BackToFront:
				v.x = RandomRange(ExplodingRegion.Left, ExplodingRegion.Right);
				v.y = RandomRange(ExplodingRegion.Down, ExplodingRegion.Up);
				v.z = ProgressionType == ExplosionProgressionType.Linear ? RegionCurrent : RandomRange(RegionStart, RegionCurrent);
				break;

			case ExplosionProgression.BottomToTop:
				v.x = RandomRange(ExplodingRegion.Left, ExplodingRegion.Right);
				v.y = ProgressionType == ExplosionProgressionType.Linear ? RegionCurrent : RandomRange(RegionStart, RegionCurrent);
				v.z = RandomRange(ExplodingRegion.Backward, ExplodingRegion.Forward);
				break;

			case ExplosionProgression.Everywhere:
				v.x = RandomRange(ExplodingRegion.Left, ExplodingRegion.Right);
				v.y = RandomRange(ExplodingRegion.Down, ExplodingRegion.Up);
				v.z = RandomRange(ExplodingRegion.Backward, ExplodingRegion.Forward);
				break;

			case ExplosionProgression.FrontToBack:
				v.x = RandomRange(ExplodingRegion.Left, ExplodingRegion.Right);
				v.y = RandomRange(ExplodingRegion.Down, ExplodingRegion.Up);
				v.z = ProgressionType == ExplosionProgressionType.Linear ? RegionCurrent : RandomRange(RegionStart, RegionCurrent);
				break;

			case ExplosionProgression.LeftToRight:
				v.x = ProgressionType == ExplosionProgressionType.Linear ? RegionCurrent : RandomRange(RegionStart, RegionCurrent);
				v.y = RandomRange(ExplodingRegion.Down, ExplodingRegion.Up);
				v.z = RandomRange(ExplodingRegion.Backward, ExplodingRegion.Forward);
				break;

			case ExplosionProgression.RightToLeft:
				v.x = ProgressionType == ExplosionProgressionType.Linear ? RegionCurrent : RandomRange(RegionStart, RegionCurrent);
				v.y = RandomRange(ExplodingRegion.Down, ExplodingRegion.Up);
				v.z = RandomRange(ExplodingRegion.Backward, ExplodingRegion.Forward);
				break;

			case ExplosionProgression.TopToBottom:
				v.x = RandomRange(ExplodingRegion.Left, ExplodingRegion.Right);
				v.y = ProgressionType == ExplosionProgressionType.Linear ? RegionCurrent : RandomRange(RegionStart, RegionCurrent);
				v.z = RandomRange(ExplodingRegion.Backward, ExplodingRegion.Forward);
				break;
		}

		return v;
	}

/// <summary>
/// Generate a random float value between two floats.
/// </summary>
/// 
/// <param name="minimum">The minimum value of the random float</param>
/// <param name="maximum">The maximum value of the random float</param>
/// <returns>A random float between the provided floats</returns>
	private float RandomRange(float minimum, float maximum) {
		double random;

	//Just checking...
		if(maximum < minimum) {
			float temp = maximum;
			maximum = minimum;
			minimum = temp;
		}

	//Calculations will vary based on the sign of the numbers
		if(minimum > 0.0f && maximum > 0.0f) {
			random = Random.NextDouble() * (maximum - minimum) + minimum;
		} else if(minimum < 0.0f && maximum < 0.0f) {
			random = Random.NextDouble() * (minimum - maximum) + maximum;
		} else {
			random = Random.NextDouble() * (Math.Abs(maximum) + Math.Abs(minimum)) - Math.Abs(minimum);
		}

		return (float)random;
	}

	#endregion
}