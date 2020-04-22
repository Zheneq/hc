public class ClientResolutionActionMessageData
{
	public ClientResolutionAction m_action;

	public int m_turnIndex;

	public AbilityPriority m_phase;

	public ClientResolutionActionMessageData(ClientResolutionAction action, int turnIndex, int phaseIndex)
	{
		m_action = action;
		m_turnIndex = turnIndex;
		m_phase = (AbilityPriority)phaseIndex;
	}
}
