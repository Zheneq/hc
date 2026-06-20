// ROGUES
// SERVER
//using System;
using System.Collections.Generic;
using System.Linq;
using AbilityContextNamespace;
//using EffectSystem;
using UnityEngine;

public class GenericAbility_Container : Ability
{
	[Separator("Target Select Component")]
	public GenericAbility_TargetSelectBase m_targetSelectComp;
	[Separator("On Hit Authored Data", "yellow")]
	public OnHitAuthoredData m_onHitData;
	protected NumericHitResultScratch m_calculatedValuesForTargeter = new NumericHitResultScratch();
	protected OnHitAuthoredData m_cachedOnHitData;

	public virtual string GetUsageForEditor()
	{
		string text = "";

		// added in rogues
#if SERVER
		text += ContextVars.GetContextUsageStr(ContextKeys.s_TargetHealthPercentage.GetName(), "the percent of max health the target actor is currently at");
		text += ContextVars.GetContextUsageStr(ContextKeys.s_PercentHealthLost.GetName(), "the percent below max health the target actor is currently at");
		text += ContextVars.GetContextUsageStr(ContextKeys.s_NumEnemyHits.GetName(), "how many enemies are hit in total");
		text += ContextVars.GetContextUsageStr(ContextKeys.s_NumAllyHits.GetName(), "how many allies are hit in total");
#endif

		if (m_targetSelectComp != null)
		{
			text += m_targetSelectComp.GetUsageForEditor();
		}
		return text;
	}

	public virtual string GetOnHitDataDesc()
	{
		if (m_onHitData != null)
		{
			return m_onHitData.GetInEditorDesc();
		}
		return "";
	}

	public virtual List<string> GetContextNamesForEditor()
	{
		List<string> list = new List<string>();

		// added in rogues
#if SERVER
		list.Add(ContextKeys.s_TargetHealthPercentage.GetName());
		list.Add(ContextKeys.s_PercentHealthLost.GetName());
		list.Add(ContextKeys.s_NumEnemyHits.GetName());
		list.Add(ContextKeys.s_NumAllyHits.GetName());
#endif

		if (m_targetSelectComp != null)
		{
			m_targetSelectComp.ListContextNamesForEditor(list);
		}
		return list;
	}

	// added in rogues
#if SERVER
	public List<string> GetOnHitIdentifiersForEditor()
	{
		HashSet<string> hashSet = new HashSet<string>();
		foreach (OnHitIntField onHitIntField in m_onHitData.m_enemyHitIntFields)
		{
			hashSet.Add(onHitIntField.GetIdentifier());
		}
		foreach (OnHitEffecField onHitEffecField in m_onHitData.m_enemyHitEffectFields)
		{
			hashSet.Add(onHitEffecField.GetIdentifier());
		}
		// rogues
		//foreach (OnHitKnockbackField onHitKnockbackField in m_onHitData.m_enemyHitKnockbackFields)
		//{
		//	hashSet.Add(onHitKnockbackField.GetIdentifier());
		//}
		foreach (OnHitIntField onHitIntField2 in m_onHitData.m_allyHitIntFields)
		{
			hashSet.Add(onHitIntField2.GetIdentifier());
		}
		foreach (OnHitEffecField onHitEffecField2 in m_onHitData.m_allyHitEffectFields)
		{
			hashSet.Add(onHitEffecField2.GetIdentifier());
		}
		foreach (OnHitBarrierField onHitBarrierField in m_onHitData.m_barrierSpawnFields)
		{
			hashSet.Add(onHitBarrierField.GetIdentifier());
		}
		// rogues
		//foreach (OnHitGroundEffectField onHitGroundEffectField in m_onHitData.m_groundEffectFields)
		//{
		//	hashSet.Add(onHitGroundEffectField.GetIdentifier());
		//}
		return hashSet.ToList();
	}
#endif

	public virtual List<GenericAbility_TargetSelectBase> GetRelevantTargetSelectCompForEditor()
	{
		List<GenericAbility_TargetSelectBase> list = new List<GenericAbility_TargetSelectBase>();
		if (m_targetSelectComp != null)
		{
			list.Add(m_targetSelectComp);
		}
		return list;
	}

	private void Start()
	{
		SetupTargetersAndCachedVars();
	}

	protected virtual void SetupTargetersAndCachedVars()
	{
		ClearTargeters();
		if (GetTargetSelectComp() != null)
		{
			// added in rogues
#if SERVER
			GetTargetSelectComp().BaseInit();
#endif

			GetTargetSelectComp().Initialize();
			foreach (AbilityUtil_Targeter targeter in GetTargetSelectComp().CreateTargeters(this))
			{
				Targeters.Add(targeter);
			}
		}
		if (CurrentAbilityMod != null)
		{
			m_cachedOnHitData = CurrentAbilityMod.GenModImpl_GetModdedOnHitData(m_onHitData); // (m_onHitData, this) in rogues
		}
		else
		{
			m_cachedOnHitData = m_onHitData;
		}
	}

	public virtual OnHitAuthoredData GetOnHitAuthoredData()
	{
		return m_cachedOnHitData == null ? m_onHitData : m_cachedOnHitData;
	}

	public virtual GenericAbility_TargetSelectBase GetTargetSelectComp()
	{
		// rogues
		// GenericAbility_AbilityMod genericAbility_AbilityMod = base.CurrentAbilityMod as GenericAbility_AbilityMod;
		// if (genericAbility_AbilityMod != null && genericAbility_AbilityMod.m_targetSelectOverride != null)
		// {
		// 	return genericAbility_AbilityMod.m_targetSelectOverride;
		// }

		return m_targetSelectComp;
	}

	// added in rogues
#if SERVER
	public override int GetExpectedNumberOfTargeters()
	{
		if (GetTargetData() != null)
		{
			return GetTargetData().Length;
		}
		return base.GetExpectedNumberOfTargeters();
	}
#endif

