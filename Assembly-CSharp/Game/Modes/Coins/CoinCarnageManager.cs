// SERVER
// ROGUES
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CoinCarnageManager : NetworkBehaviour
{
    public enum Variant
    {
        PeriodicSingleTurnin,
        SeparateTurninsPerTeam
    }

    public Variant m_variant;
    public GameObject m_coinPrefab;

    public List<CoinSpawnInfo> m_coinSpawnPointGroups;
    public int m_coinAdditionalSpawnOnDeath = 1;
    public int m_teamTurninUnlockThreshold = 10;
    public int m_individualTurninUnlockThreshold = -1;
    public int m_turnInAreaAppearanceDelay = 5;
    public int m_turnInAreaPreviewTurns = 2;
    public int m_turnInAreaDuration = 2;

    public BoardRegion[] m_coinTurninLocations;
    public GameObject m_turninRegionPrefabFriendly;
    public GameObject m_turninRegionPrefabFriendlyPreview;
    public GameObject m_turninRegionPrefabEnemy;
    public GameObject m_turninRegionPrefabEnemyPreview;

    [Space(10f)]
    public bool m_showDebugCoinCountInNameplate = true;
    public bool m_showDebugUI;
    public Text m_debugTextLeft;
    public Text m_debugTextRight;
    public Color m_debugTurnInRegionColor = Color.magenta;

    private Dictionary<ActorData, int> m_actorToCoinCount;
    private List<CoinSpawnPointTrackingData> m_coinSpawnPointTracking;
    private List<CoinCarnageCoin> m_coins;
    private Dictionary<CoinCarnageCoin, CoinSpawnPointTrackingData> m_coinToSpawnPoint;

    [SyncVar(hook = "HookSetTurninRegionLocationA")]
    private int m_coinTurninIdxTeamA = -1;
    [SyncVar(hook = "HookSetTurninRegionLocationB")]
    private int m_coinTurninIdxTeamB = -1;
    private List<int> m_turninLocationChoices;
    [SyncVar(hook = "HookSetTurnsUntilTurnInSpawn")]
    private int m_turnsUntilTurnInSpawn;
    [SyncVar(hook = "HookSetTurnsUntilTurnInDeSpawn")]
    private int m_turnsUntilTurnInDeSpawn;

    private Dictionary<Team, GameObject> m_spawnedTurnInLocationInstances = new Dictionary<Team, GameObject>();

    private static CoinCarnageManager s_instance;
    private static int kRpcRpcActorPickedUpCoin = -867352238; // removed in rogues

    public int Networkm_coinTurninIdxTeamA
    {
        get => m_coinTurninIdxTeamA;
        [param: In]
        set
        {
            if (NetworkServer.localClientActive && !syncVarHookGuard)
            {
                syncVarHookGuard = true;
                HookSetTurninRegionLocationA(value);
                syncVarHookGuard = false;
            }

            SetSyncVar(value, ref m_coinTurninIdxTeamA, 1u);
        }
    }

    public int Networkm_coinTurninIdxTeamB
    {
        get => m_coinTurninIdxTeamB;
        [param: In]
        set
        {
            if (NetworkServer.localClientActive && !syncVarHookGuard)
            {
                syncVarHookGuard = true;
                HookSetTurninRegionLocationB(value);
                syncVarHookGuard = false;
            }

            SetSyncVar(value, ref m_coinTurninIdxTeamB, 2u);
        }
    }

    public int Networkm_turnsUntilTurnInSpawn
    {
        get => m_turnsUntilTurnInSpawn;
        [param: In]
        set
        {
            if (NetworkServer.localClientActive && !syncVarHookGuard)
            {
                syncVarHookGuard = true;
                HookSetTurnsUntilTurnInSpawn(value);
                syncVarHookGuard = false;
            }

            SetSyncVar(value, ref m_turnsUntilTurnInSpawn, 4u);
        }
    }

    public int Networkm_turnsUntilTurnInDeSpawn
    {
        get => m_turnsUntilTurnInDeSpawn;
        [param: In]
        set
        {
            if (NetworkServer.localClientActive && !syncVarHookGuard)
            {
                syncVarHookGuard = true;
                HookSetTurnsUntilTurnInDeSpawn(value);
                syncVarHookGuard = false;
            }

            SetSyncVar(value, ref m_turnsUntilTurnInDeSpawn, 8u);
        }
    }

    static CoinCarnageManager()
    {
        // reactor
        RegisterRpcDelegate(typeof(CoinCarnageManager), kRpcRpcActorPickedUpCoin, InvokeRpcRpcActorPickedUpCoin);
        NetworkCRC.RegisterBehaviour("CoinCarnageManager", 0);
        // rogues
        // NetworkBehaviour.RegisterRpcDelegate(typeof(CoinCarnageManager), "RpcActorPickedUpCoin", new CmdDelegate(InvokeRpcRpcActorPickedUpCoin));
    }

    public static CoinCarnageManager Get()
    {
        return s_instance;
    }

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Log.Error("Multiple CoinCarnageManager components in this scene, remove extraneous ones.");
        }

        m_coins = new List<CoinCarnageCoin>();
        m_actorToCoinCount = new Dictionary<ActorData, int>();
        m_coinSpawnPointTracking = new List<CoinSpawnPointTrackingData>();
        m_coinToSpawnPoint = new Dictionary<CoinCarnageCoin, CoinSpawnPointTrackingData>();
        m_turninLocationChoices = new List<int>(m_coinTurninLocations.Length);

        for (int i = 0; i < m_coinTurninLocations.Length; i++)
        {
            m_coinTurninLocations.Initialize();
            m_turninLocationChoices.Add(i);
        }

        Networkm_turnsUntilTurnInSpawn = m_turnInAreaAppearanceDelay;
        Networkm_turnsUntilTurnInDeSpawn = -1;
        if (!NetworkServer.active)
        {
            SpawnTurninRegionVisuals(m_coinTurninIdxTeamA, Team.TeamA);
            if (m_variant != Variant.PeriodicSingleTurnin)
            {
                SpawnTurninRegionVisuals(m_coinTurninIdxTeamB, Team.TeamB);
            }
        }

        if (m_debugTextLeft != null)
        {
            m_debugTextLeft.text = string.Empty;
        }

        if (m_debugTextRight != null)
        {
            m_debugTextRight.text = string.Empty;
        }
    }

    private void Start()
    {
        foreach (CoinSpawnInfo coinSpawnInfo in m_coinSpawnPointGroups)
        {
            foreach (Transform spawnLocation in coinSpawnInfo.m_spawnLocations)
            {
                BoardSquare square = Board.Get().GetSquareFromTransform(spawnLocation);
                if (square != null && square.IsValidForGameplay())
                {
                    m_coinSpawnPointTracking.Add(
                        new CoinSpawnPointTrackingData(coinSpawnInfo.m_spawnRulePerSquare, square));
                }
            }
        }
    }

    private void OnDestroy()
    {
        s_instance = null;
    }

    private void OnDrawGizmos()
    {
        if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
        {
            return;
        }

        if (m_coinTurninLocations == null)
        {
            return;
        }

        foreach (BoardRegion boardRegion in m_coinTurninLocations)
        {
            // reactor
            boardRegion.Initialize();
            // rogues
            // boardRegion.Initialize(gameObject.scene);

            boardRegion.GizmosDrawRegion(m_debugTurnInRegionColor);
        }
    }

    [Server]
    public void OnActorMoved(ActorData mover, BoardSquare movementSquare)
    {
        if (!NetworkServer.active)
        {
            Debug.LogWarning(
                "[Server] function 'System.Void CoinCarnageManager::OnActorMoved(ActorData,BoardSquare)' called on client");
            return;
        }

        foreach (CoinCarnageCoin coinCarnageCoin in m_coins)
        {
            if (coinCarnageCoin.IsPickedUp() || coinCarnageCoin.GetSquare() != movementSquare)
            {
                continue;
            }

            coinCarnageCoin.PickUp(mover);
            AddCoinToActor(mover, 1);
            if (m_coinToSpawnPoint.TryGetValue(coinCarnageCoin, out CoinSpawnPointTrackingData data))
            {
                data.TurnOfLastPickup = GameFlowData.Get().CurrentTurn;
            }

            CheckTurninAvailability(mover.GetTeam());
        }
    }

    [Server]
    public void OnActorDeath(ActorData actor)
    {
        if (!NetworkServer.active)
        {
            Debug.LogWarning(
                "[Server] function 'System.Void CoinCarnageManager::OnActorDeath(ActorData)' called on client");
            return;
        }

        if (!GameplayUtils.IsValidPlayer(actor))
        {
            return;
        }

        int numCoinsToDrop = GetCoinCountForActor(actor) + m_coinAdditionalSpawnOnDeath;
        int numCoinsDropped = 0;
        ClearCoinsFromActor(actor);

        BoardSquare currentSquare = actor.GetCurrentBoardSquare();
        if (currentSquare == null)
        {
            Debug.LogError("Actor dead, has no board square");
            return;
        }

        HashSet<BoardSquare> coinOccupiedSquares = GetCoinOccupiedSquares();
        for (int i = 0; i < 6; i++)
        {
            if (numCoinsDropped >= numCoinsToDrop)
            {
                break;
            }

            List<BoardSquare> squares = AreaEffectUtils.GetSquaresInBorderLayer(currentSquare, i, true);
            foreach (BoardSquare square in squares)
            {
                if (numCoinsDropped >= numCoinsToDrop)
                {
                    break;
                }

                if (square.IsValidForGameplay() && !coinOccupiedSquares.Contains(square))
                {
                    SpawnCoinOnSquare(square);
                    numCoinsDropped++;
                }
            }
        }

        if (numCoinsDropped != numCoinsToDrop)
        {
            Debug.LogError("Did not spawn desired amount of coins!");
        }
    }

    [Server]
    public void OnTurnStart()
    {
        if (!NetworkServer.active)
        {
            Debug.LogWarning("[Server] function 'System.Void CoinCarnageManager::OnTurnStart()' called on client");
            return;
        }

        CleanupPickedUpCoins();
        SpawnCoinsForTurn();
        UpdateDebugUI();
        // if (!m_showDebugCoinCountInNameplate)
        // {
        // }
    }

    [Server]
    public void OnTurnEnd()
    {
        if (!NetworkServer.active)
        {
            Debug.LogWarning("[Server] function 'System.Void CoinCarnageManager::OnTurnEnd()' called on client");
            return;
        }

        foreach (ActorData actor in GameFlowData.Get().GetAllTeamMembers(Team.TeamA))
        {
            CheckTurninForActor(actor);
        }

        foreach (ActorData actor in GameFlowData.Get().GetAllTeamMembers(Team.TeamB))
        {
            CheckTurninForActor(actor);
        }

        CleanupPickedUpCoins();
        if (m_variant == Variant.PeriodicSingleTurnin)
        {
            if (m_turnsUntilTurnInSpawn >= 0)
            {
                Networkm_turnsUntilTurnInSpawn = m_turnsUntilTurnInSpawn - 1;
            }

            if (m_turnsUntilTurnInDeSpawn >= 0)
            {
                Networkm_turnsUntilTurnInDeSpawn = m_turnsUntilTurnInDeSpawn - 1;
            }

            if (m_turnInAreaPreviewTurns >= 0 && m_turnsUntilTurnInSpawn == m_turnInAreaPreviewTurns)
            {
                ActivateTurnInArea(Team.Invalid);
            }

            if (m_turnsUntilTurnInSpawn == 0)
            {
                Networkm_turnsUntilTurnInDeSpawn = m_turnInAreaDuration;
            }

            if (m_turnsUntilTurnInDeSpawn == 0)
            {
                DeactivateTurnInArea(Team.Invalid);
                Networkm_turnsUntilTurnInSpawn = m_turnInAreaAppearanceDelay;
                Networkm_turnsUntilTurnInDeSpawn = -1;
            }
        }

        UpdateDebugUI();
    }

    [Server]
    private void CheckTurninForActor(ActorData actor)
    {
        if (!NetworkServer.active)
        {
            Debug.LogWarning(
                "[Server] function 'System.Void CoinCarnageManager::CheckTurninForActor(ActorData)' called on client");
            return;
        }

        if (m_variant == Variant.PeriodicSingleTurnin && m_turnsUntilTurnInSpawn > 0)
        {
            return;
        }

        int turninIdx = actor.GetTeam() == Team.TeamA
            ? m_coinTurninIdxTeamA
            : m_coinTurninIdxTeamB;

        if (turninIdx <= -1 || turninIdx >= m_coinTurninLocations.Length)
        {
            return;
        }

        int coinCountForActor = GetCoinCountForActor(actor);
        if (coinCountForActor > 0 && m_coinTurninLocations[turninIdx].IsActorInRegion(actor))
        {
            ObjectivePoints objectivePoints = ObjectivePoints.Get();
            if (objectivePoints != null)
            {
                objectivePoints.AdjustPoints(coinCountForActor, actor.GetTeam());
            }

            ClearCoinsFromActor(actor);
        }
    }

    [Client]
    private void HookSetTurninRegionLocationA(int newValue)
    {
        if (!NetworkClient.active)
        {
            Debug.LogWarning(
                "[Client] function 'System.Void CoinCarnageManager::HookSetTurninRegionLocationA(System.Int32)' called on server");
            return;
        }

        Networkm_coinTurninIdxTeamA = newValue;
        bool preview = m_variant == Variant.PeriodicSingleTurnin;
        SpawnTurninRegionVisuals(m_coinTurninIdxTeamA, Team.TeamA, preview);
    }

    [Client]
    private void HookSetTurninRegionLocationB(int newValue)
    {
        if (!NetworkClient.active)
        {
            Debug.LogWarning(
                "[Client] function 'System.Void CoinCarnageManager::HookSetTurninRegionLocationB(System.Int32)' called on server");
            return;
        }

        Networkm_coinTurninIdxTeamB = newValue;
        if (m_variant != Variant.PeriodicSingleTurnin)
        {
            SpawnTurninRegionVisuals(m_coinTurninIdxTeamB, Team.TeamB);
        }
    }

    [Client]
    private void SpawnTurninRegionVisuals(int locationIndex, Team team, bool preview = false)
    {
        if (!NetworkClient.active)
        {
            Debug.LogWarning(
                "[Client] function 'System.Void CoinCarnageManager::SpawnTurninRegionVisuals(System.Int32,Team,System.Boolean)' called on server");
            return;
        }

        if (locationIndex == -1)
        {
            if (m_spawnedTurnInLocationInstances.TryGetValue(team, out GameObject value))
            {
                Destroy(value);
                m_spawnedTurnInLocationInstances.Remove(team);
            }

            return;
        }

        if (locationIndex < 0 || locationIndex >= m_coinTurninLocations.Length)
        {
            return;
        }

        BoardRegion turninLocation = m_coinTurninLocations[locationIndex];
        if (turninLocation == null)
        {
            return;
        }

        if (GameFlowData.Get() == null || GameFlowData.Get().activeOwnedActorData == null)
        {
            Log.Error("Could not find local player to spawn turn-in region.");
            return;
        }

        bool isFriendly = m_variant == Variant.PeriodicSingleTurnin
                          || team == GameFlowData.Get().activeOwnedActorData.GetTeam();

        GameObject turninRegionPrefab = isFriendly
            ? preview
                ? m_turninRegionPrefabFriendlyPreview
                : m_turninRegionPrefabFriendly
            : preview
                ? m_turninRegionPrefabEnemyPreview
                : m_turninRegionPrefabEnemy;

        if (turninRegionPrefab != null)
        {
            // reactor
            turninLocation.Initialize();
            // rogues
            // turninLocation.Initialize(gameObject.scene);

            GameObject turninRegionParentObject = new GameObject();
            List<BoardSquare> squaresInRegion = turninLocation.GetSquaresInRegion();
            foreach (BoardSquare current in squaresInRegion)
            {
                if (current.WorldBounds.HasValue)
                {
                    GameObject turninRegionObject = Instantiate(turninRegionPrefab);
                    turninRegionObject.transform.position = current.WorldBounds.Value.center;
                    turninRegionObject.transform.localScale = current.WorldBounds.Value.extents * 2f;
                    turninRegionObject.transform.parent = turninRegionParentObject.transform;
                }
            }

            if (m_spawnedTurnInLocationInstances.ContainsKey(team))
            {
                Log.Error("Turn in location already spawned for " + team);
                m_spawnedTurnInLocationInstances[team] = turninRegionParentObject;
            }
            else
            {
                m_spawnedTurnInLocationInstances.Add(team, turninRegionParentObject);
            }
        }

        if (m_variant == Variant.SeparateTurninsPerTeam)
        {
            ShowTurninAvailableMessage(isFriendly);
        }
    }

    [Client]
    private void ShowTurninAvailableMessage(bool friendlyArea)
    {
        if (!NetworkClient.active)
        {
            Debug.LogWarning(
                "[Client] function 'System.Void CoinCarnageManager::ShowTurninAvailableMessage(System.Boolean)' called on server");
            return;
        }

        string text = "[TEMP] ";
        if (m_variant == Variant.SeparateTurninsPerTeam)
        {
            text += friendlyArea ? "Your" : "Enemy";
        }
        else
        {
            text += "The";
        }

        text += " coin turn-in area has been activated.";
        InterfaceManager.Get().DisplayAlert(text, friendlyArea ? Color.cyan : Color.red, 7f);
    }

    [Client]
    private void HookSetTurnsUntilTurnInSpawn(int newValue)
    {
        if (!NetworkClient.active)
        {
            Debug.LogWarning(
                "[Client] function 'System.Void CoinCarnageManager::HookSetTurnsUntilTurnInSpawn(System.Int32)' called on server");
            return;
        }

        Networkm_turnsUntilTurnInSpawn = newValue;
        if (m_turnsUntilTurnInSpawn > 0)
        {
            string alertText = $"[TEMP] {m_turnsUntilTurnInSpawn} turns until turn-in available!";
            InterfaceManager.Get().DisplayAlert(alertText, Color.cyan, 7f);
        }

        if (m_turnsUntilTurnInSpawn == 0)
        {
            ShowTurninAvailableMessage(true);
            SpawnTurninRegionVisuals(-1, Team.TeamA);
            SpawnTurninRegionVisuals(m_coinTurninIdxTeamA, Team.TeamA);
        }
    }

    [Client]
    private void HookSetTurnsUntilTurnInDeSpawn(int newValue)
    {
        if (!NetworkClient.active)
        {
            Debug.LogWarning(
                "[Client] function 'System.Void CoinCarnageManager::HookSetTurnsUntilTurnInDeSpawn(System.Int32)' called on server");
            return;
        }

        Networkm_turnsUntilTurnInDeSpawn = newValue;
        if (m_turnsUntilTurnInDeSpawn > 0)
        {
            string alertText = $"[TEMP] {m_turnsUntilTurnInDeSpawn} turns until turn-in is no longer available!";
            InterfaceManager.Get().DisplayAlert(alertText, Color.cyan, 7f);
        }
    }

    [ClientRpc]
    public void RpcActorPickedUpCoin(int actorIndex)
    {
        ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
        if (actorData != null)
        {
            GameEventManager.Get().FireEvent(
                GameEventManager.EventType.MatchObjectiveEvent,
                new GameEventManager.MatchObjectiveEventArgs
                {
                    objective = GameEventManager.MatchObjectiveEventArgs.ObjectiveType.CoinCollected,
                    activatingActor = actorData
                });
        }
    }

    private int GetCoinCountForActor(ActorData actor)
    {
        return m_actorToCoinCount.TryGetValue(actor, out int num)
            ? num
            : 0;
    }

    [Server]
    private void AddCoinToActor(ActorData actor, int amount)
    {
        if (!NetworkServer.active)
        {
            Debug.LogWarning(
                "[Server] function 'System.Void CoinCarnageManager::AddCoinToActor(ActorData,System.Int32)' called on client");
            return;
        }

        if (m_actorToCoinCount.ContainsKey(actor))
        {
            m_actorToCoinCount[actor] += amount;
        }
        else
        {
            m_actorToCoinCount[actor] = amount;
        }

        CallRpcActorPickedUpCoin(actor.ActorIndex);
        // if (m_showDebugCoinCountInNameplate)
        // {
        // }
    }

    private void ClearCoinsFromActor(ActorData actor)
    {
        m_actorToCoinCount[actor] = 0;
        // if (m_showDebugCoinCountInNameplate)
        // {
        // }
    }

    private void CheckTurninAvailability(Team forTeam)
    {
        int teamCoinCount = 0;
        if (forTeam != Team.TeamA && forTeam != Team.TeamB)
        {
            throw new ArgumentException("Team must be Team A or Team B");
        }

        int turninIdx = forTeam == Team.TeamA
            ? m_coinTurninIdxTeamA
            : m_coinTurninIdxTeamB;
        if (turninIdx != -1)
        {
            return;
        }

        bool isAvailable = false;

        foreach (ActorData actor in GameFlowData.Get().GetAllTeamMembers(forTeam))
        {
            if (!m_actorToCoinCount.TryGetValue(actor, out var actorCoinCount))
            {
                continue;
            }

            teamCoinCount += actorCoinCount;
            if ((m_individualTurninUnlockThreshold != -1 && actorCoinCount >= m_individualTurninUnlockThreshold)
                || (m_teamTurninUnlockThreshold != -1 && teamCoinCount >= m_teamTurninUnlockThreshold))
            {
                isAvailable = true;
                break;
            }
        }

        if (isAvailable && m_turnInAreaAppearanceDelay <= 0)
        {
            ActivateTurnInArea(forTeam);
        }
    }

    private void ActivateTurnInArea(Team forTeam)
    {
        if (m_turninLocationChoices.Count == 0)
        {
            switch (m_variant)
            {
                case Variant.PeriodicSingleTurnin:
                {
                    RefillRandomLocationChoices();
                    break;
                }
                case Variant.SeparateTurninsPerTeam:
                {
                    if (forTeam == Team.TeamA)
                    {
                        Networkm_coinTurninIdxTeamA = m_coinTurninIdxTeamB;
                    }
                    else
                    {
                        Networkm_coinTurninIdxTeamB = m_coinTurninIdxTeamA;
                    }

                    return;
                }
            }
        }

        if (m_turninLocationChoices.Count == 0)
        {
            Log.Error("No turn-in locations defined in manager!");
            return;
        }

        int index = GameplayRandom.Range(0, m_turninLocationChoices.Count);
        int turninIdx = m_turninLocationChoices[index];
        m_turninLocationChoices.RemoveAt(index);
        switch (m_variant)
        {
            case Variant.SeparateTurninsPerTeam:
            {
                if (forTeam == Team.TeamA)
                {
                    Networkm_coinTurninIdxTeamA = turninIdx;
                }
                else
                {
                    Networkm_coinTurninIdxTeamB = turninIdx;
                }

                break;
            }
            case Variant.PeriodicSingleTurnin:
            {
                Networkm_coinTurninIdxTeamA = turninIdx;
                Networkm_coinTurninIdxTeamB = turninIdx;
                break;
            }
        }
    }

    private void DeactivateTurnInArea(Team forTeam)
    {
        Networkm_coinTurninIdxTeamA = -1;
        Networkm_coinTurninIdxTeamB = -1;
    }

    private void RefillRandomLocationChoices()
    {
        m_turninLocationChoices.Clear();
        for (int i = 0; i < m_coinTurninLocations.Length; i++)
        {
            m_turninLocationChoices.Add(i);
        }
    }

    private void SpawnCoinsForTurn()
    {
        for (int i = 0; i < m_coinSpawnPointTracking.Count; i++)
        {
            if (m_coinSpawnPointTracking[i].CanSpawnThisTurn(GameFlowData.Get().CurrentTurn))
            {
                CoinCarnageCoin coinCarnageCoin = SpawnCoinOnSquare(m_coinSpawnPointTracking[i].GetSpawnSquare());
                if (coinCarnageCoin != null)
                {
                    m_coinSpawnPointTracking[i].NumSpawnedSoFar++;
                    m_coinSpawnPointTracking[i].TurnOfLastSpawn = GameFlowData.Get().CurrentTurn;
                    m_coinToSpawnPoint[coinCarnageCoin] = m_coinSpawnPointTracking[i];
                }
            }
        }
    }

    private HashSet<BoardSquare> GetCoinOccupiedSquares()
    {
        HashSet<BoardSquare> result = new HashSet<BoardSquare>();
        foreach (CoinCarnageCoin coin in m_coins)
        {
            if (!coin.IsPickedUp())
            {
                result.Add(coin.GetSquare());
            }
        }

        return result;
    }

    private CoinCarnageCoin SpawnCoinOnSquare(BoardSquare square)
    {
        return square != null
               && square.IsValidForGameplay()
               && !GetCoinOccupiedSquares().Contains(square)
            ? CreateCoinObjectOnSquare(square)
            : null;
    }

    private CoinCarnageCoin CreateCoinObjectOnSquare(BoardSquare square)
    {
        if (!NetworkServer.active)
        {
            Log.Error("Attempted to call server function without server");
            return null;
        }

        GameObject coinObject = Instantiate(m_coinPrefab, square.ToVector3(), Quaternion.identity);
        CoinCarnageCoin carnageCoin = coinObject.GetComponent<CoinCarnageCoin>();
        carnageCoin.Initialize(square);
        NetworkServer.Spawn(coinObject);
        m_coins.Add(carnageCoin);
        return carnageCoin;
    }

    private void CleanupPickedUpCoins()
    {
        for (int i = m_coins.Count - 1; i >= 0; i--)
        {
            if (m_coins[i].IsPickedUp())
            {
                m_coinToSpawnPoint.Remove(m_coins[i]);
                m_coins[i].Destroy();
                m_coins.RemoveAt(i);
            }
            else if (m_coins[i] == null)
            {
                m_coins.RemoveAt(i);
            }
        }
    }

    private string GetCoinCountString(ActorData actor)
    {
        return "coin count: " + GetCoinCountForActor(actor);
    }

    private void UpdateDebugUI()
    {
        if (m_debugTextLeft == null
            || m_debugTextRight == null
            || !m_showDebugUI)
        {
            return;
        }

        int numCoinsActive = 0;
        foreach (CoinCarnageCoin current in m_coins)
        {
            if (!current.IsPickedUp())
            {
                numCoinsActive++;
            }
        }

        m_debugTextLeft.text = "Coin count [active/total tracked] = " + numCoinsActive + " / " + m_coins.Count + "\n";
        m_debugTextRight.text = "\n";

        string textTeamA = string.Empty;
        string textTeamB = string.Empty;
        foreach (KeyValuePair<ActorData, int> actorToCoinCount in m_actorToCoinCount)
        {
            ActorData actor = actorToCoinCount.Key;
            int coinCount = actorToCoinCount.Value;

            if (actor.GetTeam() == Team.TeamA)
            {
                textTeamA += actor.DisplayName + " -> " + coinCount + "\n";
            }
            else if (actor.GetTeam() == Team.TeamB)
            {
                textTeamB += actor.DisplayName + " -> " + coinCount + "\n";
            }
        }

        m_debugTextLeft.text += textTeamA;
        m_debugTextRight.text += textTeamB;
    }

    // reactor
    private void UNetVersion()
        // rogues
        // private void MirrorProcessed()
    {
    }

    protected static void InvokeRpcRpcActorPickedUpCoin(NetworkBehaviour obj, NetworkReader reader)
    {
        if (!NetworkClient.active)
        {
            Debug.LogError("RPC RpcActorPickedUpCoin called on server.");
            return;
        }

        ((CoinCarnageManager)obj).RpcActorPickedUpCoin((int)reader.ReadPackedUInt32());
    }

    public void CallRpcActorPickedUpCoin(int actorIndex)
    {
        if (!NetworkServer.active)
        {
            Debug.LogError("RPC Function RpcActorPickedUpCoin called on client.");
            return;
        }

        NetworkWriter networkWriter = new NetworkWriter();
        networkWriter.Write((short)0);
        networkWriter.Write((short)2);
        networkWriter.WritePackedUInt32((uint)kRpcRpcActorPickedUpCoin);
        networkWriter.Write(GetComponent<NetworkIdentity>().netId);
        networkWriter.WritePackedUInt32((uint)actorIndex);
        SendRPCInternal(networkWriter, 0, "RpcActorPickedUpCoin");
    }

    public override bool OnSerialize(NetworkWriter writer, bool forceAll)
    {
        if (forceAll)
        {
            writer.WritePackedUInt32((uint)m_coinTurninIdxTeamA);
            writer.WritePackedUInt32((uint)m_coinTurninIdxTeamB);
            writer.WritePackedUInt32((uint)m_turnsUntilTurnInSpawn);
            writer.WritePackedUInt32((uint)m_turnsUntilTurnInDeSpawn);
            return true;
        }

        bool flag = false;
        if ((syncVarDirtyBits & 1) != 0)
        {
            if (!flag)
            {
                writer.WritePackedUInt32(syncVarDirtyBits);
                flag = true;
            }

            writer.WritePackedUInt32((uint)m_coinTurninIdxTeamA);
        }

        if ((syncVarDirtyBits & 2) != 0)
        {
            if (!flag)
            {
                writer.WritePackedUInt32(syncVarDirtyBits);
                flag = true;
            }

            writer.WritePackedUInt32((uint)m_coinTurninIdxTeamB);
        }

        if ((syncVarDirtyBits & 4) != 0)
        {
            if (!flag)
            {
                writer.WritePackedUInt32(syncVarDirtyBits);
                flag = true;
            }

            writer.WritePackedUInt32((uint)m_turnsUntilTurnInSpawn);
        }

        if ((syncVarDirtyBits & 8) != 0)
        {
            if (!flag)
            {
                writer.WritePackedUInt32(syncVarDirtyBits);
                flag = true;
            }

            writer.WritePackedUInt32((uint)m_turnsUntilTurnInDeSpawn);
        }

        if (!flag)
        {
            writer.WritePackedUInt32(syncVarDirtyBits);
        }

        return flag;
    }

    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        if (initialState)
        {
            m_coinTurninIdxTeamA = (int)reader.ReadPackedUInt32();
            m_coinTurninIdxTeamB = (int)reader.ReadPackedUInt32();
            m_turnsUntilTurnInSpawn = (int)reader.ReadPackedUInt32();
            m_turnsUntilTurnInDeSpawn = (int)reader.ReadPackedUInt32();
            return;
        }

        int num = (int)reader.ReadPackedUInt32();
        if ((num & 1) != 0)
        {
            HookSetTurninRegionLocationA((int)reader.ReadPackedUInt32());
        }

        if ((num & 2) != 0)
        {
            HookSetTurninRegionLocationB((int)reader.ReadPackedUInt32());
        }

        if ((num & 4) != 0)
        {
            HookSetTurnsUntilTurnInSpawn((int)reader.ReadPackedUInt32());
        }

        if ((num & 8) != 0)
        {
            HookSetTurnsUntilTurnInDeSpawn((int)reader.ReadPackedUInt32());
        }
    }
}
