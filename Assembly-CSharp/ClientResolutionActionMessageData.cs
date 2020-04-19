using System;

public class ClientResolutionActionMessageData
{
	public ClientResolutionAction m_action;

	public int m_turnIndex;

	public AbilityPriority m_phase;

	public ClientResolutionActionMessageData(ClientResolutionAction action, int turnIndex, int phaseIndex)
	{
		this.m_action = action;
		this.m_turnIndex = turnIndex;
		this.m_phase = (AbilityPriority)phaseIndex;
	}
}