	public override TargetData[] GetBaseTargetData()
	{
		GenericAbility_TargetSelectBase targetSelectComp = GetTargetSelectComp();
		if (targetSelectComp != null
			&& targetSelectComp.m_useTargetDataOverride
			&& targetSelectComp.GetTargetDataOverride() != null)
		{
			return targetSelectComp.GetTargetDataOverride();
		}
		return base.GetBaseTargetData();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		m_onHitData.AddTooltipTokens(tokens);
#if SERVER
		foreach (Ability ability in m_chainAbilities)
		{
			tokens.AddRange(ability.GetTooltipTokenEntries(modAsBase));
		}
#endif
	}

	// rogues
	//public override string GetFullTooltip(int powerLevel, int expertise, int strength)
	//{
	//	string text = base.GetFullTooltip(powerLevel, expertise, strength) + "\n";
	//	IEnumerable<OnHitEffectTemplateField> source = from field in m_onHitData.m_enemyHitEffectTemplateFields.Concat(m_onHitData.m_allyHitEffectTemplateFields).Concat(m_onHitData.m_effectTemplateFields)
	//	where field != null && field.m_effectTemplate != null
	//	select field;
	//	if (source.Any<OnHitEffectTemplateField>())
	//	{
	//		string str = string.Join("\n", source.Select(delegate(OnHitEffectTemplateField field)
	//		{
	//			List<TooltipTokenEntry> list = new List<TooltipTokenEntry>();
	//			SortedSet<EffectTemplate> searched = new SortedSet<EffectTemplate>();
	//			field.m_effectTemplate.AddTooltipTokens(searched, list, field.m_effectTemplate.name, false, null);
	//			AbilityTooltipTokenContext abilityTooltipTokenContext = new AbilityTooltipTokenContext();
	//			abilityTooltipTokenContext.m_powerLevel = powerLevel;
	//			abilityTooltipTokenContext.actorData = ActorData;
	//			abilityTooltipTokenContext.ability = this;
	//			abilityTooltipTokenContext.gearStatData = null;
	//			abilityTooltipTokenContext.effectTemplate = field.m_effectTemplate;
	//			abilityTooltipTokenContext.isMatchData = (AppState.IsInGame() || AppState.GetCurrent() == AppState_GameLoading.Get());
	//			foreach (TooltipTokenEntry tooltipTokenEntry in list)
	//			{
	//				if (tooltipTokenEntry is TooltipTokenInt || tooltipTokenEntry is TooltipTokenFloat)
	//				{
	//					abilityTooltipTokenContext.tooltipTokenEntries.Add(tooltipTokenEntry);
	//				}
	//				else if (tooltipTokenEntry is TooltipTokenScript)
	//				{
	//					abilityTooltipTokenContext.tooltipTokenEntries.Add(tooltipTokenEntry);
	//				}
	//			}
	//			return TooltipTokenEntry.GetTooltipWithSubstitutes(field.m_effectTemplate.LocalizedTooltip, list, abilityTooltipTokenContext, false);
	//		}));
	//		return text + str;
	//	}
	//	return text;
	//}

	public override bool CanShowTargetableRadiusPreview()
	{
		if (GetTargetSelectComp() != null)
		{
			return GetTargetSelectComp().CanShowTargeterRangePreview(GetTargetData());
		}
		return base.CanShowTargetableRadiusPreview();
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		if (GetTargetSelectComp() != null)
		{
			return GetTargetSelectComp().GetTargeterRangePreviewRadius(this, caster);
		}
		return base.GetTargetableRadiusInSquares(caster);
	}

