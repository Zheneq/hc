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
		if (newHandler == null)
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_PointerEnterHandlers.Add(newHandler);
			return;
		}
	}

	public void AddNewHandler(IPointerExitHandler newHandler)
	{
		if (newHandler == null)
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_PointerExitHandlers.Add(newHandler);
			return;
		}
	}

	public void AddNewHandler(IPointerClickHandler newHandler)
	{
		if (newHandler == null)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_PointerClickHandlers.Add(newHandler);
			return;
		}
	}

	public void AddNewHandler(IPointerDownHandler newHandler)
	{
		if (newHandler == null)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_PointerDownHandlers.Add(newHandler);
			return;
		}
	}

	public void AddNewHandler(IScrollHandler newHandler)
	{
		if (newHandler == null)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_PointerScrollHandlers.Add(newHandler);
			return;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		for (int i = 0; i < m_PointerEnterHandlers.Count; i++)
		{
			if (m_PointerEnterHandlers[i] != null)
			{
				while (true)
				{
					switch (1)
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
				m_PointerEnterHandlers[i].OnPointerEnter(eventData);
			}
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		for (int i = 0; i < m_PointerExitHandlers.Count; i++)
		{
			if (m_PointerExitHandlers[i] != null)
			{
				while (true)
				{
					switch (1)
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
				m_PointerExitHandlers[i].OnPointerExit(eventData);
			}
		}
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

	public void OnPointerDown(PointerEventData eventData)
	{
		for (int i = 0; i < m_PointerDownHandlers.Count; i++)
		{
			if (m_PointerDownHandlers[i] != null)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_PointerDownHandlers[i].OnPointerDown(eventData);
			}
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		for (int i = 0; i < m_PointerClickHandlers.Count; i++)
		{
			if (m_PointerClickHandlers[i] != null)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_PointerClickHandlers[i].OnPointerClick(eventData);
			}
		}
	}

	public void OnScroll(PointerEventData eventData)
	{
		for (int i = 0; i < m_PointerScrollHandlers.Count; i++)
		{
			if (m_PointerScrollHandlers[i] != null)
			{
				while (true)
				{
					switch (2)
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
				m_PointerScrollHandlers[i].OnScroll(eventData);
			}
		}
	}
}
