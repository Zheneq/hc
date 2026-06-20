// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// empty in rouges
public class BazookaGirlStickyBomb : Ability
{
	public enum TargeterType
	{
		Shape,
		Laser,
		Cone
	}

	[Header("-- Targeting")]
	public TargeterType m_targeterType = TargeterType.Laser;
	public bool m_targeterPenetrateLos;
	public int m_maxTargets = -1;
	[Header("-- Targeting: If Using Laser Targeting")]
	public float m_laserWidth = 1f;
	public float m_laserRange = 5f;
	[Header("-- Targeting: If Using Shape Targeter")]
	public AbilityAreaShape m_targeterShape = AbilityAreaShape.Five_x_Five;
	[Header("-- Targeting: If Using Cone Targeter")]
	public float m_coneWidthAngle = 270f;
	public float m_coneLength = 2.5f;
	[Header("-- Bomb Info")]
	public int m_energyGainOnCastPerEnemyHit;
	public StandardEffectInfo m_enemyOnCastHitEffect;
	public ThiefPartingGiftBombInfo m_bombInfo;
	public SpoilsSpawnData m_spoilSpawnOnExplosion;

	private AbilityMod_BazookaGirlStickyBomb m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Sticky Bomb";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		switch (m_targeterType)
		{
			case TargeterType.Laser:
				Targeter = new AbilityUtil_Targeter_Laser(this, m_laserWidth, m_laserRange, m_targeterPenetrateLos, m_maxTargets);
				break;
			case TargeterType.Shape:
				Targeter = new AbilityUtil_Targeter_Shape(this, m_targeterShape, m_targeterPenetrateLos);
				break;
			default:
				Targeter = new AbilityUtil_Targeter_DirectionCone(this, m_coneWidthAngle, m_coneLength, 0f, m_targeterPenetrateLos, true);
				break;
		}
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return m_coneLength;
	}

	public int GetEnergyGainOnCastPerEnemyHit()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_energyGainOnCastPerEnemyHitMod.GetModifiedValue(m_energyGainOnCastPerEnemyHit) 
			: m_energyGainOnCastPerEnemyHit;
	}

	private StandardEffectInfo GetEnemyOnCastHitEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_enemyOnCastHitEffectOverride.GetModifiedValue(m_enemyOnCastHitEffect)
			: m_enemyOnCastHitEffect;
	}

	private bool HasCooldownModification()
	{
		return m_abilityMod != null
		       && m_abilityMod.m_cooldownModOnAction != AbilityData.ActionType.INVALID_ACTION
		       && m_abilityMod.m_cooldownAddAmount != 0;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_bombInfo.damageAmount);
		return numbers;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BazookaGirlStickyBomb abilityMod = m_abilityMod;
		AddTokenInt(tokens, "Damage", string.Empty, m_bombInfo.damageAmount);
		AddTokenInt(tokens, "EnergyGainOnCastPerEnemyHit", string.Empty, abilityMod != null
			? abilityMod.m_energyGainOnCastPerEnemyHitMod.GetModifiedValue(m_energyGainOnCastPerEnemyHit)
			: m_energyGainOnCastPerEnemyHit);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod != null
			? abilityMod.m_enemyOnCastHitEffectOverride.GetModifiedValue(m_enemyOnCastHitEffect)
			: m_enemyOnCastHitEffect, "EnemyOnCastHitEffect", m_enemyOnCastHitEffect);
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		return Targeter != null && GetEnergyGainOnCastPerEnemyHit() > 0
			? GetEnergyGainOnCastPerEnemyHit() * Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Primary)
			: 0;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BazookaGirlStickyBomb))
		{
			m_abilityMod = abilityMod as AbilityMod_BazookaGirlStickyBomb;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
	}
	
#if SERVER
	// custom
	private StandardEffectInfo GetEnemyOnExplosionHitEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_enemyOnExplosionEffectOverride.GetModifiedValue(null)
			: null;
	}
	
	// custom
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
	
	// custom
	public override void GatherAbilityResults(
		List<AbilityTarget> targets,
		ActorData caster,
		ref AbilityResults abilityResults)
	{
		switch (m_targeterType)
		{
			case TargeterType.Cone:
				GatherAbilityResultsDirectionCone(targets, caster, ref abilityResults);
				break;
			case TargeterType.Laser:
			case TargeterType.Shape:
			default:
				Log.Error($"Cannot gather ability results for {m_targeterType}!");
				break;
		}
	}
	
	// custom
	private void GatherAbilityResultsDirectionCone(
		List<AbilityTarget> targets,
		ActorData caster,
		ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		AbilityTarget currentTarget = targets[0];
		float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(currentTarget.AimDirection);
		List<ActorData> actors = AreaEffectUtils.GetActorsInCone(
			caster.GetLoSCheckPos(),
			coneCenterAngleDegrees,
			m_coneWidthAngle,
			m_coneLength, 
			0f,
			m_targeterPenetrateLos,
			caster,
			caster.GetOtherTeams(),
			nonActorTargetInfo);
		List<ActorData> targetActors = actors.Where(target => target.GetTeam() != caster.GetTeam()).ToList();
		if (!targetActors.IsNullOrEmpty())
		{
			// TODO LOW should effect be created when nobody is hit?
			BazookaGirlStickyBombEffect effect = new BazookaGirlStickyBombEffect(
				AsEffectSource(), 
				targetActors,
				caster,
				m_bombInfo,
				m_spoilSpawnOnExplosion);
			if (GetEnemyOnExplosionHitEffect() != null)
			{
				effect.OverrideOnExplosionEffectInfo(GetEnemyOnExplosionHitEffect());
			}
			if (HasCooldownModification())
			{
				effect.AddMiscEventForExplosionHit(new MiscHitEventData_AddToCasterCooldown(
					m_abilityMod.m_cooldownModOnAction, m_abilityMod.m_cooldownAddAmount));
			}
			foreach (ActorData target in targetActors)
			{
				ActorHitParameters hitParams = new ActorHitParameters(target, caster.GetFreePos());
				ActorHitResults hitResults = new ActorHitResults(0, HitActionType.Damage, GetEnemyOnCastHitEffect(), hitParams);
				hitResults.AddTechPointGainOnCaster(GetEnergyGainOnCastPerEnemyHit());
				if (effect != null)
				{
					hitResults.AddEffect(effect);
					effect = null;
				}
				abilityResults.StoreActorHit(hitResults);
			}
		}
		
		ActorHitParameters hitParamsSelf = new ActorHitParameters(caster, caster.GetFreePos());
		ActorHitResults hitResultsSelf = new ActorHitResults(0, HitActionType.Damage, (StandardEffectInfo) null, hitParamsSelf);
		abilityResults.StoreActorHit(hitResultsSelf);
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}
#endif
}
