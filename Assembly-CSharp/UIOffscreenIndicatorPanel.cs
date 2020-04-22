using System.Collections.Generic;
using UnityEngine;

public class UIOffscreenIndicatorPanel : MonoBehaviour
{
	public UIOffscreenIndicator m_offscreenIndicatorPrefab;

	public UILastKnownPosIndicator m_lastKnownPosIndicatorPrefab;

	public float borderLeft;

	public float borderRight;

	public float borderTop;

	public float borderBottom;

	public float m_indicatorScale = 1f;

	public float m_lastKnownPosIndicatorScale = 0.75f;

	private List<UIBaseIndicator> m_uiIndicators = new List<UIBaseIndicator>();

	public void AddActor(ActorData actorData)
	{
		UIOffscreenIndicator uIOffscreenIndicator = Object.Instantiate(m_offscreenIndicatorPrefab);
		m_uiIndicators.Add(uIOffscreenIndicator);
		uIOffscreenIndicator.transform.SetParent(base.transform);
		uIOffscreenIndicator.Setup(actorData, this);
		uIOffscreenIndicator.transform.localPosition = Vector3.zero;
		uIOffscreenIndicator.transform.localScale = new Vector3(m_indicatorScale, m_indicatorScale, m_indicatorScale);
		if (uIOffscreenIndicator.transform as RectTransform != null)
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
			(uIOffscreenIndicator.transform as RectTransform).anchoredPosition = new Vector2(10000f, 10000f);
		}
		UILastKnownPosIndicator uILastKnownPosIndicator = Object.Instantiate(m_lastKnownPosIndicatorPrefab);
		m_uiIndicators.Add(uILastKnownPosIndicator);
		uILastKnownPosIndicator.transform.SetParent(base.transform);
		uILastKnownPosIndicator.Setup(actorData, this);
		uILastKnownPosIndicator.transform.localPosition = Vector3.zero;
		uILastKnownPosIndicator.transform.localScale = new Vector3(m_lastKnownPosIndicatorScale, m_lastKnownPosIndicatorScale, m_lastKnownPosIndicatorScale);
		if (!(uILastKnownPosIndicator.transform as RectTransform != null))
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
			(uILastKnownPosIndicator.transform as RectTransform).anchoredPosition = new Vector2(10000f, 10000f);
			return;
		}
	}

	public void AddControlPoint(ControlPoint controlPoint)
	{
		UIOffscreenIndicator uIOffscreenIndicator = Object.Instantiate(m_offscreenIndicatorPrefab);
		m_uiIndicators.Add(uIOffscreenIndicator);
		uIOffscreenIndicator.transform.SetParent(base.transform);
		uIOffscreenIndicator.Setup(controlPoint, this);
		uIOffscreenIndicator.transform.localScale = new Vector3(m_indicatorScale, m_indicatorScale, m_indicatorScale);
		uIOffscreenIndicator.transform.localPosition = Vector3.zero;
	}

	public void AddCtfFlag(CTF_Flag flag)
	{
		UIOffscreenIndicator uIOffscreenIndicator = Object.Instantiate(m_offscreenIndicatorPrefab);
		m_uiIndicators.Add(uIOffscreenIndicator);
		uIOffscreenIndicator.transform.SetParent(base.transform);
		uIOffscreenIndicator.Setup(flag, this);
		uIOffscreenIndicator.transform.localScale = new Vector3(m_indicatorScale, m_indicatorScale, m_indicatorScale);
		uIOffscreenIndicator.transform.localPosition = Vector3.zero;
	}

	public void AddCtfFlagTurnInRegion(BoardRegion region, Team teamRegion = Team.Invalid)
	{
		List<UIBaseIndicator> list = m_uiIndicators.FindAll((UIBaseIndicator indicator) => indicator.GetAttachedRegion() == region);
		if (list.Count > 0)
		{
			while (true)
			{
				switch (6)
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
		UIOffscreenIndicator uIOffscreenIndicator = Object.Instantiate(m_offscreenIndicatorPrefab);
		m_uiIndicators.Add(uIOffscreenIndicator);
		uIOffscreenIndicator.transform.SetParent(base.transform);
		uIOffscreenIndicator.Setup(region, this, teamRegion, true);
		uIOffscreenIndicator.transform.localScale = new Vector3(m_indicatorScale, m_indicatorScale, m_indicatorScale);
		uIOffscreenIndicator.transform.localPosition = Vector3.zero;
	}

	public void AddPing(UIWorldPing ping, ActorController.PingType pingType, ActorData pingerActorData)
	{
		UIOffscreenIndicator uIOffscreenIndicator = Object.Instantiate(m_offscreenIndicatorPrefab);
		m_uiIndicators.Add(uIOffscreenIndicator);
		uIOffscreenIndicator.transform.SetParent(base.transform);
		uIOffscreenIndicator.Setup(ping, pingType, pingerActorData, this);
		uIOffscreenIndicator.transform.localScale = new Vector3(m_indicatorScale, m_indicatorScale, m_indicatorScale);
		uIOffscreenIndicator.transform.localPosition = Vector3.zero;
	}

	public void RemoveCtfFlagTurnInRegion(BoardRegion region)
	{
		List<UIBaseIndicator> list = m_uiIndicators.FindAll((UIBaseIndicator indicator) => indicator.GetAttachedRegion() == region);
		using (List<UIBaseIndicator>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIBaseIndicator current = enumerator.Current;
				if (current != null)
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
					if (current.gameObject != null)
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
						Object.Destroy(current.gameObject);
					}
				}
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_uiIndicators.RemoveAll((UIBaseIndicator indicator) => indicator.GetAttachedRegion() == region);
	}

	public void RemoveActor(ActorData actorData)
	{
		List<UIBaseIndicator> list = m_uiIndicators.FindAll((UIBaseIndicator indicator) => indicator.GetAttachedActor() == actorData);
		using (List<UIBaseIndicator>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIBaseIndicator current = enumerator.Current;
				if (current != null)
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
					if (current.gameObject != null)
					{
						Object.Destroy(current.gameObject);
					}
				}
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_uiIndicators.RemoveAll((UIBaseIndicator indicator) => indicator.GetAttachedActor() == actorData);
	}

	public void RemoveControlPoint(ControlPoint controlPoint)
	{
		List<UIBaseIndicator> list = m_uiIndicators.FindAll((UIBaseIndicator indicator) => indicator.GetAttachedControlPoint() == controlPoint);
		using (List<UIBaseIndicator>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIBaseIndicator current = enumerator.Current;
				if (current != null)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (current.gameObject != null)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						Object.Destroy(current.gameObject);
					}
				}
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_uiIndicators.RemoveAll((UIBaseIndicator indicator) => indicator.GetAttachedControlPoint() == controlPoint);
	}

	public void RemoveCtfFlag(CTF_Flag flag)
	{
		List<UIBaseIndicator> list = m_uiIndicators.FindAll((UIBaseIndicator indicator) => indicator.GetAttachedFlag() == flag);
		using (List<UIBaseIndicator>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIBaseIndicator current = enumerator.Current;
				if (current != null)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (current.gameObject != null)
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
						Object.Destroy(current.gameObject);
					}
				}
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_uiIndicators.RemoveAll((UIBaseIndicator indicator) => indicator.GetAttachedFlag() == flag);
	}

	public void RemovePing(UIWorldPing ping)
	{
		List<UIBaseIndicator> list = m_uiIndicators.FindAll((UIBaseIndicator indicator) => indicator.GetAttachedPing() == ping);
		using (List<UIBaseIndicator>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIBaseIndicator current = enumerator.Current;
				if (current != null)
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
					if (current.gameObject != null)
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
						Object.Destroy(current.gameObject);
					}
				}
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_uiIndicators.RemoveAll((UIBaseIndicator indicator) => indicator.GetAttachedPing() == ping);
	}

	private void Clear()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			Object.Destroy(child.gameObject);
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_uiIndicators.Clear();
			return;
		}
	}

	public void MarkFramesForForceUpdate()
	{
		for (int i = 0; i < m_uiIndicators.Count; i++)
		{
			UIBaseIndicator uIBaseIndicator = m_uiIndicators[i];
			if (uIBaseIndicator != null)
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
				uIBaseIndicator.MarkFrameForUpdate();
			}
		}
	}

	private void OnValidate()
	{
		foreach (UIBaseIndicator uiIndicator in m_uiIndicators)
		{
			if (uiIndicator is UIOffscreenIndicator)
			{
				uiIndicator.transform.localScale = new Vector3(m_indicatorScale, m_indicatorScale, m_indicatorScale);
			}
			else if (uiIndicator is UILastKnownPosIndicator)
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
				uiIndicator.transform.localScale = new Vector3(m_lastKnownPosIndicatorScale, m_lastKnownPosIndicatorScale, m_lastKnownPosIndicatorScale);
			}
		}
	}
}
