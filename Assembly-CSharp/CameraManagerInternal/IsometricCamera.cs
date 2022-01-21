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

		internal Vector3 TargetPosition => m_targetPosition;

		private void Awake()
		{
			if (m_tiltCurve != null)
			{
				if (m_tiltCurve.keys.Length >= 1)
				{
					return;
				}
			}
			Log.Warning("Please remove misconfigured IsometricCamera component from " + base.gameObject);
			base.enabled = false;
		}

		private void Start()
		{
			if (GameManager.IsEditorAndNotGame())
			{
				base.enabled = false;
				return;
			}
			UpdateNoEdit();
			m_maxDistFarHorz = 100f * m_maxHorzDist;
			m_maxDistFarVert = 100f * m_maxVertDist;
			m_zoomParameter = new EasedOutFloat(CalcZoomParameter(m_startHorzDist));
			m_zoomGoalValue = m_zoomParameter.GetEndValue();
			m_defaultEulerRotation.x = CalcZoomRotationX();
			m_defaultEulerRotation.y = m_defaultRotationY;
			if ((bool)CameraControls.Get())
			{
				CameraControls.Get().m_desiredRotationEulerAngles = m_defaultEulerRotation;
			}
			if (GameFlowData.Get() != null)
			{
				ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
				if (activeOwnedActorData != null)
				{
					CameraManager.Get().OnActiveOwnedActorChange(activeOwnedActorData);
					ResetCameraRotation();
				}
			}
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.CharacterRespawn);
		}

		private void OnDestroy()
		{
			if (GameEventManager.Get() == null)
			{
				return;
			}
			while (true)
			{
				GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.CharacterRespawn);
				return;
			}
		}

		public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
		{
			if (eventType != GameEventManager.EventType.CharacterRespawn)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return;
					}
				}
			}
			GameEventManager.CharacterRespawnEventArgs characterRespawnEventArgs = (GameEventManager.CharacterRespawnEventArgs)args;
			if (!(GameFlowData.Get().activeOwnedActorData == characterRespawnEventArgs.respawningCharacter))
			{
				return;
			}
			while (true)
			{
				m_respawnedBeforeUpdateEnd = true;
				return;
			}
		}

		public void SetTargetObject(GameObject targetObject, CameraManager.CameraTargetReason reason)
		{
			SetTargetObject(targetObject, reason, false);
		}

		public void SetTargetObject(GameObject targetObject, CameraManager.CameraTargetReason reason, bool putUnderMouse)
		{
			if (CameraManager.CamDebugTraceOn)
			{
				if (targetObject != m_targetObject)
				{
					object str;
					if ((bool)targetObject)
					{
						str = targetObject.name;
					}
					else
					{
						str = "NULL";
					}
					CameraManager.LogForDebugging("SetTargetObject to " + (string)str, CameraManager.CameraLogType.Isometric);
				}
			}
			object obj;
			if (targetObject == null)
			{
				obj = null;
			}
			else
			{
				obj = targetObject.GetComponent<ActorData>();
			}
			ActorData actorData = (ActorData)obj;
			if (actorData != null)
			{
				if (m_targetWithBoardSquareWasNeverSet && actorData.GetCurrentBoardSquare() != null)
				{
					ResetCameraRotation();
					m_cutToTarget = true;
					m_targetWithBoardSquareWasNeverSet = false;
				}
				m_zoomVertOffsetForAnimatedActor = new EasedOutFloat(GetZoomVertOffsetForActiveAnimatedActor((float)m_zoomParameter * (float)m_zoomParameterScale > m_minZoomParamForCoverZoomOffset));
			}
			m_targetObject = targetObject;
			m_targetObjectActor = actorData;
			m_targetReason = reason;
			if (putUnderMouse)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				EasedOutVector3 targetPosition = m_targetPosition;
				if (new Plane(Vector3.up, targetPosition).Raycast(ray, out float enter))
				{
					m_targetObjectOffset = targetPosition - ray.GetPoint(enter);
				}
			}
			else
			{
				m_targetObjectOffset = Vector3.zero;
			}
			if (!(m_targetObject != null))
			{
				return;
			}
			while (true)
			{
				if (CameraManager.Get().ShouldAutoCameraMove())
				{
					while (true)
					{
						m_targetPosition.EaseTo(m_targetObject.transform.position + m_targetObjectOffset, m_easeInTime);
						return;
					}
				}
				return;
			}
		}

		public void SetTargetPosition(Vector3 pos, float easeInTime)
		{
			m_targetPosition.EaseTo(pos, easeInTime);
		}

		public float GetInitialYAngle()
		{
			float result = m_defaultRotationY;
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
			Vector3 forward = a - b;
			forward.y = 0f;
			float magnitude = forward.magnitude;
			if (!Mathf.Approximately(magnitude, 0f))
			{
				forward /= magnitude;
				Quaternion quaternion = Quaternion.LookRotation(forward);
				if (flag)
				{
					Vector3 eulerAngles = quaternion.eulerAngles;
					result = (int)eulerAngles.y / 45 * 45;
				}
				else
				{
					Vector3 eulerAngles2 = quaternion.eulerAngles;
					int num = (int)(eulerAngles2.y - m_defaultRotationY);
					int num2 = num / 90 * 90;
					if (num - num2 > num2 + 90 - num)
					{
						num2 += 90;
					}
					result = num2;
				}
			}
			return result;
		}

		public void ResetCameraRotation()
		{
			CameraControls.Get().m_desiredRotationEulerAngles.y = m_defaultRotationY;
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
			Vector3 forward = a - b;
			forward.y = 0f;
			float magnitude = forward.magnitude;
			if (Mathf.Approximately(magnitude, 0f))
			{
				return;
			}
			forward /= magnitude;
			Quaternion quaternion = Quaternion.LookRotation(forward);
			if (flag)
			{
				ref Vector3 desiredRotationEulerAngles = ref CameraControls.Get().m_desiredRotationEulerAngles;
				Vector3 eulerAngles = quaternion.eulerAngles;
				desiredRotationEulerAngles.y = (int)eulerAngles.y / 45 * 45;
				return;
			}
			Vector3 eulerAngles2 = quaternion.eulerAngles;
			int num = (int)(eulerAngles2.y - m_defaultRotationY);
			int num2 = num / 90 * 90;
			if (num - num2 > num2 + 90 - num)
			{
				num2 += 90;
			}
			CameraControls.Get().m_desiredRotationEulerAngles.y = num2;
		}

		internal bool AllowCameraShake()
		{
			return m_targetPosition.EaseFinished();
		}

		private void UpdateNoEdit()
		{
			m_NO_EDIT_zoomInTilt = Mathf.Atan(m_tiltCurve.keys[0].value) * 57.29578f;
			m_NO_EDIT_zoomOutTilt = Mathf.Atan(m_tiltCurve.keys[m_tiltCurve.keys.Length - 1].value) * 57.29578f;
			m_NO_EDIT_zoomLevel = (float)m_zoomParameter * (float)m_zoomParameterScale;
		}

		private bool IsInMovementPhase()
		{
			ActionBufferPhase currentActionPhase = ServerClientUtils.GetCurrentActionPhase();
			int result;
			if (currentActionPhase != ActionBufferPhase.Movement && currentActionPhase != ActionBufferPhase.MovementChase)
			{
				if (currentActionPhase != ActionBufferPhase.AbilitiesWait)
				{
					result = ((currentActionPhase == ActionBufferPhase.MovementWait) ? 1 : 0);
					goto IL_0038;
				}
			}
			result = 1;
			goto IL_0038;
			IL_0038:
			return (byte)result != 0;
		}

		private void Update()
		{
			Vector3 position = base.transform.position;
			Quaternion rotationThisFrame = base.transform.rotation;
			Vector3 positionDelta = Vector3.zero;
			float zoomDelta = 0f;
			if (CameraControls.Get() == null)
			{
				return;
			}
			while (true)
			{
				if (GameFlowData.Get() == null)
				{
					return;
				}
				bool flag = false;
				flag |= !m_targetWithBoardSquareWasNeverSet;
				bool num = flag;
				int num2;
				if (GameFlowData.Get().LocalPlayerData != null)
				{
					num2 = ((GameFlowData.Get().activeOwnedActorData == null) ? 1 : 0);
				}
				else
				{
					num2 = 0;
				}
				if (((num ? 1 : 0) | num2) != 0)
				{
					CameraControls.Get().CalcDesiredTransform(base.transform, out positionDelta, out rotationThisFrame, out zoomDelta);
				}
				if (!Mathf.Approximately(zoomDelta, 0f))
				{
					float num3 = CalcZoomParameterDelta(zoomDelta);
					float value = num3 + m_zoomParameter.GetEndValue() * (float)m_zoomParameterScale;
					value = Mathf.Clamp01(value);
					m_zoomParameter.EaseTo(value, m_zoomEaseTime);
					m_zoomGoalValue = value;
					m_zoomParameterScale = new EasedFloat(1f);
				}
				else if (!Mathf.Approximately(m_zoomGoalValue, m_zoomParameter.GetEndValue()))
				{
					m_zoomParameter.EaseTo(m_zoomGoalValue, m_zoomEaseTime);
				}
				Vector3 vector;
				int num4;
				if (positionDelta.sqrMagnitude > float.Epsilon)
				{
					CameraManager.Get().SecondsRemainingToPauseForUserControl = 0.5f;
					CameraManager.Get().OnPlayerMovedCamera();
					Vector3 endValue = m_targetPosition.GetEndValue() + positionDelta;
					GameplayData gameplayData = GameplayData.Get();
					endValue.x = Mathf.Clamp(endValue.x, gameplayData.m_minimumPositionX, gameplayData.m_maximumPositionX);
					endValue.z = Mathf.Clamp(endValue.z, gameplayData.m_minimumPositionZ, gameplayData.m_maximumPositionZ);
					m_targetPosition.EaseTo(endValue, m_inputMoveEaseTime);
					SetTargetObject(null, CameraManager.CameraTargetReason.ReachedTargetObj);
				}
				else if (m_targetObject != null)
				{
					vector = m_targetObject.transform.position + m_targetObjectOffset;
					if (!m_cutToTarget)
					{
						bool flag2 = false;
						if (!m_respawnedBeforeUpdateEnd)
						{
							if (!InputManager.Get().IsKeyBindingHeld(KeyPreference.CameraCenterOnAction) && m_targetReason != CameraManager.CameraTargetReason.UserFocusingOnActor && m_targetReason != CameraManager.CameraTargetReason.CtfTurninRegionSpawned)
							{
								num4 = ((m_targetReason == CameraManager.CameraTargetReason.CtfFlagTurnedIn) ? 1 : 0);
								goto IL_0309;
							}
						}
						num4 = 1;
						goto IL_0309;
					}
					m_targetPosition = new EasedOutVector3(vector);
					m_zoomVertOffsetForAnimatedActor = new EasedOutFloat(GetZoomVertOffsetForActiveAnimatedActor((float)m_zoomParameter * (float)m_zoomParameterScale > m_minZoomParamForCoverZoomOffset));
					m_cutToTarget = false;
				}
				goto IL_0565;
				IL_0565:
				Vector3 vector2 = CalcZoomOffsetForActiveAnimatedActor(rotationThisFrame);
				if (!CameraControls.Get().IsTiltUserControlled())
				{
					float x = CalcZoomRotationX();
					Vector3 eulerAngles = rotationThisFrame.eulerAngles;
					rotationThisFrame = Quaternion.Euler(new Vector3(x, eulerAngles.y, 0f));
				}
				Vector3 vector3 = m_targetPosition + vector2;
				if (m_transitionInTimeLeft > 0f)
				{
					float num5 = Easing.ExpoEaseInOut(m_transitionInTime - m_transitionInTimeLeft, 0f, 1f, m_transitionInTime);
					base.transform.position = Vector3.Lerp(m_transitionInPosition, vector3, num5);
					base.transform.rotation = Quaternion.Slerp(m_transitionInRotation, rotationThisFrame, num5);
					if (Camera.main != null)
					{
						Camera.main.fieldOfView = (CalcFOV() - m_transitionFOV) * num5 + m_transitionFOV;
					}
					m_transitionInTimeLeft -= Time.deltaTime;
				}
				else
				{
					base.transform.position = vector3;
					base.transform.rotation = rotationThisFrame;
					if (Camera.main != null)
					{
						Camera.main.fieldOfView = CalcFOV();
					}
				}
				if ((position - base.transform.position).sqrMagnitude > float.Epsilon)
				{
					m_needNameplateSortUpdate = true;
				}
				if (m_needNameplateSortUpdate)
				{
					if (!(m_nextNameplateSortUpdateTime < 0f))
					{
						if (!(Time.time > m_nextNameplateSortUpdateTime))
						{
							goto IL_07c7;
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
					m_needNameplateSortUpdate = false;
					m_nextNameplateSortUpdateTime = Time.time + 0.6f;
				}
				goto IL_07c7;
				IL_07c7:
				m_respawnedBeforeUpdateEnd = false;
				if (!(ActorDebugUtils.Get() != null))
				{
					return;
				}
				while (true)
				{
					if (ActorDebugUtils.Get().ShowingCategory(ActorDebugUtils.DebugCategory.CameraManager))
					{
						while (true)
						{
							ActorDebugUtils.DebugCategoryInfo debugCategoryInfo = ActorDebugUtils.Get().GetDebugCategoryInfo(ActorDebugUtils.DebugCategory.CameraManager);
							debugCategoryInfo.m_stringToDisplay = "Updating Isometric Camera:\n\n";
							string stringToDisplay = debugCategoryInfo.m_stringToDisplay;
							debugCategoryInfo.m_stringToDisplay = string.Concat(stringToDisplay, "Position: ", base.transform.position, " | Rotation: ", base.transform.rotation.eulerAngles, "\n");
							stringToDisplay = debugCategoryInfo.m_stringToDisplay;
							debugCategoryInfo.m_stringToDisplay = string.Concat(stringToDisplay, "\tEased Position: ", m_targetPosition, "\n");
							stringToDisplay = debugCategoryInfo.m_stringToDisplay;
							debugCategoryInfo.m_stringToDisplay = stringToDisplay + "FOV: " + Camera.main.fieldOfView + "\n";
							stringToDisplay = debugCategoryInfo.m_stringToDisplay;
							debugCategoryInfo.m_stringToDisplay = string.Concat(stringToDisplay, "Zoom: ", m_zoomParameter, "\n");
							stringToDisplay = debugCategoryInfo.m_stringToDisplay;
							debugCategoryInfo.m_stringToDisplay = string.Concat(stringToDisplay, "\tZoom Offset: ", vector2, "\n");
							stringToDisplay = debugCategoryInfo.m_stringToDisplay;
							debugCategoryInfo.m_stringToDisplay = stringToDisplay + "\tZoom Goal: " + m_zoomGoalValue + "\n";
							return;
						}
					}
					return;
				}
				IL_0309:
				bool flag3 = (byte)num4 != 0;
				if (m_targetObjectActor != null)
				{
					if (m_targetObjectActor.GetActorMovement().AmMoving())
					{
						BoardSquarePathInfo aestheticPath = m_targetObjectActor.GetActorMovement().GetAestheticPath();
						if (aestheticPath != null)
						{
							if (aestheticPath.square != null)
							{
								if (CameraManager.Get().ShouldAutoCameraMove())
								{
									vector = aestheticPath.square.GetOccupantRefPos() + m_targetObjectOffset;
									Vector3 startValue = (vector - m_targetPosition) * Time.deltaTime * m_movementCatchUpMult + m_targetPosition;
									m_targetPosition = new EasedOutVector3(startValue);
									flag3 = false;
								}
								else if (InputManager.Get().IsKeyBindingHeld(KeyPreference.CameraCenterOnAction))
								{
									BoardSquarePathInfo pathEndpoint = aestheticPath.GetPathEndpoint();
									if (pathEndpoint != null && pathEndpoint.square != null)
									{
										vector = pathEndpoint.square.GetOccupantRefPos() + m_targetObjectOffset;
									}
								}
							}
						}
					}
					else if (!flag3)
					{
						if (CameraManager.Get().ShouldAutoCameraMove())
						{
							if (IsInMovementPhase())
							{
								float magnitude = (vector - m_targetPosition).magnitude;
								if (magnitude > 0f)
								{
									Vector3 vector4;
									if (magnitude < 1f)
									{
										vector4 = vector;
									}
									else
									{
										vector4 = (vector - m_targetPosition) * Mathf.Min(1f, Time.deltaTime * 2f * m_movementCatchUpMult) + m_targetPosition;
									}
									Vector3 startValue2 = vector4;
									m_targetPosition = new EasedOutVector3(startValue2);
								}
							}
						}
					}
				}
				if (flag3)
				{
					m_targetPosition.EaseTo(vector, m_easeInTime);
				}
				goto IL_0565;
			}
		}

		internal void ForceTransformAtDefaultAngle(Vector3 targetPos, float yEuler)
		{
			BoardSquare boardSquareSafe = Board.Get().GetSquareAtPosition(targetPos.x, targetPos.z);
			if (boardSquareSafe != null)
			{
				targetPos.y = boardSquareSafe.height;
			}
			else
			{
				targetPos.y = Board.Get().BaselineHeight;
			}
			m_targetPosition.EaseTo(targetPos, 0.0166666675f);
			CameraControls.Get().m_desiredRotationEulerAngles.y = yEuler;
			SetTargetObject(null, CameraManager.CameraTargetReason.ForcingTransform);
		}

		public void OnTransitionIn(CameraTransitionType type)
		{
			switch (type)
			{
			case CameraTransitionType.Cut:
				return;
			case CameraTransitionType.Move:
				if (CameraManager.Get().SecondsRemainingToPauseForUserControl > 0f)
				{
					Vector3 b = CalcZoomOffsetForActiveAnimatedActor(base.transform.rotation);
					Vector3 startValue = base.transform.position - b;
					startValue.y = (float)Board.Get().BaselineHeight + m_targetObjectOffset.y;
					m_targetPosition = new EasedOutVector3(startValue);
				}
				m_transitionInTimeLeft = m_transitionInTime;
				m_transitionInPosition = base.transform.position;
				m_transitionInRotation = base.transform.rotation;
				m_transitionFOV = Camera.main.fieldOfView;
				return;
			}
			while (true)
			{
				return;
			}
		}

		public void OnTransitionOut()
		{
			CameraControls.Get().m_desiredRotationEulerAngles = base.transform.rotation.eulerAngles;
			m_transitionInTimeLeft = 0f;
		}

		public void OnReconnect()
		{
			m_targetWithBoardSquareWasNeverSet = false;
		}

		private float GetMaxDistanceHorizontal()
		{
			return (DebugParameters.Get() == null || !DebugParameters.Get().GetParameterAsBool("CameraFarZoom")) ? m_maxHorzDist : m_maxDistFarHorz;
		}

		private float GetMaxDistanceVertical()
		{
			float result;
			if (DebugParameters.Get() != null && DebugParameters.Get().GetParameterAsBool("CameraFarZoom"))
			{
				result = m_maxDistFarVert;
			}
			else
			{
				result = m_maxVertDist;
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
			float maxDistanceHorizontal = GetMaxDistanceHorizontal();
			float horzOffsetForActiveActor = GetHorzOffsetForActiveActor();
			double num = (maxDistanceHorizontal != horzOffsetForActiveActor) ? ((distance - (double)horzOffsetForActiveActor) / (double)(maxDistanceHorizontal - horzOffsetForActiveActor)) : 0.0;
			return Mathf.Clamp((float)num, 0f, 1f);
		}

		private float CalcZoomParameterDelta(float distanceDelta)
		{
			float maxDistanceHorizontal = GetMaxDistanceHorizontal();
			float horzOffsetForActiveActor = GetHorzOffsetForActiveActor();
			float result;
			if (distanceDelta > 0f)
			{
				result = CalcZoomParameter(horzOffsetForActiveActor + distanceDelta);
			}
			else
			{
				result = -1f * (1f - CalcZoomParameter(maxDistanceHorizontal + distanceDelta));
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
			float horzOffsetForActiveActor = GetHorzOffsetForActiveActor();
			Vector3 result = ExtractRotationY(camRotation) * -Vector3.forward;
			float num = (float)m_zoomParameter * (float)m_zoomParameterScale;
			float num2 = GetMaxDistanceHorizontal() - horzOffsetForActiveActor;
			result *= m_horzCurve.Evaluate(num) * num2 + horzOffsetForActiveActor;
			bool forceStandingOffset = num > m_minZoomParamForCoverZoomOffset;
			m_zoomVertOffsetForAnimatedActor.EaseTo(GetZoomVertOffsetForActiveAnimatedActor(forceStandingOffset), m_coverHeightEaseTime);
			float num3 = m_zoomVertOffsetForAnimatedActor;
			num2 = GetMaxDistanceVertical() - num3;
			result.y = m_vertCurve.Evaluate(num) * num2 + num3;
			return result;
		}

		private float CalcZoomRotationX()
		{
			return Mathf.Atan(m_tiltCurve.Evaluate((float)m_zoomParameter * (float)m_zoomParameterScale)) * 57.29578f;
		}

		private float CalcFOV()
		{
			return m_fovCurve.Evaluate((float)m_zoomParameter * (float)m_zoomParameterScale) * 100f;
		}

		private static Quaternion ExtractRotationY(Quaternion q)
		{
			Vector3 eulerAngles = q.eulerAngles;
			return Quaternion.Euler(new Vector3(0f, eulerAngles.y, 0f));
		}

		private void OnDrawGizmos()
		{
			if (!CameraManager.ShouldDrawGizmosForCurrentCamera() || !(GameFlowData.Get() != null))
			{
				return;
			}
			while (true)
			{
				if (GameFlowData.Get().gameState >= GameState.BothTeams_Decision)
				{
					while (true)
					{
						Gizmos.color = Color.blue;
						Gizmos.DrawWireSphere(m_targetPosition, 0.5f);
						return;
					}
				}
				return;
			}
		}
	}
}
