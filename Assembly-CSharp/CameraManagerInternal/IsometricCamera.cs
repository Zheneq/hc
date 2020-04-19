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
					RuntimeMethodHandle runtimeMethodHandle = methodof(IsometricCamera.Awake()).MethodHandle;
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(IsometricCamera.Start()).MethodHandle;
				}
				CameraControls.Get().m_desiredRotationEulerAngles = this.m_defaultEulerRotation;
			}
			if (GameFlowData.Get() != null)
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
				ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
				if (activeOwnedActorData != null)
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
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(IsometricCamera.OnDestroy()).MethodHandle;
				}
				GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.CharacterRespawn);
			}
		}

		public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
		{
			if (eventType != GameEventManager.EventType.CharacterRespawn)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(IsometricCamera.OnGameEvent(GameEventManager.EventType, GameEventManager.GameEventArgs)).MethodHandle;
				}
			}
			else
			{
				GameEventManager.CharacterRespawnEventArgs characterRespawnEventArgs = (GameEventManager.CharacterRespawnEventArgs)args;
				if (GameFlowData.Get().activeOwnedActorData == characterRespawnEventArgs.respawningCharacter)
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
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(IsometricCamera.SetTargetObject(GameObject, CameraManager.CameraTargetReason, bool)).MethodHandle;
				}
				if (targetObject != this.m_targetObject)
				{
					string str = "SetTargetObject to ";
					string str2;
					if (targetObject)
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
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				actorData = null;
			}
			else
			{
				actorData = targetObject.GetComponent<ActorData>();
			}
			ActorData actorData2 = actorData;
			if (actorData2 != null)
			{
				if (this.m_targetWithBoardSquareWasNeverSet && actorData2.\u0012() != null)
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
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				EasedOutVector3 targetPosition = this.m_targetPosition;
				Plane plane = new Plane(Vector3.up, targetPosition);
				float distance;
				if (plane.Raycast(ray, out distance))
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
					this.m_targetObjectOffset = targetPosition - ray.GetPoint(distance);
				}
			}
			else
			{
				this.m_targetObjectOffset = Vector3.zero;
			}
			if (this.m_targetObject != null)
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
				if (CameraManager.Get().ShouldAutoCameraMove())
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
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(IsometricCamera.GetInitialYAngle()).MethodHandle;
				}
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
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (SinglePlayerCoordinator.Get().m_initialCameraRotationTarget != null)
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
					a = SinglePlayerCoordinator.Get().m_initialCameraRotationTarget.transform.position;
					flag = true;
				}
			}
			Vector3 vector = a - b;
			vector.y = 0f;
			float magnitude = vector.magnitude;
			if (!Mathf.Approximately(magnitude, 0f))
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
				vector /= magnitude;
				Quaternion quaternion = Quaternion.LookRotation(vector);
				if (flag)
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
					result = (float)((int)quaternion.eulerAngles.y / 0x2D * 0x2D);
				}
				else
				{
					int num = (int)(quaternion.eulerAngles.y - this.m_defaultRotationY);
					int num2 = num / 0x5A * 0x5A;
					if (num - num2 > num2 + 0x5A - num)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(IsometricCamera.ResetCameraRotation()).MethodHandle;
				}
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
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (SinglePlayerCoordinator.Get().m_initialCameraRotationTarget != null)
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
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
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
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(IsometricCamera.IsInMovementPhase()).MethodHandle;
				}
				if (currentActionPhase != ActionBufferPhase.AbilitiesWait)
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
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(IsometricCamera.Update()).MethodHandle;
				}
				if (!(GameFlowData.Get() == null))
				{
					bool flag = false;
					flag |= !this.m_targetWithBoardSquareWasNeverSet;
					bool flag2 = flag;
					bool flag3;
					if (GameFlowData.Get().LocalPlayerData != null)
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
						flag3 = (GameFlowData.Get().activeOwnedActorData == null);
					}
					else
					{
						flag3 = false;
					}
					flag = (flag2 || flag3);
					if (flag)
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
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							this.m_targetPosition = new EasedOutVector3(vector);
							this.m_zoomVertOffsetForAnimatedActor = new EasedOutFloat(this.GetZoomVertOffsetForActiveAnimatedActor(this.m_zoomParameter * this.m_zoomParameterScale > this.m_minZoomParamForCoverZoomOffset));
							this.m_cutToTarget = false;
						}
						else
						{
							bool flag4;
							if (!this.m_respawnedBeforeUpdateEnd)
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
								if (!InputManager.Get().IsKeyBindingHeld(KeyPreference.CameraCenterOnAction) && this.m_targetReason != CameraManager.CameraTargetReason.UserFocusingOnActor && this.m_targetReason != CameraManager.CameraTargetReason.CtfTurninRegionSpawned)
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
									flag4 = (this.m_targetReason == CameraManager.CameraTargetReason.CtfFlagTurnedIn);
									goto IL_309;
								}
							}
							flag4 = true;
							IL_309:
							bool flag5 = flag4;
							if (this.m_targetObjectActor != null)
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
								bool flag6 = this.m_targetObjectActor.\u000E().AmMoving();
								if (flag6)
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
									BoardSquarePathInfo aestheticPath = this.m_targetObjectActor.\u000E().GetAestheticPath();
									if (aestheticPath != null)
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
										if (aestheticPath.square != null)
										{
											if (CameraManager.Get().ShouldAutoCameraMove())
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
												vector = aestheticPath.square.\u001D() + this.m_targetObjectOffset;
												Vector3 startValue = (vector - this.m_targetPosition) * Time.deltaTime * this.m_movementCatchUpMult + this.m_targetPosition;
												this.m_targetPosition = new EasedOutVector3(startValue);
												flag5 = false;
											}
											else if (InputManager.Get().IsKeyBindingHeld(KeyPreference.CameraCenterOnAction))
											{
												BoardSquarePathInfo pathEndpoint = aestheticPath.GetPathEndpoint();
												if (pathEndpoint != null && pathEndpoint.square != null)
												{
													vector = pathEndpoint.square.\u001D() + this.m_targetObjectOffset;
												}
											}
										}
									}
								}
								else if (!flag5)
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
									if (CameraManager.Get().ShouldAutoCameraMove())
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
										if (this.IsInMovementPhase())
										{
											float magnitude = (vector - this.m_targetPosition).magnitude;
											if (magnitude > 0f)
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
												Vector3 vector2;
												if (magnitude < 1f)
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
								for (;;)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								this.m_targetPosition.EaseTo(vector, this.m_easeInTime);
							}
						}
					}
					Vector3 vector3 = this.CalcZoomOffsetForActiveAnimatedActor(quaternion);
					if (!CameraControls.Get().IsTiltUserControlled())
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
						quaternion = Quaternion.Euler(new Vector3(this.CalcZoomRotationX(), quaternion.eulerAngles.y, 0f));
					}
					Vector3 vector4 = this.m_targetPosition + vector3;
					if (this.m_transitionInTimeLeft > 0f)
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
						float num4 = Easing.ExpoEaseInOut(this.m_transitionInTime - this.m_transitionInTimeLeft, 0f, 1f, this.m_transitionInTime);
						base.transform.position = Vector3.Lerp(this.m_transitionInPosition, vector4, num4);
						base.transform.rotation = Quaternion.Slerp(this.m_transitionInRotation, quaternion, num4);
						if (Camera.main != null)
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
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							if (Time.time <= this.m_nextNameplateSortUpdateTime)
							{
								goto IL_7C7;
							}
						}
						if (HUD_UI.Get() != null)
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
							if (HUD_UI.Get().m_mainScreenPanel != null)
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
								if (HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel != null)
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
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (ActorDebugUtils.Get().ShowingCategory(ActorDebugUtils.DebugCategory.CameraManager, true))
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
			BoardSquare boardSquare = Board.\u000E().\u0012(targetPos.x, targetPos.z);
			if (boardSquare != null)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(IsometricCamera.ForceTransformAtDefaultAngle(Vector3, float)).MethodHandle;
				}
				targetPos.y = (float)boardSquare.height;
			}
			else
			{
				targetPos.y = (float)Board.\u000E().BaselineHeight;
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
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(IsometricCamera.OnTransitionIn(CameraTransitionType)).MethodHandle;
					}
				}
				else
				{
					if (CameraManager.Get().SecondsRemainingToPauseForUserControl > 0f)
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
						Vector3 b = this.CalcZoomOffsetForActiveAnimatedActor(base.transform.rotation);
						Vector3 startValue = base.transform.position - b;
						startValue.y = (float)Board.\u000E().BaselineHeight + this.m_targetObjectOffset.y;
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(IsometricCamera.GetMaxDistanceVertical()).MethodHandle;
				}
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
			if (GameFlowData.Get() != null && GameFlowData.Get().activeOwnedActorData != null && GameFlowData.Get().activeOwnedActorData.\u000E() != null)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(IsometricCamera.GetHorzOffsetForActiveActor()).MethodHandle;
				}
				result = GameFlowData.Get().activeOwnedActorData.\u000E().GetCameraHorzOffset();
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
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(IsometricCamera.CalcZoomParameterDelta(float)).MethodHandle;
				}
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
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(IsometricCamera.GetZoomVertOffsetForActiveAnimatedActor(bool)).MethodHandle;
				}
				result = GameFlowData.Get().activeOwnedActorData.\u000E().GetCameraVertOffset(forceStandingOffset);
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
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(IsometricCamera.OnDrawGizmos()).MethodHandle;
				}
				if (GameFlowData.Get().gameState >= GameState.BothTeams_Decision)
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
					Gizmos.color = Color.blue;
					Gizmos.DrawWireSphere(this.m_targetPosition, 0.5f);
				}
			}
		}
	}
}
