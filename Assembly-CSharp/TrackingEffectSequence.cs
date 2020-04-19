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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackingEffectSequence.Update()).MethodHandle;
			}
			if (this.m_trackingEffectVFX == null && base.Target)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				Vector3 position = base.Target.\u000E("upperRoot_JNT");
				this.m_trackingEffectVFX = UnityEngine.Object.Instantiate<Transform>(this.m_trackingEffectPrefab, position, Quaternion.identity);
				this.m_trackingEffectVFX.transform.parent = base.transform;
				base.Target.GetComponent<AudioSource>().PlayOneShot(this.m_trackingStart);
			}
		}
		if (this.m_trackingEffectVFX != null)
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
			if (base.Target)
			{
				Renderer[] componentsInChildren = this.m_trackingEffectVFX.GetComponentsInChildren<Renderer>();
				foreach (Renderer renderer in componentsInChildren)
				{
					renderer.enabled = base.Target.\u000E().enabled;
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				Vector3 position2 = base.Target.\u000E("upperRoot_JNT");
				this.m_trackingEffectVFX.position = position2;
			}
		}
	}

	private void OnDisable()
	{
		if (this.m_trackingEffectVFX)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackingEffectSequence.OnDisable()).MethodHandle;
			}
			base.Target.GetComponent<AudioSource>().PlayOneShot(this.m_trackingEnd);
			UnityEngine.Object.Destroy(this.m_trackingEffectVFX.gameObject);
		}
	}
}
