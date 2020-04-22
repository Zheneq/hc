using System.Collections.Generic;
using UnityEngine;

public class UICharacterMovementPanel : MonoBehaviour
{
	private enum IndicatorType
	{
		Movement,
		Respawn
	}

	public UICharacterMovementItem m_itemPrefab;

	public UICharacterMovementItem m_respawnItemPrefab;

	private Dictionary<BoardSquare, UICharacterMovementItem> m_displayedCharacters = new Dictionary<BoardSquare, UICharacterMovementItem>();

	private Dictionary<BoardSquare, UICharacterMovementItem> m_displayedRespawnCharacters = new Dictionary<BoardSquare, UICharacterMovementItem>();

	private List<UICharacterMovementItem> m_recycledMoveIndicatorInstances = new List<UICharacterMovementItem>();

	private List<UICharacterMovementItem> m_recycledRespawnIndicatorInstances = new List<UICharacterMovementItem>();

	private static UICharacterMovementPanel s_instance;

	private void Awake()
	{
		s_instance = this;
	}

	private void OnDestroy()
	{
		DestroyRecycledInstances(m_recycledMoveIndicatorInstances);
		DestroyRecycledInstances(m_recycledRespawnIndicatorInstances);
		s_instance = null;
	}

	public static UICharacterMovementPanel Get()
	{
		return s_instance;
	}

	public Dictionary<BoardSquare, UICharacterMovementItem> GetIndicators()
	{
		return m_displayedCharacters;
	}

	private void DestroyRecycledInstances(List<UICharacterMovementItem> instances)
	{
		if (instances == null)
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
			for (int i = 0; i < instances.Count; i++)
			{
				Object.Destroy(instances[i].gameObject);
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				instances.Clear();
				return;
			}
		}
	}

	public void RemoveMovementIndicator(ActorData data)
	{
		RemoveIndicator(data, m_displayedCharacters, IndicatorType.Movement);
	}

	public void AddMovementIndicator(BoardSquare square, ActorData data)
	{
		AddIndicator(square, data, m_displayedCharacters, m_itemPrefab, IndicatorType.Movement);
	}

	public void RemoveRespawnIndicator(ActorData data)
	{
		RemoveIndicator(data, m_displayedRespawnCharacters, IndicatorType.Respawn);
	}

	public void AddRespawnIndicator(BoardSquare square, ActorData data)
	{
		AddIndicator(square, data, m_displayedRespawnCharacters, m_respawnItemPrefab, IndicatorType.Respawn);
	}

	private void RemoveIndicator(ActorData data, Dictionary<BoardSquare, UICharacterMovementItem> displayedCharacters, IndicatorType indicatorType)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		using (Dictionary<BoardSquare, UICharacterMovementItem>.Enumerator enumerator = displayedCharacters.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<BoardSquare, UICharacterMovementItem> current = enumerator.Current;
				if (current.Value.Actors.Contains(data))
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
					if (current.Value.RemoveActor(data))
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
						RecycleInstance(current.Value, indicatorType);
						list.Add(current.Key);
					}
				}
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			displayedCharacters.Remove(list[i]);
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

	private void AddIndicator(BoardSquare square, ActorData data, Dictionary<BoardSquare, UICharacterMovementItem> displayedCharacters, UICharacterMovementItem itemPrefab, IndicatorType indicatorType)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		using (Dictionary<BoardSquare, UICharacterMovementItem>.Enumerator enumerator = displayedCharacters.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<BoardSquare, UICharacterMovementItem> current = enumerator.Current;
				if (current.Value.Actors.Contains(data))
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
					if (!(current.Key != square))
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
						break;
					}
					if (current.Value.RemoveActor(data))
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
						RecycleInstance(current.Value, indicatorType);
						list.Add(current.Key);
					}
				}
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			displayedCharacters.Remove(list[i]);
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (displayedCharacters.ContainsKey(square))
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						displayedCharacters[square].AddActor(data);
						return;
					}
				}
			}
			UICharacterMovementItem uICharacterMovementItem = PopRecycledInstance(indicatorType);
			if (uICharacterMovementItem == null)
			{
				uICharacterMovementItem = Object.Instantiate(itemPrefab);
			}
			uICharacterMovementItem.transform.SetParent(base.gameObject.transform);
			uICharacterMovementItem.transform.localEulerAngles = Vector3.zero;
			uICharacterMovementItem.transform.localScale = Vector3.one;
			uICharacterMovementItem.transform.localPosition = Vector3.zero;
			uICharacterMovementItem.Setup(square, data);
			displayedCharacters[square] = uICharacterMovementItem;
			return;
		}
	}

	private void RecycleInstance(UICharacterMovementItem item, IndicatorType indicatorType)
	{
		if (!(item != null))
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
			switch (indicatorType)
			{
			case IndicatorType.Movement:
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					m_recycledMoveIndicatorInstances.Add(item);
					return;
				}
			case IndicatorType.Respawn:
				m_recycledRespawnIndicatorInstances.Add(item);
				break;
			default:
				Log.Warning(string.Concat(GetType(), " Trying to recycle unknown indicator type"));
				Object.Destroy(item.gameObject);
				break;
			}
			return;
		}
	}

	private UICharacterMovementItem PopRecycledInstance(IndicatorType indicatorType)
	{
		UICharacterMovementItem result = null;
		List<UICharacterMovementItem> list = null;
		if (indicatorType == IndicatorType.Movement)
		{
			list = m_recycledMoveIndicatorInstances;
		}
		else if (indicatorType == IndicatorType.Respawn)
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
			list = m_recycledRespawnIndicatorInstances;
		}
		if (list != null)
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
			if (list.Count > 0)
			{
				result = list[0];
				list.RemoveAt(0);
			}
		}
		return result;
	}
}
