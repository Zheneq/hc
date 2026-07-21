// SERVER
// ROGUES
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CollectTheCoins : NetworkBehaviour
{
    public enum CollectTheCoins_VictoryCondition
    {
        TeamMustHaveMostCoins,
        TeamMustNotHaveMostCoins
    }

    [Serializable]
    public class CoinAbilityMod
    {
        public float m_minCoinsForAnyBonus = -1f;
        public float m_maxCoinsForAnyBonus = -1f;
        public float m_bonusForHavingMin;
        public float m_bonusPerCoinOverMin;
        public float m_maxBonus = -1f;

        public bool BeingActiveMatters()
        {
            return m_bonusForHavingMin != 0f || m_bonusPerCoinOverMin != 0f;
        }

        public float GetBonusForCoins(int numCoins)
        {
            bool isOverMin = numCoins >= m_minCoinsForAnyBonus || m_minCoinsForAnyBonus == -1f;
            bool isUnderMax = numCoins <= m_maxCoinsForAnyBonus || m_maxCoinsForAnyBonus == -1f;

            if (!isOverMin || !isUnderMax)
            {
                return 0f;
            }

            float bonus = m_bonusForHavingMin + m_bonusPerCoinOverMin * (numCoins - m_minCoinsForAnyBonus);
            if (m_maxBonus >= 0f)
            {
                bonus = Mathf.Min(bonus, m_maxBonus);
            }

            return bonus;
        }

#if SERVER
        // added in rogues
        public float GetBonus_Server(ActorData actor)
        {
            if (!BeingActiveMatters())
            {
                return 0f;
            }

            int numCoins = 0;
            if (NetworkServer.active)
            {
                numCoins = Get().GetCoinsForActor_Server(actor);
            }

            return GetBonusForCoins(numCoins);
        }
#endif

        public float GetBonus_Client(ActorData actor)
        {
            if (!BeingActiveMatters())
            {
                return 0f;
            }

            int numCoins = 0;
            if (NetworkClient.active)
            {
                numCoins = Get().GetCoinsForActor_Client(actor);
            }

            return GetBonusForCoins(numCoins);
        }
    }

#if SERVER
    // added in rogues
    public class ServerSideData
    {
        public HashSet<BoardSquare> m_reservedForPowerupSquares = new HashSet<BoardSquare>();
        public Dictionary<ActorData, int> m_actorsToCoins = new Dictionary<ActorData, int>();
        public Dictionary<BoardSquare, int> m_squaresToCoins = new Dictionary<BoardSquare, int>();
        public Dictionary<BoardSquare, int> m_squaresToSpills = new Dictionary<BoardSquare, int>();
        public Dictionary<BoardSquare, ActorData>
            m_coinSquaresBeingMovedOver = new Dictionary<BoardSquare, ActorData>();

        public bool IsSquareReservedForPowerups(BoardSquare square)
        {
            if (m_reservedForPowerupSquares == null)
            {
                m_reservedForPowerupSquares = new HashSet<BoardSquare>();
                if (PowerUpManager.Get() != null)
                {
                    PowerUpManager.Get().CollectSquaresToAvoidForRespawn(m_reservedForPowerupSquares, null);
                }
            }

            return m_reservedForPowerupSquares.Contains(square);
        }
    }
#endif

    public class ClientSideData
    {
        public Dictionary<ActorData, int> m_actorsToCoins_unresolved = new Dictionary<ActorData, int>();
        public Dictionary<BoardSquare, int> m_squaresToCoins_unresolved = new Dictionary<BoardSquare, int>();
        public Dictionary<BoardSquare, List<GameObject>> m_squaresToSpillsVisuals = new Dictionary<BoardSquare, List<GameObject>>();
        public Dictionary<BoardSquare, List<GameObject>> m_squaresToCoinVisuals = new Dictionary<BoardSquare, List<GameObject>>();
    }

    [Header("ObjectivePoints")]
    public int m_objPointsPerCoinOnTurnStart;
    public int m_objPointAdjustWhenHasMostCoinsOnTurnStart;
    public bool m_gainObjPointPerCoinPickedUp;
    public bool m_loseObjPointPerCoinDropped;
    [Header("Game Rules")]
    public int m_numCoinsToAwardPerCoinPowerup = 1;
    public int m_numCoinsToAwardPerNonCoinPowerup;
    public bool m_canSpillCoinsOnPowerupLocations;
    public bool m_dropCoinsOnDeath = true;
    public bool m_dropCoinsOnKnockback;
    public bool m_dropCoinsOnEvade;
    public bool m_evadersCanPickUpCoins = true;
    public bool m_knockbackedMoversCanPickUpCoins;
    [Header("Sequences")]
    public GameObject m_coinPickedUpSequence;
    public GameObject m_coinsDroppingSequence;
    public GameObject m_coinsSpillingSequence;
    [Header("Persistent Objects")]
    public GameObject m_coinOnGroundPrefab;
    public GameObject m_spillInAirPrefab;
    public float m_spillVerticalOffset = 1f;
    [Header("Scoundrel Trick Shot Mods")]
    public CoinAbilityMod m_bouncingLaserDamage;
    public CoinAbilityMod m_bouncingLaserTotalDistance;
    public CoinAbilityMod m_bouncingLaserBounceDistance;
    public CoinAbilityMod m_bouncingLaserBounces;
    public CoinAbilityMod m_bouncingLaserReduceBackupPlanCooldown;
    public CoinAbilityMod m_bouncingLaserPierces;

    private uint m_sequenceSourceId;
#if SERVER
    private ServerSideData m_serverData; // added in rogues
#endif
    private ClientSideData m_clientData;
    private static CollectTheCoins s_instance;
    private SequenceSource _sequenceSource;

#if SERVER
    protected List<MovementResults> m_evadeResults = new List<MovementResults>(); // added in rogues
    protected List<MovementResults> m_knockbackResults = new List<MovementResults>(); // added in rogues
    protected List<MovementResults> m_normalMovementResults = new List<MovementResults>(); // added in rogues
#endif

    internal SequenceSource SequenceSource
    {
        get
        {
            if (_sequenceSource == null)
            {
                _sequenceSource = new SequenceSource(null, null, m_sequenceSourceId, false);
            }

            return _sequenceSource;
        }
    }

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Log.Error("Multiple CollectTheCoins components in this scene, remove extraneous ones.");
        }

        if (NetworkServer.active)
        {
            SequenceSource sequenceSource = new SequenceSource(null, null, false);
            m_sequenceSourceId = sequenceSource.RootID;
        }

