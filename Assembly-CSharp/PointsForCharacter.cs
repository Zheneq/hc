using System;

[Serializable]
public class PointsForCharacter
{
	public CharacterType m_characterType;

	public PointsForCharacter.CalculationType m_givePointsFor;

	public int m_points;

	public enum CalculationType
	{
		AtLeastOneMatchingActor,
		PerEachMatchingActor
	}
}
