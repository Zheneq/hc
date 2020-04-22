using UnityEngine;

public class PowerUp_VisionBuff : PowerUp_Standard_Ability
{
	[Separator("Vision Buff", true)]
	public float m_visionRadius = 8.5f;

	public bool m_visionUseStraightLineDist;

	public int m_visionDuration = 2;

	public VisionProviderInfo.BrushRevealType m_visionBrushRevealType = VisionProviderInfo.BrushRevealType.Always;

	public bool m_visionAreaIgnoreLos = true;

	public bool m_visionAreaCanFunctionInGlobalBlind = true;

	[Separator("Sequence on actor with vision buff", true)]
	public GameObject m_visionBuffSeqPrefab;
}
