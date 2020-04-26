public static class FreelancerResolutionPhaseSubTypeExtensions
{
	public static bool IsPickBanSubPhase(this FreelancerResolutionPhaseSubType subtype)
	{
		int result;
		if (subtype != FreelancerResolutionPhaseSubType.PICK_BANS1)
		{
			result = ((subtype == FreelancerResolutionPhaseSubType.PICK_BANS2) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public static bool IsPickFreelancerSubPhase(this FreelancerResolutionPhaseSubType subtype)
	{
		return subtype == FreelancerResolutionPhaseSubType.PICK_FREELANCER1 || subtype == FreelancerResolutionPhaseSubType.PICK_FREELANCER2;
	}
}
