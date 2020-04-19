using System;
using System.Collections.Generic;
using UnityEngine;

public class SniperCripplingRound : Ability
{
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

	public SniperCripplingRound.ExplosionType m_explosionType = SniperCripplingRound.ExplosionType.Cone;

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
		if (this.m_abilityName == "Base Ability")
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SniperCripplingRound.Start()).MethodHandle;
			}
			this.m_abilityName = string.Empty;
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (this.m_explosionType == SniperCripplingRound.ExplosionType.Cone)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SniperCripplingRound.SetupTargeter()).MethodHandle;
			}
			AbilityUtil_Targeter_LaserWithCone abilityUtil_Targeter_LaserWithCone = new AbilityUtil_Targeter_LaserWithCone(this, this.m_laserWidth, this.m_laserRange, this.m_laserPenetrateLos, false, this.m_coneWidthAngle, this.m_coneLength, this.m_coneBackwardOffset);
			abilityUtil_Targeter_LaserWithCone.SetMaxLaserTargets(this.GetModdedMaxLaserTargets());
			abilityUtil_Targeter_LaserWithCone.SetExplodeOnPathEnd(this.m_alwaysExplodeOnPathEnd);
			abilityUtil_Targeter_LaserWithCone.SetExplodeOnEnvironmentHit(this.m_explodeOnEnvironmentHit);
			abilityUtil_Targeter_LaserWithCone.SetClampToCursorPos(this.m_clampMaxRangeToCursorPos);
			abilityUtil_Targeter_LaserWithCone.SetSnapToTargetSquareWhenClampRange(this.m_snapToTargetSquareWhenClampRange);
			abilityUtil_Targeter_LaserWithCone.SetAddDirectHitActorAsPrimary(this.GetLaserDamage() > 0);
			base.Targeter = abilityUtil_Targeter_LaserWithCone;
		}
		else
		{
			AbilityUtil_Targeter_LaserWithShape abilityUtil_Targeter_LaserWithShape = new AbilityUtil_Targeter_LaserWithShape(this, new LaserTargetingInfo
			{
				maxTargets = this.GetModdedMaxLaserTargets(),
				penetrateLos = this.m_laserPenetrateLos,
				range = this.m_laserRange,
				width = this.m_laserWidth
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
		return this.m_laserRange;
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
			int num = 0;
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
			{
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(SniperCripplingRound.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
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
		AbilityMod_SniperCripplingRound abilityMod_SniperCripplingRound = modAsBase as AbilityMod_SniperCripplingRound;
		string name = "LaserDamageAmount";
		string empty = string.Empty;
		int val;
		if (abilityMod_SniperCripplingRound)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SniperCripplingRound.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_SniperCripplingRound.m_laserDamageMod.GetModifiedValue(this.m_laserDamageAmount);
		}
		else
		{
			val = this.m_laserDamageAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		base.AddTokenInt(tokens, "ExplosionDamageAmount", string.Empty, (!abilityMod_SniperCripplingRound) ? this.m_explosionDamageAmount : abilityMod_SniperCripplingRound.m_explosionDamageMod.GetModifiedValue(this.m_explosionDamageAmount), false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectOnLaserHitTargets, "EffectOnLaserHitTargets", this.m_effectOnLaserHitTargets, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectOnExplosionHitTargets, "EffectOnExplosionHitTargets", this.m_effectOnExplosionHitTargets, true);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SniperCripplingRound))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SniperCripplingRound.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_SniperCripplingRound);
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

	private int GetLaserDamage()
	{
		int result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SniperCripplingRound.GetLaserDamage()).MethodHandle;
			}
			result = this.m_laserDamageAmount;
		}
		else
		{
			result = this.m_abilityMod.m_laserDamageMod.GetModifiedValue(this.m_laserDamageAmount);
		}
		return result;
	}

	private int GetExplosionDamage()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_explosionDamageMod.GetModifiedValue(this.m_explosionDamageAmount) : this.m_explosionDamageAmount;
	}

	private int GetLaserEffectDuration()
	{
		int num = this.m_effectOnLaserHitTargets.m_effectData.m_duration;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SniperCripplingRound.GetLaserEffectDuration()).MethodHandle;
			}
			num = this.m_abilityMod.m_enemyHitEffectDurationMod.GetModifiedValue(num);
		}
		return num;
	}

	private int GetExplosionEffectDuration()
	{
		int num = this.m_effectOnExplosionHitTargets.m_effectData.m_duration;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SniperCripplingRound.GetExplosionEffectDuration()).MethodHandle;
			}
			num = this.m_abilityMod.m_enemyHitEffectDurationMod.GetModifiedValue(num);
		}
		return num;
	}

	private int GetModdedMaxLaserTargets()
	{
		if (this.m_abilityMod != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SniperCripplingRound.GetModdedMaxLaserTargets()).MethodHandle;
			}
			return this.m_abilityMod.m_maxTargetsMod.GetModifiedValue(1);
		}
		return 1;
	}

	public enum ExplosionType
	{
		Shape,
		Cone
	}
}
