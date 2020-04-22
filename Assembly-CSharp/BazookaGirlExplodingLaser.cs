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
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					AbilityUtil_Targeter_LaserWithCone abilityUtil_Targeter_LaserWithCone = new AbilityUtil_Targeter_LaserWithCone(this, GetLaserWidth(), GetLaserRange(), LaserPenetrateLos(), false, GetConeWidthAngle(), GetConeLength(), GetConeBackwardOffset());
					abilityUtil_Targeter_LaserWithCone.SetExplodeOnPathEnd(m_alwaysExplodeOnPathEnd);
					abilityUtil_Targeter_LaserWithCone.SetExplodeOnEnvironmentHit(m_explodeOnEnvironmentHit);
					abilityUtil_Targeter_LaserWithCone.SetClampToCursorPos(m_clampMaxRangeToCursorPos);
					abilityUtil_Targeter_LaserWithCone.SetSnapToTargetSquareWhenClampRange(m_snapToTargetSquareWhenClampRange);
					abilityUtil_Targeter_LaserWithCone.SetAddDirectHitActorAsPrimary(GetLaserDamage() > 0);
					abilityUtil_Targeter_LaserWithCone.SetCoverAndLosConfig(LaserIgnoreCover(), ExplosionIgnoresCover(), ExplosionPenetrateLos());
					base.Targeter = abilityUtil_Targeter_LaserWithCone;
					return;
				}
				}
			}
		}
		LaserTargetingInfo laserTargetingInfo = new LaserTargetingInfo();
		laserTargetingInfo.maxTargets = 1;
		laserTargetingInfo.penetrateLos = LaserPenetrateLos();
		laserTargetingInfo.range = GetLaserRange();
		laserTargetingInfo.width = GetLaserWidth();
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
		return GetLaserRange() + GetConeLength();
	}

	public float GetConeWidthAngle()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_coneWidthAngleMod.GetModifiedValue(m_coneWidthAngle);
		}
		else
		{
			result = m_coneWidthAngle;
		}
		return result;
	}

	public float GetConeLength()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_coneLengthMod.GetModifiedValue(m_coneLength);
		}
		else
		{
			result = m_coneLength;
		}
		return result;
	}

	public float GetConeBackwardOffset()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(m_coneBackwardOffset);
		}
		else
		{
			result = m_coneBackwardOffset;
		}
		return result;
	}

	public float GetLaserWidth()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth);
		}
		else
		{
			result = m_laserWidth;
		}
		return result;
	}

	public float GetLaserRange()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_laserRangeMod.GetModifiedValue(m_laserRange);
		}
		else
		{
			result = m_laserRange;
		}
		return result;
	}

	public bool LaserPenetrateLos()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_laserPenetrateLosMod.GetModifiedValue(m_laserPenetrateLos);
		}
		else
		{
			result = m_laserPenetrateLos;
		}
		return result;
	}

	public int GetLaserDamage()
	{
		int result;
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_laserDamageAmount;
		}
		else
		{
			result = m_abilityMod.m_laserDamageMod.GetModifiedValue(m_laserDamageAmount);
		}
		return result;
	}

	public StandardEffectInfo GetLaserHitEffect()
	{
		StandardEffectInfo result;
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_effectOnLaserHitTargets;
		}
		else
		{
			result = m_abilityMod.m_laserHitEffectOverride.GetModifiedValue(m_effectOnLaserHitTargets);
		}
		return result;
	}

	public bool LaserIgnoreCover()
	{
		bool result;
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_laserIgnoreCover;
		}
		else
		{
			result = m_abilityMod.m_laserIgnoreCoverMod.GetModifiedValue(m_laserIgnoreCover);
		}
		return result;
	}

	public int GetCdrOnDirectHit()
	{
		return (!m_abilityMod) ? m_cdrOnDirectHit : m_abilityMod.m_cdrOnDirectHitMod.GetModifiedValue(m_cdrOnDirectHit);
	}

	public int GetExplosionDamage()
	{
		int result;
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_explosionDamageAmount;
		}
		else
		{
			result = m_abilityMod.m_explosionDamageMod.GetModifiedValue(m_explosionDamageAmount);
		}
		return result;
	}

	public StandardEffectInfo GetExplosionHitEffect()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_explosionEffectOverride.GetModifiedValue(m_effectOnExplosionHitTargets) : m_effectOnExplosionHitTargets;
	}

	public bool ExplosionIgnoresCover()
	{
		bool result;
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_explosionIgnoreCover;
		}
		else
		{
			result = m_abilityMod.m_explosionIgnoreCoverMod.GetModifiedValue(m_explosionIgnoreCover);
		}
		return result;
	}

	public bool ExplosionPenetrateLos()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_explosionIgnoreLosMod.GetModifiedValue(m_explosionPenetrateLos) : m_explosionPenetrateLos;
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
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					int num = 0;
					if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						num += GetLaserDamage();
					}
					if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						num += GetExplosionDamage();
					}
					Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
					dictionary[AbilityTooltipSymbol.Damage] = num;
					return dictionary;
				}
				}
			}
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BazookaGirlExplodingLaser abilityMod_BazookaGirlExplodingLaser = modAsBase as AbilityMod_BazookaGirlExplodingLaser;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_BazookaGirlExplodingLaser)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			val = abilityMod_BazookaGirlExplodingLaser.m_laserDamageMod.GetModifiedValue(m_laserDamageAmount);
		}
		else
		{
			val = m_laserDamageAmount;
		}
		AddTokenInt(tokens, "LaserDamageAmount", empty, val);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_BazookaGirlExplodingLaser)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			effectInfo = abilityMod_BazookaGirlExplodingLaser.m_laserHitEffectOverride.GetModifiedValue(m_effectOnLaserHitTargets);
		}
		else
		{
			effectInfo = m_effectOnLaserHitTargets;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectOnLaserHitTargets", m_effectOnLaserHitTargets);
		AddTokenInt(tokens, "ExplosionDamageAmount", string.Empty, (!abilityMod_BazookaGirlExplodingLaser) ? m_explosionDamageAmount : abilityMod_BazookaGirlExplodingLaser.m_explosionDamageMod.GetModifiedValue(m_explosionDamageAmount));
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_BazookaGirlExplodingLaser)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			effectInfo2 = abilityMod_BazookaGirlExplodingLaser.m_explosionEffectOverride.GetModifiedValue(m_effectOnExplosionHitTargets);
		}
		else
		{
			effectInfo2 = m_effectOnExplosionHitTargets;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EffectOnExplosionHitTargets", m_effectOnExplosionHitTargets);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BazookaGirlExplodingLaser))
		{
			m_abilityMod = (abilityMod as AbilityMod_BazookaGirlExplodingLaser);
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
}
