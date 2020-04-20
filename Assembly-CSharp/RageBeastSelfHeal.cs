using System;
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

	public int m_healthThreshold = 0xA;

	private AbilityMod_RageBeastSelfHeal m_abilityMod;

	private StandardActorEffectData m_cachedStandardActorEffectData;

	private void Start()
	{
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false, AbilityUtil_Targeter.AffectsActor.Always, AbilityUtil_Targeter.AffectsActor.Possible);
		base.Targeter.ShowArcToShape = false;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.HighHP, this.m_healingOnCastIfUnder),
			new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.LowHP, this.m_healingOnCastIfOver)
		};
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null && tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
		{
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			dictionary[AbilityTooltipSymbol.Healing] = this.GetHealingForCurrentHealth(targetActor);
		}
		return dictionary;
	}

	public override List<int> symbol_001D()
	{
		List<int> list = base.symbol_001D();
		list.Add(this.m_healthThreshold);
		return list;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_RageBeastSelfHeal abilityMod_RageBeastSelfHeal = modAsBase as AbilityMod_RageBeastSelfHeal;
		StandardActorEffectData standardActorEffectData;
		if (abilityMod_RageBeastSelfHeal)
		{
			standardActorEffectData = abilityMod_RageBeastSelfHeal.m_standardActorEffectDataMod.GetModifiedValue(this.m_standardActorEffectData);
		}
		else
		{
			standardActorEffectData = this.m_standardActorEffectData;
		}
		StandardActorEffectData standardActorEffectData2 = standardActorEffectData;
		standardActorEffectData2.AddTooltipTokens(tokens, "StandardActorEffectData", abilityMod_RageBeastSelfHeal != null, this.m_standardActorEffectData);
		string name = "HealingOnCastIfUnder";
		string empty = string.Empty;
		int val;
		if (abilityMod_RageBeastSelfHeal)
		{
			val = abilityMod_RageBeastSelfHeal.m_lowHealthHealOnCastMod.GetModifiedValue(this.m_healingOnCastIfUnder);
		}
		else
		{
			val = this.m_healingOnCastIfUnder;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		base.AddTokenInt(tokens, "HealingOnTickIfUnder", string.Empty, (!abilityMod_RageBeastSelfHeal) ? this.m_healingOnTickIfUnder : abilityMod_RageBeastSelfHeal.m_lowHealthHealOnTickMod.GetModifiedValue(this.m_healingOnTickIfUnder), false);
		string name2 = "HealingOnCastIfOver";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_RageBeastSelfHeal)
		{
			val2 = abilityMod_RageBeastSelfHeal.m_highHealthOnCastMod.GetModifiedValue(this.m_healingOnCastIfOver);
		}
		else
		{
			val2 = this.m_healingOnCastIfOver;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		base.AddTokenInt(tokens, "HealingOnTickIfOver", string.Empty, (!abilityMod_RageBeastSelfHeal) ? this.m_healingOnTickIfOver : abilityMod_RageBeastSelfHeal.m_highHealthOnTickMod.GetModifiedValue(this.m_healingOnTickIfOver), false);
		string name3 = "HealthThreshold";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_RageBeastSelfHeal)
		{
			val3 = abilityMod_RageBeastSelfHeal.m_healthThresholdMod.GetModifiedValue(this.m_healthThreshold);
		}
		else
		{
			val3 = this.m_healthThreshold;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RageBeastSelfHeal))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_RageBeastSelfHeal);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}

	private void SetCachedFields()
	{
		StandardActorEffectData cachedStandardActorEffectData;
		if (this.m_abilityMod)
		{
			cachedStandardActorEffectData = this.m_abilityMod.m_standardActorEffectDataMod.GetModifiedValue(this.m_standardActorEffectData);
		}
		else
		{
			cachedStandardActorEffectData = this.m_standardActorEffectData;
		}
		this.m_cachedStandardActorEffectData = cachedStandardActorEffectData;
	}

	public StandardActorEffectData GetStandardActorEffectData()
	{
		StandardActorEffectData result;
		if (this.m_cachedStandardActorEffectData != null)
		{
			result = this.m_cachedStandardActorEffectData;
		}
		else
		{
			result = this.m_standardActorEffectData;
		}
		return result;
	}

	public bool ShouldHealOverTime()
	{
		bool result;
		if (this.m_abilityMod == null)
		{
			result = this.m_healOverTime;
		}
		else
		{
			result = this.m_abilityMod.m_healOverTimeMod.GetModifiedValue(this.m_healOverTime);
		}
		return result;
	}

	public int ModdedHealthThreshold()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = this.m_healthThreshold;
		}
		else
		{
			result = this.m_abilityMod.m_healthThresholdMod.GetModifiedValue(this.m_healthThreshold);
		}
		return result;
	}

	public int ModdedHealOnCastIfUnder()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = this.m_healingOnCastIfUnder;
		}
		else
		{
			result = this.m_abilityMod.m_lowHealthHealOnCastMod.GetModifiedValue(this.m_healingOnCastIfUnder);
		}
		return result;
	}

	public int ModdedHealOnTickIfUnder()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = this.m_healingOnTickIfUnder;
		}
		else
		{
			result = this.m_abilityMod.m_lowHealthHealOnTickMod.GetModifiedValue(this.m_healingOnTickIfUnder);
		}
		return result;
	}

	public int ModdedHealOnCastIfOver()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = this.m_healingOnCastIfOver;
		}
		else
		{
			result = this.m_abilityMod.m_highHealthOnCastMod.GetModifiedValue(this.m_healingOnCastIfOver);
		}
		return result;
	}

	public int ModdedHealOnTickIfOver()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = this.m_healingOnTickIfOver;
		}
		else
		{
			result = this.m_abilityMod.m_highHealthOnTickMod.GetModifiedValue(this.m_healingOnTickIfOver);
		}
		return result;
	}

	public override bool DoesTargetActorMatchTooltipSubject(AbilityTooltipSubject subjectType, ActorData targetActor, Vector3 damageOrigin, ActorData targetingActor)
	{
		if (subjectType != AbilityTooltipSubject.HighHP)
		{
			if (subjectType != AbilityTooltipSubject.LowHP)
			{
				return base.DoesTargetActorMatchTooltipSubject(subjectType, targetActor, damageOrigin, targetingActor);
			}
		}
		bool flag = targetActor.HitPoints <= this.ModdedHealthThreshold();
		bool result;
		if (subjectType == AbilityTooltipSubject.LowHP)
		{
			result = flag;
		}
		else
		{
			result = !flag;
		}
		return result;
	}

	private int GetHealingForCurrentHealth(ActorData caster)
	{
		bool flag = caster.HitPoints <= this.ModdedHealthThreshold();
		int result;
		if (flag)
		{
			this.GetStandardActorEffectData().m_healingPerTurn = this.ModdedHealOnTickIfUnder();
			result = this.ModdedHealOnCastIfUnder();
		}
		else
		{
			this.GetStandardActorEffectData().m_healingPerTurn = this.ModdedHealOnTickIfOver();
			result = this.ModdedHealOnCastIfOver();
		}
		return result;
	}
}
