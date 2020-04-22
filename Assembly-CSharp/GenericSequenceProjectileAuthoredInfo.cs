using System;
using UnityEngine;

[Serializable]
public class GenericSequenceProjectileAuthoredInfo
{
	public GameObject m_fxPrefab;

	[Header("-- For explosion on end of projectile --")]
	public GameObject m_fxImpactPrefab;

	public bool m_spawnImpactAtFXDespawn;

	[Header("-- For Impact on actor hit --")]
	public GameObject m_targetHitFxPrefab;

	public Sequence.HitVFXSpawnTeam m_hitFxTeamFilter = Sequence.HitVFXSpawnTeam.AllExcludeCaster;

	[JointPopup("Start position for projectile")]
	public JointPopupProperty m_fxJoint;

	public Sequence.ReferenceModelType m_jointReferenceType;

	[JointPopup("FX attach joint for hit fx")]
	public JointPopupProperty m_hitPosJoint;

	public bool m_targetHitFxAttachToJoint;

	[Space(5f)]
	public float m_projectileSpeed;

	public float m_splineFractionUntilImpact = 1f;

	[Space(5f)]
	public float m_maxHeight;

	public float m_yOffset;

	public bool m_reverseDirection;

	[AudioEvent(false)]
	public string m_spawnAudioEvent;

	[AudioEvent(false)]
	public string m_impactAudioEvent;

	[AudioEvent(false)]
	public string m_targetHitAudioEvent;
}
