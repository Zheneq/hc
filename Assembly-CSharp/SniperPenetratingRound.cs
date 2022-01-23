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
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Penetrating Round";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (CanKnockbackOnHitActors())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					base.Targeter = new AbilityUtil_Targeter_SniperPenetratingRound(this, GetLaserWidth(), GetLaserRange(), m_laserInfo.penetrateLos, m_laserInfo.maxTargets, true, GetKnockbackThresholdDistance(), m_abilityMod.m_knockbackType, m_abilityMod.m_knockbackDistance);
					return;
				}
			}
		}
		base.Targeter = new AbilityUtil_Targeter_SniperPenetratingRound(this, GetLaserWidth(), GetLaserRange(), m_laserInfo.penetrateLos, m_laserInfo.maxTargets);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserRange();
	}

	public int GetModdedDamage()
	{
		if (m_abilityMod != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return m_abilityMod.m_laserDamage.GetModifiedValue(m_laserDamageAmount);
				}
			}
		}
		return m_laserDamageAmount;
	}

	public bool CanKnockbackOnHitActors()
	{
		if (m_abilityMod != null)
		{
			if (m_abilityMod.m_knockbackHitEnemy)
			{
				if (m_abilityMod.m_knockbackDistance > 0f)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	public float GetLaserWidth()
	{
		float result;
		if (m_abilityMod == null)
		{
			result = m_laserInfo.width;
		}
		else
		{
			result = m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserInfo.width);
		}
		return result;
	}

	public float GetLaserRange()
	{
		float result;
		if (m_abilityMod == null)
		{
			result = m_laserInfo.range;
		}
		else
		{
			result = m_abilityMod.m_laserRangeMod.GetModifiedValue(m_laserInfo.range);
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		if (m_abilityMod != null && m_abilityMod.m_useEnemyHitEffectOverride)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return m_abilityMod.m_enemyHitEffectOverride;
				}
			}
		}
		return m_laserHitEffect;
	}

	public float GetKnockbackThresholdDistance()
	{
		float result;
		if (m_abilityMod == null)
		{
			result = -1f;
		}
		else
		{
			result = m_abilityMod.m_knockbackThresholdDistance;
		}
		return result;
	}

	public int GetAdditionalDamageOnLowHealthTarget()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_additionalDamageOnLowHealthTargetMod.GetModifiedValue(m_additionalDamageOnLowHealthTarget);
		}
		else
		{
			result = m_additionalDamageOnLowHealthTarget;
		}
		return result;
	}

	public float GetLowHealthThreshold()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_lowHealthThresholdMod.GetModifiedValue(m_lowHealthThreshold);
		}
		else
		{
			result = m_lowHealthThreshold;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (GetModdedDamage() > 0)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetModdedDamage());
		}
		m_laserHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		if (GetLowHealthThreshold() > 0f && GetAdditionalDamageOnLowHealthTarget() > 0)
		{
			List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
			if (tooltipSubjectTypes != null)
			{
				if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
				{
					dictionary = new Dictionary<AbilityTooltipSymbol, int>();
					int num;
					if (targetActor.GetHitPointPercent() < GetLowHealthThreshold())
					{
						num = GetAdditionalDamageOnLowHealthTarget();
					}
					else
					{
						num = 0;
					}
					int num2 = num;
					dictionary[AbilityTooltipSymbol.Damage] = GetModdedDamage() + num2;
				}
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SniperPenetratingRound abilityMod_SniperPenetratingRound = modAsBase as AbilityMod_SniperPenetratingRound;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_SniperPenetratingRound)
		{
			val = abilityMod_SniperPenetratingRound.m_laserDamage.GetModifiedValue(m_laserDamageAmount);
		}
		else
		{
			val = m_laserDamageAmount;
		}
		AddTokenInt(tokens, "LaserDamageAmount", empty, val);
		AbilityMod.AddToken_EffectInfo(tokens, m_laserHitEffect, "LaserHitEffect", null, false);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_SniperPenetratingRound)
		{
			val2 = abilityMod_SniperPenetratingRound.m_additionalDamageOnLowHealthTargetMod.GetModifiedValue(m_additionalDamageOnLowHealthTarget);
		}
		else
		{
			val2 = m_additionalDamageOnLowHealthTarget;
		}
		AddTokenInt(tokens, "AdditionalDamageOnLowHealthTarget", empty2, val2);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SniperPenetratingRound))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					m_abilityMod = (abilityMod as AbilityMod_SniperPenetratingRound);
					SetupTargeter();
					return;
				}
			}
		}
		Debug.LogError("Trying to apply wrong type of ability mod");
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
