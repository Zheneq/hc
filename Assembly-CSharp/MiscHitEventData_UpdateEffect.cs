// ROGUES
// SERVER
using System.Collections.Generic;
// server-only, missing in reactor
#if SERVER
public class MiscHitEventData_UpdateEffect : MiscHitEventData
{
	public int m_effectGuid;

	public List<MiscHitEventEffectUpdateParams> m_updateParams;

	public MiscHitEventData_UpdateEffect(int effectGuid, List<MiscHitEventEffectUpdateParams> updateParams) : base(MiscHitEventType.UpdateEffect)
	{
		this.m_effectGuid = effectGuid;
		this.m_updateParams = updateParams;
	}

	public override void ExecuteMiscHitEvent(ActorHitResults actorHitResult)
	{
		if (ServerEffectManager.Get() != null)
		{
			bool flag = false;
			foreach (List<Effect> list in ServerEffectManager.Get().GetAllActorEffects().Values)
			{
				foreach (Effect effect in list)
				{
					if (effect.GetEffectGuid() == this.m_effectGuid)
					{
						effect.OnMiscHitEventUpdate(this.m_updateParams);
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				foreach (Effect effect2 in ServerEffectManager.Get().GetWorldEffects())
				{
					if (effect2.GetEffectGuid() == this.m_effectGuid)
					{
						effect2.OnMiscHitEventUpdate(this.m_updateParams);
						break;
					}
				}
			}
		}
	}
}
#endif
