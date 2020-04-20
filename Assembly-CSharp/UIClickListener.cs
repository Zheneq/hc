using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIClickListener : MonoBehaviour
{
	private static UIClickListener s_instance;

	private List<GameObject> m_hitboxes;

	private Action m_closeAction;

	public static UIClickListener Get()
	{
		return UIClickListener.s_instance;
	}

	private void Awake()
	{
		UIClickListener.s_instance = this;
		this.Disable();
	}

	public void Enable(List<GameObject> hitboxes, Action closeAction)
	{
		this.m_hitboxes = hitboxes;
		this.m_closeAction = closeAction;
		base.gameObject.SetActive(true);
	}

	public void Disable()
	{
		base.gameObject.SetActive(false);
		this.m_closeAction = null;
		this.m_hitboxes = null;
	}

	private void Update()
	{
		if (base.gameObject.activeInHierarchy)
		{
			if (this.m_closeAction != null)
			{
				if (!Input.GetMouseButtonDown(0))
				{
					if (!Input.GetMouseButtonDown(1))
					{
						return;
					}
				}
				PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
				{
					pointerId = -1
				};
				pointerEventData.position = Input.mousePosition;
				if (this.m_hitboxes != null)
				{
					List<RaycastResult> list = new List<RaycastResult>();
					EventSystem.current.RaycastAll(pointerEventData, list);
					for (int i = 0; i < list.Count; i++)
					{
						for (int j = 0; j < this.m_hitboxes.Count; j++)
						{
							if (list[i].gameObject.GetInstanceID() == this.m_hitboxes[j].GetInstanceID())
							{
								return;
							}
						}
					}
				}
				this.m_closeAction();
				base.gameObject.SetActive(false);
				return;
			}
		}
	}
}
