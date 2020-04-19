using System;
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
		this.m_image.sprite = Resources.Load<Sprite>(article.ImagePath);
		this.m_title.text = article.GetTitle();
		this.m_hitbox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnClick);
		this.m_article = article;
	}

	private void OnClick(BaseEventData data)
	{
		UILandingPageFullScreenMenus.Get().DisplayLoreArticle(this.m_article);
	}
}
