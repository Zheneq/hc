// ROGUES
// SERVER
// server-only, missing in reactor
#if SERVER
public class MiscHitEventData_UpdateFreelancerStat : MiscHitEventData
{
	public int m_statIndex;

	public int m_changeAmount;

	public ActorData m_caster;

	public MiscHitEventData_UpdateFreelancerStat(int statIndex, int changeAmount, ActorData caster) : base(MiscHitEventType.UpdateFreelancerStats)
	{
		this.m_statIndex = statIndex;
		this.m_changeAmount = changeAmount;
		this.m_caster = caster;
	}

	public override void ExecuteMiscHitEvent(ActorHitResults actorHitResult)
	{
		if (this.m_changeAmount != 0 && this.m_caster != null && this.m_caster.GetFreelancerStats() != null)
		{
			this.m_caster.GetFreelancerStats().AddToValueOfStat(this.m_statIndex, this.m_changeAmount);
		}
	}
}
#endif
