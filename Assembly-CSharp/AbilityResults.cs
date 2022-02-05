// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// was empty in reactor
public class AbilityResults
{
#if SERVER
	private ActorData m_caster;
	private Ability m_ability;
	private SequenceSource m_sequenceSource;
	private bool m_isReal;
	private bool m_forBotScoring;
	private int m_cinematicRequested;
	private bool m_gatheredResults;
	[System.NonSerialized]
	public Dictionary<ActorData, ActorHitResults> m_actorToHitResults;
	[System.NonSerialized]
	public Dictionary<Vector3, PositionHitResults> m_positionToHitResults;
	[System.NonSerialized]
	public List<NonActorTargetInfo> m_nonActorTargetInfo;
	private List<ServerClientUtils.SequenceStartData> m_abilityRunSequenceDataList;
	private Dictionary<ActorData, int> m_damageResults;
	private Dictionary<ActorData, int> m_damageResults_gross;

	public static string s_storeActorHitHeader = "<color=orange>\t Server: Storing ActorHitResult: </color>";
	public static string s_storePositionHitHeader = "<color=orange>\t Server: Storing PositionHitResult: </color>";
	public static string s_executeActorHitHeader = "<color=yellow>\t Server: Executing ActorHitResult: </color>";
	public static string s_executePositionHitHeader = "<color=yellow>\t Server: Executing PositionHitResults: </color>";

	public AbilityResults(ActorData caster, Ability ability, SequenceSource sequenceSource, bool isReal, bool forBotScoring = false)
	{
		m_caster = caster;
		m_ability = ability;
		m_sequenceSource = sequenceSource;
		m_isReal = isReal;
		m_forBotScoring = forBotScoring;
		m_cinematicRequested = -1;
		m_gatheredResults = false;
		m_actorToHitResults = new Dictionary<ActorData, ActorHitResults>();
		m_positionToHitResults = new Dictionary<Vector3, PositionHitResults>();
		m_nonActorTargetInfo = new List<NonActorTargetInfo>();
		m_damageResults = new Dictionary<ActorData, int>();
		m_damageResults_gross = new Dictionary<ActorData, int>();
		m_abilityRunSequenceDataList = null;
	}

	public Ability Ability
	{
		get
		{
			return m_ability;
		}
	}

	public ActorData Caster
	{
		get
		{
			return m_caster;
		}
	}

	public bool IsReal
	{
		get
		{
			return m_isReal;
		}
	}

	public int CinematicRequested
	{
		get
		{
			return m_cinematicRequested;
		}
		set
		{
			m_cinematicRequested = value;
		}
	}

	public SequenceSource SequenceSource
	{
		get
		{
			return m_sequenceSource;
		}
		private set
		{
			Debug.LogWarning("AbilityResults for ability " + m_ability.name + " setting sequence source, but that should be set at construction and be read-only.");
			m_sequenceSource = value;
		}
	}

	public bool GatheredResults
	{
		get
		{
			return m_gatheredResults;
		}
		set
		{
			m_gatheredResults = value;
		}
	}

	public void StoreAbilityRunSequenceStartData(List<ServerClientUtils.SequenceStartData> seqDataList)
	{
		m_abilityRunSequenceDataList = seqDataList;
	}

	public List<ServerClientUtils.SequenceStartData> AbilityRunSequenceDataList
	{
		get
		{
			return m_abilityRunSequenceDataList;
		}
		private set
		{
		}
	}

	public Dictionary<ActorData, int> DamageResults
	{
		get
		{
			return m_damageResults;
		}
		private set
		{
			m_damageResults = value;
		}
	}

	public Dictionary<ActorData, int> DamageResults_Gross
	{
		get
		{
			return m_damageResults_gross;
		}
		private set
		{
			m_damageResults_gross = value;
		}
	}

