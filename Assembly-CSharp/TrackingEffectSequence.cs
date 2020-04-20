using System;
using UnityEngine;

public class TrackingEffectSequence : Sequence
{
	public Transform m_trackingEffectPrefab;

	private Transform m_trackingEffectVFX;

	public AudioClip m_trackingStart;

	public AudioClip m_trackingEnd;

	private void Update()
	{
		if (this.m_trackingEffectPrefab && this.m_initialized)
		{
			if (this.m_trackingEffectVFX == null && base.Target)
			{
				Vector3 bonePosition = base.Target.GetBonePosition("upperRoot_JNT");
				this.m_trackingEffectVFX = UnityEngine.Object.Instantiate<Transform>(this.m_trackingEffectPrefab, bonePosition, Quaternion.identity);
				this.m_trackingEffectVFX.transform.parent = base.transform;
				base.Target.GetComponent<AudioSource>().PlayOneShot(this.m_trackingStart);
			}
		}
		if (this.m_trackingEffectVFX != null)
		{
			if (base.Target)
			{
				Renderer[] componentsInChildren = this.m_trackingEffectVFX.GetComponentsInChildren<Renderer>();
				foreach (Renderer renderer in componentsInChildren)
				{
					renderer.enabled = base.Target.GetActorModelDataRenderer().enabled;
				}
				Vector3 bonePosition2 = base.Target.GetBonePosition("upperRoot_JNT");
				this.m_trackingEffectVFX.position = bonePosition2;
			}
		}
	}

	private void OnDisable()
	{
		if (this.m_trackingEffectVFX)
		{
			base.Target.GetComponent<AudioSource>().PlayOneShot(this.m_trackingEnd);
			UnityEngine.Object.Destroy(this.m_trackingEffectVFX.gameObject);
		}
	}
}
