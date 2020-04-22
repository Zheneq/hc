public static class CharacterTypeExtensions
{
	public static string GetNameForCharacterType(CharacterType characterType, string displayName)
	{
		if (characterType != CharacterType.BattleMonk && characterType != CharacterType.BazookaGirl)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (characterType != CharacterType.Blaster && characterType != CharacterType.Claymore)
			{
				while (true)
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
					while (true)
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
						while (true)
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
							while (true)
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
								while (true)
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
									while (true)
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
										while (true)
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
											while (true)
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
												while (true)
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
		int result;
		if (characterType > CharacterType.None)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!characterType.IsWillFill() && characterType != CharacterType.FemaleWillFill)
			{
				while (true)
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
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					result = ((characterType < CharacterType.Last) ? 1 : 0);
					goto IL_0047;
				}
			}
		}
		result = 0;
		goto IL_0047;
		IL_0047:
		return (byte)result != 0;
	}

	public static bool IsValidForHumanPreGameSelection(this CharacterType characterType)
	{
		int result;
		if (characterType > CharacterType.None && characterType != CharacterType.PunchingDummy)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (characterType < CharacterType.Last)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				result = ((characterType != CharacterType.FemaleWillFill) ? 1 : 0);
				goto IL_0036;
			}
		}
		result = 0;
		goto IL_0036;
		IL_0036:
		return (byte)result != 0;
	}

	public static bool IsWillFill(this CharacterType characterType)
	{
		return characterType == CharacterType.PendingWillFill;
	}
}
