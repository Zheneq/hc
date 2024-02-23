using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ChatterComponent : MonoBehaviour, IGameEventListener
{
	public List<ScriptableObject> m_chatters = new List<ScriptableObject>();

	private CharacterResourceLink m_characterResourceLink;

	private ChatterEventOverrider m_eventOverrider;

	private List<GameEventManager.EventType> m_registeredEvents = new List<GameEventManager.EventType>();

	public void Awake()
	{
		m_registeredEvents.Clear();
		foreach (IChatterData chatter in m_chatters)
		{
			if (chatter == null)
			{
				Log.Error(new StringBuilder().Append("Chatter component on ").Append(base.gameObject.name).Append(" contains a null chatter entry").ToString());
			}
			else
			{
				if (!m_registeredEvents.Contains(chatter.GetActivateOnEvent()))
				{
					m_registeredEvents.Add(chatter.GetActivateOnEvent());
				}
				if (chatter.GetCommonData().m_oncePerTurn)
				{
					if (!m_registeredEvents.Contains(GameEventManager.EventType.TurnTick))
					{
						m_registeredEvents.Add(GameEventManager.EventType.TurnTick);
					}
				}
			}
		}
		foreach (GameEventManager.EventType registeredEvent in m_registeredEvents)
		{
			GameEventManager.Get().AddListener(this, registeredEvent);
		}
	}

	private void OnDestroy()
	{
		if (m_registeredEvents != null)
		{
			using (List<GameEventManager.EventType>.Enumerator enumerator = m_registeredEvents.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameEventManager.EventType current = enumerator.Current;
					GameEventManager.Get().RemoveListener(this, current);
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return;
					}
				}
			}
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		foreach (IChatterData chatter in m_chatters)
		{
			if (chatter != null)
			{
				if (eventType == GameEventManager.EventType.TurnTick)
				{
					if (chatter.GetCommonData().m_oncePerTurn)
					{
						chatter.GetCommonData().OnTurnTick();
					}
				}
				else if (chatter.ShouldPlayChatter(eventType, args, this))
				{
					if (m_eventOverrider != null)
					{
						m_eventOverrider.OnSubmitChatter(chatter, eventType, args);
					}
					ChatterManager.Get().SubmitChatter(chatter, base.gameObject);
				}
			}
		}
	}

	public void SetCharacterResourceLink(CharacterResourceLink characterResourceLink)
	{
		if ((bool)m_characterResourceLink)
		{
			if (m_characterResourceLink != characterResourceLink)
			{
				Debug.LogError("Character resource link is being changed; this should never happen");
			}
		}
		m_characterResourceLink = characterResourceLink;
	}

	public CharacterResourceLink GetCharacterResourceLink()
	{
		return m_characterResourceLink;
	}

	public void SetEventOverrider(ChatterEventOverrider eventOverrider)
	{
		m_eventOverrider = eventOverrider;
	}
}
