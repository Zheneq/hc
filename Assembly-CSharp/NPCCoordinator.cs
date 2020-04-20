using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NPCCoordinator : MonoBehaviour
{
	private static NPCCoordinator s_instance;

	public NPCSpawner[] m_spawners;

	public Ability m_startupAbility;

	[HideInInspector]
	public int m_nextPlayerIndex = 0x64;

	public static NPCCoordinator Get()
	{
		return NPCCoordinator.s_instance;
	}

	private void Awake()
	{
		NPCCoordinator.s_instance = this;
		for (int i = 0; i < this.m_spawners.Length; i++)
		{
			this.m_spawners[i].m_id = i;
		}
		this.LoadingState = NPCCoordinator.LoadingStateEnum.WaitingToLoad;
		this.LoadNPCs();
	}

	private void OnDestroy()
	{
		NPCCoordinator.s_instance = null;
	}

	public void SetupForResolve()
	{
		foreach (NPCSpawner npcspawner in this.m_spawners)
		{
			npcspawner.SetupForResolve();
		}
	}

	public static bool IsSpawnedNPC(ActorData actor)
	{
		if (NPCCoordinator.s_instance == null)
		{
			return false;
		}
		bool result = false;
		if (actor != null)
		{
			foreach (NPCSpawner npcspawner in NPCCoordinator.s_instance.m_spawners)
			{
				if (npcspawner.m_id == actor.SpawnerId)
				{
					return true;
				}
			}
		}
		return result;
	}

	public void AddSpawner(NPCSpawner spawner)
	{
		spawner.m_id = this.m_spawners.Length;
		Array.Resize<NPCSpawner>(ref this.m_spawners, this.m_spawners.Length + 1);
		this.m_spawners[this.m_spawners.Length - 1] = spawner;
	}

	public void OnActorDeath(ActorData actor)
	{
		foreach (NPCSpawner npcspawner in this.m_spawners)
		{
			if (npcspawner.m_id == actor.SpawnerId)
			{
				npcspawner.OnActorDeath(actor);
			}
		}
	}

	public void OnActorSpawn(ActorData actor)
	{
		foreach (NPCSpawner npcspawner in this.m_spawners)
		{
			if (npcspawner.m_id == actor.SpawnerId)
			{
				npcspawner.OnActorSpawn(actor);
			}
		}
	}

	public List<CharacterResourceLink> GetActorCharacterResourceLinks()
	{
		List<CharacterResourceLink> list = new List<CharacterResourceLink>();
		foreach (NPCSpawner npcspawner in this.m_spawners)
		{
			if (npcspawner != null)
			{
				if (npcspawner.m_actorPrefab != null && npcspawner.m_characterResourceLink != null)
				{
					list.Add(npcspawner.m_characterResourceLink);
				}
			}
		}
		return list;
	}

	public CharacterResourceLink GetNpcCharacterResourceLinkBySpawnerId(int spawnerId)
	{
		for (int i = 0; i < this.m_spawners.Length; i++)
		{
			NPCSpawner npcspawner = this.m_spawners[i];
			if (npcspawner != null)
			{
				if (npcspawner.m_id == spawnerId)
				{
					if (npcspawner.m_characterResourceLink != null)
					{
						return npcspawner.m_characterResourceLink;
					}
				}
			}
		}
		return null;
	}

	public NPCCoordinator.LoadingStateEnum LoadingState { get; private set; }

	public void LoadNPCs()
	{
		int num = 0;
		foreach (NPCSpawner npcspawner in NPCCoordinator.Get().m_spawners)
		{
			if (npcspawner != null)
			{
				if (npcspawner.m_actorPrefab != null && npcspawner.m_characterResourceLink != null)
				{
					num++;
					CharacterResourceLink characterResourceLink = npcspawner.m_characterResourceLink;
					CharacterVisualInfo linkVisualInfo = new CharacterVisualInfo(npcspawner.m_skinIndex, npcspawner.m_patternIndex, npcspawner.m_colorIndex);
					if (!NetworkServer.active && ClientGameManager.Get() != null)
					{
						ClientGameManager.Get().LoadCharacterResourceLink(characterResourceLink, linkVisualInfo);
					}
				}
			}
		}
		if (num > 0)
		{
			this.LoadingState = NPCCoordinator.LoadingStateEnum.Loading;
		}
		else
		{
			this.LoadingState = NPCCoordinator.LoadingStateEnum.Done;
		}
	}

	private void Update()
	{
		if (this.LoadingState == NPCCoordinator.LoadingStateEnum.Loading)
		{
			bool flag = true;
			bool flag2 = true;
			if (!NetworkServer.active)
			{
				if (ClientGameManager.Get() != null)
				{
					if (ClientGameManager.Get().NumCharacterResourcesCurrentlyLoading == 0)
					{
						flag2 = true;
					}
					else
					{
						flag2 = false;
					}
				}
			}
			if (flag)
			{
				if (flag2)
				{
					this.LoadingState = NPCCoordinator.LoadingStateEnum.Done;
				}
			}
		}
	}

	public enum LoadingStateEnum
	{
		WaitingToLoad,
		Loading,
		Done
	}
}
