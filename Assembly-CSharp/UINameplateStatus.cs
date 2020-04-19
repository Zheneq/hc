using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINameplateStatus : MonoBehaviour
{
	public Image m_StatusIcon;

	public TextMeshProUGUI m_StatusText;

	public Animator m_animController;

	public UINameplateItem m_parent;

	private bool gainedStatus;

	public void DisplayAsNegativeStatus(UINameplateItem parent)
	{
		this.m_animController.Play("GainedNegativeStatus");
		this.gainedStatus = true;
		this.m_parent = parent;
	}

	public void DisplayAsPositiveStatus(UINameplateItem parent)
	{
		this.m_animController.Play("GainedPositiveStatus");
		this.gainedStatus = true;
		this.m_parent = parent;
	}

	public void DisplayAsLostStatus(UINameplateItem parent)
	{
		this.m_animController.Play("LostStatus");
		this.gainedStatus = false;
		this.m_parent = parent;
	}

	public void AnimDone()
	{
		if (this.m_parent != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINameplateStatus.AnimDone()).MethodHandle;
			}
			this.m_parent.NotifyStatusAnimationDone(this, this.gainedStatus);
		}
	}

	public void Update()
	{
		if (this.m_parent == null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
