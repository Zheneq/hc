using System;

public static class FreelancerResolutionPhaseSubTypeExtensions
{
	public static bool IsPickBanSubPhase(this FreelancerResolutionPhaseSubType subtype)
	{
		bool result;
		if (subtype != FreelancerResolutionPhaseSubType.PICK_BANS1)
		{
			result = (subtype == FreelancerResolutionPhaseSubType.PICK_BANS2);
		}
		else
		{
			result = true;
		}
		return result;
	}

	public static bool IsPickFreelancerSubPhase(this FreelancerResolutionPhaseSubType subtype)
	{
		return subtype == FreelancerResolutionPhaseSubType.PICK_FREELANCER1 || subtype == FreelancerResolutionPhaseSubType.PICK_FREELANCER2;
	}
}
