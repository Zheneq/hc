using System;

[Serializable]
public class MatchResultsStats : ICloneable
{
	public MatchResultsStatline[] FriendlyStatlines;

	public MatchResultsStatline[] EnemyStatlines;

	public int RedScore;

	public int BlueScore;

	public float GameTime;

	public string VictoryCondition;

	public int VictoryConditionTurns;

	public object Clone()
	{
		return MemberwiseClone();
	}
}
