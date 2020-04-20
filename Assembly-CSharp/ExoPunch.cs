using System;
using System.Collections.Generic;
using UnityEngine;

public class ExoPunch : Ability
{
	[Header("-- Targeting")]
	public float m_coneWidthAngle = 180f;

	public float m_coneBackwardOffset;

	public float m_coneLength = 2.5f;

	public bool m_penetrateLineOfSight;

	public int m_maxTargets = 5;

	[Header("-- Knockback")]
	public float m_knockbackDistance;

	public KnockbackType m_knockbackType;

	[Header("-- On Hit Damage/Effect")]
	public int m_damageAmount = 0x14;

	public StandardEffectInfo m_targetHitEffect;

	[Header("-- Nearby Hit Bonus")]
	public float m_nearDistThreshold;

	public int m_nearEnemyExtraDamage;

	public StandardEffectInfo m_nearEnemyExtraEffect;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_ExoPunch m_abilityMod;

	private StandardEffectInfo m_cachedTargetHitEffect;

	private StandardEffectInfo m_cachedNearEnemyExtraEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Exollent Hit";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		AbilityUtil_Targeter_StretchCone abilityUtil_Targeter_StretchCone = new AbilityUtil_Targeter_StretchCone(this, this.GetConeLength(), this.GetConeLength(), this.GetConeWidthAngle(), this.GetConeWidthAngle(), AreaEffectUtils.StretchConeStyle.Linear, this.GetConeBackwardOffset(), this.PenetrateLineOfSight());
		abilityUtil_Targeter_StretchCone.InitKnockbackData(this.GetKnockbackDistance(), this.GetKnockbackType(), 0f, KnockbackType.AwayFromSource);
		base.Targeter = abilityUtil_Targeter_StretchCone;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetConeLength();
	}

	private void SetCachedFields()
	{
		this.m_cachedTargetHitEffect = ((!this.m_abilityMod) ? this.m_targetHitEffect : this.m_abilityMod.m_targetHitEffectMod.GetModifiedValue(this.m_targetHitEffect));
		StandardEffectInfo cachedNearEnemyExtraEffect;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoPunch.SetCachedFields()).MethodHandle;
			}
			cachedNearEnemyExtraEffect = this.m_abilityMod.m_nearEnemyExtraEffectMod.GetModifiedValue(this.m_nearEnemyExtraEffect);
		}
		else
		{
			cachedNearEnemyExtraEffect = this.m_nearEnemyExtraEffect;
		}
		this.m_cachedNearEnemyExtraEffect = cachedNearEnemyExtraEffect;
	}

	public float GetConeWidthAngle()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoPunch.GetConeWidthAngle()).MethodHandle;
			}
			result = this.m_abilityMod.m_coneWidthAngleMod.GetModifiedValue(this.m_coneWidthAngle);
		}
		else
		{
			result = this.m_coneWidthAngle;
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
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoPunch.GetConeBackwardOffset()).MethodHandle;
			}
			result = this.m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(this.m_coneBackwardOffset);
		}
		else
		{
			result = this.m_coneBackwardOffset;
		}
		return result;
	}

	public float GetConeLength()
	{
		return (!this.m_abilityMod) ? this.m_coneLength : this.m_abilityMod.m_coneLengthMod.GetModifiedValue(this.m_coneLength);
	}

	public bool PenetrateLineOfSight()
	{
		return (!this.m_abilityMod) ? this.m_penetrateLineOfSight : this.m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(this.m_penetrateLineOfSight);
	}

	public int GetMaxTargets()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoPunch.GetMaxTargets()).MethodHandle;
			}
			result = this.m_abilityMod.m_maxTargetsMod.GetModifiedValue(this.m_maxTargets);
		}
		else
		{
			result = this.m_maxTargets;
		}
		return result;
	}

	public float GetKnockbackDistance()
	{
		return (!this.m_abilityMod) ? this.m_knockbackDistance : this.m_abilityMod.m_knockbackDistanceMod.GetModifiedValue(this.m_knockbackDistance);
	}

	public KnockbackType GetKnockbackType()
	{
		KnockbackType result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoPunch.GetKnockbackType()).MethodHandle;
			}
			result = this.m_abilityMod.m_knockbackTypeMod.GetModifiedValue(this.m_knockbackType);
		}
		else
		{
			result = this.m_knockbackType;
		}
		return result;
	}

	public int GetDamageAmount()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoPunch.GetDamageAmount()).MethodHandle;
			}
			result = this.m_abilityMod.m_damageAmountMod.GetModifiedValue(this.m_damageAmount);
		}
		else
		{
			result = this.m_damageAmount;
		}
		return result;
	}

	public StandardEffectInfo GetTargetHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedTargetHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoPunch.GetTargetHitEffect()).MethodHandle;
			}
			result = this.m_cachedTargetHitEffect;
		}
		else
		{
			result = this.m_targetHitEffect;
		}
		return result;
	}

	public float GetNearDistThreshold()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoPunch.GetNearDistThreshold()).MethodHandle;
			}
			result = this.m_abilityMod.m_nearDistThresholdMod.GetModifiedValue(this.m_nearDistThreshold);
		}
		else
		{
			result = this.m_nearDistThreshold;
		}
		return result;
	}

	public int GetNearEnemyExtraDamage()
	{
		return (!this.m_abilityMod) ? this.m_nearEnemyExtraDamage : this.m_abilityMod.m_nearEnemyExtraDamageMod.GetModifiedValue(this.m_nearEnemyExtraDamage);
	}

	public StandardEffectInfo GetNearEnemyExtraEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedNearEnemyExtraEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoPunch.GetNearEnemyExtraEffect()).MethodHandle;
			}
			result = this.m_cachedNearEnemyExtraEffect;
		}
		else
		{
			result = this.m_nearEnemyExtraEffect;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, this.GetDamageAmount())
		};
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		float num = this.GetNearDistThreshold() * Board.Get().squareSize;
		if (num > 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoPunch.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			ActorData actorData = base.ActorData;
			Vector3 vector = targetActor.GetTravelBoardSquareWorldPosition() - actorData.GetTravelBoardSquareWorldPosition();
			vector.y = 0f;
			if (vector.magnitude < num)
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
				List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
				if (tooltipSubjectTypes != null)
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
					if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
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
						if (actorData != null)
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
							dictionary = new Dictionary<AbilityTooltipSymbol, int>();
							int num2 = this.GetDamageAmount();
							if (this.GetNearEnemyExtraDamage() > 0)
							{
								num2 += this.GetNearEnemyExtraDamage();
							}
							dictionary[AbilityTooltipSymbol.Damage] = num2;
						}
					}
				}
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "Damage", "damage in the cone", this.GetDamageAmount(), false);
		base.AddTokenInt(tokens, "Knockback_Distance", "range of knockback for hit enemies", Mathf.RoundToInt(this.GetKnockbackDistance()), false);
		base.AddTokenInt(tokens, "Cone_Angle", "angle of the damage cone", (int)this.GetConeWidthAngle(), false);
		base.AddTokenInt(tokens, "Cone_Length", "range of the damage cone", Mathf.RoundToInt(this.GetConeLength()), false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ExoPunch))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoPunch.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_ExoPunch);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}
}
