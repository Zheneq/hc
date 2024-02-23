using System.Collections.Generic;
using UnityEngine;

public class BazookaGirlExplodingLaser : Ability
{
	public enum ExplosionType
	{
		Shape,
		Cone
	}

	[Header("-- Targeting")]
	public bool m_clampMaxRangeToCursorPos;
	public bool m_snapToTargetShapeCenterWhenClampRange;
	public bool m_snapToTargetSquareWhenClampRange;
	[Header("-- Targeting: If using Shape")]
	public AbilityAreaShape m_explosionShape = AbilityAreaShape.Three_x_Three;
	[Header("-- Targeting: If using Cone")]
	public float m_coneWidthAngle = 60f;
	public float m_coneLength = 4f;
	public float m_coneBackwardOffset;
	[Header("-- Laser Params")]
	public float m_laserWidth = 0.5f;
	public float m_laserRange = 5f;
	public bool m_laserIgnoreCover;
	public bool m_laserPenetrateLos;
	[Header("-- Laser Hit")]
	public int m_laserDamageAmount = 5;
	public StandardEffectInfo m_effectOnLaserHitTargets;
	[Header("-- Cooldown reduction on direct laser hit --")]
	public int m_cdrOnDirectHit;
	public AbilityData.ActionType m_cdrTargetActionType = AbilityData.ActionType.INVALID_ACTION;
	[Header("-- Explosion Params")]
	public ExplosionType m_explosionType = ExplosionType.Cone;
	public bool m_alwaysExplodeOnPathEnd;
	public bool m_explodeOnEnvironmentHit;
	public bool m_explosionIgnoreCover;
	public bool m_explosionPenetrateLos;
	[Header("-- Explosion Hit")]
	public int m_explosionDamageAmount = 3;
	public StandardEffectInfo m_effectOnExplosionHitTargets;

