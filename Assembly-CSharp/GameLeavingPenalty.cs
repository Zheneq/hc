using System;

[Serializable]
public class GameLeavingPenalty
{
	public float PointsGainedForLeaving = 2f;

	public float PointsForgivenForRejoining = 1f;

	public float PointsForgivenPerCompleteGameFinished = 0.5f;
}
