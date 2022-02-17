// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StandardGroundEffectInfo
{
	public bool m_applyGroundEffect;
	public GroundEffectField m_groundEffectData;

	public virtual void ReportAbilityTooltipNumbers(ref List<AbilityTooltipNumber> numbers, AbilityTooltipSubject enemySubject, AbilityTooltipSubject allySubject)
	{
		if (m_applyGroundEffect)
		{
			m_groundEffectData.ReportAbilityTooltipNumbers(ref numbers, enemySubject, allySubject);
		}
	}

#if SERVER
	public List<ActorData> GetAffectableActorsInField(AbilityTarget abilityTarget, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		if (m_applyGroundEffect)
		{
			return m_groundEffectData.GetAffectableActorsInField(abilityTarget, caster, nonActorTargetInfo);
		}
		return new List<ActorData>();
	}
#endif

#if SERVER
	public List<ActorData> GetAffectableActorsInField(BoardSquare targetSquare, Vector3 freePos, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		if (m_applyGroundEffect)
		{
			return m_groundEffectData.GetAffectableActorsInField(targetSquare, freePos, caster, nonActorTargetInfo);
		}
		return new List<ActorData>();
	}
#endif

#if SERVER
	public void SetupActorHitResult(ref ActorHitResults hitRes, ActorData caster, BoardSquare targetSquare, int numHits = 1)
	{
		if (m_applyGroundEffect)
		{
			m_groundEffectData.SetupActorHitResult(ref hitRes, caster, targetSquare, numHits);
		}
	}
#endif
}
