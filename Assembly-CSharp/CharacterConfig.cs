using System;
using System.Collections.Generic;

[Serializable]
public class CharacterConfig
{
	public CharacterType CharacterType;

	public CharacterRole CharacterRole;

	public bool AllowForBots;

	public bool AllowForPlayers;

	public bool IsHidden;

	public DateTime IsHiddenFromFreeRotationUntil;

	public List<GameType> GameTypesProhibitedFrom;

	public int Difficulty;

	public CharacterConfig Clone()
	{
		return (CharacterConfig)MemberwiseClone();
	}
}
