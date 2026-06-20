using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIArticleSelectButton : MonoBehaviour
{
	public Image m_image;

	public TextMeshProUGUI m_title;

	public _ButtonSwapSprite m_hitbox;

	private LoreArticle m_article;

	public void Setup(LoreArticle article)
	{
		m_image.sprite = Resources.Load<Sprite>(article.ImagePath);
		m_title.text = article.GetTitle();
		m_hitbox.callback = OnClick;
		m_article = article;
	}

	private void OnClick(BaseEventData data)
	{
		UILandingPageFullScreenMenus.Get().DisplayLoreArticle(m_article);
	}
}
