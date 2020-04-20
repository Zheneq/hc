using System;
using UnityEngine;

namespace CameraManagerInternal
{
	public class IsometricCamera : MonoBehaviour, IGameEventListener
	{
		public float m_movementCatchUpMult = 1.75f;

		public float m_startHorzDist = 12f;

		public float m_maxHorzDist = 15f;

		public float m_maxVertDist = 15f;

		public float m_defaultRotationY = 50f;

		public float m_easeInTime = 1f;

		public float m_heightAdjustMaxSpeed = 8f;

		public float m_movementHorzDist = 10f;

		public AnimationCurve m_vertCurve = new AnimationCurve();

		public AnimationCurve m_tiltCurve = new AnimationCurve();

		public AnimationCurve m_horzCurve = new AnimationCurve();

		public AnimationCurve m_fovCurve = new AnimationCurve();

		public float m_minZoomParamForCoverZoomOffset = 0.6f;

		public float m_coverHeightEaseTime = 1.4f;

		public float m_zoomEaseTime = 0.5f;

		public float m_transitionInTime = 0.3f;

		public float m_inputMoveEaseTime = 0.3f;

		public float m_NO_EDIT_zoomInTilt;

		public float m_NO_EDIT_zoomOutTilt;

		public float m_NO_EDIT_zoomLevel;

		private Vector3 m_defaultEulerRotation = new Vector3(50f, 50f, 0f);

		private float m_maxDistFarHorz;

		private float m_maxDistFarVert;

		private float m_transitionInTimeLeft;

		private Vector3 m_transitionInPosition;

		private Quaternion m_transitionInRotation;

		private float m_transitionFOV;

		private GameObject m_targetObject;

		private CameraManager.CameraTargetReason m_targetReason;

		private ActorData m_targetObjectActor;

		private Vector3 m_targetObjectOffset;

		private bool m_cutToTarget = true;

		private bool m_respawnedBeforeUpdateEnd;

		private float m_zoomGoalValue;

		private bool m_needNameplateSortUpdate;

		private float m_nextNameplateSortUpdateTime = -1f;

		private const float c_minNameplateSortInterval = 0.6f;

		private EasedOutFloat m_zoomParameter = new EasedOutFloat(1f);

		private EasedFloat m_zoomParameterScale = new EasedFloat(1f);

		private EasedOutFloat m_zoomVertOffsetForAnimatedActor = new EasedOutFloat(1f);

		private EasedOutVector3 m_targetPosition = new EasedOutVector3(Vector3.zero);

		private bool m_targetWithBoardSquareWasNeverSet = true;

		internal Vector3 TargetPosition
		{
			get
			{
				return this.m_targetPosition;
			}
		}

		private void Awake()
		{
			if (this.m_tiltCurve != null)
			{
				if (this.m_tiltCurve.keys.Length >= 1)
				{
					return;
				}
			}
			Log.Warning("Please remove misconfigured IsometricCamera component from " + base.gameObject, new object[0]);
			base.enabled = false;
		}

		private void Start()
		{
			if (GameManager.IsEditorAndNotGame())
			{
				base.enabled = false;
				return;
			}
			this.UpdateNoEdit();
			this.m_maxDistFarHorz = 100f * this.m_maxHorzDist;
			this.m_maxDistFarVert = 100f * this.m_maxVertDist;
			this.m_zoomParameter = new EasedOutFloat(this.CalcZoomParameter((double)this.m_startHorzDist));
			this.m_zoomGoalValue = this.m_zoomParameter.GetEndValue();
			this.m_defaultEulerRotation.x = this.CalcZoomRotationX();
			this.m_defaultEulerRotation.y = this.m_defaultRotationY;
			if (CameraControls.Get())
			{
				CameraControls.Get().m_desiredRotationEulerAngles = this.m_defaultEulerRotation;
			}
			if (GameFlowData.Get() != null)
			{
				ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
				if (activeOwnedActorData != null)
				{
					CameraManager.Get().OnActiveOwnedActorChange(activeOwnedActorData);
					this.ResetCameraRotation();
				}
			}
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.CharacterRespawn);
		}

