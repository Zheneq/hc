using System;
using System.Collections.Generic;
using UnityEngine;

public class ClericHammerThrow : Ability
{
	[Separator("Targeting", true)]
	public float m_maxDistToRingCenter = 5.5f;

	public float m_outerRadius = 2.5f;

	public float m_innerRadius = 1f;

	public bool m_ignoreLos;

	public bool m_clampRingToCursorPos = true;

	[Separator("On Hit", true)]
	public int m_outerHitDamage = 0xF;

	public StandardEffectInfo m_outerEnemyHitEffect;

	public int m_innerHitDamage = 0x14;

	public StandardEffectInfo m_innerEnemyHitEffect;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	private List<ClericHammerThrow.RadiusToHitData> m_cachedRadiusToHitData = new List<ClericHammerThrow.RadiusToHitData>();

	private AbilityMod_ClericHammerThrow m_abilityMod;

	private Cleric_SyncComponent m_syncComp;

	private StandardEffectInfo m_cachedOuterEnemyHitEffect;

	private StandardEffectInfo m_cachedInnerEnemyHitEffect;

	private StandardEffectInfo m_cachedOuterEnemyHitEffectWithNoInnerHits;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "ClericHammerThrow";
		}
		this.m_syncComp = base.GetComponent<Cleric_SyncComponent>();
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		this.m_cachedRadiusToHitData.Clear();
		this.m_cachedRadiusToHitData.Add(new ClericHammerThrow.RadiusToHitData(this.GetInnerRadius(), this.GetInnerHitDamage(), this.GetInnerEnemyHitEffect()));
		this.m_cachedRadiusToHitData.Add(new ClericHammerThrow.RadiusToHitData(this.GetOuterRadius(), this.GetOuterHitDamage(), this.GetOuterEnemyHitEffect()));
		this.m_cachedRadiusToHitData.Sort();
		AbilityUtil_Targeter_MartyrLaser targeter = new AbilityUtil_Targeter_MartyrLaser(this, 0f, this.GetMaxDistToRingCenter(), false, -1, true, false, false, true, false, this.GetOuterRadius(), this.GetInnerRadius(), false, true, false);
		base.Targeter = targeter;
		base.Targeter.SetShowArcToShape(true);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "OuterHitDamage", string.Empty, this.m_outerHitDamage, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_outerEnemyHitEffect, "OuterEnemyHitEffect", this.m_outerEnemyHitEffect, true);
		base.AddTokenInt(tokens, "InnerHitDamage", string.Empty, this.m_innerHitDamage, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_innerEnemyHitEffect, "InnerEnemyHitEffect", this.m_innerEnemyHitEffect, true);
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedOuterEnemyHitEffect;
		if (this.m_abilityMod)
		{
			cachedOuterEnemyHitEffect = this.m_abilityMod.m_outerEnemyHitEffectMod.GetModifiedValue(this.m_outerEnemyHitEffect);
		}
		else
		{
			cachedOuterEnemyHitEffect = this.m_outerEnemyHitEffect;
		}
		this.m_cachedOuterEnemyHitEffect = cachedOuterEnemyHitEffect;
		StandardEffectInfo cachedInnerEnemyHitEffect;
		if (this.m_abilityMod)
		{
			cachedInnerEnemyHitEffect = this.m_abilityMod.m_innerEnemyHitEffectMod.GetModifiedValue(this.m_innerEnemyHitEffect);
		}
		else
		{
			cachedInnerEnemyHitEffect = this.m_innerEnemyHitEffect;
		}
		this.m_cachedInnerEnemyHitEffect = cachedInnerEnemyHitEffect;
		StandardEffectInfo cachedOuterEnemyHitEffectWithNoInnerHits;
		if (this.m_abilityMod)
		{
			cachedOuterEnemyHitEffectWithNoInnerHits = this.m_abilityMod.m_outerEnemyHitEffectWithNoInnerHits.GetModifiedValue(null);
		}
		else
		{
			cachedOuterEnemyHitEffectWithNoInnerHits = null;
		}
		this.m_cachedOuterEnemyHitEffectWithNoInnerHits = cachedOuterEnemyHitEffectWithNoInnerHits;
	}

	public float GetMaxDistToRingCenter()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_maxDistToRingCenterMod.GetModifiedValue(this.m_maxDistToRingCenter);
		}
		else
		{
			result = this.m_maxDistToRingCenter;
		}
		return result;
	}

	public float GetOuterRadius()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_outerRadiusMod.GetModifiedValue(this.m_outerRadius);
		}
		else
		{
			result = this.m_outerRadius;
		}
		return result;
	}

	public float GetInnerRadius()
	{
		return (!this.m_abilityMod) ? this.m_innerRadius : this.m_abilityMod.m_innerRadiusMod.GetModifiedValue(this.m_innerRadius);
	}

	public bool IgnoreLos()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_ignoreLosMod.GetModifiedValue(this.m_ignoreLos);
		}
		else
		{
			result = this.m_ignoreLos;
		}
		return result;
	}

	public bool ClampRingToCursorPos()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_clampRingToCursorPosMod.GetModifiedValue(this.m_clampRingToCursorPos);
		}
		else
		{
			result = this.m_clampRingToCursorPos;
		}
		return result;
	}

	public int GetOuterHitDamage()
	{
		return (!this.m_abilityMod) ? this.m_outerHitDamage : this.m_abilityMod.m_outerHitDamageMod.GetModifiedValue(this.m_outerHitDamage);
	}

	public StandardEffectInfo GetOuterEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedOuterEnemyHitEffect != null)
		{
			result = this.m_cachedOuterEnemyHitEffect;
		}
		else
		{
			result = this.m_outerEnemyHitEffect;
		}
		return result;
	}

	public int GetInnerHitDamage()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_innerHitDamageMod.GetModifiedValue(this.m_innerHitDamage);
		}
		else
		{
			result = this.m_innerHitDamage;
		}
		return result;
	}

	public StandardEffectInfo GetInnerEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedInnerEnemyHitEffect != null)
		{
			result = this.m_cachedInnerEnemyHitEffect;
		}
		else
		{
			result = this.m_innerEnemyHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetOuterEnemyHitEffectWithNoInnerHits()
	{
		StandardEffectInfo result;
		if (this.m_cachedOuterEnemyHitEffectWithNoInnerHits != null)
		{
			result = this.m_cachedOuterEnemyHitEffectWithNoInnerHits;
		}
		else
		{
			result = null;
		}
		return result;
	}

	public int GetExtraInnerDamagePerOuterHit()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraInnerDamagePerOuterHit.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetExtraTPGainInAreaBuff()
	{
		return (!this.m_abilityMod) ? 0 : this.m_abilityMod.m_extraTechPointGainInAreaBuff.GetModifiedValue(0);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Secondary, this.m_outerHitDamage);
		return result;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Secondary) > 0 && this.m_cachedRadiusToHitData.Count > 0)
		{
			AbilityUtil_Targeter_MartyrLaser abilityUtil_Targeter_MartyrLaser = base.Targeter as AbilityUtil_Targeter_MartyrLaser;
			ClericHammerThrow.RadiusToHitData bestMatchingData = AbilityCommon_LayeredRings.GetBestMatchingData<ClericHammerThrow.RadiusToHitData>(this.m_cachedRadiusToHitData, targetActor.GetCurrentBoardSquare(), abilityUtil_Targeter_MartyrLaser.m_lastLaserEndPos, base.ActorData, true);
			if (bestMatchingData != null)
			{
				int num = 0;
				if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Tertiary) == 0)
				{
					num = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Tertiary) * this.GetExtraInnerDamagePerOuterHit();
				}
				results.m_damage = bestMatchingData.m_damage + num;
				return true;
			}
		}
		return false;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (caster.GetAbilityData().HasQueuedAbilityOfType(typeof(ClericAreaBuff)))
		{
			return this.GetExtraTPGainInAreaBuff() * base.Targeter.GetNumActorsInRange();
		}
		return base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ClericHammerThrow))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_ClericHammerThrow);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}

	[Serializable]
	public class RadiusToHitData : RadiusToDataBase
	{
		public int m_damage;

		public StandardEffectInfo m_hitEffectInfo;

		public RadiusToHitData(float radiusInSquares, int damage, StandardEffectInfo hitEffect)
		{
			this.m_radius = radiusInSquares;
			this.m_damage = damage;
			this.m_hitEffectInfo = hitEffect;
		}
	}
}
