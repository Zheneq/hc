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
			Targeter = new AbilityUtil_Targeter_LaserWithCone(
				this,
				ModdedLaserWidth(),
				ModdedLaserLength(),
				m_penetrateLineOfSight,
				false,
				ModdedConeAngle(),
				ModdedConeLength(),
				m_coneBackwardOffset);
		}
		else
		{
			Targeter = new AbilityUtil_Targeter_Laser(
				this,
				ModdedLaserWidth(),
				ModdedLaserLength(),
				m_penetrateLineOfSight,
				1);
		}
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		float coneLength = 0f;
		if (ShouldExplode())
		{
			coneLength = ModdedConeLength();
		}
		return ModdedLaserLength() + coneLength;
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
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null)
		{
			return null;
		}
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

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SpaceMarineHandCannon abilityMod_SpaceMarineHandCannon = modAsBase as AbilityMod_SpaceMarineHandCannon;
		AddTokenInt(tokens, "PrimaryDamage", string.Empty, abilityMod_SpaceMarineHandCannon != null
			? abilityMod_SpaceMarineHandCannon.m_laserDamageMod.GetModifiedValue(m_primaryDamage)
			: m_primaryDamage);
		AddTokenInt(tokens, "ConeDamage", string.Empty, abilityMod_SpaceMarineHandCannon != null
			? abilityMod_SpaceMarineHandCannon.m_coneDamageMod.GetModifiedValue(m_coneDamage)
			: m_coneDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectInfoOnPrimaryTarget, "EffectInfoOnPrimaryTarget", m_effectInfoOnPrimaryTarget);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectInfoOnConeTargets, "EffectInfoOnConeTargets", m_effectInfoOnConeTargets);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SpaceMarineHandCannon))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		
		m_abilityMod = abilityMod as AbilityMod_SpaceMarineHandCannon;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public int ModdedLaserDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserDamageMod.GetModifiedValue(m_primaryDamage)
			: m_primaryDamage;
	}

	public int ModdedConeDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneDamageMod.GetModifiedValue(m_coneDamage)
			: m_coneDamage;
	}

	public float ModdedConeAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneAngleMod.GetModifiedValue(m_coneWidthAngle)
			: m_coneWidthAngle;
	}

	public float ModdedConeLength()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneLengthMod.GetModifiedValue(m_coneLength)
			: m_coneLength;
	}

	public float ModdedLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserWidthMod.GetModifiedValue(m_primaryWidth)
			: m_primaryWidth;
	}

	public float ModdedLaserLength()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserLengthMod.GetModifiedValue(m_primaryLength)
			: m_primaryLength;
	}

	public bool ShouldExplode()
	{
		return m_abilityMod == null || m_abilityMod.m_shouldExplodeMod.GetModifiedValue(true);
	}

	public StandardEffectInfo GetLaserEffectInfo()
	{
		return m_abilityMod != null && m_abilityMod.m_useLaserHitEffectOverride
			? m_abilityMod.m_laserHitEffectOverride
			: m_effectInfoOnPrimaryTarget;
	}

	public StandardEffectInfo GetConeEffectInfo()
	{
		return m_abilityMod != null && m_abilityMod.m_useConeHitEffectOverride
			? m_abilityMod.m_coneHitEffectOverride
			: m_effectInfoOnConeTargets;
	}
}
