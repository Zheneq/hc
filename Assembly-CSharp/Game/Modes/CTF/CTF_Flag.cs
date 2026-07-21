// SERVER
// ROGUES
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CTF_Flag : NetworkBehaviour
{
    public byte m_flagGuid;
    public Team m_team;

    private ActorData m_serverHolderActor;
    private BoardSquare m_serverIdleSquare;
    private ActorData m_clientHolderActor;
    private BoardSquare m_clientIdleSquare;
    private Sequence m_flagBeingHeldSequenceInstance;

    private bool m_initializedOffscreenIndicator;
    private bool m_notifiedOfSpawn;
    private bool m_alreadyTurnedIn;
    private int m_lastClientUpdateFlagHolderEventGuid = -1;
    private ActorData m_gatheredHolderActor;
    private BoardSquare m_gatheredIdleSquare;
    private BoardSquarePathInfo m_gatheredPath;
    private int m_gatheredMovementDamageSincePickedUp;
    private int m_gatheredMovementDamageSinceTurnStart;
    private BoardSquare m_originalSquare;
    private int m_damageOnHolderSincePickedUp_Gross;
    private int m_damageOnHolderSinceTurnStart_Gross;
    private int m_clientUnresolvedDamageOnHolder;
    private int m_numFullTurnsSpentHeldInTurninRegion;
    private bool m_spentEntireTurnReadyToBeTurnedIn;
    
#if SERVER
    private StandardActorEffect m_flagHolderEffect; // added in rogues
#endif

    public ActorData ServerHolderActor
    {
        get => m_serverHolderActor;
        set
        {
            if (value != m_serverHolderActor)
            {
                m_serverHolderActor = value;
                DamageOnHolderSincePickedUp_Gross = 0;
                DamageOnHolderSinceTurnStart_Gross = 0;
            }
        }
    }

    public BoardSquare ServerIdleSquare
    {
        get => m_serverIdleSquare;
        set
        {
            if (value != m_serverIdleSquare)
            {
                m_serverIdleSquare = value;
            }
        }
    }

    public ActorData ClientHolderActor
    {
        get => m_clientHolderActor;
        set
        {
            if (value == m_clientHolderActor)
            {
                return;
            }
            
            m_clientHolderActor = value;
            ClientUnresolvedDamageOnHolder = 0;
            if (CaptureTheFlag.Get() == null || CaptureTheFlag.Get().m_flagBeingHeldSequence == null)
            {
                return;
            }
            
            if (m_flagBeingHeldSequenceInstance != null)
            {
                if (!m_flagBeingHeldSequenceInstance.MarkedForRemoval)
                {
                    m_flagBeingHeldSequenceInstance.MarkForRemoval();
                }

                m_flagBeingHeldSequenceInstance = null;
            }

            if (m_clientHolderActor != null)
            {
                Sequence[] sequences = SequenceManager.Get().CreateClientSequences(
                    CaptureTheFlag.Get().m_flagBeingHeldSequence,
                    m_clientHolderActor.CurrentBoardSquare,
                    m_clientHolderActor.AsArray(),
                    m_clientHolderActor,
                    CaptureTheFlag.Get().SequenceSource,
                    null);
                
                if (sequences != null
                    && sequences.Length != 0
                    && sequences.Length <= 1)
                {
                    m_flagBeingHeldSequenceInstance = sequences[0];
                }
                else
                {
                    Debug.LogError("CTF_Flag creating flag-being-held sequence, but had bad output.");
                }
            }
        }
    }

    public BoardSquare ClientIdleSquare
    {
        get => m_clientIdleSquare;
        set
        {
            if (value != m_clientIdleSquare)
            {
                m_clientIdleSquare = value;
            }
        }
    }

    public int LastClientUpdateFlagHolderEventGuid
    {
        get => m_lastClientUpdateFlagHolderEventGuid;
        set
        {
            if (m_lastClientUpdateFlagHolderEventGuid != value)
            {
                m_lastClientUpdateFlagHolderEventGuid = value;
            }
        }
    }

    public ActorData GatheredHolderActor
    {
        get => m_gatheredHolderActor;
        set
        {
            if (value != m_gatheredHolderActor)
            {
                m_gatheredHolderActor = value;
                GatheredMovementDamageSincePickedUp = 0;
                GatheredMovementDamageSinceTurnStart = 0;
            }
        }
    }

    public BoardSquare GatheredIdleSquare
    {
        get => m_gatheredIdleSquare;
        set
        {
            if (value != m_gatheredIdleSquare)
            {
                m_gatheredIdleSquare = value;
            }
        }
    }

    public BoardSquarePathInfo GatheredPath
    {
        get => m_gatheredPath;
        set
        {
            if (value != m_gatheredPath)
            {
                m_gatheredPath = value;
            }
        }
    }

    public int GatheredMovementDamageSincePickedUp
    {
        get => m_gatheredMovementDamageSincePickedUp;
        set
        {
            if (value != m_gatheredMovementDamageSincePickedUp)
            {
                m_gatheredMovementDamageSincePickedUp = value;
            }
        }
    }

    public int GatheredMovementDamageSinceTurnStart
    {
        get => m_gatheredMovementDamageSinceTurnStart;
        set
        {
            if (m_gatheredMovementDamageSinceTurnStart != value)
            {
                m_gatheredMovementDamageSinceTurnStart = value;
            }
        }
    }

    public int DamageOnHolderSincePickedUp_Gross
    {
        get => m_damageOnHolderSincePickedUp_Gross;
        set
        {
            if (value != m_damageOnHolderSincePickedUp_Gross)
            {
                m_damageOnHolderSincePickedUp_Gross = value;
                ClientUnresolvedDamageOnHolder = 0;
            }
        }
    }

    public int DamageOnHolderSinceTurnStart_Gross
    {
        get => m_damageOnHolderSinceTurnStart_Gross;
        set
        {
            if (value != m_damageOnHolderSinceTurnStart_Gross)
            {
                m_damageOnHolderSinceTurnStart_Gross = value;
                ClientUnresolvedDamageOnHolder = 0;
            }
        }
    }

    public int ClientUnresolvedDamageOnHolder
    {
        get => m_clientUnresolvedDamageOnHolder;
        set
        {
            if (value != m_clientUnresolvedDamageOnHolder)
            {
                m_clientUnresolvedDamageOnHolder = value;
            }
        }
    }

    public int NumFullTurnsSpentHeldInTurninRegion
    {
        get => m_numFullTurnsSpentHeldInTurninRegion;
        private set
        {
            if (m_numFullTurnsSpentHeldInTurninRegion != value)
            {
                m_numFullTurnsSpentHeldInTurninRegion = value;
            }
        }
    }

    public bool SpentEntireTurnReadyToBeTurnedIn
    {
        get => m_spentEntireTurnReadyToBeTurnedIn;
        private set
        {
            if (m_spentEntireTurnReadyToBeTurnedIn != value)
            {
                m_spentEntireTurnReadyToBeTurnedIn = value;
            }
        }
    }
    
    public void Initialize(BoardSquare square, Team team, byte flagGuid)
    {
        m_originalSquare = square;
        m_team = team;
        m_flagGuid = flagGuid;
        m_serverHolderActor = null;
        m_serverIdleSquare = square;
        m_clientHolderActor = null;
        m_clientIdleSquare = square;
        UpdatePosition();
    }

    public Sprite GetIcon()
    {
        return CaptureTheFlag.Get() != null
            ? CaptureTheFlag.Get().m_flagIcon
            : null;
    }

    public bool ShouldShowIndicator()
    {
        return m_clientIdleSquare != null;
    }
    
    public void OnNotHeldInTurninRegion()
    {
        NumFullTurnsSpentHeldInTurninRegion = 0;
        SpentEntireTurnReadyToBeTurnedIn = false;
    }

    private void Start()
    {
        if (CaptureTheFlag.Get() != null)
        {
            CaptureTheFlag.Get().OnNewFlagStarted(this);
        }
    }

    private void OnDestroy()
    {
        if (CaptureTheFlag.Get() != null)
        {
            CaptureTheFlag.Get().OnFlagDestroyed(this);
        }

        if (HUD_UI.Get() != null)
        {
            HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.RemoveCtfFlag(this);
        }
    }

    public Vector3 GetPosition()
    {
        return ClientHolderActor != null
            ? ClientHolderActor.IsActorVisibleToClient()
                ? ClientHolderActor.transform.position
                : transform.position
            : ClientIdleSquare != null
                ? ClientIdleSquare.ToVector3()
                : transform.position;
    }

    public Quaternion GetRotation()
    {
        return ClientHolderActor != null
            ? ClientHolderActor.transform.rotation
            : Quaternion.identity;
    }

    public BoardSquare GetOriginalSquare()
    {
        return m_originalSquare;
    }

    public Team GetIntrinsicTeam()
    {
        return m_team;
    }

    public Team GetCapturingTeam_Client()
    {
        if (m_team == Team.TeamA)
        {
            return Team.TeamB;
        }

        if (m_team == Team.TeamB)
        {
            return Team.TeamA;
        }

        if (ClientHolderActor != null && ClientHolderActor.GetTeam() == Team.TeamA)
        {
            return Team.TeamA;
        }

        if (ClientHolderActor != null && ClientHolderActor.GetTeam() == Team.TeamB)
        {
            return Team.TeamB;
        }

        return Team.Objects;
    }
    
#if SERVER
    // added in rogues
    public Team GetCapturingTeam_Server()
    {
        if (m_team == Team.TeamA)
        {
            return Team.TeamB;
        }

        if (m_team == Team.TeamB)
        {
            return Team.TeamA;
        }

        if (GatheredHolderActor != null && GatheredHolderActor.GetTeam() == Team.TeamA)
        {
            return Team.TeamA;
        }

        if (GatheredHolderActor != null && GatheredHolderActor.GetTeam() == Team.TeamB)
        {
            return Team.TeamB;
        }

        return Team.Objects;
    }

    // added in rogues
    public BoardRegion GetTeamAlignedTurninRegion(BoardRegion flagTurninTeamA, BoardRegion flagTurninTeamB)
    {
        switch (GetCapturingTeam_Server())
        {
            case Team.TeamA:
                return flagTurninTeamA;
            case Team.TeamB:
                return flagTurninTeamB;
            default:
                return null;
        }
    }

    // added in rogues
    public bool CanBeTurnedIn(
        BoardSquare currentSquare,
        BoardRegion flagTurninTeamA,
        BoardRegion flagTurninTeamB,
        BoardRegion flagTurninNeutral)
    {
        if (flagTurninNeutral != null
            && flagTurninNeutral.GetSquaresInRegion().Contains(currentSquare))
        {
            return true;
        }

        if (GatheredHolderActor != null)
        {
            BoardRegion teamAlignedTurninRegion = GetTeamAlignedTurninRegion(flagTurninTeamA, flagTurninTeamB);
            if (teamAlignedTurninRegion != null)
            {
                List<BoardSquare> squaresInRegion = teamAlignedTurninRegion.GetSquaresInRegion();
                return squaresInRegion != null && squaresInRegion.Contains(currentSquare);
            }
        }

        return false;
    }

    // added in rogues
    public void CTF_Flag_OnTurnEnd(bool canBeTurnedIn)
    {
        if (!canBeTurnedIn)
        {
            NumFullTurnsSpentHeldInTurninRegion = 0;
            SpentEntireTurnReadyToBeTurnedIn = false;
            return;
        }

        if (!SpentEntireTurnReadyToBeTurnedIn)
        {
            SpentEntireTurnReadyToBeTurnedIn = true;
            return;
        }

        NumFullTurnsSpentHeldInTurninRegion += 1;
    }

    // added in rogues
    public void CTF_Flag_OnTurnStart()
    {
        DamageOnHolderSinceTurnStart_Gross = 0;
        MarkAsDirty();
    }

    // added in rogues
    public void MarkAsDirty()
    {
        ClientHolderActor = m_serverHolderActor;
        ClientIdleSquare = m_serverIdleSquare;
        SetDirtyBit(1u);
    }

    // added in rogues
    public void OnPickedUp_Server(ActorData newHolder)
    {
        if (!NetworkServer.active)
        {
            Log.Error("Calling CTF_Flag.OnPickedUp_Server on a non-server.");
            return;
        }

        if (ServerHolderActor != null)
        {
            Log.Error($"CTF Flag held by {ServerHolderActor.DisplayName} is being picked up "
                      + $"by {newHolder.DisplayName} without first being dropped.");
        }

        if (m_flagHolderEffect != null)
        {
            Log.Error($"CTF Flag held by {ServerHolderActor.DisplayName} is being picked up "
                      + $"by {newHolder.DisplayName} without first clearing the holder-effect.");
        }

        ServerHolderActor = newHolder;
        ServerIdleSquare = null;
        if (CaptureTheFlag.Get().m_flagHolderEffect.m_applyEffect)
        {
            StandardActorEffectData effectData = CaptureTheFlag.Get().m_flagHolderEffect.m_effectData;
            EffectSource parent = new EffectSource($"Flag Holder: {name}", null, null);
            m_flagHolderEffect = new StandardActorEffect(
                parent,
                newHolder.GetCurrentBoardSquare(),
                newHolder,
                newHolder,
                effectData);
            m_flagHolderEffect.OverrideCanBeDispelledByStatusImmunity(false);
            ServerEffectManager.Get().ApplyEffect(m_flagHolderEffect);
        }
    }

    // added in rogues
    public void OnReturned_Server(ActorData returner)
    {
        if (!NetworkServer.active)
        {
            Log.Error("Calling CTF_Flag.OnReturned_Server on a non-server.");
            return;
        }

        if (ServerHolderActor != null)
        {
            Log.Error($"CTF Flag held by {ServerHolderActor.DisplayName} is being returned up "
                      + $"by {returner.DisplayName} without first being dropped.");
        }

        ServerIdleSquare = m_originalSquare;
    }

    // added in rogues
    public void OnDropped_Server(BoardSquare newIdleSquare)
    {
        if (!NetworkServer.active)
        {
            Log.Error("Calling CTF_Flag.OnDropped_Server on a non-server.");
            return;
        }

        if (m_flagHolderEffect != null)
        {
            if (!ServerHolderActor.IsDead())
            {
                List<Effect> actorEffects = ServerEffectManager.Get().GetActorEffects(ServerHolderActor);
                ServerEffectManager.Get().RemoveEffect(m_flagHolderEffect, actorEffects);
            }

            m_flagHolderEffect = null;
        }

        ServerIdleSquare = newIdleSquare;
        ServerHolderActor = null;
    }

    // added in rogues
    public void OnTurnedIn_Server()
    {
        if (!NetworkServer.active)
        {
            Log.Error("Calling CTF_Flag.OnTurnedIn_Server on a non-server.");
            return;
        }

        if (m_flagHolderEffect != null)
        {
            List<Effect> actorEffects = ServerEffectManager.Get().GetActorEffects(ServerHolderActor);
            ServerEffectManager.Get().RemoveEffect(m_flagHolderEffect, actorEffects);
            m_flagHolderEffect = null;
        }
    }
#endif
    
    public void OnPickedUp_Client(ActorData newHolder, int eventGuid)
    {
        if (!NetworkClient.active)
        {
            Log.Error("Calling CTF_Flag.OnPickedUp_Client on a non-client.");
            return;
        }

        if (eventGuid != -1 && eventGuid <= LastClientUpdateFlagHolderEventGuid)
        {
            return;
        }
        
        LastClientUpdateFlagHolderEventGuid = eventGuid;
        ActorData clientHolderActor = ClientHolderActor;
        ClientHolderActor = newHolder;
        ClientIdleSquare = null;
        if (CaptureTheFlag.Get() != null)
        {
            CaptureTheFlag.Get().Client_OnFlagHolderChanged(
                clientHolderActor,
                ClientHolderActor,
                false,
                m_alreadyTurnedIn);
        }

        GameEventManager.MatchObjectiveEventArgs matchObjectiveEventArgs =
            new GameEventManager.MatchObjectiveEventArgs
            {
                objective = GameEventManager.MatchObjectiveEventArgs.ObjectiveType.CasePickedUp_Client,
                controlPoint = null,
                activatingActor = newHolder,
                team = newHolder.GetTeam()
            };
        GameEventManager.Get().FireEvent(GameEventManager.EventType.MatchObjectiveEvent, matchObjectiveEventArgs);
    }

    public void OnReturned_Client(ActorData returner)
    {
        if (!NetworkClient.active)
        {
            Log.Error("Calling CTF_Flag.OnReturned_Client on a non-client.");
            return;
        }

        ClientHolderActor = null;
        ClientIdleSquare = m_originalSquare;
    }

    public void OnDropped_Client(BoardSquare newIdleSquare, int eventGuid)
    {
        if (!NetworkClient.active)
        {
            Log.Error("Calling CTF_Flag.OnDropped_Client on a non-client.");
            return;
        }

        if (eventGuid != -1 && eventGuid <= LastClientUpdateFlagHolderEventGuid)
        {
            return;
        }
        
        LastClientUpdateFlagHolderEventGuid = eventGuid;
        ActorData clientHolderActor = ClientHolderActor;
        ClientHolderActor = null;
        ClientIdleSquare = newIdleSquare;
        
        if (CaptureTheFlag.Get() != null)
        {
            CaptureTheFlag.Get().Client_OnFlagHolderChanged(
                clientHolderActor,
                ClientHolderActor,
                false,
                m_alreadyTurnedIn);
        }
    }

    public void OnTurnedIn_Client(ActorData capturingActor, int eventGuid)
    {
        if (!NetworkClient.active)
        {
            Log.Error("Calling CTF_Flag.OnTurnedIn_Client on a non-client.");
            return;
        }

        if (eventGuid != -1 && eventGuid <= LastClientUpdateFlagHolderEventGuid)
        {
            return;
        }
        
        LastClientUpdateFlagHolderEventGuid = eventGuid;
        ActorData clientHolderActor = ClientHolderActor;
        ClientHolderActor = null;
        ClientIdleSquare = null;
        
        if (CaptureTheFlag.Get() != null)
        {
            CaptureTheFlag.Get().Client_OnFlagHolderChanged(
                clientHolderActor,
                ClientHolderActor,
                true,
                m_alreadyTurnedIn);
        }

        m_alreadyTurnedIn = true;
        GameEventManager.MatchObjectiveEventArgs matchObjectiveEventArgs =
            new GameEventManager.MatchObjectiveEventArgs
            {
                objective = GameEventManager.MatchObjectiveEventArgs.ObjectiveType.FlagTurnedIn_Client,
                controlPoint = null,
                activatingActor = capturingActor,
                team = capturingActor.GetTeam()
            };
        GameEventManager.Get().FireEvent(GameEventManager.EventType.MatchObjectiveEvent, matchObjectiveEventArgs);
    }

    public override bool OnSerialize(NetworkWriter writer, bool initialState)
    {
        uint dirtyBits = initialState ? uint.MaxValue : syncVarDirtyBits;

        sbyte holderActorIndex = ServerHolderActor == null
            ? (sbyte)ActorData.s_invalidActorIndex
            : (sbyte)ServerHolderActor.ActorIndex;

        sbyte x;
        sbyte y;
        if (ServerIdleSquare == null)
        {
            x = -1;
            y = -1;
        }
        else
        {
            x = (sbyte)ServerIdleSquare.x;
            y = (sbyte)ServerIdleSquare.y;
        }

        writer.Write(m_flagGuid);
        writer.Write((byte)m_team);
        writer.Write(holderActorIndex);
        writer.Write(x);
        writer.Write(y);
        writer.Write(DamageOnHolderSincePickedUp_Gross);
        writer.Write(DamageOnHolderSinceTurnStart_Gross);
        
        return dirtyBits != 0;
    }

    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        byte flagGuid = reader.ReadByte();
        byte team = reader.ReadByte();
        sbyte holderActorIndex = reader.ReadSByte();
        sbyte x = reader.ReadSByte();
        sbyte y = reader.ReadSByte();
        int damageSincePickedUp = reader.ReadInt32();
        int damageSinceTurnStart = reader.ReadInt32();

        m_flagGuid = flagGuid;
        m_team = (Team)team;

        ActorData clientHolderActor = ClientHolderActor;
        ClientHolderActor = holderActorIndex != (sbyte)ActorData.s_invalidActorIndex
            ? GameFlowData.Get().FindActorByActorIndex(holderActorIndex)
            : null;

        if (x == -1 && y == -1)
        {
            ClientIdleSquare = null;
        }
        else
        {
            ClientIdleSquare = Board.Get().GetSquareFromIndex(x, y);
        }

        if (clientHolderActor != ClientHolderActor && CaptureTheFlag.Get() != null)
        {
            CaptureTheFlag.Get().Client_OnFlagHolderChanged(
                clientHolderActor,
                ClientHolderActor,
                false,
                m_alreadyTurnedIn);
        }

        DamageOnHolderSincePickedUp_Gross = damageSincePickedUp;
        DamageOnHolderSinceTurnStart_Gross = damageSinceTurnStart;
    }

    private void Update()
    {
        if (!NetworkClient.active)
        {
            return;
        }

        if (HUD_UI.Get() != null && !m_initializedOffscreenIndicator)
        {
            HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.AddCtfFlag(this);
            m_initializedOffscreenIndicator = true;
        }

        UpdatePosition();
        foreach (MeshRenderer meshRenderer in GetComponents<MeshRenderer>())
        {
            meshRenderer.enabled = ClientHolderActor == null;
        }

        foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.enabled = ClientHolderActor == null;
        }

        if (!m_notifiedOfSpawn
            && InterfaceManager.Get() != null
            && CaptureTheFlag.Get() != null)
        {
            InterfaceManager.Get().DisplayAlert(
                StringUtil.TR("BriefcaseLocated", "CTF"),
                CaptureTheFlag.Get().m_textColor_neutral);
            m_notifiedOfSpawn = true;
        }
    }

    public void UpdatePosition()
    {
        transform.position = GetPosition();
        transform.rotation = GetRotation();
    }

    // reactor
    private void UNetVersion()
    // rogues
    // private void MirrorProcessed()
    {
    }
}
