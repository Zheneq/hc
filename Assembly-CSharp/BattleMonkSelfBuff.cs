using System.Collections.Generic;
using UnityEngine;

public class BattleMonkSelfBuff : Ability
{
	public int m_damagePerHit = 10;

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
		Setup();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BattleMonkSelfBuff abilityMod_BattleMonkSelfBuff = modAsBase as AbilityMod_BattleMonkSelfBuff;
		int num;
		if ((bool)abilityMod_BattleMonkSelfBuff)
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
			num = abilityMod_BattleMonkSelfBuff.m_damageReturnMod.GetModifiedValue(m_damagePerHit);
		}
		else
		{
			num = m_damagePerHit;
		}
		int val = num;
		int num2;
		if ((bool)abilityMod_BattleMonkSelfBuff)
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
			num2 = abilityMod_BattleMonkSelfBuff.m_absorbMod.GetModifiedValue(m_standardActorEffectData.m_absorbAmount);
		}
		else
		{
			num2 = m_standardActorEffectData.m_absorbAmount;
		}
		int val2 = num2;
		tokens.Add(new TooltipTokenInt("DamageReturn", "damage amount on revenge hit", val));
		tokens.Add(new TooltipTokenInt("ShieldAmount", "shield amount", val2));
	}

	private void Setup()
	{
		SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_Shape(this, CanTargetNearbyAllies() ? GetAllyTargetShape() : AbilityAreaShape.SingleSquare, m_ignoreLos, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, CanTargetNearbyAllies(), AbilityUtil_Targeter.AffectsActor.Always);
		base.Targeter.ShowArcToShape = false;
	}

	private void SetCachedFields()
	{
		m_cachedReturnEffectOnEnemy = ((!m_abilityMod) ? m_returnEffectOnEnemy : m_abilityMod.m_returnEffectOnEnemyMod.GetModifiedValue(m_returnEffectOnEnemy));
	}

	public int GetDamagePerHit()
	{
		int result;
		if (m_abilityMod == null)
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
			result = m_damagePerHit;
		}
		else
		{
			result = m_abilityMod.m_damageReturnMod.GetModifiedValue(m_damagePerHit);
		}
		return result;
	}

	public StandardEffectInfo GetReturnEffectOnEnemy()
	{
		StandardEffectInfo result;
		if (m_cachedReturnEffectOnEnemy != null)
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
			result = m_cachedReturnEffectOnEnemy;
		}
		else
		{
			result = m_returnEffectOnEnemy;
		}
		return result;
	}

	public int GetAbsorbAmount()
	{
		int result;
		if (m_abilityMod == null)
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
			result = m_standardActorEffectData.m_absorbAmount;
		}
		else
		{
			result = m_abilityMod.m_absorbMod.GetModifiedValue(m_standardActorEffectData.m_absorbAmount);
		}
		return result;
	}

	public bool CanTargetNearbyAllies()
	{
		return !(m_abilityMod == null) && m_abilityMod.m_hitNearbyAlliesMod.GetModifiedValue(false);
	}

	public AbilityAreaShape GetAllyTargetShape()
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
			result = 0;
		}
		else
		{
			result = (int)m_abilityMod.m_allyTargetShapeMod.GetModifiedValue(AbilityAreaShape.SingleSquare);
		}
		return (AbilityAreaShape)result;
	}

	public StandardEffectInfo GetSelfEffect()
	{
		object result;
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
			result = null;
		}
		else
		{
			result = m_abilityMod.m_effectOnSelfNextTurn;
		}
		return (StandardEffectInfo)result;
	}

	public int GetDurationOfSelfEffect(int numHits)
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
			result = 0;
		}
		else
		{
			result = m_abilityMod.m_selfEffectDurationPerHit.GetModifiedValue(numHits);
		}
		return result;
	}

	public bool HasEffectForStartOfNextTurn()
	{
		int result;
		if (GetSelfEffect() != null)
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
			result = (GetSelfEffect().m_applyEffect ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, m_damagePerHit));
		m_standardActorEffectData.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		return numbers;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		int absorbAmount = GetAbsorbAmount();
		AbilityTooltipHelper.ReportAbsorb(ref numbers, AbilityTooltipSubject.Self, absorbAmount);
		if (CanTargetNearbyAllies())
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
			if (m_abilityMod.m_effectOnAllyHit.m_applyEffect)
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
				m_abilityMod.m_effectOnAllyHit.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
			}
		}
		return numbers;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BattleMonkSelfBuff))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_abilityMod = (abilityMod as AbilityMod_BattleMonkSelfBuff);
					Setup();
					return;
				}
			}
		}
		Debug.LogError("Trying to apply wrong type of ability mod");
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
