using System;
using System.Collections.Generic;
using UnityEngine;

public class SorceressDamageField : Ability
{
	public AbilityAreaShape m_shape = AbilityAreaShape.Three_x_Three;

	public bool m_penetrateLineOfSight;

	public int m_duration;

	public int m_damage;

	public int m_healing;

	public StandardEffectInfo m_effectOnEnemies;

	public StandardEffectInfo m_effectOnAllies;

	[Header("-- Sequences")]
	public GameObject m_hittingEnemyPrefab;

	public GameObject m_hittingAllyPrefab;

	public GameObject m_persistentGroundPrefab;

	public GameObject m_onHitPulsePrefab;

	private AbilityMod_SorceressDamageField m_abilityMod;

	private StandardEffectInfo m_cachedEffectOnEnemies;

	private StandardEffectInfo m_cachedEffectOnAllies;

	private void Start()
	{
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		bool flag;
		if (this.GetDamage() <= 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SorceressDamageField.SetupTargeter()).MethodHandle;
			}
			flag = this.GetEnemyHitEffect().m_applyEffect;
		}
		else
		{
			flag = true;
		}
		bool affectsEnemies = flag;
		bool flag2;
		if (this.GetHealing() <= 0)
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
			flag2 = this.GetAllyHitEffect().m_applyEffect;
		}
		else
		{
			flag2 = true;
		}
		bool flag3 = flag2;
		AbilityUtil_Targeter.AffectsActor affectsCaster = (!flag3) ? AbilityUtil_Targeter.AffectsActor.Never : AbilityUtil_Targeter.AffectsActor.Possible;
		AbilityUtil_Targeter_Shape.DamageOriginType damageOriginType = AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape;
		base.Targeter = new AbilityUtil_Targeter_Shape(this, this.GetEffectShape(), this.m_penetrateLineOfSight, damageOriginType, affectsEnemies, flag3, affectsCaster, AbilityUtil_Targeter.AffectsActor.Possible);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, this.m_damage));
		this.m_effectOnEnemies.ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Enemy);
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Self, this.m_healing));
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally, this.m_healing));
		this.m_effectOnAllies.ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Ally);
		return list;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.GetDamage());
		this.m_effectOnEnemies.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Enemy);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, this.GetHealing());
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Ally, this.GetHealing());
		this.m_effectOnAllies.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SorceressDamageField abilityMod_SorceressDamageField = modAsBase as AbilityMod_SorceressDamageField;
		string name = "Duration";
		string empty = string.Empty;
		int val;
		if (abilityMod_SorceressDamageField)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SorceressDamageField.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_SorceressDamageField.m_durationMod.GetModifiedValue(this.m_duration);
		}
		else
		{
			val = this.m_duration;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		base.AddTokenInt(tokens, "Damage", string.Empty, (!abilityMod_SorceressDamageField) ? this.m_damage : abilityMod_SorceressDamageField.m_damageMod.GetModifiedValue(this.m_damage), false);
		string name2 = "Healing";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_SorceressDamageField)
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
			val2 = abilityMod_SorceressDamageField.m_healingMod.GetModifiedValue(this.m_healing);
		}
		else
		{
			val2 = this.m_healing;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		StandardEffectInfo effectInfo;
		if (abilityMod_SorceressDamageField)
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
			effectInfo = abilityMod_SorceressDamageField.m_onEnemyEffectOverride.GetModifiedValue(this.m_effectOnEnemies);
		}
		else
		{
			effectInfo = this.m_effectOnEnemies;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectOnEnemies", this.m_effectOnEnemies, true);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_SorceressDamageField) ? this.m_effectOnAllies : abilityMod_SorceressDamageField.m_onAllyEffectOverride.GetModifiedValue(this.m_effectOnAllies), "EffectOnAllies", this.m_effectOnAllies, true);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SorceressDamageField))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SorceressDamageField.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_SorceressDamageField);
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

	private AbilityAreaShape GetEffectShape()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_shapeOverride.GetModifiedValue(this.m_shape) : this.m_shape;
	}

	private GameObject GetPersistentSequencePrefab()
	{
		if (!(this.m_abilityMod == null))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SorceressDamageField.GetPersistentSequencePrefab()).MethodHandle;
			}
			if (!(this.m_abilityMod.m_persistentSequencePrefabOverride == null))
			{
				return this.m_abilityMod.m_persistentSequencePrefabOverride;
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
		}
		return this.m_persistentGroundPrefab;
	}

	private int GetDuration()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_durationMod.GetModifiedValue(this.m_duration) : this.m_duration;
	}

	private int GetDamage()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SorceressDamageField.GetDamage()).MethodHandle;
			}
			result = this.m_damage;
		}
		else
		{
			result = this.m_abilityMod.m_damageMod.GetModifiedValue(this.m_damage);
		}
		return result;
	}

	private int GetHealing()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SorceressDamageField.GetHealing()).MethodHandle;
			}
			result = this.m_healing;
		}
		else
		{
			result = this.m_abilityMod.m_healingMod.GetModifiedValue(this.m_healing);
		}
		return result;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEffectOnEnemies;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SorceressDamageField.SetCachedFields()).MethodHandle;
			}
			cachedEffectOnEnemies = this.m_abilityMod.m_onEnemyEffectOverride.GetModifiedValue(this.m_effectOnEnemies);
		}
		else
		{
			cachedEffectOnEnemies = this.m_effectOnEnemies;
		}
		this.m_cachedEffectOnEnemies = cachedEffectOnEnemies;
		StandardEffectInfo cachedEffectOnAllies;
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
			cachedEffectOnAllies = this.m_abilityMod.m_onAllyEffectOverride.GetModifiedValue(this.m_effectOnAllies);
		}
		else
		{
			cachedEffectOnAllies = this.m_effectOnAllies;
		}
		this.m_cachedEffectOnAllies = cachedEffectOnAllies;
	}

	private StandardEffectInfo GetAllyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnAllies != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SorceressDamageField.GetAllyHitEffect()).MethodHandle;
			}
			result = this.m_cachedEffectOnAllies;
		}
		else
		{
			result = this.m_effectOnAllies;
		}
		return result;
	}

	private StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnEnemies != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SorceressDamageField.GetEnemyHitEffect()).MethodHandle;
			}
			result = this.m_cachedEffectOnEnemies;
		}
		else
		{
			result = this.m_effectOnEnemies;
		}
		return result;
	}
}
