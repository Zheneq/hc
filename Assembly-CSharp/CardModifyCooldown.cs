// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// Brain Juice
public class CardModifyCooldown : Ability
{
	[Header("-- Whether to delay cooldown modification till end of combat --")]
	public bool m_modifyCooldownOnEndOfCombat;
	[Header("-- Cooldown Modify Amount")]
	public int m_cooldownModificationAmount = -1;
	public int m_minCooldownAmount;
	public int m_maxCooldownAmount = 100;
	[Header("-- Affected Abilities")]
	public bool m_reduceAbility0 = true;
	public bool m_reduceAbility1 = true;
	public bool m_reduceAbility2 = true;
	public bool m_reduceAbility3 = true;
	public bool m_reduceAbility4 = true;
	[Header("-- Targeting")]
	public bool m_shapeCenterAroundCaster = true;
	public AbilityAreaShape m_shape;
	public bool m_penetrateLineOfSight = true;
	public bool m_friendly = true;

#if SERVER
	private bool[] m_modifyAbilities;  // added in rogues
#endif

	private void Start()
	{
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			m_shape,
			true,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			!m_friendly,
			m_friendly);
		Targeter.ShowArcToShape = false;
#if SERVER
		// added in rogues
		m_modifyAbilities = new[]
		{
			m_reduceAbility0,
			m_reduceAbility1,
			m_reduceAbility2,
			m_reduceAbility3,
			m_reduceAbility4
		};
#endif
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "CooldownModificationAmount", string.Empty, Mathf.Abs(m_cooldownModificationAmount));
		AddTokenInt(tokens, "MinCooldownAmount", string.Empty, m_minCooldownAmount);
		AddTokenInt(tokens, "MaxCooldownAmount", string.Empty, m_maxCooldownAmount);
	}

#if SERVER
	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new ServerClientUtils.SequenceStartData(
			AsEffectSource().GetSequencePrefab(),
			caster.GetFreePos(),
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		Vector3 freePos = targets[0].FreePos;
		BoardSquare centerSquare = Board.Get().GetSquare(targets[0].GridPos);
		if (m_shapeCenterAroundCaster)
		{
			freePos = caster.GetFreePos();
			centerSquare = caster.GetCurrentBoardSquare();
		}
		List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
			m_shape,
			freePos,
			centerSquare,
			m_penetrateLineOfSight,
			caster,
			m_friendly ? caster.GetTeamAsList() : caster.GetOtherTeams(),
			null);
		foreach (ActorData actorData in actorsInShape)
		{
			if (actorData.GetComponent<AbilityData>() == null)
			{
				continue;
			}
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, actorData.GetFreePos()));
			if (m_modifyCooldownOnEndOfCombat && GetRunPriority() != AbilityPriority.Combat_Final)
			{
				List<AbilityData.ActionType> list = new List<AbilityData.ActionType>();
				for (AbilityData.ActionType i = AbilityData.ActionType.ABILITY_0; i <= AbilityData.ActionType.ABILITY_4; i++)
				{
					if (m_modifyAbilities[(int)i])
					{
						list.Add(i);
					}
				}
				CardModifyCooldownEffect effect = new CardModifyCooldownEffect(
					AsEffectSource(),
					actorData,
					caster,
					list,
					m_cooldownModificationAmount);
				actorHitResults.AddEffect(effect);
			}
			else
			{
				for (AbilityData.ActionType i = AbilityData.ActionType.ABILITY_0; i <= AbilityData.ActionType.ABILITY_4; i++)
				{
					if (m_modifyAbilities[(int)i])
					{
						actorHitResults.AddMiscHitEvent(
							new MiscHitEventData_AddToCasterCooldown(i, m_cooldownModificationAmount));
						actorHitResults.AddMiscHitEvent(
							new MiscHitEventData_ProgressCasterStockRefreshTime(i, -1 * m_cooldownModificationAmount));
					}
				}
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
	}

	// added in rogues
	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		return null;
	}
#endif
}
