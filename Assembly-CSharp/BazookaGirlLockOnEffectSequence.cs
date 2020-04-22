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
		if (!m_trackingEffectPrefab)
		{
			return;
		}
		while (true)
		{
			if (!m_initialized)
			{
				return;
			}
			while (true)
			{
				if (!(m_trackingEffectVFX == null))
				{
					return;
				}
				while (true)
				{
					if (!base.Target)
					{
						return;
					}
					while (true)
					{
						if (!m_fxJoint.IsInitialized())
						{
							m_fxJoint.Initialize(base.Target.gameObject);
						}
						m_trackingEffectVFX = InstantiateFX(m_trackingEffectPrefab);
						AttachToBone(m_trackingEffectVFX, m_fxJoint.m_jointObject);
						m_trackingEffectVFX.transform.localPosition = Vector3.zero;
						m_trackingEffectVFX.transform.localRotation = Quaternion.identity;
						AudioManager.PostEvent(m_audioEventApply, m_trackingEffectVFX.gameObject);
						base.Source.OnSequenceHit(this, base.Target, null);
						return;
					}
				}
			}
		}
	}

	private void OnDisable()
	{
		if (!m_trackingEffectVFX)
		{
			return;
		}
		while (true)
		{
			Object.Destroy(m_trackingEffectVFX);
			return;
		}
	}
}
