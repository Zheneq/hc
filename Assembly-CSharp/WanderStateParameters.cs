using System;
using UnityEngine;

[Serializable]
public class WanderStateParameters : StateParameters
{
	[Tooltip("Minimum number of squares I wander to from my current location (not square size). This is a direct line. It does not take into account walls/total squares traversed.")]
	public int MinWanderDistanceInSquares;

	[Tooltip("Maximum number of squares I wander to from my current location (not square size). This is a direct line. It does not take into account walls/total squares traversed.")]
	public int MaxWanderDistanceInSquares = 1;

	[Tooltip("You can assign this to a game object and I will wander within WanderDistanceInSquares to him. If set to null, I wander WanderDistanceInSquares from my current position.")]
	public GameObject WanderRealativeTo;

	[Tooltip("The min/maximum number of time I wait when I reach my new square. Use 0 for no delay.")]
	public int MinWaitTurns;

	[Tooltip("The min/maximum number of time I wait when I reach my new square. Use 0 for no delay.")]
	public int MaxWaitTurns;
}
