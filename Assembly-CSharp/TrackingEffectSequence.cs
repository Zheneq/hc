using UnityEngine;

public class TrackingEffectSequence : Sequence
{
	public Transform m_trackingEffectPrefab;

	private Transform m_trackingEffectVFX;

	public AudioClip m_trackingStart;

	public AudioClip m_trackingEnd;

	private void Update()
	{
		if ((bool)m_trackingEffectPrefab && m_initialized)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_trackingEffectVFX == null && (bool)base.Target)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				Vector3 bonePosition = base.Target.GetBonePosition("upperRoot_JNT");
				m_trackingEffectVFX = Object.Instantiate(m_trackingEffectPrefab, bonePosition, Quaternion.identity);
				m_trackingEffectVFX.transform.parent = base.transform;
				base.Target.GetComponent<AudioSource>().PlayOneShot(m_trackingStart);
			}
		}
		if (!(m_trackingEffectVFX != null))
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
			if ((bool)base.Target)
			{
				Renderer[] componentsInChildren = m_trackingEffectVFX.GetComponentsInChildren<Renderer>();
				Renderer[] array = componentsInChildren;
				foreach (Renderer renderer in array)
				{
					renderer.enabled = base.Target.GetActorModelDataRenderer().enabled;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					Vector3 bonePosition2 = base.Target.GetBonePosition("upperRoot_JNT");
					m_trackingEffectVFX.position = bonePosition2;
					return;
				}
			}
			return;
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
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			base.Target.GetComponent<AudioSource>().PlayOneShot(m_trackingEnd);
			Object.Destroy(m_trackingEffectVFX.gameObject);
			return;
		}
	}
}