	private AbilityMod_BazookaGirlExplodingLaser m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Exploding Laser";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (m_explosionType == ExplosionType.Cone)
		{
			AbilityUtil_Targeter_LaserWithCone targeter = new AbilityUtil_Targeter_LaserWithCone(this, GetLaserWidth(), GetLaserRange(), LaserPenetrateLos(), false, GetConeWidthAngle(), GetConeLength(), GetConeBackwardOffset());
			targeter.SetExplodeOnPathEnd(m_alwaysExplodeOnPathEnd);
			targeter.SetExplodeOnEnvironmentHit(m_explodeOnEnvironmentHit);
			targeter.SetClampToCursorPos(m_clampMaxRangeToCursorPos);
			targeter.SetSnapToTargetSquareWhenClampRange(m_snapToTargetSquareWhenClampRange);
			targeter.SetAddDirectHitActorAsPrimary(GetLaserDamage() > 0);
			targeter.SetCoverAndLosConfig(LaserIgnoreCover(), ExplosionIgnoresCover(), ExplosionPenetrateLos());
			Targeter = targeter;
		}
		else
		{
			LaserTargetingInfo laserTargetingInfo = new LaserTargetingInfo
			{
				maxTargets = 1,
				penetrateLos = LaserPenetrateLos(),
				range = GetLaserRange(),
				width = GetLaserWidth()
			};
			AbilityUtil_Targeter_LaserWithShape targeter = new AbilityUtil_Targeter_LaserWithShape(this, laserTargetingInfo, m_explosionShape);
			targeter.SetExplodeOnPathEnd(m_alwaysExplodeOnPathEnd);
			targeter.SetExplodeOnEnvironmentHit(m_explodeOnEnvironmentHit);
			targeter.SetClampToCursorPos(m_clampMaxRangeToCursorPos);
			targeter.SetSnapToTargetShapeCenterWhenClampRange(m_snapToTargetShapeCenterWhenClampRange);
			targeter.SetSnapToTargetSquareWhenClampRange(m_snapToTargetSquareWhenClampRange);
			targeter.SetAddDirectHitActorAsPrimary(GetLaserDamage() > 0);
			Targeter = targeter;
		}
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserRange() + GetConeLength();
	}

	public float GetConeWidthAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneWidthAngleMod.GetModifiedValue(m_coneWidthAngle)
			: m_coneWidthAngle;
	}

	public float GetConeLength()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_coneLengthMod.GetModifiedValue(m_coneLength) 
			: m_coneLength;
	}

	public float GetConeBackwardOffset()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(m_coneBackwardOffset) 
			: m_coneBackwardOffset;
	}

	public float GetLaserWidth()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth) 
			: m_laserWidth;
	}

	public float GetLaserRange()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_laserRangeMod.GetModifiedValue(m_laserRange) 
			: m_laserRange;
	}

	public bool LaserPenetrateLos()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_laserPenetrateLosMod.GetModifiedValue(m_laserPenetrateLos) 
			: m_laserPenetrateLos;
	}

	public int GetLaserDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserDamageMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount;
	}

	public StandardEffectInfo GetLaserHitEffect()
	{
		StandardEffectInfo result = m_abilityMod != null
			? m_abilityMod.m_laserHitEffectOverride.GetModifiedValue(m_effectOnLaserHitTargets)
			: m_effectOnLaserHitTargets;
		return result;
	}

	public bool LaserIgnoreCover()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserIgnoreCoverMod.GetModifiedValue(m_laserIgnoreCover)
			: m_laserIgnoreCover;
	}

	public int GetCdrOnDirectHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrOnDirectHitMod.GetModifiedValue(m_cdrOnDirectHit)
			: m_cdrOnDirectHit;
	}

	public int GetExplosionDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explosionDamageMod.GetModifiedValue(m_explosionDamageAmount)
			: m_explosionDamageAmount;
	}

	public StandardEffectInfo GetExplosionHitEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explosionEffectOverride.GetModifiedValue(m_effectOnExplosionHitTargets)
			: m_effectOnExplosionHitTargets;
	}

	public bool ExplosionIgnoresCover()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explosionIgnoreCoverMod.GetModifiedValue(m_explosionIgnoreCover)
			: m_explosionIgnoreCover;
	}

	public bool ExplosionPenetrateLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explosionIgnoreLosMod.GetModifiedValue(m_explosionPenetrateLos)
			: m_explosionPenetrateLos;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_laserDamageAmount);
		m_effectOnLaserHitTargets.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, m_explosionDamageAmount);
		m_effectOnExplosionHitTargets.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null) return null;
		int damage = 0;
		if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
		{
			damage += GetLaserDamage();
		}
		if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
		{
			damage += GetExplosionDamage();
		}
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		dictionary[AbilityTooltipSymbol.Damage] = damage;
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BazookaGirlExplodingLaser abilityMod_BazookaGirlExplodingLaser = modAsBase as AbilityMod_BazookaGirlExplodingLaser;
		AddTokenInt(tokens, "LaserDamageAmount", string.Empty, abilityMod_BazookaGirlExplodingLaser != null
			? abilityMod_BazookaGirlExplodingLaser.m_laserDamageMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_BazookaGirlExplodingLaser != null
			? abilityMod_BazookaGirlExplodingLaser.m_laserHitEffectOverride.GetModifiedValue(m_effectOnLaserHitTargets)
			: m_effectOnLaserHitTargets, "EffectOnLaserHitTargets", m_effectOnLaserHitTargets);
		AddTokenInt(tokens, "ExplosionDamageAmount", string.Empty, abilityMod_BazookaGirlExplodingLaser != null
			? abilityMod_BazookaGirlExplodingLaser.m_explosionDamageMod.GetModifiedValue(m_explosionDamageAmount)
			: m_explosionDamageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_BazookaGirlExplodingLaser != null
			? abilityMod_BazookaGirlExplodingLaser.m_explosionEffectOverride.GetModifiedValue(m_effectOnExplosionHitTargets)
			: m_effectOnExplosionHitTargets, "EffectOnExplosionHitTargets", m_effectOnExplosionHitTargets);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_BazookaGirlExplodingLaser))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_BazookaGirlExplodingLaser;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
