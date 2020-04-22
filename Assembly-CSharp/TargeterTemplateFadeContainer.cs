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
				while (true)
				{
					switch (4)
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
				HighlightUtils.DestroyObjectAndMaterials(gameObject);
			}
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
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
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			float num = Time.time - m_tsFadeStart;
			int num2;
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (targetingActor != null)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
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
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!hasActiveHighlights)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (GameFlowData.Get().gameState == GameState.BothTeams_Decision)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (flag)
						{
							float opacityFromTargeterData = AbilityUtil_Targeter.GetOpacityFromTargeterData(HighlightUtils.Get().m_targeterRemoveFadeOpacity, num);
							AbilityUtil_Targeter.SetTargeterHighlightOpacity(m_highlightsToFade, opacityFromTargeterData);
							return;
						}
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
					}
				}
			}
			CleanUpExistingHighlight();
			return;
		}
	}
}
