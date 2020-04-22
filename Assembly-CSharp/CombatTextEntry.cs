using UnityEngine;

public class CombatTextEntry
{
	private float m_spawnTime;

	private string m_text;

	private CombatTextCategory m_category;

	private BuffIconToDisplay m_icon;

	private float m_textVisibleDuration;

	private ActorData m_actorData;

	public CombatTextEntry(ActorData actor, string text, CombatTextCategory category, BuffIconToDisplay icon, float visibleDuration)
	{
		m_actorData = actor;
		m_text = text;
		m_category = category;
		m_icon = icon;
		m_textVisibleDuration = visibleDuration;
		m_spawnTime = -1f;
	}

	internal bool IsWaitingToActivate()
	{
		return m_spawnTime <= 0f;
	}

	internal bool ShouldEnd()
	{
		bool result = false;
		if (!IsWaitingToActivate())
		{
			result = (Time.time - m_spawnTime > m_textVisibleDuration);
		}
		return result;
	}

	internal void Activate()
	{
		m_spawnTime = Time.time;
		HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.PlayCombatText(m_actorData, m_text, m_category, m_icon);
	}
}
