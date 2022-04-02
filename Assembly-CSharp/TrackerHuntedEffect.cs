// ROGUES
// SERVER
#if SERVER
public class TrackerHuntedEffect : StandardActorEffect
{
	private TrackerDroneTrackerComponent m_droneTracker;

	public TrackerHuntedEffect(EffectSource parent, BoardSquare targetSquare, ActorData target, ActorData caster, StandardActorEffectData effectData, TrackerDroneTrackerComponent droneTracker)
		: base(parent, targetSquare, target, caster, effectData)
	{
		if (m_effectName.Length == 0)
		{
			m_effectName = parent.Ability.m_abilityName;
		}
		m_droneTracker = droneTracker;
	}

	public override void OnStart()
	{
		base.OnStart();
		if (m_droneTracker != null)
		{
			m_droneTracker.AddTrackedActorByIndex(Target.ActorIndex);
		}
	}

	public override void OnEnd()
	{
		base.OnEnd();
		if (m_droneTracker != null)
		{
			m_droneTracker.RemoveTrackedActorByIndex(Target.ActorIndex);
		}
	}
}
#endif