#if SERVER
        // added in rogues
        m_serverData = new ServerSideData();
#endif
        m_clientData = new ClientSideData();
    }

    private void OnDestroy()
    {
        s_instance = null;
    }

    public static CollectTheCoins Get()
    {
        return s_instance;
    }

#if SERVER
    // added in rogues
    public bool HasModForAbility(Ability ability, ActorData caster)
    {
        return ability is ScoundrelBouncingLaser
               && Mathf.RoundToInt(m_bouncingLaserReduceBackupPlanCooldown.GetBonus_Server(caster)) > 0;
    }

    // added in rogues
    public AbilityModCooldownReduction CreateAbilityModCooldownReductionForAbility(Ability ability, ActorData caster)
    {
        return ability is ScoundrelBouncingLaser
            ? CreateBouncingLaserCooldownReductionForBackupPlan(caster)
            : null;
    }

    // added in rogues
    private AbilityModCooldownReduction CreateBouncingLaserCooldownReductionForBackupPlan(ActorData caster)
    {
        int cdReductionPerHit = Mathf.RoundToInt(m_bouncingLaserReduceBackupPlanCooldown.GetBonus_Server(caster));
        return new AbilityModCooldownReduction
        {
            m_onAbility = AbilityData.ActionType.ABILITY_3,
            m_modAmountType = AbilityModCooldownReduction.ModAmountType.FlatOnAnyEnemyHit,
            m_baseValue = cdReductionPerHit
        };
    }

    // added in rogues
    public void OnTurnEnd()
    {
        ResolveSpills_Server();
        m_serverData.m_coinSquaresBeingMovedOver.Clear();
        AdjustObjectivePoints_Server();
    }

    // added in rogues
    private void AdjustObjectivePoints_Server()
    {
        if (m_objPointsPerCoinOnTurnStart == 0 && m_objPointAdjustWhenHasMostCoinsOnTurnStart == 0)
        {
            return;
        }

        CalculateCoins_Server(out int coinsTeamA, out int coinsTeamB);
        if (m_objPointsPerCoinOnTurnStart != 0)
        {
            int pointsTeamA = coinsTeamA * m_objPointsPerCoinOnTurnStart;
            int pointsTeamB = coinsTeamB * m_objPointsPerCoinOnTurnStart;
            ObjectivePoints.Get().AdjustPoints(pointsTeamA, Team.TeamA);
            ObjectivePoints.Get().AdjustPoints(pointsTeamB, Team.TeamB);
        }

        if (m_objPointAdjustWhenHasMostCoinsOnTurnStart != 0)
        {
            if (coinsTeamA > coinsTeamB)
            {
                ObjectivePoints.Get().AdjustPoints(m_objPointAdjustWhenHasMostCoinsOnTurnStart, Team.TeamA);
                return;
            }

            if (coinsTeamB > coinsTeamA)
            {
                ObjectivePoints.Get().AdjustPoints(m_objPointAdjustWhenHasMostCoinsOnTurnStart, Team.TeamB);
            }
        }
    }

    // added in rogues
    public void OnActorDeath(ActorData actor)
    {
        if (m_serverData.m_actorsToCoins.ContainsKey(actor) && m_serverData.m_actorsToCoins[actor] > 0)
        {
            OnActorDroppedCoins_Server(actor, actor.GetMostRecentDeathSquare());
        }
    }

    // added in rogues
    public void OnTurnStart()
    {
        if (!NetworkServer.active)
        {
            return;
        }
        
        if (NetworkClient.active)
        {
            SynchCoinVisualsToDictionary(m_serverData.m_squaresToCoins);
            m_clientData.m_actorsToCoins_unresolved = new Dictionary<ActorData, int>(m_serverData.m_actorsToCoins);
            m_clientData.m_squaresToCoins_unresolved =
                new Dictionary<BoardSquare, int>(m_serverData.m_squaresToCoins);
        }

        SetDirtyBit(1U);
    }

    // added in rogues
    public void ResolveSpills_Server()
    {
        if (m_serverData.m_squaresToSpills.Count <= 0)
        {
            return;
        }
        
        foreach (KeyValuePair<BoardSquare, int> keyValuePair in m_serverData.m_squaresToSpills)
        {
            BoardSquare square = keyValuePair.Key;
            int numSpilled = keyValuePair.Value;
            
            float distance = 0f;
            for (int i = 0; i < numSpilled;)
            {
                List<BoardSquare> potentialSpillSquares = GetPotentialCoinSpillSquares_Server(square, distance);
                foreach (BoardSquare potentialSpillSquare in potentialSpillSquares)
                {
                    ActorData occupantActor = potentialSpillSquare.OccupantActor;
                    if (occupantActor != null)
                    {
                        OnActorGainedCoins_Server(occupantActor, 1);
                    }
                    else
                    {
                        SpawnCoinOnSquare_Server(potentialSpillSquare, 1);
                    }

                    i++;
                    if (i >= numSpilled)
                    {
                        break;
                    }
                }

                if (distance == 0f)
                {
                    distance = 1f;
                }
                else
                {
                    distance += 0.5f;
                }
            }
        }

        m_serverData.m_squaresToSpills.Clear();
    }
