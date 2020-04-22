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
		return m_draggingObject;
	}

	private void CopyChildrenImages(Image sourceImage, Image parent)
	{
		if (parent == null)
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (sourceImage == null)
			{
				while (true)
				{
					switch (3)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			Image[] componentsInChildren = sourceImage.GetComponentsInChildren<Image>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Image component = componentsInChildren[i].GetComponent<Image>();
				if (component.gameObject == sourceImage.gameObject)
				{
					continue;
				}
				GameObject gameObject = new GameObject();
				gameObject.transform.SetParent(parent.transform, false);
				gameObject.transform.SetAsLastSibling();
				Image image = gameObject.AddComponent<Image>();
				CanvasGroup canvasGroup = gameObject.AddComponent<CanvasGroup>();
				canvasGroup.blocksRaycasts = false;
				image.sprite = component.sprite;
				image.color = component.color;
				if (m_CopyChildrenImages)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					CopyChildrenImages(component, image);
				}
				image.SetNativeSize();
			}
			while (true)
			{
				switch (4)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		Canvas canvas = FindInParents<Canvas>(base.gameObject);
		if (canvas == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		Image component = GetComponent<Image>();
		m_DraggingIcon = new GameObject("icon");
		m_DraggingIcon.transform.SetParent(canvas.transform, false);
		m_DraggingIcon.transform.SetAsLastSibling();
		Image image = m_DraggingIcon.AddComponent<Image>();
		CanvasGroup canvasGroup = m_DraggingIcon.AddComponent<CanvasGroup>();
		canvasGroup.blocksRaycasts = false;
		image.sprite = component.sprite;
		if (m_CopyChildrenImages)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			CopyChildrenImages(component, image);
		}
		image.SetNativeSize();
		if (dragOnSurfaces)
		{
			m_DraggingPlane = (base.transform as RectTransform);
		}
		else
		{
			m_DraggingPlane = (canvas.transform as RectTransform);
		}
		m_draggingObject = base.gameObject;
		SetDraggedPosition(eventData);
	}

	public void OnDrag(PointerEventData data)
	{
		if (!(m_DraggingIcon != null))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			SetDraggedPosition(data);
			return;
		}
	}

	private void SetDraggedPosition(PointerEventData data)
	{
		if (dragOnSurfaces)
		{
			while (true)
			{
				switch (3)
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
			if (data.pointerEnter != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (data.pointerEnter.transform as RectTransform != null)
				{
					m_DraggingPlane = (data.pointerEnter.transform as RectTransform);
				}
			}
		}
		RectTransform component = m_DraggingIcon.GetComponent<RectTransform>();
		if (!RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, data.position, data.pressEventCamera, out Vector3 worldPoint))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			component.position = worldPoint;
			component.rotation = m_DraggingPlane.rotation;
			return;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (m_DraggingIcon != null)
		{
			Object.Destroy(m_DraggingIcon);
		}
		m_draggingObject = null;
	}

	public static T FindInParents<T>(GameObject go) where T : Component
	{
		if (go == null)
		{
			return (T)null;
		}
		T component = go.GetComponent<T>();
		if ((Object)component != (Object)null)
		{
			return component;
		}
		Transform parent = go.transform.parent;
		while (parent != null)
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
			if (!((Object)component == (Object)null))
			{
				break;
			}
			component = parent.gameObject.GetComponent<T>();
			parent = parent.parent;
		}
		return component;
	}
}
