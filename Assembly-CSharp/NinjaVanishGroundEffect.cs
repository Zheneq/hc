// ROGUES
// SERVER
using UnityEngine;

#if SERVER
// added in rogues
public class NinjaVanishGroundEffect : StandardGroundEffect
{
	private int m_selfHealOnTurnStart;

	public NinjaVanishGroundEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		Vector3 shapeFreePos,
		ActorData target,
		ActorData caster,
		GroundEffectField fieldInfo,
		int selfHealOnTurnStart)
		: base(parent, targetSquare, shapeFreePos, target, caster, fieldInfo)
	{
		m_selfHealOnTurnStart = selfHealOnTurnStart;
	}

	public override void OnTurnStart()
	{
		base.OnTurnStart();
		if (m_selfHealOnTurnStart > 0
		    && Caster != null
		    && !Caster.IsDead()
		    && m_affectedSquares.Contains(Caster.GetCurrentBoardSquare()))
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Caster, Caster.GetFreePos()));
			actorHitResults.SetBaseHealing(m_selfHealOnTurnStart);
			MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(Caster, Caster, actorHitResults, Parent.Ability);
		}
	}
}
#endif
