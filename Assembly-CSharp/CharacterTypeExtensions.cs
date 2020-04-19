using System;

public static class CharacterTypeExtensions
{
	public static string GetNameForCharacterType(CharacterType characterType, string displayName)
	{
		if (characterType != CharacterType.BattleMonk && characterType != CharacterType.BazookaGirl)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CharacterTypeExtensions.GetNameForCharacterType(CharacterType, string)).MethodHandle;
			}
			if (characterType != CharacterType.Blaster && characterType != CharacterType.Claymore)
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
				if (characterType != CharacterType.DigitalSorceress)
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
					if (characterType != CharacterType.Gremlins)
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
						if (characterType != CharacterType.NanoSmith)
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
							if (characterType != CharacterType.RageBeast)
							{
								for (;;)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								if (characterType != CharacterType.RobotAnimal)
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
									if (characterType != CharacterType.Scoundrel && characterType != CharacterType.Sniper)
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
										if (characterType != CharacterType.SpaceMarine)
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
											if (characterType != CharacterType.Spark && characterType != CharacterType.Tracker)
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CharacterType.IsValidForHumanGameplay()).MethodHandle;
			}
			if (!characterType.IsWillFill() && characterType != CharacterType.FemaleWillFill)
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
				if (characterType != CharacterType.PunchingDummy)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CharacterType.IsValidForHumanPreGameSelection()).MethodHandle;
			}
			if (characterType < CharacterType.Last)
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
