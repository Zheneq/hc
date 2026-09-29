using System;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

[Serializable]
public class NPCSpawner
{
    private enum NPCSpawnerState
    {
        NotYetSpawned,
        Active,
        WaitingToRespawn,
        Inactive
    }

    public string m_spawnerTitle;
    public GameObject m_actorPrefab;
    public CharacterResourceLink m_characterResourceLink;
    public NPCBrain m_actorBrain;
    public NPCLocation m_spawnPoint;
    public Transform m_destination;
    public SpawnLookAtPoint m_initialSpawnLookAtPoint;
    public int m_spawnTurn = -1;
    public int m_respawnTime = -1;
    public int m_spawnScriptIndex = -1;
    public int m_despawnScriptIndex = -1;
    public int m_skinIndex;
    public int m_patternIndex;
    public int m_colorIndex;
    public Team m_team;
    public string m_actorNameOverride;
    public bool m_isPlayer;
    public ActivatableObject[] m_activationsOnDeath;
    public ActivatableObject[] m_activationsOnSpawn;
    public bool m_applyEffectOnNPC;
    public StandardActorEffectData m_effectOnNPC;
    public string[] m_tagsToApplyToActor;
    [HideInInspector]
    public ActorData m_actor;
    [HideInInspector]
    public int m_id = -1;

    private NPCSpawnerState m_state;

    public bool HasSpawned()
    {
        return m_state != NPCSpawnerState.NotYetSpawned;
    }

    public bool IsActive()
    {
        return m_state == NPCSpawnerState.Active;
    }

    public void OnActorDeath(ActorData actor)
    {
        if (m_id != actor.SpawnerId)
        {
            return;
        }

        foreach (ActivatableObject activatableObject in m_activationsOnDeath)
        {
            activatableObject.Activate();
        }
    }

    public void OnActorSpawn(ActorData actor)
    {
        if (m_id != actor.SpawnerId)
        {
            return;
        }

        foreach (ActivatableObject activatableObject in m_activationsOnSpawn)
        {
            activatableObject.Activate();
        }
    }

    public void SetupForResolve()
    {
        if (m_state != NPCSpawnerState.Active)
        {
            return;
        }

        ActorTurnSM component = m_actor.GetComponent<ActorTurnSM>();
        if ((bool)component)
        {
            component.OnMessage(TurnMessage.BEGIN_RESOLVE);
        }
    }

#if SERVER
    // custom, see also rogues OnTurnStart_PveSpawner
    public void OnTurnStart()
    {
        if (!NetworkServer.active)
        {
            return;
        }

        int currentTurn = GameFlowData.Get().CurrentTurn;
        switch (m_state)
        {
            case NPCSpawnerState.NotYetSpawned:
            {
                if (m_spawnTurn != -1 && currentTurn >= m_spawnTurn)
                {
                    Spawn();
                }

                break;
            }
            case NPCSpawnerState.Active:
            {
                if (m_actor != null
                    && m_actor.IsDead()
                    && m_respawnTime != -1
                    && !GameplayUtils.IsPlayerControlled(m_actor))
                {
                    m_state = NPCSpawnerState.WaitingToRespawn;
                }

                break;
            }
            case NPCSpawnerState.WaitingToRespawn:
            {
                if (m_actor != null
                    && currentTurn >= m_actor.LastDeathTurn + m_respawnTime)
                {
                    Respawn();
                }

                break;
            }
        }

        if (m_state == NPCSpawnerState.Active
            && m_actor != null
            && !GameFlowData.Get().GetPlayers().Contains(m_actor.gameObject))
        {
            m_actor.OnTurnStart();
            ActorTurnSM actorTurnSM = m_actor.GetComponent<ActorTurnSM>();
            if (actorTurnSM)
            {
                actorTurnSM.OnMessage(TurnMessage.TURN_START);
            }
        }
    }

    // custom
    public void OnScriptUpdate(int newScriptIndex)
    {
        if (!NetworkServer.active)
        {
            return;
        }

        if (m_state == NPCSpawnerState.NotYetSpawned
            && m_spawnScriptIndex != -1
            && newScriptIndex == m_spawnScriptIndex)
        {
            Spawn();
        }
        else if (m_state != NPCSpawnerState.Inactive
                 && m_despawnScriptIndex != -1
                 && newScriptIndex == m_despawnScriptIndex)
        {
            Despawn();
        }
    }

    // custom
    public void OnActorAddedToGameFlowData(ActorData actor)
    {
        if (!NetworkServer.active || actor != m_actor || !m_applyEffectOnNPC)
        {
            return;
        }

        if (m_effectOnNPC == null)
        {
            Log.Error($"NPCSpawner \"{m_spawnerTitle}\" effect is null");
            return;
        }

        Ability startupAbility = NPCCoordinator.Get().m_startupAbility;
        if (startupAbility == null)
        {
            Log.Warning($"NPCSpawner \"{m_spawnerTitle}\" startup ability is null");
            return;
        }

        StandardActorEffect effect = new StandardActorEffect(
            startupAbility.AsEffectSource() ?? new EffectSource(startupAbility),
            m_actor.GetCurrentBoardSquare(),
            m_actor,
            m_actor,
            m_effectOnNPC);
        ServerEffectManager.Get().ApplyEffect(effect);
    }
    
