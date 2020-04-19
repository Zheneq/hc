using System;
using System.Collections.Generic;
using UnityEngine;

public class RageBeastCharge : Ability
{
	[Space(10f)]
	public int m_damageAmount = 0x14;

	public int m_damageNearChargeEnd;

	public float m_damageRadius = 5f;

	public float m_radiusBehindStart;

	public float m_radiusBeyondEnd;

	public bool m_penetrateLineOfSight;

	public StandardEffectInfo m_enemyHitEffectNearChargeEnd;

	private AbilityMod_RageBeastCharge m_abilityMod;

	private StandardEffectInfo m_cachedEnemyHitEffectNearChargeEnd;

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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RageBeastCharge.Start()).MethodHandle;
			}
			this.m_abilityName = "Pain Train";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_ChargeAoE(this, this.m_radiusBehindStart, this.ModdedChargeEndRadius(), this.ModdedChargeLineRadius(), -1, false, this.m_penetrateLineOfSight);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, this.ModdedDamage())
		};
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RageBeastCharge.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Far) && this.ModdedDamageNearChargeEnd() > 0)
			{
				dictionary[AbilityTooltipSymbol.Damage] = this.ModdedDamageNearChargeEnd();
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_RageBeastCharge abilityMod_RageBeastCharge = modAsBase as AbilityMod_RageBeastCharge;
		string name = "DamageAmount";
		string empty = string.Empty;
		int val;
		if (abilityMod_RageBeastCharge)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RageBeastCharge.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_RageBeastCharge.m_damageMod.GetModifiedValue(this.m_damageAmount);
		}
		else
		{
			val = this.m_damageAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "DamageNearChargeEnd";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_RageBeastCharge)
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
			val2 = abilityMod_RageBeastCharge.m_damageNearChargeEndMod.GetModifiedValue(this.m_damageNearChargeEnd);
		}
		else
		{
			val2 = this.m_damageNearChargeEnd;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_enemyHitEffectNearChargeEnd, "EnemyHitEffectNearChargeEnd", this.m_enemyHitEffectNearChargeEnd, true);
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquare = Board.\u000E().\u000E(target.GridPos);
		if (boardSquare != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RageBeastCharge.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
			}
			if (boardSquare.\u0016() && boardSquare != caster.\u0012())
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
				return KnockbackUtils.BuildStraightLineChargePath(caster, boardSquare) != null;
			}
		}
		return false;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RageBeastCharge))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_RageBeastCharge);
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

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEnemyHitEffectNearChargeEnd;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RageBeastCharge.SetCachedFields()).MethodHandle;
			}
			cachedEnemyHitEffectNearChargeEnd = this.m_abilityMod.m_enemyHitEffectNearChargeEndMod.GetModifiedValue(this.m_enemyHitEffectNearChargeEnd);
		}
		else
		{
			cachedEnemyHitEffectNearChargeEnd = this.m_enemyHitEffectNearChargeEnd;
		}
		this.m_cachedEnemyHitEffectNearChargeEnd = cachedEnemyHitEffectNearChargeEnd;
	}

	public int ModdedDamage()
	{
		int result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RageBeastCharge.ModdedDamage()).MethodHandle;
			}
			result = this.m_damageAmount;
		}
		else
		{
			result = this.m_abilityMod.m_damageMod.GetModifiedValue(this.m_damageAmount);
		}
		return result;
	}

	public int ModdedDamageNearChargeEnd()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_damageNearChargeEndMod.GetModifiedValue(this.m_damageNearChargeEnd) : this.m_damageNearChargeEnd;
	}

	public float ModdedChargeLineRadius()
	{
		float result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RageBeastCharge.ModdedChargeLineRadius()).MethodHandle;
			}
			result = this.m_damageRadius;
		}
		else
		{
			result = this.m_abilityMod.m_chargeLineRadiusMod.GetModifiedValue(this.m_damageRadius);
		}
		return result;
	}

	public float ModdedChargeEndRadius()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_chargeEndRadius.GetModifiedValue(this.m_radiusBeyondEnd) : this.m_radiusBeyondEnd;
	}

	public StandardEffectInfo GetEnemyHitEffectNearChargeEnd()
	{
		return (this.m_cachedEnemyHitEffectNearChargeEnd == null) ? this.m_enemyHitEffectNearChargeEnd : this.m_cachedEnemyHitEffectNearChargeEnd;
	}
}
