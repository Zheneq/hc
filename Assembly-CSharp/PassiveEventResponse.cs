using System;

[Serializable]
public class PassiveEventResponse
{
	public int m_healthBonus;

	public int m_techPointsBonus;

	public int m_personalCreditsBonus;

	public int m_mechanicPointAdjust;

	public PassiveActionType[] m_actions;

	public StandardEffectInfo m_effect;
}
