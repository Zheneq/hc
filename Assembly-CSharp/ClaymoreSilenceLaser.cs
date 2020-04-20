using System;
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
	public int m_effectExplosionDamage = 0xA;

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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Dirty Fighting";
		}
		this.SetupTargeter();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetLaserRange();
	}

	private void SetCachedFields()
	{
		StandardActorEffectData cachedEnemyHitEffectData;
		if (this.m_abilityMod)
		{
			cachedEnemyHitEffectData = this.m_abilityMod.m_enemyHitEffectDataMod.GetModifiedValue(this.m_enemyHitEffectData);
		}
		else
		{
			cachedEnemyHitEffectData = this.m_enemyHitEffectData;
		}
		this.m_cachedEnemyHitEffectData = cachedEnemyHitEffectData;
	}

	public float GetLaserRange()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_laserRangeMod.GetModifiedValue(this.m_laserRange);
		}
		else
		{
			result = this.m_laserRange;
		}
		return result;
	}

	public float GetLaserWidth()
	{
		return (!this.m_abilityMod) ? this.m_laserWidth : this.m_abilityMod.m_laserWidthMod.GetModifiedValue(this.m_laserWidth);
	}

	public int GetLaserMaxTargets()
	{
		return (!this.m_abilityMod) ? this.m_laserMaxTargets : this.m_abilityMod.m_laserMaxTargetsMod.GetModifiedValue(this.m_laserMaxTargets);
	}

	public bool GetPenetrateLos()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_penetrateLosMod.GetModifiedValue(this.m_penetrateLos);
		}
		else
		{
			result = this.m_penetrateLos;
		}
		return result;
	}

	public int GetOnCastDamageAmount()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_onCastDamageAmountMod.GetModifiedValue(this.m_onCastDamageAmount);
		}
		else
		{
			result = this.m_onCastDamageAmount;
		}
		return result;
	}

	public StandardActorEffectData GetEnemyHitEffectData()
	{
		StandardActorEffectData result;
		if (this.m_cachedEnemyHitEffectData != null)
		{
			result = this.m_cachedEnemyHitEffectData;
		}
		else
		{
			result = this.m_enemyHitEffectData;
		}
		return result;
	}

	public int GetEffectExplosionDamage()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_effectExplosionDamageMod.GetModifiedValue(this.m_effectExplosionDamage);
		}
		else
		{
			result = this.m_effectExplosionDamage;
		}
		return result;
	}

	public int GetExplosionDamageAfterFirstHit()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_explosionDamageAfterFirstHitMod.GetModifiedValue(this.m_explosionDamageAfterFirstHit);
		}
		else
		{
			result = this.m_explosionDamageAfterFirstHit;
		}
		return result;
	}

	public bool ExplosionReduceCooldownOnlyIfHitByAlly()
	{
		return (!this.m_abilityMod) ? this.m_explosionReduceCooldownOnlyIfHitByAlly : this.m_abilityMod.m_explosionReduceCooldownOnlyIfHitByAllyMod.GetModifiedValue(this.m_explosionReduceCooldownOnlyIfHitByAlly);
	}

	public int GetExplosionCooldownReduction()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_explosionCooldownReductionMod.GetModifiedValue(this.m_explosionCooldownReduction);
		}
		else
		{
			result = this.m_explosionCooldownReduction;
		}
		return result;
	}

	public bool CanExplodeOncePerTurn()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_canExplodeOncePerTurnMod.GetModifiedValue(false);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public int CalcExplosionDamageForOrderIndex(int hitOrder)
	{
		int explosionDamageAfterFirstHit = this.GetExplosionDamageAfterFirstHit();
		if (explosionDamageAfterFirstHit > 0 && hitOrder > 0)
		{
			return explosionDamageAfterFirstHit;
		}
		return this.GetEffectExplosionDamage();
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_Laser(this, this.GetLaserWidth(), this.GetLaserRange(), this.GetPenetrateLos(), this.GetLaserMaxTargets(), false, false);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ClaymoreSilenceLaser abilityMod_ClaymoreSilenceLaser = modAsBase as AbilityMod_ClaymoreSilenceLaser;
		string name = "LaserMaxTargets";
		string empty = string.Empty;
		int val;
		if (abilityMod_ClaymoreSilenceLaser)
		{
			val = abilityMod_ClaymoreSilenceLaser.m_laserMaxTargetsMod.GetModifiedValue(this.m_laserMaxTargets);
		}
		else
		{
			val = this.m_laserMaxTargets;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "OnCastDamageAmount";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_ClaymoreSilenceLaser)
		{
			val2 = abilityMod_ClaymoreSilenceLaser.m_onCastDamageAmountMod.GetModifiedValue(this.m_onCastDamageAmount);
		}
		else
		{
			val2 = this.m_onCastDamageAmount;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		string name3 = "EffectExplosionDamage";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_ClaymoreSilenceLaser)
		{
			val3 = abilityMod_ClaymoreSilenceLaser.m_effectExplosionDamageMod.GetModifiedValue(this.m_effectExplosionDamage);
		}
		else
		{
			val3 = this.m_effectExplosionDamage;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		base.AddTokenInt(tokens, "ExplosionDamageAfterFirstHit", string.Empty, this.m_explosionDamageAfterFirstHit, false);
		StandardActorEffectData standardActorEffectData;
		if (abilityMod_ClaymoreSilenceLaser)
		{
			standardActorEffectData = abilityMod_ClaymoreSilenceLaser.m_enemyHitEffectDataMod.GetModifiedValue(this.m_enemyHitEffectData);
		}
		else
		{
			standardActorEffectData = this.m_enemyHitEffectData;
		}
		StandardActorEffectData standardActorEffectData2 = standardActorEffectData;
		standardActorEffectData2.AddTooltipTokens(tokens, "EnemyHitEffectData", abilityMod_ClaymoreSilenceLaser != null, this.m_enemyHitEffectData);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		if (this.GetOnCastDamageAmount() > 0)
		{
			AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetOnCastDamageAmount());
		}
		else
		{
			AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetEffectExplosionDamage());
		}
		this.GetEnemyHitEffectData().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Tertiary);
		return result;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (this.GetOnCastDamageAmount() <= 0)
		{
			if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary) > 0)
			{
				if (base.Targeter is AbilityUtil_Targeter_Laser)
				{
					AbilityUtil_Targeter_Laser abilityUtil_Targeter_Laser = base.Targeter as AbilityUtil_Targeter_Laser;
					List<AbilityUtil_Targeter_Laser.HitActorContext> hitActorContext = abilityUtil_Targeter_Laser.GetHitActorContext();
					for (int i = 0; i < hitActorContext.Count; i++)
					{
						if (hitActorContext[i].actor == targetActor)
						{
							results.m_damage = this.CalcExplosionDamageForOrderIndex(i);
							return true;
						}
					}
				}
			}
		}
		return true;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ClaymoreSilenceLaser))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_ClaymoreSilenceLaser);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}
}
