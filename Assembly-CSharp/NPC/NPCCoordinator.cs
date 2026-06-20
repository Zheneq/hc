using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NPCCoordinator : MonoBehaviour
{
	public enum LoadingStateEnum
	{
		WaitingToLoad,
		Loading,
		Done
	}

	private static NPCCoordinator s_instance;

	public NPCSpawner[] m_spawners;
	public Ability m_startupAbility;
	[HideInInspector]
	public int m_nextPlayerIndex = 100;

	public LoadingStateEnum LoadingState { get; private set; }
	public static NPCCoordinator Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		for (int i = 0; i < m_spawners.Length; i++)
		{
			m_spawners[i].m_id = i;
		}
		LoadingState = LoadingStateEnum.WaitingToLoad;
		LoadNPCs();
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	public void SetupForResolve()
	{
		foreach (NPCSpawner nPCSpawner in m_spawners)
		{
			nPCSpawner.SetupForResolve();
		}
	}

	public static bool IsSpawnedNPC(ActorData actor)
	{
		if (s_instance == null)
		{
			return false;
		}
		if (actor != null)
		{
			foreach (NPCSpawner nPCSpawner in s_instance.m_spawners)
			{
				if (nPCSpawner.m_id == actor.SpawnerId)
				{
					return true;
				}
			}
		}
		return false;
	}

	public void AddSpawner(NPCSpawner spawner)
	{
		spawner.m_id = m_spawners.Length;
		Array.Resize(ref m_spawners, m_spawners.Length + 1);
		m_spawners[m_spawners.Length - 1] = spawner;
	}

	public void OnActorDeath(ActorData actor)
	{
		foreach (NPCSpawner nPCSpawner in m_spawners)
		{
			if (nPCSpawner.m_id == actor.SpawnerId)
			{
				nPCSpawner.OnActorDeath(actor);
			}
		}
	}

	public void OnActorSpawn(ActorData actor)
	{
		foreach (NPCSpawner nPCSpawner in m_spawners)
		{
			if (nPCSpawner.m_id == actor.SpawnerId)
			{
				nPCSpawner.OnActorSpawn(actor);
			}
		}
	}

	public List<CharacterResourceLink> GetActorCharacterResourceLinks()
	{
		List<CharacterResourceLink> list = new List<CharacterResourceLink>();
		foreach (NPCSpawner nPCSpawner in m_spawners)
		{
			if (nPCSpawner != null
				&& nPCSpawner.m_actorPrefab != null
				&& nPCSpawner.m_characterResourceLink != null)
			{
				list.Add(nPCSpawner.m_characterResourceLink);
			}
		}
		return list;
	}

	public CharacterResourceLink GetNpcCharacterResourceLinkBySpawnerId(int spawnerId)
	{
		foreach (NPCSpawner nPCSpawner in m_spawners)
		{
			if (nPCSpawner != null
				&& nPCSpawner.m_id == spawnerId
				&& nPCSpawner.m_characterResourceLink != null)
			{
				return nPCSpawner.m_characterResourceLink;
			}
		}
		return null;
	}

	public void LoadNPCs()
	{
		int num = 0;
		foreach (NPCSpawner nPCSpawner in Get().m_spawners)
		{
			if (nPCSpawner != null && nPCSpawner.m_actorPrefab != null && nPCSpawner.m_characterResourceLink != null)
			{
				num++;
				CharacterVisualInfo linkVisualInfo = new CharacterVisualInfo(nPCSpawner.m_skinIndex, nPCSpawner.m_patternIndex, nPCSpawner.m_colorIndex);
				if (!NetworkServer.active && ClientGameManager.Get() != null)
				{
					ClientGameManager.Get().LoadCharacterResourceLink(nPCSpawner.m_characterResourceLink, linkVisualInfo);
				}
			}
		}
		if (num > 0)
		{
			LoadingState = LoadingStateEnum.Loading;
		}
		else
		{
			LoadingState = LoadingStateEnum.Done;
		}
	}

	private void Update()
	{
		if (LoadingState != LoadingStateEnum.Loading)
		{
			return;
		}
		bool isServerDoneLoading = true;
		bool isClientDoneLoading = true;
		if (!NetworkServer.active && ClientGameManager.Get() != null)
		{
			isClientDoneLoading = ClientGameManager.Get().NumCharacterResourcesCurrentlyLoading == 0;
		}
		if (isServerDoneLoading && isClientDoneLoading)
		{
			LoadingState = LoadingStateEnum.Done;
		}
	}
	
#if SERVER
	// TODO NPC custom placeholder code
	public void OnTurnStart()
	{

	}
#endif
}
