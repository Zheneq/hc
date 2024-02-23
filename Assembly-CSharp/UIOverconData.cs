using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class UIOverconData : MonoBehaviour
{
	[Serializable]
	public class NameToOverconEntry
	{
		public int m_overconId;

		public bool m_isHidden;

		public string m_commandName;

		public string m_displayName;

		public string m_obtainedDescription;

		public int m_sortOrder;

		public int m_maxUsesPerMatch = 3;

		public InventoryItemRarity m_rarity;

		[AssetFileSelector("", "", "")]
		[Header("-- Static Sprite --")]
		public string m_staticSpritePath;

		public float m_initialAlpha = 1f;

		[AssetFileSelector("", "", "")]
		[Header("-- Icon Sprite --")]
		public string m_iconSpritePath;

		[AssetFileSelector("Assets/Prefabs/New GUI/V2/Resources/OverconPrefabs/", "OverconPrefabs/", ".prefab")]
		[Header("-- Custom Prfab for more fancy overcons --")]
		public string m_customPrefabPath;

		public float m_customPrefabHeightOffset;

		[AudioEvent(false)]
		[Header("-- Audio Event --")]
		public string m_audioEvent = string.Empty;

		[Header("-- if <= 0, will use default time (8 seconds) before destroying")]
		public float m_ageInSeconds = -1f;

		[Header("-- Unlock Data --")]
		public GameBalanceVars.UnlockData m_unlockDataForGeneratingLobbyData;

		public string GetUnlocalizedDisplayName()
		{
			if (string.IsNullOrEmpty(m_displayName))
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return m_commandName;
					}
				}
			}
			return m_displayName;
		}

		public string GetDisplayName()
		{
			string text = StringUtil.TR_GetOverconDisplayName(m_overconId);
			if (text.IsNullOrEmpty())
			{
				text = new StringBuilder().Append("#overcon").Append(m_overconId).ToString();
			}
			return text;
		}

		public string GetCommandName()
		{
			string text = StringUtil.TR_GetOverconCommandName(m_overconId);
			if (text.IsNullOrEmpty())
			{
				text = m_commandName;
			}
			return text;
		}

		public string GetObtainedDescription()
		{
			string text = StringUtil.TR_GetOverconObtainedDesc(m_overconId);
			if (text.IsNullOrEmpty())
			{
				text = m_obtainedDescription;
			}
			return text;
		}

		public GameBalanceVars.OverconUnlockData CreateUnlockDataEntry()
		{
			GameBalanceVars.OverconUnlockData overconUnlockData = new GameBalanceVars.OverconUnlockData();
			overconUnlockData.ID = m_overconId;
			overconUnlockData.Name = m_commandName;
			overconUnlockData.m_sortOrder = m_sortOrder;
			overconUnlockData.m_isHidden = m_isHidden;
			overconUnlockData.m_commandName = m_commandName;
			overconUnlockData.Rarity = m_rarity;
			overconUnlockData.m_unlockData = m_unlockDataForGeneratingLobbyData.Clone();
			return overconUnlockData;
		}
	}

	public const float c_defaultOverconAgeInSeconds = 8f;

	public List<NameToOverconEntry> m_nameToOverconEntry = new List<NameToOverconEntry>();

	private static UIOverconData s_instance;

	private Dictionary<int, NameToOverconEntry> m_cachedIdToOverconMap = new Dictionary<int, NameToOverconEntry>();

	private Dictionary<string, int> m_cachedNameToId = new Dictionary<string, int>();

	public static UIOverconData Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		m_cachedIdToOverconMap = new Dictionary<int, NameToOverconEntry>();
		m_cachedNameToId = new Dictionary<string, int>();
	}

	private void Start()
	{
		for (int i = 0; i < m_nameToOverconEntry.Count; i++)
		{
			NameToOverconEntry nameToOverconEntry = m_nameToOverconEntry[i];
			if (nameToOverconEntry.m_overconId <= 0 || m_cachedIdToOverconMap.ContainsKey(nameToOverconEntry.m_overconId))
			{
				continue;
			}
			m_cachedIdToOverconMap.Add(nameToOverconEntry.m_overconId, nameToOverconEntry);
			if (!m_cachedNameToId.ContainsKey(nameToOverconEntry.m_commandName))
			{
				m_cachedNameToId.Add(nameToOverconEntry.m_commandName.ToLower(), nameToOverconEntry.m_overconId);
				string commandName = nameToOverconEntry.GetCommandName();
				if (commandName.IsNullOrEmpty())
				{
					continue;
				}
				if (!m_cachedNameToId.ContainsKey(commandName.ToLower()))
				{
					m_cachedNameToId.Add(commandName.ToLower(), nameToOverconEntry.m_overconId);
				}
			}
			else
			{
				Log.Error("UIOverconData has duplicate overcon names");
			}
		}
		while (true)
		{
			if (ClientGameManager.Get() != null)
			{
				while (true)
				{
					ClientGameManager.Get().OnUseOverconNotification += HandleUseOverconNotification;
					return;
				}
			}
			return;
		}
	}

	private void OnDestroy()
	{
		if (!(s_instance == this))
		{
			return;
		}
		while (true)
		{
			if (ClientGameManager.Get() != null)
			{
				ClientGameManager.Get().OnUseOverconNotification -= HandleUseOverconNotification;
			}
			s_instance = null;
			return;
		}
	}

	public int GetOverconIdByName(string name)
	{
		string key = name.ToLower();
		if (m_cachedNameToId.ContainsKey(key))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return m_cachedNameToId[key];
				}
			}
		}
		return -1;
	}

	public NameToOverconEntry GetOverconEntryById(int overconId)
	{
		if (m_cachedIdToOverconMap.ContainsKey(overconId))
		{
			return m_cachedIdToOverconMap[overconId];
		}
		return null;
	}

	public void HandleUseOverconNotification(UseOverconResponse notification)
	{
		UseOvercon(notification.OverconId, notification.ActorId, false);
	}

	public void UseOvercon(int overconId, int actorIndex, bool skipValidation)
	{
		if (!(GameFlowData.Get() != null))
		{
			return;
		}
		while (true)
		{
			if (!(GameFlowData.Get().activeOwnedActorData != null))
			{
				return;
			}
			object obj;
			if (GameFlowData.Get() != null)
			{
				obj = GameFlowData.Get().activeOwnedActorData;
			}
			else
			{
				obj = null;
			}
			ActorData x = (ActorData)obj;
			ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
			if (!(actorData != null))
			{
				return;
			}
			while (true)
			{
				if (!(x != null))
				{
					return;
				}
				while (true)
				{
					if (!actorData.IsActorVisibleToClient())
					{
						return;
					}
					while (true)
					{
						if (!(HUD_UI.Get() != null))
						{
							return;
						}
						while (true)
						{
							if (!(Get() != null))
							{
								return;
							}
							while (true)
							{
								NameToOverconEntry overconEntryById = Get().GetOverconEntryById(overconId);
								if (overconEntryById != null)
								{
									while (true)
									{
										HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.SpawnOverconForActor(actorData, overconEntryById, skipValidation);
										return;
									}
								}
								return;
							}
						}
					}
				}
			}
		}
	}
}
