using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class _DragOntoAble : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
	public Image containerImage;

	public Image receivingImage;

	private Color normalColor;

	public Color highlightColor = Color.yellow;

	public void OnEnable()
	{
		if (this.containerImage != null)
		{
			this.normalColor = this.containerImage.color;
		}
	}

	public void OnDrop(PointerEventData data)
	{
		this.containerImage.color = this.normalColor;
		if (this.receivingImage == null)
		{
			return;
		}
		Sprite dropSprite = this.GetDropSprite(data);
		if (dropSprite != null)
		{
			this.receivingImage.overrideSprite = dropSprite;
		}
	}

	public void OnPointerEnter(PointerEventData data)
	{
		if (this.containerImage == null)
		{
			return;
		}
		Sprite dropSprite = this.GetDropSprite(data);
		if (dropSprite != null)
		{
			this.containerImage.color = this.highlightColor;
		}
	}

	public void OnPointerExit(PointerEventData data)
	{
		if (this.containerImage == null)
		{
			return;
		}
		this.containerImage.color = this.normalColor;
	}

	private Sprite GetDropSprite(PointerEventData data)
	{
		GameObject pointerDrag = data.pointerDrag;
		if (pointerDrag == null)
		{
			return null;
		}
		Image component = pointerDrag.GetComponent<Image>();
		if (component == null)
		{
			return null;
		}
		return component.sprite;
	}
}
