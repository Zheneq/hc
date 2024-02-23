using System.Collections.Generic;
using System.Linq;

internal class LobbyGameplayFreelancerSetQueryInterface : IFreelancerSetQueryInterface
{
	private LobbyGameplayData m_lgd;

	internal LobbyGameplayFreelancerSetQueryInterface(LobbyGameplayData lgd)
	{
		m_lgd = lgd;
	}

	public HashSet<CharacterType> GetCharacterTypesFromRoles(List<CharacterRole> roles)
	{
		HashSet<CharacterType> hashSet = new HashSet<CharacterType>();
		using (Dictionary<CharacterType, LobbyCharacterGameplayData>.Enumerator enumerator = m_lgd.CharacterData.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				System.Collections.Generic.KeyValuePair<CharacterType, LobbyCharacterGameplayData> current = enumerator.Current;
				if (!roles.IsNullOrEmpty())
				{
					if (roles.Contains(current.Value.CharacterRole))
					{
						hashSet.Add(current.Key);
					}
				}
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return hashSet;
				}
			}
		}
	}

	public bool DoesCharacterMatchRoles(CharacterType freelancer, List<CharacterRole> roles)
	{
		if (roles.IsNullOrEmpty())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}

		LobbyCharacterGameplayData value;
		if (!m_lgd.CharacterData.TryGetValue(freelancer, out value))
		{
			return false;
		}
		return roles.Contains(value.CharacterRole);
	}

	public HashSet<CharacterType> GetCharacterTypesFromFractionGroupIds(List<int> groupIds)
	{
		HashSet<CharacterType> retVal = new HashSet<CharacterType>();
		m_lgd.FactionData.m_factionGroups.Where((FactionGroup p) => groupIds.Contains(p.FactionGroupID)).ToList().ForEach(delegate(FactionGroup p)
		{
			retVal.UnionWith(p.Characters);
		});
		return retVal;
	}

	public bool DoesCharacterMatchFractionGroupIds(CharacterType freelancer, List<int> groupIds)
	{
		return m_lgd.FactionData.m_factionGroups.Exists(delegate(FactionGroup p)
		{
			int result;
			if (groupIds.Contains(p.FactionGroupID))
			{
				result = (p.Characters.Contains(freelancer) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		});
	}
}
