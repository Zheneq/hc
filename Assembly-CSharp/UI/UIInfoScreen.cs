using UnityEngine;

public class UIInfoScreen : MonoBehaviour
{
    private void Update()
    {
        if (GameFlowData.Get() == null
            || GameManager.Get() == null
            || GameManager.Get().GameConfig.GameType == GameType.Tutorial)
        {
            return;
        }

        bool isDecisionOrResolve = GameFlowData.Get().gameState == GameState.BothTeams_Decision
                                   || GameFlowData.Get().gameState == GameState.BothTeams_Resolve;
        
        if (isDecisionOrResolve && InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.ToggleInfo))
        {
            Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
            UIGameOverScreen.SetupTeamMemberList(
                GameplayUtils.GenerateStatsFromGame(
                    teamViewing,
                    GameFlowData.Get().LocalPlayerData.playerControllerId));
            UIGameStatsWindow.Get().ToggleStatsWindow();
        }
    }
}