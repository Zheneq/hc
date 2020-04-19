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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NPCSpawner.OnActorDeath(ActorData)).MethodHandle;
			}
			foreach (ActivatableObject activatableObject in this.m_activationsOnDeath)
			{
				activatableObject.Activate();
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NPCSpawner.OnActorSpawn(ActorData)).MethodHandle;
			}
		}
	}

	public void SetupForResolve()
	{
		if (this.m_state == NPCSpawner.NPCSpawnerState.Active)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NPCSpawner.SetupForResolve()).MethodHandle;
			}
			ActorTurnSM component = this.m_actor.GetComponent<ActorTurnSM>();
			if (component)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
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
