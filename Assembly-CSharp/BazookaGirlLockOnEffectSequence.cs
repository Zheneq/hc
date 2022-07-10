using UnityEngine;

public class BazookaGirlLockOnEffectSequence : Sequence
{
	public GameObject m_trackingEffectPrefab;

	private GameObject m_trackingEffectVFX;

	[AudioEvent(false)]
	public string m_audioEventApply;
	[JointPopup("FX attach joint (or start position for projectiles).")]
	public JointPopupProperty m_fxJoint;

	private void Update()
	{
		if (!m_trackingEffectPrefab || !m_initialized || m_trackingEffectVFX != null || !Target)
		{
			return;
		}
		if (!m_fxJoint.IsInitialized())
		{
			m_fxJoint.Initialize(Target.gameObject);
		}
		m_trackingEffectVFX = InstantiateFX(m_trackingEffectPrefab);
		AttachToBone(m_trackingEffectVFX, m_fxJoint.m_jointObject);
		m_trackingEffectVFX.transform.localPosition = Vector3.zero;
		m_trackingEffectVFX.transform.localRotation = Quaternion.identity;
		AudioManager.PostEvent(m_audioEventApply, m_trackingEffectVFX.gameObject);
		Source.OnSequenceHit(this, Target, null);
	}

	private void OnDisable()
	{
		if (m_trackingEffectVFX)
		{
			Destroy(m_trackingEffectVFX);
		}
	}
}
