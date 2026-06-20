// ROGUES
// SERVER
using UnityEngine;

// same in reactor & rogues
public class NanoSmithBoltInfoComponent : MonoBehaviour
{
	[Header("-- Bolt Info ----------------------------")]
	public NanoSmithBoltInfo m_boltInfo;
	[Header("-- Per Ability Range Overrides")]
	public float m_smiteRangeOverride = 2f;
	public float m_anvilSlamRangeOverride = -1f;
	public float m_battleForgedRangeOverride = -1f;
	public float m_boltLaserRangeOverride = -1f;
	[TextArea(1, 10)]
	public string m_notes;
}
