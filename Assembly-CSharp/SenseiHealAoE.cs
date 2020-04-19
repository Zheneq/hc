using System;
using System.Collections.Generic;
using UnityEngine;

public class SenseiHealAoE : Ability
{
	[Separator("Targeting Info", true)]
	public float m_circleRadius = 3f;

	public bool m_penetrateLoS;

	public bool m_penetrateEnemyBarriers = true;

	[Space(10f)]
	public bool m_includeSelf;

	[Separator("Self Hit", true)]
	public int m_selfHeal = 0x14;

	[Space(10f)]
	public float m_selfLowHealthThresh;

	public int m_extraSelfHealForLowHealth;

	[Separator("Ally Hit", true)]
	public int m_allyHeal = 0x14;

	public int m_extraAllyHealIfSingleHit;

	[Space(10f)]
	public int m_extraHealForAdjacent;

	public float m_healChangeStartDist;

	public float m_healChangePerDist;

	[Header("-- Extra Ally Heal for low health")]
	public float m_allyLowHealthThresh;

	public int m_extraAllyHealForLowHealth;

	public StandardEffectInfo m_allyHitEffect;

	[Space(10f)]
	public int m_allyEnergyGain;

	[Header("-- Cooldown Reduction for damaging hits")]
	public int m_cdrForAnyDamage;

	public int m_cdrForDamagePerUniqueAbility = 1;

	[Separator("For trigger on Subsequent Turns", true)]
	public int m_turnsAfterInitialCast;

	public int m_allyHealOnSubsequentTurns;

	public int m_selfHealOnSubsequentTurns;

	public StandardEffectInfo m_allyEffectOnSubsequentTurns;

	[Header("-- Energy gain on subsequent turns")]
	public bool m_ignoreDefaultEnergyOnSubseqTurns = true;

	public int m_energyPerAllyHitOnSubseqTurns;

	public int m_energyOnSelfHitOnSubseqTurns;

	[Header("-- Sequences --")]
	public GameObject m_hitSequencePrefab;

	[Header("    Only used if has heal on subsequent turns")]
	public GameObject m_persistentSeqOnSubsequentTurnsPrefab;

	private AbilityMod_SenseiHealAoE m_abilityMod;

	private AbilityData m_abilityData;

	private SenseiBide m_bideAbility;

	private AbilityData.ActionType m_bideActionType = AbilityData.ActionType.INVALID_ACTION;

	private StandardEffectInfo m_cachedAllyHitEffect;

