using System.Collections.Generic;
using UnityEngine;

public class TargeterTemplateFadeContainer
{
	private List<GameObject> m_highlightsToFade = new List<GameObject>();

	private float m_tsFadeStart = -1f;

	private const float c_maxDuration = 3f;

	public void TrackHighlights(List<GameObject> highlights)
	{
		CleanUpExistingHighlight();
		AbilityUtil_Targeter.SetTargeterHighlightColor(highlights, HighlightUtils.Get().m_targeterRemoveColor);
		for (int i = 0; i < highlights.Count; i++)
		{
			if (highlights[i] != null)
			{
				m_highlightsToFade.Add(highlights[i]);
			}
		}
		m_tsFadeStart = Time.time;
	}

	public void CleanUpExistingHighlight()
	{
		for (int i = 0; i < m_highlightsToFade.Count; i++)
		{
			GameObject gameObject = m_highlightsToFade[i];
			if (gameObject != null)
			{
				HighlightUtils.DestroyObjectAndMaterials(gameObject);
			}
		}
		while (true)
		{
			m_highlightsToFade.Clear();
			return;
		}
	}

	public void UpdateFade(ActorData targetingActor, bool hasActiveHighlights)
	{
		if (m_highlightsToFade.Count <= 0)
		{
			return;
		}
		while (true)
		{
			float num = Time.time - m_tsFadeStart;
			int num2;
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				if (targetingActor != null)
				{
					num2 = ((GameFlowData.Get().activeOwnedActorData.GetTeam() == targetingActor.GetTeam()) ? 1 : 0);
					goto IL_0088;
				}
			}
			num2 = 0;
			goto IL_0088;
			IL_0088:
			bool flag = (byte)num2 != 0;
			if (!(num >= 3f))
			{
				if (!hasActiveHighlights)
				{
					if (GameFlowData.Get().gameState == GameState.BothTeams_Decision)
					{
						if (flag)
						{
							float opacityFromTargeterData = AbilityUtil_Targeter.GetOpacityFromTargeterData(HighlightUtils.Get().m_targeterRemoveFadeOpacity, num);
							AbilityUtil_Targeter.SetTargeterHighlightOpacity(m_highlightsToFade, opacityFromTargeterData);
							return;
						}
					}
				}
			}
			CleanUpExistingHighlight();
			return;
		}
	}
}
