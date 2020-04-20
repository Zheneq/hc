using System;
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
		float alpha = this.m_closeButtonHover.alpha;
		if (this.m_redEnemyButtonHover.alpha > alpha)
		{
			alpha = this.m_redEnemyButtonHover.alpha;
			result = ActorController.PingType.Enemy;
		}
		if (this.m_blueMoveButtonHover.alpha > alpha)
		{
			alpha = this.m_blueMoveButtonHover.alpha;
			result = ActorController.PingType.Move;
		}
		if (this.m_yellowDefendButtonHover.alpha > alpha)
		{
			alpha = this.m_yellowDefendButtonHover.alpha;
			result = ActorController.PingType.Defend;
		}
		if (this.m_greenAssistButtonHover.alpha > alpha)
		{
			alpha = this.m_greenAssistButtonHover.alpha;
			result = ActorController.PingType.Assist;
		}
		return result;
	}
}
