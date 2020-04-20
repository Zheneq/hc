using System;
using System.Collections.Generic;
using UnityEngine;

public class UICharacterMovementPanel : MonoBehaviour
{
	public UICharacterMovementItem m_itemPrefab;

	public UICharacterMovementItem m_respawnItemPrefab;

	private Dictionary<BoardSquare, UICharacterMovementItem> m_displayedCharacters = new Dictionary<BoardSquare, UICharacterMovementItem>();

	private Dictionary<BoardSquare, UICharacterMovementItem> m_displayedRespawnCharacters = new Dictionary<BoardSquare, UICharacterMovementItem>();

	private List<UICharacterMovementItem> m_recycledMoveIndicatorInstances = new List<UICharacterMovementItem>();

	private List<UICharacterMovementItem> m_recycledRespawnIndicatorInstances = new List<UICharacterMovementItem>();

	private static UICharacterMovementPanel s_instance;

	private void Awake()
	{
		UICharacterMovementPanel.s_instance = this;
	}

	private void OnDestroy()
	{
		this.DestroyRecycledInstances(this.m_recycledMoveIndicatorInstances);
		this.DestroyRecycledInstances(this.m_recycledRespawnIndicatorInstances);
		UICharacterMovementPanel.s_instance = null;
	}

	public static UICharacterMovementPanel Get()
	{
		return UICharacterMovementPanel.s_instance;
	}

	public Dictionary<BoardSquare, UICharacterMovementItem> GetIndicators()
	{
		return this.m_displayedCharacters;
	}

	private void DestroyRecycledInstances(List<UICharacterMovementItem> instances)
	{
		if (instances != null)
		{
			for (int i = 0; i < instances.Count; i++)
			{
				UnityEngine.Object.Destroy(instances[i].gameObject);
			}
			instances.Clear();
		}
	}

	public void RemoveMovementIndicator(ActorData data)
	{
		this.RemoveIndicator(data, this.m_displayedCharacters, UICharacterMovementPanel.IndicatorType.Movement);
	}

	public void AddMovementIndicator(BoardSquare square, ActorData data)
	{
		this.AddIndicator(square, data, this.m_displayedCharacters, this.m_itemPrefab, UICharacterMovementPanel.IndicatorType.Movement);
	}

	public void RemoveRespawnIndicator(ActorData data)
	{
		this.RemoveIndicator(data, this.m_displayedRespawnCharacters, UICharacterMovementPanel.IndicatorType.Respawn);
	}

	public void AddRespawnIndicator(BoardSquare square, ActorData data)
	{
		this.AddIndicator(square, data, this.m_displayedRespawnCharacters, this.m_respawnItemPrefab, UICharacterMovementPanel.IndicatorType.Respawn);
	}

	private void RemoveIndicator(ActorData data, Dictionary<BoardSquare, UICharacterMovementItem> displayedCharacters, UICharacterMovementPanel.IndicatorType indicatorType)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		using (Dictionary<BoardSquare, UICharacterMovementItem>.Enumerator enumerator = displayedCharacters.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<BoardSquare, UICharacterMovementItem> keyValuePair = enumerator.Current;
				if (keyValuePair.Value.Actors.Contains(data))
				{
					if (keyValuePair.Value.RemoveActor(data))
					{
						this.RecycleInstance(keyValuePair.Value, indicatorType);
						list.Add(keyValuePair.Key);
					}
				}
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			displayedCharacters.Remove(list[i]);
		}
	}

	private void AddIndicator(BoardSquare square, ActorData data, Dictionary<BoardSquare, UICharacterMovementItem> displayedCharacters, UICharacterMovementItem itemPrefab, UICharacterMovementPanel.IndicatorType indicatorType)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		using (Dictionary<BoardSquare, UICharacterMovementItem>.Enumerator enumerator = displayedCharacters.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<BoardSquare, UICharacterMovementItem> keyValuePair = enumerator.Current;
				if (keyValuePair.Value.Actors.Contains(data))
				{
					if (!(keyValuePair.Key != square))
					{
						return;
					}
					if (keyValuePair.Value.RemoveActor(data))
					{
						this.RecycleInstance(keyValuePair.Value, indicatorType);
						list.Add(keyValuePair.Key);
					}
				}
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			displayedCharacters.Remove(list[i]);
		}
		if (displayedCharacters.ContainsKey(square))
		{
			displayedCharacters[square].AddActor(data);
		}
		else
		{
			UICharacterMovementItem uicharacterMovementItem = this.PopRecycledInstance(indicatorType);
			if (uicharacterMovementItem == null)
			{
				uicharacterMovementItem = UnityEngine.Object.Instantiate<UICharacterMovementItem>(itemPrefab);
			}
			uicharacterMovementItem.transform.SetParent(base.gameObject.transform);
			uicharacterMovementItem.transform.localEulerAngles = Vector3.zero;
			uicharacterMovementItem.transform.localScale = Vector3.one;
			uicharacterMovementItem.transform.localPosition = Vector3.zero;
			uicharacterMovementItem.Setup(square, data);
			displayedCharacters[square] = uicharacterMovementItem;
		}
	}

	private void RecycleInstance(UICharacterMovementItem item, UICharacterMovementPanel.IndicatorType indicatorType)
	{
		if (item != null)
		{
			if (indicatorType == UICharacterMovementPanel.IndicatorType.Movement)
			{
				this.m_recycledMoveIndicatorInstances.Add(item);
			}
			else if (indicatorType == UICharacterMovementPanel.IndicatorType.Respawn)
			{
				this.m_recycledRespawnIndicatorInstances.Add(item);
			}
			else
			{
				Log.Warning(base.GetType() + " Trying to recycle unknown indicator type", new object[0]);
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}
	}

	private UICharacterMovementItem PopRecycledInstance(UICharacterMovementPanel.IndicatorType indicatorType)
	{
		UICharacterMovementItem result = null;
		List<UICharacterMovementItem> list = null;
		if (indicatorType == UICharacterMovementPanel.IndicatorType.Movement)
		{
			list = this.m_recycledMoveIndicatorInstances;
		}
		else if (indicatorType == UICharacterMovementPanel.IndicatorType.Respawn)
		{
			list = this.m_recycledRespawnIndicatorInstances;
		}
		if (list != null)
		{
			if (list.Count > 0)
			{
				result = list[0];
				list.RemoveAt(0);
			}
		}
		return result;
	}

	private enum IndicatorType
	{
		Movement,
		Respawn
	}
}
