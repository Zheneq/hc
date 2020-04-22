using System.Collections.Generic;
using UnityEngine;

public class SpaceMarineHandCannon : Ability
{
	public int m_primaryDamage;

	public float m_primaryWidth = 1f;

	public float m_primaryLength = 3f;

	public int m_coneDamage;

	public float m_coneWidthAngle = 60f;

	public float m_coneLength = 4f;

	public float m_coneBackwardOffset;

	public bool m_penetrateLineOfSight;

	public StandardEffectInfo m_effectInfoOnPrimaryTarget;

	public StandardEffectInfo m_effectInfoOnConeTargets;

	private AbilityMod_SpaceMarineHandCannon m_abilityMod;

	private void Start()
	{
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (ShouldExplode())
		{
			base.Targeter = new AbilityUtil_Targeter_LaserWithCone(this, ModdedLaserWidth(), ModdedLaserLength(), m_penetrateLineOfSight, false, ModdedConeAngle(), ModdedConeLength(), m_coneBackwardOffset);
		}
		else
		{
			base.Targeter = new AbilityUtil_Targeter_Laser(this, ModdedLaserWidth(), ModdedLaserLength(), m_penetrateLineOfSight, 1);
		}
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		float num = 0f;
		if (ShouldExplode())
		{
			num = ModdedConeLength();
		}
		return ModdedLaserLength() + num;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, m_primaryDamage));
		m_effectInfoOnPrimaryTarget.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Secondary, m_coneDamage));
		m_effectInfoOnConeTargets.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
				{
					Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
					if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
					{
						dictionary[AbilityTooltipSymbol.Damage] = ModdedLaserDamage();
					}
					else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
					{
						dictionary[AbilityTooltipSymbol.Damage] = ModdedConeDamage();
					}
					return dictionary;
				}
				}
			}
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SpaceMarineHandCannon abilityMod_SpaceMarineHandCannon = modAsBase as AbilityMod_SpaceMarineHandCannon;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_SpaceMarineHandCannon)
		{
			val = abilityMod_SpaceMarineHandCannon.m_laserDamageMod.GetModifiedValue(m_primaryDamage);
		}
		else
		{
			val = m_primaryDamage;
		}
		AddTokenInt(tokens, "PrimaryDamage", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_SpaceMarineHandCannon)
		{
			val2 = abilityMod_SpaceMarineHandCannon.m_coneDamageMod.GetModifiedValue(m_coneDamage);
		}
		else
		{
			val2 = m_coneDamage;
		}
		AddTokenInt(tokens, "ConeDamage", empty2, val2);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectInfoOnPrimaryTarget, "EffectInfoOnPrimaryTarget", m_effectInfoOnPrimaryTarget);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectInfoOnConeTargets, "EffectInfoOnConeTargets", m_effectInfoOnConeTargets);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SpaceMarineHandCannon))
		{
			m_abilityMod = (abilityMod as AbilityMod_SpaceMarineHandCannon);
			SetupTargeter();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public int ModdedLaserDamage()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = m_primaryDamage;
		}
		else
		{
			result = m_abilityMod.m_laserDamageMod.GetModifiedValue(m_primaryDamage);
		}
		return result;
	}

	public int ModdedConeDamage()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = m_coneDamage;
		}
		else
		{
			result = m_abilityMod.m_coneDamageMod.GetModifiedValue(m_coneDamage);
		}
		return result;
	}

	public float ModdedConeAngle()
	{
		float result;
		if (m_abilityMod == null)
		{
			result = m_coneWidthAngle;
		}
		else
		{
			result = m_abilityMod.m_coneAngleMod.GetModifiedValue(m_coneWidthAngle);
		}
		return result;
	}

	public float ModdedConeLength()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_coneLengthMod.GetModifiedValue(m_coneLength) : m_coneLength;
	}

	public float ModdedLaserWidth()
	{
		float result;
		if (m_abilityMod == null)
		{
			result = m_primaryWidth;
		}
		else
		{
			result = m_abilityMod.m_laserWidthMod.GetModifiedValue(m_primaryWidth);
		}
		return result;
	}

	public float ModdedLaserLength()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_laserLengthMod.GetModifiedValue(m_primaryLength) : m_primaryLength;
	}

	public bool ShouldExplode()
	{
		return m_abilityMod == null || m_abilityMod.m_shouldExplodeMod.GetModifiedValue(true);
	}

	public StandardEffectInfo GetLaserEffectInfo()
	{
		if (m_abilityMod != null)
		{
			if (m_abilityMod.m_useLaserHitEffectOverride)
			{
				return m_abilityMod.m_laserHitEffectOverride;
			}
		}
		return m_effectInfoOnPrimaryTarget;
	}

	public StandardEffectInfo GetConeEffectInfo()
	{
		if (m_abilityMod != null)
		{
			if (m_abilityMod.m_useConeHitEffectOverride)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return m_abilityMod.m_coneHitEffectOverride;
					}
				}
			}
		}
		return m_effectInfoOnConeTargets;
	}
}
