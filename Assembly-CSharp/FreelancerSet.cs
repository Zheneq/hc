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
		if (this.Types != null)
		{
			return this.Types.Contains(freelancer);
		}
		if (this.Roles != null)
		{
			return qi.DoesCharacterMatchRoles(freelancer, this.Roles);
		}
		if (this.FactionGroups != null)
		{
			return qi.DoesCharacterMatchFractionGroupIds(freelancer, this.FactionGroups);
		}
		throw new Exception("Bad FreelancerSet definition");
	}

	public HashSet<CharacterType> GetAllowedCharacters(IFreelancerSetQueryInterface qi)
	{
		if (this.Types != null)
		{
			return new HashSet<CharacterType>(this.Types);
		}
		if (this.Roles != null)
		{
			return qi.GetCharacterTypesFromRoles(this.Roles);
		}
		if (this.FactionGroups != null)
		{
			return qi.GetCharacterTypesFromFractionGroupIds(this.FactionGroups);
		}
		throw new Exception("Bad FreelancerSet definition");
	}

	internal void ValidateSelf(LobbyGameConfig gameConfig, string subTypeName)
	{
		int num = 0;
		int num2 = num;
		int num3;
		if (this.Types.IsNullOrEmpty<CharacterType>())
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
		if (this.Roles.IsNullOrEmpty<CharacterRole>())
		{
			num5 = 0;
		}
		else
		{
			num5 = 1;
		}
		num = num4 + num5;
		num += ((!this.FactionGroups.IsNullOrEmpty<int>()) ? 1 : 0);
		if (num != 1)
		{
			throw new Exception(string.Format("The {0} sub type {1} has been poorly configured; each TeamComposition Rule should have only one of Types, Roles, or FactionGroups.", gameConfig.GameType, subTypeName));
		}
	}
}
