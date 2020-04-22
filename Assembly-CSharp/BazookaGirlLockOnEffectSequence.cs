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
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!m_initialized)
			{
				return;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (!(m_trackingEffectVFX == null))
				{
					return;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					if (!base.Target)
					{
						return;
					}
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						if (!m_fxJoint.IsInitialized())
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
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
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Object.Destroy(m_trackingEffectVFX);
			return;
		}
	}
}
