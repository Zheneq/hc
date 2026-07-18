// ROGUES
// SERVER
using System.Collections.Generic;
// server-only, missing in reactor

#if SERVER
public class MiscHitEventData_UpdatePassive : MiscHitEventData
{
	private Passive m_passive;

	public List<MiscHitEventPassiveUpdateParams> m_updateParams;

	public MiscHitEventData_UpdatePassive(Passive passive, List<MiscHitEventPassiveUpdateParams> updateParams) : base(MiscHitEventType.UpdatePassive)
	{
		this.m_passive = passive;
		this.m_updateParams = updateParams;
	}

	public override void ExecuteMiscHitEvent(ActorHitResults actorHitResult)
	{
		if (this.m_passive != null && this.m_updateParams != null)
		{
			this.m_passive.OnMiscHitEventUpdate(this.m_updateParams);
		}
	}
}
#endif
