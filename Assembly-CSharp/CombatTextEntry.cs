using System;
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
		this.m_actorData = actor;
		this.m_text = text;
		this.m_category = category;
		this.m_icon = icon;
		this.m_textVisibleDuration = visibleDuration;
		this.m_spawnTime = -1f;
	}

	internal bool IsWaitingToActivate()
	{
		return this.m_spawnTime <= 0f;
	}

	internal bool ShouldEnd()
	{
		bool result = false;
		if (!this.IsWaitingToActivate())
		{
			result = (Time.time - this.m_spawnTime > this.m_textVisibleDuration);
		}
		return result;
	}

	internal void Activate()
	{
		this.m_spawnTime = Time.time;
		HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.PlayCombatText(this.m_actorData, this.m_text, this.m_category, this.m_icon);
	}
}
