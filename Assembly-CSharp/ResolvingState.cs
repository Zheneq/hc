using UnityEngine.Networking;

public class ResolvingState : TurnState
{
	private float m_stateTime;

	public ResolvingState(ActorTurnSM masterSM)
		: base(masterSM)
	{
	}

	public override void OnEnter()
	{
		ActorData component = m_SM.GetComponent<ActorData>();
		if (HUD_UI.Get() != null)
		{
			if (component == GameFlowData.Get().activeOwnedActorData)
			{
				InterfaceManager.Get().CancelAlert(StringUtil.TR("PostRespawnMovement", "Global"));
				UINotificationPanel notificationPanel = HUD_UI.Get().m_mainScreenPanel.m_notificationPanel;
				notificationPanel.DisplayNotification(UINotificationPanel.GamePhaseDisplay.Resolving);
			}
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateNameplateUntargeted(component, true);
		}
		HighlightUtils.Get().SetCursorType(HighlightUtils.CursorType.NoCursorType);
		component.GetActorMovement().OnGameStateChange(false);
		m_stateTime = 0f;
		bool hasQueuedMovement = component.HasQueuedMovement();
		AbilityData abilityData = m_SM.GetComponent<AbilityData>();
		bool hasQueuedAbilities;
		if ((bool)abilityData)
		{
			hasQueuedAbilities = abilityData.HasQueuedAbilities();
			abilityData.ClearSelectedAbility();
			abilityData.ClearLastSelectedAbility();
		}
		else
		{
			hasQueuedAbilities = false;
		}
		if (!hasQueuedMovement && !hasQueuedAbilities)
		{
			m_SM.NextState = TurnStateEnum.WAITING;
		}
	}

	public override void OnMsg(TurnMessage msg, int extraData)
	{
		switch (msg)
		{
			case TurnMessage.TURN_START:
			{
				ActorData component = m_SM.GetComponent<ActorData>();
				Log.Error($"{(component != null ? component.DebugNameString() : "NULL actor")} " +
				          $"handling TURN_START message in Resolving state");
				m_SM.SetupForNewTurn();
				m_SM.NextState = TurnStateEnum.DECIDING;
				return;
			}
			case TurnMessage.MOVEMENT_RESOLVED:
			case TurnMessage.CLIENTS_RESOLVED_ABILITIES:
				m_SM.NextState = TurnStateEnum.WAITING;
				return;
			case TurnMessage.RESPAWN:
				m_SM.NextState = TurnStateEnum.RESPAWNING;
				return;
		}
	}

	public override void OnExit()
	{
		if (m_SM.GetComponent<AbilityData>() != null)
		{
			m_SM.GetComponent<AbilityData>().ClearSelectedAbility();
		}
	}

	public override void Update()
	{
		ActorData component = m_SM.GetComponent<ActorData>();
		if (component != null && component.IsDead())
		{
			m_SM.NextState = TurnStateEnum.WAITING;
		}
		if (m_stateTime >= GameFlowData.Get().m_resolveTimeoutLimit * 0.9f
		    && !GameplayUtils.IsMinion(m_SM))
		{
			string activeOwnedDisplayName = GameFlowData.Get().activeOwnedActorData != null
				? GameFlowData.Get().activeOwnedActorData.DisplayName
				: "NULL";
			Log.Error($"{(NetworkServer.active ? "Server" : "Client")} timed out " +
			          $"{m_SM.GetComponent<ActorData>().DisplayName} of resolving state " +
			          $"(owned actor = {activeOwnedDisplayName})");
			m_SM.NextState = TurnStateEnum.WAITING;
		}
		if (GameFlowData.Get() == null
		    || !GameFlowData.Get().IsResolutionPaused())
		{
			m_stateTime += GameTime.deltaTime;
		}
	}
}
