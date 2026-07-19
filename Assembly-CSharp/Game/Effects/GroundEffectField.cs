// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

// reactor
[Serializable]
// rogues
//[CreateAssetMenu(fileName = "NewGroundEffectField", menuName = "Design Objects/Ground Effect Field")]
public class GroundEffectField
// added in rogues
//: ScriptableObject
{
	public int duration = 2;
	public int hitDelayTurns;
	public AbilityAreaShape shape = AbilityAreaShape.Three_x_Three;
	public bool canIncludeCaster = true;
	[Header("-- Whether to ignore movement hits")]
	public bool ignoreMovementHits;
	public bool endIfHasDoneHits;
	public bool ignoreNonCasterAllies;
	[Header("-- For Hits --")]
	// rogues
	//public List<OnHitAuthoredData> onHitData = new List<OnHitAuthoredData>();
	//[HideInInspector] // added in rogues
	public int damageAmount;
	//[HideInInspector] // added in rogues
	public int subsequentDamageAmount;
	//[HideInInspector] // added in rogues
	public int healAmount;
	//[HideInInspector] // added in rogues
	public int subsequentHealAmount;
	// rogues
	//[HideInInspector]
	//public float damageCoeff;
	// rogues
	//[HideInInspector]
	//public float healCoeff;
	//[HideInInspector] // added in rogues
	public int energyGain;
	//[HideInInspector] // added in rogues
	public int subsequentEnergyGain;
	public bool stopMovementInField;
	public bool stopMovementOutOfField;
	//[HideInInspector] // added in rogues
	public StandardEffectInfo effectOnEnemies;
	//[HideInInspector] // added in rogues
	public StandardEffectInfo effectOnAllies;
	[Header("-- Sequences --")]
	public GameObject persistentSequencePrefab;
	public GameObject hitPulseSequencePrefab;
	public GameObject allyHitSequencePrefab;
	public GameObject enemyHitSequencePrefab;
	public bool perSquareSequences;

	internal bool penetrateLos => true;

	// rogues
	//   private static bool AffectsEnemies(OnHitAuthoredData data)
	//{
	//	return data != null &&
	//		(
	//			data.m_effectTemplateFields.Count > 0
	//			|| data.m_enemyHitIntFields.Count > 0
	//			|| data.m_enemyHitKnockbackFields.Count > 0
	//			|| data.m_enemyHitCooldownReductionFields.Count > 0
	//			|| data.m_enemyHitEffectFields.Count > 0
	//			|| data.m_enemyHitEffectTemplateFields.Count > 0);
	//}

	// rogues
	//private static bool AffectsAllies(OnHitAuthoredData data)
	//{
	//	return data != null && (
	//			data.m_effectTemplateFields.Count > 0
	//			|| data.m_allyHitIntFields.Count > 0
	//			|| data.m_allyHitKnockbackFields.Count > 0
	//			|| data.m_allyHitCooldownReductionFields.Count > 0
	//			|| data.m_allyHitEffectFields.Count > 0
	//			|| data.m_allyHitEffectTemplateFields.Count > 0);
	//}

	public void ReportAbilityTooltipNumbers(ref List<AbilityTooltipNumber> numbers, AbilityTooltipSubject enemySubject, AbilityTooltipSubject allySubject)
	{
		if (effectOnEnemies != null)  // NOTE CHANGE null check added in rogues
		{
			effectOnEnemies.ReportAbilityTooltipNumbers(ref numbers, enemySubject);
		}
		if (effectOnAllies != null)  // NOTE CHANGE null check added in rogues
		{
			effectOnAllies.ReportAbilityTooltipNumbers(ref numbers, allySubject);
		}
		AbilityTooltipHelper.ReportDamage(ref numbers, enemySubject, damageAmount);
		AbilityTooltipHelper.ReportHealing(ref numbers, allySubject, healAmount);
		AbilityTooltipHelper.ReportEnergy(ref numbers, allySubject, energyGain);
	}

	// reactor
	public bool IncludeEnemies()
	{
		return damageAmount > 0 || effectOnEnemies.m_applyEffect;
	}
	// rogues
	//public bool IncludeEnemies()
	//{
	//	for (int i = 0; i < onHitData.Count; i++)
	//	{
	//		if (AffectsEnemies(onHitData[i]))
	//		{
	//			return true;
	//		}
	//	}
	//	return false;
	//}

	// reactor
	public bool IncludeAllies()
	{
		return healAmount > 0 || effectOnAllies.m_applyEffect || energyGain > 0;
	}

	// rogues
	//public bool IncludeAllies()
	//{
	//	for (int i = 0; i < onHitData.Count; i++)
	//	{
	//		if (GroundEffectField.AffectsAllies(onHitData[i]))
	//		{
	//			return true;
	//		}
	//	}
	//	return false;
	//}

	public List<Team> GetAffectedTeams(ActorData allyActor)
	{
		return TargeterUtils.GetRelevantTeams(allyActor, IncludeAllies(), IncludeEnemies());
	}

	public void AddTooltipTokens(List<TooltipTokenEntry> tokens, string name, bool addCompare = false, GroundEffectField other = null)
	{
		bool addDiff = addCompare && other != null;
		AbilityMod.AddToken_IntDiff(tokens, name + "_Duration", "", duration, addDiff, addDiff ? other.duration : 0);
		AbilityMod.AddToken_IntDiff(tokens, name + "_HitDelayTurns", "", hitDelayTurns, addDiff, addDiff ? other.hitDelayTurns : 0);
		AbilityMod.AddToken_IntDiff(tokens, name + "_Damage", "", damageAmount, addDiff, addDiff ? other.damageAmount : 0);
		AbilityMod.AddToken_IntDiff(tokens, name + "_SubsequentDamage", "", subsequentDamageAmount, addDiff, addDiff ? other.subsequentDamageAmount : 0);
		AbilityMod.AddToken_IntDiff(tokens, name + "_Healing", "", healAmount, addDiff, addDiff ? other.healAmount : 0);
		AbilityMod.AddToken_IntDiff(tokens, name + "_SubsequentHealing", "", subsequentHealAmount, addDiff, addDiff ? other.subsequentHealAmount : 0);
		AbilityMod.AddToken_IntDiff(tokens, name + "_AllyEnergyGain", "", energyGain, addDiff, addDiff ? other.energyGain : 0);
		AbilityMod.AddToken_IntDiff(tokens, name + "_SubsequentEnergyGain", "", subsequentEnergyGain, addDiff, addDiff ? other.subsequentEnergyGain : 0);
		AbilityMod.AddToken_EffectInfo(tokens, effectOnEnemies, "EnemyHitEffect", addDiff ? other.effectOnEnemies : null, addDiff);
		AbilityMod.AddToken_EffectInfo(tokens, effectOnAllies, "AllyHitEffect", addDiff ? other.effectOnAllies : null, addDiff);

		// rogues
		//foreach (OnHitAuthoredData onHitAuthoredData in onHitData)
		//{
		//	onHitAuthoredData.AddTooltipTokens(tokens);
		//}
	}

	public GroundEffectField GetShallowCopy()
	{
		return (GroundEffectField)MemberwiseClone();
	}

	// added in rogues
#if SERVER
	public List<ActorData> GetAffectableActorsInField(AbilityTarget abilityTarget, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		BoardSquare square = Board.Get().GetSquare(abilityTarget.GridPos);
		return GetAffectableActorsInField(square, abilityTarget.FreePos, caster, nonActorTargetInfo);
	}
#endif

	// added in rogues
#if SERVER
	public List<ActorData> GetAffectableActorsInField(BoardSquare targetSquare, Vector3 freePos, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(shape, freePos, targetSquare, penetrateLos, caster, null, nonActorTargetInfo);
		List<ActorData> list = new List<ActorData>();
		foreach (ActorData actorData in actorsInShape)
		{
			if (CanBeAffected(actorData, caster))
			{
				list.Add(actorData);
			}
		}
		return list;
	}
#endif

	// added in rogues
#if SERVER
	public bool CanBeAffected(ActorData actor, ActorData caster)
	{
		bool isAlly = actor.GetTeam() == caster.GetTeam();
		return (!isAlly && IncludeEnemies())
		       || (isAlly && IncludeAllies() && (canIncludeCaster || actor != caster) && (!ignoreNonCasterAllies || actor == caster));
	}
#endif

	// added in rogues
#if SERVER
	public void SetupActorHitResult(ref ActorHitResults hitRes, ActorData caster, BoardSquare targetSquare, int numHits = 1)
	{
		ActorData target = hitRes.m_hitParameters.Target;
		if (numHits > 0)
		{
			bool flag = target.GetTeam() == caster.GetTeam();
			if (!flag)
			{
				int num = damageAmount + (numHits - 1) * subsequentDamageAmount;
				if (num > 0)
				{
					hitRes.AddBaseDamage(num);
				}
				// rogues
				//else
				//{
				//    hitRes.ModifyDamageCoeff(this.damageCoeff, this.damageCoeff);
				//}
				hitRes.AddStandardEffectInfo(effectOnEnemies);
			}
			else
			{
				int num2 = healAmount + (numHits - 1) * subsequentHealAmount;
				int addAmount = energyGain + (numHits - 1) * subsequentEnergyGain;
				if (num2 > 0)
				{
					hitRes.AddBaseHealing(num2);
				}
				// rogues
				//else
				//{
				//                hitRes.ModifyHealingCoeff(this.healCoeff, this.healCoeff);
				//            }
				hitRes.AddTechPointGain(addAmount);
				hitRes.AddStandardEffectInfo(effectOnAllies);
			}
			
			// rogues - this effect does not use generic ability system in reactor
			// ActorHitContext actorContext = new ActorHitContext();
			// ContextVars abilityContext = new ContextVars();
			// NumericHitResultScratch numericHitResultScratch = new NumericHitResultScratch();
			// foreach (OnHitAuthoredData onHitAuthoredData in onHitData)
			// {
			// 	if (flag)
			// 	{
			// 		GenericAbility_Container.CalcIntFieldValues(target, caster, actorContext, abilityContext, onHitAuthoredData.m_allyHitIntFields, numericHitResultScratch);
			// 		GenericAbility_Container.SetNumericFieldsOnHitResults(hitRes, numericHitResultScratch);
			// 		GenericAbility_Container.SetKnockbackFieldsOnHitResults(target, caster, actorContext, abilityContext, hitRes, onHitAuthoredData.m_allyHitKnockbackFields);
			// 		GenericAbility_Container.SetCooldownReductionFieldsOnHitResults(target, caster, actorContext, abilityContext, hitRes, onHitAuthoredData.m_allyHitCooldownReductionFields, numHits);
			// 		GenericAbility_Container.SetEffectFieldsOnHitResults(target, caster, actorContext, abilityContext, hitRes, onHitAuthoredData.m_allyHitEffectFields);
			// 		GenericAbility_Container.SetEffectTemplateFieldsOnHitResults(target, caster, actorContext, abilityContext, hitRes, onHitAuthoredData.m_allyHitEffectTemplateFields);
			// 	}
			// 	else
			// 	{
			// 		GenericAbility_Container.CalcIntFieldValues(target, caster, actorContext, abilityContext, onHitAuthoredData.m_enemyHitIntFields, numericHitResultScratch);
			// 		GenericAbility_Container.SetNumericFieldsOnHitResults(hitRes, numericHitResultScratch);
			// 		GenericAbility_Container.SetKnockbackFieldsOnHitResults(target, caster, actorContext, abilityContext, hitRes, onHitAuthoredData.m_enemyHitKnockbackFields);
			// 		GenericAbility_Container.SetCooldownReductionFieldsOnHitResults(target, caster, actorContext, abilityContext, hitRes, onHitAuthoredData.m_enemyHitCooldownReductionFields, numHits);
			// 		GenericAbility_Container.SetEffectFieldsOnHitResults(target, caster, actorContext, abilityContext, hitRes, onHitAuthoredData.m_enemyHitEffectFields);
			// 		GenericAbility_Container.SetEffectTemplateFieldsOnHitResults(target, caster, actorContext, abilityContext, hitRes, onHitAuthoredData.m_enemyHitEffectTemplateFields);
			// 	}
			// }
		}
	}
#endif

	public string GetInEditorDescription(string header = "GroundEffectField", string indent = "    ", bool diff = false, GroundEffectField other = null)
	{
		bool addDiff = diff && other != null;
		string otherSep = "\t        \t | in base  =";
		string desc = InEditorDescHelper.BoldedStirng(header) + "\n";
		if (duration <= 0)
		{
			desc += indent + "WARNING: IS PERMANENT (duration <= 0). Woof Woof Woof Woof\n";
		}
		else
		{
			desc += InEditorDescHelper.AssembleFieldWithDiff("[ Max Duration ] = ", indent, otherSep, duration, addDiff, addDiff ? other.duration : 0);
		}
		if (hitDelayTurns > 0)
		{
			desc += InEditorDescHelper.AssembleFieldWithDiff("[ Hit Delay Turns ] = ", indent, otherSep, hitDelayTurns, addDiff, addDiff ? other.hitDelayTurns : 0);
		}
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ Shape ] = ", indent, otherSep, shape, addDiff, addDiff ? other.shape : AbilityAreaShape.SingleSquare);
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ Ignore Movement Hits? ] = ", indent, otherSep, ignoreMovementHits, addDiff, addDiff && other.ignoreMovementHits);
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ End If Has Done Hits? ] = ", indent, otherSep, endIfHasDoneHits, addDiff, addDiff && other.endIfHasDoneHits);
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ Ignore Non Caster Allies? ] = ", indent, otherSep, ignoreNonCasterAllies, addDiff, addDiff && other.ignoreNonCasterAllies);
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ Damage ] = ", indent, otherSep, damageAmount, addDiff, addDiff ? other.damageAmount : 0);
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ Subsequent Damage ] = ", indent, otherSep, subsequentDamageAmount, addDiff, addDiff ? other.subsequentDamageAmount : 0);
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ Healing ] = ", indent, otherSep, healAmount, addDiff, addDiff ? other.healAmount : 0);
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ Subsequent Healing ] = ", indent, otherSep, subsequentHealAmount, addDiff, addDiff ? other.subsequentHealAmount : 0);
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ EnergyGain ] = ", indent, otherSep, energyGain, addDiff, addDiff ? other.energyGain : 0);
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ Subsequent EnergyGain ] = ", indent, otherSep, subsequentEnergyGain, addDiff, addDiff ? other.subsequentEnergyGain : 0);
		if (effectOnEnemies.m_applyEffect)
		{
			desc += indent + "Effect on Enemies:\n";
			desc += effectOnEnemies.m_effectData.GetInEditorDescription(indent, false, addDiff, addDiff ? other.effectOnEnemies.m_effectData : null);
		}
		if (effectOnAllies.m_applyEffect)
		{
			desc += indent + "Effect on Allies:\n";
			desc += effectOnAllies.m_effectData.GetInEditorDescription(indent, false, addDiff, addDiff ? other.effectOnAllies.m_effectData : null);
		}
		desc += InEditorDescHelper.AssembleFieldWithDiff("Persistent Sequence Prefab", indent, otherSep, persistentSequencePrefab, addDiff, addDiff ? other.persistentSequencePrefab : null);
		desc += InEditorDescHelper.AssembleFieldWithDiff("Hit Pulse Sequence", indent, otherSep, hitPulseSequencePrefab, addDiff, addDiff ? other.hitPulseSequencePrefab : null);
		desc += InEditorDescHelper.AssembleFieldWithDiff("Ally Hit Sequence", indent, otherSep, allyHitSequencePrefab, addDiff, addDiff ? other.allyHitSequencePrefab : null);
		desc += InEditorDescHelper.AssembleFieldWithDiff("Enemy Hit Sequence", indent, otherSep, enemyHitSequencePrefab, addDiff, addDiff ? other.enemyHitSequencePrefab : null);
		return desc + "-  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -\n";
	}
}
