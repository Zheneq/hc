using System;
using System.Collections.Generic;
using UnityEngine;

public class UICombatTextPanel : MonoBehaviour
{
	public float m_combatTextVisibleDuration = 2f;

	private List<CombatTextEntry> m_textEntries;

	private float m_lastTextCreatedTime = -1f;

	private static float m_timeBetweenText = 0.05f;

	public void QueueCombatText(ActorData actorData, string combatText, CombatTextCategory category, BuffIconToDisplay icon)
	{
		CombatTextEntry item = new CombatTextEntry(actorData, combatText, category, icon, this.m_combatTextVisibleDuration);
		if (this.m_textEntries != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICombatTextPanel.QueueCombatText(ActorData, string, CombatTextCategory, BuffIconToDisplay)).MethodHandle;
			}
			this.m_textEntries.Add(item);
		}
		else
		{
			Log.Warning("Trying to queue combat text, but CombatTextPanel hasn't set up text entries.", new object[0]);
		}
	}

	private void Awake()
	{
		this.m_textEntries = new List<CombatTextEntry>();
	}

	private void RemoveExpiredEntries()
	{
		if (this.m_textEntries != null)
		{
			for (int i = this.m_textEntries.Count - 1; i >= 0; i--)
			{
				if (this.m_textEntries[i].ShouldEnd())
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(UICombatTextPanel.RemoveExpiredEntries()).MethodHandle;
					}
					this.m_textEntries.RemoveAt(i);
				}
			}
		}
	}

	private void Update()
	{
		this.RemoveExpiredEntries();
		if (this.m_lastTextCreatedTime >= 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICombatTextPanel.Update()).MethodHandle;
			}
			if (Time.time - this.m_lastTextCreatedTime <= UICombatTextPanel.m_timeBetweenText)
			{
				return;
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		for (int i = 0; i < this.m_textEntries.Count; i++)
		{
			CombatTextEntry combatTextEntry = this.m_textEntries[i];
			if (combatTextEntry.IsWaitingToActivate())
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
				if (!(CameraManager.Get() == null))
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
					if (CameraManager.Get().InCinematic())
					{
						goto IL_B3;
					}
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				combatTextEntry.Activate();
				this.m_lastTextCreatedTime = Time.time;
				break;
			}
			IL_B3:;
		}
	}

	private void OnEnable()
	{
		this.Update();
	}
}
