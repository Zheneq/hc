using System.Collections.Generic;
using UnityEngine;

public class SparkAoeBuffDebuff : Ability
{
	public enum TargetingType
	{
		UseShape,
		UseRadius
	}

	[Header("-- Targeting")]
	public TargetingType m_TargetingType;

	public bool m_penetrateLos;

	[Header("-- Shape")]
	public AbilityAreaShape m_shape = AbilityAreaShape.Five_x_Five_NoCorners;

	[Header("-- Radius")]
	public float m_radius = 6f;

	[Header("-- Damage and Healing")]
	public int m_damageAmount;

	public int m_allyHealAmount = 10;

	[Header("-- Self Heal Per Hit")]
	public int m_baseSelfHeal;

	public int m_selfHealAmountPerHit;

	public bool m_selfHealCountEnemyHit = true;

	public bool m_selfHealCountAllyHit = true;

	[Header("-- Normal Hit Effects")]
	public StandardEffectInfo m_selfHitEffect;

	public StandardEffectInfo m_allyHitEffect;

	public StandardEffectInfo m_enemyHitEffect;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	public GameObject m_sequenceOnEnemies;

	public GameObject m_sequenceOnAllies;

	private AbilityMod_SparkAoeBuffDebuff m_abilityMod;

	private StandardEffectInfo m_cachedSelfHitEffect;

