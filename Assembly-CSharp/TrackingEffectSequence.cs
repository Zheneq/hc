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
			if (m_trackingEffectVFX == null && (bool)base.Target)
			{
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
			if ((bool)base.Target)
			{
				Renderer[] componentsInChildren = m_trackingEffectVFX.GetComponentsInChildren<Renderer>();
				Renderer[] array = componentsInChildren;
				foreach (Renderer renderer in array)
				{
					renderer.enabled = base.Target.GetModelRenderer().enabled;
				}
				while (true)
				{
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
			base.Target.GetComponent<AudioSource>().PlayOneShot(m_trackingEnd);
			Object.Destroy(m_trackingEffectVFX.gameObject);
			return;
		}
	}
}
