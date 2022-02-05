// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameplayResponseForActor
{
	public StandardEffectInfo m_effect;

	// removed in rogues
	[Space(5f)]
	public int m_credits;

	// rogues
	//[Header("-- Damage/Heal by multiply on Power of actor")]
	//public float m_minDamageCoeff;
	//public float m_maxDamageCoeff;
	//public float m_minHealCoeff;
	//public float m_maxHealCoeff;

	[Header("-- direct values, used if not using multipliers")]  // added in rogues
	public int m_healing;
	public int m_damage;
	public int m_techPoints;

	// added in rogues
	//public bool m_resetCooldowns;

	// added in rogues
	//public bool m_fullHeal;

	public AbilityStatMod[] m_permanentStatMods;
	public StatusType[] m_permanentStatusChanges;
	public GameObject m_sequenceToPlay;

	// added in rogues
#if SERVER
	public void ResetValues()
	{
		// rogues
		//this.m_minDamageCoeff = 0f;
		//this.m_maxDamageCoeff = 0f;
		//this.m_minHealCoeff = 0f;
		//this.m_maxHealCoeff = 0f;
		m_healing = 0;
		m_damage = 0;
		m_techPoints = 0;
		//m_resetCooldowns = false;
		//m_fullHeal = false;
		m_effect = new StandardEffectInfo
		{
			m_effectData = new StandardActorEffectData()
		};
		m_effect.m_effectData.InitWithDefaultValues();
		m_permanentStatMods = new AbilityStatMod[0];
		m_permanentStatusChanges = new StatusType[0];

		// custom
		m_credits = 0;
	}
#endif

	public virtual void ReportAbilityTooltipNumbers(ref List<AbilityTooltipNumber> numbers, AbilityTooltipSubject subject)
	{
		if (m_damage != 0)
		{
			numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, subject, m_damage));
		}
		if (m_healing != 0)
		{
			numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, subject, m_healing));
		}
		m_effect.ReportAbilityTooltipNumbers(ref numbers, subject);
	}

	public GameplayResponseForActor GetShallowCopy()
	{
		return (GameplayResponseForActor)MemberwiseClone();
	}

	public void AddTooltipTokens(List<TooltipTokenEntry> tokens, string name, bool addCompare, GameplayResponseForActor other)
	{
		bool addDiff = addCompare && other != null;
		// rogues
		//TooltipTokenHelper.AddTokenRange(tokens, name + "_DamageRange", this.m_minDamageCoeff, this.m_maxDamageCoeff, "");
		//TooltipTokenHelper.AddTokenRange(tokens, name + "_HealRange", this.m_minHealCoeff, this.m_maxHealCoeff, "");

		int damageVal = addDiff ? other.m_damage : 0;
		AbilityMod.AddToken_IntDiff(tokens, name + "_Damage", "damage on response", m_damage, addDiff, damageVal);
		int healingVal = addDiff ? other.m_healing : 0;
		AbilityMod.AddToken_IntDiff(tokens, name + "_Healing", "healing on response", m_healing, addDiff, healingVal);
		StandardEffectInfo effectVal = addDiff ? other.m_effect : null;
		AbilityMod.AddToken_EffectInfo(tokens, m_effect, name + "_Effect", effectVal);
	}

	public string GetInEditorDescription(string header = "- Response -", string indent = "", bool showDiff = false, GameplayResponseForActor other = null)
	{
		bool addDiff = showDiff && other != null;
		string otherSep = "\t        \t | in base  =";
		string desc = "\n" + InEditorDescHelper.BoldedStirng(header) + "\n"
			// rogues
			//+ InEditorDescHelper.AssembleFieldWithDiff("[ Min Damage Coeff ] = ", indent, otherSep, this.m_minDamageCoeff, addDiff, addDiff ? other.m_minDamageCoeff : 0f, null)
			//+ InEditorDescHelper.AssembleFieldWithDiff("[ Max Damage Coeff ] = ", indent, otherSep, this.m_maxDamageCoeff, addDiff, addDiff ? other.m_maxDamageCoeff : 0f, null)
			//+ InEditorDescHelper.AssembleFieldWithDiff("[ Min Heal Coeff ] = ", indent, otherSep, this.m_minHealCoeff, addDiff, addDiff ? other.m_minHealCoeff : 0f, null)
			//+ InEditorDescHelper.AssembleFieldWithDiff("[ Max Heal Coeff ] = ", indent, otherSep, this.m_maxHealCoeff, addDiff, addDiff ? other.m_maxHealCoeff : 0f, null)
			;

		// removed in rogues
		float otherCredits = addDiff ? other.m_credits : 0;
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ Credits ] = ", indent, otherSep, m_credits, addDiff, otherCredits, ((float f) => f != 0f));
		
		int otherHealing = addDiff ? other.m_healing : 0;
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ Healing ] = ", indent, otherSep, m_healing, addDiff, otherHealing);
		int otherDamage = addDiff ? other.m_damage : 0;
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ Damage ] = ", indent, otherSep, m_damage, addDiff, otherDamage);
		float otherTechPoints = addDiff ? other.m_techPoints : 0;
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ TechPoints ] = ", indent, otherSep, m_techPoints, addDiff, otherTechPoints, ((float f) => f != 0f));
		StandardEffectInfo otherEffect = addDiff ? other.m_effect : null;
		desc += AbilityModHelper.GetModEffectInfoDesc(m_effect, "{ Effect on Moved-Through }", indent, addDiff, otherEffect);
		AbilityStatMod[] otherMods = addDiff ? other.m_permanentStatMods : null;
		desc += InEditorDescHelper.GetListDiffString("Permanent Stat Mods:", indent, m_permanentStatMods, addDiff, otherMods);
		StatusType[] otherStatuses = addDiff ? other.m_permanentStatusChanges : null;
		desc += InEditorDescHelper.GetListDiffString("Permanent Status Changes:", indent, m_permanentStatusChanges, addDiff, otherStatuses);
		GameObject otherSequence = addDiff ? other.m_sequenceToPlay : null;
		desc += InEditorDescHelper.AssembleFieldWithDiff("Response Hit Sequence", indent, otherSep, m_sequenceToPlay, addDiff, otherSequence);
		return desc + indent + "-- END of Move-Through Response Output --\n";
	}

	public bool HasResponse()
	{
		return m_effect.m_applyEffect
			// rogues
			//|| this.m_minDamageCoeff > 0f
			//|| this.m_minHealCoeff > 0f

			|| m_healing != 0
			|| m_damage != 0
			|| m_techPoints != 0
			|| m_permanentStatMods != null && m_permanentStatMods.Length > 0
			|| m_permanentStatusChanges != null && m_permanentStatusChanges.Length > 0
			|| m_sequenceToPlay != null;
			// added in rogues
			//|| m_fullHeal 
			//|| m_resetCooldowns;
	}

	// added in rogues