	// heavily reworked in rogues
	// TODO recreate reactor version
	public void StoreActorHit(ActorHitResults hitResults)
	{
		//Debug.LogError("AbilityResults::StoreActorHit is not implemented!");


        if (hitResults.m_hitParameters.Target.IgnoreForAbilityHits)
        {
            if (Application.isEditor)
            {
                Debug.LogWarning("Adding actor hit with an actor that's marked not relevant for ability hit");
            }
            return;
        }
        hitResults.m_hitParameters.Caster = m_caster;
        hitResults.m_hitParameters.AbilityResults = this;
        hitResults.m_hitParameters.DamageSource = new DamageSource(m_ability, hitResults.m_hitParameters.Origin);
        ActorData target = hitResults.m_hitParameters.Target;
        float num = 1f;
        //HitChanceBracket.HitType hitType = HitChanceBracket.HitType.Normal;
        bool flag = false;
        Ability relevantAbility = hitResults.m_hitParameters.GetRelevantAbility();
        int abilityIndex = (int)((relevantAbility != null) ? relevantAbility.CachedActionType : AbilityData.ActionType.INVALID_ACTION);
        if (m_forBotScoring)
        {
            flag = true;
        }
        //else if (relevantAbility != null && relevantAbility.IgnoreAccuracySystem())
        //{
        //    flag = true;
        //}
        //EquipmentStats equipmentStats = this.m_caster.GetEquipmentStats();
        //EquipmentStats equipmentStats2 = target.GetEquipmentStats();
        //hitResults.InitBaseValuesByCoeff(target, this.m_caster);
        //HitChanceBracketType hitChanceBracketType = hitResults.TargetInCover(ServerCombatManager.DamageType.Ability);
        //if (!flag && !hitResults.m_serverGuaranteeHit && this.m_caster.GetTeam() != target.GetTeam())
        //{
        //    int num2 = PveGameplayData.RollRandomPct();
        //    int num3 = this.m_caster.GetBaseStatValue(GearStatType.AccuracyAdjustment);
        //    num3 = Mathf.RoundToInt(equipmentStats.GetTotalStatValueForSlot(GearStatType.AccuracyAdjustment, (float)num3, abilityIndex, target));
        //    if (relevantAbility != null)
        //    {
        //        num3 += relevantAbility.GetAccuracyAdjust();
        //        float dist = relevantAbility.CalcDistForAccuracyAdjust(target, this.m_caster);
        //        int num4 = relevantAbility.GetProximityAccuAdjustData(this.m_caster).CalcAdjustAmount(dist, 0f);
        //        num3 += num4;
        //    }
        //    int num5 = target.GetBaseStatValue(GearStatType.DefenseAdjustment);
        //    num5 = Mathf.RoundToInt(equipmentStats2.GetTotalStatValueForSlot(GearStatType.DefenseAdjustment, (float)num5, -1, target));
        //    bool convertMissToGlance = relevantAbility != null && relevantAbility.ConvertMissToGlance();
        //    int startAdjustCrit = EquipmentStats.CalcBracketCriticalStartAdjustment(equipmentStats, equipmentStats2, abilityIndex, target);
        //    int startAdjustGlance = EquipmentStats.CalcBracketGlanceStartAdjustment(equipmentStats, equipmentStats2, abilityIndex, target);
        //    int startAdjustDodge = EquipmentStats.CalcBracketDodgeStartAdjustment(equipmentStats, equipmentStats2, abilityIndex, target);
        //    int startAdjustBlock = EquipmentStats.CalcBracketBlockStartAdjustment(equipmentStats, equipmentStats2, abilityIndex, hitChanceBracketType, target);
        //    HitChanceBracket bracketForHitPct = PveGameplayData.Get().GetBracketForHitPct(num2, num3, num5, hitChanceBracketType, convertMissToGlance, startAdjustCrit, startAdjustGlance, startAdjustDodge, startAdjustBlock);
        //    hitType = bracketForHitPct.m_hitType;
        //    num = bracketForHitPct.m_damageMult;
        //    if (hitType == HitChanceBracket.HitType.Crit)
        //    {
        //        num = equipmentStats.GetTotalStatValueForSlot(GearStatType.CritMultiplierAdjustment, num, abilityIndex, target);
        //        for (AbilityData.ActionType actionType = AbilityData.ActionType.ABILITY_0; actionType < AbilityData.ActionType.ABILITY_5; actionType++)
        //        {
        //            Ability abilityOfActionType = this.m_caster.GetAbilityData().GetAbilityOfActionType(actionType);
        //            if (abilityOfActionType != null && abilityOfActionType != relevantAbility && abilityOfActionType.m_tags.Contains(AbilityTags.ValidOnlyWhenInCombat) && this.m_caster.GetAbilityData().GetCooldownRemaining(actionType) > 0)
        //            {
        //                hitResults.AddMiscHitEvent(new MiscHitEventData_AddToCasterCooldown(actionType, -1));
        //            }
        //        }
        //    }
        //    if (!this.Caster.HasBotController)
        //    {
        //        PveLog.DebugLog(string.Concat(new object[]
        //        {
        //            "storing hit with random roll of ",
        //            num2,
        //            " | accuracy = ",
        //            num3,
        //            " | cover = ",
        //            hitChanceBracketType,
        //            " | hitType= ",
        //            hitType,
        //            " | damageMult = ",
        //            num
        //        }), null);
        //    }
        //}
        //float num6 = (float)target.GetBaseStatValue(GearStatType.IncomingDamageMultiplierAdjustment);
        //num6 = equipmentStats2.GetTotalStatValueForSlot(GearStatType.IncomingDamageMultiplierAdjustment, num6, -1, target);
        //num *= num6;
        //if (hitType != HitChanceBracket.HitType.Miss)
        //{
        //    hitResults.ProcessEffectTemplates();
        //}
        if (num > 0f)
        {
            if (hitResults.BaseDamage > 0)
            {
                hitResults.SetBaseDamage(Mathf.RoundToInt((float)hitResults.BaseDamage * num));
            }
            hitResults.CalcFinalValues(ServerCombatManager.DamageType.Ability, ServerCombatManager.HealingType.Ability, ServerCombatManager.TechPointChangeType.Ability, this.IsReal);
            //hitResults.m_hitType = hitType;
        }
        else
        {
            hitResults = new ActorHitResults(hitResults.m_hitParameters);
            //hitResults.m_hitType = hitType;
            //if (hitChanceBracketType != HitChanceBracketType.Default)
            //{
                BoardSquare currentBoardSquare = hitResults.m_hitParameters.Target.GetCurrentBoardSquare();
                float x = hitResults.m_hitParameters.DamageSource.DamageSourceLocation.x;
                float z = hitResults.m_hitParameters.DamageSource.DamageSourceLocation.z;
                float num7 = x - currentBoardSquare.worldX;
                float num8 = z - currentBoardSquare.worldY;
                ActorCover.CoverDirections coverDirections = ActorCover.CoverDirections.INVALID;
                BoardSquare boardSquare = this.TryGetAdjacentCoverSquare(currentBoardSquare.x, currentBoardSquare.y, num7, num8, out coverDirections);
                if (boardSquare == null && num7 == num8)
                {
                    boardSquare = this.TryGetAdjacentCoverSquare(currentBoardSquare.x, currentBoardSquare.y, 0f, num8, out coverDirections);
                }
                if (boardSquare == null
					//|| !DynamicMissionGeoManager.Get().HasCoverAt(boardSquare)
					)
                {
                    boardSquare = currentBoardSquare;
                    switch (coverDirections)
                    {
                        case ActorCover.CoverDirections.X_POS:
                            coverDirections = ActorCover.CoverDirections.X_NEG;
                            break;
                        case ActorCover.CoverDirections.X_NEG:
                            coverDirections = ActorCover.CoverDirections.X_POS;
                            break;
                        case ActorCover.CoverDirections.Y_POS:
                            coverDirections = ActorCover.CoverDirections.Y_NEG;
                            break;
                        case ActorCover.CoverDirections.Y_NEG:
                            coverDirections = ActorCover.CoverDirections.Y_POS;
                            break;
                    }
                }
                //if (boardSquare != null && DynamicMissionGeoManager.Get().HasCoverAt(boardSquare))
                //{
                //    ThinCover.CoverType thinCover = boardSquare.GetThinCover(coverDirections);
                //    if ((boardSquare.IsThickCover() || thinCover != ThinCover.CoverType.None) && hitResults.m_hitParameters.Ability != null)
                //    {
                //        hitResults.AddDynamicMissionGeometryDamage(boardSquare, hitResults.m_hitParameters.Ability.GetGeometryDamageAmount(true));
                //    }
                //    else
                //    {
                //        Debug.LogWarning(string.Concat(new object[]
                //        {
                //            "A miss occured at ",
                //            currentBoardSquare,
                //            " in ",
                //            hitChanceBracketType,
                //            " but no cover was found at adjacent square ",
                //            boardSquare,
                //            " in direction ",
                //            coverDirections
                //        }));
                //    }
                //}
                //else
                //{
                //    Debug.LogWarning(string.Concat(new object[]
                //    {
                //        "A miss occured at ",
                //        currentBoardSquare,
                //        " in ",
                //        hitChanceBracketType,
                //        " but no cover was found at adjacent square ",
                //        boardSquare,
                //        " in direction ",
                //        coverDirections
                //    }));
                //}
            //}
        }
        ActorData target2 = hitResults.m_hitParameters.Target;
        if (this.m_actorToHitResults.ContainsKey(target2))
        {
            Debug.LogError(string.Concat(new string[]
            {
                AbilityResults.s_storeActorHitHeader,
                "Trying to store duplicate ActorHitResult for ",
                target2.DebugNameString(),
                " | Ability = ",
                (this.m_ability != null) ? this.m_ability.m_abilityName : "NULL"
            }));
            return;
        }
        this.m_actorToHitResults.Add(target2, hitResults);
        if (this.m_damageResults.ContainsKey(target2))
        {
            Dictionary<ActorData, int> dictionary = this.m_damageResults;
            ActorData key = target2;
            dictionary[key] += hitResults.HealthDelta;
        }
        else
        {
            this.m_damageResults.Add(target2, hitResults.HealthDelta);
        }
        if (hitResults.LifestealHealingOnCaster > 0)
        {
            if (this.m_damageResults.ContainsKey(this.m_caster))
            {
                Dictionary<ActorData, int> dictionary = this.m_damageResults;
                ActorData key = this.m_caster;
                dictionary[key] += hitResults.LifestealHealingOnCaster;
            }
            else
            {
                this.m_damageResults[this.m_caster] = hitResults.LifestealHealingOnCaster;
            }
        }
        if (this.m_damageResults_gross.ContainsKey(target2))
        {
            Dictionary<ActorData, int> dictionary = this.m_damageResults_gross;
            ActorData key = target2;
            dictionary[key] += hitResults.FinalDamage;
        }
        else
        {
            this.m_damageResults_gross.Add(target2, hitResults.FinalDamage);
        }
        hitResults.AdjustDamageResultsWithReactions(ref this.m_damageResults, ref this.m_damageResults_gross);
        hitResults.AdjustDamageResultsWithPowerups(ref this.m_damageResults);
        if (AbilityResults.DebugTraceOn && GameFlowData.Get() != null && GameFlowData.Get().gameState == GameState.BothTeams_Resolve)
        {
            Debug.LogWarning(AbilityResults.s_storeActorHitHeader + hitResults.ToDebugString());
        }
    }

