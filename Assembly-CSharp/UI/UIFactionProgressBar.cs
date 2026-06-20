using UnityEngine;
using UnityEngine.UI;

public class UIFactionProgressBar : MonoBehaviour
{
	public LayoutElement m_LayoutElement;

	public Image m_EmptyBar;

	public ImageFilledSloped m_ProgressFillBar;

	public Image m_CompletedBar;

	public ImageFilledSloped m_WhiteShineBar;

	private void Awake()
	{
		UIManager.SetGameObjectActive(m_WhiteShineBar, false);
	}
}
