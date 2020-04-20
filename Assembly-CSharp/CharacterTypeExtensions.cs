using System;

public static class CharacterTypeExtensions
{
	public static string GetNameForCharacterType(CharacterType characterType, string displayName)
	{
		if (characterType != CharacterType.BattleMonk && characterType != CharacterType.BazookaGirl)
		{
			if (characterType != CharacterType.Blaster && characterType != CharacterType.Claymore)
			{
				if (characterType != CharacterType.DigitalSorceress)
				{
					if (characterType != CharacterType.Gremlins)
					{
						if (characterType != CharacterType.NanoSmith)
						{
							if (characterType != CharacterType.RageBeast)
							{
								if (characterType != CharacterType.RobotAnimal)
								{
									if (characterType != CharacterType.Scoundrel && characterType != CharacterType.Sniper)
									{
										if (characterType != CharacterType.SpaceMarine)
										{
											if (characterType != CharacterType.Spark && characterType != CharacterType.Tracker)
											{
												if (characterType != CharacterType.Trickster)
												{
													return characterType.ToString();
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		return displayName;
	}

	public static bool IsValidForHumanGameplay(this CharacterType characterType)
	{
		if (characterType > CharacterType.None)
		{
			if (!characterType.IsWillFill() && characterType != CharacterType.FemaleWillFill)
			{
				if (characterType != CharacterType.PunchingDummy)
				{
					return characterType < CharacterType.Last;
				}
			}
		}
		return false;
	}

	public static bool IsValidForHumanPreGameSelection(this CharacterType characterType)
	{
		if (characterType > CharacterType.None && characterType != CharacterType.PunchingDummy)
		{
			if (characterType < CharacterType.Last)
			{
				return characterType != CharacterType.FemaleWillFill;
			}
		}
		return false;
	}

	public static bool IsWillFill(this CharacterType characterType)
	{
		return characterType == CharacterType.PendingWillFill;
	}
}
