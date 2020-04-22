using System.Collections.Generic;
using UnityEngine;

public class RageBeastSelfHeal : Ability
{
	public bool m_healOverTime = true;

	public StandardActorEffectData m_standardActorEffectData;

	public int m_healingOnCastIfUnder = 4;

	public int m_healingOnTickIfUnder = 4;

	public int m_healingOnCastIfOver = 2;

	public int m_healingOnTickIfOver = 2;

	public int m_healthThreshold = 10;

	private AbilityMod_RageBeastSelfHeal m_abilityMod;

	private StandardActorEffectData m_cachedStandardActorEffectData;

	private void Start()
	{
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false, AbilityUtil_Targeter.AffectsActor.Always);
		base.Targeter.ShowArcToShape = false;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.HighHP, m_healingOnCastIfUnder));
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.LowHP, m_healingOnCastIfOver));
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null && tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
		{
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			dictionary[AbilityTooltipSymbol.Healing] = GetHealingForCurrentHealth(targetActor);
		}
		return dictionary;
	}

	public override List<int> _001D()
	{
		List<int> list = base._001D();
		list.Add(m_healthThreshold);
		return list;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_RageBeastSelfHeal abilityMod_RageBeastSelfHeal = modAsBase as AbilityMod_RageBeastSelfHeal;
		StandardActorEffectData standardActorEffectData;
		if ((bool)abilityMod_RageBeastSelfHeal)
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
			standardActorEffectData = abilityMod_RageBeastSelfHeal.m_standardActorEffectDataMod.GetModifiedValue(m_standardActorEffectData);
		}
		else
		{
			standardActorEffectData = m_standardActorEffectData;
		}
		StandardActorEffectData standardActorEffectData2 = standardActorEffectData;
		standardActorEffectData2.AddTooltipTokens(tokens, "StandardActorEffectData", abilityMod_RageBeastSelfHeal != null, m_standardActorEffectData);
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_RageBeastSelfHeal)
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
			val = abilityMod_RageBeastSelfHeal.m_lowHealthHealOnCastMod.GetModifiedValue(m_healingOnCastIfUnder);
		}
		else
		{
			val = m_healingOnCastIfUnder;
		}
		AddTokenInt(tokens, "HealingOnCastIfUnder", empty, val);
		AddTokenInt(tokens, "HealingOnTickIfUnder", string.Empty, (!abilityMod_RageBeastSelfHeal) ? m_healingOnTickIfUnder : abilityMod_RageBeastSelfHeal.m_lowHealthHealOnTickMod.GetModifiedValue(m_healingOnTickIfUnder));
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_RageBeastSelfHeal)
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
			val2 = abilityMod_RageBeastSelfHeal.m_highHealthOnCastMod.GetModifiedValue(m_healingOnCastIfOver);
		}
		else
		{
			val2 = m_healingOnCastIfOver;
		}
		AddTokenInt(tokens, "HealingOnCastIfOver", empty2, val2);
		AddTokenInt(tokens, "HealingOnTickIfOver", string.Empty, (!abilityMod_RageBeastSelfHeal) ? m_healingOnTickIfOver : abilityMod_RageBeastSelfHeal.m_highHealthOnTickMod.GetModifiedValue(m_healingOnTickIfOver));
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_RageBeastSelfHeal)
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
			val3 = abilityMod_RageBeastSelfHeal.m_healthThresholdMod.GetModifiedValue(m_healthThreshold);
		}
		else
		{
			val3 = m_healthThreshold;
		}
		AddTokenInt(tokens, "HealthThreshold", empty3, val3);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_RageBeastSelfHeal))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityMod = (abilityMod as AbilityMod_RageBeastSelfHeal);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	private void SetCachedFields()
	{
		StandardActorEffectData cachedStandardActorEffectData;
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
			cachedStandardActorEffectData = m_abilityMod.m_standardActorEffectDataMod.GetModifiedValue(m_standardActorEffectData);
		}
		else
		{
			cachedStandardActorEffectData = m_standardActorEffectData;
		}
		m_cachedStandardActorEffectData = cachedStandardActorEffectData;
	}

	public StandardActorEffectData GetStandardActorEffectData()
	{
		StandardActorEffectData result;
		if (m_cachedStandardActorEffectData != null)
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
			result = m_cachedStandardActorEffectData;
		}
		else
		{
			result = m_standardActorEffectData;
		}
		return result;
	}

	public bool ShouldHealOverTime()
	{
		bool result;
		if (m_abilityMod == null)
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
			result = m_healOverTime;
		}
		else
		{
			result = m_abilityMod.m_healOverTimeMod.GetModifiedValue(m_healOverTime);
		}
		return result;
	}

	public int ModdedHealthThreshold()
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
			result = m_healthThreshold;
		}
		else
		{
			result = m_abilityMod.m_healthThresholdMod.GetModifiedValue(m_healthThreshold);
		}
		return result;
	}

	public int ModdedHealOnCastIfUnder()
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
			result = m_healingOnCastIfUnder;
		}
		else
		{
			result = m_abilityMod.m_lowHealthHealOnCastMod.GetModifiedValue(m_healingOnCastIfUnder);
		}
		return result;
	}

	public int ModdedHealOnTickIfUnder()
	{
		int result;
		if (m_abilityMod == null)
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
			result = m_healingOnTickIfUnder;
		}
		else
		{
			result = m_abilityMod.m_lowHealthHealOnTickMod.GetModifiedValue(m_healingOnTickIfUnder);
		}
		return result;
	}

	public int ModdedHealOnCastIfOver()
	{
		int result;
		if (m_abilityMod == null)
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
			result = m_healingOnCastIfOver;
		}
		else
		{
			result = m_abilityMod.m_highHealthOnCastMod.GetModifiedValue(m_healingOnCastIfOver);
		}
		return result;
	}

	public int ModdedHealOnTickIfOver()
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
			result = m_healingOnTickIfOver;
		}
		else
		{
			result = m_abilityMod.m_highHealthOnTickMod.GetModifiedValue(m_healingOnTickIfOver);
		}
		return result;
	}

	public override bool DoesTargetActorMatchTooltipSubject(AbilityTooltipSubject subjectType, ActorData targetActor, Vector3 damageOrigin, ActorData targetingActor)
	{
		if (subjectType != AbilityTooltipSubject.HighHP)
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
			if (subjectType != AbilityTooltipSubject.LowHP)
			{
				return base.DoesTargetActorMatchTooltipSubject(subjectType, targetActor, damageOrigin, targetingActor);
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		bool flag = targetActor.HitPoints <= ModdedHealthThreshold();
		if (subjectType == AbilityTooltipSubject.LowHP)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return flag;
				}
			}
		}
		return !flag;
	}

	private int GetHealingForCurrentHealth(ActorData caster)
	{
		if (caster.HitPoints <= ModdedHealthThreshold())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					GetStandardActorEffectData().m_healingPerTurn = ModdedHealOnTickIfUnder();
					return ModdedHealOnCastIfUnder();
				}
			}
		}
		GetStandardActorEffectData().m_healingPerTurn = ModdedHealOnTickIfOver();
		return ModdedHealOnCastIfOver();
	}
}
