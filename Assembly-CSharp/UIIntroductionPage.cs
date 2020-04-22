using UnityEngine;

public class UIIntroductionPage : MonoBehaviour
{
	public RectTransform m_container;

	public _SelectableBtn m_backBtn;

	public _SelectableBtn m_nextBtn;

	public void SetVisible(bool visible)
	{
		UIManager.SetGameObjectActive(m_container, visible);
	}
}
