using System;
using System.Collections.Generic;
using System.Linq;

internal class LobbyGameplayFreelancerSetQueryInterface : IFreelancerSetQueryInterface
{
	private LobbyGameplayData m_lgd;

	internal LobbyGameplayFreelancerSetQueryInterface(LobbyGameplayData lgd)
	{
		this.m_lgd = lgd;
	}

	public HashSet<CharacterType> GetCharacterTypesFromRoles(List<CharacterRole> roles)
	{
		HashSet<CharacterType> hashSet = new HashSet<CharacterType>();
		using (Dictionary<CharacterType, LobbyCharacterGameplayData>.Enumerator enumerator = this.m_lgd.CharacterData.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<CharacterType, LobbyCharacterGameplayData> keyValuePair = enumerator.Current;
				if (!roles.IsNullOrEmpty<CharacterRole>())
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(LobbyGameplayFreelancerSetQueryInterface.GetCharacterTypesFromRoles(List<CharacterRole>)).MethodHandle;
					}
					if (roles.Contains(keyValuePair.Value.CharacterRole))
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
						hashSet.Add(keyValuePair.Key);
					}
				}
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return hashSet;
	}

	public bool DoesCharacterMatchRoles(CharacterType freelancer, List<CharacterRole> roles)
	{
		if (roles.IsNullOrEmpty<CharacterRole>())
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LobbyGameplayFreelancerSetQueryInterface.DoesCharacterMatchRoles(CharacterType, List<CharacterRole>)).MethodHandle;
			}
			return false;
		}
		LobbyCharacterGameplayData lobbyCharacterGameplayData;
		return this.m_lgd.CharacterData.TryGetValue(freelancer, out lobbyCharacterGameplayData) && roles.Contains(lobbyCharacterGameplayData.CharacterRole);
	}

	public HashSet<CharacterType> GetCharacterTypesFromFractionGroupIds(List<int> groupIds)
	{
		HashSet<CharacterType> retVal = new HashSet<CharacterType>();
		(from p in this.m_lgd.FactionData.m_factionGroups
		where groupIds.Contains(p.FactionGroupID)
		select p).ToList<FactionGroup>().ForEach(delegate(FactionGroup p)
		{
			retVal.UnionWith(p.Characters);
		});
		return retVal;
	}

	public bool DoesCharacterMatchFractionGroupIds(CharacterType freelancer, List<int> groupIds)
	{
		return this.m_lgd.FactionData.m_factionGroups.Exists(delegate(FactionGroup p)
		{
			bool result;
			if (groupIds.Contains(p.FactionGroupID))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LobbyGameplayFreelancerSetQueryInterface.<DoesCharacterMatchFractionGroupIds>c__AnonStorey1.<>m__0(FactionGroup)).MethodHandle;
				}
				result = p.Characters.Contains(freelancer);
			}
			else
			{
				result = false;
			}
			return result;
		});
	}
}
