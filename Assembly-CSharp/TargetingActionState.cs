using UnityEngine;
using UnityEngine.Networking;

public class TargetingActionState : TurnState
{
	public int TargetIndex
	{
		get
		{
			return m_SM.GetAbilityTargets().Count;
		}
		private set
		{
		}
	}

	public TargetingActionState(ActorTurnSM masterSM)
		: base(masterSM)
	{
	}

	public override void OnEnter()
	{
		SetupTargeting();
	}

	private void SetupTargeting()
	{
		m_SM.ClearAbilityTargets();
		if (!(GameFlowData.Get().activeOwnedActorData != null))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(GameFlowData.Get().activeOwnedActorData.GetComponent<ActorTurnSM>() == m_SM))
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
				ActorData component = m_SM.GetComponent<ActorData>();
				AbilityData component2 = m_SM.GetComponent<AbilityData>();
				BoardSquare autoSelectTarget = component2.GetAutoSelectTarget();
				if (!(autoSelectTarget != null))
				{
					return;
				}
				AbilityTarget newTarget = AbilityTarget.CreateAbilityTargetFromBoardSquare(autoSelectTarget, component.GetTravelBoardSquareWorldPosition());
				m_SM.AddAbilityTarget(newTarget);
				AbilityData.ActionType selectedActionType = component2.GetSelectedActionType();
				m_SM.OnQueueAbilityRequest(selectedActionType);
				if (NetworkServer.active)
				{
					while (true)
					{
						switch (4)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
				m_SM.SendCastAbility(selectedActionType);
				m_SM.NextState = TurnStateEnum.VALIDATING_ACTION_REQUEST;
				return;
			}
		}
	}

	private bool SelectTarget(AbilityTarget abilityTargetToUse)
	{
		bool flag = m_SM.SelectTarget(abilityTargetToUse);
		if (flag)
		{
			m_SM.NextState = TurnStateEnum.VALIDATING_ACTION_REQUEST;
		}
		return flag;
	}

	public override void OnSelectedAbilityChanged()
	{
		SetupTargeting();
	}

	public override void OnMsg(TurnMessage msg, int extraData)
	{
		AbilityData component = m_SM.GetComponent<AbilityData>();
		switch (msg)
		{
		case TurnMessage.SELECTED_ABILITY:
		case TurnMessage.MOVEMENT_ACCEPTED:
		case TurnMessage.MOVEMENT_REJECTED:
		case TurnMessage.RESPAWN:
		case TurnMessage.PICK_RESPAWN:
		case TurnMessage.PICKED_RESPAWN:
		case TurnMessage.CANCEL_SINGLE_ABILITY:
		case TurnMessage.CANCEL_MOVEMENT:
			break;
		case TurnMessage.ABILITY_REQUEST_ACCEPTED:
			m_SM.NextState = TurnStateEnum.DECIDING;
			break;
		case TurnMessage.BEGIN_RESOLVE:
			m_SM.NextState = TurnStateEnum.WAITING;
			break;
		case TurnMessage.CANCEL_BUTTON_CLICKED:
			component.ClearSelectedAbility();
			component.ClearActionsToCancelOnTargetingComplete();
			if (NetworkClient.active)
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
				UISounds.GetUISounds().Play("ui/ingame/v1/action_undo");
			}
			m_SM.NextState = TurnStateEnum.DECIDING;
			break;
		case TurnMessage.DISCONNECTED:
			m_SM.NextState = TurnStateEnum.CONFIRMED;
			break;
		case TurnMessage.ABILITY_REQUEST_REJECTED:
			component.ClearSelectedAbility();
			m_SM.NextState = TurnStateEnum.DECIDING;
			break;
		case TurnMessage.MOVE_BUTTON_CLICKED:
			m_SM.NextState = TurnStateEnum.DECIDING_MOVEMENT;
			break;
		case TurnMessage.MOVEMENT_RESOLVED:
			Log.Error(m_SM.GetComponent<ActorData>().DisplayName + "Received a 'Movement Resolved' message in the TargetingAction state, which is unexpected.");
			m_SM.NextState = TurnStateEnum.WAITING;
			break;
		case TurnMessage.CLIENTS_RESOLVED_ABILITIES:
			Log.Warning(m_SM.GetComponent<ActorData>().DisplayName + "Received a 'CLIENTS_RESOLVED_ABILITIES' message in the TargetingAction state, which is unexpected.");
			m_SM.NextState = TurnStateEnum.WAITING;
			break;
		case TurnMessage.DONE_BUTTON_CLICKED:
			m_SM.NextState = TurnStateEnum.CONFIRMED;
			if (!SinglePlayerManager.Get())
			{
				break;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				SinglePlayerManager.Get().OnActorLockInEntered(m_SM.GetComponent<ActorData>());
				return;
			}
		}
	}

	public override void Update()
	{
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (GameFlowData.Get().activeOwnedActorData.GetComponent<ActorTurnSM>() == m_SM)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				AbilityData component = m_SM.GetComponent<AbilityData>();
				ActorData component2 = m_SM.GetComponent<ActorData>();
				if ((bool)component)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if ((bool)component2)
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
						Ability selectedAbility = component.GetSelectedAbility();
						if (selectedAbility != null)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							Ability.TargetingParadigm targetingParadigm = selectedAbility.GetTargetingParadigm(TargetIndex);
							int num;
							if (targetingParadigm != Ability.TargetingParadigm.Direction)
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
								num = ((targetingParadigm == Ability.TargetingParadigm.Position) ? 1 : 0);
							}
							else
							{
								num = 1;
							}
							bool flag = (byte)num != 0;
							if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.CycleTarget) && flag)
							{
								component.NextSoftTarget();
							}
						}
						if (!Input.GetMouseButtonUp(0))
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!Input.GetMouseButtonUp(1))
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
								if (!InputManager.Get().GetAcceptButtonDown())
								{
									goto IL_026d;
								}
								while (true)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
							}
						}
						if (InterfaceManager.Get().ShouldHandleMouseClick() && !m_SM.HandledMouseInput)
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
							m_SM.HandledMouseInput = true;
							AbilityTarget abilityTargetForTargeterUpdate = AbilityTarget.GetAbilityTargetForTargeterUpdate();
							if (selectedAbility != null)
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
								if (component.ValidateAbilityOnTarget(selectedAbility, abilityTargetForTargeterUpdate, TargetIndex))
								{
									while (true)
									{
										switch (5)
										{
										case 0:
											continue;
										}
										break;
									}
									if (Input.GetMouseButtonUp(1))
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
										if (!Options_UI.Get().GetRightClickingConfirmsAbilityTargets())
										{
											m_SM.RequestCancel();
											goto IL_026d;
										}
									}
									bool flag2 = SelectTarget(abilityTargetForTargeterUpdate);
									int num2;
									if (selectedAbility.GetRunPriority() == AbilityPriority.Evasion)
									{
										while (true)
										{
											switch (2)
											{
											case 0:
												continue;
											}
											break;
										}
										num2 = (selectedAbility.CanOverrideMoveStartSquare() ? 1 : 0);
									}
									else
									{
										num2 = 0;
									}
									bool flag3 = (byte)num2 != 0;
									if (flag2)
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
										if (!flag3)
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
											UISounds.GetUISounds().Play("ui/ingame/v1/action");
											goto IL_026d;
										}
									}
									UISounds.GetUISounds().Play("ui/ingame/v1/action_target_selected");
								}
							}
						}
					}
				}
			}
		}
		goto IL_026d;
		IL_026d:
		m_SM.UpdateEndTurnKey();
	}
}