    // rogues, from PveNpcDefinition
    private CharacterVisualInfo CreateVisualInfo()
    {
        CharacterVisualInfo characterVisualInfo = new CharacterVisualInfo
        {
            skinIndex = m_skinIndex,
            patternIndex = m_patternIndex,
            colorIndex = m_colorIndex
        };
        if (m_characterResourceLink != null
            && m_characterResourceLink.IsVisualInfoSelectionValid(characterVisualInfo))
        {
            return characterVisualInfo;
        }
        
        characterVisualInfo.ResetToDefault();
        return characterVisualInfo;
    }
    
    // custom, based on rogues InstaniateNPC
    public void Spawn()
    {
        if (!NetworkServer.active)
        {
            return;
        }

        if (m_actorPrefab == null)
        {
            Log.Error($"NPCSpawner \"{m_spawnerTitle}\" has no actor prefab");
            return;
        }

        if (m_spawnPoint == null)
        {
            Log.Error($"NPCSpawner \"{m_spawnerTitle}\" has no spawn point");
        }

        BoardSquare spawnSquare = NPCUtils.FindOpenSquareToSpawnOn(
            m_spawnPoint != null
                ? m_spawnPoint.boardSquare
                : null,
            m_actorPrefab.transform);
        if (spawnSquare == null)
        {
            Log.Error($"NPCSpawner \"{m_spawnerTitle}\" failed to find a square to spawn on");
            return;
        }

        Vector3 spawnPos = spawnSquare.GetOccupantRefPos();
        GameObject spawnedActor = Object.Instantiate(m_actorPrefab, spawnPos, Quaternion.identity);
        if (spawnedActor == null)
        {
            throw new ApplicationException("Failed to instantiate NPC " + m_actorPrefab + "!");
        }

        BotController botController = spawnedActor.GetComponent<BotController>();
        if (botController == null)
        {
            botController = spawnedActor.AddComponent<BotController>();
        }
        if (m_actorBrain != null)
        {
            NPCBrain brain = m_actorBrain.Create(botController, m_destination);
            if (brain != null)
            {
                botController.PushBrain(brain);
            }
        }

        m_actor = spawnedActor.GetComponent<ActorData>();
        if (m_actor == null)
        {
            throw new ApplicationException("No ActorData component found on " + spawnedActor.name);
        }
        if (m_isPlayer)
        {
            m_actor.PlayerIndex = NPCCoordinator.Get().m_nextPlayerIndex++;
        }
        m_actor.SpawnerId = m_id;
        m_actor.HasBotController = true;
        foreach (string tag in m_tagsToApplyToActor)
        {
            if (!tag.IsNullOrEmpty())
            {
                m_actor.AddTag(tag);
            }
        }

        m_actor.m_visualInfo = CreateVisualInfo();
        PrefabResourceLink heroPrefabLinkFromSelection = m_characterResourceLink.GetHeroPrefabLinkFromSelection(
            m_actor.m_visualInfo,
            out CharacterSkin _);
        m_actor.Initialize(heroPrefabLinkFromSelection, false);
        if (!m_actorNameOverride.IsNullOrEmpty())
        {
            m_actor.m_displayName = m_actorNameOverride;
        }
        m_actor.InitActorNetworkVisibilityObjects();
        PassiveData passiveData = m_actor.GetPassiveData();
        if (passiveData)
        {
            passiveData.Initialize();
        }
        
        m_actor.TeleportToBoardSquare(
            spawnSquare,
            GetSpawnFacing(spawnPos),
            ActorData.TeleportType.InitialSpawn,
            null);
        
        m_actor.SetTeam(m_team);
        m_state = NPCSpawnerState.Active;
        NetworkServer.Spawn(spawnedActor);
    }

    // custom, see rogues OnTurnStart_PveSpawner
    private void Respawn()
    {
        if (m_spawnPoint == null)
        {
            Log.Error($"NPCSpawner \"{m_spawnerTitle}\" has no spawn point");
        }
        
        BoardSquare spawnSquare = NPCUtils.FindOpenSquareToSpawnOn(
            m_spawnPoint != null ? m_spawnPoint.boardSquare : null,
            m_actor.transform);
        m_actor.GetComponent<ServerActorController>().RespawnOnSquare(spawnSquare);
        m_state = NPCSpawnerState.Active;
    }

    // custom, see rogues OnTurnEnd_PveSpawner
    public void Despawn()
    {
        if (!NetworkServer.active)
        {
            return;
        }

        if (m_actor != null)
        {
            GameplayUtils.DestroyActor(m_actor.gameObject);
            m_actor = null;
        }
        
        m_state = NPCSpawnerState.Inactive;
    }

    // copied from SpawnPointManager
    private Vector3 GetSpawnFacing(Vector3 spawnPosition)
    {
        Vector3 result = new Vector3(1f, 0f, 0f);
        if (m_initialSpawnLookAtPoint != null)
        {
            Vector3 vector = m_initialSpawnLookAtPoint.transform.position - spawnPosition;
            vector.y = 0f;
            if (Mathf.Abs(vector.x) > Mathf.Abs(vector.z))
            {
                if (vector.x > 0f)
                {
                    result = new Vector3(1f, 0f, 0f);
                }
                else
                {
                    result = new Vector3(-1f, 0f, 0f);
                }
            }
            else if (vector.z > 0f)
            {
                result = new Vector3(0f, 0f, 1f);
            }
            else
            {
                result = new Vector3(0f, 0f, -1f);
            }
        }
        return result;
    }
#endif
}