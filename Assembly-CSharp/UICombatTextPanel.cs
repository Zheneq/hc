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
		CombatTextEntry item = new CombatTextEntry(actorData, combatText, category, icon, m_combatTextVisibleDuration);
		if (m_textEntries != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_textEntries.Add(item);
					return;
				}
			}
		}
		Log.Warning("Trying to queue combat text, but CombatTextPanel hasn't set up text entries.");
	}

	private void Awake()
	{
		m_textEntries = new List<CombatTextEntry>();
	}

	private void RemoveExpiredEntries()
	{
		if (m_textEntries == null)
		{
			return;
		}
		for (int num = m_textEntries.Count - 1; num >= 0; num--)
		{
			if (m_textEntries[num].ShouldEnd())
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
				m_textEntries.RemoveAt(num);
			}
		}
	}

	private void Update()
	{
		RemoveExpiredEntries();
		if (!(m_lastTextCreatedTime < 0f))
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
			if (!(Time.time - m_lastTextCreatedTime > m_timeBetweenText))
			{
				return;
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
		int num = 0;
		CombatTextEntry combatTextEntry;
		while (true)
		{
			if (num >= m_textEntries.Count)
			{
				return;
			}
			combatTextEntry = m_textEntries[num];
			if (combatTextEntry.IsWaitingToActivate())
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
				if (CameraManager.Get() == null)
				{
					break;
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
				if (!CameraManager.Get().InCinematic())
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
					break;
				}
			}
			num++;
		}
		combatTextEntry.Activate();
		m_lastTextCreatedTime = Time.time;
	}

	private void OnEnable()
	{
		Update();
	}
}
