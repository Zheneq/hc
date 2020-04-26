using UnityEngine;

public class UIInfoScreen : MonoBehaviour
{
	private void Update()
	{
		if (!(GameFlowData.Get() != null))
		{
			return;
		}
		while (true)
		{
			if (!(GameManager.Get() != null))
			{
				return;
			}
			while (true)
			{
				if (GameManager.Get().GameConfig.GameType == GameType.Tutorial)
				{
					return;
				}
				while (true)
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
						while (true)
						{
							Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
							UIGameOverScreen.SetupTeamMemberList(GameplayUtils.GenerateStatsFromGame(teamViewing, GameFlowData.Get().LocalPlayerData.playerControllerId));
							UIGameStatsWindow.Get().ToggleStatsWindow();
							return;
						}
					}
					return;
				}
			}
		}
	}
}
