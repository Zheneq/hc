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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlLockOnEffectSequence.Update()).MethodHandle;
			}
			if (this.m_initialized)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_trackingEffectVFX == null)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (base.Target)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!this.m_fxJoint.IsInitialized())
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlLockOnEffectSequence.OnDisable()).MethodHandle;
			}
			UnityEngine.Object.Destroy(this.m_trackingEffectVFX);
		}
	}
}