	private BoardSquare TryGetAdjacentCoverSquare(int x, int y, float xDiff, float yDiff, out ActorCover.CoverDirections coverDir)
	{
		BoardSquare squareFromIndex;
		if (Mathf.Abs(xDiff) >= Mathf.Abs(yDiff))
		{
			if (xDiff > 0f)
			{
				squareFromIndex = Board.Get().GetSquareFromIndex(x + 1, y);
				coverDir = ActorCover.CoverDirections.X_NEG;
			}
			else
			{
				squareFromIndex = Board.Get().GetSquareFromIndex(x - 1, y);
				coverDir = ActorCover.CoverDirections.X_POS;
			}
		}
		else if (yDiff > 0f)
		{
			squareFromIndex = Board.Get().GetSquareFromIndex(x, y + 1);
			coverDir = ActorCover.CoverDirections.Y_NEG;
		}
		else
		{
			squareFromIndex = Board.Get().GetSquareFromIndex(x, y - 1);
			coverDir = ActorCover.CoverDirections.Y_POS;
		}
		return squareFromIndex;
	}

	public bool ExecuteForActor(ActorData target)
	{
		bool result = false;
		if (m_actorToHitResults.ContainsKey(target))
		{
			if (!m_actorToHitResults[target].ExecutedResults)
			{
				m_actorToHitResults[target].ExecuteResults();
				result = true;
			}
		}
		else
		{
			Debug.LogError($"{m_caster.name} {m_caster.DisplayName}'s {m_ability.m_abilityName} trying to execute on {target.name} {target.DisplayName}, but that actor isn't in hit results.");
		}
		return result;
	}

