﻿using System;
using UnityEngine;
using UnityEngine.Networking;

public class TargetingActionState : TurnState
{
	public TargetingActionState(ActorTurnSM masterSM) : base(masterSM)
	{
	}

	public int TargetIndex
	{
		get
		{
			return this.m_SM.GetAbilityTargets().Count;
		}
		private set
		{
		}
	}

	public override void OnEnter()
	{
		this.SetupTargeting();
	}

	private void SetupTargeting()
	{
		this.m_SM.ClearAbilityTargets();
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetingActionState.SetupTargeting()).MethodHandle;
			}
			if (GameFlowData.Get().activeOwnedActorData.GetComponent<ActorTurnSM>() == this.m_SM)
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
				ActorData component = this.m_SM.GetComponent<ActorData>();
				AbilityData component2 = this.m_SM.GetComponent<AbilityData>();
				BoardSquare autoSelectTarget = component2.GetAutoSelectTarget();
				if (autoSelectTarget != null)
				{
					AbilityTarget newTarget = AbilityTarget.CreateAbilityTargetFromBoardSquare(autoSelectTarget, component.GetTravelBoardSquareWorldPosition());
					this.m_SM.AddAbilityTarget(newTarget);
					AbilityData.ActionType selectedActionType = component2.GetSelectedActionType();
					this.m_SM.OnQueueAbilityRequest(selectedActionType);
					if (NetworkServer.active)
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
					}
					else
					{
						this.m_SM.SendCastAbility(selectedActionType);
						this.m_SM.NextState = TurnStateEnum.VALIDATING_ACTION_REQUEST;
					}
				}
			}
		}
	}

	private bool SelectTarget(AbilityTarget abilityTargetToUse)
	{
		bool flag = this.m_SM.SelectTarget(abilityTargetToUse, false);
		if (flag)
		{
			this.m_SM.NextState = TurnStateEnum.VALIDATING_ACTION_REQUEST;
		}
		return flag;
	}

	public override void OnSelectedAbilityChanged()
	{
		this.SetupTargeting();
	}

	public override void OnMsg(TurnMessage msg, int extraData)
	{
		AbilityData component = this.m_SM.GetComponent<AbilityData>();
		switch (msg)
		{
		case TurnMessage.BEGIN_RESOLVE:
			this.m_SM.NextState = TurnStateEnum.WAITING;
			break;
		case TurnMessage.MOVEMENT_RESOLVED:
			Log.Error(this.m_SM.GetComponent<ActorData>().DisplayName + "Received a 'Movement Resolved' message in the TargetingAction state, which is unexpected.", new object[0]);
			this.m_SM.NextState = TurnStateEnum.WAITING;
			break;
		case TurnMessage.CLIENTS_RESOLVED_ABILITIES:
			Log.Warning(this.m_SM.GetComponent<ActorData>().DisplayName + "Received a 'CLIENTS_RESOLVED_ABILITIES' message in the TargetingAction state, which is unexpected.", new object[0]);
			this.m_SM.NextState = TurnStateEnum.WAITING;
			break;
		case TurnMessage.CANCEL_BUTTON_CLICKED:
			component.ClearSelectedAbility();
			component.ClearActionsToCancelOnTargetingComplete();
			if (NetworkClient.active)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TargetingActionState.OnMsg(TurnMessage, int)).MethodHandle;
				}
				UISounds.GetUISounds().Play("ui/ingame/v1/action_undo");
			}
			this.m_SM.NextState = TurnStateEnum.DECIDING;
			break;
		case TurnMessage.ABILITY_REQUEST_ACCEPTED:
			this.m_SM.NextState = TurnStateEnum.DECIDING;
			break;
		case TurnMessage.ABILITY_REQUEST_REJECTED:
			component.ClearSelectedAbility();
			this.m_SM.NextState = TurnStateEnum.DECIDING;
			break;
		case TurnMessage.MOVE_BUTTON_CLICKED:
			this.m_SM.NextState = TurnStateEnum.DECIDING_MOVEMENT;
			break;
		case TurnMessage.DONE_BUTTON_CLICKED:
			this.m_SM.NextState = TurnStateEnum.CONFIRMED;
			if (SinglePlayerManager.Get())
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				SinglePlayerManager.Get().OnActorLockInEntered(this.m_SM.GetComponent<ActorData>());
			}
			break;
		case TurnMessage.DISCONNECTED:
			this.m_SM.NextState = TurnStateEnum.CONFIRMED;
			break;
		}
	}

	public override void Update()
	{
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetingActionState.Update()).MethodHandle;
			}
			if (GameFlowData.Get().activeOwnedActorData.GetComponent<ActorTurnSM>() == this.m_SM)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				AbilityData component = this.m_SM.GetComponent<AbilityData>();
				ActorData component2 = this.m_SM.GetComponent<ActorData>();
				if (component)
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
					if (component2)
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
						Ability selectedAbility = component.GetSelectedAbility();
						if (selectedAbility != null)
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
							Ability.TargetingParadigm targetingParadigm = selectedAbility.GetTargetingParadigm(this.TargetIndex);
							bool flag;
							if (targetingParadigm != Ability.TargetingParadigm.Direction)
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
								flag = (targetingParadigm == Ability.TargetingParadigm.Position);
							}
							else
							{
								flag = true;
							}
							bool flag2 = flag;
							if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.CycleTarget) && flag2)
							{
								component.NextSoftTarget();
							}
						}
						if (!Input.GetMouseButtonUp(0))
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
							if (!Input.GetMouseButtonUp(1))
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
								if (!InputManager.Get().GetAcceptButtonDown())
								{
									goto IL_26D;
								}
								for (;;)
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
						if (InterfaceManager.Get().ShouldHandleMouseClick() && !this.m_SM.HandledMouseInput)
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
							this.m_SM.HandledMouseInput = true;
							AbilityTarget abilityTargetForTargeterUpdate = AbilityTarget.GetAbilityTargetForTargeterUpdate();
							if (selectedAbility != null)
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
								if (component.ValidateAbilityOnTarget(selectedAbility, abilityTargetForTargeterUpdate, this.TargetIndex, -1f, -1f))
								{
									for (;;)
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
										for (;;)
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
											this.m_SM.RequestCancel(false);
											goto IL_26D;
										}
									}
									bool flag3 = this.SelectTarget(abilityTargetForTargeterUpdate);
									bool flag4;
									if (selectedAbility.GetRunPriority() == AbilityPriority.Evasion)
									{
										for (;;)
										{
											switch (2)
											{
											case 0:
												continue;
											}
											break;
										}
										flag4 = selectedAbility.CanOverrideMoveStartSquare();
									}
									else
									{
										flag4 = false;
									}
									bool flag5 = flag4;
									if (flag3)
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
										if (!flag5)
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
											UISounds.GetUISounds().Play("ui/ingame/v1/action");
											goto IL_26D;
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
		IL_26D:
		this.m_SM.UpdateEndTurnKey();
	}
}