		private void OnDestroy()
		{
			if (GameEventManager.Get() != null)
			{
				GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.CharacterRespawn);
			}
		}

		public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
		{
			if (eventType != GameEventManager.EventType.CharacterRespawn)
			{
			}
			else
			{
				GameEventManager.CharacterRespawnEventArgs characterRespawnEventArgs = (GameEventManager.CharacterRespawnEventArgs)args;
				if (GameFlowData.Get().activeOwnedActorData == characterRespawnEventArgs.respawningCharacter)
				{
					this.m_respawnedBeforeUpdateEnd = true;
				}
			}
		}

		public void SetTargetObject(GameObject targetObject, CameraManager.CameraTargetReason reason)
		{
			this.SetTargetObject(targetObject, reason, false);
		}

		public void SetTargetObject(GameObject targetObject, CameraManager.CameraTargetReason reason, bool putUnderMouse)
		{
			if (CameraManager.CamDebugTraceOn)
			{
				if (targetObject != this.m_targetObject)
				{
					string str = "SetTargetObject to ";
					string str2;
					if (targetObject)
					{
						str2 = targetObject.name;
					}
					else
					{
						str2 = "NULL";
					}
					CameraManager.LogForDebugging(str + str2, CameraManager.CameraLogType.Isometric);
				}
			}
			ActorData actorData;
			if (targetObject == null)
			{
				actorData = null;
			}
			else
			{
				actorData = targetObject.GetComponent<ActorData>();
			}
			ActorData actorData2 = actorData;
			if (actorData2 != null)
			{
				if (this.m_targetWithBoardSquareWasNeverSet && actorData2.GetCurrentBoardSquare() != null)
				{
					this.ResetCameraRotation();
					this.m_cutToTarget = true;
					this.m_targetWithBoardSquareWasNeverSet = false;
				}
				this.m_zoomVertOffsetForAnimatedActor = new EasedOutFloat(this.GetZoomVertOffsetForActiveAnimatedActor(this.m_zoomParameter * this.m_zoomParameterScale > this.m_minZoomParamForCoverZoomOffset));
			}
			this.m_targetObject = targetObject;
			this.m_targetObjectActor = actorData2;
			this.m_targetReason = reason;
			if (putUnderMouse)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				EasedOutVector3 targetPosition = this.m_targetPosition;
				Plane plane = new Plane(Vector3.up, targetPosition);
				float distance;
				if (plane.Raycast(ray, out distance))
				{
					this.m_targetObjectOffset = targetPosition - ray.GetPoint(distance);
				}
			}
			else
			{
				this.m_targetObjectOffset = Vector3.zero;
			}
			if (this.m_targetObject != null)
			{
				if (CameraManager.Get().ShouldAutoCameraMove())
				{
					this.m_targetPosition.EaseTo(this.m_targetObject.transform.position + this.m_targetObjectOffset, this.m_easeInTime);
				}
			}
		}

		public void SetTargetPosition(Vector3 pos, float easeInTime)
		{
			this.m_targetPosition.EaseTo(pos, easeInTime);
		}

		public float GetInitialYAngle()
		{
			float result = this.m_defaultRotationY;
			Vector3 position;
			if (GameFlowData.Get().activeOwnedActorData == null)
			{
				position = base.transform.position;
			}
			else
			{
				position = GameFlowData.Get().activeOwnedActorData.transform.position;
			}
			Vector3 b = position;
			Vector3 a = CameraManager.Get().CameraPositionBounds.center;
			bool flag = false;
			if (SinglePlayerManager.Get() != null)
			{
				if (SinglePlayerCoordinator.Get().m_initialCameraRotationTarget != null)
				{
					a = SinglePlayerCoordinator.Get().m_initialCameraRotationTarget.transform.position;
					flag = true;
				}
			}
			Vector3 vector = a - b;
			vector.y = 0f;
			float magnitude = vector.magnitude;
			if (!Mathf.Approximately(magnitude, 0f))
			{
				vector /= magnitude;
				Quaternion quaternion = Quaternion.LookRotation(vector);
				if (flag)
				{
					result = (float)((int)quaternion.eulerAngles.y / 0x2D * 0x2D);
				}
				else
				{
					int num = (int)(quaternion.eulerAngles.y - this.m_defaultRotationY);
					int num2 = num / 0x5A * 0x5A;
					if (num - num2 > num2 + 0x5A - num)
					{
						num2 += 0x5A;
					}
					result = (float)num2;
				}
			}
			return result;
		}

		public void ResetCameraRotation()
		{
			CameraControls.Get().m_desiredRotationEulerAngles.y = this.m_defaultRotationY;
			Vector3 position;
			if (GameFlowData.Get().activeOwnedActorData == null)
			{
				position = base.transform.position;
			}
			else
			{
				position = GameFlowData.Get().activeOwnedActorData.transform.position;
			}
			Vector3 b = position;
			Vector3 a = CameraManager.Get().CameraPositionBounds.center;
			bool flag = false;
			if (SinglePlayerManager.Get() != null)
			{
				if (SinglePlayerCoordinator.Get().m_initialCameraRotationTarget != null)
				{
					a = SinglePlayerCoordinator.Get().m_initialCameraRotationTarget.transform.position;
					flag = true;
				}
			}
			Vector3 vector = a - b;
			vector.y = 0f;
			float magnitude = vector.magnitude;
			if (!Mathf.Approximately(magnitude, 0f))
			{
				vector /= magnitude;
				Quaternion quaternion = Quaternion.LookRotation(vector);
				if (flag)
				{
					CameraControls.Get().m_desiredRotationEulerAngles.y = (float)((int)quaternion.eulerAngles.y / 0x2D * 0x2D);
				}
				else
				{
					int num = (int)(quaternion.eulerAngles.y - this.m_defaultRotationY);
					int num2 = num / 0x5A * 0x5A;
					if (num - num2 > num2 + 0x5A - num)
					{
						num2 += 0x5A;
					}
					CameraControls.Get().m_desiredRotationEulerAngles.y = (float)num2;
				}
			}
		}

		internal bool AllowCameraShake()
		{
			return this.m_targetPosition.EaseFinished();
		}

		private void UpdateNoEdit()
		{
			this.m_NO_EDIT_zoomInTilt = Mathf.Atan(this.m_tiltCurve.keys[0].value) * 57.29578f;
			this.m_NO_EDIT_zoomOutTilt = Mathf.Atan(this.m_tiltCurve.keys[this.m_tiltCurve.keys.Length - 1].value) * 57.29578f;
			this.m_NO_EDIT_zoomLevel = this.m_zoomParameter * this.m_zoomParameterScale;
		}

		private bool IsInMovementPhase()
		{
			ActionBufferPhase currentActionPhase = ServerClientUtils.GetCurrentActionPhase();
			if (currentActionPhase != ActionBufferPhase.Movement && currentActionPhase != ActionBufferPhase.MovementChase)
			{
				if (currentActionPhase != ActionBufferPhase.AbilitiesWait)
				{
					return currentActionPhase == ActionBufferPhase.MovementWait;
				}
			}
			return true;
		}

		private void Update()
		{
			Vector3 position = base.transform.position;
			Quaternion quaternion = base.transform.rotation;
			Vector3 zero = Vector3.zero;
			float num = 0f;
			if (!(CameraControls.Get() == null))
			{
				if (!(GameFlowData.Get() == null))
				{
					bool flag = false;
					flag |= !this.m_targetWithBoardSquareWasNeverSet;
					bool flag2 = flag;
					bool flag3;
					if (GameFlowData.Get().LocalPlayerData != null)
					{
						flag3 = (GameFlowData.Get().activeOwnedActorData == null);
					}
					else
					{
						flag3 = false;
					}
					flag = (flag2 || flag3);
					if (flag)
					{
						CameraControls.Get().CalcDesiredTransform(base.transform, out zero, out quaternion, out num);
					}
					if (!Mathf.Approximately(num, 0f))
					{
						float num2 = this.CalcZoomParameterDelta(num);
						float num3 = num2 + this.m_zoomParameter.GetEndValue() * this.m_zoomParameterScale;
						num3 = Mathf.Clamp01(num3);
						this.m_zoomParameter.EaseTo(num3, this.m_zoomEaseTime);
						this.m_zoomGoalValue = num3;
						this.m_zoomParameterScale = new EasedFloat(1f);
					}
					else if (!Mathf.Approximately(this.m_zoomGoalValue, this.m_zoomParameter.GetEndValue()))
					{
						this.m_zoomParameter.EaseTo(this.m_zoomGoalValue, this.m_zoomEaseTime);
					}
					if (zero.sqrMagnitude > 1.401298E-45f)
					{
						CameraManager.Get().SecondsRemainingToPauseForUserControl = 0.5f;
						CameraManager.Get().OnPlayerMovedCamera();
						Vector3 endValue = this.m_targetPosition.GetEndValue() + zero;
						GameplayData gameplayData = GameplayData.Get();
						endValue.x = Mathf.Clamp(endValue.x, gameplayData.m_minimumPositionX, gameplayData.m_maximumPositionX);
						endValue.z = Mathf.Clamp(endValue.z, gameplayData.m_minimumPositionZ, gameplayData.m_maximumPositionZ);
						this.m_targetPosition.EaseTo(endValue, this.m_inputMoveEaseTime);
						this.SetTargetObject(null, CameraManager.CameraTargetReason.ReachedTargetObj);
					}
					else if (this.m_targetObject != null)
					{
						Vector3 vector = this.m_targetObject.transform.position + this.m_targetObjectOffset;
						if (this.m_cutToTarget)
						{
							this.m_targetPosition = new EasedOutVector3(vector);
							this.m_zoomVertOffsetForAnimatedActor = new EasedOutFloat(this.GetZoomVertOffsetForActiveAnimatedActor(this.m_zoomParameter * this.m_zoomParameterScale > this.m_minZoomParamForCoverZoomOffset));
							this.m_cutToTarget = false;
						}
						else
						{
							bool flag4;
							if (!this.m_respawnedBeforeUpdateEnd)
							{
								if (!InputManager.Get().IsKeyBindingHeld(KeyPreference.CameraCenterOnAction) && this.m_targetReason != CameraManager.CameraTargetReason.UserFocusingOnActor && this.m_targetReason != CameraManager.CameraTargetReason.CtfTurninRegionSpawned)
								{
									flag4 = (this.m_targetReason == CameraManager.CameraTargetReason.CtfFlagTurnedIn);
									goto IL_309;
								}
							}
							flag4 = true;
							IL_309:
							bool flag5 = flag4;
							if (this.m_targetObjectActor != null)
							{
								bool flag6 = this.m_targetObjectActor.GetActorMovement().AmMoving();
								if (flag6)
								{
									BoardSquarePathInfo aestheticPath = this.m_targetObjectActor.GetActorMovement().GetAestheticPath();
									if (aestheticPath != null)
									{
										if (aestheticPath.square != null)
										{
											if (CameraManager.Get().ShouldAutoCameraMove())
											{
												vector = aestheticPath.square.GetWorldPosition() + this.m_targetObjectOffset;
												Vector3 startValue = (vector - this.m_targetPosition) * Time.deltaTime * this.m_movementCatchUpMult + this.m_targetPosition;
												this.m_targetPosition = new EasedOutVector3(startValue);
												flag5 = false;
											}
											else if (InputManager.Get().IsKeyBindingHeld(KeyPreference.CameraCenterOnAction))
											{
												BoardSquarePathInfo pathEndpoint = aestheticPath.GetPathEndpoint();
												if (pathEndpoint != null && pathEndpoint.square != null)
												{
													vector = pathEndpoint.square.GetWorldPosition() + this.m_targetObjectOffset;
												}
											}
										}
									}
								}
								else if (!flag5)
								{
									if (CameraManager.Get().ShouldAutoCameraMove())
									{
										if (this.IsInMovementPhase())
										{
											float magnitude = (vector - this.m_targetPosition).magnitude;
											if (magnitude > 0f)
											{
												Vector3 vector2;
												if (magnitude < 1f)
												{
													vector2 = vector;
												}
												else
												{
													vector2 = (vector - this.m_targetPosition) * Mathf.Min(1f, Time.deltaTime * 2f * this.m_movementCatchUpMult) + this.m_targetPosition;
												}
												Vector3 startValue2 = vector2;
												this.m_targetPosition = new EasedOutVector3(startValue2);
											}
										}
									}
								}
							}
							if (flag5)
							{
								this.m_targetPosition.EaseTo(vector, this.m_easeInTime);
							}
						}
					}
					Vector3 vector3 = this.CalcZoomOffsetForActiveAnimatedActor(quaternion);
					if (!CameraControls.Get().IsTiltUserControlled())
					{
						quaternion = Quaternion.Euler(new Vector3(this.CalcZoomRotationX(), quaternion.eulerAngles.y, 0f));
					}
					Vector3 vector4 = this.m_targetPosition + vector3;
					if (this.m_transitionInTimeLeft > 0f)
					{
						float num4 = Easing.ExpoEaseInOut(this.m_transitionInTime - this.m_transitionInTimeLeft, 0f, 1f, this.m_transitionInTime);
						base.transform.position = Vector3.Lerp(this.m_transitionInPosition, vector4, num4);
						base.transform.rotation = Quaternion.Slerp(this.m_transitionInRotation, quaternion, num4);
						if (Camera.main != null)
						{
							Camera.main.fieldOfView = (this.CalcFOV() - this.m_transitionFOV) * num4 + this.m_transitionFOV;
						}
						this.m_transitionInTimeLeft -= Time.deltaTime;
					}
					else
					{
						base.transform.position = vector4;
						base.transform.rotation = quaternion;
						if (Camera.main != null)
						{
							Camera.main.fieldOfView = this.CalcFOV();
						}
					}
					if ((position - base.transform.position).sqrMagnitude > 1.401298E-45f)
					{
						this.m_needNameplateSortUpdate = true;
					}
					if (this.m_needNameplateSortUpdate)
					{
						if (this.m_nextNameplateSortUpdateTime >= 0f)
						{
							if (Time.time <= this.m_nextNameplateSortUpdateTime)
							{
								goto IL_7C7;
							}
						}
						if (HUD_UI.Get() != null)
						{
							if (HUD_UI.Get().m_mainScreenPanel != null)
							{
								if (HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel != null)
								{
									HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.SortNameplates();
								}
							}
						}
						this.m_needNameplateSortUpdate = false;
						this.m_nextNameplateSortUpdateTime = Time.time + 0.6f;
					}
					IL_7C7:
					this.m_respawnedBeforeUpdateEnd = false;
					if (ActorDebugUtils.Get() != null)
					{
						if (ActorDebugUtils.Get().ShowingCategory(ActorDebugUtils.DebugCategory.CameraManager, true))
						{
							ActorDebugUtils.DebugCategoryInfo debugCategoryInfo = ActorDebugUtils.Get().GetDebugCategoryInfo(ActorDebugUtils.DebugCategory.CameraManager);
							debugCategoryInfo.m_stringToDisplay = "Updating Isometric Camera:\n\n";
							ActorDebugUtils.DebugCategoryInfo debugCategoryInfo2 = debugCategoryInfo;
							string stringToDisplay = debugCategoryInfo2.m_stringToDisplay;
							debugCategoryInfo2.m_stringToDisplay = string.Concat(new object[]
							{
								stringToDisplay,
								"Position: ",
								base.transform.position,
								" | Rotation: ",
								base.transform.rotation.eulerAngles,
								"\n"
							});
							ActorDebugUtils.DebugCategoryInfo debugCategoryInfo3 = debugCategoryInfo;
							stringToDisplay = debugCategoryInfo3.m_stringToDisplay;
							debugCategoryInfo3.m_stringToDisplay = string.Concat(new object[]
							{
								stringToDisplay,
								"\tEased Position: ",
								this.m_targetPosition,
								"\n"
							});
							ActorDebugUtils.DebugCategoryInfo debugCategoryInfo4 = debugCategoryInfo;
							stringToDisplay = debugCategoryInfo4.m_stringToDisplay;
							debugCategoryInfo4.m_stringToDisplay = string.Concat(new object[]
							{
								stringToDisplay,
								"FOV: ",
								Camera.main.fieldOfView,
								"\n"
							});
							ActorDebugUtils.DebugCategoryInfo debugCategoryInfo5 = debugCategoryInfo;
							stringToDisplay = debugCategoryInfo5.m_stringToDisplay;
							debugCategoryInfo5.m_stringToDisplay = string.Concat(new object[]
							{
								stringToDisplay,
								"Zoom: ",
								this.m_zoomParameter,
								"\n"
							});
							ActorDebugUtils.DebugCategoryInfo debugCategoryInfo6 = debugCategoryInfo;
							stringToDisplay = debugCategoryInfo6.m_stringToDisplay;
							debugCategoryInfo6.m_stringToDisplay = string.Concat(new object[]
							{
								stringToDisplay,
								"\tZoom Offset: ",
								vector3,
								"\n"
							});
							ActorDebugUtils.DebugCategoryInfo debugCategoryInfo7 = debugCategoryInfo;
							stringToDisplay = debugCategoryInfo7.m_stringToDisplay;
							debugCategoryInfo7.m_stringToDisplay = string.Concat(new object[]
							{
								stringToDisplay,
								"\tZoom Goal: ",
								this.m_zoomGoalValue,
								"\n"
							});
						}
					}
					return;
				}
			}
		}

		internal void ForceTransformAtDefaultAngle(Vector3 targetPos, float yEuler)
		{
			BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(targetPos.x, targetPos.z);
			if (boardSquareSafe != null)
			{
				targetPos.y = (float)boardSquareSafe.height;
			}
			else
			{
				targetPos.y = (float)Board.Get().BaselineHeight;
			}
			this.m_targetPosition.EaseTo(targetPos, 0.0166666675f);
			CameraControls.Get().m_desiredRotationEulerAngles.y = yEuler;
			this.SetTargetObject(null, CameraManager.CameraTargetReason.ForcingTransform);
		}

		public void OnTransitionIn(CameraTransitionType type)
		{
			if (type != CameraTransitionType.Cut)
			{
				if (type != CameraTransitionType.Move)
				{
				}
				else
				{
					if (CameraManager.Get().SecondsRemainingToPauseForUserControl > 0f)
					{
						Vector3 b = this.CalcZoomOffsetForActiveAnimatedActor(base.transform.rotation);
						Vector3 startValue = base.transform.position - b;
						startValue.y = (float)Board.Get().BaselineHeight + this.m_targetObjectOffset.y;
						this.m_targetPosition = new EasedOutVector3(startValue);
					}
					this.m_transitionInTimeLeft = this.m_transitionInTime;
					this.m_transitionInPosition = base.transform.position;
					this.m_transitionInRotation = base.transform.rotation;
					this.m_transitionFOV = Camera.main.fieldOfView;
				}
			}
		}

		public void OnTransitionOut()
		{
			CameraControls.Get().m_desiredRotationEulerAngles = base.transform.rotation.eulerAngles;
			this.m_transitionInTimeLeft = 0f;
		}

		public void OnReconnect()
		{
			this.m_targetWithBoardSquareWasNeverSet = false;
		}

		private float GetMaxDistanceHorizontal()
		{
			return (DebugParameters.Get() == null || !DebugParameters.Get().GetParameterAsBool("CameraFarZoom")) ? this.m_maxHorzDist : this.m_maxDistFarHorz;
		}

		private float GetMaxDistanceVertical()
		{
			float result;
			if (DebugParameters.Get() != null && DebugParameters.Get().GetParameterAsBool("CameraFarZoom"))
			{
				result = this.m_maxDistFarVert;
			}
			else
			{
				result = this.m_maxVertDist;
			}
			return result;
		}

		private float GetHorzOffsetForActiveActor()
		{
			float result = 1.5f;
			if (GameFlowData.Get() != null && GameFlowData.Get().activeOwnedActorData != null && GameFlowData.Get().activeOwnedActorData.GetActorModelData() != null)
			{
				result = GameFlowData.Get().activeOwnedActorData.GetActorModelData().GetCameraHorzOffset();
			}
			return result;
		}

		private float CalcZoomParameter(double distance)
		{
			float maxDistanceHorizontal = this.GetMaxDistanceHorizontal();
			float horzOffsetForActiveActor = this.GetHorzOffsetForActiveActor();
			double num = (maxDistanceHorizontal != horzOffsetForActiveActor) ? ((distance - (double)horzOffsetForActiveActor) / (double)(maxDistanceHorizontal - horzOffsetForActiveActor)) : 0.0;
			return Mathf.Clamp((float)num, 0f, 1f);
		}

		private float CalcZoomParameterDelta(float distanceDelta)
		{
			float maxDistanceHorizontal = this.GetMaxDistanceHorizontal();
			float horzOffsetForActiveActor = this.GetHorzOffsetForActiveActor();
			float result;
			if (distanceDelta > 0f)
			{
				result = this.CalcZoomParameter((double)(horzOffsetForActiveActor + distanceDelta));
			}
			else
			{
				result = -1f * (1f - this.CalcZoomParameter((double)(maxDistanceHorizontal + distanceDelta)));
			}
			return result;
		}

		private float GetZoomVertOffsetForActiveAnimatedActor(bool forceStandingOffset)
		{
			float result;
			if (GameFlowData.Get() != null && GameFlowData.Get().activeOwnedActorData != null)
			{
				result = GameFlowData.Get().activeOwnedActorData.GetActorModelData().GetCameraVertOffset(forceStandingOffset);
			}
			else
			{
				result = 1.2f;
			}
			return result;
		}

		public Vector3 CalcZoomOffsetForActiveAnimatedActor(Quaternion camRotation)
		{
			float horzOffsetForActiveActor = this.GetHorzOffsetForActiveActor();
			Vector3 vector = IsometricCamera.ExtractRotationY(camRotation) * -Vector3.forward;
			float num = this.m_zoomParameter * this.m_zoomParameterScale;
			float num2 = this.GetMaxDistanceHorizontal() - horzOffsetForActiveActor;
			vector *= this.m_horzCurve.Evaluate(num) * num2 + horzOffsetForActiveActor;
			bool forceStandingOffset = num > this.m_minZoomParamForCoverZoomOffset;
			this.m_zoomVertOffsetForAnimatedActor.EaseTo(this.GetZoomVertOffsetForActiveAnimatedActor(forceStandingOffset), this.m_coverHeightEaseTime);
			float num3 = this.m_zoomVertOffsetForAnimatedActor;
			num2 = this.GetMaxDistanceVertical() - num3;
			vector.y = this.m_vertCurve.Evaluate(num) * num2 + num3;
			return vector;
		}

		private float CalcZoomRotationX()
		{
			return Mathf.Atan(this.m_tiltCurve.Evaluate(this.m_zoomParameter * this.m_zoomParameterScale)) * 57.29578f;
		}

		private float CalcFOV()
		{
			return this.m_fovCurve.Evaluate(this.m_zoomParameter * this.m_zoomParameterScale) * 100f;
		}

		private static Quaternion ExtractRotationY(Quaternion q)
		{
			return Quaternion.Euler(new Vector3(0f, q.eulerAngles.y, 0f));
		}

		private void OnDrawGizmos()
		{
			if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
			{
				return;
			}
			if (GameFlowData.Get() != null)
			{
				if (GameFlowData.Get().gameState >= GameState.BothTeams_Decision)
				{
					Gizmos.color = Color.blue;
					Gizmos.DrawWireSphere(this.m_targetPosition, 0.5f);
				}
			}
		}
	}
}
