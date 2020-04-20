using System;
using System.Collections.Generic;
using UnityEngine;

public class SniperPenetratingRound : Ability
{
	[Header("-- Targeting --")]
	public LaserTargetingInfo m_laserInfo;

	[Header("-- On Hit Stuff --")]
	public int m_laserDamageAmount = 5;

	public StandardEffectInfo m_laserHitEffect;

	[Header("-- Bonus Damage from Target Health Threshold (0 to 1) --")]
	public int m_additionalDamageOnLowHealthTarget;

	public float m_lowHealthThreshold;

	private AbilityMod_SniperPenetratingRound m_abilityMod;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Penetrating Round";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (this.CanKnockbackOnHitActors())
		{
			base.Targeter = new AbilityUtil_Targeter_SniperPenetratingRound(this, this.GetLaserWidth(), this.GetLaserRange(), this.m_laserInfo.penetrateLos, this.m_laserInfo.maxTargets, true, this.GetKnockbackThresholdDistance(), this.m_abilityMod.m_knockbackType, this.m_abilityMod.m_knockbackDistance);
		}
		else
		{
			base.Targeter = new AbilityUtil_Targeter_SniperPenetratingRound(this, this.GetLaserWidth(), this.GetLaserRange(), this.m_laserInfo.penetrateLos, this.m_laserInfo.maxTargets);
		}
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetLaserRange();
	}

	public int GetModdedDamage()
	{
		if (this.m_abilityMod != null)
		{
			return this.m_abilityMod.m_laserDamage.GetModifiedValue(this.m_laserDamageAmount);
		}
		return this.m_laserDamageAmount;
	}

	public bool CanKnockbackOnHitActors()
	{
		if (this.m_abilityMod != null)
		{
			if (this.m_abilityMod.m_knockbackHitEnemy)
			{
				if (this.m_abilityMod.m_knockbackDistance > 0f)
				{
					return true;
				}
			}
		}
		return false;
	}

	public float GetLaserWidth()
	{
		float result;
		if (this.m_abilityMod == null)
		{
			result = this.m_laserInfo.width;
		}
		else
		{
			result = this.m_abilityMod.m_laserWidthMod.GetModifiedValue(this.m_laserInfo.width);
		}
		return result;
	}

	public float GetLaserRange()
	{
		float result;
		if (this.m_abilityMod == null)
		{
			result = this.m_laserInfo.range;
		}
		else
		{
			result = this.m_abilityMod.m_laserRangeMod.GetModifiedValue(this.m_laserInfo.range);
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		if (this.m_abilityMod != null && this.m_abilityMod.m_useEnemyHitEffectOverride)
		{
			return this.m_abilityMod.m_enemyHitEffectOverride;
		}
		return this.m_laserHitEffect;
	}

	public float GetKnockbackThresholdDistance()
	{
		float result;
		if (this.m_abilityMod == null)
		{
			result = -1f;
		}
		else
		{
			result = this.m_abilityMod.m_knockbackThresholdDistance;
		}
		return result;
	}

	public int GetAdditionalDamageOnLowHealthTarget()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_additionalDamageOnLowHealthTargetMod.GetModifiedValue(this.m_additionalDamageOnLowHealthTarget);
		}
		else
		{
			result = this.m_additionalDamageOnLowHealthTarget;
		}
		return result;
	}

	public float GetLowHealthThreshold()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_lowHealthThresholdMod.GetModifiedValue(this.m_lowHealthThreshold);
		}
		else
		{
			result = this.m_lowHealthThreshold;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		if (this.GetModdedDamage() > 0)
		{
			AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetModdedDamage());
		}
		this.m_laserHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		if (this.GetLowHealthThreshold() > 0f && this.GetAdditionalDamageOnLowHealthTarget() > 0)
		{
			List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
			if (tooltipSubjectTypes != null)
			{
				if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
				{
					dictionary = new Dictionary<AbilityTooltipSymbol, int>();
					int num;
					if (targetActor.GetHitPointShareOfMax() < this.GetLowHealthThreshold())
					{
						num = this.GetAdditionalDamageOnLowHealthTarget();
					}
					else
					{
						num = 0;
					}
					int num2 = num;
					dictionary[AbilityTooltipSymbol.Damage] = this.GetModdedDamage() + num2;
				}
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SniperPenetratingRound abilityMod_SniperPenetratingRound = modAsBase as AbilityMod_SniperPenetratingRound;
		string name = "LaserDamageAmount";
		string empty = string.Empty;
		int val;
		if (abilityMod_SniperPenetratingRound)
		{
			val = abilityMod_SniperPenetratingRound.m_laserDamage.GetModifiedValue(this.m_laserDamageAmount);
		}
		else
		{
			val = this.m_laserDamageAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_laserHitEffect, "LaserHitEffect", null, false);
		string name2 = "AdditionalDamageOnLowHealthTarget";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_SniperPenetratingRound)
		{
			val2 = abilityMod_SniperPenetratingRound.m_additionalDamageOnLowHealthTargetMod.GetModifiedValue(this.m_additionalDamageOnLowHealthTarget);
		}
		else
		{
			val2 = this.m_additionalDamageOnLowHealthTarget;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SniperPenetratingRound))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_SniperPenetratingRound);
			this.SetupTargeter();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}
}