#endif
    
    public override bool OnSerialize(NetworkWriter writer, bool initialState)
    {
#if SERVER
        // added in rogues
        ulong dirtyBits = initialState ? ulong.MaxValue : syncVarDirtyBits;
        
        writer.Write(m_sequenceSourceId);

        writer.Write((sbyte)m_serverData.m_actorsToCoins.Count);
        writer.Write((sbyte)m_serverData.m_squaresToCoins.Count);
        
        foreach (KeyValuePair<ActorData, int> actorToCoins in m_serverData.m_actorsToCoins)
        {
            writer.Write((sbyte)actorToCoins.Key.ActorIndex);
            writer.Write((sbyte)actorToCoins.Value);
        }

        foreach (KeyValuePair<BoardSquare, int> squareToCoins in m_serverData.m_squaresToCoins)
        {
            writer.Write((sbyte)squareToCoins.Key.x);
            writer.Write((sbyte)squareToCoins.Key.y);
            writer.Write((sbyte)squareToCoins.Value);
        }

        return dirtyBits > 0UL;
#else
        return false;
#endif
    }

    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        uint sequenceSourceId = reader.ReadUInt32();
        m_sequenceSourceId = sequenceSourceId;
        
        sbyte numActors = reader.ReadSByte();
        sbyte numSquares = reader.ReadSByte();
        
        Dictionary<ActorData, int> actorsToCoins = new Dictionary<ActorData, int>();
        Dictionary<BoardSquare, int> squaresToCoins = new Dictionary<BoardSquare, int>();
        for (sbyte i = 0; i < numActors; i += 1)
        {
            sbyte actorIndex = reader.ReadSByte();
            sbyte num = reader.ReadSByte();
            ActorData actor = GameFlowData.Get().FindActorByActorIndex(actorIndex);
            actorsToCoins.Add(actor, num);
        }

        for (sbyte i = 0; i < numSquares; i += 1)
        {
            sbyte x = reader.ReadSByte();
            sbyte y = reader.ReadSByte();
            sbyte num = reader.ReadSByte();
            BoardSquare boardSquare = Board.Get().GetSquareFromIndex(x, y);
            squaresToCoins.Add(boardSquare, num);
        }

        SynchCoinVisualsToDictionary(squaresToCoins);
        m_clientData.m_actorsToCoins_unresolved = actorsToCoins;
        m_clientData.m_squaresToCoins_unresolved = squaresToCoins;
    }

