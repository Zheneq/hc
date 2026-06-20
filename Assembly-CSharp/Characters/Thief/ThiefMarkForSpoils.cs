// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class ThiefMarkForSpoils : Ability
{
	[Header("-- Spoil Spawn Info")]
	public SpoilsSpawnData m_spoilSpawnData; // TODO THIEF unused
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;
	public GameObject m_persistentSequencePrefab;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Mark For Spoils";
		}
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			AbilityAreaShape.SingleSquare,
			false,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			false,
			false,
			AbilityUtil_Targeter.AffectsActor.Always);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>();
	}
	
#if SERVER
	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new ServerClientUtils.SequenceStartData(
			m_castSequencePrefab,
			targets[0].FreePos,
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
		if (!ServerEffectManager.Get().HasEffect(caster, typeof(ThiefCreateSpoilsMarkerEffect)))
		{
			actorHitResults.AddEffect(new ThiefCreateSpoilsMarkerEffect(AsEffectSource(), caster, m_persistentSequencePrefab));
		}
		abilityResults.StoreActorHit(actorHitResults);
	}
#endif
}
