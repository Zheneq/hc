public static class GameWideDataExtensions
{
	public static string GetDisplayName(this CharacterType characterType)
	{
		return GameWideData.Get().GetCharacterDisplayName(characterType);
	}
}
