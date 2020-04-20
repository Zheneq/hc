using System;
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
		if (this.m_trackingEffectPrefab)
		{
			if (this.m_initialized)
			{
				if (this.m_trackingEffectVFX == null)
				{
					if (base.Target)
					{
						if (!this.m_fxJoint.IsInitialized())
						{
							this.m_fxJoint.Initialize(base.Target.gameObject);
						}
						this.m_trackingEffectVFX = base.InstantiateFX(this.m_trackingEffectPrefab);
						base.AttachToBone(this.m_trackingEffectVFX, this.m_fxJoint.m_jointObject);
						this.m_trackingEffectVFX.transform.localPosition = Vector3.zero;
						this.m_trackingEffectVFX.transform.localRotation = Quaternion.identity;
						AudioManager.PostEvent(this.m_audioEventApply, this.m_trackingEffectVFX.gameObject);
						base.Source.OnSequenceHit(this, base.Target, null, ActorModelData.RagdollActivation.HealthBased, true);
					}
				}
			}
		}
	}

	private void OnDisable()
	{
		if (this.m_trackingEffectVFX)
		{
			UnityEngine.Object.Destroy(this.m_trackingEffectVFX);
		}
	}
}