	private StandardEffectInfo m_cachedAllyHitEffect;

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityName = "Spark Aoe Buff Debuff";
		}
		SetupTargeter();
	}

	public float GetTargetingRadius()
	{
		float result;
		if ((bool)m_abilityMod)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_radiusMod.GetModifiedValue(m_radius);
		}
		else
		{
			result = m_radius;
		}
		return result;
	}

	public AbilityAreaShape GetHitShape()
	{
		return m_shape;
	}

	public bool ShouldIgnoreLos()
	{
		bool result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_ignoreLosMod.GetModifiedValue(m_penetrateLos);
		}
		else
		{
			result = m_penetrateLos;
		}
		return result;
	}

	public int GetAllyHeal(AbilityMod_SparkAoeBuffDebuff mod)
	{
		return (!mod) ? m_allyHealAmount : mod.m_allyHealMod.GetModifiedValue(m_allyHealAmount);
	}

	public int GetBaseSelfHeal(AbilityMod_SparkAoeBuffDebuff mod)
	{
		int result;
		if ((bool)mod)
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
			result = mod.m_baseSelfHealMod.GetModifiedValue(m_baseSelfHeal);
		}
		else
		{
			result = m_baseSelfHeal;
		}
		return result;
	}

	public int GetSelfHealPerHit(AbilityMod_SparkAoeBuffDebuff mod)
	{
		return (!mod) ? m_selfHealAmountPerHit : mod.m_selfHealPerHitMod.GetModifiedValue(m_selfHealAmountPerHit);
	}

	public bool SelfHealCountAllyHit()
	{
		return (!m_abilityMod) ? m_selfHealCountAllyHit : m_abilityMod.m_selfHealHitCountAlly.GetModifiedValue(m_selfHealCountAllyHit);
	}

	public bool SelfHealCountEnemyHit()
	{
		bool result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_selfHealHitCountEnemy.GetModifiedValue(m_selfHealCountEnemyHit);
		}
		else
		{
			result = m_selfHealCountEnemyHit;
		}
		return result;
	}

	public int GetShieldOnSelfPerAllyHit()
	{
		int result;
		if ((bool)m_abilityMod)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_shieldOnSelfPerAllyHitMod.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetShieldOnSelfDuration()
	{
		int result;
		if ((bool)m_abilityMod)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_shieldOnSelfDuration;
		}
		else
		{
			result = 1;
		}
		return result;
	}

	public bool IncludeCaster()
	{
		int result;
		if (!GetSelfHitEffect().m_applyEffect)
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
			if (GetSelfHealPerHit(m_abilityMod) <= 0)
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
				if (GetBaseSelfHeal(m_abilityMod) <= 0)
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
					result = ((GetShieldOnSelfPerAllyHit() > 0) ? 1 : 0);
					goto IL_0064;
				}
			}
		}
		result = 1;
		goto IL_0064;
		IL_0064:
		return (byte)result != 0;
	}

	public bool IncludeAllies()
	{
		int result;
		if (!GetAllyHitEffect().m_applyEffect)
		{
			while (true)
			{
				switch (3)
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
			result = ((GetAllyHeal(m_abilityMod) > 0) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public bool IncludeEnemies()
	{
		int result;
		if (!GetEnemyHitEffect().m_applyEffect)
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
			result = ((m_damageAmount > 0) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public StandardEffectInfo GetSelfHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedSelfHitEffect != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_cachedSelfHitEffect;
		}
		else
		{
			result = m_selfHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		return (m_cachedAllyHitEffect == null) ? m_allyHitEffect : m_cachedAllyHitEffect;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedEnemyHitEffect != null)
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
			result = m_cachedEnemyHitEffect;
		}
		else
		{
			result = m_enemyHitEffect;
		}
		return result;
	}

	private void SetCachedFields()
	{
		m_cachedSelfHitEffect = ((!m_abilityMod) ? m_selfHitEffect : m_abilityMod.m_selfHitEffectMod.GetModifiedValue(m_selfHitEffect));
		StandardEffectInfo cachedAllyHitEffect;
		if ((bool)m_abilityMod)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			cachedAllyHitEffect = m_abilityMod.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect);
		}
		else
		{
			cachedAllyHitEffect = m_allyHitEffect;
		}
		m_cachedAllyHitEffect = cachedAllyHitEffect;
		StandardEffectInfo cachedEnemyHitEffect;
		if ((bool)m_abilityMod)
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
			cachedEnemyHitEffect = m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = m_enemyHitEffect;
		}
		m_cachedEnemyHitEffect = cachedEnemyHitEffect;
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		if (m_TargetingType == TargetingType.UseShape)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					AbilityUtil_Targeter.AffectsActor affectsCaster = AbilityUtil_Targeter.AffectsActor.Possible;
					if (!IncludeCaster())
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
						affectsCaster = AbilityUtil_Targeter.AffectsActor.Never;
					}
					base.Targeter = new AbilityUtil_Targeter_Shape(this, GetHitShape(), ShouldIgnoreLos(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, IncludeEnemies(), IncludeAllies(), affectsCaster);
					return;
				}
				}
			}
		}
		base.Targeter = new AbilityUtil_Targeter_AoE_Smooth(this, GetTargetingRadius(), ShouldIgnoreLos(), IncludeEnemies(), IncludeAllies());
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SparkAoeBuffDebuff abilityMod_SparkAoeBuffDebuff = modAsBase as AbilityMod_SparkAoeBuffDebuff;
		AddTokenInt(tokens, "Heal_OnAlly", "heal on ally", GetAllyHeal(abilityMod_SparkAoeBuffDebuff));
		AddTokenInt(tokens, "Heal_OnSelfBase", "heal on self, base amount", GetBaseSelfHeal(abilityMod_SparkAoeBuffDebuff));
		AddTokenInt(tokens, "Heal_OnSelfPerHit", "heal on self, per hit", GetSelfHealPerHit(abilityMod_SparkAoeBuffDebuff));
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_SparkAoeBuffDebuff)
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
			effectInfo = abilityMod_SparkAoeBuffDebuff.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect);
		}
		else
		{
			effectInfo = m_allyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectOnAlly", m_allyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_SparkAoeBuffDebuff) ? m_selfHitEffect : abilityMod_SparkAoeBuffDebuff.m_selfHitEffectMod.GetModifiedValue(m_selfHitEffect), "EffectOnSelf", m_selfHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_SparkAoeBuffDebuff) ? m_enemyHitEffect : abilityMod_SparkAoeBuffDebuff.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect), "EffectOnEnemy", m_enemyHitEffect);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		GetSelfHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		GetAllyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		GetEnemyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetBaseSelfHeal(m_abilityMod) + GetSelfHealPerHit(m_abilityMod));
		AbilityTooltipHelper.ReportAbsorb(ref numbers, AbilityTooltipSubject.Self, GetShieldOnSelfPerAllyHit());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, GetAllyHeal(m_abilityMod));
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, m_damageAmount);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		if (GetSelfHealPerHit(m_abilityMod) <= 0)
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
			if (GetBaseSelfHeal(m_abilityMod) <= 0 && GetShieldOnSelfPerAllyHit() <= 0)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return null;
					}
				}
			}
		}
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
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
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
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
				List<ActorData> visibleActorsInRangeByTooltipSubject = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Primary);
				int num = 0;
				int num2 = 0;
				for (int i = 0; i < visibleActorsInRangeByTooltipSubject.Count; i++)
				{
					if (visibleActorsInRangeByTooltipSubject[i].GetTeam() != targetActor.GetTeam())
					{
						num++;
					}
					else if (visibleActorsInRangeByTooltipSubject[i] != targetActor)
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
						num2++;
					}
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				int num4 = dictionary[AbilityTooltipSymbol.Healing] = CalcSelfHealAmountFromHits(num2, num);
				if (GetShieldOnSelfPerAllyHit() > 0)
				{
					int num5 = 0;
					StandardEffectInfo selfHitEffect = GetSelfHitEffect();
					if (selfHitEffect.m_applyEffect && selfHitEffect.m_effectData.m_absorbAmount > 0)
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
						num5 = selfHitEffect.m_effectData.m_absorbAmount;
					}
					dictionary[AbilityTooltipSymbol.Absorb] = num5 + num2 * GetShieldOnSelfPerAllyHit();
				}
			}
		}
		return dictionary;
	}

	private int CalcSelfHealAmountFromHits(int allyHits, int enemyHits)
	{
		int result = 0;
		if (GetSelfHealPerHit(m_abilityMod) <= 0)
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
			if (GetBaseSelfHeal(m_abilityMod) <= 0)
			{
				goto IL_0093;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		int num = 0;
		if (SelfHealCountAllyHit())
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
			num += allyHits;
		}
		if (SelfHealCountEnemyHit())
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
			num += enemyHits;
		}
		result = GetBaseSelfHeal(m_abilityMod) + num * GetSelfHealPerHit(m_abilityMod);
		goto IL_0093;
		IL_0093:
		return result;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SparkAoeBuffDebuff))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityMod = (abilityMod as AbilityMod_SparkAoeBuffDebuff);
			SetupTargeter();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
