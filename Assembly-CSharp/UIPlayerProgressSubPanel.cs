using UnityEngine;

public class UIPlayerProgressSubPanel : MonoBehaviour
{
	public Animator m_animator;

	public string m_animationPrefix;

	private bool m_active;

	public bool IsActive => m_active;

	public virtual void ClickedOnPageIndicator(UIPageIndicator pageIndicator)
	{
	}

	public virtual void SetActive(bool visible)
	{
		m_active = visible;
		if (visible)
		{
			while (true)
			{
				switch (4)
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
			UIManager.SetGameObjectActive(base.gameObject, true);
		}
		if (m_animator.isInitialized)
		{
			UIAnimationEventManager.Get().PlayAnimation(m_animator, m_animationPrefix + ((!visible) ? "OUT" : "IN"), HandleAnimationEnd, string.Empty);
		}
	}

	private void HandleAnimationEnd()
	{
		if (m_active)
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
			UIManager.SetGameObjectActive(base.gameObject, false);
			return;
		}
	}
}
