using System;
using UnityEngine;

public class UIPlayerProgressSubPanel : MonoBehaviour
{
	public Animator m_animator;

	public string m_animationPrefix;

	private bool m_active;

	public bool IsActive
	{
		get
		{
			return this.m_active;
		}
	}

	public virtual void ClickedOnPageIndicator(UIPageIndicator pageIndicator)
	{
	}

	public virtual void SetActive(bool visible)
	{
		this.m_active = visible;
		if (visible)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressSubPanel.SetActive(bool)).MethodHandle;
			}
			UIManager.SetGameObjectActive(base.gameObject, true, null);
		}
		if (this.m_animator.isInitialized)
		{
			UIAnimationEventManager.Get().PlayAnimation(this.m_animator, this.m_animationPrefix + ((!visible) ? "OUT" : "IN"), new UIAnimationEventManager.AnimationDoneCallback(this.HandleAnimationEnd), string.Empty, 0, 0f, true, false, null, null);
		}
	}

	private void HandleAnimationEnd()
	{
		if (!this.m_active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressSubPanel.HandleAnimationEnd()).MethodHandle;
			}
			UIManager.SetGameObjectActive(base.gameObject, false, null);
		}
	}
}
