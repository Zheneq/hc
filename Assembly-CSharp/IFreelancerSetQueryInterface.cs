using System.Collections.Generic;

public interface IFreelancerSetQueryInterface
{
	HashSet<CharacterType> GetCharacterTypesFromRoles(List<CharacterRole> roles);

	HashSet<CharacterType> GetCharacterTypesFromFractionGroupIds(List<int> groupIds);

	bool DoesCharacterMatchRoles(CharacterType freelancer, List<CharacterRole> roles);

	bool DoesCharacterMatchFractionGroupIds(CharacterType freelancer, List<int> groupIds);
}
