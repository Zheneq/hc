using System;
using UnityEngine;

namespace CameraManagerInternal
{
	public class AbilitiesCamera : MonoBehaviour
	{
		private static AbilitiesCamera s_instance;

		[Tooltip("Amount to frame above the ability bar. A percentage of FOV")]
		public float m_bottomHUDClearance = 0.12f;

		public float m_easeInTime = 1.5f;

		public float m_easeInTimeWithinGroup = 1.2f;

		public float m_easeInTimeForSimilarBounds = 1f;

		[Space(10f)]
		public float m_kbdInputMoveEaseTime = 0.7f;

		public float m_mouseWheelInputZoomEaseTime = 1f;

		public float m_defaultExtraDistance = 2f;

		[Space(10f)]
		public float m_minAutoHeight = 7.7f;

		public float m_maxAutoHeight = 999f;

		private float m_minManualHeight = 2f;

		public float m_maxManualHeight = 25f;

		[Space(10f)]
		[Tooltip("Time desired to keep the frame after an ability hits, when hitting only caster or nothing")]
		public float m_frameTimeAfterHitMin = 0.5f;

		public float m_frameTimeAfterHitMax = 2f;

		[Tooltip("Seconds to add to Frame Time After Hit Min for each other actor hit")]
		public float m_frameTimeAfterHitPerOtherActor = 0.7f;

		[Tooltip("Read only. Inspect this value on GameCamera in the Hierarchy view while playing non-cinematic Battle Cam sequences")]
		public float m_READ_ONLY_Height;

		public float m_READ_ONLY_Distance;

		private EasedVector3Quart m_orbitLengths = new EasedVector3Quart(Vector3.zero);

		private EasedOutDegreesQuart m_orbitEulerAngleY = new EasedOutDegreesQuart(0f);

		private Eased<Vector3> m_targetBottomEased = new EasedVector3Quart(Vector3.zero);

		private Eased<float> m_zoomParameter = new EasedOutFloatQuart(0f);

		private float m_minDistTargetBottomToCam;

		[Header("-- Tilt angle and FOV. (Low Position used for ragdolling hits, etc) --")]
		public float m_defaultXRotation = 45f;

		public float m_defaultFOV = 50f;

		[Space(5f)]
		public bool m_enableLowPositionCamera;

		public float m_lowPositionXRotation = 20f;

		public float m_lowPositionFOV = 25f;

		[Tooltip("Min distance from camera bounds in order to use low position")]
		public float m_lowPositionClearanceFromMaxBounds = 10f;

		private Eased<float> m_easedDesiredXRotation = new EasedFloatQuart(40f);

		private Eased<float> m_easedDesiredFOV = new EasedFloatQuart(50f);

		private float m_transitionInTimeLeft;

		private float m_transitionInTime;

		private Quaternion m_transitionInRotation;

		private float m_transitionInFOV;

		private bool m_needNameplateSortUpdate;

		private float m_nextNameplateSortUpdateTime = -1f;

		private const float c_minNameplateSortInterval = 0.8f;

		public float m_secondsToPauseForUserControl = 1f;

		private float m_secondsRemainingToPauseForUserControl;

		private bool m_autoMoveInLastUpdate;

		[Header("-- Distance for merging similar camera bounds, max diff from any side --")]
		public float m_boundMergeSideDistThreshold = 3.1f;

		[Header("-- For checking whether ability cam should move between phases --")]
		public float m_similarCenterDistThreshold = 4f;

		public float m_similarBoundSideMaxDiff = 5f;

		[Header("   whether consider current framing as smimilar to last if last one contains current one")]
		public bool m_considerFramingSimilarIfInsidePrevious;

		private float m_easeToTargetStartTime;

		private Bounds m_target;

		private int m_targetSetTurn;

		private bool m_targetSetForQuickerTransition;

		private bool m_targetSetForLowPosition;

		internal static AbilitiesCamera Get()
		{
			return AbilitiesCamera.s_instance;
		}

		internal bool IsDisabledUntilSetTarget
		{
			get
			{
				return this.m_targetSetTurn != GameFlowData.Get().CurrentTurn;
			}
		}

