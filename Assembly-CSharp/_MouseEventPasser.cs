using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class _MouseEventPasser : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerClickHandler, IScrollHandler, IEventSystemHandler
{
	private List<IPointerEnterHandler> m_PointerEnterHandlers = new List<IPointerEnterHandler>();

	private List<IPointerExitHandler> m_PointerExitHandlers = new List<IPointerExitHandler>();

	private List<IPointerDownHandler> m_PointerDownHandlers = new List<IPointerDownHandler>();

	private List<IPointerClickHandler> m_PointerClickHandlers = new List<IPointerClickHandler>();

	private List<IScrollHandler> m_PointerScrollHandlers = new List<IScrollHandler>();

	public void AddNewHandler(IPointerEnterHandler newHandler)
	{
		if (newHandler != null)
		{
			this.m_PointerEnterHandlers.Add(newHandler);
		}
	}

	public void AddNewHandler(IPointerExitHandler newHandler)
	{
		if (newHandler != null)
		{
			this.m_PointerExitHandlers.Add(newHandler);
		}
	}

	public void AddNewHandler(IPointerClickHandler newHandler)
	{
		if (newHandler != null)
		{
			this.m_PointerClickHandlers.Add(newHandler);
		}
	}

	public void AddNewHandler(IPointerDownHandler newHandler)
	{
		if (newHandler != null)
		{
			this.m_PointerDownHandlers.Add(newHandler);
		}
	}

	public void AddNewHandler(IScrollHandler newHandler)
	{
		if (newHandler != null)
		{
			this.m_PointerScrollHandlers.Add(newHandler);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		for (int i = 0; i < this.m_PointerEnterHandlers.Count; i++)
		{
			if (this.m_PointerEnterHandlers[i] != null)
			{
				this.m_PointerEnterHandlers[i].OnPointerEnter(eventData);
			}
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		for (int i = 0; i < this.m_PointerExitHandlers.Count; i++)
		{
			if (this.m_PointerExitHandlers[i] != null)
			{
				this.m_PointerExitHandlers[i].OnPointerExit(eventData);
			}
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		for (int i = 0; i < this.m_PointerDownHandlers.Count; i++)
		{
			if (this.m_PointerDownHandlers[i] != null)
			{
				this.m_PointerDownHandlers[i].OnPointerDown(eventData);
			}
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		for (int i = 0; i < this.m_PointerClickHandlers.Count; i++)
		{
			if (this.m_PointerClickHandlers[i] != null)
			{
				this.m_PointerClickHandlers[i].OnPointerClick(eventData);
			}
		}
	}

	public void OnScroll(PointerEventData eventData)
	{
		for (int i = 0; i < this.m_PointerScrollHandlers.Count; i++)
		{
			if (this.m_PointerScrollHandlers[i] != null)
			{
				this.m_PointerScrollHandlers[i].OnScroll(eventData);
			}
		}
	}
}