	public void StorePositionHit(PositionHitResults hitResults)
	{
		hitResults.m_hitParameters.Caster = m_caster;
		hitResults.m_hitParameters.Ability = m_ability;
		hitResults.m_hitParameters.DamageSource = new DamageSource(m_ability, hitResults.m_hitParameters.Pos);
		Vector3 pos = hitResults.m_hitParameters.Pos;
		if (m_positionToHitResults.ContainsKey(pos))
		{
			Debug.LogError(string.Concat(new string[]
			{
				s_storePositionHitHeader,
				" Trying to store duplicate PositionHitResult for position ",
				pos.ToString(),
				" | Ability = ",
				(m_ability != null) ? m_ability.m_abilityName : "NULL"
			}));
			return;
		}
		m_positionToHitResults.Add(pos, hitResults);
		if (DebugTraceOn && GameFlowData.Get() != null && GameFlowData.Get().gameState == GameState.BothTeams_Resolve)
		{
			Debug.LogWarning(s_storePositionHitHeader + hitResults.ToDebugString());
		}
	}

	public PositionHitResults GetStoredPositionHit(Vector3 position)
	{
		if (m_positionToHitResults.ContainsKey(position))
		{
			return m_positionToHitResults[position];
		}
		return null;
	}

	public bool ExecuteForPosition(Vector3 position)
	{
		bool result = false;
		if (m_positionToHitResults.ContainsKey(position))
		{
			m_positionToHitResults[position].ExecuteResults();
			result = true;
		}
		else if (DebugTraceOn)
		{
			Debug.LogWarning("<color=yellow>OnPositionHit for a position we don't care about: " + position.ToString() + "</color>");
		}
		return result;
	}

