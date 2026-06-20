// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// Echo Boost
public class Card_ExtendBuffDuration_Ability : Ability
{
	[Header("-- Status to extend duration --")]
	public int m_extendAmount = 1;
	public List<StatusType> m_buffsToExtend;
	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Buff Enhancer";
		}
		Setup();
	}

	private void Setup()
	{
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			AbilityAreaShape.SingleSquare,
			false,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			false);
		Targeter.ShowArcToShape = false;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "ExtendAmount", string.Empty, m_extendAmount);
	}

#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new List<ServerClientUtils.SequenceStartData>
		{
			new ServerClientUtils.SequenceStartData(
				m_castSequencePrefab,
				caster.GetSquareAtPhaseStart(),
				additionalData.m_abilityResults.HitActorsArray(),
				caster,
				additionalData.m_sequenceSource)
		};
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
		Dictionary<StatusType, int> newBuffDurations = new Dictionary<StatusType, int>();
		ActorStatus actorStatus = caster.GetActorStatus();
		if (actorStatus != null)
		{
			foreach (StatusType buffToExtend in m_buffsToExtend)
			{
				if (actorStatus.HasStatus(buffToExtend) && !newBuffDurations.ContainsKey(buffToExtend))
				{
					int duration = Mathf.Max(1, actorStatus.GetDurationOfStatus(buffToExtend));
					newBuffDurations[buffToExtend] = duration + m_extendAmount;
				}
			}
			Dictionary<int, List<StatusType>> statusChangesByDurationLeft = new Dictionary<int, List<StatusType>>();
			foreach (StatusType buffToExtend in newBuffDurations.Keys)
			{
				int newBuffDuration = newBuffDurations[buffToExtend];
				if (newBuffDuration > 0)
				{
					if (!statusChangesByDurationLeft.ContainsKey(newBuffDuration))
					{
						statusChangesByDurationLeft[newBuffDuration] = new List<StatusType>();
					}
					statusChangesByDurationLeft[newBuffDuration].Add(buffToExtend);
				}
			}
			foreach (int newBuffDuration in statusChangesByDurationLeft.Keys)
			{
				StandardActorEffectData standardActorEffectData = new StandardActorEffectData();
				standardActorEffectData.SetValues(
					"Buff Extend",
					Mathf.Max(1, newBuffDuration),
					0,
					0,
					0,
					ServerCombatManager.HealingType.Effect,
					0,
					0,
					new AbilityStatMod[0],
					statusChangesByDurationLeft[newBuffDuration].ToArray(),
					StandardActorEffectData.StatusDelayMode.NoStatusesDelayToTurnStart);
				StandardActorEffect effect = new StandardActorEffect(
					AsEffectSource(),
					caster.GetCurrentBoardSquare(),
					caster,
					caster,
					standardActorEffectData);
				actorHitResults.AddEffect(effect);
			}
		}
		abilityResults.StoreActorHit(actorHitResults);
	}
#endif
}