		private void Awake()
		{
			AbilitiesCamera.s_instance = this;
			this.m_easedDesiredXRotation = new EasedFloatQuart(this.m_defaultXRotation);
			this.m_easedDesiredFOV = new EasedFloatQuart(this.m_defaultFOV);
		}

		private void OnDestroy()
		{
			AbilitiesCamera.s_instance = null;
		}

		internal void SetTarget(Bounds bounds, bool quickerTransition = false, bool desiredUseLowPosition = false)
		{
			bool flag;
			if (desiredUseLowPosition)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilitiesCamera.SetTarget(Bounds, bool, bool)).MethodHandle;
				}
				flag = this.m_enableLowPositionCamera;
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
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
				Bounds cameraPositionBounds = CameraManager.Get().CameraPositionBounds;
				if (Mathf.Abs(bounds.center.x - (cameraPositionBounds.center.x + cameraPositionBounds.extents.x)) > this.m_lowPositionClearanceFromMaxBounds && Mathf.Abs(bounds.center.x - (cameraPositionBounds.center.x - cameraPositionBounds.extents.x)) > this.m_lowPositionClearanceFromMaxBounds)
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
					if (Mathf.Abs(bounds.center.z - (cameraPositionBounds.center.z + cameraPositionBounds.extents.z)) > this.m_lowPositionClearanceFromMaxBounds)
					{
						if (Mathf.Abs(bounds.center.z - (cameraPositionBounds.center.z - cameraPositionBounds.extents.z)) > this.m_lowPositionClearanceFromMaxBounds)
						{
							goto IL_1AF;
						}
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
					}
				}
				if (CameraManager.CamDebugTraceOn)
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
					CameraManager.LogForDebugging(string.Concat(new object[]
					{
						"Ignoring request to use low position for ability cam, too close to edge of map.\nBounds: ",
						bounds,
						" maxBounds: ",
						cameraPositionBounds
					}), CameraManager.CameraLogType.Ability);
				}
				flag2 = false;
			}
			IL_1AF:
			bool flag3 = this.m_targetSetForLowPosition != flag2;
			this.m_targetSetTurn = GameFlowData.Get().CurrentTurn;
			this.m_targetSetForLowPosition = flag2;
			this.m_targetSetForQuickerTransition = quickerTransition;
			if (!(this.m_target != bounds))
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
				if (!flag3)
				{
					return;
				}
			}
			if (CameraManager.CamDebugTraceOn)
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
				CameraManager.LogForDebugging(string.Concat(new object[]
				{
					"Setting bounds: ",
					this.m_target.ToString(),
					" -> ",
					bounds.ToString(),
					" | quickTransition ",
					quickerTransition,
					" | useLowPosition ",
					flag2
				}), CameraManager.CameraLogType.Ability);
			}
			this.m_target = bounds;
			if (this.m_secondsRemainingToPauseForUserControl <= 0f)
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
				if (base.enabled && CameraManager.Get().ShouldAutoCameraMove())
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
					float num;
					if (quickerTransition)
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
						num = this.m_easeInTimeWithinGroup;
					}
					else
					{
						num = this.m_easeInTime;
					}
					float easeInTime = num;
					this.EaseToTarget(easeInTime, flag2);
				}
			}
		}

		internal Bounds GetTarget()
		{
			return this.m_target;
		}

		internal bool IsMovingAutomatically()
		{
			bool result;
			if (!this.m_targetBottomEased.EaseFinished())
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilitiesCamera.IsMovingAutomatically()).MethodHandle;
				}
				result = (this.m_secondsRemainingToPauseForUserControl <= 0f);
			}
			else
			{
				result = false;
			}
			return result;
		}

		private void Update()
		{
			Vector3 position = base.transform.position;
			base.transform.position = this.CalcCurrentPosition();
			base.transform.rotation = this.CalcCurrentRotation();
			Vector3 eulerAngles = base.transform.eulerAngles;
			float desiredXRotation = this.GetDesiredXRotation();
			float desiredFOV = this.GetDesiredFOV();
			if (Mathf.Approximately(eulerAngles.x, desiredXRotation))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilitiesCamera.Update()).MethodHandle;
				}
				if (Mathf.Approximately(Camera.main.fieldOfView, desiredFOV))
				{
					goto IL_192;
				}
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
			float t = Time.time - this.m_transitionInTime;
			float num;
			if (this.m_targetSetForQuickerTransition)
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
				num = this.m_easeInTimeWithinGroup;
			}
			else
			{
				num = this.m_easeInTime;
			}
			float d = num;
			float num2 = Mathf.Clamp01(Easing.QuartEaseInOut(t, 0f, 1f, d));
			float x = Mathf.LerpAngle(this.m_transitionInRotation.eulerAngles.x, desiredXRotation, num2);
			base.transform.rotation = Quaternion.Euler(x, eulerAngles.y, eulerAngles.z);
			Camera.main.fieldOfView = (desiredFOV - this.m_transitionInFOV) * num2 + this.m_transitionInFOV;
			this.m_transitionInTimeLeft -= Time.deltaTime;
			Vector3 desiredRotationEulerAngles = CameraControls.Get().m_desiredRotationEulerAngles;
			CameraControls.Get().m_desiredRotationEulerAngles = new Vector3(x, desiredRotationEulerAngles.y, desiredRotationEulerAngles.z);
			IL_192:
			if (Application.isEditor)
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
				this.m_READ_ONLY_Height = base.transform.position.y - (float)Board.Get().BaselineHeight;
				this.m_READ_ONLY_Distance = (base.transform.position - this.m_target.center).magnitude;
			}
			Vector3 b;
			Quaternion lhs;
			float num3;
			CameraControls.Get().CalcDesiredTransform(base.transform, out b, out lhs, out num3);
			if (!Mathf.Approximately(num3, 0f))
			{
				this.AdjustZoomParam(num3, true, this.m_mouseWheelInputZoomEaseTime);
			}
			bool flag = CameraManager.Get().ShouldAutoCameraMove();
			if (lhs != base.transform.rotation)
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
				this.m_orbitEulerAngleY.EaseTo(CameraControls.Get().m_desiredRotationEulerAngles.y, CameraControls.Get().m_keyboardRotationDuration);
			}
			bool flag2 = CameraControls.Get().IsMouseDragMoveRequested();
			if (flag2)
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
				this.m_secondsRemainingToPauseForUserControl = 0.1f;
				CameraManager.Get().SecondsRemainingToPauseForUserControl = 0.1f;
			}
			bool flag3 = b.sqrMagnitude > float.Epsilon;
			if (!flag3)
			{
				if (!this.m_autoMoveInLastUpdate)
				{
					goto IL_3E0;
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
				if (flag)
				{
					goto IL_3E0;
				}
			}
			if (!flag2)
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
				this.m_secondsRemainingToPauseForUserControl = this.m_secondsToPauseForUserControl;
				CameraManager.Get().SecondsRemainingToPauseForUserControl = this.m_secondsToPauseForUserControl;
			}
			if (flag3)
			{
				CameraManager.Get().OnPlayerMovedCamera();
			}
			Vector3 vector = this.CalcDesiredPosition();
			Vector3 a = vector + b;
			Vector3 endValue = a - vector + this.m_targetBottomEased.GetEndValue();
			GameplayData gameplayData = GameplayData.Get();
			endValue.x = Mathf.Clamp(endValue.x, gameplayData.m_minimumPositionX, gameplayData.m_maximumPositionX);
			endValue.z = Mathf.Clamp(endValue.z, gameplayData.m_minimumPositionZ, gameplayData.m_maximumPositionZ);
			this.m_targetBottomEased = new EasedOutVector3Quart(this.m_targetBottomEased);
			this.m_targetBottomEased.EaseTo(endValue, this.m_kbdInputMoveEaseTime);
			IL_3E0:
			if (this.m_secondsRemainingToPauseForUserControl > 0f)
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
				this.m_secondsRemainingToPauseForUserControl -= Time.deltaTime;
				if (this.m_secondsRemainingToPauseForUserControl <= 0f)
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
					if (flag)
					{
						this.EaseToTarget(this.m_easeInTime, false);
					}
				}
			}
			if ((position - base.transform.position).sqrMagnitude > 1.401298E-45f)
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
				this.m_needNameplateSortUpdate = true;
			}
			if (this.m_needNameplateSortUpdate)
			{
				if (this.m_nextNameplateSortUpdateTime >= 0f)
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
					if (Time.time <= this.m_nextNameplateSortUpdateTime)
					{
						goto IL_521;
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
				}
				if (HUD_UI.Get() != null && HUD_UI.Get().m_mainScreenPanel != null && HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel != null)
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
					HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.SortNameplates();
				}
				this.m_needNameplateSortUpdate = false;
				this.m_nextNameplateSortUpdateTime = Time.time + 0.8f;
			}
			IL_521:
			this.m_autoMoveInLastUpdate = flag;
			if (ActorDebugUtils.Get() != null)
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
				if (ActorDebugUtils.Get().ShowingCategory(ActorDebugUtils.DebugCategory.CameraManager, true))
				{
					ActorDebugUtils.DebugCategoryInfo debugCategoryInfo = ActorDebugUtils.Get().GetDebugCategoryInfo(ActorDebugUtils.DebugCategory.CameraManager);
					debugCategoryInfo.m_stringToDisplay = "Updating Abilities Camera:\n\n";
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
						"FOV: ",
						Camera.main.fieldOfView,
						"\n"
					});
					ActorDebugUtils.DebugCategoryInfo debugCategoryInfo4 = debugCategoryInfo;
					stringToDisplay = debugCategoryInfo4.m_stringToDisplay;
					debugCategoryInfo4.m_stringToDisplay = string.Concat(new object[]
					{
						stringToDisplay,
						"Height: ",
						this.m_READ_ONLY_Height,
						" | Distance: ",
						this.m_READ_ONLY_Distance,
						"\n"
					});
					ActorDebugUtils.DebugCategoryInfo debugCategoryInfo5 = debugCategoryInfo;
					stringToDisplay = debugCategoryInfo5.m_stringToDisplay;
					debugCategoryInfo5.m_stringToDisplay = string.Concat(new object[]
					{
						stringToDisplay,
						"Time Remaining Under User Control: ",
						this.m_secondsRemainingToPauseForUserControl,
						"\n"
					});
				}
			}
		}

		public void OnTransitionIn(CameraTransitionType type)
		{
			this.m_secondsRemainingToPauseForUserControl = CameraManager.Get().SecondsRemainingToPauseForUserControl;
			this.m_zoomParameter = new EasedOutFloatQuart(0f);
			if (type != CameraTransitionType.Cut)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilitiesCamera.OnTransitionIn(CameraTransitionType)).MethodHandle;
				}
				if (type != CameraTransitionType.Move)
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
				}
				else
				{
					this.m_orbitEulerAngleY = new EasedOutDegreesQuart(base.transform.eulerAngles.y);
					float num;
					if (this.m_targetSetForQuickerTransition)
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
						num = this.m_easeInTimeWithinGroup;
					}
					else
					{
						num = this.m_easeInTime;
					}
					float num2 = num;
					this.m_transitionInTimeLeft = num2;
					this.EaseToTarget(num2, this.m_targetSetForLowPosition);
					this.m_transitionInRotation = base.transform.rotation;
					this.m_transitionInFOV = Camera.main.fieldOfView;
				}
			}
			else
			{
				Camera.main.fieldOfView = this.m_defaultFOV;
				CameraControls.Get().m_desiredRotationEulerAngles = new Vector3(this.m_defaultXRotation, CameraControls.Get().m_desiredRotationEulerAngles.y, 0f);
				this.m_orbitEulerAngleY = new EasedOutDegreesQuart(CameraControls.Get().m_desiredRotationEulerAngles.y);
				base.transform.rotation = Quaternion.Euler(CameraControls.Get().m_desiredRotationEulerAngles);
				this.m_transitionInFOV = this.m_defaultFOV;
				this.m_transitionInRotation = base.transform.rotation;
				base.transform.position = this.CalcDesiredPosition();
				this.m_transitionInTimeLeft = 0f;
				Vector3 startValue = new Vector3(this.m_target.center.x, this.m_target.min.y, this.m_target.center.z);
				this.m_targetBottomEased = new EasedVector3Quart(startValue);
				this.m_orbitLengths = new EasedVector3Quart(this.CalcDesiredOrbitLengths(out this.m_minDistTargetBottomToCam, false));
			}
			this.m_transitionInTime = Time.time;
		}

		public void OnTransitionOut()
		{
			this.m_targetBottomEased = new EasedVector3Quart(new Vector3(this.m_target.center.x, this.m_target.min.y, this.m_target.center.z));
		}

		internal float GetSecondsRemainingToPauseForUserControl()
		{
			return this.m_secondsRemainingToPauseForUserControl;
		}

		internal void OnAutoCenterCameraPreferenceSet()
		{
			this.m_secondsRemainingToPauseForUserControl = 0f;
		}

		private void EaseToTarget(float easeInTime, bool useLowPosition)
		{
			this.m_easeToTargetStartTime = Time.time;
			if (useLowPosition)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilitiesCamera.EaseToTarget(float, bool)).MethodHandle;
				}
				AudioManager.PostEvent("Set_state_action_cam", null);
			}
			this.m_orbitLengths = new EasedVector3Quart(this.CalcDesiredOrbitLengths(out this.m_minDistTargetBottomToCam, useLowPosition));
			Vector3 vector = new Vector3(this.m_target.center.x, this.m_target.min.y, this.m_target.center.z);
			this.m_targetBottomEased = new EasedVector3Quart(vector);
			Vector3 b = this.CalcDesiredPosition();
			this.m_targetBottomEased = new EasedVector3Quart(base.transform.position - b + vector);
			this.m_targetBottomEased.EaseTo(vector, easeInTime);
			this.AdjustZoomParam(0f, false, easeInTime);
			this.m_orbitEulerAngleY.EaseTo(this.m_orbitEulerAngleY.EndValue(), easeInTime);
			float num;
			if (useLowPosition)
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
				num = this.m_lowPositionXRotation;
			}
			else
			{
				num = this.m_defaultXRotation;
			}
			float num2 = num;
			this.m_easedDesiredXRotation.EaseTo(num2, easeInTime);
			float num3;
			if (useLowPosition)
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
				num3 = this.m_lowPositionFOV;
			}
			else
			{
				num3 = this.m_defaultFOV;
			}
			float endValue = num3;
			this.m_easedDesiredFOV.EaseTo(endValue, easeInTime);
			if (CameraManager.CamDebugTraceOn)
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
				CameraManager.LogForDebugging(string.Concat(new object[]
				{
					"EaseToTarget ",
					this.m_target.ToString(),
					" easeInTime: ",
					easeInTime,
					"\nDesired X Rotation = ",
					num2,
					"\nSeconds remaining under user control = ",
					this.m_secondsRemainingToPauseForUserControl
				}), CameraManager.CameraLogType.Ability);
			}
		}

		private void AdjustZoomParam(float zoomDelta, bool userControlled, float easeInTime)
		{
			float num = zoomDelta + this.m_zoomParameter.GetEndValue();
			Vector3 endValue = this.m_orbitLengths.GetEndValue();
			float magnitude = endValue.magnitude;
			Vector3 vector = (magnitude <= 0f) ? Vector3.up : (endValue / magnitude);
			if (magnitude + this.m_defaultExtraDistance + num < this.m_minDistTargetBottomToCam)
			{
				num = this.m_minDistTargetBottomToCam - magnitude - this.m_defaultExtraDistance;
			}
			float b = endValue.y + vector.y * (this.m_defaultExtraDistance + this.m_zoomParameter.GetEndValue());
			float num2 = endValue.y + vector.y * (this.m_defaultExtraDistance + num);
			float max = (!userControlled) ? this.m_maxAutoHeight : this.m_maxManualHeight;
			num2 = Mathf.Clamp(num2, this.m_minManualHeight, max);
			if (!Mathf.Approximately(num2, b))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilitiesCamera.AdjustZoomParam(float, bool, float)).MethodHandle;
				}
				if (vector.y > 0f)
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
					num = (num2 - endValue.y - vector.y * this.m_defaultExtraDistance) / vector.y;
					this.m_zoomParameter = ((!userControlled) ? new EasedFloatQuart(this.m_zoomParameter) : new EasedOutFloatQuart(this.m_zoomParameter));
					this.m_zoomParameter.EaseTo(num, easeInTime);
				}
			}
		}

		internal float CalcPositionEaseTimeRemaining()
		{
			return this.m_targetBottomEased.CalcTimeRemaining();
		}

		private float CalcTotalEaseTimeRemaining()
		{
			float a = this.m_targetBottomEased.CalcTimeRemaining();
			float a2 = this.m_orbitEulerAngleY.CalcTimeRemaining();
			float a3 = this.m_orbitLengths.CalcTimeRemaining();
			float num = this.m_zoomParameter.CalcTimeRemaining();
			return Mathf.Max(a, Mathf.Max(a2, Mathf.Max(a3, Mathf.Max(new float[]
			{
				num
			}))));
		}

		internal float CalcFrameTimeAfterHit(int numOtherActorsHit)
		{
			return Mathf.Min(this.m_frameTimeAfterHitMax, this.m_frameTimeAfterHitMin + (float)numOtherActorsHit * this.m_frameTimeAfterHitPerOtherActor);
		}

		private Quaternion CalcCurrentRotation()
		{
			return Quaternion.Euler(base.transform.eulerAngles.x, this.m_orbitEulerAngleY, 0f);
		}

		private Quaternion CalcDesiredRotation()
		{
			return this.CalcDesiredRotation(CameraControls.Get().m_desiredRotationEulerAngles.y, false);
		}

		private Quaternion CalcDesiredRotation(float eulerAngleY, bool useLowPosition)
		{
			float num;
			if (useLowPosition)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilitiesCamera.CalcDesiredRotation(float, bool)).MethodHandle;
				}
				num = this.m_lowPositionXRotation;
			}
			else
			{
				num = this.m_defaultXRotation;
			}
			float x = num;
			Vector3 euler = new Vector3(x, eulerAngleY, 0f);
			return Quaternion.Euler(euler);
		}

		private Vector3 CalcCurrentPosition()
		{
			return this.m_targetBottomEased + Quaternion.Euler(0f, this.m_orbitEulerAngleY, 0f) * (this.m_orbitLengths + this.m_orbitLengths.normalized * (this.m_zoomParameter + this.m_defaultExtraDistance));
		}

		private Vector3 CalcDesiredPosition()
		{
			return this.m_targetBottomEased.GetEndValue() + Quaternion.Euler(0f, this.m_orbitEulerAngleY.GetEndValue(), 0f) * (this.m_orbitLengths.GetEndValue() + this.m_orbitLengths.GetEndValue().normalized * (this.m_zoomParameter.GetEndValue() + this.m_defaultExtraDistance));
		}

		private unsafe Vector3 CalcDesiredOrbitLengths(out float minDistTargetBottomToCam, bool useLowPosition)
		{
			float y = CameraControls.Get().m_desiredRotationEulerAngles.y;
			float fieldOfView = Camera.main.fieldOfView;
			Camera main = Camera.main;
			float fieldOfView2;
			if (useLowPosition)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilitiesCamera.CalcDesiredOrbitLengths(float*, bool)).MethodHandle;
				}
				fieldOfView2 = this.m_lowPositionFOV;
			}
			else
			{
				fieldOfView2 = this.m_defaultFOV;
			}
			main.fieldOfView = fieldOfView2;
			Quaternion rotation = base.transform.rotation;
			base.transform.rotation = this.CalcDesiredRotation(y, useLowPosition);
			Vector3 position = base.transform.position;
			Quaternion rotation2 = Quaternion.AngleAxis(-this.m_bottomHUDClearance * Camera.main.fieldOfView / 2f, base.transform.right);
			Vector3 vector = rotation2 * base.transform.forward;
			Ray ray = new Ray(base.transform.position, vector);
			Plane plane = new Plane(Vector3.up, this.m_target.center);
			Vector3 b = Vector3.zero;
			float distance;
			if (plane.Raycast(ray, out distance))
			{
				Vector3 point = ray.GetPoint(distance);
				b = this.m_target.center - point;
			}
			Vector3 b2 = Vector3.zero;
			Vector3[] array = new Vector3[]
			{
				new Vector3(this.m_target.min.x, this.m_target.max.y, this.m_target.min.z) - b,
				new Vector3(this.m_target.min.x, this.m_target.max.y, this.m_target.max.z) - b,
				new Vector3(this.m_target.max.x, this.m_target.max.y, this.m_target.min.z) - b,
				new Vector3(this.m_target.max.x, this.m_target.max.y, this.m_target.max.z) - b,
				new Vector3(this.m_target.min.x, this.m_target.min.y, this.m_target.min.z) - b,
				new Vector3(this.m_target.min.x, this.m_target.min.y, this.m_target.max.z) - b,
				new Vector3(this.m_target.max.x, this.m_target.min.y, this.m_target.min.z) - b,
				new Vector3(this.m_target.max.x, this.m_target.min.y, this.m_target.max.z) - b
			};
			Plane[] array2 = GeometryUtility.CalculateFrustumPlanes(Camera.main);
			for (int i = 0; i < array2.Length; i++)
			{
				Plane plane2 = array2[i];
				if (Vector3.Dot(base.transform.forward, plane2.normal) < -0.9f)
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
				}
				else
				{
					if (Vector3.Dot(plane2.normal, base.transform.up) > 0f)
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
						plane2.SetNormalAndPosition(rotation2 * plane2.normal, base.transform.position);
						array2[i] = plane2;
					}
					array2[i] = plane2;
				}
			}
			bool flag = true;
			foreach (Plane plane3 in array2)
			{
				if (Vector3.Dot(base.transform.forward, plane3.normal) < -0.9f)
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
					foreach (Vector3 inPt in array)
					{
						if (plane3.GetDistanceToPoint(inPt) < 0f)
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
							flag = false;
							break;
						}
					}
				}
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			Vector3 vector2;
			if (flag)
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
				vector2 = -vector;
			}
			else
			{
				vector2 = vector;
			}
			Vector3 direction = vector2;
			float num = (!flag) ? float.MinValue : float.MaxValue;
			foreach (Vector3 vector3 in array)
			{
				Ray ray2 = new Ray(vector3, direction);
				foreach (Plane plane4 in array2)
				{
					float num2;
					if (Vector3.Dot(base.transform.forward, plane4.normal) < -0.9f)
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
					}
					else if (plane4.Raycast(ray2, out num2))
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
						Vector3 vector4 = vector3 - ray2.GetPoint(num2);
						if (flag)
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
							if (num2 < num)
							{
								goto IL_692;
							}
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						if (flag)
						{
							goto IL_69A;
						}
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (num2 <= num)
						{
							goto IL_69A;
						}
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						IL_692:
						b2 = vector4;
						num = num2;
					}
					IL_69A:;
				}
			}
			Vector3 vector5 = base.transform.position + b + b2;
			Vector3 b3 = new Vector3(this.m_target.center.x, this.m_target.min.y, this.m_target.center.z);
			Vector3 vector6 = vector5 - b3;
			minDistTargetBottomToCam = vector6.magnitude;
			Vector3 vector7;
			if (minDistTargetBottomToCam > 0f)
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
				vector7 = vector6 / minDistTargetBottomToCam;
			}
			else
			{
				vector7 = Vector3.up;
			}
			Vector3 a = vector7;
			if (Vector3.Dot(Vector3.up, vector6) > 0f)
			{
				float num3 = vector5.y + a.y * this.m_defaultExtraDistance - (float)Board.Get().BaselineHeight;
				float num4 = (!useLowPosition) ? this.m_defaultXRotation : this.m_lowPositionXRotation;
				if (num3 < this.m_minAutoHeight)
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
					vector5 = a * this.m_minAutoHeight / Mathf.Cos(0.0174532924f * num4) + b3;
				}
				else if (num3 > this.m_maxAutoHeight)
				{
					vector5 = a * this.m_maxAutoHeight / Mathf.Cos(0.0174532924f * num4) + b3;
				}
			}
			base.transform.position = vector5;
			Vector3 result = this.CalcCurrentOrbitLengths();
			Camera.main.fieldOfView = fieldOfView;
			base.transform.rotation = rotation;
			base.transform.position = position;
			return result;
		}

		private Vector3 CalcCurrentOrbitLengths()
		{
			Vector3 center = this.m_target.center;
			center.y -= this.m_target.extents.y;
			Vector3 result = base.transform.position - center;
			result.z = -Mathf.Sqrt(result.x * result.x + result.z * result.z);
			result.x = 0f;
			return result;
		}

		private float GetDesiredXRotation()
		{
			return this.m_easedDesiredXRotation;
		}

		private float GetDesiredFOV()
		{
			return this.m_easedDesiredFOV;
		}
	}
}
