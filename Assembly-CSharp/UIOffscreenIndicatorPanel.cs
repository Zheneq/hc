using System;
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
		UIOffscreenIndicator uioffscreenIndicator = UnityEngine.Object.Instantiate<UIOffscreenIndicator>(this.m_offscreenIndicatorPrefab);
		this.m_uiIndicators.Add(uioffscreenIndicator);
		uioffscreenIndicator.transform.SetParent(base.transform);
		uioffscreenIndicator.Setup(actorData, this);
		uioffscreenIndicator.transform.localPosition = Vector3.zero;
		uioffscreenIndicator.transform.localScale = new Vector3(this.m_indicatorScale, this.m_indicatorScale, this.m_indicatorScale);
		if (uioffscreenIndicator.transform as RectTransform != null)
		{
			(uioffscreenIndicator.transform as RectTransform).anchoredPosition = new Vector2(10000f, 10000f);
		}
		UILastKnownPosIndicator uilastKnownPosIndicator = UnityEngine.Object.Instantiate<UILastKnownPosIndicator>(this.m_lastKnownPosIndicatorPrefab);
		this.m_uiIndicators.Add(uilastKnownPosIndicator);
		uilastKnownPosIndicator.transform.SetParent(base.transform);
		uilastKnownPosIndicator.Setup(actorData, this);
		uilastKnownPosIndicator.transform.localPosition = Vector3.zero;
		uilastKnownPosIndicator.transform.localScale = new Vector3(this.m_lastKnownPosIndicatorScale, this.m_lastKnownPosIndicatorScale, this.m_lastKnownPosIndicatorScale);
		if (uilastKnownPosIndicator.transform as RectTransform != null)
		{
			(uilastKnownPosIndicator.transform as RectTransform).anchoredPosition = new Vector2(10000f, 10000f);
		}
	}

	public void AddControlPoint(ControlPoint controlPoint)
	{
		UIOffscreenIndicator uioffscreenIndicator = UnityEngine.Object.Instantiate<UIOffscreenIndicator>(this.m_offscreenIndicatorPrefab);
		this.m_uiIndicators.Add(uioffscreenIndicator);
		uioffscreenIndicator.transform.SetParent(base.transform);
		uioffscreenIndicator.Setup(controlPoint, this);
		uioffscreenIndicator.transform.localScale = new Vector3(this.m_indicatorScale, this.m_indicatorScale, this.m_indicatorScale);
		uioffscreenIndicator.transform.localPosition = Vector3.zero;
	}

	public void AddCtfFlag(CTF_Flag flag)
	{
		UIOffscreenIndicator uioffscreenIndicator = UnityEngine.Object.Instantiate<UIOffscreenIndicator>(this.m_offscreenIndicatorPrefab);
		this.m_uiIndicators.Add(uioffscreenIndicator);
		uioffscreenIndicator.transform.SetParent(base.transform);
		uioffscreenIndicator.Setup(flag, this);
		uioffscreenIndicator.transform.localScale = new Vector3(this.m_indicatorScale, this.m_indicatorScale, this.m_indicatorScale);
		uioffscreenIndicator.transform.localPosition = Vector3.zero;
	}

	public void AddCtfFlagTurnInRegion(BoardRegion region, Team teamRegion = Team.Invalid)
	{
		List<UIBaseIndicator> list = this.m_uiIndicators.FindAll((UIBaseIndicator indicator) => indicator.GetAttachedRegion() == region);
		if (list.Count > 0)
		{
			return;
		}
		UIOffscreenIndicator uioffscreenIndicator = UnityEngine.Object.Instantiate<UIOffscreenIndicator>(this.m_offscreenIndicatorPrefab);
		this.m_uiIndicators.Add(uioffscreenIndicator);
		uioffscreenIndicator.transform.SetParent(base.transform);
		uioffscreenIndicator.Setup(region, this, teamRegion, true);
		uioffscreenIndicator.transform.localScale = new Vector3(this.m_indicatorScale, this.m_indicatorScale, this.m_indicatorScale);
		uioffscreenIndicator.transform.localPosition = Vector3.zero;
	}

	public void AddPing(UIWorldPing ping, ActorController.PingType pingType, ActorData pingerActorData)
	{
		UIOffscreenIndicator uioffscreenIndicator = UnityEngine.Object.Instantiate<UIOffscreenIndicator>(this.m_offscreenIndicatorPrefab);
		this.m_uiIndicators.Add(uioffscreenIndicator);
		uioffscreenIndicator.transform.SetParent(base.transform);
		uioffscreenIndicator.Setup(ping, pingType, pingerActorData, this);
		uioffscreenIndicator.transform.localScale = new Vector3(this.m_indicatorScale, this.m_indicatorScale, this.m_indicatorScale);
		uioffscreenIndicator.transform.localPosition = Vector3.zero;
	}

	public void RemoveCtfFlagTurnInRegion(BoardRegion region)
	{
		List<UIBaseIndicator> list = this.m_uiIndicators.FindAll((UIBaseIndicator indicator) => indicator.GetAttachedRegion() == region);
		using (List<UIBaseIndicator>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIBaseIndicator uibaseIndicator = enumerator.Current;
				if (uibaseIndicator != null)
				{
					if (uibaseIndicator.gameObject != null)
					{
						UnityEngine.Object.Destroy(uibaseIndicator.gameObject);
					}
				}
			}
		}
		this.m_uiIndicators.RemoveAll((UIBaseIndicator indicator) => indicator.GetAttachedRegion() == region);
	}

	public void RemoveActor(ActorData actorData)
	{
		List<UIBaseIndicator> list = this.m_uiIndicators.FindAll((UIBaseIndicator indicator) => indicator.GetAttachedActor() == actorData);
		using (List<UIBaseIndicator>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIBaseIndicator uibaseIndicator = enumerator.Current;
				if (uibaseIndicator != null)
				{
					if (uibaseIndicator.gameObject != null)
					{
						UnityEngine.Object.Destroy(uibaseIndicator.gameObject);
					}
				}
			}
		}
		this.m_uiIndicators.RemoveAll((UIBaseIndicator indicator) => indicator.GetAttachedActor() == actorData);
	}

	public void RemoveControlPoint(ControlPoint controlPoint)
	{
		List<UIBaseIndicator> list = this.m_uiIndicators.FindAll((UIBaseIndicator indicator) => indicator.GetAttachedControlPoint() == controlPoint);
		using (List<UIBaseIndicator>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIBaseIndicator uibaseIndicator = enumerator.Current;
				if (uibaseIndicator != null)
				{
					if (uibaseIndicator.gameObject != null)
					{
						UnityEngine.Object.Destroy(uibaseIndicator.gameObject);
					}
				}
			}
		}
		this.m_uiIndicators.RemoveAll((UIBaseIndicator indicator) => indicator.GetAttachedControlPoint() == controlPoint);
	}

	public void RemoveCtfFlag(CTF_Flag flag)
	{
		List<UIBaseIndicator> list = this.m_uiIndicators.FindAll((UIBaseIndicator indicator) => indicator.GetAttachedFlag() == flag);
		using (List<UIBaseIndicator>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIBaseIndicator uibaseIndicator = enumerator.Current;
				if (uibaseIndicator != null)
				{
					if (uibaseIndicator.gameObject != null)
					{
						UnityEngine.Object.Destroy(uibaseIndicator.gameObject);
					}
				}
			}
		}
		this.m_uiIndicators.RemoveAll((UIBaseIndicator indicator) => indicator.GetAttachedFlag() == flag);
	}

	public void RemovePing(UIWorldPing ping)
	{
		List<UIBaseIndicator> list = this.m_uiIndicators.FindAll((UIBaseIndicator indicator) => indicator.GetAttachedPing() == ping);
		using (List<UIBaseIndicator>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIBaseIndicator uibaseIndicator = enumerator.Current;
				if (uibaseIndicator != null)
				{
					if (uibaseIndicator.gameObject != null)
					{
						UnityEngine.Object.Destroy(uibaseIndicator.gameObject);
					}
				}
			}
		}
		this.m_uiIndicators.RemoveAll((UIBaseIndicator indicator) => indicator.GetAttachedPing() == ping);
	}

	private void Clear()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			UnityEngine.Object.Destroy(child.gameObject);
		}
		this.m_uiIndicators.Clear();
	}

	public void MarkFramesForForceUpdate()
	{
		for (int i = 0; i < this.m_uiIndicators.Count; i++)
		{
			UIBaseIndicator uibaseIndicator = this.m_uiIndicators[i];
			if (uibaseIndicator != null)
			{
				uibaseIndicator.MarkFrameForUpdate();
			}
		}
	}

	private void OnValidate()
	{
		foreach (UIBaseIndicator uibaseIndicator in this.m_uiIndicators)
		{
			if (uibaseIndicator is UIOffscreenIndicator)
			{
				uibaseIndicator.transform.localScale = new Vector3(this.m_indicatorScale, this.m_indicatorScale, this.m_indicatorScale);
			}
			else if (uibaseIndicator is UILastKnownPosIndicator)
			{
				uibaseIndicator.transform.localScale = new Vector3(this.m_lastKnownPosIndicatorScale, this.m_lastKnownPosIndicatorScale, this.m_lastKnownPosIndicatorScale);
			}
		}
	}
}
