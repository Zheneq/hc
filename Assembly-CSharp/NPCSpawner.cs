using System;
using UnityEngine;

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
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ActivatableObject[] activationsOnDeath = m_activationsOnDeath;
			foreach (ActivatableObject activatableObject in activationsOnDeath)
			{
				activatableObject.Activate();
			}
			while (true)
			{
				switch (5)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public void OnActorSpawn(ActorData actor)
	{
		if (m_id != actor.SpawnerId)
		{
			return;
		}
		ActivatableObject[] activationsOnSpawn = m_activationsOnSpawn;
		foreach (ActivatableObject activatableObject in activationsOnSpawn)
		{
			activatableObject.Activate();
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return;
		}
	}

	public void SetupForResolve()
	{
		if (m_state != NPCSpawnerState.Active)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ActorTurnSM component = m_actor.GetComponent<ActorTurnSM>();
			if ((bool)component)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					component.OnMessage(TurnMessage.BEGIN_RESOLVE);
					return;
				}
			}
			return;
		}
	}
}
