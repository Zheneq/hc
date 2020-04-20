using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class TeamCompositionRules
{
	public Dictionary<TeamCompositionRules.SlotTypes, FreelancerSet> Rules;

	private bool MatchesSlotType(TeamCompositionRules.SlotTypes st, Team team, int slot)
	{
		switch (st)
		{
		case TeamCompositionRules.SlotTypes.All:
			return true;
		case TeamCompositionRules.SlotTypes.TeamA:
			return team == Team.TeamA;
		case TeamCompositionRules.SlotTypes.TeamB:
			return team == Team.TeamB;
		case TeamCompositionRules.SlotTypes.A1:
		{
			bool result;
			if (team == Team.TeamA)
			{
				result = (slot == 1);
			}
			else
			{
				result = false;
			}
			return result;
		}
		case TeamCompositionRules.SlotTypes.A2:
		{
			bool result2;
			if (team == Team.TeamA)
			{
				result2 = (slot == 2);
			}
			else
			{
				result2 = false;
			}
			return result2;
		}
		case TeamCompositionRules.SlotTypes.A3:
		{
			bool result3;
			if (team == Team.TeamA)
			{
				result3 = (slot == 3);
			}
			else
			{
				result3 = false;
			}
			return result3;
		}
		case TeamCompositionRules.SlotTypes.A4:
		{
			bool result4;
			if (team == Team.TeamA)
			{
				result4 = (slot == 4);
			}
			else
			{
				result4 = false;
			}
			return result4;
		}
		case TeamCompositionRules.SlotTypes.A5:
		{
			bool result5;
			if (team == Team.TeamA)
			{
				result5 = (slot == 5);
			}
			else
			{
				result5 = false;
			}
			return result5;
		}
		case TeamCompositionRules.SlotTypes.B1:
			return team == Team.TeamB && slot == 1;
		case TeamCompositionRules.SlotTypes.B2:
			return team == Team.TeamB && slot == 2;
		case TeamCompositionRules.SlotTypes.B3:
		{
			bool result6;
			if (team == Team.TeamB)
			{
				result6 = (slot == 3);
			}
			else
			{
				result6 = false;
			}
			return result6;
		}
		case TeamCompositionRules.SlotTypes.B4:
			return team == Team.TeamB && slot == 4;
		case TeamCompositionRules.SlotTypes.B5:
		{
			bool result7;
			if (team == Team.TeamB)
			{
				result7 = (slot == 5);
			}
			else
			{
				result7 = false;
			}
			return result7;
		}
		case TeamCompositionRules.SlotTypes.Slot1:
			return slot == 1;
		case TeamCompositionRules.SlotTypes.Slot2:
			return slot == 2;
		case TeamCompositionRules.SlotTypes.Slot3:
			return slot == 3;
		case TeamCompositionRules.SlotTypes.Slot4:
			return slot == 4;
		case TeamCompositionRules.SlotTypes.Slot5:
			return slot == 5;
		default:
			throw new Exception("Unimplemented slot type");
		}
	}

	public bool IsCharacterAllowed(CharacterType freelancer, IFreelancerSetQueryInterface qi)
	{
		bool result;
		if (this.Rules != null)
		{
			result = this.Rules.Values.ToList<FreelancerSet>().Exists((FreelancerSet p) => p.IsCharacterAllowed(freelancer, qi));
		}
		else
		{
			result = true;
		}
		return result;
	}

	public bool IsCharacterAllowedInSlot(CharacterType freelancer, Team team, int slot, IFreelancerSetQueryInterface qi)
	{
		using (Dictionary<TeamCompositionRules.SlotTypes, FreelancerSet>.Enumerator enumerator = this.Rules.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<TeamCompositionRules.SlotTypes, FreelancerSet> keyValuePair = enumerator.Current;
				if (this.MatchesSlotType(keyValuePair.Key, team, slot))
				{
					return keyValuePair.Value.IsCharacterAllowed(freelancer, qi);
				}
			}
		}
		throw new Exception(string.Format("There is no TeamComposition Rule to cover team {0} slot {1}", team, slot));
	}

	public List<CharacterType> GetAllowedFreelancers(IFreelancerSetQueryInterface qi)
	{
		HashSet<CharacterType> hashSet = new HashSet<CharacterType>();
		using (Dictionary<TeamCompositionRules.SlotTypes, FreelancerSet>.Enumerator enumerator = this.Rules.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<TeamCompositionRules.SlotTypes, FreelancerSet> keyValuePair = enumerator.Current;
				hashSet.UnionWith(keyValuePair.Value.GetAllowedCharacters(qi));
			}
		}
		return hashSet.ToList<CharacterType>();
	}

	public LocalizationPayload GenerateFailure(IQueueRequirementApplicant applicant, IFreelancerSetQueryInterface qi)
	{
		List<CharacterType> allowedFreelancers = this.GetAllowedFreelancers(qi);
		if (allowedFreelancers.Count == 1)
		{
			return LocalizationPayload.Create("XMustHaveAccessToY", "Matchmaking", new LocalizationArg[]
			{
				applicant.LocalizedHandle,
				LocalizationArg_Freelancer.Create(allowedFreelancers.First<CharacterType>())
			});
		}
		if (allowedFreelancers.Count == 2)
		{
			return LocalizationPayload.Create("XMustHaveAccessToYOrZ", "Matchmaking", new LocalizationArg[]
			{
				applicant.LocalizedHandle,
				LocalizationArg_Freelancer.Create(allowedFreelancers[0]),
				LocalizationArg_Freelancer.Create(allowedFreelancers[1])
			});
		}
		return LocalizationPayload.Create("XMustHaveAccessToYZOrOthers", "Matchmaking", new LocalizationArg[]
		{
			applicant.LocalizedHandle,
			LocalizationArg_Freelancer.Create(allowedFreelancers[0]),
			LocalizationArg_Freelancer.Create(allowedFreelancers[1])
		});
	}

	public void ValidateSelf(IFreelancerSetQueryInterface qi, LobbyGameConfig gameConfig, FreelancerDuplicationRuleTypes resolvedDuplicationRule, string subTypeName)
	{
		this.Rules.Values.ToList<FreelancerSet>().ForEach(delegate(FreelancerSet p)
		{
			p.ValidateSelf(gameConfig, subTypeName);
		});
		List<CharacterType> allowedFreelancers = this.GetAllowedFreelancers(qi);
		if (resolvedDuplicationRule == FreelancerDuplicationRuleTypes.noneInGame)
		{
			if (allowedFreelancers.Count<CharacterType>() < gameConfig.TotalHumanPlayers)
			{
				throw new Exception(string.Format("The {0} sub type {1} has been poorly configured. DuplicationRule=noneInGame & only {2} freelancers in AllowedFreelancers", gameConfig.GameType, subTypeName, allowedFreelancers.Count<CharacterType>()));
			}
		}
		else if (resolvedDuplicationRule == FreelancerDuplicationRuleTypes.noneInTeam && allowedFreelancers.Count<CharacterType>() < gameConfig.MaxGroupSize)
		{
			throw new Exception(string.Format("The {0} sub type {1} has been poorly configured. DuplicationRule=noneInTeam & only {2} freelancers in AllowedFreelancers", gameConfig.GameType, subTypeName, allowedFreelancers.Count<CharacterType>()));
		}
	}

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
}
