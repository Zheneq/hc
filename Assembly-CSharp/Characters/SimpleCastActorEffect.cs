public class SimpleCastActorEffect : Ability
{
	public StandardActorEffectData m_effectData;

	public AbilityAreaShape m_shape = AbilityAreaShape.Three_x_Three_NoCorners;

	public bool m_penetrateLineOfSight;

	public bool m_friendly;

	private void Start()
	{
		base.Targeter = new AbilityUtil_Targeter_Shape(this, m_shape, m_penetrateLineOfSight, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, !m_friendly, m_friendly);
	}
}
