using System;
using UnityEngine;

[Serializable]
public class StatusVfxPrefabToJoint
{
	public string m_name;

	public StatusType m_status = StatusType.INVALID;

	public GameObject m_statusVfxPrefab;

	[JointPopup("Status VFX Attach Joint")]
	public JointPopupProperty m_vfxJoint;

	public bool m_alignToRootOrientation;
}
