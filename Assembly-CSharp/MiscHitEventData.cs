// ROGUES
// SERVER
using UnityEngine;

// server-only, missing in reactor
#if SERVER
public class MiscHitEventData
{
	public MiscHitEventType m_type;

	public MiscHitEventData(MiscHitEventType type)
	{
		this.m_type = type;
	}

	public virtual void ExecuteMiscHitEvent(ActorHitResults actorHitResult)
	{
		ActorHitParameters hitParameters = actorHitResult.m_hitParameters;
		switch (this.m_type)
		{
		case MiscHitEventType.ClearCharacterAbilityCooldowns:
			hitParameters.Target.GetAbilityData().ClearCharacterAbilityCooldowns();
			return;
		case MiscHitEventType.CasterForceChaseTarget:
			if (!ServerActionBuffer.Get().HasPendingForcedChaseRequest(hitParameters.Caster))
			{
				ServerAbilityUtils.ForceChase(hitParameters.Caster, hitParameters.Target, true);
				if (hitParameters.Ability != null)
				{
					AbilityData.ActionType actionTypeOfAbility = hitParameters.Caster.GetAbilityData().GetActionTypeOfAbility(hitParameters.Ability);
					hitParameters.Caster.GetAbilityData().SetQueuedAction(actionTypeOfAbility, false);
					return;
				}
			}
			break;
		case MiscHitEventType.TargetForceChaseCaster:
			if (!hitParameters.Target.GetActorStatus().IsImmuneToForcedChase())
			{
				ServerAbilityUtils.ForceChase(hitParameters.Target, hitParameters.Caster, false);
				hitParameters.Target.GetAbilityData().UnqueueActions();
				return;
			}
			break;
		default:
			Debug.LogError("MiscHitEventData type cannot be handled by base class: " + this.m_type.ToString());
			break;
		}
	}
}
#endif
