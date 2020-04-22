using UnityEngine;

public class BigPingPanel : MonoBehaviour
{
	public CanvasGroup m_redEnemyButtonHover;

	public CanvasGroup m_blueMoveButtonHover;

	public CanvasGroup m_yellowDefendButtonHover;

	public CanvasGroup m_greenAssistButtonHover;

	public CanvasGroup m_closeButtonHover;

	public GameObject m_closeButton;

	public GameObject m_line;

	public ActorController.PingType GetPingType()
	{
		ActorController.PingType result = ActorController.PingType.Default;
		float alpha = m_closeButtonHover.alpha;
		if (m_redEnemyButtonHover.alpha > alpha)
		{
			alpha = m_redEnemyButtonHover.alpha;
			result = ActorController.PingType.Enemy;
		}
		if (m_blueMoveButtonHover.alpha > alpha)
		{
			alpha = m_blueMoveButtonHover.alpha;
			result = ActorController.PingType.Move;
		}
		if (m_yellowDefendButtonHover.alpha > alpha)
		{
			alpha = m_yellowDefendButtonHover.alpha;
			result = ActorController.PingType.Defend;
		}
		if (m_greenAssistButtonHover.alpha > alpha)
		{
			alpha = m_greenAssistButtonHover.alpha;
			result = ActorController.PingType.Assist;
		}
		return result;
	}
}