	protected virtual void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		// removed in rogues
		Log.Error("Please implement GenModImpl_SetModRef in derived class " + GetType());
	}

	protected virtual void GenModImpl_ClearModRef()
	{
		// removed in rogues
		Log.Error("Please implement GenModImpl_ClearModRef in derived class " + GetType());
	}

	protected virtual void SetTargetSelectModReference() // (AbilityMod currentMod) in rogues
	{
		if (m_targetSelectComp == null)
		{
			return;
		}
		// reactor
		if (CurrentAbilityMod != null)
		{
			CurrentAbilityMod.GenModImpl_SetTargetSelectMod(m_targetSelectComp);
		}
		else
		{
			m_targetSelectComp.ClearTargetSelectMod();
		}
		// rogues
		//if (currentMod != null)
		//{
		//	currentMod.GenModImpl_SetTargetSelectMod(m_targetSelectComp);
		//	return;
		//}
		//else
		//{
		//	m_targetSelectComp.ClearTargetSelectMod();
		//}
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		GenModImpl_SetModRef(abilityMod);
		SetTargetSelectModReference();  // (abilityMod) in rogues
		SetupTargetersAndCachedVars();
	}

	protected override void OnRemoveAbilityMod()
	{
		GenModImpl_ClearModRef();
		SetTargetSelectModReference();  // (null) in rogues
		SetupTargetersAndCachedVars();
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (GetTargetSelectComp() != null)
		{
			return GetTargetSelectComp().HandleCanCastValidation(this, caster);
		}
		return true;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (GetTargetSelectComp() != null)
		{
			return GetTargetSelectComp().HandleCustomTargetValidation(this, caster, target, targetIndex, currentTargets);
		}
		return true;
	}

	public override bool GetCustomTargeterNumbers(
		ActorData targetActor,
		int currentTargeterIndex,
		TargetingNumberUpdateScratch results)
	{
		ActorData actorData = ActorData;
		if (actorData == null)
		{
			return false;
		}
		GetHitContextForTargetingNumbers(
			currentTargeterIndex,
			out Dictionary<ActorData, ActorHitContext> actorHitContext,
			out ContextVars abilityContext);
		if (!actorHitContext.ContainsKey(targetActor))
		{
			return false;
		}
		PreProcessTargetingNumbers(targetActor, currentTargeterIndex, actorHitContext, abilityContext);
		m_calculatedValuesForTargeter.Reset();
		if (actorData.GetTeam() == targetActor.GetTeam())
		{
			CalcIntFieldValues(
				targetActor,
				actorData,
				actorHitContext[targetActor],
				abilityContext,
				GetOnHitAuthoredData().m_allyHitIntFields,
				m_calculatedValuesForTargeter);
			results.m_absorb = CalcAbsorbFromEffectFields(
				targetActor,
				actorData,
				actorHitContext[targetActor],
				abilityContext,
				GetOnHitAuthoredData().m_allyHitEffectFields);
		}
		else
		{
			CalcIntFieldValues(
				targetActor,
				actorData,
				actorHitContext[targetActor],
				abilityContext,
				GetOnHitAuthoredData().m_enemyHitIntFields,
				m_calculatedValuesForTargeter);
			results.m_absorb = CalcAbsorbFromEffectFields(
				targetActor,
				actorData,
				actorHitContext[targetActor],
				abilityContext,
				GetOnHitAuthoredData().m_enemyHitEffectFields);
		}

		// rogues
		//EquipmentStats equipmentStats = actorData.GetEquipmentStats();
		//int num = Mathf.RoundToInt(equipmentStats.GetTotalStatValueForSlot(GearStatType.PowerAdjustment, (float)actorData.GetBaseStatValue(GearStatType.PowerAdjustment), (int)base.CachedActionType, targetActor));
		//if (targetActor != null && targetActor.GetTeam() == actorData.GetTeam())
		//{
		//	num += Mathf.RoundToInt(equipmentStats.GetTotalStatValueForSlot(GearStatType.ExpertiseAdjustment, (float)actorData.GetBaseStatValue(GearStatType.ExpertiseAdjustment), (int)base.CachedActionType, targetActor));
		//}
		//else if (targetActor != null && targetActor.GetTeam() != actorData.GetTeam())
		//{
		//	num += Mathf.RoundToInt(equipmentStats.GetTotalStatValueForSlot(GearStatType.StrengthAdjustment, (float)actorData.GetBaseStatValue(GearStatType.StrengthAdjustment), (int)base.CachedActionType, targetActor));
		//}

		// reactor
		results.m_damage = m_calculatedValuesForTargeter.m_damage;
		results.m_healing = m_calculatedValuesForTargeter.m_healing;
		results.m_energy = m_calculatedValuesForTargeter.m_energyGain;
		// rogues
		//float num2 = 0.5f * (m_calculatedValuesForTargeter.m_damageMin + m_calculatedValuesForTargeter.m_damageMax);
		//float num3 = 0.5f * (m_calculatedValuesForTargeter.m_healingMin + m_calculatedValuesForTargeter.m_healingMax);
		//float num4 = 0.5f * (m_calculatedValuesForTargeter.m_energyGainMin + m_calculatedValuesForTargeter.m_energyGainMax);
		//results.m_damage = Mathf.RoundToInt(num2 * (float)num);
		//results.m_healing = Mathf.RoundToInt(num3 * (float)num);
		//results.m_energy = Mathf.RoundToInt(num4 * (float)num);

		PostProcessTargetingNumbers(targetActor, currentTargeterIndex, actorHitContext, abilityContext, actorData, results);
		return true;
	}

	public virtual void GetHitContextForTargetingNumbers(
		int currentTargeterIndex,
		out Dictionary<ActorData, ActorHitContext> actorHitContext,
		out ContextVars abilityContext)
	{
		actorHitContext = Targeter.GetActorContextVars();
		abilityContext = Targeter.GetNonActorSpecificContext();
	}

	// empty in reactor
	public virtual void PreProcessTargetingNumbers(
		ActorData targetActor,
		int currentTargetIndex,
		Dictionary<ActorData, ActorHitContext> actorHitContext,
		ContextVars abilityContext)
	{
#if SERVER
		int numEnemyHits  = 0;
		int numAllyHits  = 0;
		foreach (KeyValuePair<ActorData, ActorHitContext> keyValuePair in actorHitContext)
		{
			if (keyValuePair.Key == targetActor)
			{
				float hitPointPercent = keyValuePair.Key.GetHitPointPercent();
				float value = 1f - hitPointPercent;
				keyValuePair.Value.m_contextVars.SetValue(ContextKeys.s_PercentHealthLost.GetKey(), value);
				keyValuePair.Value.m_contextVars.SetValue(ContextKeys.s_TargetHealthPercentage.GetKey(), hitPointPercent);
				// rogues
				//foreach (OnHitKnockbackField onHitKnockbackField in m_onHitData.m_enemyHitKnockbackFields)
				//{
				//	if (TargetFilterHelper.ActorMeetsConditions(onHitKnockbackField.m_conditions, targetActor, base.ActorData, keyValuePair.Value, abilityContext))
				//	{
				//		GetTargetSelectComp().CreateKnockbackPreviewLines(base.ActorData, base.Targeters[currentTargetIndex], targetActor, onHitKnockbackField, GetTargetData()[currentTargetIndex], abilityContext);
				//	}
				//}
			}
			if (keyValuePair.Key.GetTeam() == ActorData.GetTeam())
			{
				numAllyHits++;
			}
			else
			{
				numEnemyHits++;
			}
		}
		abilityContext.SetValue(ContextKeys.s_NumEnemyHits.GetKey(), numEnemyHits);
		abilityContext.SetValue(ContextKeys.s_NumAllyHits.GetKey(), numAllyHits);
#endif
	}

	public virtual void PostProcessTargetingNumbers(
		ActorData targetActor,
		int currentTargeterIndex,
		Dictionary<ActorData, ActorHitContext> actorHitContext,
		ContextVars abilityContext,
		ActorData caster,
		TargetingNumberUpdateScratch results)
	{
	}

	internal override ActorData.MovementType GetMovementType()
	{
		if (GetTargetSelectComp() != null)
		{
			return GetTargetSelectComp().GetMovementType();
		}
		return base.GetMovementType();
	}

	// added in rogues