#if SERVER
    // added in rogues
    private List<BoardSquare> GetPotentialCoinSpillSquares_Server(BoardSquare center, float distance)
    {
        List<BoardSquare> result = new List<BoardSquare>();
        if (center == null)
        {
            return result;
        }

        int num = Mathf.FloorToInt(distance);
        for (int i = -num; i <= num; i++)
        {
            for (int j = -num; j <= num; j++)
            {
                BoardSquare squareFromIndex = Board.Get().GetSquareFromIndex(center.x + i, center.y + j);
                if (squareFromIndex != null
                    && squareFromIndex.IsValidForGameplay()
                    && Mathf.Abs(center.HorizontalDistanceOnBoardTo(squareFromIndex) - distance) < 0.01f
                    && (m_canSpillCoinsOnPowerupLocations || !m_serverData.IsSquareReservedForPowerups(squareFromIndex))
                    && (!m_serverData.m_squaresToCoins.ContainsKey(squareFromIndex) || m_serverData.m_squaresToCoins[squareFromIndex] <= 0)
                    && center.GetLOS(squareFromIndex.x, squareFromIndex.y))
                {
                    result.Add(squareFromIndex);
                }
            }
        }

        return result;
    }

    // added in rogues
    public List<MovementResults> GetMovementResultsForMovementStage(MovementStage movementStage)
    {
        switch (movementStage)
        {
            case MovementStage.Evasion:
                return m_evadeResults;
            case MovementStage.Knockback:
                return m_knockbackResults;
            case MovementStage.Normal:
                return m_normalMovementResults;
            default:
                return null;
        }
    }

    // added in rogues
    public void ExecuteUnexecutedMovementResults_Ctc(MovementStage movementStage, bool failsafe)
    {
        switch (movementStage)
        {
            case MovementStage.Evasion:
                MovementResults.ExecuteUnexecutedHits(m_evadeResults, failsafe);
                return;
            case MovementStage.Knockback:
                MovementResults.ExecuteUnexecutedHits(m_knockbackResults, failsafe);
                return;
            case MovementStage.Normal:
                MovementResults.ExecuteUnexecutedHits(m_normalMovementResults, failsafe);
                break;
        }
    }

    // added in rogues
    public void ExecuteUnexecutedMovementResultsForDistance_Ctc(
        float distance,
        MovementStage movementStage,
        bool failsafe,
        out bool stillHasUnexecutedHits,
        out float nextUnexecutedHitDistance)
    {
        stillHasUnexecutedHits = false;
        nextUnexecutedHitDistance = -1f;
        switch (movementStage)
        {
            case MovementStage.Evasion:
                MovementResults.ExecuteUnexecutedHitsForDistance(
                    m_evadeResults,
                    distance,
                    failsafe,
                    out stillHasUnexecutedHits,
                    out nextUnexecutedHitDistance);
                return;
            case MovementStage.Knockback:
                MovementResults.ExecuteUnexecutedHitsForDistance(
                    m_knockbackResults,
                    distance,
                    failsafe,
                    out stillHasUnexecutedHits,
                    out nextUnexecutedHitDistance);
                return;
            case MovementStage.Normal:
                MovementResults.ExecuteUnexecutedHitsForDistance(
                    m_normalMovementResults,
                    distance,
                    failsafe,
                    out stillHasUnexecutedHits,
                    out nextUnexecutedHitDistance);
                break;
        }
    }

    // added in rogues
    public void ClearNormalMovementResults()
    {
        m_normalMovementResults.Clear();
    }

    // added in rogues
    public void GatherResultsInResponseToEvades(MovementCollection collection)
    {
        m_evadeResults.Clear();
        if (m_evadersCanPickUpCoins)
        {
            GatherMovementResults(collection, ref m_evadeResults);
        }

        foreach (MovementResults movementResults in m_evadeResults)
        {
            movementResults.m_triggeringPath.m_moverHasGameplayHitHere = true;
        }
    }

    // added in rogues
    public void GatherResultsInResponseToKnockbacks(MovementCollection collection)
    {
        m_knockbackResults.Clear();
        if (m_knockbackedMoversCanPickUpCoins)
        {
            GatherMovementResults(collection, ref m_knockbackResults);
        }

        foreach (MovementResults knockbackResult in m_knockbackResults)
        {
            TheatricsManager.Get().OnKnockbackMovementHitGathered(knockbackResult.GetTriggeringActor());
        }
    }

    // added in rogues
    public void GatherGrossDamageResults_Ctc_Evasion(
        ref Dictionary<ActorData, int> actorToGrossDamage_real,
        ref Dictionary<ActorData, ServerGameplayUtils.DamageDodgedStats> stats)
    {
        Dictionary<ActorData, int> fakeDamageTaken = new Dictionary<ActorData, int>();
        foreach (MovementResults movementResults in GetMovementResultsForMovementStage(MovementStage.Evasion))
        {
            Dictionary<ActorData, int> grossResults = movementResults.GetMovementDamageResults_Gross();
            ServerGameplayUtils.CalcDamageDodgedAndIntercepted(grossResults, fakeDamageTaken, ref stats);
            ServerGameplayUtils.IntegrateHpDeltas(grossResults, ref actorToGrossDamage_real);
        }
    }

    // added in rogues
    public MovementResults BuildCoinPickUpMovementResults(
        ActorData mover,
        BoardSquarePathInfo triggeringPathSegment,
        MovementStage movementStage)
    {
        return GameModeUtils.BuildGameModeEventMovementResults(
            mover,
            triggeringPathSegment,
            movementStage,
            new GameModeEvent
            {
                m_eventType = GameModeEventType.Ctc_CoinPickedUp,
                m_primaryActor = mover,
                m_square = triggeringPathSegment.square
            },
            m_coinPickedUpSequence,
            null);
    }

    // added in rogues
    public MovementResults BuildCoinDropMovementResults(
        ActorData mover,
        BoardSquarePathInfo triggeringPathSegment,
        MovementStage movementStage)
    {
        return GameModeUtils.BuildGameModeEventMovementResults(
            mover,
            triggeringPathSegment,
            movementStage,
            new GameModeEvent
            {
                m_eventType = GameModeEventType.Ctc_CoinsDropped,
                m_primaryActor = mover,
                m_square = triggeringPathSegment.square
            },
            m_coinsDroppingSequence,
            null);
    }

    // added in rogues
    private bool CanPathPickUpCoin(BoardSquarePathInfo currentlyConsideredPath, MovementStage movementStage)
    {
        if (m_serverData.m_coinSquaresBeingMovedOver.ContainsKey(currentlyConsideredPath.square))
        {
            return false;
        }

        if (currentlyConsideredPath.m_moverClashesHere)
        {
            return false;
        }

        if (currentlyConsideredPath.IsPathEndpoint() && !currentlyConsideredPath.WillDieAtEnd())
        {
            return true;
        }

        if (movementStage == MovementStage.Evasion)
        {
            return m_evadersCanPickUpCoins;
        }

        return movementStage != MovementStage.Knockback || m_knockbackedMoversCanPickUpCoins;
    }

    // added in rogues
    public virtual void GatherMovementResults(
        MovementCollection movement,
        ref List<MovementResults> movementResultsList)
    {
        foreach (KeyValuePair<BoardSquare, int> squareToCoins in m_serverData.m_squaresToCoins)
        {
            BoardSquare square = squareToCoins.Key;
            if (squareToCoins.Value <= 0 || m_serverData.m_coinSquaresBeingMovedOver.ContainsKey(square))
            {
                continue;
            }
            
            BoardSquarePathInfo triggeringPathSegment = null;
            MovementInstance bestMovement = null;
            float currentShortestMoveCost = 0f;
            
            foreach (MovementInstance movementInstance in movement.m_movementInstances)
            {
                for (BoardSquarePathInfo step = movementInstance.m_path; step != null; step = step.next)
                {
                    if (step.square != square)
                    {
                        continue;
                    }
                    
                    bool isValid = (movementInstance.m_groundBased || step.IsPathEndpoint())
                                && !step.IsPathStartPoint();
                    bool isValidForEvasion = step.IsPathEndpoint()
                                 || m_evadersCanPickUpCoins
                                 || movement.m_movementStage != MovementStage.Evasion;
                    bool isValidForKnockback = step.IsPathEndpoint()
                                 || m_knockbackedMoversCanPickUpCoins
                                 || movement.m_movementStage != MovementStage.Knockback;
                    bool isNotClash = !step.m_moverClashesHere;
                    if (isValid && isValidForEvasion && isValidForKnockback && isNotClash)
                    {
                        bool isBetter = MovementUtils.IsBetterMovementPathForGameplayThan(
                            movementInstance,
                            step.moveCost,
                            bestMovement,
                            currentShortestMoveCost);
                        
                        if (isBetter)
                        {
                            triggeringPathSegment = step;
                            bestMovement = movementInstance;
                            currentShortestMoveCost = step.moveCost;
                            break;
                        }
                    }
                }
            }

            if (bestMovement != null)
            {
                MovementResults pickUpResults = BuildCoinPickUpMovementResults(
                    bestMovement.m_mover,
                    triggeringPathSegment,
                    movement.m_movementStage);
                movementResultsList.Add(pickUpResults);
                m_serverData.m_coinSquaresBeingMovedOver.Add(square, bestMovement.m_mover);
            }
        }
    }

    // added in rogues
    public void GatherCtcResultsInResponseToMovementSegment(
        ServerGameplayUtils.MovementGameplayData gameplayData,
        MovementStage movementStage,
        ref List<MovementResults> moveResultsForSegment)
    {
        List<MovementResults> segmentResults = new List<MovementResults>();
        
        if (!gameplayData.m_currentlyConsideredPath.m_moverDiesHere)
        {
            if (CanPathPickUpCoin(gameplayData.m_currentlyConsideredPath, movementStage))
            {
                foreach (KeyValuePair<BoardSquare, int> squareToCoins in m_serverData.m_squaresToCoins)
                {
                    BoardSquare square = squareToCoins.Key;
                    if (squareToCoins.Value > 0 && square == gameplayData.m_currentlyConsideredPath.square)
                    {
                        MovementResults pickUpResults = BuildCoinPickUpMovementResults(
                            gameplayData.Actor,
                            gameplayData.m_currentlyConsideredPath,
                            movementStage);
                        segmentResults.Add(pickUpResults);
                        m_serverData.m_coinSquaresBeingMovedOver.Add(square, gameplayData.Actor);
                    }
                }
            }
        }
        else
        {
            if (m_dropCoinsOnDeath
                && m_serverData.m_actorsToCoins.ContainsKey(gameplayData.Actor)
                && m_serverData.m_actorsToCoins[gameplayData.Actor] > 0)
            {
                MovementResults dropResults = BuildCoinDropMovementResults(
                    gameplayData.Actor,
                    gameplayData.m_currentlyConsideredPath,
                    movementStage);
                segmentResults.Add(dropResults);
            }
        }
        
        List<MovementResults> movementResultsForMovementStage = GetMovementResultsForMovementStage(movementStage);
        foreach (MovementResults movementResults in segmentResults)
        {
            movementResultsForMovementStage.Add(movementResults);
            if (movementResults.ShouldMovementHitUpdateTargetLastKnownPos(gameplayData.Actor))
            {
                gameplayData.m_currentlyConsideredPath.m_visibleToEnemies = true;
                gameplayData.m_currentlyConsideredPath.m_updateLastKnownPos = true;
            }

            gameplayData.m_currentlyConsideredPath.m_moverHasGameplayHitHere = true;
            moveResultsForSegment.Add(movementResults);
        }
    }

    // added in rogues
    public int GetCoinsForActor_Server(ActorData actor)
    {
        return m_serverData.m_actorsToCoins.TryGetValue(actor, out int num)
            ? num
            : 0;
    }

    // added in rogues
    public void ExecuteServerGameModeEvent(GameModeEvent gameModeEvent)
    {
        if (gameModeEvent == null)
        {
            return;
        }

        GameModeEventType eventType = gameModeEvent.m_eventType;
        switch (eventType)
        {
            case GameModeEventType.Ctc_CoinPickedUp:
            {
                BoardSquare square = gameModeEvent.m_square;
                if (m_serverData.m_squaresToCoins.ContainsKey(square))
                {
                    int numCoins = m_serverData.m_squaresToCoins[square];
                    OnActorGainedCoins_Server(gameModeEvent.m_primaryActor, numCoins);
                    m_serverData.m_squaresToCoins[square] = 0;
                    m_serverData.m_squaresToCoins.Remove(square);
                }
                break;
            }
            case GameModeEventType.Ctc_CoinsDropped:
            {
                OnActorDroppedCoins_Server(gameModeEvent.m_primaryActor, gameModeEvent.m_square);
                break;
            }
            case GameModeEventType.Ctc_NonCoinPowerupTouched:
                OnActorGainedCoins_Server(gameModeEvent.m_primaryActor, m_numCoinsToAwardPerNonCoinPowerup);
                break;
            case GameModeEventType.Ctc_CoinPowerupTouched:
                OnActorGainedCoins_Server(gameModeEvent.m_primaryActor, m_numCoinsToAwardPerCoinPowerup);
                break;
            default:
                Debug.LogError("CollectTheCoins trying to handle non-Ctc event type " + eventType + ".");
                break;
        }
    }

    // added in rogues
    public void OnActorGainedCoins_Server(ActorData actor, int numCoins)
    {
        if (actor == null)
        {
            Debug.LogError("CollectTheCoins-- trying to assign coins to a null actor.");
            return;
        }

        if (numCoins == 0)
        {
            Debug.LogError($"CollectTheCoins-- trying to assign 0 spawn coins to actor {actor.DebugNameString()}.");
            return;
        }

        if (m_serverData.m_actorsToCoins.ContainsKey(actor))
        {
            m_serverData.m_actorsToCoins[actor] += numCoins;
        }
        else
        {
            m_serverData.m_actorsToCoins.Add(actor, numCoins);
        }

        if (m_gainObjPointPerCoinPickedUp)
        {
            ObjectivePoints.Get().AdjustPoints(numCoins, actor.GetTeam());
        }
    }

    // added in rogues
    public void SpawnCoinOnSquare_Server(BoardSquare square, int numCoins)
    {
        if (square == null)
        {
            Debug.LogError("CollectTheCoins-- trying to spawn coins on a null square.");
            return;
        }

        if (numCoins == 0)
        {
            Debug.LogError($"CollectTheCoins-- trying to 0 spawn coins on square {BoardSquare.DebugString(square)}.");
            return;
        }

        if (m_serverData.m_squaresToCoins.ContainsKey(square))
        {
            m_serverData.m_squaresToCoins[square] += numCoins;
        }
        else
        {
            m_serverData.m_squaresToCoins.Add(square, numCoins);
        }
    }

    // added in rogues
    public void OnActorDroppedCoins_Server(ActorData actor, BoardSquare square)
    {
        if (actor == null)
        {
            Debug.LogError("CollectTheCoins-- null actor trying to drop coins.");
            return;
        }

        if (square == null)
        {
            Debug.LogError($"CollectTheCoins-- actor {actor.DebugNameString()} trying to drop coins on a null square.");
            return;
        }

        if (!m_serverData.m_actorsToCoins.ContainsKey(actor))
        {
            Debug.LogError($"CollectTheCoins-- actor {actor.DebugNameString()} trying to drop coins "
                           + $"on square {BoardSquare.DebugString(square)}, but that actor isn't in m_actorsToCoins.");
            return;
        }

        int num = m_serverData.m_actorsToCoins[actor];
        m_serverData.m_actorsToCoins[actor] = 0;
        m_serverData.m_actorsToCoins.Remove(actor);
        if (num == 0)
        {
            Debug.LogError($"CollectTheCoins-- actor {actor.DebugNameString()} trying to drop coins "
                           + $"on square {BoardSquare.DebugString(square)}, but that actor has 0 coins.");
            return;
        }

        if (m_serverData.m_squaresToSpills.ContainsKey(square))
        {
            m_serverData.m_squaresToSpills[square] += num;
        }
        else
        {
            m_serverData.m_squaresToSpills.Add(square, num);
        }

        if (m_loseObjPointPerCoinDropped)
        {
            ObjectivePoints.Get().AdjustPoints(-num, actor.GetTeam());
        }
    }
