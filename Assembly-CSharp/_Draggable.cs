using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class _Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
{
	public bool dragOnSurfaces = true;

	public bool m_CopyChildrenImages;

	private static GameObject m_draggingObject;

	private GameObject m_DraggingIcon;

	private RectTransform m_DraggingPlane;

	public static GameObject GetDraggedObject()
	{
		return _Draggable.m_draggingObject;
	}

	private void CopyChildrenImages(Image sourceImage, Image parent)
	{
		if (!(parent == null))
		{
			if (!(sourceImage == null))
			{
				Image[] componentsInChildren = sourceImage.GetComponentsInChildren<Image>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					Image component = componentsInChildren[i].GetComponent<Image>();
					if (!(component.gameObject == sourceImage.gameObject))
					{
						GameObject gameObject = new GameObject();
						gameObject.transform.SetParent(parent.transform, false);
						gameObject.transform.SetAsLastSibling();
						Image image = gameObject.AddComponent<Image>();
						CanvasGroup canvasGroup = gameObject.AddComponent<CanvasGroup>();
						canvasGroup.blocksRaycasts = false;
						image.sprite = component.sprite;
						image.color = component.color;
						if (this.m_CopyChildrenImages)
						{
							this.CopyChildrenImages(component, image);
						}
						image.SetNativeSize();
					}
				}
				return;
			}
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		Canvas canvas = _Draggable.FindInParents<Canvas>(base.gameObject);
		if (canvas == null)
		{
			return;
		}
		Image component = base.GetComponent<Image>();
		this.m_DraggingIcon = new GameObject("icon");
		this.m_DraggingIcon.transform.SetParent(canvas.transform, false);
		this.m_DraggingIcon.transform.SetAsLastSibling();
		Image image = this.m_DraggingIcon.AddComponent<Image>();
		CanvasGroup canvasGroup = this.m_DraggingIcon.AddComponent<CanvasGroup>();
		canvasGroup.blocksRaycasts = false;
		image.sprite = component.sprite;
		if (this.m_CopyChildrenImages)
		{
			this.CopyChildrenImages(component, image);
		}
		image.SetNativeSize();
		if (this.dragOnSurfaces)
		{
			this.m_DraggingPlane = (base.transform as RectTransform);
		}
		else
		{
			this.m_DraggingPlane = (canvas.transform as RectTransform);
		}
		_Draggable.m_draggingObject = base.gameObject;
		this.SetDraggedPosition(eventData);
	}

	public void OnDrag(PointerEventData data)
	{
		if (this.m_DraggingIcon != null)
		{
			this.SetDraggedPosition(data);
		}
	}

	private void SetDraggedPosition(PointerEventData data)
	{
		if (this.dragOnSurfaces)
		{
			if (data.pointerEnter != null)
			{
				if (data.pointerEnter.transform as RectTransform != null)
				{
					this.m_DraggingPlane = (data.pointerEnter.transform as RectTransform);
				}
			}
		}
		RectTransform component = this.m_DraggingIcon.GetComponent<RectTransform>();
		Vector3 position;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this.m_DraggingPlane, data.position, data.pressEventCamera, out position))
		{
			component.position = position;
			component.rotation = this.m_DraggingPlane.rotation;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (this.m_DraggingIcon != null)
		{
			UnityEngine.Object.Destroy(this.m_DraggingIcon);
		}
		_Draggable.m_draggingObject = null;
	}

	public static T FindInParents<T>(GameObject go) where T : Component
	{
		if (go == null)
		{
			return (T)((object)null);
		}
		T component = go.GetComponent<T>();
		if (component != null)
		{
			return component;
		}
		Transform parent = go.transform.parent;
		while (parent != null)
		{
			if (!(component == null))
			{
				break;
			}
			component = parent.gameObject.GetComponent<T>();
			parent = parent.parent;
		}
		return component;
	}
}
