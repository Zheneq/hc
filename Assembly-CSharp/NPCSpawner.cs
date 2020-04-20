using System;
using UnityEngine;

[Serializable]
public class NPCSpawner
{
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

	private NPCSpawner.NPCSpawnerState m_state;

	public bool HasSpawned()
	{
		return this.m_state != NPCSpawner.NPCSpawnerState.NotYetSpawned;
	}

	public bool IsActive()
	{
		return this.m_state == NPCSpawner.NPCSpawnerState.Active;
	}

	public void OnActorDeath(ActorData actor)
	{
		if (this.m_id == actor.SpawnerId)
		{
			foreach (ActivatableObject activatableObject in this.m_activationsOnDeath)
			{
				activatableObject.Activate();
			}
		}
	}

	public void OnActorSpawn(ActorData actor)
	{
		if (this.m_id == actor.SpawnerId)
		{
			foreach (ActivatableObject activatableObject in this.m_activationsOnSpawn)
			{
				activatableObject.Activate();
			}
		}
	}

	public void SetupForResolve()
	{
		if (this.m_state == NPCSpawner.NPCSpawnerState.Active)
		{
			ActorTurnSM component = this.m_actor.GetComponent<ActorTurnSM>();
			if (component)
			{
				component.OnMessage(TurnMessage.BEGIN_RESOLVE, true);
			}
		}
	}

	private enum NPCSpawnerState
	{
		NotYetSpawned,
		Active,
		WaitingToRespawn,
		Inactive
	}
}