	private StandardEffectInfo m_cachedAllyEffectOnSubsequentTurns;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHealAoE.Start()).MethodHandle;
			}
			this.m_abilityName = "Sensei Heal";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		this.m_abilityData = base.GetComponent<AbilityData>();
		if (this.m_abilityData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHealAoE.Setup()).MethodHandle;
			}
			this.m_bideAbility = (this.m_abilityData.GetAbilityOfType(typeof(SenseiBide)) as SenseiBide);
			this.m_bideActionType = this.m_abilityData.GetActionTypeOfAbility(this.m_bideAbility);
		}
		AbilityUtil_Targeter_AoE_Smooth abilityUtil_Targeter_AoE_Smooth = new AbilityUtil_Targeter_AoE_Smooth(this, this.GetCircleRadius(), this.PenetrateLoS(), false, true, -1);
		abilityUtil_Targeter_AoE_Smooth.SetAffectedGroups(false, true, this.m_includeSelf);
		abilityUtil_Targeter_AoE_Smooth.m_adjustPosInConfirmedTargeting = true;
		abilityUtil_Targeter_AoE_Smooth.m_customCenterPosDelegate = new AbilityUtil_Targeter_AoE_Smooth.CustomCenterPosDelegate(this.GetCenterPosForTargeter);
		abilityUtil_Targeter_AoE_Smooth.m_affectCasterDelegate = new AbilityUtil_Targeter_AoE_Smooth.IsAffectingCasterDelegate(this.CanIncludeSelfForTargeter);
		abilityUtil_Targeter_AoE_Smooth.m_penetrateEnemyBarriers = this.m_penetrateEnemyBarriers;
		base.Targeter = abilityUtil_Targeter_AoE_Smooth;
		base.Targeter.ShowArcToShape = false;
	}

	private bool CanIncludeSelfForTargeter(ActorData caster, List<ActorData> actorsSoFar)
	{
		return this.m_includeSelf;
	}

	private Vector3 GetCenterPosForTargeter(ActorData caster, AbilityTarget currentTarget)
	{
		Vector3 result = caster.\u0016();
		if (caster.\u000E() != null && this.GetRunPriority() > AbilityPriority.Evasion)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHealAoE.GetCenterPosForTargeter(ActorData, AbilityTarget)).MethodHandle;
			}
			BoardSquare evadeDestinationForTargeter = caster.\u000E().GetEvadeDestinationForTargeter();
			if (evadeDestinationForTargeter != null)
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
				result = evadeDestinationForTargeter.ToVector3();
			}
		}
		return result;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedAllyHitEffect;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHealAoE.SetCachedFields()).MethodHandle;
			}
			cachedAllyHitEffect = this.m_abilityMod.m_allyHitEffectMod.GetModifiedValue(this.m_allyHitEffect);
		}
		else
		{
			cachedAllyHitEffect = this.m_allyHitEffect;
		}
		this.m_cachedAllyHitEffect = cachedAllyHitEffect;
		StandardEffectInfo cachedAllyEffectOnSubsequentTurns;
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
			cachedAllyEffectOnSubsequentTurns = this.m_abilityMod.m_allyEffectOnSubsequentTurnsMod.GetModifiedValue(this.m_allyEffectOnSubsequentTurns);
		}
		else
		{
			cachedAllyEffectOnSubsequentTurns = this.m_allyEffectOnSubsequentTurns;
		}
		this.m_cachedAllyEffectOnSubsequentTurns = cachedAllyEffectOnSubsequentTurns;
	}

	public float GetCircleRadius()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHealAoE.GetCircleRadius()).MethodHandle;
			}
			result = this.m_abilityMod.m_circleRadiusMod.GetModifiedValue(this.m_circleRadius);
		}
		else
		{
			result = this.m_circleRadius;
		}
		return result;
	}

	public bool PenetrateLoS()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHealAoE.PenetrateLoS()).MethodHandle;
			}
			result = this.m_abilityMod.m_penetrateLoSMod.GetModifiedValue(this.m_penetrateLoS);
		}
		else
		{
			result = this.m_penetrateLoS;
		}
		return result;
	}

	public bool IncludeSelf()
	{
		return (!this.m_abilityMod) ? this.m_includeSelf : this.m_abilityMod.m_includeSelfMod.GetModifiedValue(this.m_includeSelf);
	}

	public int GetSelfHeal()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHealAoE.GetSelfHeal()).MethodHandle;
			}
			result = this.m_abilityMod.m_selfHealMod.GetModifiedValue(this.m_selfHeal);
		}
		else
		{
			result = this.m_selfHeal;
		}
		return result;
	}

	public float GetSelfLowHealthThresh()
	{
		float result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHealAoE.GetSelfLowHealthThresh()).MethodHandle;
			}
			result = this.m_abilityMod.m_selfLowHealthThreshMod.GetModifiedValue(this.m_selfLowHealthThresh);
		}
		else
		{
			result = this.m_selfLowHealthThresh;
		}
		return result;
	}

	public int GetExtraSelfHealForLowHealth()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHealAoE.GetExtraSelfHealForLowHealth()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraSelfHealForLowHealthMod.GetModifiedValue(this.m_extraSelfHealForLowHealth);
		}
		else
		{
			result = this.m_extraSelfHealForLowHealth;
		}
		return result;
	}

	public int GetAllyHeal()
	{
		return (!this.m_abilityMod) ? this.m_allyHeal : this.m_abilityMod.m_allyHealMod.GetModifiedValue(this.m_allyHeal);
	}

	public int GetExtraAllyHealIfSingleHit()
	{
		return (!this.m_abilityMod) ? this.m_extraAllyHealIfSingleHit : this.m_abilityMod.m_extraAllyHealIfSingleHitMod.GetModifiedValue(this.m_extraAllyHealIfSingleHit);
	}

	public int GetExtraHealForAdjacent()
	{
		return (!this.m_abilityMod) ? this.m_extraHealForAdjacent : this.m_abilityMod.m_extraHealForAdjacentMod.GetModifiedValue(this.m_extraHealForAdjacent);
	}

	public float GetHealChangeStartDist()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHealAoE.GetHealChangeStartDist()).MethodHandle;
			}
			result = this.m_abilityMod.m_healChangeStartDistMod.GetModifiedValue(this.m_healChangeStartDist);
		}
		else
		{
			result = this.m_healChangeStartDist;
		}
		return result;
	}

	public float GetHealChangePerDist()
	{
		return (!this.m_abilityMod) ? this.m_healChangePerDist : this.m_abilityMod.m_healChangePerDistMod.GetModifiedValue(this.m_healChangePerDist);
	}

	public float GetAllyLowHealthThresh()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHealAoE.GetAllyLowHealthThresh()).MethodHandle;
			}
			result = this.m_abilityMod.m_allyLowHealthThreshMod.GetModifiedValue(this.m_allyLowHealthThresh);
		}
		else
		{
			result = this.m_allyLowHealthThresh;
		}
		return result;
	}

	public int GetExtraAllyHealForLowHealth()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHealAoE.GetExtraAllyHealForLowHealth()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraAllyHealForLowHealthMod.GetModifiedValue(this.m_extraAllyHealForLowHealth);
		}
		else
		{
			result = this.m_extraAllyHealForLowHealth;
		}
		return result;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedAllyHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHealAoE.GetAllyHitEffect()).MethodHandle;
			}
			result = this.m_cachedAllyHitEffect;
		}
		else
		{
			result = this.m_allyHitEffect;
		}
		return result;
	}

	public int GetAllyEnergyGain()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHealAoE.GetAllyEnergyGain()).MethodHandle;
			}
			result = this.m_abilityMod.m_allyEnergyGainMod.GetModifiedValue(this.m_allyEnergyGain);
		}
		else
		{
			result = this.m_allyEnergyGain;
		}
		return result;
	}

	public int GetCdrForAnyDamage()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHealAoE.GetCdrForAnyDamage()).MethodHandle;
			}
			result = this.m_abilityMod.m_cdrForAnyDamageMod.GetModifiedValue(this.m_cdrForAnyDamage);
		}
		else
		{
			result = this.m_cdrForAnyDamage;
		}
		return result;
	}

	public int GetCdrForDamagePerUniqueAbility()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHealAoE.GetCdrForDamagePerUniqueAbility()).MethodHandle;
			}
			result = this.m_abilityMod.m_cdrForDamagePerUniqueAbilityMod.GetModifiedValue(this.m_cdrForDamagePerUniqueAbility);
		}
		else
		{
			result = this.m_cdrForDamagePerUniqueAbility;
		}
		return result;
	}

	public int GetTurnsAfterInitialCast()
	{
		return (!this.m_abilityMod) ? this.m_turnsAfterInitialCast : this.m_abilityMod.m_turnsAfterInitialCastMod.GetModifiedValue(this.m_turnsAfterInitialCast);
	}

	public int GetAllyHealOnSubsequentTurns()
	{
		return (!this.m_abilityMod) ? this.m_allyHealOnSubsequentTurns : this.m_abilityMod.m_allyHealOnSubsequentTurnsMod.GetModifiedValue(this.m_allyHealOnSubsequentTurns);
	}

	public int GetSelfHealOnSubsequentTurns()
	{
		return (!this.m_abilityMod) ? this.m_selfHealOnSubsequentTurns : this.m_abilityMod.m_selfHealOnSubsequentTurnsMod.GetModifiedValue(this.m_selfHealOnSubsequentTurns);
	}

	public StandardEffectInfo GetAllyEffectOnSubsequentTurns()
	{
		StandardEffectInfo result;
		if (this.m_cachedAllyEffectOnSubsequentTurns != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHealAoE.GetAllyEffectOnSubsequentTurns()).MethodHandle;
			}
			result = this.m_cachedAllyEffectOnSubsequentTurns;
		}
		else
		{
			result = this.m_allyEffectOnSubsequentTurns;
		}
		return result;
	}

	public bool IgnoreDefaultEnergyOnSubseqTurns()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHealAoE.IgnoreDefaultEnergyOnSubseqTurns()).MethodHandle;
			}
			result = this.m_abilityMod.m_ignoreDefaultEnergyOnSubseqTurnsMod.GetModifiedValue(this.m_ignoreDefaultEnergyOnSubseqTurns);
		}
		else
		{
			result = this.m_ignoreDefaultEnergyOnSubseqTurns;
		}
		return result;
	}

	public int GetEnergyPerAllyHitOnSubseqTurns()
	{
		return (!this.m_abilityMod) ? this.m_energyPerAllyHitOnSubseqTurns : this.m_abilityMod.m_energyPerAllyHitOnSubseqTurnsMod.GetModifiedValue(this.m_energyPerAllyHitOnSubseqTurns);
	}

	public int GetEnergyOnSelfHitOnSubseqTurns()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHealAoE.GetEnergyOnSelfHitOnSubseqTurns()).MethodHandle;
			}
			result = this.m_abilityMod.m_energyOnSelfHitOnSubseqTurnsMod.GetModifiedValue(this.m_energyOnSelfHitOnSubseqTurns);
		}
		else
		{
			result = this.m_energyOnSelfHitOnSubseqTurns;
		}
		return result;
	}

	public int CalcExtraHealFromDist(ActorData targetActor, Vector3 centerPos)
	{
		if (this.GetExtraHealForAdjacent() > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHealAoE.CalcExtraHealFromDist(ActorData, Vector3)).MethodHandle;
			}
			Vector3 vector = targetActor.\u0016() - centerPos;
			vector.y = 0f;
			float num = vector.magnitude;
			if (this.GetHealChangeStartDist() > 0f)
			{
				num -= this.GetHealChangeStartDist();
				num = Mathf.Max(0f, num / Board.\u000E().squareSize);
			}
			int num2 = Mathf.RoundToInt(this.GetHealChangePerDist() * num);
			return Mathf.Max(0, this.GetExtraHealForAdjacent() + num2);
		}
		return 0;
	}

	public int CalcExtraHealFromBide()
	{
		int num = 0;
		if (this.m_bideAbility != null && this.m_bideAbility.GetExtraHealOnHealAoeIfQueued() > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHealAoE.CalcExtraHealFromBide()).MethodHandle;
			}
			if (this.m_abilityData.HasQueuedAction(this.m_bideActionType))
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
				num += this.m_bideAbility.GetExtraHealOnHealAoeIfQueued();
			}
		}
		return num;
	}

	public int CalcTotalAllyHeal(ActorData targetActor, Vector3 centerPos, int numAllies)
	{
		int num = this.GetAllyHeal();
		if (this.GetExtraAllyHealIfSingleHit() > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHealAoE.CalcTotalAllyHeal(ActorData, Vector3, int)).MethodHandle;
			}
			if (numAllies == 1)
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
				num += this.GetExtraAllyHealIfSingleHit();
			}
		}
		num += this.CalcExtraHealFromDist(targetActor, centerPos);
		num += this.CalcExtraHealFromBide();
		if (this.GetExtraAllyHealForLowHealth() > 0)
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
			if (this.GetAllyLowHealthThresh() > 0f)
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
				if (targetActor.\u0012() < this.GetAllyLowHealthThresh())
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
					num += this.GetExtraAllyHealForLowHealth();
				}
			}
		}
		return num;
	}

	public int CalcTotalSelfHeal(ActorData caster)
	{
		int num = this.GetSelfHeal();
		num += this.CalcExtraHealFromBide();
		if (this.GetExtraSelfHealForLowHealth() > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHealAoE.CalcTotalSelfHeal(ActorData)).MethodHandle;
			}
			if (this.GetSelfLowHealthThresh() > 0f)
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
				if (caster.\u0012() < this.GetSelfLowHealthThresh())
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
					num += this.GetExtraSelfHealForLowHealth();
				}
			}
		}
		return num;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "SelfHeal", string.Empty, this.m_selfHeal, false);
		base.AddTokenFloatAsPct(tokens, "SelfLowHealthThresh_Pct", string.Empty, this.m_selfLowHealthThresh, false);
		base.AddTokenInt(tokens, "ExtraSelfHealForLowHealth", string.Empty, this.m_extraSelfHealForLowHealth, false);
		base.AddTokenInt(tokens, "HealAmount", string.Empty, this.m_allyHeal, false);
		base.AddTokenInt(tokens, "ExtraAllyHealIfSingleHit", string.Empty, this.m_extraAllyHealIfSingleHit, false);
		base.AddTokenInt(tokens, "ExtraHealForAdjacent", string.Empty, this.m_extraHealForAdjacent, false);
		base.AddTokenFloatAsPct(tokens, "AllyLowHealthThresh_Pct", string.Empty, this.m_allyLowHealthThresh, false);
		base.AddTokenInt(tokens, "ExtraAllyHealForLowHealth", string.Empty, this.m_extraAllyHealForLowHealth, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_allyHitEffect, "AllyHitEffect", this.m_allyHitEffect, true);
		base.AddTokenInt(tokens, "AllyEnergyGain", string.Empty, this.m_allyEnergyGain, false);
		base.AddTokenInt(tokens, "CdrForAnyDamage", string.Empty, this.m_cdrForAnyDamage, false);
		base.AddTokenInt(tokens, "CdrForDamagePerUniqueAbility", string.Empty, this.m_cdrForDamagePerUniqueAbility, false);
		base.AddTokenInt(tokens, "TurnsAfterInitialCast", string.Empty, this.m_turnsAfterInitialCast, false);
		base.AddTokenInt(tokens, "AllyHealOnSubsequentTurns", string.Empty, this.m_allyHealOnSubsequentTurns, false);
		base.AddTokenInt(tokens, "SelfHealOnSubsequentTurns", string.Empty, this.m_selfHealOnSubsequentTurns, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_allyEffectOnSubsequentTurns, "AllyEffectOnSubsequentTurns", this.m_allyEffectOnSubsequentTurns, true);
		base.AddTokenInt(tokens, "EnergyPerAllyHitOnSubseqTurns", string.Empty, this.m_energyPerAllyHitOnSubseqTurns, false);
		base.AddTokenInt(tokens, "EnergyOnSelfHitOnSubseqTurns", string.Empty, this.m_energyOnSelfHitOnSubseqTurns, false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Ally, this.GetAllyHeal());
		this.m_allyHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		if (this.IncludeSelf())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHealAoE.CalculateAbilityTooltipNumbers()).MethodHandle;
			}
			AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, this.GetSelfHeal());
			this.m_allyHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		}
		AbilityTooltipHelper.ReportEnergy(ref result, AbilityTooltipSubject.Ally, this.GetAllyEnergyGain());
		return result;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		bool flag = base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Ally) > 0;
		bool flag2 = targetActor == base.ActorData;
		if (!flag)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHealAoE.GetCustomTargeterNumbers(ActorData, int, TargetingNumberUpdateScratch)).MethodHandle;
			}
			if (!flag2)
			{
				return true;
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		int healing = 0;
		if (flag)
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
			int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
			if (base.Targeter is AbilityUtil_Targeter_AoE_Smooth)
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
				AbilityUtil_Targeter_AoE_Smooth abilityUtil_Targeter_AoE_Smooth = base.Targeter as AbilityUtil_Targeter_AoE_Smooth;
				healing = this.CalcTotalAllyHeal(targetActor, abilityUtil_Targeter_AoE_Smooth.m_lastUpdatedCenterPos, visibleActorsCountByTooltipSubject);
			}
		}
		else if (flag2)
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
			healing = this.CalcTotalSelfHeal(base.ActorData);
		}
		results.m_healing = healing;
		return true;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SenseiHealAoE))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiHealAoE.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_SenseiHealAoE);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
