public static class FreelancerResolutionPhaseSubTypeExtensions
{
    public static bool IsPickBanSubPhase(this FreelancerResolutionPhaseSubType subtype)
    {
        return subtype == FreelancerResolutionPhaseSubType.PICK_BANS1
               || subtype == FreelancerResolutionPhaseSubType.PICK_BANS2;
    }

    public static bool IsPickFreelancerSubPhase(this FreelancerResolutionPhaseSubType subtype)
    {
        return subtype == FreelancerResolutionPhaseSubType.PICK_FREELANCER1
               || subtype == FreelancerResolutionPhaseSubType.PICK_FREELANCER2;
    }
}