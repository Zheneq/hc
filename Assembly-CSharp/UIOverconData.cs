using System;
using System.Collections.Generic;
using LobbyGameClientMessages;
using UnityEngine;

public class UIOverconData : MonoBehaviour
{
	public const float c_defaultOverconAgeInSeconds = 8f;

	public List<UIOverconData.NameToOverconEntry> m_nameToOverconEntry = new List<UIOverconData.NameToOverconEntry>();

	private static UIOverconData s_instance;

	private Dictionary<int, UIOverconData.NameToOverconEntry> m_cachedIdToOverconMap = new Dictionary<int, UIOverconData.NameToOverconEntry>();

	private Dictionary<string, int> m_cachedNameToId = new Dictionary<string, int>();

	public static UIOverconData Get()
	{
		return UIOverconData.s_instance;
	}

	private void Awake()
	{
		UIOverconData.s_instance = this;
		this.m_cachedIdToOverconMap = new Dictionary<int, UIOverconData.NameToOverconEntry>();
		this.m_cachedNameToId = new Dictionary<string, int>();
	}

	private void Start()
	{
		for (int i = 0; i < this.m_nameToOverconEntry.Count; i++)
		{
			UIOverconData.NameToOverconEntry nameToOverconEntry = this.m_nameToOverconEntry[i];
			if (nameToOverconEntry.m_overconId > 0 && !this.m_cachedIdToOverconMap.ContainsKey(nameToOverconEntry.m_overconId))
			{
				this.m_cachedIdToOverconMap.Add(nameToOverconEntry.m_overconId, nameToOverconEntry);
				if (!this.m_cachedNameToId.ContainsKey(nameToOverconEntry.m_commandName))
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIOverconData.Start()).MethodHandle;
					}
					this.m_cachedNameToId.Add(nameToOverconEntry.m_commandName.ToLower(), nameToOverconEntry.m_overconId);
					string commandName = nameToOverconEntry.GetCommandName();
					if (!commandName.IsNullOrEmpty())
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!this.m_cachedNameToId.ContainsKey(commandName.ToLower()))
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
							this.m_cachedNameToId.Add(commandName.ToLower(), nameToOverconEntry.m_overconId);
						}
					}
				}
				else
				{
					Log.Error("UIOverconData has duplicate overcon names", new object[0]);
				}
			}
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
		if (ClientGameManager.Get() != null)
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
			ClientGameManager.Get().OnUseOverconNotification += this.HandleUseOverconNotification;
		}
	}

	private void OnDestroy()
	{
		if (UIOverconData.s_instance == this)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIOverconData.OnDestroy()).MethodHandle;
			}
			if (ClientGameManager.Get() != null)
			{
				ClientGameManager.Get().OnUseOverconNotification -= this.HandleUseOverconNotification;
			}
			UIOverconData.s_instance = null;
		}
	}

	public int GetOverconIdByName(string name)
	{
		string key = name.ToLower();
		if (this.m_cachedNameToId.ContainsKey(key))
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIOverconData.GetOverconIdByName(string)).MethodHandle;
			}
			return this.m_cachedNameToId[key];
		}
		return -1;
	}

	public UIOverconData.NameToOverconEntry GetOverconEntryById(int overconId)
	{
		if (this.m_cachedIdToOverconMap.ContainsKey(overconId))
		{
			return this.m_cachedIdToOverconMap[overconId];
		}
		return null;
	}

	public void HandleUseOverconNotification(UseOverconResponse notification)
	{
		this.UseOvercon(notification.OverconId, notification.ActorId, false);
	}

	public void UseOvercon(int overconId, int actorIndex, bool skipValidation)
	{
		if (GameFlowData.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIOverconData.UseOvercon(int, int, bool)).MethodHandle;
			}
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				ActorData actorData;
				if (GameFlowData.Get() != null)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					actorData = GameFlowData.Get().activeOwnedActorData;
				}
				else
				{
					actorData = null;
				}
				ActorData x = actorData;
				ActorData actorData2 = GameFlowData.Get().FindActorByActorIndex(actorIndex);
				if (actorData2 != null)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (x != null)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (actorData2.\u0018())
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							if (HUD_UI.Get() != null)
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
								if (UIOverconData.Get() != null)
								{
									for (;;)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
									UIOverconData.NameToOverconEntry overconEntryById = UIOverconData.Get().GetOverconEntryById(overconId);
									if (overconEntryById != null)
									{
										for (;;)
										{
											switch (4)
											{
											case 0:
												continue;
											}
											break;
										}
										HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.SpawnOverconForActor(actorData2, overconEntryById, skipValidation);
									}
								}
							}
						}
					}
				}
			}
		}
	}

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
			if (string.IsNullOrEmpty(this.m_displayName))
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIOverconData.NameToOverconEntry.GetUnlocalizedDisplayName()).MethodHandle;
				}
				return this.m_commandName;
			}
			return this.m_displayName;
		}

		public string GetDisplayName()
		{
			string text = StringUtil.TR_GetOverconDisplayName(this.m_overconId);
			if (text.IsNullOrEmpty())
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIOverconData.NameToOverconEntry.GetDisplayName()).MethodHandle;
				}
				text = string.Format("#overcon{0}", this.m_overconId);
			}
			return text;
		}

		public string GetCommandName()
		{
			string text = StringUtil.TR_GetOverconCommandName(this.m_overconId);
			if (text.IsNullOrEmpty())
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIOverconData.NameToOverconEntry.GetCommandName()).MethodHandle;
				}
				text = this.m_commandName;
			}
			return text;
		}

		public string GetObtainedDescription()
		{
			string text = StringUtil.TR_GetOverconObtainedDesc(this.m_overconId);
			if (text.IsNullOrEmpty())
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIOverconData.NameToOverconEntry.GetObtainedDescription()).MethodHandle;
				}
				text = this.m_obtainedDescription;
			}
			return text;
		}

		public GameBalanceVars.OverconUnlockData CreateUnlockDataEntry()
		{
			return new GameBalanceVars.OverconUnlockData
			{
				ID = this.m_overconId,
				Name = this.m_commandName,
				m_sortOrder = this.m_sortOrder,
				m_isHidden = this.m_isHidden,
				m_commandName = this.m_commandName,
				Rarity = this.m_rarity,
				m_unlockData = this.m_unlockDataForGeneratingLobbyData.Clone()
			};
		}
	}
}
