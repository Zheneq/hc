using System;
using System.Collections.Generic;

[Serializable]
public class FreelancerSet
{
	public List<CharacterType> Types;

	public List<CharacterRole> Roles;

	public List<int> FactionGroups;

	public bool IsCharacterAllowed(CharacterType freelancer, IFreelancerSetQueryInterface qi)
	{
		if (Types != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return Types.Contains(freelancer);
				}
			}
		}
		if (Roles != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return qi.DoesCharacterMatchRoles(freelancer, Roles);
				}
			}
		}
		if (FactionGroups != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return qi.DoesCharacterMatchFractionGroupIds(freelancer, FactionGroups);
				}
			}
		}
		throw new Exception("Bad FreelancerSet definition");
	}

	public HashSet<CharacterType> GetAllowedCharacters(IFreelancerSetQueryInterface qi)
	{
		if (Types != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return new HashSet<CharacterType>(Types);
				}
			}
		}
		if (Roles != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return qi.GetCharacterTypesFromRoles(Roles);
				}
			}
		}
		if (FactionGroups != null)
		{
			return qi.GetCharacterTypesFromFractionGroupIds(FactionGroups);
		}
		throw new Exception("Bad FreelancerSet definition");
	}

	internal void ValidateSelf(LobbyGameConfig gameConfig, string subTypeName)
	{
		int num = 0;
		int num2 = num;
		int num3;
		if (Types.IsNullOrEmpty())
		{
			num3 = 0;
		}
		else
		{
			num3 = 1;
		}
		num = num2 + num3;
		int num4 = num;
		int num5;
		if (Roles.IsNullOrEmpty())
		{
			num5 = 0;
		}
		else
		{
			num5 = 1;
		}
		num = num4 + num5;
		num += ((!FactionGroups.IsNullOrEmpty()) ? 1 : 0);
		if (num == 1)
		{
			return;
		}
		while (true)
		{
			throw new Exception($"The {gameConfig.GameType} sub type {subTypeName} has been poorly configured; each TeamComposition Rule should have only one of Types, Roles, or FactionGroups.");
		}
	}
}