#if SERVER
	public override ServerEvadeUtils.ChargeSegment[] GetChargePath(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		if (GetTargetSelectComp() != null)
		{
			ServerEvadeUtils.ChargeSegment[] chargePath = GetTargetSelectComp().GetChargePath(targets, caster, additionalData);
			if (chargePath != null)
			{
				float movementSpeed = CalcMovementSpeed(GetEvadeDistance(chargePath));
				GetTargetSelectComp().ApplyMovementSpeed(chargePath, movementSpeed);
				return chargePath;
			}
		}
		return base.GetChargePath(targets, caster, additionalData);
	}
#endif

	// added in rogues
#if SERVER
	public override BoardSquare GetValidChargeTestSourceSquare(ServerEvadeUtils.ChargeSegment[] chargeSegments)
	{
		GenericAbility_TargetSelectBase targetSelectComp = GetTargetSelectComp();
		if (targetSelectComp != null)
		{
			return targetSelectComp.GetValidChargeTestSourceSquare(chargeSegments);
		}
		return base.GetValidChargeTestSourceSquare(chargeSegments);
	}
#endif

	// added in rogues
#if SERVER
	public override Vector3 GetChargeBestSquareTestVector(ServerEvadeUtils.ChargeSegment[] chargeSegments)
	{
		GenericAbility_TargetSelectBase targetSelectComp = GetTargetSelectComp();
		if (targetSelectComp != null)
		{
			return targetSelectComp.GetChargeBestSquareTestVector(chargeSegments);
		}
		return base.GetChargeBestSquareTestVector(chargeSegments);
	}
#endif

	// added in rogues
#if SERVER
	public override bool GetChargeThroughInvalidSquares()
	{
		GenericAbility_TargetSelectBase targetSelectComp = GetTargetSelectComp();
		return targetSelectComp != null && targetSelectComp.GetChargeThroughInvalidSquares();
	}
#endif

	public static void CalcIntFieldValues(
		ActorData targetActor,
		ActorData caster,
		ActorHitContext actorContext,
		ContextVars abilityContext,
		List<OnHitIntField> intFields,
		NumericHitResultScratch result)
	{
		result.Reset();
		foreach (OnHitIntField current in intFields)
		{
			if (TargetFilterHelper.ActorMeetsConditions(current.m_conditions, targetActor, caster, actorContext, abilityContext))
			{
				// reactor
				int num = current.CalcValue(actorContext, abilityContext);
				if (current.m_hitType == OnHitIntField.HitType.Damage && result.m_damage == 0)
				{
					result.m_damage = num;
				}
				else if (current.m_hitType == OnHitIntField.HitType.Healing && result.m_healing == 0)
				{
					result.m_healing = num;
				}
				else if (current.m_hitType == OnHitIntField.HitType.EnergyChange)
				{
					if (num > 0)
					{
						if (result.m_energyGain == 0)
						{
							result.m_energyGain = num;
						}
					}
					else if (num < 0 && result.m_energyLoss == 0)
					{
						result.m_energyLoss = -1 * num;
					}
				}
				// rogues
				//current.CalcValues(actorContext, abilityContext, out float num, out float num2);
				//if (current.m_hitType == OnHitIntField.HitType.Damage)
				//{
				//	result.m_damageMin += num;
				//	result.m_damageMax += num2;
				//}
				//else if (current.m_hitType == OnHitIntField.HitType.Healing)
				//{
				//	result.m_healingMin += num;
				//	result.m_healingMax += num2;
				//}
				//else if (current.m_hitType == OnHitIntField.HitType.EnergyGain)
				//{
				//	result.m_energyGainMin += num;
				//	result.m_energyGainMax += num2;
				//}
			}
		}
	}

	protected static int CalcAbsorbFromEffectFields( // public in rogues
		ActorData targetActor,
		ActorData caster,
		ActorHitContext actorContext,
		ContextVars abilityContext,
		List<OnHitEffecField> effectFields)
	{
		int absorb = 0;
		foreach (OnHitEffecField effectField in effectFields)
		{
			if (TargetFilterHelper.ActorMeetsConditions(effectField.m_conditions, targetActor, caster, actorContext, abilityContext)
				&& effectField.m_effect.m_applyEffect
				&& effectField.m_effect.m_effectData.m_absorbAmount > 0)
			{
				absorb += effectField.m_effect.m_effectData.m_absorbAmount;
			}
		}
		return absorb;
	}

	// added in rogues
