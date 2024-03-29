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
			UIManager.SetGameObjectActive(base.gameObject, false);
			return;
		}
	}
}
