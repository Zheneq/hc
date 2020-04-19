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
				RuntimeMethodHandle runtimeMethodHandle = methodof(_DragOntoAble.OnEnable()).MethodHandle;
			}
			this.normalColor = this.containerImage.color;
		}
	}

	public void OnDrop(PointerEventData data)
	{
		this.containerImage.color = this.normalColor;
		if (this.receivingImage == null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(_DragOntoAble.OnDrop(PointerEventData)).MethodHandle;
			}
			return;
		}
		Sprite dropSprite = this.GetDropSprite(data);
		if (dropSprite != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			this.receivingImage.overrideSprite = dropSprite;
		}
	}

	public void OnPointerEnter(PointerEventData data)
	{
		if (this.containerImage == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(_DragOntoAble.OnPointerEnter(PointerEventData)).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(_DragOntoAble.OnPointerExit(PointerEventData)).MethodHandle;
			}
			return;
		}
		this.containerImage.color = this.normalColor;
	}

	private Sprite GetDropSprite(PointerEventData data)
	{
		GameObject pointerDrag = data.pointerDrag;
		if (pointerDrag == null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(_DragOntoAble.GetDropSprite(PointerEventData)).MethodHandle;
			}
			return null;
		}
		Image component = pointerDrag.GetComponent<Image>();
		if (component == null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			return null;
		}
		return component.sprite;
	}
}
