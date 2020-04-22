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
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(GameManager.Get() != null))
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (GameManager.Get().GameConfig.GameType == GameType.Tutorial)
				{
					return;
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					if (GameFlowData.Get().gameState != GameState.BothTeams_Decision)
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
						if (GameFlowData.Get().gameState != GameState.BothTeams_Resolve)
						{
							return;
						}
					}
					if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.ToggleInfo))
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
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
