using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class TeamCompositionRules
{
	public enum SlotTypes
	{
		All,
		TeamA,
		TeamB,
		A1,
		A2,
		A3,
		A4,
		A5,
		B1,
		B2,
		B3,
		B4,
		B5,
		Slot1,
		Slot2,
		Slot3,
		Slot4,
		Slot5
	}

	public Dictionary<SlotTypes, FreelancerSet> Rules;

	private bool MatchesSlotType(SlotTypes st, Team team, int slot)
	{
		switch (st)
		{
		case SlotTypes.All:
			return true;
		case SlotTypes.TeamA:
			return team == Team.TeamA;
		case SlotTypes.TeamB:
			return team == Team.TeamB;
		case SlotTypes.Slot1:
			return slot == 1;
		case SlotTypes.Slot2:
			return slot == 2;
		case SlotTypes.Slot3:
			return slot == 3;
		case SlotTypes.Slot4:
			return slot == 4;
		case SlotTypes.Slot5:
			return slot == 5;
		case SlotTypes.A1:
		{
			int result7;
			if (team == Team.TeamA)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				result7 = ((slot == 1) ? 1 : 0);
			}
			else
			{
				result7 = 0;
			}
			return (byte)result7 != 0;
		}
		case SlotTypes.A2:
		{
			int result6;
			if (team == Team.TeamA)
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
				result6 = ((slot == 2) ? 1 : 0);
			}
			else
			{
				result6 = 0;
			}
			return (byte)result6 != 0;
		}
		case SlotTypes.A3:
		{
			int result2;
			if (team == Team.TeamA)
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
				result2 = ((slot == 3) ? 1 : 0);
			}
			else
			{
				result2 = 0;
			}
			return (byte)result2 != 0;
		}
		case SlotTypes.A4:
		{
			int result5;
			if (team == Team.TeamA)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				result5 = ((slot == 4) ? 1 : 0);
			}
			else
			{
				result5 = 0;
			}
			return (byte)result5 != 0;
		}
		case SlotTypes.A5:
		{
			int result3;
			if (team == Team.TeamA)
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
				result3 = ((slot == 5) ? 1 : 0);
			}
			else
			{
				result3 = 0;
			}
			return (byte)result3 != 0;
		}
		case SlotTypes.B1:
			return team == Team.TeamB && slot == 1;
		case SlotTypes.B2:
			return team == Team.TeamB && slot == 2;
		case SlotTypes.B3:
		{
			int result4;
			if (team == Team.TeamB)
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
				result4 = ((slot == 3) ? 1 : 0);
			}
			else
			{
				result4 = 0;
			}
			return (byte)result4 != 0;
		}
		case SlotTypes.B4:
			return team == Team.TeamB && slot == 4;
		case SlotTypes.B5:
		{
			int result;
			if (team == Team.TeamB)
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
				result = ((slot == 5) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}
		default:
			throw new Exception("Unimplemented slot type");
		}
	}

	public bool IsCharacterAllowed(CharacterType freelancer, IFreelancerSetQueryInterface qi)
	{
		int result;
		if (Rules != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = (Rules.Values.ToList().Exists((FreelancerSet p) => p.IsCharacterAllowed(freelancer, qi)) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public bool IsCharacterAllowedInSlot(CharacterType freelancer, Team team, int slot, IFreelancerSetQueryInterface qi)
	{
		using (Dictionary<SlotTypes, FreelancerSet>.Enumerator enumerator = Rules.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<SlotTypes, FreelancerSet> current = enumerator.Current;
				if (MatchesSlotType(current.Key, team, slot))
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							if (1 == 0)
							{
								/*OpCode not supported: LdMemberToken*/;
							}
							return current.Value.IsCharacterAllowed(freelancer, qi);
						}
					}
				}
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		throw new Exception($"There is no TeamComposition Rule to cover team {team} slot {slot}");
	}

	public List<CharacterType> GetAllowedFreelancers(IFreelancerSetQueryInterface qi)
	{
		HashSet<CharacterType> hashSet = new HashSet<CharacterType>();
		using (Dictionary<SlotTypes, FreelancerSet>.Enumerator enumerator = Rules.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				hashSet.UnionWith(enumerator.Current.Value.GetAllowedCharacters(qi));
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					goto end_IL_0014;
				}
			}
			end_IL_0014:;
		}
		return hashSet.ToList();
	}

	public LocalizationPayload GenerateFailure(IQueueRequirementApplicant applicant, IFreelancerSetQueryInterface qi)
	{
		List<CharacterType> allowedFreelancers = GetAllowedFreelancers(qi);
		if (allowedFreelancers.Count == 1)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return LocalizationPayload.Create("XMustHaveAccessToY", "Matchmaking", applicant.LocalizedHandle, LocalizationArg_Freelancer.Create(allowedFreelancers.First()));
				}
			}
		}
		if (allowedFreelancers.Count == 2)
		{
			return LocalizationPayload.Create("XMustHaveAccessToYOrZ", "Matchmaking", applicant.LocalizedHandle, LocalizationArg_Freelancer.Create(allowedFreelancers[0]), LocalizationArg_Freelancer.Create(allowedFreelancers[1]));
		}
		return LocalizationPayload.Create("XMustHaveAccessToYZOrOthers", "Matchmaking", applicant.LocalizedHandle, LocalizationArg_Freelancer.Create(allowedFreelancers[0]), LocalizationArg_Freelancer.Create(allowedFreelancers[1]));
	}

	public void ValidateSelf(IFreelancerSetQueryInterface qi, LobbyGameConfig gameConfig, FreelancerDuplicationRuleTypes resolvedDuplicationRule, string subTypeName)
	{
		Rules.Values.ToList().ForEach(delegate(FreelancerSet p)
		{
			p.ValidateSelf(gameConfig, subTypeName);
		});
		List<CharacterType> allowedFreelancers = GetAllowedFreelancers(qi);
		switch (resolvedDuplicationRule)
		{
		case FreelancerDuplicationRuleTypes.noneInGame:
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (allowedFreelancers.Count() < gameConfig.TotalHumanPlayers)
				{
					throw new Exception($"The {gameConfig.GameType} sub type {subTypeName} has been poorly configured. DuplicationRule=noneInGame & only {allowedFreelancers.Count()} freelancers in AllowedFreelancers");
				}
				return;
			}
		case FreelancerDuplicationRuleTypes.noneInTeam:
			if (allowedFreelancers.Count() >= gameConfig.MaxGroupSize)
			{
				break;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				throw new Exception($"The {gameConfig.GameType} sub type {subTypeName} has been poorly configured. DuplicationRule=noneInTeam & only {allowedFreelancers.Count()} freelancers in AllowedFreelancers");
			}
		}
	}
}
