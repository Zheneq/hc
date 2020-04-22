using System;
using System.Collections.Generic;
using UnityEngine;

public class SoldierGrenade : Ability
{
	[Serializable]
	public class ShapeToDamage : ShapeToDataBase
	{
		public int m_damage;

		public ShapeToDamage(AbilityAreaShape shape, int damage)
		{
			m_shape = shape;
			m_damage = damage;
		}
	}

	[Header("-- Targeting --")]
	public AbilityAreaShape m_shape = AbilityAreaShape.Three_x_Three;

	public bool m_penetrateLos;

	[Header("-- On Hit Stuff --")]
	public int m_damageAmount = 10;

	public StandardEffectInfo m_enemyHitEffect;

	[Space(10f)]
	public int m_allyHealAmount;

	public StandardEffectInfo m_allyHitEffect;

	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_SoldierGrenade m_abilityMod;

	private AbilityData m_abilityData;

	private SoldierStimPack m_stimAbility;

	private List<ShapeToDamage> m_cachedShapeToDamage = new List<ShapeToDamage>();

	private List<AbilityAreaShape> m_shapes = new List<AbilityAreaShape>();

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private StandardEffectInfo m_cachedAllyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
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
			m_abilityName = "Grenade";
		}
		Setup();
	}

	private void Setup()
	{
		if (m_abilityData == null)
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
			m_abilityData = GetComponent<AbilityData>();
		}
		if (m_stimAbility == null && m_abilityData != null)
		{
			m_stimAbility = (m_abilityData.GetAbilityOfType(typeof(SoldierStimPack)) as SoldierStimPack);
		}
		SetCachedFields();
		m_cachedShapeToDamage.Clear();
		m_cachedShapeToDamage.Add(new ShapeToDamage(GetShape(), GetDamageAmount()));
		if (m_abilityMod != null)
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
			if (m_abilityMod.m_useAdditionalShapeOverride)
			{
				for (int i = 0; i < m_abilityMod.m_additionalShapeToDamageOverride.Count; i++)
				{
					ShapeToDamage shapeToDamage = m_abilityMod.m_additionalShapeToDamageOverride[i];
					m_cachedShapeToDamage.Add(new ShapeToDamage(shapeToDamage.m_shape, shapeToDamage.m_damage));
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		m_cachedShapeToDamage.Sort();
		m_shapes.Clear();
		for (int j = 0; j < m_cachedShapeToDamage.Count; j++)
		{
			m_shapes.Add(m_cachedShapeToDamage[j].m_shape);
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			List<AbilityTooltipSubject> list = new List<AbilityTooltipSubject>();
			list.Add(AbilityTooltipSubject.Primary);
			List<AbilityTooltipSubject> subjects = list;
			base.Targeter = new AbilityUtil_Targeter_MultipleShapes(this, m_shapes, subjects, PenetrateLos(), IncludeEnemies(), IncludeAllies());
			return;
		}
	}

	public int GetDamageForShapeIndex(int shapeIndex)
	{
		if (m_cachedShapeToDamage != null)
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
			if (shapeIndex < m_cachedShapeToDamage.Count)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return m_cachedShapeToDamage[shapeIndex].m_damage;
					}
				}
			}
		}
		return GetDamageAmount();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEnemyHitEffect;
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
			cachedEnemyHitEffect = m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = m_enemyHitEffect;
		}
		m_cachedEnemyHitEffect = cachedEnemyHitEffect;
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
			cachedAllyHitEffect = m_abilityMod.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect);
		}
		else
		{
			cachedAllyHitEffect = m_allyHitEffect;
		}
		m_cachedAllyHitEffect = cachedAllyHitEffect;
	}

	public AbilityAreaShape GetShape()
	{
		AbilityAreaShape result;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_shapeMod.GetModifiedValue(m_shape);
		}
		else
		{
			result = m_shape;
		}
		return result;
	}

	public bool PenetrateLos()
	{
		return (!m_abilityMod) ? m_penetrateLos : m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLos);
	}

	public int GetDamageAmount()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount);
		}
		else
		{
			result = m_damageAmount;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedEnemyHitEffect != null)
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
			result = m_cachedEnemyHitEffect;
		}
		else
		{
			result = m_enemyHitEffect;
		}
		return result;
	}

	public int GetAllyHealAmount()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_allyHealAmountMod.GetModifiedValue(m_allyHealAmount);
		}
		else
		{
			result = m_allyHealAmount;
		}
		return result;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedAllyHitEffect != null)
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
			result = m_cachedAllyHitEffect;
		}
		else
		{
			result = m_allyHitEffect;
		}
		return result;
	}

	public bool IncludeEnemies()
	{
		int result;
		if (GetDamageAmount() <= 0)
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
			result = (GetEnemyHitEffect().m_applyEffect ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public bool IncludeAllies()
	{
		int result;
		if (GetAllyHealAmount() <= 0)
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
			result = (GetAllyHitEffect().m_applyEffect ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDamageAmount());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, GetAllyHealAmount());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetAllyHealAmount());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> result = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeters[currentTargeterIndex].GetTooltipSubjectTypes(targetActor);
		ActorData actorData = base.ActorData;
		if (tooltipSubjectTypes != null)
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
			if (actorData != null)
			{
				result = new Dictionary<AbilityTooltipSymbol, int>();
				List<AbilityUtil_Targeter_MultipleShapes.HitActorContext> hitActorContext = (base.Targeter as AbilityUtil_Targeter_MultipleShapes).GetHitActorContext();
				{
					foreach (AbilityUtil_Targeter_MultipleShapes.HitActorContext item in hitActorContext)
					{
						if (item.m_actor == targetActor && targetActor.GetTeam() != actorData.GetTeam())
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									break;
								default:
									result[AbilityTooltipSymbol.Damage] = GetDamageForShapeIndex(item.m_hitShapeIndex);
									return result;
								}
							}
						}
					}
					return result;
				}
			}
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SoldierGrenade abilityMod_SoldierGrenade = modAsBase as AbilityMod_SoldierGrenade;
		AddTokenInt(tokens, "DamageAmount", string.Empty, (!abilityMod_SoldierGrenade) ? m_damageAmount : abilityMod_SoldierGrenade.m_damageAmountMod.GetModifiedValue(m_damageAmount));
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_SoldierGrenade)
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
			effectInfo = abilityMod_SoldierGrenade.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect);
		}
		else
		{
			effectInfo = m_enemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyHitEffect", m_enemyHitEffect);
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_SoldierGrenade)
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
			val = abilityMod_SoldierGrenade.m_allyHealAmountMod.GetModifiedValue(m_allyHealAmount);
		}
		else
		{
			val = m_allyHealAmount;
		}
		AddTokenInt(tokens, "AllyHealAmount", empty, val);
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_SoldierGrenade)
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
			effectInfo2 = abilityMod_SoldierGrenade.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect);
		}
		else
		{
			effectInfo2 = m_allyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "AllyHitEffect", m_allyHitEffect);
	}

	public override float GetRangeInSquares(int targetIndex)
	{
		float num = base.GetRangeInSquares(targetIndex);
		if (m_abilityData != null)
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
			if (m_stimAbility != null)
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
				if (m_stimAbility.GetGrenadeExtraRange() > 0f)
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
					if (m_abilityData.HasQueuedAbilityOfType(typeof(SoldierStimPack)))
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
						num += m_stimAbility.GetGrenadeExtraRange();
					}
				}
			}
		}
		return num;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SoldierGrenade))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityMod = (abilityMod as AbilityMod_SoldierGrenade);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
