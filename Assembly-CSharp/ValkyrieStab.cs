using System;
using System.Collections.Generic;
using UnityEngine;

public class ValkyrieStab : Ability
{
	[Header("-- Targeting")]
	public float m_coneWidthMinAngle = 10f;

	public float m_coneWidthMaxAngle = 70f;

	public float m_coneBackwardOffset;

	public float m_coneMinLength = 2.5f;

	public float m_coneMaxLength = 5f;

	public AreaEffectUtils.StretchConeStyle m_coneStretchStyle;

	public bool m_penetrateLineOfSight;

	public int m_maxTargets = 5;

	[Header("-- On Hit Damage/Effect")]
	public int m_damageAmount = 0x14;

	public int m_lessDamagePerTarget = 3;

	public StandardEffectInfo m_targetHitEffect;

	[Header("-- Sequences")]
	public GameObject m_centerProjectileSequencePrefab;

	public GameObject m_sideProjectileSequencePrefab;

	private Valkyrie_SyncComponent m_syncComp;

	private AbilityMod_ValkyrieStab m_abilityMod;

	private StandardEffectInfo m_cachedTargetHitEffect;

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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieStab.Start()).MethodHandle;
			}
			this.m_abilityName = "Spear Poke";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.m_syncComp = base.GetComponent<Valkyrie_SyncComponent>();
		this.SetCachedFields();
		AbilityUtil_Targeter_ReverseStretchCone targeter = new AbilityUtil_Targeter_ReverseStretchCone(this, this.GetConeMinLength(), this.GetConeMaxLength(), this.GetConeWidthMinAngle(), this.GetConeWidthMaxAngle(), this.m_coneStretchStyle, this.GetConeBackwardOffset(), this.PenetrateLineOfSight());
		base.Targeter = targeter;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetConeMaxLength() + this.GetConeBackwardOffset();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedTargetHitEffect;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieStab.SetCachedFields()).MethodHandle;
			}
			cachedTargetHitEffect = this.m_abilityMod.m_targetHitEffectMod.GetModifiedValue(this.m_targetHitEffect);
		}
		else
		{
			cachedTargetHitEffect = this.m_targetHitEffect;
		}
		this.m_cachedTargetHitEffect = cachedTargetHitEffect;
	}

	public float GetConeWidthMinAngle()
	{
		return (!this.m_abilityMod) ? this.m_coneWidthMinAngle : this.m_abilityMod.m_coneWidthMinAngleMod.GetModifiedValue(this.m_coneWidthMinAngle);
	}

	public float GetConeWidthMaxAngle()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieStab.GetConeWidthMaxAngle()).MethodHandle;
			}
			result = this.m_abilityMod.m_coneWidthMaxAngleMod.GetModifiedValue(this.m_coneWidthMaxAngle);
		}
		else
		{
			result = this.m_coneWidthMaxAngle;
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
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieStab.GetConeBackwardOffset()).MethodHandle;
			}
			result = this.m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(this.m_coneBackwardOffset);
		}
		else
		{
			result = this.m_coneBackwardOffset;
		}
		return result;
	}

	public float GetConeMinLength()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieStab.GetConeMinLength()).MethodHandle;
			}
			result = this.m_abilityMod.m_coneMinLengthMod.GetModifiedValue(this.m_coneMinLength);
		}
		else
		{
			result = this.m_coneMinLength;
		}
		return result;
	}

	public float GetConeMaxLength()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieStab.GetConeMaxLength()).MethodHandle;
			}
			result = this.m_abilityMod.m_coneMaxLengthMod.GetModifiedValue(this.m_coneMaxLength);
		}
		else
		{
			result = this.m_coneMaxLength;
		}
		return result;
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
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieStab.GetMaxTargets()).MethodHandle;
			}
			result = this.m_abilityMod.m_maxTargetsMod.GetModifiedValue(this.m_maxTargets);
		}
		else
		{
			result = this.m_maxTargets;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieStab.GetDamageAmount()).MethodHandle;
			}
			result = this.m_abilityMod.m_damageAmountMod.GetModifiedValue(this.m_damageAmount);
		}
		else
		{
			result = this.m_damageAmount;
		}
		return result;
	}

	public int GetLessDamagePerTarget()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieStab.GetLessDamagePerTarget()).MethodHandle;
			}
			result = this.m_abilityMod.m_lessDamagePerTargetMod.GetModifiedValue(this.m_lessDamagePerTarget);
		}
		else
		{
			result = this.m_lessDamagePerTarget;
		}
		return result;
	}

	public int GetExtraDamageOnSpearTip()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieStab.GetExtraDamageOnSpearTip()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraDamageOnSpearTip.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetExtraDamageFirstTarget()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieStab.GetExtraDamageFirstTarget()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraDamageFirstTarget.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public StandardEffectInfo GetTargetHitEffect()
	{
		return (this.m_cachedTargetHitEffect == null) ? this.m_targetHitEffect : this.m_cachedTargetHitEffect;
	}

	public int GetExtraAbsorbNextShieldBlockPerHit()
	{
		return (!this.m_abilityMod) ? 0 : this.m_abilityMod.m_perHitExtraAbsorbNextShieldBlock.GetModifiedValue(0);
	}

	public int GetMaxExtraAbsorbNextShieldBlock()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieStab.GetMaxExtraAbsorbNextShieldBlock()).MethodHandle;
			}
			result = this.m_abilityMod.m_maxExtraAbsorbNextShieldBlock.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ValkyrieStab))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieStab.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_ValkyrieStab);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
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
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityUtil_Targeter.ActorTarget> actorsInRange = base.Targeter.GetActorsInRange();
		List<ActorData> list = new List<ActorData>();
		int num = 0;
		using (List<AbilityUtil_Targeter.ActorTarget>.Enumerator enumerator = actorsInRange.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AbilityUtil_Targeter.ActorTarget actorTarget = enumerator.Current;
				list.Add(actorTarget.m_actor);
				if (actorTarget.m_actor == targetActor)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieStab.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
					}
					if (actorTarget.m_subjectTypes.Contains(AbilityTooltipSubject.Far))
					{
						num = this.GetExtraDamageOnSpearTip();
					}
				}
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
		int num2 = this.GetDamageAmount();
		bool flag = true;
		num2 += this.GetExtraDamageFirstTarget();
		foreach (ActorData x in list)
		{
			if (x == targetActor)
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
				dictionary[AbilityTooltipSymbol.Damage] = num2 + num;
				break;
			}
			if (this.m_syncComp == null)
			{
				goto IL_128;
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!this.m_syncComp.m_skipDamageReductionForNextStab)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					goto IL_128;
				}
			}
			IL_139:
			if (flag)
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
				flag = false;
				num2 -= this.GetExtraDamageFirstTarget();
				continue;
			}
			continue;
			IL_128:
			num2 = Mathf.Max(0, num2 - this.GetLessDamagePerTarget());
			goto IL_139;
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "Damage", "damage in the cone", this.GetDamageAmount(), false);
		base.AddTokenInt(tokens, "LessDamagePerTarget", string.Empty, this.m_lessDamagePerTarget, false);
		base.AddTokenInt(tokens, "Cone_MinAngle", "smallest angle of the damage cone", (int)this.GetConeWidthMinAngle(), false);
		base.AddTokenInt(tokens, "Cone_MaxAngle", "largest angle of the damage cone", (int)this.GetConeWidthMaxAngle(), false);
		base.AddTokenInt(tokens, "Cone_MinLength", "shortest range of the damage cone", Mathf.RoundToInt(this.GetConeMinLength()), false);
		base.AddTokenInt(tokens, "Cone_MaxLength", "longest range of the damage cone", Mathf.RoundToInt(this.GetConeMaxLength()), false);
		base.AddTokenInt(tokens, "MaxTargets", string.Empty, this.m_maxTargets, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_targetHitEffect, "TargetHitEffect", this.m_targetHitEffect, true);
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		min = this.GetConeMinLength() * Board.\u000E().squareSize;
		max = this.GetConeMaxLength() * Board.\u000E().squareSize;
		return true;
	}
}
