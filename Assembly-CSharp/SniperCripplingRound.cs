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
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					AbilityUtil_Targeter_LaserWithCone abilityUtil_Targeter_LaserWithCone = new AbilityUtil_Targeter_LaserWithCone(this, m_laserWidth, m_laserRange, m_laserPenetrateLos, false, m_coneWidthAngle, m_coneLength, m_coneBackwardOffset);
					abilityUtil_Targeter_LaserWithCone.SetMaxLaserTargets(GetModdedMaxLaserTargets());
					abilityUtil_Targeter_LaserWithCone.SetExplodeOnPathEnd(m_alwaysExplodeOnPathEnd);
					abilityUtil_Targeter_LaserWithCone.SetExplodeOnEnvironmentHit(m_explodeOnEnvironmentHit);
					abilityUtil_Targeter_LaserWithCone.SetClampToCursorPos(m_clampMaxRangeToCursorPos);
					abilityUtil_Targeter_LaserWithCone.SetSnapToTargetSquareWhenClampRange(m_snapToTargetSquareWhenClampRange);
					abilityUtil_Targeter_LaserWithCone.SetAddDirectHitActorAsPrimary(GetLaserDamage() > 0);
					base.Targeter = abilityUtil_Targeter_LaserWithCone;
					return;
				}
				}
			}
		}
		LaserTargetingInfo laserTargetingInfo = new LaserTargetingInfo();
		laserTargetingInfo.maxTargets = GetModdedMaxLaserTargets();
		laserTargetingInfo.penetrateLos = m_laserPenetrateLos;
		laserTargetingInfo.range = m_laserRange;
		laserTargetingInfo.width = m_laserWidth;
		AbilityUtil_Targeter_LaserWithShape abilityUtil_Targeter_LaserWithShape = new AbilityUtil_Targeter_LaserWithShape(this, laserTargetingInfo, m_explosionShape);
		abilityUtil_Targeter_LaserWithShape.SetExplodeOnPathEnd(m_alwaysExplodeOnPathEnd);
		abilityUtil_Targeter_LaserWithShape.SetExplodeOnEnvironmentHit(m_explodeOnEnvironmentHit);
		abilityUtil_Targeter_LaserWithShape.SetClampToCursorPos(m_clampMaxRangeToCursorPos);
		abilityUtil_Targeter_LaserWithShape.SetSnapToTargetShapeCenterWhenClampRange(m_snapToTargetShapeCenterWhenClampRange);
		abilityUtil_Targeter_LaserWithShape.SetSnapToTargetSquareWhenClampRange(m_snapToTargetSquareWhenClampRange);
		abilityUtil_Targeter_LaserWithShape.SetAddDirectHitActorAsPrimary(GetLaserDamage() > 0);
		base.Targeter = abilityUtil_Targeter_LaserWithShape;
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
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			int num = 0;
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
			{
				num += GetLaserDamage();
			}
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
			{
				num += GetExplosionDamage();
			}
			Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			dictionary[AbilityTooltipSymbol.Damage] = num;
			return dictionary;
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SniperCripplingRound abilityMod_SniperCripplingRound = modAsBase as AbilityMod_SniperCripplingRound;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_SniperCripplingRound)
		{
			val = abilityMod_SniperCripplingRound.m_laserDamageMod.GetModifiedValue(m_laserDamageAmount);
		}
		else
		{
			val = m_laserDamageAmount;
		}
		AddTokenInt(tokens, "LaserDamageAmount", empty, val);
		AddTokenInt(tokens, "ExplosionDamageAmount", string.Empty, (!abilityMod_SniperCripplingRound) ? m_explosionDamageAmount : abilityMod_SniperCripplingRound.m_explosionDamageMod.GetModifiedValue(m_explosionDamageAmount));
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnLaserHitTargets, "EffectOnLaserHitTargets", m_effectOnLaserHitTargets);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnExplosionHitTargets, "EffectOnExplosionHitTargets", m_effectOnExplosionHitTargets);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SniperCripplingRound))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					m_abilityMod = (abilityMod as AbilityMod_SniperCripplingRound);
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

	private int GetLaserDamage()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = m_laserDamageAmount;
		}
		else
		{
			result = m_abilityMod.m_laserDamageMod.GetModifiedValue(m_laserDamageAmount);
		}
		return result;
	}

	private int GetExplosionDamage()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_explosionDamageMod.GetModifiedValue(m_explosionDamageAmount) : m_explosionDamageAmount;
	}

	private int GetLaserEffectDuration()
	{
		int num = m_effectOnLaserHitTargets.m_effectData.m_duration;
		if (m_abilityMod != null)
		{
			num = m_abilityMod.m_enemyHitEffectDurationMod.GetModifiedValue(num);
		}
		return num;
	}

	private int GetExplosionEffectDuration()
	{
		int num = m_effectOnExplosionHitTargets.m_effectData.m_duration;
		if (m_abilityMod != null)
		{
			num = m_abilityMod.m_enemyHitEffectDurationMod.GetModifiedValue(num);
		}
		return num;
	}

	private int GetModdedMaxLaserTargets()
	{
		if (m_abilityMod != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return m_abilityMod.m_maxTargetsMod.GetModifiedValue(1);
				}
			}
		}
		return 1;
	}
}
