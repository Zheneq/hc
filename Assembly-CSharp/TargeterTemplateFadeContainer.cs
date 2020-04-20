using System;
using System.Collections.Generic;
using UnityEngine;

public class TargeterTemplateFadeContainer
{
	private List<GameObject> m_highlightsToFade = new List<GameObject>();

	private float m_tsFadeStart = -1f;

	private const float c_maxDuration = 3f;

	public void TrackHighlights(List<GameObject> highlights)
	{
		this.CleanUpExistingHighlight();
		AbilityUtil_Targeter.SetTargeterHighlightColor(highlights, HighlightUtils.Get().m_targeterRemoveColor, true, true);
		for (int i = 0; i < highlights.Count; i++)
		{
			if (highlights[i] != null)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(TargeterTemplateFadeContainer.TrackHighlights(List<GameObject>)).MethodHandle;
				}
				this.m_highlightsToFade.Add(highlights[i]);
			}
		}
		this.m_tsFadeStart = Time.time;
	}

	public void CleanUpExistingHighlight()
	{
		for (int i = 0; i < this.m_highlightsToFade.Count; i++)
		{
			GameObject gameObject = this.m_highlightsToFade[i];
			if (gameObject != null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(TargeterTemplateFadeContainer.CleanUpExistingHighlight()).MethodHandle;
				}
				HighlightUtils.DestroyObjectAndMaterials(gameObject);
			}
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
		this.m_highlightsToFade.Clear();
	}

	public void UpdateFade(ActorData targetingActor, bool hasActiveHighlights)
	{
		if (this.m_highlightsToFade.Count > 0)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargeterTemplateFadeContainer.UpdateFade(ActorData, bool)).MethodHandle;
			}
			float num = Time.time - this.m_tsFadeStart;
			bool flag;
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				for (;;)
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
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					flag = (GameFlowData.Get().activeOwnedActorData.GetTeam() == targetingActor.GetTeam());
					goto IL_88;
				}
			}
			flag = false;
			IL_88:
			bool flag2 = flag;
			if (num < 3f)
			{
				for (;;)
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
					for (;;)
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
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (flag2)
						{
							float opacityFromTargeterData = AbilityUtil_Targeter.GetOpacityFromTargeterData(HighlightUtils.Get().m_targeterRemoveFadeOpacity, num);
							AbilityUtil_Targeter.SetTargeterHighlightOpacity(this.m_highlightsToFade, opacityFromTargeterData);
							return;
						}
						for (;;)
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
			this.CleanUpExistingHighlight();
		}
	}
}
