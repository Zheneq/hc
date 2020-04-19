using System;
using System.Collections.Generic;
using UnityEngine;

public class BazookaGirlExplodingLaser : Ability
{
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
	public BazookaGirlExplodingLaser.ExplosionType m_explosionType = BazookaGirlExplodingLaser.ExplosionType.Cone;

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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Exploding Laser";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (this.m_explosionType == BazookaGirlExplodingLaser.ExplosionType.Cone)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlExplodingLaser.SetupTargeter()).MethodHandle;
			}
			AbilityUtil_Targeter_LaserWithCone abilityUtil_Targeter_LaserWithCone = new AbilityUtil_Targeter_LaserWithCone(this, this.GetLaserWidth(), this.GetLaserRange(), this.LaserPenetrateLos(), false, this.GetConeWidthAngle(), this.GetConeLength(), this.GetConeBackwardOffset());
			abilityUtil_Targeter_LaserWithCone.SetExplodeOnPathEnd(this.m_alwaysExplodeOnPathEnd);
			abilityUtil_Targeter_LaserWithCone.SetExplodeOnEnvironmentHit(this.m_explodeOnEnvironmentHit);
			abilityUtil_Targeter_LaserWithCone.SetClampToCursorPos(this.m_clampMaxRangeToCursorPos);
			abilityUtil_Targeter_LaserWithCone.SetSnapToTargetSquareWhenClampRange(this.m_snapToTargetSquareWhenClampRange);
			abilityUtil_Targeter_LaserWithCone.SetAddDirectHitActorAsPrimary(this.GetLaserDamage() > 0);
			abilityUtil_Targeter_LaserWithCone.SetCoverAndLosConfig(this.LaserIgnoreCover(), this.ExplosionIgnoresCover(), this.ExplosionPenetrateLos());
			base.Targeter = abilityUtil_Targeter_LaserWithCone;
		}
		else
		{
			AbilityUtil_Targeter_LaserWithShape abilityUtil_Targeter_LaserWithShape = new AbilityUtil_Targeter_LaserWithShape(this, new LaserTargetingInfo
			{
				maxTargets = 1,
				penetrateLos = this.LaserPenetrateLos(),
				range = this.GetLaserRange(),
				width = this.GetLaserWidth()
			}, this.m_explosionShape);
			abilityUtil_Targeter_LaserWithShape.SetExplodeOnPathEnd(this.m_alwaysExplodeOnPathEnd);
			abilityUtil_Targeter_LaserWithShape.SetExplodeOnEnvironmentHit(this.m_explodeOnEnvironmentHit);
			abilityUtil_Targeter_LaserWithShape.SetClampToCursorPos(this.m_clampMaxRangeToCursorPos);
			abilityUtil_Targeter_LaserWithShape.SetSnapToTargetShapeCenterWhenClampRange(this.m_snapToTargetShapeCenterWhenClampRange);
			abilityUtil_Targeter_LaserWithShape.SetSnapToTargetSquareWhenClampRange(this.m_snapToTargetSquareWhenClampRange);
			abilityUtil_Targeter_LaserWithShape.SetAddDirectHitActorAsPrimary(this.GetLaserDamage() > 0);
			base.Targeter = abilityUtil_Targeter_LaserWithShape;
		}
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetLaserRange() + this.GetConeLength();
	}

	public float GetConeWidthAngle()
	{
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlExplodingLaser.GetConeWidthAngle()).MethodHandle;
			}
			result = this.m_abilityMod.m_coneWidthAngleMod.GetModifiedValue(this.m_coneWidthAngle);
		}
		else
		{
			result = this.m_coneWidthAngle;
		}
		return result;
	}

	public float GetConeLength()
	{
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlExplodingLaser.GetConeLength()).MethodHandle;
			}
			result = this.m_abilityMod.m_coneLengthMod.GetModifiedValue(this.m_coneLength);
		}
		else
		{
			result = this.m_coneLength;
		}
		return result;
	}

	public float GetConeBackwardOffset()
	{
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlExplodingLaser.GetConeBackwardOffset()).MethodHandle;
			}
			result = this.m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(this.m_coneBackwardOffset);
		}
		else
		{
			result = this.m_coneBackwardOffset;
		}
		return result;
	}

	public float GetLaserWidth()
	{
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlExplodingLaser.GetLaserWidth()).MethodHandle;
			}
			result = this.m_abilityMod.m_laserWidthMod.GetModifiedValue(this.m_laserWidth);
		}
		else
		{
			result = this.m_laserWidth;
		}
		return result;
	}

	public float GetLaserRange()
	{
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlExplodingLaser.GetLaserRange()).MethodHandle;
			}
			result = this.m_abilityMod.m_laserRangeMod.GetModifiedValue(this.m_laserRange);
		}
		else
		{
			result = this.m_laserRange;
		}
		return result;
	}

	public bool LaserPenetrateLos()
	{
		bool result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlExplodingLaser.LaserPenetrateLos()).MethodHandle;
			}
			result = this.m_abilityMod.m_laserPenetrateLosMod.GetModifiedValue(this.m_laserPenetrateLos);
		}
		else
		{
			result = this.m_laserPenetrateLos;
		}
		return result;
	}

	public int GetLaserDamage()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlExplodingLaser.GetLaserDamage()).MethodHandle;
			}
			result = this.m_laserDamageAmount;
		}
		else
		{
			result = this.m_abilityMod.m_laserDamageMod.GetModifiedValue(this.m_laserDamageAmount);
		}
		return result;
	}

	public StandardEffectInfo GetLaserHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_abilityMod == null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlExplodingLaser.GetLaserHitEffect()).MethodHandle;
			}
			result = this.m_effectOnLaserHitTargets;
		}
		else
		{
			result = this.m_abilityMod.m_laserHitEffectOverride.GetModifiedValue(this.m_effectOnLaserHitTargets);
		}
		return result;
	}

	public bool LaserIgnoreCover()
	{
		bool result;
		if (this.m_abilityMod == null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlExplodingLaser.LaserIgnoreCover()).MethodHandle;
			}
			result = this.m_laserIgnoreCover;
		}
		else
		{
			result = this.m_abilityMod.m_laserIgnoreCoverMod.GetModifiedValue(this.m_laserIgnoreCover);
		}
		return result;
	}

	public int GetCdrOnDirectHit()
	{
		return (!this.m_abilityMod) ? this.m_cdrOnDirectHit : this.m_abilityMod.m_cdrOnDirectHitMod.GetModifiedValue(this.m_cdrOnDirectHit);
	}

	public int GetExplosionDamage()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlExplodingLaser.GetExplosionDamage()).MethodHandle;
			}
			result = this.m_explosionDamageAmount;
		}
		else
		{
			result = this.m_abilityMod.m_explosionDamageMod.GetModifiedValue(this.m_explosionDamageAmount);
		}
		return result;
	}

	public StandardEffectInfo GetExplosionHitEffect()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_explosionEffectOverride.GetModifiedValue(this.m_effectOnExplosionHitTargets) : this.m_effectOnExplosionHitTargets;
	}

	public bool ExplosionIgnoresCover()
	{
		bool result;
		if (this.m_abilityMod == null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlExplodingLaser.ExplosionIgnoresCover()).MethodHandle;
			}
			result = this.m_explosionIgnoreCover;
		}
		else
		{
			result = this.m_abilityMod.m_explosionIgnoreCoverMod.GetModifiedValue(this.m_explosionIgnoreCover);
		}
		return result;
	}

	public bool ExplosionPenetrateLos()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_explosionIgnoreLosMod.GetModifiedValue(this.m_explosionPenetrateLos) : this.m_explosionPenetrateLos;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_laserDamageAmount);
		this.m_effectOnLaserHitTargets.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Secondary, this.m_explosionDamageAmount);
		this.m_effectOnExplosionHitTargets.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Secondary);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlExplodingLaser.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			int num = 0;
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				num += this.GetLaserDamage();
			}
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				num += this.GetExplosionDamage();
			}
			Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			dictionary[AbilityTooltipSymbol.Damage] = num;
			return dictionary;
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BazookaGirlExplodingLaser abilityMod_BazookaGirlExplodingLaser = modAsBase as AbilityMod_BazookaGirlExplodingLaser;
		string name = "LaserDamageAmount";
		string empty = string.Empty;
		int val;
		if (abilityMod_BazookaGirlExplodingLaser)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlExplodingLaser.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_BazookaGirlExplodingLaser.m_laserDamageMod.GetModifiedValue(this.m_laserDamageAmount);
		}
		else
		{
			val = this.m_laserDamageAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		StandardEffectInfo effectInfo;
		if (abilityMod_BazookaGirlExplodingLaser)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			effectInfo = abilityMod_BazookaGirlExplodingLaser.m_laserHitEffectOverride.GetModifiedValue(this.m_effectOnLaserHitTargets);
		}
		else
		{
			effectInfo = this.m_effectOnLaserHitTargets;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectOnLaserHitTargets", this.m_effectOnLaserHitTargets, true);
		base.AddTokenInt(tokens, "ExplosionDamageAmount", string.Empty, (!abilityMod_BazookaGirlExplodingLaser) ? this.m_explosionDamageAmount : abilityMod_BazookaGirlExplodingLaser.m_explosionDamageMod.GetModifiedValue(this.m_explosionDamageAmount), false);
		StandardEffectInfo effectInfo2;
		if (abilityMod_BazookaGirlExplodingLaser)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			effectInfo2 = abilityMod_BazookaGirlExplodingLaser.m_explosionEffectOverride.GetModifiedValue(this.m_effectOnExplosionHitTargets);
		}
		else
		{
			effectInfo2 = this.m_effectOnExplosionHitTargets;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EffectOnExplosionHitTargets", this.m_effectOnExplosionHitTargets, true);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BazookaGirlExplodingLaser))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_BazookaGirlExplodingLaser);
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

	public enum ExplosionType
	{
		Shape,
		Cone
	}
}
