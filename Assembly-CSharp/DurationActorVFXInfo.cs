using UnityEngine;

public class DurationActorVFXInfo
{
	private GameObject m_vfxInst;

	private float m_displayMaxDuration;

	private float m_remainingDisplayTime;

	public DurationActorVFXInfo(GameObject vfxPrefab, float maxDuration, GameObject parentObject)
	{
		m_displayMaxDuration = maxDuration;
		if (vfxPrefab != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					m_vfxInst = Object.Instantiate(vfxPrefab);
					if (m_vfxInst != null)
					{
						if (parentObject != null)
						{
							m_vfxInst.transform.parent = parentObject.transform;
						}
						m_vfxInst.transform.localPosition = Vector3.zero;
						m_vfxInst.transform.localRotation = Quaternion.identity;
						m_vfxInst.SetActive(false);
					}
					return;
				}
			}
		}
		m_vfxInst = null;
	}

	public void OnUpdate()
	{
		if (!(m_vfxInst != null))
		{
			return;
		}
		while (true)
		{
			if (m_remainingDisplayTime > 0f)
			{
				m_remainingDisplayTime -= Time.deltaTime;
				if (m_remainingDisplayTime <= 0f)
				{
					HideVfx();
				}
			}
			return;
		}
	}

	public void ShowVfxAtPosition(Vector3 position, bool actorVisible, Vector3 lookDir)
	{
		if (!(m_vfxInst != null))
		{
			return;
		}
		while (true)
		{
			m_vfxInst.transform.position = position;
			ShowVfx(actorVisible, lookDir);
			return;
		}
	}

	public void ShowVfx(bool actorVisible, Vector3 lookDir)
	{
		if (m_vfxInst != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (actorVisible)
					{
						m_vfxInst.SetActive(true);
					}
					if (lookDir != Vector3.zero)
					{
						m_vfxInst.transform.rotation = Quaternion.LookRotation(lookDir);
					}
					m_remainingDisplayTime = m_displayMaxDuration;
					return;
				}
			}
		}
		m_remainingDisplayTime = 0f;
	}

	public void HideVfx()
	{
		if (m_vfxInst != null)
		{
			m_vfxInst.SetActive(false);
		}
		m_remainingDisplayTime = 0f;
	}

	public void DestroyVfx()
	{
		if (!(m_vfxInst != null))
		{
			return;
		}
		while (true)
		{
			Object.Destroy(m_vfxInst);
			m_vfxInst = null;
			return;
		}
	}
}