#if SERVER
	public ActorHitResults ConvertToActorHitResults(ActorData targetActor, Vector3 origin, Barrier barrier, Ability sourceAbility)
	{
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(targetActor, origin));
		int num = m_damage;
		if (barrier != null && sourceAbility != null)
		{
			num = sourceAbility.GetBarrierDamageForActor(num, targetActor, origin, barrier);
		}
		actorHitResults.AddStandardEffectInfo(m_effect);

		// rogues
		//if (this.m_minDamageCoeff > 0f)
		//{
		//	actorHitResults.ModifyDamageCoeff(this.m_minDamageCoeff, this.m_maxDamageCoeff);
		//}
		//else
		if (num > 0)
		{
			actorHitResults.AddBaseDamage(num);
		}

		// rogues
		//if (this.m_minHealCoeff > 0f)
		//{
		//	actorHitResults.ModifyHealingCoeff(this.m_minHealCoeff, this.m_maxHealCoeff);
		//}
		//else
		if (m_healing > 0)
		{
			actorHitResults.AddBaseHealing(m_healing);
		}
		if (m_techPoints > 0)
		{
			actorHitResults.AddTechPointGain(m_techPoints);
		}
		else if (m_techPoints < 0)
		{
			actorHitResults.AddTechPointLoss(Mathf.Abs(m_techPoints));
		}
		// rogues
		//if (m_resetCooldowns)
		//{
		//	actorHitResults.AddMiscHitEvent(new MiscHitEventData(MiscHitEventType.ClearCharacterAbilityCooldowns));
		//}
		//if (m_fullHeal)
		//{
		//	actorHitResults.AddBaseHealing(targetActor.GetMaxHitPoints());
		//}
		actorHitResults.AddPermanentStatMods(m_permanentStatMods);
		actorHitResults.AddPermanentStatusChanges(m_permanentStatusChanges);
		return actorHitResults;
	}
#endif
}
