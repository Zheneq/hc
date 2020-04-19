using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleMonkSelfBuff : Ability
{
	public int m_damagePerHit = 0xA;

	public StandardActorEffectData m_standardActorEffectData;

	[Header("-- Enemy Hit Effect --")]
	public StandardEffectInfo m_returnEffectOnEnemy;

	[Header("-- Whether to ignore LoS when checking for allies, used for mod")]
	public bool m_ignoreLos;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	public GameObject m_reactionProjectilePrefab;

	private AbilityMod_BattleMonkSelfBuff m_abilityMod;

	private StandardEffectInfo m_cachedReturnEffectOnEnemy;

	private void Start()
	{
		this.Setup();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BattleMonkSelfBuff abilityMod_BattleMonkSelfBuff = modAsBase as AbilityMod_BattleMonkSelfBuff;
		int num;
		if (abilityMod_BattleMonkSelfBuff)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkSelfBuff.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			num = abilityMod_BattleMonkSelfBuff.m_damageReturnMod.GetModifiedValue(this.m_damagePerHit);
		}
		else
		{
			num = this.m_damagePerHit;
		}
		int val = num;
		int num2;
		if (abilityMod_BattleMonkSelfBuff)
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
			num2 = abilityMod_BattleMonkSelfBuff.m_absorbMod.GetModifiedValue(this.m_standardActorEffectData.m_absorbAmount);
		}
		else
		{
			num2 = this.m_standardActorEffectData.m_absorbAmount;
		}
		int val2 = num2;
		tokens.Add(new TooltipTokenInt("DamageReturn", "damage amount on revenge hit", val));
		tokens.Add(new TooltipTokenInt("ShieldAmount", "shield amount", val2));
	}

	private void Setup()
	{
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_Shape(this, (!this.CanTargetNearbyAllies()) ? AbilityAreaShape.SingleSquare : this.GetAllyTargetShape(), this.m_ignoreLos, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, this.CanTargetNearbyAllies(), AbilityUtil_Targeter.AffectsActor.Always, AbilityUtil_Targeter.AffectsActor.Possible);
		base.Targeter.ShowArcToShape = false;
	}

	private void SetCachedFields()
	{
		this.m_cachedReturnEffectOnEnemy = ((!this.m_abilityMod) ? this.m_returnEffectOnEnemy : this.m_abilityMod.m_returnEffectOnEnemyMod.GetModifiedValue(this.m_returnEffectOnEnemy));
	}

	public int GetDamagePerHit()
	{
		int result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkSelfBuff.GetDamagePerHit()).MethodHandle;
			}
			result = this.m_damagePerHit;
		}
		else
		{
			result = this.m_abilityMod.m_damageReturnMod.GetModifiedValue(this.m_damagePerHit);
		}
		return result;
	}

	public StandardEffectInfo GetReturnEffectOnEnemy()
	{
		StandardEffectInfo result;
		if (this.m_cachedReturnEffectOnEnemy != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkSelfBuff.GetReturnEffectOnEnemy()).MethodHandle;
			}
			result = this.m_cachedReturnEffectOnEnemy;
		}
		else
		{
			result = this.m_returnEffectOnEnemy;
		}
		return result;
	}

	public int GetAbsorbAmount()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkSelfBuff.GetAbsorbAmount()).MethodHandle;
			}
			result = this.m_standardActorEffectData.m_absorbAmount;
		}
		else
		{
			result = this.m_abilityMod.m_absorbMod.GetModifiedValue(this.m_standardActorEffectData.m_absorbAmount);
		}
		return result;
	}

	public bool CanTargetNearbyAllies()
	{
		return !(this.m_abilityMod == null) && this.m_abilityMod.m_hitNearbyAlliesMod.GetModifiedValue(false);
	}

	public AbilityAreaShape GetAllyTargetShape()
	{
		AbilityAreaShape result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkSelfBuff.GetAllyTargetShape()).MethodHandle;
			}
			result = AbilityAreaShape.SingleSquare;
		}
		else
		{
			result = this.m_abilityMod.m_allyTargetShapeMod.GetModifiedValue(AbilityAreaShape.SingleSquare);
		}
		return result;
	}

	public StandardEffectInfo GetSelfEffect()
	{
		StandardEffectInfo result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkSelfBuff.GetSelfEffect()).MethodHandle;
			}
			result = null;
		}
		else
		{
			result = this.m_abilityMod.m_effectOnSelfNextTurn;
		}
		return result;
	}

	public int GetDurationOfSelfEffect(int numHits)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkSelfBuff.GetDurationOfSelfEffect(int)).MethodHandle;
			}
			result = 0;
		}
		else
		{
			result = this.m_abilityMod.m_selfEffectDurationPerHit.GetModifiedValue(numHits);
		}
		return result;
	}

	public bool HasEffectForStartOfNextTurn()
	{
		bool result;
		if (this.GetSelfEffect() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkSelfBuff.HasEffectForStartOfNextTurn()).MethodHandle;
			}
			result = this.GetSelfEffect().m_applyEffect;
		}
		else
		{
			result = false;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, this.m_damagePerHit));
		this.m_standardActorEffectData.ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Self);
		return list;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		int absorbAmount = this.GetAbsorbAmount();
		AbilityTooltipHelper.ReportAbsorb(ref result, AbilityTooltipSubject.Self, absorbAmount);
		if (this.CanTargetNearbyAllies())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkSelfBuff.CalculateNameplateTargetingNumbers()).MethodHandle;
			}
			if (this.m_abilityMod.m_effectOnAllyHit.m_applyEffect)
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
				this.m_abilityMod.m_effectOnAllyHit.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
			}
		}
		return result;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BattleMonkSelfBuff))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkSelfBuff.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_BattleMonkSelfBuff);
			this.Setup();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
