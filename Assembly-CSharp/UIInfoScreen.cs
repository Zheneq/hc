using System;
using UnityEngine;

public class UIInfoScreen : MonoBehaviour
{
	private void Update()
	{
		if (GameFlowData.Get() != null)
		{
			if (GameManager.Get() != null)
			{
				if (GameManager.Get().GameConfig.GameType != GameType.Tutorial)
				{
					if (GameFlowData.Get().gameState != GameState.BothTeams_Decision)
					{
						if (GameFlowData.Get().gameState != GameState.BothTeams_Resolve)
						{
							return;
						}
					}
					if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.ToggleInfo))
					{
						Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
						UIGameOverScreen.SetupTeamMemberList(GameplayUtils.GenerateStatsFromGame(teamViewing, (int)GameFlowData.Get().LocalPlayerData.playerControllerId));
						UIGameStatsWindow.Get().ToggleStatsWindow();
					}
				}
			}
		}
	}
}
