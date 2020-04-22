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
		if (!(containerImage != null))
		{
			return;
		}
		while (true)
		{
			normalColor = containerImage.color;
			return;
		}
	}

	public void OnDrop(PointerEventData data)
	{
		containerImage.color = normalColor;
		if (receivingImage == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		Sprite dropSprite = GetDropSprite(data);
		if (!(dropSprite != null))
		{
			return;
		}
		while (true)
		{
			receivingImage.overrideSprite = dropSprite;
			return;
		}
	}

	public void OnPointerEnter(PointerEventData data)
	{
		if (containerImage == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		Sprite dropSprite = GetDropSprite(data);
		if (dropSprite != null)
		{
			containerImage.color = highlightColor;
		}
	}

	public void OnPointerExit(PointerEventData data)
	{
		if (containerImage == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		containerImage.color = normalColor;
	}

	private Sprite GetDropSprite(PointerEventData data)
	{
		GameObject pointerDrag = data.pointerDrag;
		if (pointerDrag == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		Image component = pointerDrag.GetComponent<Image>();
		if (component == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		return component.sprite;
	}
}
