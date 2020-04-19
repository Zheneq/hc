using System;
using System.Collections.Generic;
using UnityEngine;

public class ChatterComponent : MonoBehaviour, IGameEventListener
{
	public List<ScriptableObject> m_chatters = new List<ScriptableObject>();

	private CharacterResourceLink m_characterResourceLink;

	private ChatterEventOverrider m_eventOverrider;

	private List<GameEventManager.EventType> m_registeredEvents = new List<GameEventManager.EventType>();

	public void Awake()
	{
		this.m_registeredEvents.Clear();
		foreach (ScriptableObject scriptableObject in this.m_chatters)
		{
			IChatterData chatterData = (IChatterData)scriptableObject;
			if (chatterData == null)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ChatterComponent.Awake()).MethodHandle;
				}
				Log.Error("Chatter component on " + base.gameObject.name + " contains a null chatter entry", new object[0]);
			}
			else
			{
				if (!this.m_registeredEvents.Contains(chatterData.GetActivateOnEvent()))
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_registeredEvents.Add(chatterData.GetActivateOnEvent());
				}
				if (chatterData.GetCommonData().m_oncePerTurn)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!this.m_registeredEvents.Contains(GameEventManager.EventType.TurnTick))
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						this.m_registeredEvents.Add(GameEventManager.EventType.TurnTick);
					}
				}
			}
		}
		foreach (GameEventManager.EventType eventType in this.m_registeredEvents)
		{
			GameEventManager.Get().AddListener(this, eventType);
		}
	}

	private void OnDestroy()
	{
		if (this.m_registeredEvents != null)
		{
			using (List<GameEventManager.EventType>.Enumerator enumerator = this.m_registeredEvents.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameEventManager.EventType eventType = enumerator.Current;
					GameEventManager.Get().RemoveListener(this, eventType);
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ChatterComponent.OnDestroy()).MethodHandle;
				}
			}
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		foreach (ScriptableObject scriptableObject in this.m_chatters)
		{
			IChatterData chatterData = (IChatterData)scriptableObject;
			if (chatterData != null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ChatterComponent.OnGameEvent(GameEventManager.EventType, GameEventManager.GameEventArgs)).MethodHandle;
				}
				if (eventType == GameEventManager.EventType.TurnTick)
				{
					if (chatterData.GetCommonData().m_oncePerTurn)
					{
						chatterData.GetCommonData().OnTurnTick();
					}
				}
				else if (chatterData.ShouldPlayChatter(eventType, args, this))
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.m_eventOverrider != null)
					{
						this.m_eventOverrider.OnSubmitChatter(chatterData, eventType, args);
					}
					ChatterManager.Get().SubmitChatter(chatterData, base.gameObject);
				}
			}
		}
	}

	public void SetCharacterResourceLink(CharacterResourceLink characterResourceLink)
	{
		if (this.m_characterResourceLink)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ChatterComponent.SetCharacterResourceLink(CharacterResourceLink)).MethodHandle;
			}
			if (this.m_characterResourceLink != characterResourceLink)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				Debug.LogError("Character resource link is being changed; this should never happen");
			}
		}
		this.m_characterResourceLink = characterResourceLink;
	}

	public CharacterResourceLink GetCharacterResourceLink()
	{
		return this.m_characterResourceLink;
	}

	public void SetEventOverrider(ChatterEventOverrider eventOverrider)
	{
		this.m_eventOverrider = eventOverrider;
	}
}
