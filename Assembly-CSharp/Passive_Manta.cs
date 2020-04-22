using System.Collections.Generic;

public class Passive_Manta : Passive
{
	private MantaCreateBarriers m_createBarriersAbility;

	private MantaRegeneration m_regenAbility;

	private StandardBarrierData m_ultBarrierInfo;

	private List<BarrierPoseInfo> m_ultBarrierLocations;

	public int DamageReceivedForRegeneration
	{
		get;
		private set;
	}
}