#endif
    
    public int GetCoinsForActor_Client(ActorData actor)
    {
        return m_clientData.m_actorsToCoins_unresolved.TryGetValue(actor, out int num)
            ? num
            : 0;
    }

    public void ExecuteClientGameModeEvent(ClientGameModeEvent gameModeEvent)
    {
        if (gameModeEvent == null)
        {
            return;
        }

        GameModeEventType eventType = gameModeEvent.m_eventType;
        switch (eventType)
        {
            case GameModeEventType.Ctc_CoinPickedUp:
            {
                BoardSquare square = gameModeEvent.m_square;
                if (m_clientData.m_squaresToCoins_unresolved.ContainsKey(square))
                {
                    int numCoins = m_clientData.m_squaresToCoins_unresolved[square];
                    OnActorGainedCoins_Client(gameModeEvent.m_primaryActor, numCoins);
                    RemoveCoinVisualsFromSquare(square);
                    m_clientData.m_squaresToCoins_unresolved[square] = 0;
                    m_clientData.m_squaresToCoins_unresolved.Remove(square);
                }
                break;
            }
            case GameModeEventType.Ctc_CoinsDropped:
            {
                OnActorDroppedCoins_Client(gameModeEvent.m_primaryActor, gameModeEvent.m_square);
                break;
            }
            case GameModeEventType.Ctc_NonCoinPowerupTouched:
            {
                OnActorGainedCoins_Client(gameModeEvent.m_primaryActor, m_numCoinsToAwardPerNonCoinPowerup);
                break;
            }
            case GameModeEventType.Ctc_CoinPowerupTouched:
            {
                OnActorGainedCoins_Client(gameModeEvent.m_primaryActor, m_numCoinsToAwardPerCoinPowerup);
                break;
            }
            default:
            {
                Debug.LogError("CollectTheCoins trying to handle non-Ctc event type " + eventType + ".");
                break;
            }
        }
    }

    public void OnActorGainedCoins_Client(ActorData actor, int numCoins)
    {
        if (actor == null)
        {
            Debug.LogError("CollectTheCoins (client)-- trying to assign coins to a null actor.");
            return;
        }

        if (numCoins == 0)
        {
            Debug.LogError($"CollectTheCoins (client)-- trying to assign 0 spawn coins to actor {actor.DebugNameString()}.");
            return;
        }

        if (!m_clientData.m_actorsToCoins_unresolved.ContainsKey(actor))
        {
            m_clientData.m_actorsToCoins_unresolved.Add(actor, numCoins);
        }
        else
        {
            m_clientData.m_actorsToCoins_unresolved[actor] += numCoins;
        }

        if (!m_gainObjPointPerCoinPickedUp)
        {
            return;
        }
        
        ObjectivePoints.Get().AdjustUnresolvedPoints(numCoins, actor.GetTeam());
    }

    public void OnActorDroppedCoins_Client(ActorData actor, BoardSquare square)
    {
        if (actor == null)
        {
            Debug.LogError("CollectTheCoins (client)-- null actor trying to drop coins.");
            return;
        }

        if (square == null)
        {
            Debug.LogError($"CollectTheCoins (client)-- actor {actor.DebugNameString()} trying to drop coins on a null square.");
            return;
        }

        if (!m_clientData.m_actorsToCoins_unresolved.TryGetValue(actor, out int num))
        {
            Debug.LogError($"CollectTheCoins (client)-- actor {actor.DebugNameString()} trying to drop coins "
                           + $"on square {BoardSquare.DebugString(square)}, "
                           + "but that actor isn't in m_clientData.m_actorsToCoins_unresolved.");
            return;
        }

        m_clientData.m_actorsToCoins_unresolved[actor] = 0;
        m_clientData.m_actorsToCoins_unresolved.Remove(actor);
        if (num == 0)
        {
            Debug.LogError(
                $"CollectTheCoins (client)-- actor {actor.DebugNameString()} trying to drop coins "
                + $"on square {BoardSquare.DebugString(square)}, but that actor has 0 coins.");
            return;
        }

        AddCoinSpillVisualToSquare(square, num);
        
        if (m_loseObjPointPerCoinDropped)
        {
            Team team = actor.GetTeam();
            ObjectivePoints.Get().AdjustUnresolvedPoints(-num, team);
        }
    }

    public void AddCoinVisualToSquare(BoardSquare square)
    {
        List<GameObject> list;
        if (m_clientData.m_squaresToCoinVisuals.TryGetValue(square, out List<GameObject> coinObjects))
        {
            list = coinObjects;
        }
        else
        {
            list = new List<GameObject>();
            m_clientData.m_squaresToCoinVisuals.Add(square, list);
        }

        GameObject item = Instantiate(m_coinOnGroundPrefab, square.ToVector3(), Quaternion.identity);
        list.Add(item);
    }

    public void RemoveCoinVisualsFromSquare(BoardSquare square)
    {
        if (!m_clientData.m_squaresToCoinVisuals.TryGetValue(square, out List<GameObject> coinObjects))
        {
            return;
        }

        foreach (GameObject coinObject in coinObjects)
        {
            Destroy(coinObject);
        }
        coinObjects.Clear();
        
        m_clientData.m_squaresToCoinVisuals.Remove(square);
    }

    public void SynchCoinVisualsToDictionary(Dictionary<BoardSquare, int> squaresToCoins)
    {
        List<BoardSquare> squaresToUpdate = new List<BoardSquare>();
        foreach (KeyValuePair<BoardSquare, List<GameObject>> squaresToCoinVisual in m_clientData.m_squaresToCoinVisuals)
        {
            BoardSquare square = squaresToCoinVisual.Key;
            int count = squaresToCoinVisual.Value.Count;
            int num = squaresToCoins.TryGetValue(square, out int coinNum)
                ? coinNum
                : 0;

            if (count > num)
            {
                squaresToUpdate.Add(square);
            }
        }

        foreach (BoardSquare square in squaresToUpdate)
        {
            RemoveCoinVisualsFromSquare(square);
        }

        foreach (KeyValuePair<BoardSquare, int> squareToCoins in squaresToCoins)
        {
            BoardSquare square = squareToCoins.Key;
            int actualNum = squareToCoins.Value;
            int visualNum = m_clientData.m_squaresToCoinVisuals.TryGetValue(square, out List<GameObject> coinObjects)
                ? coinObjects.Count
                : 0;

            if (actualNum > visualNum)
            {
                for (int i = visualNum; i < actualNum; i++)
                {
                    AddCoinVisualToSquare(square);
                }
            }
        }
    }

    public void AddCoinSpillVisualToSquare(BoardSquare square, int numCoinsInSpill)
    {
        if (m_spillInAirPrefab == null)
        {
            return;
        }

        List<GameObject> list;
        if (m_clientData.m_squaresToSpillsVisuals.TryGetValue(square, out List<GameObject> coinObjects))
        {
            list = coinObjects;
        }
        else
        {
            list = new List<GameObject>();
            m_clientData.m_squaresToSpillsVisuals.Add(square, list);
        }

        Vector3 position = square.ToVector3() + Vector3.up * m_spillVerticalOffset;
        GameObject item = Instantiate(m_spillInAirPrefab, position, Quaternion.identity);
        list.Add(item);
    }

    public void ClearCoinSpillVisuals()
    {
        foreach (List<GameObject> coinObjects in m_clientData.m_squaresToSpillsVisuals.Values)
        {
            foreach (GameObject coinObject in coinObjects)
            {
                Destroy(coinObject);
            }

            coinObjects.Clear();
        }

        m_clientData.m_squaresToSpillsVisuals.Clear();
    }

    public void Client_OnActorDeath(ActorData actor)
    {
        if (!m_clientData.m_actorsToCoins_unresolved.ContainsKey(actor)
            || m_clientData.m_actorsToCoins_unresolved[actor] <= 0)
        {
            return;
        }

        OnActorDroppedCoins_Client(actor, actor.GetTravelBoardSquare());
    }

    public void OnTurnTick()
    {
        ClearCoinSpillVisuals();
    }

    private void CalculateCoins_Server(out int coinsTeamA, out int coinsTeamB)
    {
        coinsTeamA = 0;
        coinsTeamB = 0;
#if SERVER
        // added in rogues
        if (m_serverData.m_actorsToCoins == null)
        {
            return;
        }
        
        foreach (KeyValuePair<ActorData, int> actorToCoins in m_serverData.m_actorsToCoins)
        {
            if (actorToCoins.Value == 0)
            {
                continue;
            }
            
            if (actorToCoins.Key.GetTeam() == Team.TeamA)
            {
                coinsTeamA += actorToCoins.Value;
            }
            else if (actorToCoins.Key.GetTeam() == Team.TeamB)
            {
                coinsTeamB += actorToCoins.Value;
            }
        }
#endif
    }

    private void CalculateCoins_Client(out int coinsTeamA, out int coinsTeamB)
    {
        coinsTeamA = 0;
        coinsTeamB = 0;
        if (m_clientData.m_actorsToCoins_unresolved == null)
        {
            return;
        }
        
        foreach (KeyValuePair<ActorData, int> actorToCoins in m_clientData.m_actorsToCoins_unresolved)
        {
            if (actorToCoins.Value == 0)
            {
                continue;
            }
            
            if (actorToCoins.Key.GetTeam() == Team.TeamA)
            {
                coinsTeamA += actorToCoins.Value;
            }
            else if (actorToCoins.Key.GetTeam() == Team.TeamB)
            {
                coinsTeamB += actorToCoins.Value;
            }
        }
    }

    public static bool AreCtcVictoryConditionsMetForTeam(CollectTheCoins_VictoryCondition[] conditions, Team checkTeam)
    {
        if (Get() == null)
        {
            return true;
        }

        if (conditions == null || conditions.Length == 0)
        {
            return true;
        }

        if (checkTeam != Team.TeamA && checkTeam != Team.TeamB)
        {
            return true;
        }

        int coinsTeamA;
        int coinsTeamB;
        if (NetworkServer.active)
        {
            Get().CalculateCoins_Server(out coinsTeamA, out coinsTeamB);
        }
        else
        {
            Get().CalculateCoins_Client(out coinsTeamA, out coinsTeamB);
        }

        int numTeamA;
        int numTeamB;
        if (checkTeam == Team.TeamA)
        {
            numTeamA = coinsTeamA;
            numTeamB = coinsTeamB;
        }
        else
        {
            numTeamA = coinsTeamB;
            numTeamB = coinsTeamA;
        }

        foreach (CollectTheCoins_VictoryCondition condition in conditions)
        {
            if (condition == CollectTheCoins_VictoryCondition.TeamMustHaveMostCoins)
            {
                if (numTeamA <= numTeamB)
                {
                    return false;
                }
            }
            else if (condition == CollectTheCoins_VictoryCondition.TeamMustNotHaveMostCoins)
            {
                if (numTeamA > numTeamB)
                {
                    return false;
                }
            }
        }

        return true;
    }

    // reactor
    private void UNetVersion()
    // rogues
    // private void MirrorProcessed()
    {
    }
}