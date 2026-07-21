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
    private ClientSideData m_clientData;
    private static CollectTheCoins s_instance;
    private SequenceSource _sequenceSource;

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
        if (eventType == GameModeEventType.Ctc_CoinPickedUp)
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

            return;
        }

        switch (eventType)
        {
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

    private void UNetVersion()
    {
    }

    public override bool OnSerialize(NetworkWriter writer, bool forceAll)
    {
        return false;
    }
}