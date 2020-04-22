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

	public LoadingStateEnum LoadingState
	{
		get;
		private set;
	}

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
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			LoadingState = LoadingStateEnum.WaitingToLoad;
			LoadNPCs();
			return;
		}
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	public void SetupForResolve()
	{
		NPCSpawner[] spawners = m_spawners;
		foreach (NPCSpawner nPCSpawner in spawners)
		{
			nPCSpawner.SetupForResolve();
		}
		while (true)
		{
			switch (6)
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

	public static bool IsSpawnedNPC(ActorData actor)
	{
		if (s_instance == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return false;
				}
			}
		}
		bool result = false;
		if (actor != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			NPCSpawner[] spawners = s_instance.m_spawners;
			int num = 0;
			while (true)
			{
				if (num < spawners.Length)
				{
					NPCSpawner nPCSpawner = spawners[num];
					if (nPCSpawner.m_id == actor.SpawnerId)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						result = true;
						break;
					}
					num++;
					continue;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				break;
			}
		}
		return result;
	}

	public void AddSpawner(NPCSpawner spawner)
	{
		spawner.m_id = m_spawners.Length;
		Array.Resize(ref m_spawners, m_spawners.Length + 1);
		m_spawners[m_spawners.Length - 1] = spawner;
	}

	public void OnActorDeath(ActorData actor)
	{
		NPCSpawner[] spawners = m_spawners;
		foreach (NPCSpawner nPCSpawner in spawners)
		{
			if (nPCSpawner.m_id == actor.SpawnerId)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				nPCSpawner.OnActorDeath(actor);
			}
		}
		while (true)
		{
			switch (1)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public void OnActorSpawn(ActorData actor)
	{
		NPCSpawner[] spawners = m_spawners;
		foreach (NPCSpawner nPCSpawner in spawners)
		{
			if (nPCSpawner.m_id == actor.SpawnerId)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				nPCSpawner.OnActorSpawn(actor);
			}
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

	public List<CharacterResourceLink> GetActorCharacterResourceLinks()
	{
		List<CharacterResourceLink> list = new List<CharacterResourceLink>();
		NPCSpawner[] spawners = m_spawners;
		foreach (NPCSpawner nPCSpawner in spawners)
		{
			if (nPCSpawner != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (nPCSpawner.m_actorPrefab != null && nPCSpawner.m_characterResourceLink != null)
				{
					list.Add(nPCSpawner.m_characterResourceLink);
				}
			}
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			return list;
		}
	}

	public CharacterResourceLink GetNpcCharacterResourceLinkBySpawnerId(int spawnerId)
	{
		for (int i = 0; i < m_spawners.Length; i++)
		{
			NPCSpawner nPCSpawner = m_spawners[i];
			if (nPCSpawner == null)
			{
				continue;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (nPCSpawner.m_id != spawnerId)
			{
				continue;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!(nPCSpawner.m_characterResourceLink != null))
			{
				continue;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				return nPCSpawner.m_characterResourceLink;
			}
		}
		return null;
	}

	public void LoadNPCs()
	{
		int num = 0;
		NPCSpawner[] spawners = Get().m_spawners;
		foreach (NPCSpawner nPCSpawner in spawners)
		{
			if (nPCSpawner == null)
			{
				continue;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (nPCSpawner.m_actorPrefab != null && nPCSpawner.m_characterResourceLink != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				num++;
				CharacterResourceLink characterResourceLink = nPCSpawner.m_characterResourceLink;
				CharacterVisualInfo linkVisualInfo = new CharacterVisualInfo(nPCSpawner.m_skinIndex, nPCSpawner.m_patternIndex, nPCSpawner.m_colorIndex);
				if (!NetworkServer.active && ClientGameManager.Get() != null)
				{
					ClientGameManager.Get().LoadCharacterResourceLink(characterResourceLink, linkVisualInfo);
				}
			}
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (num > 0)
			{
				LoadingState = LoadingStateEnum.Loading;
			}
			else
			{
				LoadingState = LoadingStateEnum.Done;
			}
			return;
		}
	}

	private void Update()
	{
		if (LoadingState != LoadingStateEnum.Loading)
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
			bool flag = true;
			bool flag2 = true;
			if (!NetworkServer.active)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (ClientGameManager.Get() != null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (ClientGameManager.Get().NumCharacterResourcesCurrentlyLoading == 0)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						flag2 = true;
					}
					else
					{
						flag2 = false;
					}
				}
			}
			if (!flag)
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				if (flag2)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						LoadingState = LoadingStateEnum.Done;
						return;
					}
				}
				return;
			}
		}
	}
}