#if SERVER
	protected virtual void PreProcessForCalcAbilityHits(
		List<AbilityTarget> targets,
		ActorData caster,
		Dictionary<ActorData, ActorHitContext> actorHitContextMap,
		ContextVars abilityContext)
	{
		int numEnemyHits = 0;
		int numAllyHit = 0;
		foreach (KeyValuePair<ActorData, ActorHitContext> keyValuePair in actorHitContextMap)
		{
			float hitPointPercent = keyValuePair.Key.GetHitPointPercent();
			float value = 1f - hitPointPercent;
			keyValuePair.Value.m_contextVars.SetValue(ContextKeys.s_PercentHealthLost.GetKey(), value);
			keyValuePair.Value.m_contextVars.SetValue(ContextKeys.s_TargetHealthPercentage.GetKey(), hitPointPercent);
			if (keyValuePair.Key.GetTeam() == caster.GetTeam())
			{
				numAllyHit++;
			}
			else
			{
				numEnemyHits++;
			}
		}
		abilityContext.SetValue(ContextKeys.s_NumEnemyHits.GetKey(), numEnemyHits);
		abilityContext.SetValue(ContextKeys.s_NumAllyHits.GetKey(), numAllyHit);
	}

	// added in rogues
	protected virtual void ProcessGatheredHits(
		List<AbilityTarget> targets,
		ActorData caster,
		AbilityResults abilityResults,
		List<ActorHitResults> actorHitResults,
		List<PositionHitResults> positionHitResults,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		// rogues - no effect template fields in reactor
		// if (actorHitResults.Any())
		// {
		//     foreach (OnHitEffectTemplateField onHitEffectTemplateField in m_onHitData.m_effectTemplateFields)
		//     {
		//         EffectTemplate effectTemplate = onHitEffectTemplateField.m_effectTemplate;
		//         IEnumerable<KeyValuePair<ActorData, ActorHitResults>> actorToHitResults = abilityResults.m_actorToHitResults;
		//         KeyValuePair<ActorData, ActorHitResults> keyValuePair = actorToHitResults.FirstOrDefault(kvp => kvp.Value.m_hitParameters.Caster == caster);
		//         ActorData target = keyValuePair.Key ?? caster;
		//         if (keyValuePair.Value == null)
		//         {
		//             caster.CurrentBoardSquare.ToVector3();
		//         }
		//         else
		//         {
		//             Vector3 origin = keyValuePair.Value.m_hitParameters.Origin;
		//         }
		//         BoardSquare targetSquare = (keyValuePair.Value != null) ? keyValuePair.Value.m_hitParameters.Target.CurrentBoardSquare : caster.CurrentBoardSquare;
		//         EffectSystem.Effect effect = new EffectSystem.Effect(effectTemplate, effectTemplate, new EffectSource(this, abilityResults), targetSquare, target, caster);
		//         effect.targets = (from actorHitResult in actorHitResults
		//                           select actorHitResult.m_hitParameters.Target).ToList();
		//         ActorHitResults actorHitResults2 = actorHitResults.FirstOrDefault(actorHitResult => actorHitResult.m_hitParameters.Target == caster);
		//         if (actorHitResults2 == null)
		//         {
		//             actorHitResults2 = new ActorHitResults(new ActorHitParameters(caster, caster.CurrentBoardSquare.ToVector3()));
		//             actorHitResults.Add(actorHitResults2);
		//         }
		//         effect.Resolve();
		//         actorHitResults2.AddEffect(effect);
		//     }
		// }
	}

	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		if (GetTargetSelectComp() != null)
		{
			return GetTargetSelectComp().CreateSequenceStartData(targets, caster, additionalData);
		}
		return new List<ServerClientUtils.SequenceStartData>();
	}

	// added in rogues
	public override List<ActorData> GetHitActors(
		List<AbilityTarget> targets,
		ActorData caster,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<ActorData> hitActors = base.GetHitActors(targets, caster, nonActorTargetInfo);
		GenericAbility_TargetSelectBase targetSelectComp = GetTargetSelectComp();
		targetSelectComp.ResetContextData();
		targetSelectComp.CalcHitTargets(targets, caster, nonActorTargetInfo);
		foreach (ActorData hitActor in targetSelectComp.m_contextCalcData.m_actorToHitContext.Keys)
		{
			hitActors.Add(hitActor);
		}
		return hitActors;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		GenericAbility_TargetSelectBase targetSelectComp = GetTargetSelectComp();
		if (targetSelectComp == null)
		{
			Log.Error(GetType() + " has no target select handler or on-hit interpreter");
			return;
		}
		targetSelectComp.ResetContextData();
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		targetSelectComp.CalcHitTargets(targets, caster, nonActorTargetInfo);
		PreProcessForCalcAbilityHits(
			targets,
			caster,
			targetSelectComp.GetActorHitContextMap(),
			targetSelectComp.GetNonActorSpecificContext());
		CalcAbilityHits(
			targetSelectComp.GetActorHitContextMap(),
			targetSelectComp.GetNonActorSpecificContext(),
			caster,
			GetOnHitAuthoredData(),
			this,
			abilityResults.SequenceSource,
			out List<ActorHitResults> actorHitResults,
			out List<PositionHitResults> positionHitResults);

		ProcessGatheredHits(targets, caster, abilityResults, actorHitResults, positionHitResults, nonActorTargetInfo);

		foreach (ActorHitResults hitResults in actorHitResults)
		{
			abilityResults.StoreActorHit(hitResults);
		}
		foreach (PositionHitResults hitResults2 in positionHitResults)
		{
			abilityResults.StorePositionHit(hitResults2);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// rogues
	//public override void GetAbilityStatusData(out Dictionary<string, string> statusData, bool includeNames = false)
	//{
	//	base.GetAbilityStatusData(out statusData, includeNames);
	//	base.GatherVisibleStatusTooltips(ref statusData, m_cachedOnHitData.m_effectTemplateFields, includeNames);
	//	base.GatherVisibleStatusTooltips(ref statusData, m_cachedOnHitData.m_allyHitEffectTemplateFields, includeNames);
	//	base.GatherVisibleStatusTooltips(ref statusData, m_cachedOnHitData.m_enemyHitEffectTemplateFields, includeNames);
	//}

	// added in rogues
	public static ActorHitResults GetResultsForActorInList(ActorData actor, List<ActorHitResults> actorHitResults)
	{
		if (actorHitResults != null)
		{
			for (int i = 0; i < actorHitResults.Count; i++)
			{
				ActorHitResults actorHitResults2 = actorHitResults[i];
				if (actorHitResults2.m_hitParameters.Target == actor)
				{
					return actorHitResults2;
				}
			}
		}
		return null;
	}

	// added in rogues
	public static int GetNumEnemyResults(ActorData caster, List<ActorHitResults> actorHitResults)
	{
		int num = 0;
		if (actorHitResults != null)
		{
			for (int i = 0; i < actorHitResults.Count; i++)
			{
				if (actorHitResults[i].m_hitParameters.Target.GetTeam() != caster.GetTeam())
				{
					num++;
				}
			}
		}
		return num;
	}

	// added in rogues
	public static StandardActorEffect CreateShieldEffect(Ability ability, ActorData caster, int totalShielding, int shieldDuration)
	{
		StandardActorEffectData standardActorEffectData = new StandardActorEffectData();
		standardActorEffectData.InitWithDefaultValues();
		// bool beginsAfterBlastPhase = ability.GetRunPriority() >= AbilityPriority.Combat_Damage;
		int duration = Mathf.Max(1, shieldDuration);
		// if (beginsAfterBlastPhase)
		// {
		// 	duration++;
		// }
		standardActorEffectData.m_duration = duration;
		// if (!beginsAfterBlastPhase)
		// {
			standardActorEffectData.m_absorbAmount = totalShielding;
		// }
		StandardActorEffect standardActorEffect = new StandardActorEffect(
			ability.AsEffectSource(),
			caster.GetCurrentBoardSquare(),
			caster,
			caster,
			standardActorEffectData);
		// if (beginsAfterBlastPhase)
		// {
		// 	standardActorEffect.SetNextTurnAbsorbOverride(totalShielding);
		// }
		return standardActorEffect;
	}

	// added in rogues
	public static void CalcAbilityHits(
		Dictionary<ActorData, ActorHitContext> actorHitContextMap,
		ContextVars abilityContext,
		ActorData caster,
		OnHitAuthoredData onHitAuthoredData,
		Ability ability,
		SequenceSource seqSource,
		out List<ActorHitResults> actorHits,
		out List<PositionHitResults> posHits)
	{
		actorHits = new List<ActorHitResults>();
		posHits = new List<PositionHitResults>();
		NumericHitResultScratch numericHitResultScratch = new NumericHitResultScratch();
		if (onHitAuthoredData.m_barrierSpawnFields != null && onHitAuthoredData.m_barrierSpawnFields.Count > 0)
		{
			List<Barrier> list = new List<Barrier>();
			Vector3 pos = caster.GetFreePos();
			if (abilityContext.HasVarVec3(ContextKeys.s_CenterPos.GetKey()))
			{
				pos = abilityContext.GetValueVec3(ContextKeys.s_CenterPos.GetKey());
			}
			PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters(pos));
			foreach (OnHitBarrierField onHitBarrierField in onHitAuthoredData.m_barrierSpawnFields)
			{
				if (string.IsNullOrEmpty(onHitBarrierField.m_centerPosContextName)
				    || string.IsNullOrEmpty(onHitBarrierField.m_facingDirContextName))
				{
					continue;
				}
				int centerPosContextKey = onHitBarrierField.GetCenterPosContextKey();
				int facingDirContextKey = onHitBarrierField.GetFacingDirContextKey();
				if (!abilityContext.HasVarVec3(centerPosContextKey)
				    || !abilityContext.HasVarVec3(facingDirContextKey))
				{
					continue;
				}
				Vector3 center = abilityContext.GetValueVec3(centerPosContextKey);
				Vector3 facingDir = abilityContext.GetValueVec3(facingDirContextKey);
				if (!string.IsNullOrEmpty(onHitBarrierField.m_barrierWidthContextName))
				{
					int barrierWidthContextKey = onHitBarrierField.GetBarrierWidthContextKey();
					if (abilityContext.HasVarFloat(barrierWidthContextKey))
					{
						onHitBarrierField.m_barrierData.m_width = abilityContext.GetValueFloat(barrierWidthContextKey);
					}
				}
				Barrier barrier = new Barrier(
					"",
					center,
					facingDir,
					caster,
					onHitBarrierField.m_barrierData,
					!onHitBarrierField.m_barrierData.m_barrierSequencePrefabs.IsNullOrEmpty(),
					seqSource);
				barrier.SetSourceAbility(ability);
				list.Add(barrier);
				positionHitResults.AddBarrier(barrier);
			}
			if (list.Count > 0)
			{
				LinkedBarrierData linkData = new LinkedBarrierData();
				BarrierManager.Get().LinkBarriers(list, linkData);
			}
			posHits.Add(positionHitResults);
		}
		
		// rogues - no ground effect fields in reactor
		//List<StandardGroundEffect> list2 = new List<StandardGroundEffect>(onHitAuthoredData.m_groundEffectFields.Count);
		//if (onHitAuthoredData.m_groundEffectFields != null && onHitAuthoredData.m_groundEffectFields.Count > 0)
		//{
		//	for (int j = 0; j < onHitAuthoredData.m_groundEffectFields.Count; j++)
		//	{
		//		OnHitGroundEffectField onHitGroundEffectField = onHitAuthoredData.m_groundEffectFields[j];
		//		if (!string.IsNullOrEmpty(onHitGroundEffectField.m_centerPosContextName))
		//		{
		//			if (TargetFilterHelper.PassContextCompareFilters(onHitGroundEffectField.m_compareConditions, null, abilityContext))
		//			{
		//				int centerPosContextKey2 = onHitGroundEffectField.GetCenterPosContextKey(false);
		//				if (abilityContext.HasVarVec3(centerPosContextKey2))
		//				{
		//					Vector3 valueVec3 = abilityContext.GetValueVec3(centerPosContextKey2);
		//					PositionHitResults positionHitResults2 = new PositionHitResults(new PositionHitParameters(valueVec3));
		//					StandardGroundEffect standardGroundEffect = new StandardGroundEffect(ability.AsEffectSource(), Board.Get().GetSquareFromVec3(valueVec3), valueVec3, null, caster, onHitGroundEffectField.m_groundEffect);
		//					positionHitResults2.AddEffect(standardGroundEffect);
		//					list2.Add(standardGroundEffect);
		//					posHits.Add(positionHitResults2);
		//				}
		//				else
		//				{
		//					Debug.LogError("no centerPos key " + onHitGroundEffectField.m_centerPosContextName);
		//				}
		//			}
		//		}
		//		else
		//		{
		//			Debug.LogError("no centerPosContextName");
		//		}
		//	}
		//	foreach (StandardGroundEffect standardGroundEffect2 in list2)
		//	{
		//		standardGroundEffect2.SetLinkedGroundEffects(list2);
		//	}
		//}
		
		foreach (ActorData actorData in actorHitContextMap.Keys)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, actorHitContextMap[actorData].m_hitOrigin));
			bool ignoreMinCoverDist = actorHitContextMap[actorData].m_ignoreMinCoverDist;
			if (ignoreMinCoverDist)
			{
				actorHitResults.SetIgnoreCoverMinDist(ignoreMinCoverDist);
			}
			ActorHitContext actorContext = actorHitContextMap[actorData];
			if (actorData.GetTeam() == caster.GetTeam())
			{
				CalcIntFieldValues(actorData, caster, actorContext, abilityContext, onHitAuthoredData.m_allyHitIntFields, numericHitResultScratch);
				SetNumericFieldsOnHitResults(actorHitResults, numericHitResultScratch);
				// rogues
				// SetKnockbackFieldsOnHitResults(actorData, caster, actorContext, abilityContext, actorHitResults, onHitAuthoredData.m_allyHitKnockbackFields);
				// SetCooldownReductionFieldsOnHitResults(actorData, caster, actorContext, abilityContext, actorHitResults, onHitAuthoredData.m_allyHitCooldownReductionFields, actorHitContextMap.Count);
				SetEffectFieldsOnHitResults(actorData, caster, actorContext, abilityContext, actorHitResults, onHitAuthoredData.m_allyHitEffectFields);
				// rogues
				// SetEffectTemplateFieldsOnHitResults(actorData, caster, actorContext, abilityContext, actorHitResults, onHitAuthoredData.m_allyHitEffectTemplateFields);
			}
			else
			{
				CalcIntFieldValues(actorData, caster, actorContext, abilityContext, onHitAuthoredData.m_enemyHitIntFields, numericHitResultScratch);
				SetNumericFieldsOnHitResults(actorHitResults, numericHitResultScratch);
				// rogues
				// SetKnockbackFieldsOnHitResults(actorData, caster, actorContext, abilityContext, actorHitResults, onHitAuthoredData.m_enemyHitKnockbackFields);
				// SetCooldownReductionFieldsOnHitResults(actorData, caster, actorContext, abilityContext, actorHitResults, onHitAuthoredData.m_enemyHitCooldownReductionFields, actorHitContextMap.Count);
				SetEffectFieldsOnHitResults(actorData, caster, actorContext, abilityContext, actorHitResults, onHitAuthoredData.m_enemyHitEffectFields);

				// rogues
				// SetEffectTemplateFieldsOnHitResults(actorData, caster, actorContext, abilityContext, actorHitResults, onHitAuthoredData.m_enemyHitEffectTemplateFields);
			}
			//foreach (StandardGroundEffect standardGroundEffect3 in list2)
			//{
			//	standardGroundEffect3.SetupActorHitResults(ref actorHitResults, actorData.CurrentBoardSquare);
			//}
			actorHits.Add(actorHitResults);
		}
	}
	
	// custom TODO LOW merge with CalcAbilityHits?
	public static void ApplyActorHitData(
		ActorData caster,
		ActorData targetActor,
		ActorHitResults actorHitResults,
		OnHitAuthoredData hitData,
		ActorHitContext actorContext = null,
		ContextVars abilityContext = null)
	{
		NumericHitResultScratch numericHitResultScratch = new NumericHitResultScratch();
		if (actorContext == null)
		{
			actorContext = new ActorHitContext();
		}
		if (abilityContext == null)
		{
			abilityContext = new ContextVars();
		}
		bool isEnemy = caster.GetTeam() != targetActor.GetTeam();
        
		CalcIntFieldValues(
			targetActor,
			caster,
			actorContext,
			abilityContext,
			isEnemy ? hitData.m_enemyHitIntFields : hitData.m_allyHitIntFields,
			numericHitResultScratch);
		SetNumericFieldsOnHitResults(actorHitResults, numericHitResultScratch);
		SetEffectFieldsOnHitResults(
			targetActor,
			caster,
			actorContext,
			abilityContext,
			actorHitResults,
			isEnemy ? hitData.m_enemyHitEffectFields : hitData.m_allyHitEffectFields);
	}
	
	// custom
	public static ActorHitResults GetOrAddHitResults(ActorData actor, List<ActorHitResults> actorHitResults)
	{
		ActorHitResults casterHitResults = actorHitResults
			.FirstOrDefault(ahr => ahr.m_hitParameters.Target == actor);
		if (casterHitResults == null)
		{
			casterHitResults = new ActorHitResults(new ActorHitParameters(actor, actor.GetLoSCheckPos()));
			actorHitResults.Add(casterHitResults);
		}

		return casterHitResults;
	}

	// custom
	public static void SetNumericFieldsOnHitResults(ActorHitResults hitRes, NumericHitResultScratch calcScratch)
	{
		if (calcScratch.m_damage > 0)
		{
			hitRes.AddBaseDamage(calcScratch.m_damage);
		}
		if (calcScratch.m_healing > 0)
		{
			hitRes.AddBaseHealing(calcScratch.m_healing);
		}
		if (calcScratch.m_energyGain > 0)
		{
			hitRes.AddTechPointGain(calcScratch.m_energyGain);
		}
		if (calcScratch.m_energyLoss > 0)
		{
			hitRes.AddTechPointLoss(calcScratch.m_energyLoss);
		}
	}
	// rogues
	//public static void SetNumericFieldsOnHitResults(ActorHitResults hitRes, NumericHitResultScratch calcScratch)
	//{
	//	if (calcScratch.m_damageMin > 0f)
	//	{
	//		hitRes.ModifyDamageCoeff(calcScratch.m_damageMin, calcScratch.m_damageMax);
	//	}
	//	if (calcScratch.m_healingMin > 0f)
	//	{
	//		hitRes.ModifyHealingCoeff(calcScratch.m_healingMin, calcScratch.m_healingMax);
	//	}
	//	if (calcScratch.m_energyGainMin > 0f)
	//	{
	//		hitRes.ModifyTechPointGainCoeff(calcScratch.m_energyGainMin, calcScratch.m_energyGainMax);
	//	}
	//}

	// rogues - no knockback fields in reactor
	//public static void SetKnockbackFieldsOnHitResults(ActorData targetActor, ActorData caster, ActorHitContext actorContext, ContextVars abilityContext, ActorHitResults hitRes, List<OnHitKnockbackField> knockbackFields)
	//{
	//	Vector3 vector = actorContext.m_hitOrigin;
	//	if (abilityContext.HasVarVec3(ContextKeys.s_TargetHitPos.GetKey()))
	//	{
	//		vector = abilityContext.GetValueVec3(ContextKeys.s_TargetHitPos.GetKey());
	//	}
	//	if (abilityContext.HasVarVec3(ContextKeys.s_KnockbackOrigin.GetKey()))
	//	{
	//		vector = abilityContext.GetValueVec3(ContextKeys.s_KnockbackOrigin.GetKey());
	//	}
	//	foreach (OnHitKnockbackField onHitKnockbackField in knockbackFields)
	//	{
	//		if (TargetFilterHelper.ActorMeetsConditions(onHitKnockbackField.m_conditions, targetActor, caster, actorContext, abilityContext))
	//		{
	//			hitRes.AddKnockbackData(new KnockbackHitData(targetActor, caster, onHitKnockbackField.m_knockbackType, targetActor.GetLoSCheckPos() - vector, vector, onHitKnockbackField.m_distance));
	//		}
	//	}
	//}

	// rogues - no cdr fields in reactor
	//public static void SetCooldownReductionFieldsOnHitResults(ActorData targetActor, ActorData caster, ActorHitContext actorContext, ContextVars abilityContext, ActorHitResults hitRes, List<OnHitCooldownReductionField> cdrFields, int numHits)
	//{
	//	foreach (OnHitCooldownReductionField onHitCooldownReductionField in cdrFields)
	//	{
	//		if (TargetFilterHelper.ActorMeetsConditions(onHitCooldownReductionField.m_conditions, targetActor, caster, actorContext, abilityContext))
	//		{
	//			if (caster.GetTeam() == targetActor.GetTeam())
	//			{
	//				onHitCooldownReductionField.m_cooldownReduction.AppendCooldownMiscEvents(hitRes, false, numHits, 0);
	//			}
	//			else
	//			{
	//				onHitCooldownReductionField.m_cooldownReduction.AppendCooldownMiscEvents(hitRes, false, 0, numHits);
	//			}
	//		}
	//	}
	//}

	// added in rogues
	public static void SetEffectFieldsOnHitResults(
		ActorData targetActor,
		ActorData caster,
		ActorHitContext actorContext,
		ContextVars abilityContext,
		ActorHitResults hitRes,
		List<OnHitEffecField> effectFields)
	{
		foreach (OnHitEffecField onHitEffecField in effectFields)
		{
			if (TargetFilterHelper.ActorMeetsConditions(onHitEffecField.m_conditions, targetActor, caster, actorContext, abilityContext))
			{
				hitRes.AddStandardEffectInfo(onHitEffecField.m_effect);
				if (onHitEffecField.m_skipRemainingEffectEntriesIfMatch)
				{
					break;
				}
			}
		}
	}

	// rogues - no effect template fields in reactor
	// public static void SetEffectTemplateFieldsOnHitResults(
	// 	ActorData targetActor,
	// 	ActorData caster,
	// 	ActorHitContext actorContext,
	// 	ContextVars abilityContext,
	// 	ActorHitResults hitRes,
	// 	List<OnHitEffectTemplateField> effectTemplateFields)
	// {
	// 	foreach (OnHitEffectTemplateField onHitEffectTemplateField in effectTemplateFields)
	// 	{
	// 		if (TargetFilterHelper.ActorMeetsConditions(onHitEffectTemplateField.m_conditions, targetActor, caster, actorContext, abilityContext))
	// 		{
	// 			hitRes.AddEffectTemplate(onHitEffectTemplateField.m_effectTemplate);
	// 			if (onHitEffectTemplateField.m_skipRemainingEffectEntriesIfMatch)
	// 			{
	// 				break;
	// 			}
	// 		}
	// 	}
	// }

	// added in rogues
	public override BoardSquare GetIdealDestination(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		GenericAbility_TargetSelectBase targetSelectComp = GetTargetSelectComp();
		if (targetSelectComp != null)
		{
			BoardSquare idealDestination = targetSelectComp.GetIdealDestination(targets, caster, additionalData);
			if (idealDestination != null)
			{
				return idealDestination;
			}
		}
		return base.GetIdealDestination(targets, caster, additionalData);
	}

	// added in rogues
	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		GenericAbility_TargetSelectBase targetSelectComp = GetTargetSelectComp();
		if (targetSelectComp != null)
		{
			return targetSelectComp.CalcPointsOfInterestForCamera(targets, caster);
		}
		return base.CalcPointsOfInterestForCamera(targets, caster);
	}
#endif
}
