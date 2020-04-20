using System;
using UnityEngine;

public class DurationActorVFXInfo
{
	private GameObject m_vfxInst;

	private float m_displayMaxDuration;

	private float m_remainingDisplayTime;

	public DurationActorVFXInfo(GameObject vfxPrefab, float maxDuration, GameObject parentObject)
	{
		this.m_displayMaxDuration = maxDuration;
		if (vfxPrefab != null)
		{
			this.m_vfxInst = UnityEngine.Object.Instantiate<GameObject>(vfxPrefab);
			if (this.m_vfxInst != null)
			{
				if (parentObject != null)
				{
					this.m_vfxInst.transform.parent = parentObject.transform;
				}
				this.m_vfxInst.transform.localPosition = Vector3.zero;
				this.m_vfxInst.transform.localRotation = Quaternion.identity;
				this.m_vfxInst.SetActive(false);
			}
		}
		else
		{
			this.m_vfxInst = null;
		}
	}

	public void OnUpdate()
	{
		if (this.m_vfxInst != null)
		{
			if (this.m_remainingDisplayTime > 0f)
			{
				this.m_remainingDisplayTime -= Time.deltaTime;
				if (this.m_remainingDisplayTime <= 0f)
				{
					this.HideVfx();
				}
			}
		}
	}

	public void ShowVfxAtPosition(Vector3 position, bool actorVisible, Vector3 lookDir)
	{
		if (this.m_vfxInst != null)
		{
			this.m_vfxInst.transform.position = position;
			this.ShowVfx(actorVisible, lookDir);
		}
	}

	public void ShowVfx(bool actorVisible, Vector3 lookDir)
	{
		if (this.m_vfxInst != null)
		{
			if (actorVisible)
			{
				this.m_vfxInst.SetActive(true);
			}
			if (lookDir != Vector3.zero)
			{
				this.m_vfxInst.transform.rotation = Quaternion.LookRotation(lookDir);
			}
			this.m_remainingDisplayTime = this.m_displayMaxDuration;
		}
		else
		{
			this.m_remainingDisplayTime = 0f;
		}
	}

	public void HideVfx()
	{
		if (this.m_vfxInst != null)
		{
			this.m_vfxInst.SetActive(false);
		}
		this.m_remainingDisplayTime = 0f;
	}

	public void DestroyVfx()
	{
		if (this.m_vfxInst != null)
		{
			UnityEngine.Object.Destroy(this.m_vfxInst);
			this.m_vfxInst = null;
		}
	}
}
