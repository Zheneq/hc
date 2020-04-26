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

		internal bool IsDisabledUntilSetTarget => m_targetSetTurn != GameFlowData.Get().CurrentTurn;

		internal static AbilitiesCamera Get()
		{
			return s_instance;
		}

		private void Awake()
		{
			s_instance = this;
			m_easedDesiredXRotation = new EasedFloatQuart(m_defaultXRotation);
			m_easedDesiredFOV = new EasedFloatQuart(m_defaultFOV);
		}

		private void OnDestroy()
		{
			s_instance = null;
		}

		internal void SetTarget(Bounds bounds, bool quickerTransition = false, bool desiredUseLowPosition = false)
		{
			int num;
			if (desiredUseLowPosition)
			{
				num = (m_enableLowPositionCamera ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			bool flag = (byte)num != 0;
			if (flag)
			{
				Bounds cameraPositionBounds = CameraManager.Get().CameraPositionBounds;
				Vector3 center = bounds.center;
				float x = center.x;
				Vector3 center2 = cameraPositionBounds.center;
				float x2 = center2.x;
				Vector3 extents = cameraPositionBounds.extents;
				if (!(Mathf.Abs(x - (x2 + extents.x)) <= m_lowPositionClearanceFromMaxBounds))
				{
					Vector3 center3 = bounds.center;
					float x3 = center3.x;
					Vector3 center4 = cameraPositionBounds.center;
					float x4 = center4.x;
					Vector3 extents2 = cameraPositionBounds.extents;
					if (!(Mathf.Abs(x3 - (x4 - extents2.x)) <= m_lowPositionClearanceFromMaxBounds))
					{
						Vector3 center5 = bounds.center;
						float z = center5.z;
						Vector3 center6 = cameraPositionBounds.center;
						float z2 = center6.z;
						Vector3 extents3 = cameraPositionBounds.extents;
						if (!(Mathf.Abs(z - (z2 + extents3.z)) <= m_lowPositionClearanceFromMaxBounds))
						{
							Vector3 center7 = bounds.center;
							float z3 = center7.z;
							Vector3 center8 = cameraPositionBounds.center;
							float z4 = center8.z;
							Vector3 extents4 = cameraPositionBounds.extents;
							if (!(Mathf.Abs(z3 - (z4 - extents4.z)) <= m_lowPositionClearanceFromMaxBounds))
							{
								goto IL_01af;
							}
						}
					}
				}
				if (CameraManager.CamDebugTraceOn)
				{
					CameraManager.LogForDebugging(string.Concat("Ignoring request to use low position for ability cam, too close to edge of map.\nBounds: ", bounds, " maxBounds: ", cameraPositionBounds), CameraManager.CameraLogType.Ability);
				}
				flag = false;
			}
			goto IL_01af;
			IL_01af:
			bool flag2 = m_targetSetForLowPosition != flag;
			m_targetSetTurn = GameFlowData.Get().CurrentTurn;
			m_targetSetForLowPosition = flag;
			m_targetSetForQuickerTransition = quickerTransition;
			if (!(m_target != bounds))
			{
				if (!flag2)
				{
					return;
				}
			}
			if (CameraManager.CamDebugTraceOn)
			{
				CameraManager.LogForDebugging("Setting bounds: " + m_target.ToString() + " -> " + bounds.ToString() + " | quickTransition " + quickerTransition + " | useLowPosition " + flag, CameraManager.CameraLogType.Ability);
			}
			m_target = bounds;
			if (!(m_secondsRemainingToPauseForUserControl <= 0f))
			{
				return;
			}
			while (true)
			{
				if (!base.enabled || !CameraManager.Get().ShouldAutoCameraMove())
				{
					return;
				}
				while (true)
				{
					float num2;
					if (quickerTransition)
					{
						num2 = m_easeInTimeWithinGroup;
					}
					else
					{
						num2 = m_easeInTime;
					}
					float easeInTime = num2;
					EaseToTarget(easeInTime, flag);
					return;
				}
			}
		}

		internal Bounds GetTarget()
		{
			return m_target;
		}

		internal bool IsMovingAutomatically()
		{
			int result;
			if (!m_targetBottomEased.EaseFinished())
			{
				result = ((m_secondsRemainingToPauseForUserControl <= 0f) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}

		private void Update()
		{
			Vector3 position = base.transform.position;
			base.transform.position = CalcCurrentPosition();
			base.transform.rotation = CalcCurrentRotation();
			Vector3 eulerAngles = base.transform.eulerAngles;
			float desiredXRotation = GetDesiredXRotation();
			float desiredFOV = GetDesiredFOV();
			if (Mathf.Approximately(eulerAngles.x, desiredXRotation))
			{
				if (Mathf.Approximately(Camera.main.fieldOfView, desiredFOV))
				{
					goto IL_0192;
				}
			}
			float t = Time.time - m_transitionInTime;
			float num;
			if (m_targetSetForQuickerTransition)
			{
				num = m_easeInTimeWithinGroup;
			}
			else
			{
				num = m_easeInTime;
			}
			float d = num;
			float num2 = Mathf.Clamp01(Easing.QuartEaseInOut(t, 0f, 1f, d));
			Vector3 eulerAngles2 = m_transitionInRotation.eulerAngles;
			float x = Mathf.LerpAngle(eulerAngles2.x, desiredXRotation, num2);
			base.transform.rotation = Quaternion.Euler(x, eulerAngles.y, eulerAngles.z);
			Camera.main.fieldOfView = (desiredFOV - m_transitionInFOV) * num2 + m_transitionInFOV;
			m_transitionInTimeLeft -= Time.deltaTime;
			Vector3 desiredRotationEulerAngles = CameraControls.Get().m_desiredRotationEulerAngles;
			CameraControls.Get().m_desiredRotationEulerAngles = new Vector3(x, desiredRotationEulerAngles.y, desiredRotationEulerAngles.z);
			goto IL_0192;
			IL_0192:
			if (Application.isEditor)
			{
				Vector3 position2 = base.transform.position;
				m_READ_ONLY_Height = position2.y - (float)Board.Get().BaselineHeight;
				m_READ_ONLY_Distance = (base.transform.position - m_target.center).magnitude;
			}
			CameraControls.Get().CalcDesiredTransform(base.transform, out Vector3 positionDelta, out Quaternion rotationThisFrame, out float zoomDelta);
			if (!Mathf.Approximately(zoomDelta, 0f))
			{
				AdjustZoomParam(zoomDelta, true, m_mouseWheelInputZoomEaseTime);
			}
			bool flag = CameraManager.Get().ShouldAutoCameraMove();
			if (rotationThisFrame != base.transform.rotation)
			{
				m_orbitEulerAngleY.EaseTo(CameraControls.Get().m_desiredRotationEulerAngles.y, CameraControls.Get().m_keyboardRotationDuration);
			}
			bool flag2 = CameraControls.Get().IsMouseDragMoveRequested();
			if (flag2)
			{
				m_secondsRemainingToPauseForUserControl = 0.1f;
				CameraManager.Get().SecondsRemainingToPauseForUserControl = 0.1f;
			}
			bool flag3 = positionDelta.sqrMagnitude > float.Epsilon;
			if (flag3)
			{
				goto IL_02fc;
			}
			if (m_autoMoveInLastUpdate)
			{
				if (!flag)
				{
					goto IL_02fc;
				}
			}
			goto IL_03e0;
			IL_02fc:
			if (!flag2)
			{
				m_secondsRemainingToPauseForUserControl = m_secondsToPauseForUserControl;
				CameraManager.Get().SecondsRemainingToPauseForUserControl = m_secondsToPauseForUserControl;
			}
			if (flag3)
			{
				CameraManager.Get().OnPlayerMovedCamera();
			}
			Vector3 vector = CalcDesiredPosition();
			Vector3 a = vector + positionDelta;
			Vector3 endValue = a - vector + m_targetBottomEased.GetEndValue();
			GameplayData gameplayData = GameplayData.Get();
			endValue.x = Mathf.Clamp(endValue.x, gameplayData.m_minimumPositionX, gameplayData.m_maximumPositionX);
			endValue.z = Mathf.Clamp(endValue.z, gameplayData.m_minimumPositionZ, gameplayData.m_maximumPositionZ);
			m_targetBottomEased = new EasedOutVector3Quart(m_targetBottomEased);
			m_targetBottomEased.EaseTo(endValue, m_kbdInputMoveEaseTime);
			goto IL_03e0;
			IL_03e0:
			if (m_secondsRemainingToPauseForUserControl > 0f)
			{
				m_secondsRemainingToPauseForUserControl -= Time.deltaTime;
				if (m_secondsRemainingToPauseForUserControl <= 0f)
				{
					if (flag)
					{
						EaseToTarget(m_easeInTime, false);
					}
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
						goto IL_0521;
					}
				}
				if (HUD_UI.Get() != null && HUD_UI.Get().m_mainScreenPanel != null && HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel != null)
				{
					HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.SortNameplates();
				}
				m_needNameplateSortUpdate = false;
				m_nextNameplateSortUpdateTime = Time.time + 0.8f;
			}
			goto IL_0521;
			IL_0521:
			m_autoMoveInLastUpdate = flag;
			if (!(ActorDebugUtils.Get() != null))
			{
				return;
			}
			while (true)
			{
				if (ActorDebugUtils.Get().ShowingCategory(ActorDebugUtils.DebugCategory.CameraManager))
				{
					ActorDebugUtils.DebugCategoryInfo debugCategoryInfo = ActorDebugUtils.Get().GetDebugCategoryInfo(ActorDebugUtils.DebugCategory.CameraManager);
					debugCategoryInfo.m_stringToDisplay = "Updating Abilities Camera:\n\n";
					string stringToDisplay = debugCategoryInfo.m_stringToDisplay;
					debugCategoryInfo.m_stringToDisplay = string.Concat(stringToDisplay, "Position: ", base.transform.position, " | Rotation: ", base.transform.rotation.eulerAngles, "\n");
					stringToDisplay = debugCategoryInfo.m_stringToDisplay;
					debugCategoryInfo.m_stringToDisplay = stringToDisplay + "FOV: " + Camera.main.fieldOfView + "\n";
					stringToDisplay = debugCategoryInfo.m_stringToDisplay;
					debugCategoryInfo.m_stringToDisplay = stringToDisplay + "Height: " + m_READ_ONLY_Height + " | Distance: " + m_READ_ONLY_Distance + "\n";
					stringToDisplay = debugCategoryInfo.m_stringToDisplay;
					debugCategoryInfo.m_stringToDisplay = stringToDisplay + "Time Remaining Under User Control: " + m_secondsRemainingToPauseForUserControl + "\n";
				}
				return;
			}
		}

		public void OnTransitionIn(CameraTransitionType type)
		{
			m_secondsRemainingToPauseForUserControl = CameraManager.Get().SecondsRemainingToPauseForUserControl;
			m_zoomParameter = new EasedOutFloatQuart(0f);
			if (type != 0)
			{
				if (type != CameraTransitionType.Move)
				{
				}
				else
				{
					Vector3 eulerAngles = base.transform.eulerAngles;
					m_orbitEulerAngleY = new EasedOutDegreesQuart(eulerAngles.y);
					float transitionInTimeLeft;
					if (m_targetSetForQuickerTransition)
					{
						transitionInTimeLeft = m_easeInTimeWithinGroup;
					}
					else
					{
						transitionInTimeLeft = m_easeInTime;
					}
					EaseToTarget(m_transitionInTimeLeft = transitionInTimeLeft, m_targetSetForLowPosition);
					m_transitionInRotation = base.transform.rotation;
					m_transitionInFOV = Camera.main.fieldOfView;
				}
			}
			else
			{
				Camera.main.fieldOfView = m_defaultFOV;
				CameraControls.Get().m_desiredRotationEulerAngles = new Vector3(m_defaultXRotation, CameraControls.Get().m_desiredRotationEulerAngles.y, 0f);
				m_orbitEulerAngleY = new EasedOutDegreesQuart(CameraControls.Get().m_desiredRotationEulerAngles.y);
				base.transform.rotation = Quaternion.Euler(CameraControls.Get().m_desiredRotationEulerAngles);
				m_transitionInFOV = m_defaultFOV;
				m_transitionInRotation = base.transform.rotation;
				base.transform.position = CalcDesiredPosition();
				m_transitionInTimeLeft = 0f;
				Vector3 center = m_target.center;
				float x = center.x;
				Vector3 min = m_target.min;
				float y = min.y;
				Vector3 center2 = m_target.center;
				Vector3 startValue = new Vector3(x, y, center2.z);
				m_targetBottomEased = new EasedVector3Quart(startValue);
				m_orbitLengths = new EasedVector3Quart(CalcDesiredOrbitLengths(out m_minDistTargetBottomToCam, false));
			}
			m_transitionInTime = Time.time;
		}

		public void OnTransitionOut()
		{
			Vector3 center = m_target.center;
			float x = center.x;
			Vector3 min = m_target.min;
			float y = min.y;
			Vector3 center2 = m_target.center;
			m_targetBottomEased = new EasedVector3Quart(new Vector3(x, y, center2.z));
		}

		internal float GetSecondsRemainingToPauseForUserControl()
		{
			return m_secondsRemainingToPauseForUserControl;
		}

		internal void OnAutoCenterCameraPreferenceSet()
		{
			m_secondsRemainingToPauseForUserControl = 0f;
		}

		private void EaseToTarget(float easeInTime, bool useLowPosition)
		{
			m_easeToTargetStartTime = Time.time;
			if (useLowPosition)
			{
				AudioManager.PostEvent("Set_state_action_cam");
			}
			m_orbitLengths = new EasedVector3Quart(CalcDesiredOrbitLengths(out m_minDistTargetBottomToCam, useLowPosition));
			Vector3 center = m_target.center;
			float x = center.x;
			Vector3 min = m_target.min;
			float y = min.y;
			Vector3 center2 = m_target.center;
			Vector3 vector = new Vector3(x, y, center2.z);
			m_targetBottomEased = new EasedVector3Quart(vector);
			Vector3 b = CalcDesiredPosition();
			m_targetBottomEased = new EasedVector3Quart(base.transform.position - b + vector);
			m_targetBottomEased.EaseTo(vector, easeInTime);
			AdjustZoomParam(0f, false, easeInTime);
			m_orbitEulerAngleY.EaseTo(m_orbitEulerAngleY.EndValue(), easeInTime);
			float num;
			if (useLowPosition)
			{
				num = m_lowPositionXRotation;
			}
			else
			{
				num = m_defaultXRotation;
			}
			float num2 = num;
			m_easedDesiredXRotation.EaseTo(num2, easeInTime);
			float num3;
			if (useLowPosition)
			{
				num3 = m_lowPositionFOV;
			}
			else
			{
				num3 = m_defaultFOV;
			}
			float endValue = num3;
			m_easedDesiredFOV.EaseTo(endValue, easeInTime);
			if (!CameraManager.CamDebugTraceOn)
			{
				return;
			}
			while (true)
			{
				CameraManager.LogForDebugging("EaseToTarget " + m_target.ToString() + " easeInTime: " + easeInTime + "\nDesired X Rotation = " + num2 + "\nSeconds remaining under user control = " + m_secondsRemainingToPauseForUserControl, CameraManager.CameraLogType.Ability);
				return;
			}
		}

		private void AdjustZoomParam(float zoomDelta, bool userControlled, float easeInTime)
		{
			float num = zoomDelta + m_zoomParameter.GetEndValue();
			Vector3 endValue = m_orbitLengths.GetEndValue();
			float magnitude = endValue.magnitude;
			Vector3 vector = (!(magnitude > 0f)) ? Vector3.up : (endValue / magnitude);
			if (magnitude + m_defaultExtraDistance + num < m_minDistTargetBottomToCam)
			{
				num = m_minDistTargetBottomToCam - magnitude - m_defaultExtraDistance;
			}
			float b = endValue.y + vector.y * (m_defaultExtraDistance + m_zoomParameter.GetEndValue());
			float value = endValue.y + vector.y * (m_defaultExtraDistance + num);
			float max = (!userControlled) ? m_maxAutoHeight : m_maxManualHeight;
			value = Mathf.Clamp(value, m_minManualHeight, max);
			if (Mathf.Approximately(value, b))
			{
				return;
			}
			while (true)
			{
				if (vector.y > 0f)
				{
					while (true)
					{
						num = (value - endValue.y - vector.y * m_defaultExtraDistance) / vector.y;
						m_zoomParameter = ((!userControlled) ? ((Eased<float>)new EasedFloatQuart(m_zoomParameter)) : ((Eased<float>)new EasedOutFloatQuart(m_zoomParameter)));
						m_zoomParameter.EaseTo(num, easeInTime);
						return;
					}
				}
				return;
			}
		}

		internal float CalcPositionEaseTimeRemaining()
		{
			return m_targetBottomEased.CalcTimeRemaining();
		}

		private float CalcTotalEaseTimeRemaining()
		{
			float a = m_targetBottomEased.CalcTimeRemaining();
			float a2 = m_orbitEulerAngleY.CalcTimeRemaining();
			float a3 = m_orbitLengths.CalcTimeRemaining();
			float num = m_zoomParameter.CalcTimeRemaining();
			return Mathf.Max(a, Mathf.Max(a2, Mathf.Max(a3, Mathf.Max(num))));
		}

		internal float CalcFrameTimeAfterHit(int numOtherActorsHit)
		{
			return Mathf.Min(m_frameTimeAfterHitMax, m_frameTimeAfterHitMin + (float)numOtherActorsHit * m_frameTimeAfterHitPerOtherActor);
		}

		private Quaternion CalcCurrentRotation()
		{
			Vector3 eulerAngles = base.transform.eulerAngles;
			return Quaternion.Euler(eulerAngles.x, m_orbitEulerAngleY, 0f);
		}

		private Quaternion CalcDesiredRotation()
		{
			return CalcDesiredRotation(CameraControls.Get().m_desiredRotationEulerAngles.y, false);
		}

		private Quaternion CalcDesiredRotation(float eulerAngleY, bool useLowPosition)
		{
			float num;
			if (useLowPosition)
			{
				num = m_lowPositionXRotation;
			}
			else
			{
				num = m_defaultXRotation;
			}
			float x = num;
			Vector3 euler = new Vector3(x, eulerAngleY, 0f);
			return Quaternion.Euler(euler);
		}

		private Vector3 CalcCurrentPosition()
		{
			return m_targetBottomEased + Quaternion.Euler(0f, m_orbitEulerAngleY, 0f) * (m_orbitLengths + ((Vector3)m_orbitLengths).normalized * ((float)m_zoomParameter + m_defaultExtraDistance));
		}

		private Vector3 CalcDesiredPosition()
		{
			return m_targetBottomEased.GetEndValue() + Quaternion.Euler(0f, m_orbitEulerAngleY.GetEndValue(), 0f) * (m_orbitLengths.GetEndValue() + m_orbitLengths.GetEndValue().normalized * (m_zoomParameter.GetEndValue() + m_defaultExtraDistance));
		}

		private Vector3 CalcDesiredOrbitLengths(out float minDistTargetBottomToCam, bool useLowPosition)
		{
			float y = CameraControls.Get().m_desiredRotationEulerAngles.y;
			float fieldOfView = Camera.main.fieldOfView;
			Camera main = Camera.main;
			float fieldOfView2;
			if (useLowPosition)
			{
				fieldOfView2 = m_lowPositionFOV;
			}
			else
			{
				fieldOfView2 = m_defaultFOV;
			}
			main.fieldOfView = fieldOfView2;
			Quaternion rotation = base.transform.rotation;
			base.transform.rotation = CalcDesiredRotation(y, useLowPosition);
			Vector3 position = base.transform.position;
			Quaternion rotation2 = Quaternion.AngleAxis((0f - m_bottomHUDClearance) * Camera.main.fieldOfView / 2f, base.transform.right);
			Vector3 vector = rotation2 * base.transform.forward;
			Ray ray = new Ray(base.transform.position, vector);
			Plane plane = new Plane(Vector3.up, m_target.center);
			Vector3 b = Vector3.zero;
			if (plane.Raycast(ray, out float enter))
			{
				Vector3 point = ray.GetPoint(enter);
				b = m_target.center - point;
			}
			Vector3 b2 = Vector3.zero;
			Vector3[] array = new Vector3[8];
			ref Vector3 reference = ref array[0];
			Vector3 min = m_target.min;
			float x = min.x;
			Vector3 max = m_target.max;
			float y2 = max.y;
			Vector3 min2 = m_target.min;
			reference = new Vector3(x, y2, min2.z) - b;
			ref Vector3 reference2 = ref array[1];
			Vector3 min3 = m_target.min;
			float x2 = min3.x;
			Vector3 max2 = m_target.max;
			float y3 = max2.y;
			Vector3 max3 = m_target.max;
			reference2 = new Vector3(x2, y3, max3.z) - b;
			ref Vector3 reference3 = ref array[2];
			Vector3 max4 = m_target.max;
			float x3 = max4.x;
			Vector3 max5 = m_target.max;
			float y4 = max5.y;
			Vector3 min4 = m_target.min;
			reference3 = new Vector3(x3, y4, min4.z) - b;
			ref Vector3 reference4 = ref array[3];
			Vector3 max6 = m_target.max;
			float x4 = max6.x;
			Vector3 max7 = m_target.max;
			float y5 = max7.y;
			Vector3 max8 = m_target.max;
			reference4 = new Vector3(x4, y5, max8.z) - b;
			ref Vector3 reference5 = ref array[4];
			Vector3 min5 = m_target.min;
			float x5 = min5.x;
			Vector3 min6 = m_target.min;
			float y6 = min6.y;
			Vector3 min7 = m_target.min;
			reference5 = new Vector3(x5, y6, min7.z) - b;
			ref Vector3 reference6 = ref array[5];
			Vector3 min8 = m_target.min;
			float x6 = min8.x;
			Vector3 min9 = m_target.min;
			float y7 = min9.y;
			Vector3 max9 = m_target.max;
			reference6 = new Vector3(x6, y7, max9.z) - b;
			ref Vector3 reference7 = ref array[6];
			Vector3 max10 = m_target.max;
			float x7 = max10.x;
			Vector3 min10 = m_target.min;
			float y8 = min10.y;
			Vector3 min11 = m_target.min;
			reference7 = new Vector3(x7, y8, min11.z) - b;
			ref Vector3 reference8 = ref array[7];
			Vector3 max11 = m_target.max;
			float x8 = max11.x;
			Vector3 min12 = m_target.min;
			float y9 = min12.y;
			Vector3 max12 = m_target.max;
			reference8 = new Vector3(x8, y9, max12.z) - b;
			Vector3[] array2 = array;
			Plane[] array3 = GeometryUtility.CalculateFrustumPlanes(Camera.main);
			for (int i = 0; i < array3.Length; i++)
			{
				Plane plane2 = array3[i];
				if (Vector3.Dot(base.transform.forward, plane2.normal) < -0.9f)
				{
					continue;
				}
				if (Vector3.Dot(plane2.normal, base.transform.up) > 0f)
				{
					plane2.SetNormalAndPosition(rotation2 * plane2.normal, base.transform.position);
					array3[i] = plane2;
				}
				array3[i] = plane2;
			}
			bool flag = true;
			for (int j = 0; j < array3.Length; j++)
			{
				Plane plane3 = array3[j];
				if (Vector3.Dot(base.transform.forward, plane3.normal) < -0.9f)
				{
					continue;
				}
				foreach (Vector3 inPt in array2)
				{
					if (plane3.GetDistanceToPoint(inPt) < 0f)
					{
						flag = false;
						break;
					}
				}
			}
			while (true)
			{
				Vector3 vector2;
				if (flag)
				{
					vector2 = -vector;
				}
				else
				{
					vector2 = vector;
				}
				Vector3 direction = vector2;
				float num = (!flag) ? float.MinValue : float.MaxValue;
				foreach (Vector3 vector3 in array2)
				{
					Ray ray2 = new Ray(vector3, direction);
					for (int m = 0; m < array3.Length; m++)
					{
						Plane plane4 = array3[m];
						if (Vector3.Dot(base.transform.forward, plane4.normal) < -0.9f)
						{
							continue;
						}
						if (!plane4.Raycast(ray2, out float enter2))
						{
							continue;
						}
						Vector3 vector4 = vector3 - ray2.GetPoint(enter2);
						if (flag)
						{
							if (enter2 < num)
							{
								goto IL_0692;
							}
						}
						if (flag)
						{
							continue;
						}
						if (!(enter2 > num))
						{
							continue;
						}
						goto IL_0692;
						IL_0692:
						b2 = vector4;
						num = enter2;
					}
				}
				Vector3 vector5 = base.transform.position + b + b2;
				Vector3 center = m_target.center;
				float x9 = center.x;
				Vector3 min13 = m_target.min;
				float y10 = min13.y;
				Vector3 center2 = m_target.center;
				Vector3 b3 = new Vector3(x9, y10, center2.z);
				Vector3 vector6 = vector5 - b3;
				minDistTargetBottomToCam = vector6.magnitude;
				Vector3 vector7;
				if (minDistTargetBottomToCam > 0f)
				{
					vector7 = vector6 / minDistTargetBottomToCam;
				}
				else
				{
					vector7 = Vector3.up;
				}
				Vector3 a = vector7;
				if (Vector3.Dot(Vector3.up, vector6) > 0f)
				{
					float num2 = vector5.y + a.y * m_defaultExtraDistance - (float)Board.Get().BaselineHeight;
					float num3 = (!useLowPosition) ? m_defaultXRotation : m_lowPositionXRotation;
					if (num2 < m_minAutoHeight)
					{
						vector5 = a * m_minAutoHeight / Mathf.Cos((float)Math.PI / 180f * num3) + b3;
					}
					else if (num2 > m_maxAutoHeight)
					{
						vector5 = a * m_maxAutoHeight / Mathf.Cos((float)Math.PI / 180f * num3) + b3;
					}
				}
				base.transform.position = vector5;
				Vector3 result = CalcCurrentOrbitLengths();
				Camera.main.fieldOfView = fieldOfView;
				base.transform.rotation = rotation;
				base.transform.position = position;
				return result;
			}
		}

		private Vector3 CalcCurrentOrbitLengths()
		{
			Vector3 center = m_target.center;
			float y = center.y;
			Vector3 extents = m_target.extents;
			center.y = y - extents.y;
			Vector3 result = base.transform.position - center;
			result.z = 0f - Mathf.Sqrt(result.x * result.x + result.z * result.z);
			result.x = 0f;
			return result;
		}

		private float GetDesiredXRotation()
		{
			return m_easedDesiredXRotation;
		}

		private float GetDesiredFOV()
		{
			return m_easedDesiredFOV;
		}
	}
}
