using System;
using UnityEngine;

public class UIInfoScreen : MonoBehaviour
{
	private void Update()
	{
		if (GameFlowData.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIInfoScreen.Update()).MethodHandle;
			}
			if (GameManager.Get() != null)
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
				if (GameManager.Get().GameConfig.GameType != GameType.Tutorial)
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
					if (GameFlowData.Get().gameState != GameState.BothTeams_Decision)
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
						if (GameFlowData.Get().gameState != GameState.BothTeams_Resolve)
						{
							return;
						}
					}
					if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.ToggleInfo))
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
						UIGameOverScreen.SetupTeamMemberList(GameplayUtils.GenerateStatsFromGame(teamViewing, (int)GameFlowData.Get().LocalPlayerData.playerControllerId));
						UIGameStatsWindow.Get().ToggleStatsWindow();
					}
				}
			}
		}
	}
}
