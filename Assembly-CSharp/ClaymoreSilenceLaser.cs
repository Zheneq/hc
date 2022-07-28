using System.Collections.Generic;
using UnityEngine;

public class ClaymoreSilenceLaser : Ability
{
	[Header("-- Targeting")]
	public float m_laserRange = 4f;
	public float m_laserWidth = 1f;
	public int m_laserMaxTargets;
	public bool m_penetrateLos;
	[Header("-- Hit Damage/Effects")]
	public int m_onCastDamageAmount;
	public StandardActorEffectData m_enemyHitEffectData;
	[Header("-- On Reaction Hit/Explosion Triggered")]
	public int m_effectExplosionDamage = 10;
	public int m_explosionDamageAfterFirstHit;
	public bool m_explosionReduceCooldownOnlyIfHitByAlly;
	public int m_explosionCooldownReduction;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;
	public GameObject m_effectOnExplosionSequencePrefab;

	private AbilityMod_ClaymoreSilenceLaser m_abilityMod;
	private StandardActorEffectData m_cachedEnemyHitEffectData;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Dirty Fighting";
		}
		SetupTargeter();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserRange();
	}

	private void SetCachedFields()
	{
		m_cachedEnemyHitEffectData = m_abilityMod != null
			? m_abilityMod.m_enemyHitEffectDataMod.GetModifiedValue(m_enemyHitEffectData)
			: m_enemyHitEffectData;
	}

	public float GetLaserRange()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_laserRangeMod.GetModifiedValue(m_laserRange) 
			: m_laserRange;
	}

	public float GetLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth)
			: m_laserWidth;
	}

	public int GetLaserMaxTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserMaxTargetsMod.GetModifiedValue(m_laserMaxTargets)
			: m_laserMaxTargets;
	}

	public bool GetPenetrateLos()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLos) 
			: m_penetrateLos;
	}

	public int GetOnCastDamageAmount()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_onCastDamageAmountMod.GetModifiedValue(m_onCastDamageAmount) 
			: m_onCastDamageAmount;
	}

	public StandardActorEffectData GetEnemyHitEffectData()
	{
		return m_cachedEnemyHitEffectData ?? m_enemyHitEffectData;
	}

	public int GetEffectExplosionDamage()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_effectExplosionDamageMod.GetModifiedValue(m_effectExplosionDamage) 
			: m_effectExplosionDamage;
	}

	public int GetExplosionDamageAfterFirstHit()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_explosionDamageAfterFirstHitMod.GetModifiedValue(m_explosionDamageAfterFirstHit) 
			: m_explosionDamageAfterFirstHit;
	}

	public bool ExplosionReduceCooldownOnlyIfHitByAlly()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explosionReduceCooldownOnlyIfHitByAllyMod.GetModifiedValue(m_explosionReduceCooldownOnlyIfHitByAlly)
			: m_explosionReduceCooldownOnlyIfHitByAlly;
	}

	public int GetExplosionCooldownReduction()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_explosionCooldownReductionMod.GetModifiedValue(m_explosionCooldownReduction) 
			: m_explosionCooldownReduction;
	}

	public bool CanExplodeOncePerTurn()
	{
		return m_abilityMod != null && m_abilityMod.m_canExplodeOncePerTurnMod.GetModifiedValue(false);
	}

	public int CalcExplosionDamageForOrderIndex(int hitOrder)
	{
		int explosionDamageAfterFirstHit = GetExplosionDamageAfterFirstHit();
		if (explosionDamageAfterFirstHit > 0 && hitOrder > 0)
		{
			return explosionDamageAfterFirstHit;
		}
		return GetEffectExplosionDamage();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_Laser(this, GetLaserWidth(), GetLaserRange(), GetPenetrateLos(), GetLaserMaxTargets());
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ClaymoreSilenceLaser abilityMod_ClaymoreSilenceLaser = modAsBase as AbilityMod_ClaymoreSilenceLaser;
		AddTokenInt(tokens, "LaserMaxTargets", string.Empty, abilityMod_ClaymoreSilenceLaser != null
			? abilityMod_ClaymoreSilenceLaser.m_laserMaxTargetsMod.GetModifiedValue(m_laserMaxTargets)
			: m_laserMaxTargets);
		AddTokenInt(tokens, "OnCastDamageAmount", string.Empty, abilityMod_ClaymoreSilenceLaser != null
			? abilityMod_ClaymoreSilenceLaser.m_onCastDamageAmountMod.GetModifiedValue(m_onCastDamageAmount)
			: m_onCastDamageAmount);
		AddTokenInt(tokens, "EffectExplosionDamage", string.Empty, abilityMod_ClaymoreSilenceLaser != null
			? abilityMod_ClaymoreSilenceLaser.m_effectExplosionDamageMod.GetModifiedValue(m_effectExplosionDamage)
			: m_effectExplosionDamage);
		AddTokenInt(tokens, "ExplosionDamageAfterFirstHit", string.Empty, m_explosionDamageAfterFirstHit);
		StandardActorEffectData enemyHitEffectData = abilityMod_ClaymoreSilenceLaser != null
			? abilityMod_ClaymoreSilenceLaser.m_enemyHitEffectDataMod.GetModifiedValue(m_enemyHitEffectData)
			: m_enemyHitEffectData;
		enemyHitEffectData.AddTooltipTokens(tokens, "EnemyHitEffectData", abilityMod_ClaymoreSilenceLaser != null, m_enemyHitEffectData);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (GetOnCastDamageAmount() > 0)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetOnCastDamageAmount());
		}
		else
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetEffectExplosionDamage());
		}
		GetEnemyHitEffectData().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Tertiary);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (GetOnCastDamageAmount() > 0
		    || Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary) <= 0
		    || !(Targeter is AbilityUtil_Targeter_Laser))
		{
			return true;
		}
		AbilityUtil_Targeter_Laser abilityUtil_Targeter_Laser = Targeter as AbilityUtil_Targeter_Laser;
		List<AbilityUtil_Targeter_Laser.HitActorContext> hitActorContexts = abilityUtil_Targeter_Laser.GetHitActorContext();
		for (int i = 0; i < hitActorContexts.Count; i++)
		{
			AbilityUtil_Targeter_Laser.HitActorContext hitActorContext = hitActorContexts[i];
			if (hitActorContext.actor == targetActor)
			{
				results.m_damage = CalcExplosionDamageForOrderIndex(i);
				break;
			}
		}
		return true;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ClaymoreSilenceLaser))
		{
			m_abilityMod = abilityMod as AbilityMod_ClaymoreSilenceLaser;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
