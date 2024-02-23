using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ControlpadGameplay : MonoBehaviour
{
	private static ControlpadGameplay s_instance;

	private ControlpadAimingConfig m_aimingConfig;

	private int m_lastCacheFrame = -1;

	private ControllerInputSnapshot m_curFrameInput;

	private ControllerInputSnapshot m_prevFrameInput;

	private bool m_usingControllerInputForTargeting;

	private List<float> m_timeStartedHoldingDownInputs;

	private Vector3 m_lastNonzeroLeftStickWorldDir;

	private Vector3 m_controllerAimDir;

	private Vector3 m_controllerAimPos;

	private Vector3 m_aimingOriginPos;

	public ControllerInputSnapshot CurFrameInput
	{
		get
		{
			CacheInputThisFrame();
			return m_curFrameInput;
		}
		private set
		{
			if (m_curFrameInput == value)
			{
				return;
			}
			while (true)
			{
				m_curFrameInput = value;
				return;
			}
		}
	}

	public ControllerInputSnapshot PrevFrameInput
	{
		get
		{
			CacheInputThisFrame();
			return m_prevFrameInput;
		}
		private set
		{
			if (m_prevFrameInput != value)
			{
				m_prevFrameInput = value;
			}
		}
	}

	public bool UsingControllerInput
	{
		get
		{
			CacheInputThisFrame();
			return m_usingControllerInputForTargeting;
		}
		private set
		{
			if (m_usingControllerInputForTargeting == value)
			{
				return;
			}
			while (true)
			{
				m_usingControllerInputForTargeting = value;
				return;
			}
		}
	}

	public Vector3 LastNonzeroLeftStickWorldDir
	{
		get
		{
			return m_lastNonzeroLeftStickWorldDir;
		}
		private set
		{
			if (!(m_lastNonzeroLeftStickWorldDir != value))
			{
				return;
			}
			while (true)
			{
				m_lastNonzeroLeftStickWorldDir = value;
				if (m_aimingConfig == null)
				{
					while (true)
					{
						ControllerAimDir = m_lastNonzeroLeftStickWorldDir;
						return;
					}
				}
				return;
			}
		}
	}

	public Vector3 LastNonzeroRightStickWorldDir
	{
		get;
		private set;
	}

	public Vector3 LastNonzeroDpadWorldDir
	{
		get;
		private set;
	}

	public Vector3 ControllerAimDir
	{
		get
		{
			return m_controllerAimDir;
		}
		private set
		{
			if (!(m_controllerAimDir != value))
			{
				return;
			}
			while (true)
			{
				m_controllerAimDir = value;
				return;
			}
		}
	}

	public Vector3 ControllerAimPos
	{
		get
		{
			return m_controllerAimPos;
		}
		private set
		{
			if (!(m_controllerAimPos != value))
			{
				return;
			}
			while (true)
			{
				m_controllerAimPos = value;
				return;
			}
		}
	}

	public Vector3 ControllerAimingOriginPos
	{
		get
		{
			return m_aimingOriginPos;
		}
		private set
		{
			if (!(m_aimingOriginPos != value))
			{
				return;
			}
			while (true)
			{
				m_aimingOriginPos = value;
				return;
			}
		}
	}

	public bool ShowDebugGUI
	{
		get;
		set;
	}

	private void Awake()
	{
		s_instance = this;
		PrevFrameInput = new ControllerInputSnapshot();
		CurFrameInput = new ControllerInputSnapshot();
		m_timeStartedHoldingDownInputs = new List<float>(18);
		for (int i = 0; i < 18; i++)
		{
			m_timeStartedHoldingDownInputs.Add(0f);
		}
		m_aimingConfig = new ControlpadAimingConfig();
		m_aimingConfig.SetupRotation(ControlpadInputValue.LeftStickX, ControlpadInputSign.Positive, ControlpadInputValue.LeftStickX, ControlpadInputSign.Negative);
		m_aimingConfig.SetupDepthMovement(ControlpadInputValue.LeftStickY, ControlpadInputSign.Positive, ControlpadInputValue.LeftStickY, ControlpadInputSign.Negative);
		m_aimingConfig.SetupPositionMovement(ControlpadInputValue.LeftStickY, ControlpadInputSign.Positive, ControlpadInputValue.LeftStickY, ControlpadInputSign.Negative, ControlpadInputValue.LeftStickX, ControlpadInputSign.Positive, ControlpadInputValue.LeftStickX, ControlpadInputSign.Negative);
		m_aimingConfig.SetupSpeeds(ControlpadAimingSpeed.DefaultAnalogStickRotation(), ControlpadAimingSpeed.DefaultAnalogStickDepth(), ControlpadAimingSpeed.DefaultAnalogStickTranslation());
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	public static ControlpadGameplay Get()
	{
		return s_instance;
	}

	private void Update()
	{
		CacheInputThisFrame();
	}

	private void CacheInputThisFrame()
	{
		if (m_lastCacheFrame >= Time.frameCount)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		m_lastCacheFrame = Time.frameCount;
		if (!(GameManager.Get() == null))
		{
			if (GameManager.Get().GameplayOverrides != null)
			{
				if (!GameManager.Get().GameplayOverrides.DisableControlPadInput)
				{
					PrevFrameInput.CopySnapshotValuesFrom(CurFrameInput);
					CurFrameInput.CacheInputThisFrame();
					UpdateTimeStartedHoldingDownInputs();
					DetermineUserPreferredInput();
					UpdateAiming();
					UpdateLastSetDirections();
					return;
				}
			}
		}
		UsingControllerInput = false;
		CurFrameInput.ClearAllValues();
	}

	public void UpdateTimeStartedHoldingDownInputs()
	{
		if (m_timeStartedHoldingDownInputs == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					Debug.LogWarning("ControlpadGameplay-- UpdateTimeStartedHoldingDownInputs is being called, but m_timeStartedHoldingDownInputs is null.  (How did that happen...?)");
					return;
				}
			}
		}
		for (int i = 0; i < 18; i++)
		{
			ControlpadInputValue input = (ControlpadInputValue)i;
			if (Mathf.Abs(CurFrameInput.GetValueOfInput(input)) >= 0.9f)
			{
				if (m_timeStartedHoldingDownInputs[i] == 0f)
				{
					m_timeStartedHoldingDownInputs[i] = GameTime.time;
				}
			}
			else if (m_timeStartedHoldingDownInputs[i] != 0f)
			{
				m_timeStartedHoldingDownInputs[i] = 0f;
			}
		}
		while (true)
		{
			switch (1)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public float GetTimeSpentHoldingDownInput(ControlpadInputValue inputType)
	{
		if (inputType == ControlpadInputValue.INVALID)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return 0f;
				}
			}
		}
		float num = m_timeStartedHoldingDownInputs[(int)inputType];
		if (num == 0f)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return 0f;
				}
			}
		}
		float time = GameTime.time;
		return time - num;
	}

	public void DetermineUserPreferredInput()
	{
		int num;
		if (CurFrameInput.LeftStickX == 0f)
		{
			if (CurFrameInput.LeftStickY == 0f && CurFrameInput.RightStickX == 0f)
			{
				if (CurFrameInput.RightStickY == 0f)
				{
					if (CurFrameInput.DpadX == 0f && CurFrameInput.DpadY == 0f)
					{
						num = (CurFrameInput.IsUsingAnyGamepadButton() ? 1 : 0);
						goto IL_00c7;
					}
				}
			}
		}
		num = 1;
		goto IL_00c7;
		IL_012a:
		int num2;
		bool flag = (byte)num2 != 0;
		bool flag2;
		if (UsingControllerInput)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (flag && !flag2)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								UsingControllerInput = false;
								return;
							}
						}
					}
					return;
				}
			}
		}
		if (flag)
		{
			return;
		}
		while (true)
		{
			if (flag2)
			{
				while (true)
				{
					UsingControllerInput = true;
					return;
				}
			}
			return;
		}
		IL_00c7:
		flag2 = ((byte)num != 0);
		if (CurFrameInput.MouseX == PrevFrameInput.MouseX)
		{
			if (CurFrameInput.MouseY == PrevFrameInput.MouseY)
			{
				num2 = (CurFrameInput.IsUsingAnyMouseButton() ? 1 : 0);
				goto IL_012a;
			}
		}
		num2 = 1;
		goto IL_012a;
	}

	public void UpdateLastSetDirections()
	{
		if (CurFrameInput.LeftStickWorldDir.sqrMagnitude > 0f)
		{
			LastNonzeroLeftStickWorldDir = CurFrameInput.LeftStickWorldDir;
		}
		if (CurFrameInput.RightStickWorldDir.sqrMagnitude > 0f)
		{
			LastNonzeroRightStickWorldDir = CurFrameInput.RightStickWorldDir;
		}
		if (!(CurFrameInput.DpadWorldDir.sqrMagnitude > 0f))
		{
			return;
		}
		while (true)
		{
			LastNonzeroDpadWorldDir = CurFrameInput.DpadWorldDir;
			return;
		}
	}

	public void UpdateAiming()
	{
		if (ControllerAimDir.sqrMagnitude == 0f)
		{
			Camera main = Camera.main;
			if (main != null)
			{
				Vector3 forward = main.transform.forward;
				Vector3 controllerAimDir = new Vector3(forward.x, 0f, forward.z);
				controllerAimDir.Normalize();
				ControllerAimDir = controllerAimDir;
			}
		}
		ActorData actorData = null;
		if (GameFlowData.Get() != null)
		{
			actorData = GameFlowData.Get().activeOwnedActorData;
		}
		if (!(actorData != null) || !(Board.Get() != null) || !UsingControllerInput)
		{
			return;
		}
		while (true)
		{
			Ability selectedAbility = actorData.GetAbilityData().GetSelectedAbility();
			int targetSelectionIndex = actorData.GetActorTurnSM().GetTargetSelectionIndex();
			if (selectedAbility != null && targetSelectionIndex >= 0)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
					{
						List<AbilityTarget> abilityTargets = actorData.GetActorTurnSM().GetAbilityTargets();
						switch (selectedAbility.GetControlpadTargetingParadigm(targetSelectionIndex))
						{
						case Ability.TargetingParadigm.Direction:
							while (true)
							{
								switch (6)
								{
								case 0:
									break;
								default:
									UpdateAiming_DirectionTargeter(actorData, selectedAbility, targetSelectionIndex, abilityTargets);
									return;
								}
							}
						case Ability.TargetingParadigm.Position:
							UpdateAiming_PositionTargeter();
							break;
						case Ability.TargetingParadigm.BoardSquare:
							UpdateAiming_PositionTargeter();
							break;
						}
						return;
					}
					}
				}
			}
			if (actorData.GetActorTurnSM().AmDecidingMovement())
			{
				while (true)
				{
					UpdateAiming_PositionTargeter();
					return;
				}
			}
			return;
		}
	}

	private void UpdateAiming_PositionTargeter()
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (activeOwnedActorData != null)
		{
			if (activeOwnedActorData.GetActorTurnSM().IsAbilityOrPingSelectorVisible())
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
		}
		float valueOfInput = CurFrameInput.GetValueOfInput(m_aimingConfig.m_translationUp);
		float timeSpentHoldingDownInput = GetTimeSpentHoldingDownInput(m_aimingConfig.m_translationUp);
		float valueOfInput2 = CurFrameInput.GetValueOfInput(m_aimingConfig.m_translationDown);
		float timeSpentHoldingDownInput2 = GetTimeSpentHoldingDownInput(m_aimingConfig.m_translationDown);
		float valueOfInput3 = CurFrameInput.GetValueOfInput(m_aimingConfig.m_translationRight);
		float timeSpentHoldingDownInput3 = GetTimeSpentHoldingDownInput(m_aimingConfig.m_translationRight);
		float valueOfInput4 = CurFrameInput.GetValueOfInput(m_aimingConfig.m_translationLeft);
		float timeSpentHoldingDownInput4 = GetTimeSpentHoldingDownInput(m_aimingConfig.m_translationLeft);
		float num;
		if (valueOfInput > 0f && m_aimingConfig.m_translationUpSign == ControlpadInputSign.Positive)
		{
			num = m_aimingConfig.m_translationSpeed.GetSpeed(timeSpentHoldingDownInput, valueOfInput);
		}
		else if (valueOfInput < 0f && m_aimingConfig.m_translationUpSign == ControlpadInputSign.Negative)
		{
			num = m_aimingConfig.m_translationSpeed.GetSpeed(timeSpentHoldingDownInput, Mathf.Abs(valueOfInput));
		}
		else
		{
			if (valueOfInput2 > 0f)
			{
				if (m_aimingConfig.m_translationDownSign == ControlpadInputSign.Positive)
				{
					num = 0f - m_aimingConfig.m_translationSpeed.GetSpeed(timeSpentHoldingDownInput2, valueOfInput2);
					goto IL_01fe;
				}
			}
			if (valueOfInput2 < 0f)
			{
				if (m_aimingConfig.m_translationDownSign == ControlpadInputSign.Negative)
				{
					num = 0f - m_aimingConfig.m_translationSpeed.GetSpeed(timeSpentHoldingDownInput2, Mathf.Abs(valueOfInput2));
					goto IL_01fe;
				}
			}
			num = 0f;
		}
		goto IL_01fe;
		IL_0304:
		float num2;
		float num3 = num2 * GameTime.deltaTime;
		float num4 = num * GameTime.deltaTime;
		if (num3 == 0f)
		{
			if (num4 == 0f)
			{
				return;
			}
		}
		Camera main = Camera.main;
		if (!(main != null))
		{
			return;
		}
		while (true)
		{
			Vector3 forward = main.transform.forward;
			Vector3 vector = new Vector3(forward.x, 0f, forward.z);
			vector.Normalize();
			Vector3 a = -Vector3.Cross(vector, Vector3.up);
			a.Normalize();
			Vector3 b = a * num3 + vector * num4;
			Vector3 vector2 = ControllerAimPos + b;
			GameplayData gameplayData = GameplayData.Get();
			if (gameplayData != null)
			{
				vector2.x = Mathf.Clamp(vector2.x, gameplayData.m_minimumPositionX, gameplayData.m_maximumPositionX);
				vector2.z = Mathf.Clamp(vector2.z, gameplayData.m_minimumPositionZ, gameplayData.m_maximumPositionZ);
			}
			ControllerAimPos = vector2;
			if (activeOwnedActorData != null)
			{
				ControllerAimingOriginPos = activeOwnedActorData.GetFreePos();
				Vector3 controllerAimDir = ControllerAimPos - ControllerAimingOriginPos;
				controllerAimDir.y = 0f;
				controllerAimDir.Normalize();
				ControllerAimDir = controllerAimDir;
			}
			CameraManager.Get().SetTargetPosition(vector2, 0.5f);
			return;
		}
		IL_01fe:
		if (valueOfInput3 > 0f)
		{
			if (m_aimingConfig.m_translationRightSign == ControlpadInputSign.Positive)
			{
				num2 = m_aimingConfig.m_translationSpeed.GetSpeed(timeSpentHoldingDownInput3, valueOfInput3);
				goto IL_0304;
			}
		}
		if (valueOfInput3 < 0f && m_aimingConfig.m_translationRightSign == ControlpadInputSign.Negative)
		{
			num2 = m_aimingConfig.m_translationSpeed.GetSpeed(timeSpentHoldingDownInput3, Mathf.Abs(valueOfInput3));
		}
		else
		{
			if (valueOfInput4 > 0f)
			{
				if (m_aimingConfig.m_translationLeftSign == ControlpadInputSign.Positive)
				{
					num2 = 0f - m_aimingConfig.m_translationSpeed.GetSpeed(timeSpentHoldingDownInput4, valueOfInput4);
					goto IL_0304;
				}
			}
			if (valueOfInput4 < 0f)
			{
				if (m_aimingConfig.m_translationLeftSign == ControlpadInputSign.Negative)
				{
					num2 = 0f - m_aimingConfig.m_translationSpeed.GetSpeed(timeSpentHoldingDownInput4, Mathf.Abs(valueOfInput4));
					goto IL_0304;
				}
			}
			num2 = 0f;
		}
		goto IL_0304;
	}

	private void UpdateAiming_DirectionTargeter(ActorData clientActor, Ability abilityBeingTargeted, int currentIndex, List<AbilityTarget> targetsSoFar)
	{
		float valueOfInput = CurFrameInput.GetValueOfInput(m_aimingConfig.m_rotateClockwise);
		float timeSpentHoldingDownInput = GetTimeSpentHoldingDownInput(m_aimingConfig.m_rotateClockwise);
		float valueOfInput2 = CurFrameInput.GetValueOfInput(m_aimingConfig.m_rotateAntiClockwise);
		float timeSpentHoldingDownInput2 = GetTimeSpentHoldingDownInput(m_aimingConfig.m_rotateAntiClockwise);
		float valueOfInput3 = CurFrameInput.GetValueOfInput(m_aimingConfig.m_depthForward);
		float timeSpentHoldingDownInput3 = GetTimeSpentHoldingDownInput(m_aimingConfig.m_depthForward);
		float valueOfInput4 = CurFrameInput.GetValueOfInput(m_aimingConfig.m_depthBackward);
		float timeSpentHoldingDownInput4 = GetTimeSpentHoldingDownInput(m_aimingConfig.m_depthBackward);
		float num;
		if (valueOfInput > 0f && m_aimingConfig.m_rotateClockwiseSign == ControlpadInputSign.Positive)
		{
			num = m_aimingConfig.m_rotationSpeed.GetSpeed(timeSpentHoldingDownInput, valueOfInput);
		}
		else if (valueOfInput < 0f && m_aimingConfig.m_rotateClockwiseSign == ControlpadInputSign.Negative)
		{
			num = m_aimingConfig.m_rotationSpeed.GetSpeed(timeSpentHoldingDownInput, Mathf.Abs(valueOfInput));
		}
		else
		{
			if (valueOfInput2 > 0f)
			{
				if (m_aimingConfig.m_rotateAntiClockwiseSign == ControlpadInputSign.Positive)
				{
					num = 0f - m_aimingConfig.m_rotationSpeed.GetSpeed(timeSpentHoldingDownInput2, valueOfInput2);
					goto IL_01c3;
				}
			}
			if (valueOfInput2 < 0f && m_aimingConfig.m_rotateAntiClockwiseSign == ControlpadInputSign.Negative)
			{
				num = 0f - m_aimingConfig.m_rotationSpeed.GetSpeed(timeSpentHoldingDownInput2, Mathf.Abs(valueOfInput2));
			}
			else
			{
				num = 0f;
			}
		}
		goto IL_01c3;
		IL_01c3:
		float num2;
		if (valueOfInput3 > 0f)
		{
			if (m_aimingConfig.m_depthForwardSign == ControlpadInputSign.Positive)
			{
				num2 = m_aimingConfig.m_depthSpeed.GetSpeed(timeSpentHoldingDownInput3, valueOfInput3);
				goto IL_02cd;
			}
		}
		if (valueOfInput3 < 0f && m_aimingConfig.m_depthForwardSign == ControlpadInputSign.Negative)
		{
			num2 = m_aimingConfig.m_depthSpeed.GetSpeed(timeSpentHoldingDownInput3, Mathf.Abs(valueOfInput3));
		}
		else
		{
			if (valueOfInput4 > 0f)
			{
				if (m_aimingConfig.m_depthBackwardSign == ControlpadInputSign.Positive)
				{
					num2 = 0f - m_aimingConfig.m_depthSpeed.GetSpeed(timeSpentHoldingDownInput4, valueOfInput4);
					goto IL_02cd;
				}
			}
			if (valueOfInput4 < 0f && m_aimingConfig.m_depthBackwardSign == ControlpadInputSign.Negative)
			{
				num2 = 0f - m_aimingConfig.m_depthSpeed.GetSpeed(timeSpentHoldingDownInput4, Mathf.Abs(valueOfInput4));
			}
			else
			{
				num2 = 0f;
			}
		}
		goto IL_02cd;
		IL_02cd:
		float num3 = -1f * num * GameTime.deltaTime;
		float num4 = num2 * GameTime.deltaTime;
		if (abilityBeingTargeted != null)
		{
			if (abilityBeingTargeted.HasAimingOriginOverride(clientActor, currentIndex, targetsSoFar, out Vector3 overridePos))
			{
				ControllerAimingOriginPos = overridePos;
				goto IL_0330;
			}
		}
		ControllerAimingOriginPos = clientActor.GetFreePos();
		goto IL_0330;
		IL_0330:
		float num5 = VectorUtils.HorizontalAngle_Deg(ControllerAimDir);
		float magnitude = (ControllerAimingOriginPos - Board.Get().PlayerFreePos).magnitude;
		float angle = num5 + num3;
		float value = magnitude + num4;
		value = Mathf.Clamp(value, 0.01f, 50f);
		if (abilityBeingTargeted != null && abilityBeingTargeted.HasRestrictedFreeAimDegrees(clientActor, currentIndex, targetsSoFar, out float min, out float max))
		{
			angle = VectorUtils.ClampAngle_Deg(angle, min, max);
		}
		if (abilityBeingTargeted != null)
		{
			if (abilityBeingTargeted.HasRestrictedFreePosDistance(clientActor, currentIndex, targetsSoFar, out float min2, out float max2))
			{
				value = Mathf.Clamp(value, min2, max2);
			}
		}
		Vector3 vector = VectorUtils.AngleDegreesToVector(angle);
		Vector3 controllerAimPos = ControllerAimingOriginPos + vector * value;
		ControllerAimDir = vector;
		ControllerAimPos = controllerAimPos;
	}

	public void OnCameraCenteredOnActor(ActorData cameraActor)
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (cameraActor != activeOwnedActorData)
		{
			ControllerAimDir = (cameraActor.GetFreePos() - activeOwnedActorData.GetFreePos()).normalized;
		}
		if (!(cameraActor != null))
		{
			return;
		}
		while (true)
		{
			ControllerAimPos = cameraActor.GetFreePos();
			return;
		}
	}

	public void OnTurnTick()
	{
		if (GameFlowData.Get() == null)
		{
			return;
		}
		while (true)
		{
			if (GameFlowData.Get().activeOwnedActorData == null)
			{
				while (true)
				{
					switch (2)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			if (activeOwnedActorData.IsDead())
			{
				return;
			}
			while (true)
			{
				if (activeOwnedActorData.CurrentBoardSquare != null && activeOwnedActorData.IsActorVisibleToClient())
				{
					while (true)
					{
						ControllerAimPos = activeOwnedActorData.GetFreePos();
						return;
					}
				}
				return;
			}
		}
	}

	private void OnGUI()
	{
		if (!ShowDebugGUI)
		{
			return;
		}
		while (true)
		{
			Rect screenRect = new Rect(60f, 5f, 800f, 500f);
			GUILayout.Window(632146, screenRect, DrawDebugGUIWindow, "Gamepad Debug Window");
			return;
		}
	}

	private void DrawDebugGUIWindow(int windowId)
	{
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Hide Me", GetDebugGUIButtonStyle(), GUILayout.Width(80f)))
		{
			ShowDebugGUI = false;
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginVertical();
		Vector3 controllerAimDir = ControllerAimDir;
		object arg = controllerAimDir.x;
		Vector3 controllerAimDir2 = ControllerAimDir;
		string text = new StringBuilder().Append("Aim dir: (").Append(arg).Append(", ").Append(controllerAimDir2.z).Append(")").ToString();
		string text2 = new StringBuilder().Append(">>In Degrees: ").Append(VectorUtils.HorizontalAngle_Deg(ControllerAimDir)).ToString();
		Vector3 controllerAimPos = ControllerAimPos;
		object arg2 = controllerAimPos.x;
		Vector3 controllerAimPos2 = ControllerAimPos;
		object arg3 = controllerAimPos2.y;
		Vector3 controllerAimPos3 = ControllerAimPos;
		string text3 = new StringBuilder().Append("Aim pos: (").Append(arg2).Append(", ").Append(arg3).Append(", ").Append(controllerAimPos3.z).Append(")").ToString();
		Vector3 controllerAimingOriginPos = ControllerAimingOriginPos;
		object arg4 = controllerAimingOriginPos.x;
		Vector3 controllerAimingOriginPos2 = ControllerAimingOriginPos;
		object arg5 = controllerAimingOriginPos2.y;
		Vector3 controllerAimingOriginPos3 = ControllerAimingOriginPos;
		string text4 = new StringBuilder().Append("Origin pos: (").Append(arg4).Append(", ").Append(arg5).Append(", ").Append(controllerAimingOriginPos3.z).Append(")").ToString();
		string text5 = new StringBuilder().Append("Left stick: (").Append(CurFrameInput.LeftStickX).Append(", ").Append(CurFrameInput.LeftStickY).Append(")").ToString();
		string text6 = new StringBuilder().Append("Right stick: (").Append(CurFrameInput.RightStickX).Append(", ").Append(CurFrameInput.RightStickY).Append(")").ToString();
		string text7 = new StringBuilder().Append("D-pad: (").Append(CurFrameInput.DpadX).Append(", ").Append(CurFrameInput.DpadY).Append(")").ToString();
		string text8 = new StringBuilder().Append("A: ").Append(CurFrameInput.Button_A.GetDebugString()).ToString();
		string text9 = new StringBuilder().Append("B: ").Append(CurFrameInput.Button_B.GetDebugString()).ToString();
		string text10 = new StringBuilder().Append("X: ").Append(CurFrameInput.Button_X.GetDebugString()).ToString();
		string text11 = new StringBuilder().Append("Y: ").Append(CurFrameInput.Button_Y.GetDebugString()).ToString();
		string text12 = new StringBuilder().Append("Start: ").Append(CurFrameInput.Button_start.GetDebugString()).ToString();
		string text13 = new StringBuilder().Append("Back: ").Append(CurFrameInput.Button_back.GetDebugString()).ToString();
		string text14 = new StringBuilder().Append("Left shoulder: ").Append(CurFrameInput.Button_leftShoulder.GetDebugString()).ToString();
		string text15 = new StringBuilder().Append("Right shoulder: ").Append(CurFrameInput.Button_rightShoulder.GetDebugString()).ToString();
		string text16 = new StringBuilder().Append("Left trigger: ").Append(CurFrameInput.LeftTrigger).ToString();
		string text17 = new StringBuilder().Append("Right trigger: ").Append(CurFrameInput.RightTrigger).ToString();
		string text18 = new StringBuilder().Append("Left stick in: ").Append(CurFrameInput.Button_leftStickIn.GetDebugString()).ToString();
		string text19 = new StringBuilder().Append("Right stick in: ").Append(CurFrameInput.Button_rightStickIn.GetDebugString()).ToString();
		GUILayout.Label(text);
		GUILayout.Label(text2);
		GUILayout.Label(text3);
		GUILayout.Label(text4);
		GUILayout.Label(text5);
		GUILayout.Label(text6);
		GUILayout.Label(text7);
		GUILayout.Label(text8);
		GUILayout.Label(text9);
		GUILayout.Label(text10);
		GUILayout.Label(text11);
		GUILayout.Label(text12);
		GUILayout.Label(text13);
		GUILayout.Label(text14);
		GUILayout.Label(text15);
		GUILayout.Label(text16);
		GUILayout.Label(text17);
		GUILayout.Label(text18);
		GUILayout.Label(text19);
		GUILayout.EndHorizontal();
	}

	private GUIStyle GetDebugGUIButtonStyle()
	{
		GUIStyle gUIStyle = new GUIStyle(GUI.skin.button);
		gUIStyle.alignment = TextAnchor.MiddleLeft;
		gUIStyle.richText = true;
		gUIStyle.fontSize = 15;
		return gUIStyle;
	}

	public bool GetButton(ControlpadInputValue controllerCode)
	{
		return CurFrameInput.GetValueOfInput(controllerCode) == 1f;
	}

	public float GetAxisValue(ControlpadInputValue controllerCode)
	{
		return CurFrameInput.GetValueOfInput(controllerCode);
	}

	public bool GetButtonDown(ControlpadInputValue controllerCode)
	{
		switch (controllerCode)
		{
		case ControlpadInputValue.Button_A:
			return CurFrameInput.Button_A.Down;
		case ControlpadInputValue.Button_B:
			return CurFrameInput.Button_B.Down;
		case ControlpadInputValue.Button_X:
			return CurFrameInput.Button_X.Down;
		case ControlpadInputValue.Button_Y:
			return CurFrameInput.Button_Y.Down;
		case ControlpadInputValue.Button_leftShoulder:
			return CurFrameInput.Button_leftShoulder.Down;
		case ControlpadInputValue.Button_rightShoulder:
			return CurFrameInput.Button_rightShoulder.Down;
		case ControlpadInputValue.Button_start:
			return CurFrameInput.Button_start.Down;
		case ControlpadInputValue.Button_back:
			return CurFrameInput.Button_back.Down;
		case ControlpadInputValue.Button_leftStickIn:
			return CurFrameInput.Button_leftStickIn.Down;
		case ControlpadInputValue.Button_rightStickIn:
			return CurFrameInput.Button_rightStickIn.Down;
		default:
			return false;
		}
	}

	public bool GetButtonUp(ControlpadInputValue controllerCode)
	{
		switch (controllerCode)
		{
		case ControlpadInputValue.Button_A:
			return CurFrameInput.Button_A.Up;
		case ControlpadInputValue.Button_B:
			return CurFrameInput.Button_B.Up;
		case ControlpadInputValue.Button_X:
			return CurFrameInput.Button_X.Up;
		case ControlpadInputValue.Button_Y:
			return CurFrameInput.Button_Y.Up;
		case ControlpadInputValue.Button_leftShoulder:
			return CurFrameInput.Button_leftShoulder.Up;
		case ControlpadInputValue.Button_rightShoulder:
			return CurFrameInput.Button_rightShoulder.Up;
		case ControlpadInputValue.Button_start:
			return CurFrameInput.Button_start.Up;
		case ControlpadInputValue.Button_back:
			return CurFrameInput.Button_back.Up;
		case ControlpadInputValue.Button_leftStickIn:
			return CurFrameInput.Button_leftStickIn.Up;
		case ControlpadInputValue.Button_rightStickIn:
			return CurFrameInput.Button_rightStickIn.Up;
		default:
			return false;
		}
	}
}
