// ROGUES
// SERVER
using System.Collections.Generic;

#if SERVER
public class BlasterOverchargeEffect : StandardActorEffect
{
	private readonly int m_overchargeUses; // custom
	
	public BlasterOverchargeEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		StandardActorEffectData data,
		int overchargeUses) // custom
		: base(parent, targetSquare, target, caster, data)
	{
		m_overchargeUses = overchargeUses; // custom
	}

	public override void OnStart()
	{
		base.OnStart();
		Blaster_SyncComponent component = Caster.GetComponent<Blaster_SyncComponent>();
		// custom
		component.Networkm_overchargeBuffs = component.m_overchargeBuffs + 1;
		component.Networkm_overchargeUses = m_overchargeUses;
		// rogues
		// component.Networkm_overchargeCount = component.m_overchargeCount + 1;
		component.m_lastOverchargeTurn = GameFlowData.Get().CurrentTurn;
	}

	public override void OnMiscHitEventUpdate(List<MiscHitEventEffectUpdateParams> updateParams)
	{
		if (updateParams != null)
		{
			foreach (MiscHitEventEffectUpdateParams param in updateParams)
			{
				if (param is CastCountUpdateParam)
				{
					Blaster_SyncComponent component = Caster.GetComponent<Blaster_SyncComponent>();
					// custom
					component.Networkm_overchargeBuffs = component.m_overchargeBuffs + 1;
					// rogues
					// component.Networkm_overchargeCount = component.m_overchargeCount + 1;
				}
			}
		}
	}

	public override void OnEnd()
	{
		base.OnEnd();
		Blaster_SyncComponent component = Caster.GetComponent<Blaster_SyncComponent>();
		// custom
		component.Networkm_overchargeBuffs = 0;
		component.Networkm_overchargeUses = 0;
		// rogues
		// component.Networkm_overchargeCount = 0;
		component.m_lastOverchargeTurn = GameFlowData.Get().CurrentTurn;
	}

	public override void OnTurnStart()
	{
		base.OnTurnStart();
		Caster.GetComponent<Blaster_SyncComponent>().m_lastOverchargeTurn = GameFlowData.Get().CurrentTurn;
	}

	public class CastCountUpdateParam : MiscHitEventEffectUpdateParams
	{
	}
}
#endif