	public void StoreNonActorTargetInfo(List<NonActorTargetInfo> nonActorTargetInfo)
	{
		if (nonActorTargetInfo != null)
		{
			m_nonActorTargetInfo.AddRange(nonActorTargetInfo);
		}
	}

	public ActorData[] HitActorsArray()
	{
		ActorData[] array = new ActorData[m_actorToHitResults.Count];
		m_actorToHitResults.Keys.CopyTo(array, 0);
		return array;
	}

	public List<ActorData> HitActorList()
	{
		List<ActorData> list = new List<ActorData>();
		foreach (ActorData item in m_actorToHitResults.Keys)
		{
			list.Add(item);
		}
		return list;
	}

	public bool HasHitOnActor(ActorData actor)
	{
		return m_actorToHitResults.ContainsKey(actor);
	}

	public bool HitsDoneExecuting()
	{
		return !GatheredResults || (ActorHitResults.HitsInCollectionDoneExecuting(m_actorToHitResults) && PositionHitResults.HitsInCollectionDoneExecuting(m_positionToHitResults));
	}

	public void ExecuteUnexecutedAbilityHits(bool asFailsafe)
	{
		if (!GatheredResults)
		{
			return;
		}
		string logHeader = string.Concat($"{m_caster.name} {m_caster.DisplayName}'s {m_ability.m_abilityName}");
		ActorHitResults.ExecuteUnexecutedActorHits(m_actorToHitResults, asFailsafe, logHeader);
		PositionHitResults.ExecuteUnexecutedPositionHits(m_positionToHitResults, asFailsafe, logHeader);
		Ability.OnExecutedAbility(this);
	}

