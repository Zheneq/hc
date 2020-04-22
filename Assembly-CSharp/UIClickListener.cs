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
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		Disable();
	}

	public void Enable(List<GameObject> hitboxes, Action closeAction)
	{
		m_hitboxes = hitboxes;
		m_closeAction = closeAction;
		base.gameObject.SetActive(true);
	}

	public void Disable()
	{
		base.gameObject.SetActive(false);
		m_closeAction = null;
		m_hitboxes = null;
	}

	private void Update()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		while (true)
		{
			if (m_closeAction == null)
			{
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
			if (!Input.GetMouseButtonDown(0))
			{
				if (!Input.GetMouseButtonDown(1))
				{
					return;
				}
			}
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			pointerEventData.pointerId = -1;
			PointerEventData pointerEventData2 = pointerEventData;
			pointerEventData2.position = Input.mousePosition;
			if (m_hitboxes != null)
			{
				List<RaycastResult> list = new List<RaycastResult>();
				EventSystem.current.RaycastAll(pointerEventData2, list);
				for (int i = 0; i < list.Count; i++)
				{
					for (int j = 0; j < m_hitboxes.Count; j++)
					{
						if (list[i].gameObject.GetInstanceID() == m_hitboxes[j].GetInstanceID())
						{
							while (true)
							{
								switch (2)
								{
								default:
									return;
								case 0:
									break;
								}
							}
						}
					}
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							goto end_IL_00fd;
						}
						continue;
						end_IL_00fd:
						break;
					}
				}
			}
			m_closeAction();
			base.gameObject.SetActive(false);
			return;
		}
	}
}
