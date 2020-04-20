using System;
using UnityEngine;

[Serializable]
public class ActivatableObject
{
	public Transform m_sceneObject;

	public ActivatableObject.ActivationAction m_activation;

	public void Activate()
	{
		if (this.m_activation == ActivatableObject.ActivationAction.SetActive)
		{
			this.m_sceneObject.gameObject.SetActive(true);
		}
		else if (this.m_activation == ActivatableObject.ActivationAction.ClearActive)
		{
			this.m_sceneObject.gameObject.SetActive(false);
		}
		else if (this.m_activation == ActivatableObject.ActivationAction.ToggleActive)
		{
			this.m_sceneObject.gameObject.SetActive(!this.m_sceneObject.gameObject.activeSelf);
		}
	}

	public void SetIsActive(bool active)
	{
		this.m_sceneObject.gameObject.SetActive(active);
	}

	public enum ActivationAction
	{
		SetActive,
		ClearActive,
		ToggleActive
	}
}
