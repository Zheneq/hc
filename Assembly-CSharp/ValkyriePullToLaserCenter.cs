using System;
using System.Collections.Generic;
using UnityEngine;

public class ValkyriePullToLaserCenter : Ability
{
	[Header("-- Targeting")]
	public float m_laserWidth = 5f;

	public float m_laserRangeInSquares = 6.5f;

	public int m_maxTargets = 5;

	public bool m_lengthIgnoreLos = true;

	[Header("-- Damage & effects")]
	public int m_damage = 0x28;

	public StandardEffectInfo m_effectToEnemies;

	public int m_extraDamageForCenterHits;

	public float m_centerHitWidth = 0.1f;

	[Header("-- Knockback on Cast")]
	public float m_maxKnockbackDist = 3f;

	public KnockbackType m_knockbackType = KnockbackType.PerpendicularPullToAimDir;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_ValkyriePullToLaserCenter m_abilityMod;

	private StandardEffectInfo m_cachedEffectToEnemies;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyriePullToLaserCenter.Start()).MethodHandle;
			}
			this.m_abilityName = "Valkyrie Pull Beam";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_KnockbackLaser(this, this.GetLaserWidth(), this.GetLaserRangeInSquares(), false, this.m_maxTargets, this.GetMaxKnockbackDist(), this.GetMaxKnockbackDist(), this.m_knockbackType, false)
		{
			LengthIgnoreWorldGeo = this.m_lengthIgnoreLos
		};
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetLaserRangeInSquares();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEffectToEnemies;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyriePullToLaserCenter.SetCachedFields()).MethodHandle;
			}
			cachedEffectToEnemies = this.m_abilityMod.m_effectToEnemiesMod.GetModifiedValue(this.m_effectToEnemies);
		}
		else
		{
			cachedEffectToEnemies = this.m_effectToEnemies;
		}
		this.m_cachedEffectToEnemies = cachedEffectToEnemies;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "MaxTargets", string.Empty, this.m_maxTargets, false);
		base.AddTokenInt(tokens, "Damage", string.Empty, this.m_damage, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectToEnemies, "EffectToEnemies", this.m_effectToEnemies, true);
		base.AddTokenInt(tokens, "ExtraDamageForCenterHits", string.Empty, this.m_extraDamageForCenterHits, false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.GetDamage());
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		int num = this.GetDamage();
		int extraDamageIfKnockedInPlace = this.GetExtraDamageIfKnockedInPlace();
		if (extraDamageIfKnockedInPlace != 0 && !targetActor.\u000E().IsMovementDebuffImmune(true))
		{
			List<AbilityUtil_Targeter.ActorTarget> actorsInRange = base.Targeter.GetActorsInRange();
			using (List<AbilityUtil_Targeter.ActorTarget>.Enumerator enumerator = actorsInRange.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AbilityUtil_Targeter.ActorTarget actorTarget = enumerator.Current;
					if (actorTarget.m_actor == targetActor)
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
							RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyriePullToLaserCenter.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
						}
						if (actorTarget.m_subjectTypes.Contains(AbilityTooltipSubject.HighHP))
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
							num += extraDamageIfKnockedInPlace;
						}
						goto IL_BE;
					}
				}
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		IL_BE:
		int extraDamageForCenterHits = this.GetExtraDamageForCenterHits();
		if (extraDamageForCenterHits > 0 && base.Targeter is AbilityUtil_Targeter_KnockbackLaser)
		{
			AbilityUtil_Targeter_KnockbackLaser abilityUtil_Targeter_KnockbackLaser = base.Targeter as AbilityUtil_Targeter_KnockbackLaser;
			bool flag = AreaEffectUtils.IsSquareInBoxByActorRadius(targetActor.\u0012(), base.ActorData.\u0015(), abilityUtil_Targeter_KnockbackLaser.GetLastLaserEndPos(), this.GetCenterHitWidth());
			if (flag)
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
				num += extraDamageForCenterHits;
			}
		}
		dictionary[AbilityTooltipSymbol.Damage] = num;
		return dictionary;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyriePullToLaserCenter.GetLaserWidth()).MethodHandle;
			}
			result = this.m_abilityMod.m_laserWidthMod.GetModifiedValue(this.m_laserWidth);
		}
		else
		{
			result = this.m_laserWidth;
		}
		return result;
	}

	public float GetLaserRangeInSquares()
	{
		return (!this.m_abilityMod) ? this.m_laserRangeInSquares : this.m_abilityMod.m_laserRangeInSquaresMod.GetModifiedValue(this.m_laserRangeInSquares);
	}

	public int GetMaxTargets()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyriePullToLaserCenter.GetMaxTargets()).MethodHandle;
			}
			result = this.m_abilityMod.m_maxTargetsMod.GetModifiedValue(this.m_maxTargets);
		}
		else
		{
			result = this.m_maxTargets;
		}
		return result;
	}

	public bool LengthIgnoreLos()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyriePullToLaserCenter.LengthIgnoreLos()).MethodHandle;
			}
			result = this.m_abilityMod.m_lengthIgnoreLosMod.GetModifiedValue(this.m_lengthIgnoreLos);
		}
		else
		{
			result = this.m_lengthIgnoreLos;
		}
		return result;
	}

	public int GetDamage()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyriePullToLaserCenter.GetDamage()).MethodHandle;
			}
			result = this.m_abilityMod.m_damageMod.GetModifiedValue(this.m_damage);
		}
		else
		{
			result = this.m_damage;
		}
		return result;
	}

	public int GetExtraDamageIfKnockedInPlace()
	{
		return (!this.m_abilityMod) ? 0 : this.m_abilityMod.m_extraDamageIfKnockedInPlaceMod.GetModifiedValue(0);
	}

	public StandardEffectInfo GetEffectToEnemies()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectToEnemies != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyriePullToLaserCenter.GetEffectToEnemies()).MethodHandle;
			}
			result = this.m_cachedEffectToEnemies;
		}
		else
		{
			result = this.m_effectToEnemies;
		}
		return result;
	}

	public int GetExtraDamageForCenterHits()
	{
		int result;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyriePullToLaserCenter.GetExtraDamageForCenterHits()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraDamageForCenterHitsMod.GetModifiedValue(this.m_extraDamageForCenterHits);
		}
		else
		{
			result = this.m_extraDamageForCenterHits;
		}
		return result;
	}

	public float GetCenterHitWidth()
	{
		float result;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyriePullToLaserCenter.GetCenterHitWidth()).MethodHandle;
			}
			result = this.m_abilityMod.m_centerHitWidthMod.GetModifiedValue(this.m_centerHitWidth);
		}
		else
		{
			result = this.m_centerHitWidth;
		}
		return result;
	}

	public float GetMaxKnockbackDist()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyriePullToLaserCenter.GetMaxKnockbackDist()).MethodHandle;
			}
			result = this.m_abilityMod.m_maxKnockbackDistMod.GetModifiedValue(this.m_maxKnockbackDist);
		}
		else
		{
			result = this.m_maxKnockbackDist;
		}
		return result;
	}

	public KnockbackType GetKnockbackType()
	{
		return (!this.m_abilityMod) ? this.m_knockbackType : this.m_abilityMod.m_knockbackTypeMod.GetModifiedValue(this.m_knockbackType);
	}

	public bool ShouldSkipDamageReductionOnNextTurnStab()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyriePullToLaserCenter.ShouldSkipDamageReductionOnNextTurnStab()).MethodHandle;
			}
			result = this.m_abilityMod.m_nextTurnStabSkipsDamageReduction.GetModifiedValue(false);
		}
		else
		{
			result = false;
		}
		return result;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ValkyriePullToLaserCenter))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyriePullToLaserCenter.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_ValkyriePullToLaserCenter);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
