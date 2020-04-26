using System;

[Serializable]
public class PointsForCharacter
{
	public enum CalculationType
	{
		AtLeastOneMatchingActor,
		PerEachMatchingActor
	}

	public CharacterType m_characterType;

	public CalculationType m_givePointsFor;

	public int m_points;
}