	public void AddHitActorIds(HashSet<int> hitActorIds)
	{
		foreach (ActorData actorData in m_actorToHitResults.Keys)
		{
			hitActorIds.Add(actorData.ActorIndex);
		}
	}

	public void GenerateAbilityEvent()
	{
		if (m_caster.HasBotController)
		{
			return;
		}
		GameManager gameManager = GameManager.Get();
		if (gameManager != null && gameManager.GameConfig != null)  // GameConfig replaced with GameMission in rogues
		{
			EventLogMessage eventLogMessage = new EventLogMessage("abilities", "AbilityUsed");
			eventLogMessage.AddData("ProcessCode", (HydrogenConfig.Get() != null) ? HydrogenConfig.Get().ProcessCode : "?");
			eventLogMessage.AddData("BuildVersion", BuildVersion.FullVersionString);
			eventLogMessage.AddData("Map", gameManager.GameConfig.Map);
			//eventLogMessage.AddData("Encounter", gameManager.GameMission.Encounter);  // rogues-only
			eventLogMessage.AddData("Turn", GameFlowData.Get().CurrentTurn);
			eventLogMessage.AddData("PhaseTriggered", m_ability.RunPriority.ToString());
			eventLogMessage.AddData("Taunted", CinematicRequested > 0);
			m_caster.GenerateEventData(eventLogMessage, true);
			eventLogMessage.AddData("AbilityName", m_ability.m_abilityName);
			eventLogMessage.AddData("AbilityMod", (m_ability.CurrentAbilityMod != null) ? m_ability.CurrentAbilityMod.m_name : "None");
			eventLogMessage.Write();
		}
	}

	public Dictionary<ActorData, Vector2> GetKnockbackTargets()
	{
		Dictionary<ActorData, Vector2> dictionary = new Dictionary<ActorData, Vector2>();
		foreach (ActorHitResults actorHitResults in m_actorToHitResults.Values)
		{
			if (actorHitResults.HasKnockback)
			{
				Vector2 knockbackDeltaForType = KnockbackUtils.GetKnockbackDeltaForType(actorHitResults.KnockbackHitData);
				dictionary.Add(actorHitResults.m_hitParameters.Target, knockbackDeltaForType);
			}
		}
		return dictionary;
	}

