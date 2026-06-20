// ROGUES
// SERVER
using UnityEngine;

#if SERVER
// added in rogues
public class ArcherAbsorbGroundEffect : StandardGroundEffect
{
	private int m_lessAbsorbPerTurn = 5;
	private int m_originalAbsorbAmount;

	public ArcherAbsorbGroundEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		Vector3 shapeFreePos,
		ActorData target,
		ActorData caster,
		GroundEffectField fieldInfo,
		int lessAbsorbPerTurn) : base(parent,
		targetSquare,
		shapeFreePos,
		target,
		caster,
		fieldInfo)
	{
		m_originalAbsorbAmount = fieldInfo.effectOnAllies.m_effectData.m_absorbAmount;
		m_lessAbsorbPerTurn = lessAbsorbPerTurn;
	}

	public void OverrideHitPhaseBeforeStart(AbilityPriority hitPhase)
	{
		HitPhase = hitPhase;
	}

	public override void OnTurnEnd()
	{
		base.OnTurnEnd();
		m_fieldInfo.effectOnAllies.m_effectData.m_absorbAmount -= m_lessAbsorbPerTurn;
	}

	public override void OnEnd()
	{
		base.OnEnd();
		m_fieldInfo.effectOnAllies.m_effectData.m_absorbAmount = m_originalAbsorbAmount;
	}

	public override bool AddActorAnimEntryIfHasHits(AbilityPriority phaseIndex)
	{
		return false;
	}
}
#endif
