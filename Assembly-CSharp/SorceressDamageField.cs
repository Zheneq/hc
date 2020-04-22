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
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		int num;
		if (GetDamage() <= 0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num = (GetEnemyHitEffect().m_applyEffect ? 1 : 0);
		}
		else
		{
			num = 1;
		}
		bool affectsEnemies = (byte)num != 0;
		int num2;
		if (GetHealing() <= 0)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			num2 = (GetAllyHitEffect().m_applyEffect ? 1 : 0);
		}
		else
		{
			num2 = 1;
		}
		bool flag = (byte)num2 != 0;
		AbilityUtil_Targeter.AffectsActor affectsCaster = flag ? AbilityUtil_Targeter.AffectsActor.Possible : AbilityUtil_Targeter.AffectsActor.Never;
		AbilityUtil_Targeter_Shape.DamageOriginType damageOriginType = AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape;
		base.Targeter = new AbilityUtil_Targeter_Shape(this, GetEffectShape(), m_penetrateLineOfSight, damageOriginType, affectsEnemies, flag, affectsCaster);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, m_damage));
		m_effectOnEnemies.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Self, m_healing));
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally, m_healing));
		m_effectOnAllies.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		return numbers;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDamage());
		m_effectOnEnemies.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetHealing());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, GetHealing());
		m_effectOnAllies.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		return numbers;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SorceressDamageField abilityMod_SorceressDamageField = modAsBase as AbilityMod_SorceressDamageField;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_SorceressDamageField)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			val = abilityMod_SorceressDamageField.m_durationMod.GetModifiedValue(m_duration);
		}
		else
		{
			val = m_duration;
		}
		AddTokenInt(tokens, "Duration", empty, val);
		AddTokenInt(tokens, "Damage", string.Empty, (!abilityMod_SorceressDamageField) ? m_damage : abilityMod_SorceressDamageField.m_damageMod.GetModifiedValue(m_damage));
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_SorceressDamageField)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			val2 = abilityMod_SorceressDamageField.m_healingMod.GetModifiedValue(m_healing);
		}
		else
		{
			val2 = m_healing;
		}
		AddTokenInt(tokens, "Healing", empty2, val2);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_SorceressDamageField)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			effectInfo = abilityMod_SorceressDamageField.m_onEnemyEffectOverride.GetModifiedValue(m_effectOnEnemies);
		}
		else
		{
			effectInfo = m_effectOnEnemies;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectOnEnemies", m_effectOnEnemies);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_SorceressDamageField) ? m_effectOnAllies : abilityMod_SorceressDamageField.m_onAllyEffectOverride.GetModifiedValue(m_effectOnAllies), "EffectOnAllies", m_effectOnAllies);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SorceressDamageField))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_abilityMod = (abilityMod as AbilityMod_SorceressDamageField);
					SetupTargeter();
					return;
				}
			}
		}
		Debug.LogError("Trying to apply wrong type of ability mod");
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private AbilityAreaShape GetEffectShape()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_shapeOverride.GetModifiedValue(m_shape) : m_shape;
	}

	private GameObject GetPersistentSequencePrefab()
	{
		if (!(m_abilityMod == null))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(m_abilityMod.m_persistentSequencePrefabOverride == null))
			{
				return m_abilityMod.m_persistentSequencePrefabOverride;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return m_persistentGroundPrefab;
	}

	private int GetDuration()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_durationMod.GetModifiedValue(m_duration) : m_duration;
	}

	private int GetDamage()
	{
		int result;
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_damage;
		}
		else
		{
			result = m_abilityMod.m_damageMod.GetModifiedValue(m_damage);
		}
		return result;
	}

	private int GetHealing()
	{
		int result;
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_healing;
		}
		else
		{
			result = m_abilityMod.m_healingMod.GetModifiedValue(m_healing);
		}
		return result;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEffectOnEnemies;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			cachedEffectOnEnemies = m_abilityMod.m_onEnemyEffectOverride.GetModifiedValue(m_effectOnEnemies);
		}
		else
		{
			cachedEffectOnEnemies = m_effectOnEnemies;
		}
		m_cachedEffectOnEnemies = cachedEffectOnEnemies;
		StandardEffectInfo cachedEffectOnAllies;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			cachedEffectOnAllies = m_abilityMod.m_onAllyEffectOverride.GetModifiedValue(m_effectOnAllies);
		}
		else
		{
			cachedEffectOnAllies = m_effectOnAllies;
		}
		m_cachedEffectOnAllies = cachedEffectOnAllies;
	}

	private StandardEffectInfo GetAllyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOnAllies != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_cachedEffectOnAllies;
		}
		else
		{
			result = m_effectOnAllies;
		}
		return result;
	}

	private StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOnEnemies != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_cachedEffectOnEnemies;
		}
		else
		{
			result = m_effectOnEnemies;
		}
		return result;
	}
}