	public void FinalizeAbilityResults()
	{
		foreach (KeyValuePair<ActorData, ActorHitResults> keyValuePair in m_actorToHitResults)
		{
			keyValuePair.Value.FinalizeHitResults(IsReal);
		}
		if (m_nonActorTargetInfo.Count > 0)
		{
			Vector3 vector = Vector3.one;
			if (m_abilityRunSequenceDataList != null && m_abilityRunSequenceDataList.Count > 0 && m_abilityRunSequenceDataList[0] != null)
			{
				vector = m_abilityRunSequenceDataList[0].GetTargetPos();
			}
			PositionHitResults positionHitResults;
			if (m_positionToHitResults.ContainsKey(vector))
			{
				positionHitResults = m_positionToHitResults[vector];
			}
			else
			{
				positionHitResults = new PositionHitResults(new PositionHitParameters(vector));
				StorePositionHit(positionHitResults);
			}
			List<Barrier> list = new List<Barrier>();
			foreach (NonActorTargetInfo nonActorTargetInfo in m_nonActorTargetInfo)
			{
				if (nonActorTargetInfo is NonActorTargetInfo_BarrierBlock)
				{
					NonActorTargetInfo_BarrierBlock nonActorTargetInfo_BarrierBlock = nonActorTargetInfo as NonActorTargetInfo_BarrierBlock;
					if (nonActorTargetInfo_BarrierBlock.m_barrier != null && (nonActorTargetInfo_BarrierBlock.m_barrier == null || !list.Contains(nonActorTargetInfo_BarrierBlock.m_barrier)))
					{
						MovementResults reactHitResults = nonActorTargetInfo_BarrierBlock.GetReactHitResults(Caster);
						if (reactHitResults != null)
						{
							positionHitResults.AddReactionOnPositionHit(reactHitResults);
							list.Add(nonActorTargetInfo_BarrierBlock.m_barrier);
							IntegrateHpDeltaForPositionReactResult(reactHitResults);
							if (DebugTraceOn)
							{
								Debug.LogWarning("<color=white>Storing Barrier Block Info at pos " + nonActorTargetInfo_BarrierBlock.m_crossPos + "</color>");
							}
						}
					}
				}
				// rogues most likely
				//else if (nonActorTargetInfo is NonActorTargetInfo_DestructibleGeoHit)
				//{
				//	NonActorTargetInfo_DestructibleGeoHit nonActorTargetInfo_DestructibleGeoHit = nonActorTargetInfo as NonActorTargetInfo_DestructibleGeoHit;
				//	if (nonActorTargetInfo_DestructibleGeoHit != null && nonActorTargetInfo_DestructibleGeoHit.m_geo != null)
				//	{
				//		positionHitResults.AddDynamicMissionGeometryDamage(nonActorTargetInfo_DestructibleGeoHit.m_wallSquare, m_ability.GetGeometryDamageAmount(false));
				//	}
				//}
			}
		}
		if (GameWideData.Get() != null && GameWideData.Get().FreeAutomaticOverconOnCatalyst_OverconId != -1 && Caster != null && Ability != null && AbilityData.IsCard(Caster.GetAbilityData().GetActionTypeOfAbility(Ability)) && m_actorToHitResults.ContainsKey(Caster))
		{
			m_actorToHitResults[Caster].AddOvercon(GameWideData.Get().FreeAutomaticOverconOnCatalyst_OverconId);
		}
	}

	public void IntegrateHpDeltaForPositionReactResult(MovementResults reactRes)
	{
		Dictionary<ActorData, int> movementDamageResults = reactRes.GetMovementDamageResults();
		if (movementDamageResults != null)
		{
			foreach (ActorData actorData in movementDamageResults.Keys)
			{
				int num = movementDamageResults[actorData];
				if (m_damageResults.ContainsKey(actorData))
				{
					Dictionary<ActorData, int> dictionary = m_damageResults;
					ActorData key = actorData;
					dictionary[key] += num;
				}
				else
				{
					m_damageResults.Add(actorData, num);
				}
			}
		}
		Dictionary<ActorData, int> movementDamageResults_Gross = reactRes.GetMovementDamageResults_Gross();
		if (movementDamageResults_Gross != null)
		{
			foreach (ActorData actorData2 in movementDamageResults_Gross.Keys)
			{
				int num2 = movementDamageResults_Gross[actorData2];
				if (m_damageResults_gross.ContainsKey(actorData2))
				{
					Dictionary<ActorData, int> dictionary = m_damageResults_gross;
					ActorData key = actorData2;
					dictionary[key] += num2;
				}
				else
				{
					m_damageResults_gross.Add(actorData2, num2);
				}
			}
		}
	}

	public bool ShouldMovementHitUpdateTargetLastKnownPos(ActorData mover)
	{
		return m_actorToHitResults.ContainsKey(mover) && m_actorToHitResults[mover].ShouldMovementHitUpdateTargetLastKnownPos();
	}

	public static bool DebugTraceOn
	{
		get
		{
			return false;
		}
	}
#endif
}
