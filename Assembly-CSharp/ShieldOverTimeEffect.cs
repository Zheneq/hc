// ROGUES
// SERVER
using UnityEngine;

// added in rogues
#if SERVER
public class ShieldOverTimeEffect : Effect
{
	private int m_shieldPerTurn;
	private int m_delayTurns;

	public ShieldOverTimeEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		AbilityPriority hitPhase,
		int duration,
		int shieldPerTurn,
		int delayTurns,
		GameObject hitSequencePrefab)
		: base(parent, targetSquare, target, caster)
	{
		HitPhase = hitPhase;
		m_time.duration = Mathf.Max(1, duration);
		m_shieldPerTurn = shieldPerTurn;
		m_delayTurns = delayTurns;
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		if (m_time.age >= m_delayTurns)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Target, Target.GetFreePos()));
			int duration = m_time.duration - m_time.age;
			StandardActorEffectData standardActorEffectData = new StandardActorEffectData();
			standardActorEffectData.SetValues("ShieldPerTurn",
				duration,
				0,
				0,
				0,
				ServerCombatManager.HealingType.Effect,
				0,
				m_shieldPerTurn,
				new AbilityStatMod[0],
				new StatusType[0],
				StandardActorEffectData.StatusDelayMode.DefaultBehavior);
			StandardActorEffect effect = new StandardActorEffect(
				Parent, Target.GetCurrentBoardSquare(), Target, Caster, standardActorEffectData);
			actorHitResults.AddEffect(effect);
			effectResults.StoreActorHit(actorHitResults);
		}
	}

	public override bool ShouldEndEarly()
	{
		return base.ShouldEndEarly() || Target.IsDead();
	}
}
#endif
