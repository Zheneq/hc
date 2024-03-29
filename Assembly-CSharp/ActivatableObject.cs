using System;
using UnityEngine;

[Serializable]
public class ActivatableObject
{
	public enum ActivationAction
	{
		SetActive,
		ClearActive,
		ToggleActive
	}

	public Transform m_sceneObject;

	public ActivationAction m_activation;

	public void Activate()
	{
		if (m_activation == ActivationAction.SetActive)
		{
			m_sceneObject.gameObject.SetActive(true);
			return;
		}
		if (m_activation == ActivationAction.ClearActive)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					m_sceneObject.gameObject.SetActive(false);
					return;
				}
			}
		}
		if (m_activation != ActivationAction.ToggleActive)
		{
			return;
		}
		while (true)
		{
			m_sceneObject.gameObject.SetActive(!m_sceneObject.gameObject.activeSelf);
			return;
		}
	}

	public void SetIsActive(bool active)
	{
		m_sceneObject.gameObject.SetActive(active);
	}
}
