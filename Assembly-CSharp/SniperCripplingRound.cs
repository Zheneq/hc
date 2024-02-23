using System.Collections.Generic;
using UnityEngine;

public class SniperCripplingRound : Ability
{
	public enum ExplosionType
	{
		Shape,
		Cone
	}

	[Header("-- Laser info ------------------------------------------")]
	public int m_laserDamageAmount = 5;
	public float m_laserWidth = 0.5f;
	public float m_laserRange = 5f;
	public bool m_laserPenetrateLos;
	[Header("-- Explosion --------------------------------------------")]
	public int m_explosionDamageAmount = 3;
	public bool m_alwaysExplodeOnPathEnd;
	public bool m_explodeOnEnvironmentHit;
	public bool m_clampMaxRangeToCursorPos;
	public bool m_snapToTargetShapeCenterWhenClampRange;
	public bool m_snapToTargetSquareWhenClampRange;
	public ExplosionType m_explosionType = ExplosionType.Cone;
	[Header("-- If using Shape")]
	public AbilityAreaShape m_explosionShape = AbilityAreaShape.Three_x_Three;
	[Header("-- If using Cone")]
	public float m_coneWidthAngle = 60f;
	public float m_coneLength = 4f;
	public float m_coneBackwardOffset;
	[Header("-- Effects ----------------------------------------------")]
	public StandardEffectInfo m_effectOnLaserHitTargets;
	[Header("-----")]
	public StandardEffectInfo m_effectOnExplosionHitTargets;

	private AbilityMod_SniperCripplingRound m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = string.Empty;
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (m_explosionType == ExplosionType.Cone)
		{
			AbilityUtil_Targeter_LaserWithCone targeter = new AbilityUtil_Targeter_LaserWithCone(
				this,
				m_laserWidth,
				m_laserRange,
				m_laserPenetrateLos, 
				false,
				m_coneWidthAngle,
				m_coneLength,
				m_coneBackwardOffset);
			targeter.SetMaxLaserTargets(GetModdedMaxLaserTargets());
			targeter.SetExplodeOnPathEnd(m_alwaysExplodeOnPathEnd);
			targeter.SetExplodeOnEnvironmentHit(m_explodeOnEnvironmentHit);
			targeter.SetClampToCursorPos(m_clampMaxRangeToCursorPos);
			targeter.SetSnapToTargetSquareWhenClampRange(m_snapToTargetSquareWhenClampRange);
			targeter.SetAddDirectHitActorAsPrimary(GetLaserDamage() > 0);
			Targeter = targeter;
		}
		else
		{
			LaserTargetingInfo laserTargetingInfo = new LaserTargetingInfo
			{
				maxTargets = GetModdedMaxLaserTargets(),
				penetrateLos = m_laserPenetrateLos,
				range = m_laserRange,
				width = m_laserWidth
			};
			AbilityUtil_Targeter_LaserWithShape targeter = new AbilityUtil_Targeter_LaserWithShape(
				this,
				laserTargetingInfo,
				m_explosionShape);
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
		return m_laserRange;
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
		if (tooltipSubjectTypes == null)
		{
			return null;
		}
		int damage = 0;
		if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
		{
			damage += GetLaserDamage();
		}
		if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
		{
			damage += GetExplosionDamage();
		}

		var ints = new Dictionary<AbilityTooltipSymbol, int>();
		ints[AbilityTooltipSymbol.Damage] = damage;
		return ints;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SniperCripplingRound abilityMod_SniperCripplingRound = modAsBase as AbilityMod_SniperCripplingRound;
		AddTokenInt(tokens, "LaserDamageAmount", string.Empty, abilityMod_SniperCripplingRound != null
			? abilityMod_SniperCripplingRound.m_laserDamageMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount);
		AddTokenInt(tokens, "ExplosionDamageAmount", string.Empty, abilityMod_SniperCripplingRound != null
			? abilityMod_SniperCripplingRound.m_explosionDamageMod.GetModifiedValue(m_explosionDamageAmount)
			: m_explosionDamageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnLaserHitTargets, "EffectOnLaserHitTargets", m_effectOnLaserHitTargets);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnExplosionHitTargets, "EffectOnExplosionHitTargets", m_effectOnExplosionHitTargets);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SniperCripplingRound))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}

		m_abilityMod = abilityMod as AbilityMod_SniperCripplingRound;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private int GetLaserDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserDamageMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount;
	}

	private int GetExplosionDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explosionDamageMod.GetModifiedValue(m_explosionDamageAmount)
			: m_explosionDamageAmount;
	}

	private int GetLaserEffectDuration()
	{
		int duration = m_effectOnLaserHitTargets.m_effectData.m_duration;
		if (m_abilityMod != null)
		{
			duration = m_abilityMod.m_enemyHitEffectDurationMod.GetModifiedValue(duration);
		}
		return duration;
	}

	private int GetExplosionEffectDuration()
	{
		int duration = m_effectOnExplosionHitTargets.m_effectData.m_duration;
		if (m_abilityMod != null)
		{
			duration = m_abilityMod.m_enemyHitEffectDurationMod.GetModifiedValue(duration);
		}
		return duration;
	}

	private int GetModdedMaxLaserTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTargetsMod.GetModifiedValue(1)
			: 1;
	}
}
