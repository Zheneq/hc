using System;
using System.Collections.Generic;
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

	private void Awake()
	{
		ControlpadGameplay.s_instance = this;
		this.PrevFrameInput = new ControllerInputSnapshot();
		this.CurFrameInput = new ControllerInputSnapshot();
		this.m_timeStartedHoldingDownInputs = new List<float>(0x12);
		for (int i = 0; i < 0x12; i++)
		{
			this.m_timeStartedHoldingDownInputs.Add(0f);
		}
		this.m_aimingConfig = new ControlpadAimingConfig();
		this.m_aimingConfig.SetupRotation(ControlpadInputValue.LeftStickX, ControlpadInputSign.Positive, ControlpadInputValue.LeftStickX, ControlpadInputSign.Negative);
		this.m_aimingConfig.SetupDepthMovement(ControlpadInputValue.LeftStickY, ControlpadInputSign.Positive, ControlpadInputValue.LeftStickY, ControlpadInputSign.Negative);
		this.m_aimingConfig.SetupPositionMovement(ControlpadInputValue.LeftStickY, ControlpadInputSign.Positive, ControlpadInputValue.LeftStickY, ControlpadInputSign.Negative, ControlpadInputValue.LeftStickX, ControlpadInputSign.Positive, ControlpadInputValue.LeftStickX, ControlpadInputSign.Negative);
		this.m_aimingConfig.SetupSpeeds(ControlpadAimingSpeed.DefaultAnalogStickRotation(), ControlpadAimingSpeed.DefaultAnalogStickDepth(), ControlpadAimingSpeed.DefaultAnalogStickTranslation());
	}

	private void OnDestroy()
	{
		ControlpadGameplay.s_instance = null;
	}

	public static ControlpadGameplay Get()
	{
		return ControlpadGameplay.s_instance;
	}

	private void Update()
	{
		this.CacheInputThisFrame();
	}

	private void CacheInputThisFrame()
	{
		if (this.m_lastCacheFrame >= Time.frameCount)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlpadGameplay.CacheInputThisFrame()).MethodHandle;
			}
			return;
		}
		this.m_lastCacheFrame = Time.frameCount;
		if (!(GameManager.Get() == null))
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
			if (GameManager.Get().GameplayOverrides != null)
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
				if (!GameManager.Get().GameplayOverrides.DisableControlPadInput)
				{
					this.PrevFrameInput.CopySnapshotValuesFrom(this.CurFrameInput);
					this.CurFrameInput.CacheInputThisFrame();
					this.UpdateTimeStartedHoldingDownInputs();
					this.DetermineUserPreferredInput();
					this.UpdateAiming();
					this.UpdateLastSetDirections();
					return;
				}
			}
		}
		this.UsingControllerInput = false;
		this.CurFrameInput.ClearAllValues();
	}

	public void UpdateTimeStartedHoldingDownInputs()
	{
		if (this.m_timeStartedHoldingDownInputs == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlpadGameplay.UpdateTimeStartedHoldingDownInputs()).MethodHandle;
			}
			Debug.LogWarning("ControlpadGameplay-- UpdateTimeStartedHoldingDownInputs is being called, but m_timeStartedHoldingDownInputs is null.  (How did that happen...?)");
			return;
		}
		for (int i = 0; i < 0x12; i++)
		{
			ControlpadInputValue input = (ControlpadInputValue)i;
			if (Mathf.Abs(this.CurFrameInput.GetValueOfInput(input)) >= 0.9f)
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
				if (this.m_timeStartedHoldingDownInputs[i] == 0f)
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
					this.m_timeStartedHoldingDownInputs[i] = GameTime.time;
				}
			}
			else if (this.m_timeStartedHoldingDownInputs[i] != 0f)
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
				this.m_timeStartedHoldingDownInputs[i] = 0f;
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
	}

	public float GetTimeSpentHoldingDownInput(ControlpadInputValue inputType)
	{
		if (inputType == ControlpadInputValue.INVALID)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlpadGameplay.GetTimeSpentHoldingDownInput(ControlpadInputValue)).MethodHandle;
			}
			return 0f;
		}
		float num = this.m_timeStartedHoldingDownInputs[(int)inputType];
		if (num == 0f)
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
			return 0f;
		}
		float time = GameTime.time;
		return time - num;
	}

	public void DetermineUserPreferredInput()
	{
		bool flag;
		if (this.CurFrameInput.LeftStickX == 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlpadGameplay.DetermineUserPreferredInput()).MethodHandle;
			}
			if (this.CurFrameInput.LeftStickY == 0f && this.CurFrameInput.RightStickX == 0f)
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
				if (this.CurFrameInput.RightStickY == 0f)
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
					if (this.CurFrameInput.DpadX == 0f && this.CurFrameInput.DpadY == 0f)
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
						flag = this.CurFrameInput.IsUsingAnyGamepadButton();
						goto IL_C7;
					}
				}
			}
		}
		flag = true;
		IL_C7:
		bool flag2 = flag;
		bool flag3;
		if (this.CurFrameInput.MouseX == this.PrevFrameInput.MouseX)
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
			if (this.CurFrameInput.MouseY == this.PrevFrameInput.MouseY)
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
				flag3 = this.CurFrameInput.IsUsingAnyMouseButton();
				goto IL_12A;
			}
		}
		flag3 = true;
		IL_12A:
		bool flag4 = flag3;
		if (this.UsingControllerInput)
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
			if (flag4 && !flag2)
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
				this.UsingControllerInput = false;
			}
		}
		else if (!flag4)
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
			if (flag2)
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
				this.UsingControllerInput = true;
			}
		}
	}

	public void UpdateLastSetDirections()
	{
		if (this.CurFrameInput.LeftStickWorldDir.sqrMagnitude > 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlpadGameplay.UpdateLastSetDirections()).MethodHandle;
			}
			this.LastNonzeroLeftStickWorldDir = this.CurFrameInput.LeftStickWorldDir;
		}
		if (this.CurFrameInput.RightStickWorldDir.sqrMagnitude > 0f)
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
			this.LastNonzeroRightStickWorldDir = this.CurFrameInput.RightStickWorldDir;
		}
		if (this.CurFrameInput.DpadWorldDir.sqrMagnitude > 0f)
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
			this.LastNonzeroDpadWorldDir = this.CurFrameInput.DpadWorldDir;
		}
	}

	public void UpdateAiming()
	{
		if (this.ControllerAimDir.sqrMagnitude == 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlpadGameplay.UpdateAiming()).MethodHandle;
			}
			Camera main = Camera.main;
			if (main != null)
			{
				Vector3 forward = main.transform.forward;
				Vector3 controllerAimDir = new Vector3(forward.x, 0f, forward.z);
				controllerAimDir.Normalize();
				this.ControllerAimDir = controllerAimDir;
			}
		}
		ActorData actorData = null;
		if (GameFlowData.Get() != null)
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
			actorData = GameFlowData.Get().activeOwnedActorData;
		}
		if (actorData != null && Board.Get() != null && this.UsingControllerInput)
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
			Ability selectedAbility = actorData.GetAbilityData().GetSelectedAbility();
			int targetSelectionIndex = actorData.GetActorTurnSM().GetTargetSelectionIndex();
			if (selectedAbility != null && targetSelectionIndex >= 0)
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
				List<AbilityTarget> abilityTargets = actorData.GetActorTurnSM().GetAbilityTargets();
				Ability.TargetingParadigm controlpadTargetingParadigm = selectedAbility.GetControlpadTargetingParadigm(targetSelectionIndex);
				if (controlpadTargetingParadigm == Ability.TargetingParadigm.Direction)
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
					this.UpdateAiming_DirectionTargeter(actorData, selectedAbility, targetSelectionIndex, abilityTargets);
				}
				else if (controlpadTargetingParadigm == Ability.TargetingParadigm.Position)
				{
					this.UpdateAiming_PositionTargeter();
				}
				else if (controlpadTargetingParadigm == Ability.TargetingParadigm.BoardSquare)
				{
					this.UpdateAiming_PositionTargeter();
				}
			}
			else if (actorData.GetActorTurnSM().AmDecidingMovement())
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
				this.UpdateAiming_PositionTargeter();
			}
		}
	}

	private void UpdateAiming_PositionTargeter()
	{
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlpadGameplay.UpdateAiming_PositionTargeter()).MethodHandle;
			}
			if (activeOwnedActorData.GetActorTurnSM().IsAbilityOrPingSelectorVisible())
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
				return;
			}
		}
		float valueOfInput = this.CurFrameInput.GetValueOfInput(this.m_aimingConfig.m_translationUp);
		float timeSpentHoldingDownInput = this.GetTimeSpentHoldingDownInput(this.m_aimingConfig.m_translationUp);
		float valueOfInput2 = this.CurFrameInput.GetValueOfInput(this.m_aimingConfig.m_translationDown);
		float timeSpentHoldingDownInput2 = this.GetTimeSpentHoldingDownInput(this.m_aimingConfig.m_translationDown);
		float valueOfInput3 = this.CurFrameInput.GetValueOfInput(this.m_aimingConfig.m_translationRight);
		float timeSpentHoldingDownInput3 = this.GetTimeSpentHoldingDownInput(this.m_aimingConfig.m_translationRight);
		float valueOfInput4 = this.CurFrameInput.GetValueOfInput(this.m_aimingConfig.m_translationLeft);
		float timeSpentHoldingDownInput4 = this.GetTimeSpentHoldingDownInput(this.m_aimingConfig.m_translationLeft);
		float num;
		if (valueOfInput > 0f && this.m_aimingConfig.m_translationUpSign == ControlpadInputSign.Positive)
		{
			num = this.m_aimingConfig.m_translationSpeed.GetSpeed(timeSpentHoldingDownInput, valueOfInput);
		}
		else if (valueOfInput < 0f && this.m_aimingConfig.m_translationUpSign == ControlpadInputSign.Negative)
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
			num = this.m_aimingConfig.m_translationSpeed.GetSpeed(timeSpentHoldingDownInput, Mathf.Abs(valueOfInput));
		}
		else
		{
			if (valueOfInput2 > 0f)
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
				if (this.m_aimingConfig.m_translationDownSign == ControlpadInputSign.Positive)
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
					num = -this.m_aimingConfig.m_translationSpeed.GetSpeed(timeSpentHoldingDownInput2, valueOfInput2);
					goto IL_1FE;
				}
			}
			if (valueOfInput2 < 0f)
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
				if (this.m_aimingConfig.m_translationDownSign == ControlpadInputSign.Negative)
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
					num = -this.m_aimingConfig.m_translationSpeed.GetSpeed(timeSpentHoldingDownInput2, Mathf.Abs(valueOfInput2));
					goto IL_1FE;
				}
			}
			num = 0f;
		}
		IL_1FE:
		float num2;
		if (valueOfInput3 > 0f)
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
			if (this.m_aimingConfig.m_translationRightSign == ControlpadInputSign.Positive)
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
				num2 = this.m_aimingConfig.m_translationSpeed.GetSpeed(timeSpentHoldingDownInput3, valueOfInput3);
				goto IL_304;
			}
		}
		if (valueOfInput3 < 0f && this.m_aimingConfig.m_translationRightSign == ControlpadInputSign.Negative)
		{
			num2 = this.m_aimingConfig.m_translationSpeed.GetSpeed(timeSpentHoldingDownInput3, Mathf.Abs(valueOfInput3));
		}
		else
		{
			if (valueOfInput4 > 0f)
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
				if (this.m_aimingConfig.m_translationLeftSign == ControlpadInputSign.Positive)
				{
					num2 = -this.m_aimingConfig.m_translationSpeed.GetSpeed(timeSpentHoldingDownInput4, valueOfInput4);
					goto IL_304;
				}
			}
			if (valueOfInput4 < 0f)
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
				if (this.m_aimingConfig.m_translationLeftSign == ControlpadInputSign.Negative)
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
					num2 = -this.m_aimingConfig.m_translationSpeed.GetSpeed(timeSpentHoldingDownInput4, Mathf.Abs(valueOfInput4));
					goto IL_304;
				}
			}
			num2 = 0f;
		}
		IL_304:
		float num3 = num2 * GameTime.deltaTime;
		float num4 = num * GameTime.deltaTime;
		if (num3 == 0f)
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
			if (num4 == 0f)
			{
				return;
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
		}
		Camera main = Camera.main;
		if (main != null)
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
			Vector3 forward = main.transform.forward;
			Vector3 vector = new Vector3(forward.x, 0f, forward.z);
			vector.Normalize();
			Vector3 a = -Vector3.Cross(vector, Vector3.up);
			a.Normalize();
			Vector3 b = a * num3 + vector * num4;
			Vector3 vector2 = this.ControllerAimPos + b;
			GameplayData gameplayData = GameplayData.Get();
			if (gameplayData != null)
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
				vector2.x = Mathf.Clamp(vector2.x, gameplayData.m_minimumPositionX, gameplayData.m_maximumPositionX);
				vector2.z = Mathf.Clamp(vector2.z, gameplayData.m_minimumPositionZ, gameplayData.m_maximumPositionZ);
			}
			this.ControllerAimPos = vector2;
			if (activeOwnedActorData != null)
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
				this.ControllerAimingOriginPos = activeOwnedActorData.GetTravelBoardSquareWorldPosition();
				Vector3 controllerAimDir = this.ControllerAimPos - this.ControllerAimingOriginPos;
				controllerAimDir.y = 0f;
				controllerAimDir.Normalize();
				this.ControllerAimDir = controllerAimDir;
			}
			CameraManager.Get().SetTargetPosition(vector2, 0.5f);
		}
	}

	private void UpdateAiming_DirectionTargeter(ActorData clientActor, Ability abilityBeingTargeted, int currentIndex, List<AbilityTarget> targetsSoFar)
	{
		float valueOfInput = this.CurFrameInput.GetValueOfInput(this.m_aimingConfig.m_rotateClockwise);
		float timeSpentHoldingDownInput = this.GetTimeSpentHoldingDownInput(this.m_aimingConfig.m_rotateClockwise);
		float valueOfInput2 = this.CurFrameInput.GetValueOfInput(this.m_aimingConfig.m_rotateAntiClockwise);
		float timeSpentHoldingDownInput2 = this.GetTimeSpentHoldingDownInput(this.m_aimingConfig.m_rotateAntiClockwise);
		float valueOfInput3 = this.CurFrameInput.GetValueOfInput(this.m_aimingConfig.m_depthForward);
		float timeSpentHoldingDownInput3 = this.GetTimeSpentHoldingDownInput(this.m_aimingConfig.m_depthForward);
		float valueOfInput4 = this.CurFrameInput.GetValueOfInput(this.m_aimingConfig.m_depthBackward);
		float timeSpentHoldingDownInput4 = this.GetTimeSpentHoldingDownInput(this.m_aimingConfig.m_depthBackward);
		float num;
		if (valueOfInput > 0f && this.m_aimingConfig.m_rotateClockwiseSign == ControlpadInputSign.Positive)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlpadGameplay.UpdateAiming_DirectionTargeter(ActorData, Ability, int, List<AbilityTarget>)).MethodHandle;
			}
			num = this.m_aimingConfig.m_rotationSpeed.GetSpeed(timeSpentHoldingDownInput, valueOfInput);
		}
		else if (valueOfInput < 0f && this.m_aimingConfig.m_rotateClockwiseSign == ControlpadInputSign.Negative)
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
			num = this.m_aimingConfig.m_rotationSpeed.GetSpeed(timeSpentHoldingDownInput, Mathf.Abs(valueOfInput));
		}
		else
		{
			if (valueOfInput2 > 0f)
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
				if (this.m_aimingConfig.m_rotateAntiClockwiseSign == ControlpadInputSign.Positive)
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
					num = -this.m_aimingConfig.m_rotationSpeed.GetSpeed(timeSpentHoldingDownInput2, valueOfInput2);
					goto IL_1C3;
				}
			}
			if (valueOfInput2 < 0f && this.m_aimingConfig.m_rotateAntiClockwiseSign == ControlpadInputSign.Negative)
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
				num = -this.m_aimingConfig.m_rotationSpeed.GetSpeed(timeSpentHoldingDownInput2, Mathf.Abs(valueOfInput2));
			}
			else
			{
				num = 0f;
			}
		}
		IL_1C3:
		float num2;
		if (valueOfInput3 > 0f)
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
			if (this.m_aimingConfig.m_depthForwardSign == ControlpadInputSign.Positive)
			{
				num2 = this.m_aimingConfig.m_depthSpeed.GetSpeed(timeSpentHoldingDownInput3, valueOfInput3);
				goto IL_2CD;
			}
		}
		if (valueOfInput3 < 0f && this.m_aimingConfig.m_depthForwardSign == ControlpadInputSign.Negative)
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
			num2 = this.m_aimingConfig.m_depthSpeed.GetSpeed(timeSpentHoldingDownInput3, Mathf.Abs(valueOfInput3));
		}
		else
		{
			if (valueOfInput4 > 0f)
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
				if (this.m_aimingConfig.m_depthBackwardSign == ControlpadInputSign.Positive)
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
					num2 = -this.m_aimingConfig.m_depthSpeed.GetSpeed(timeSpentHoldingDownInput4, valueOfInput4);
					goto IL_2CD;
				}
			}
			if (valueOfInput4 < 0f && this.m_aimingConfig.m_depthBackwardSign == ControlpadInputSign.Negative)
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
				num2 = -this.m_aimingConfig.m_depthSpeed.GetSpeed(timeSpentHoldingDownInput4, Mathf.Abs(valueOfInput4));
			}
			else
			{
				num2 = 0f;
			}
		}
		IL_2CD:
		float num3 = -1f * num * GameTime.deltaTime;
		float num4 = num2 * GameTime.deltaTime;
		if (abilityBeingTargeted != null)
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
			Vector3 controllerAimingOriginPos;
			if (abilityBeingTargeted.HasAimingOriginOverride(clientActor, currentIndex, targetsSoFar, out controllerAimingOriginPos))
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
				this.ControllerAimingOriginPos = controllerAimingOriginPos;
				goto IL_330;
			}
		}
		this.ControllerAimingOriginPos = clientActor.GetTravelBoardSquareWorldPosition();
		IL_330:
		float num5 = VectorUtils.HorizontalAngle_Deg(this.ControllerAimDir);
		float magnitude = (this.ControllerAimingOriginPos - Board.Get().PlayerFreePos).magnitude;
		float angle = num5 + num3;
		float num6 = magnitude + num4;
		num6 = Mathf.Clamp(num6, 0.01f, 50f);
		float min;
		float max;
		if (abilityBeingTargeted != null && abilityBeingTargeted.HasRestrictedFreeAimDegrees(clientActor, currentIndex, targetsSoFar, out min, out max))
		{
			angle = VectorUtils.ClampAngle_Deg(angle, min, max);
		}
		if (abilityBeingTargeted != null)
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
			float min2;
			float max2;
			if (abilityBeingTargeted.HasRestrictedFreePosDistance(clientActor, currentIndex, targetsSoFar, out min2, out max2))
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
				num6 = Mathf.Clamp(num6, min2, max2);
			}
		}
		Vector3 vector = VectorUtils.AngleDegreesToVector(angle);
		Vector3 controllerAimPos = this.ControllerAimingOriginPos + vector * num6;
		this.ControllerAimDir = vector;
		this.ControllerAimPos = controllerAimPos;
	}

	public void OnCameraCenteredOnActor(ActorData cameraActor)
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (cameraActor != activeOwnedActorData)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlpadGameplay.OnCameraCenteredOnActor(ActorData)).MethodHandle;
			}
			this.ControllerAimDir = (cameraActor.GetTravelBoardSquareWorldPosition() - activeOwnedActorData.GetTravelBoardSquareWorldPosition()).normalized;
		}
		if (cameraActor != null)
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
			this.ControllerAimPos = cameraActor.GetTravelBoardSquareWorldPosition();
		}
	}

	public void OnTurnTick()
	{
		if (!(GameFlowData.Get() == null))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlpadGameplay.OnTurnTick()).MethodHandle;
			}
			if (!(GameFlowData.Get().activeOwnedActorData == null))
			{
				ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
				if (!activeOwnedActorData.IsDead())
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
					if (activeOwnedActorData.CurrentBoardSquare != null && activeOwnedActorData.IsVisibleToClient())
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
						this.ControllerAimPos = activeOwnedActorData.GetTravelBoardSquareWorldPosition();
					}
				}
				return;
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

	public ControllerInputSnapshot CurFrameInput
	{
		get
		{
			this.CacheInputThisFrame();
			return this.m_curFrameInput;
		}
		private set
		{
			if (this.m_curFrameInput != value)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ControlpadGameplay.set_CurFrameInput(ControllerInputSnapshot)).MethodHandle;
				}
				this.m_curFrameInput = value;
			}
		}
	}

	public ControllerInputSnapshot PrevFrameInput
	{
		get
		{
			this.CacheInputThisFrame();
			return this.m_prevFrameInput;
		}
		private set
		{
			if (this.m_prevFrameInput != value)
			{
				this.m_prevFrameInput = value;
			}
		}
	}

	public bool UsingControllerInput
	{
		get
		{
			this.CacheInputThisFrame();
			return this.m_usingControllerInputForTargeting;
		}
		private set
		{
			if (this.m_usingControllerInputForTargeting != value)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ControlpadGameplay.set_UsingControllerInput(bool)).MethodHandle;
				}
				this.m_usingControllerInputForTargeting = value;
			}
		}
	}

	public Vector3 LastNonzeroLeftStickWorldDir
	{
		get
		{
			return this.m_lastNonzeroLeftStickWorldDir;
		}
		private set
		{
			if (this.m_lastNonzeroLeftStickWorldDir != value)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ControlpadGameplay.set_LastNonzeroLeftStickWorldDir(Vector3)).MethodHandle;
				}
				this.m_lastNonzeroLeftStickWorldDir = value;
				if (this.m_aimingConfig == null)
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
					this.ControllerAimDir = this.m_lastNonzeroLeftStickWorldDir;
				}
			}
		}
	}

	public Vector3 LastNonzeroRightStickWorldDir { get; private set; }

	public Vector3 LastNonzeroDpadWorldDir { get; private set; }

	public Vector3 ControllerAimDir
	{
		get
		{
			return this.m_controllerAimDir;
		}
		private set
		{
			if (this.m_controllerAimDir != value)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ControlpadGameplay.set_ControllerAimDir(Vector3)).MethodHandle;
				}
				this.m_controllerAimDir = value;
			}
		}
	}

	public Vector3 ControllerAimPos
	{
		get
		{
			return this.m_controllerAimPos;
		}
		private set
		{
			if (this.m_controllerAimPos != value)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ControlpadGameplay.set_ControllerAimPos(Vector3)).MethodHandle;
				}
				this.m_controllerAimPos = value;
			}
		}
	}

	public Vector3 ControllerAimingOriginPos
	{
		get
		{
			return this.m_aimingOriginPos;
		}
		private set
		{
			if (this.m_aimingOriginPos != value)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ControlpadGameplay.set_ControllerAimingOriginPos(Vector3)).MethodHandle;
				}
				this.m_aimingOriginPos = value;
			}
		}
	}

	public bool ShowDebugGUI { get; set; }

	private void OnGUI()
	{
		if (this.ShowDebugGUI)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlpadGameplay.OnGUI()).MethodHandle;
			}
			Rect screenRect = new Rect(60f, 5f, 800f, 500f);
			GUILayout.Window(0x9A552, screenRect, new GUI.WindowFunction(this.DrawDebugGUIWindow), "Gamepad Debug Window", new GUILayoutOption[0]);
		}
	}

	private void DrawDebugGUIWindow(int windowId)
	{
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Hide Me", this.GetDebugGUIButtonStyle(), new GUILayoutOption[]
		{
			GUILayout.Width(80f)
		}))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlpadGameplay.DrawDebugGUIWindow(int)).MethodHandle;
			}
			this.ShowDebugGUI = false;
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		string text = string.Format("Aim dir: ({0}, {1})", this.ControllerAimDir.x, this.ControllerAimDir.z);
		string text2 = string.Format(">>In Degrees: {0}", VectorUtils.HorizontalAngle_Deg(this.ControllerAimDir));
		string text3 = string.Format("Aim pos: ({0}, {1}, {2})", this.ControllerAimPos.x, this.ControllerAimPos.y, this.ControllerAimPos.z);
		string text4 = string.Format("Origin pos: ({0}, {1}, {2})", this.ControllerAimingOriginPos.x, this.ControllerAimingOriginPos.y, this.ControllerAimingOriginPos.z);
		string text5 = string.Format("Left stick: ({0}, {1})", this.CurFrameInput.LeftStickX, this.CurFrameInput.LeftStickY);
		string text6 = string.Format("Right stick: ({0}, {1})", this.CurFrameInput.RightStickX, this.CurFrameInput.RightStickY);
		string text7 = string.Format("D-pad: ({0}, {1})", this.CurFrameInput.DpadX, this.CurFrameInput.DpadY);
		string text8 = string.Format("A: {0}", this.CurFrameInput.Button_A.GetDebugString());
		string text9 = string.Format("B: {0}", this.CurFrameInput.Button_B.GetDebugString());
		string text10 = string.Format("X: {0}", this.CurFrameInput.Button_X.GetDebugString());
		string text11 = string.Format("Y: {0}", this.CurFrameInput.Button_Y.GetDebugString());
		string text12 = string.Format("Start: {0}", this.CurFrameInput.Button_start.GetDebugString());
		string text13 = string.Format("Back: {0}", this.CurFrameInput.Button_back.GetDebugString());
		string text14 = string.Format("Left shoulder: {0}", this.CurFrameInput.Button_leftShoulder.GetDebugString());
		string text15 = string.Format("Right shoulder: {0}", this.CurFrameInput.Button_rightShoulder.GetDebugString());
		string text16 = string.Format("Left trigger: {0}", this.CurFrameInput.LeftTrigger);
		string text17 = string.Format("Right trigger: {0}", this.CurFrameInput.RightTrigger);
		string text18 = string.Format("Left stick in: {0}", this.CurFrameInput.Button_leftStickIn.GetDebugString());
		string text19 = string.Format("Right stick in: {0}", this.CurFrameInput.Button_rightStickIn.GetDebugString());
		GUILayout.Label(text, new GUILayoutOption[0]);
		GUILayout.Label(text2, new GUILayoutOption[0]);
		GUILayout.Label(text3, new GUILayoutOption[0]);
		GUILayout.Label(text4, new GUILayoutOption[0]);
		GUILayout.Label(text5, new GUILayoutOption[0]);
		GUILayout.Label(text6, new GUILayoutOption[0]);
		GUILayout.Label(text7, new GUILayoutOption[0]);
		GUILayout.Label(text8, new GUILayoutOption[0]);
		GUILayout.Label(text9, new GUILayoutOption[0]);
		GUILayout.Label(text10, new GUILayoutOption[0]);
		GUILayout.Label(text11, new GUILayoutOption[0]);
		GUILayout.Label(text12, new GUILayoutOption[0]);
		GUILayout.Label(text13, new GUILayoutOption[0]);
		GUILayout.Label(text14, new GUILayoutOption[0]);
		GUILayout.Label(text15, new GUILayoutOption[0]);
		GUILayout.Label(text16, new GUILayoutOption[0]);
		GUILayout.Label(text17, new GUILayoutOption[0]);
		GUILayout.Label(text18, new GUILayoutOption[0]);
		GUILayout.Label(text19, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
	}

	private GUIStyle GetDebugGUIButtonStyle()
	{
		return new GUIStyle(GUI.skin.button)
		{
			alignment = TextAnchor.MiddleLeft,
			richText = true,
			fontSize = 0xF
		};
	}

	public bool GetButton(ControlpadInputValue controllerCode)
	{
		return this.CurFrameInput.GetValueOfInput(controllerCode) == 1f;
	}

	public float GetAxisValue(ControlpadInputValue controllerCode)
	{
		return this.CurFrameInput.GetValueOfInput(controllerCode);
	}

	public bool GetButtonDown(ControlpadInputValue controllerCode)
	{
		switch (controllerCode)
		{
		case ControlpadInputValue.Button_A:
			return this.CurFrameInput.Button_A.Down;
		case ControlpadInputValue.Button_B:
			return this.CurFrameInput.Button_B.Down;
		case ControlpadInputValue.Button_X:
			return this.CurFrameInput.Button_X.Down;
		case ControlpadInputValue.Button_Y:
			return this.CurFrameInput.Button_Y.Down;
		case ControlpadInputValue.Button_leftShoulder:
			return this.CurFrameInput.Button_leftShoulder.Down;
		case ControlpadInputValue.Button_rightShoulder:
			return this.CurFrameInput.Button_rightShoulder.Down;
		case ControlpadInputValue.Button_start:
			return this.CurFrameInput.Button_start.Down;
		case ControlpadInputValue.Button_back:
			return this.CurFrameInput.Button_back.Down;
		case ControlpadInputValue.Button_leftStickIn:
			return this.CurFrameInput.Button_leftStickIn.Down;
		case ControlpadInputValue.Button_rightStickIn:
			return this.CurFrameInput.Button_rightStickIn.Down;
		default:
			return false;
		}
	}

	public bool GetButtonUp(ControlpadInputValue controllerCode)
	{
		switch (controllerCode)
		{
		case ControlpadInputValue.Button_A:
			return this.CurFrameInput.Button_A.Up;
		case ControlpadInputValue.Button_B:
			return this.CurFrameInput.Button_B.Up;
		case ControlpadInputValue.Button_X:
			return this.CurFrameInput.Button_X.Up;
		case ControlpadInputValue.Button_Y:
			return this.CurFrameInput.Button_Y.Up;
		case ControlpadInputValue.Button_leftShoulder:
			return this.CurFrameInput.Button_leftShoulder.Up;
		case ControlpadInputValue.Button_rightShoulder:
			return this.CurFrameInput.Button_rightShoulder.Up;
		case ControlpadInputValue.Button_start:
			return this.CurFrameInput.Button_start.Up;
		case ControlpadInputValue.Button_back:
			return this.CurFrameInput.Button_back.Up;
		case ControlpadInputValue.Button_leftStickIn:
			return this.CurFrameInput.Button_leftStickIn.Up;
		case ControlpadInputValue.Button_rightStickIn:
			return this.CurFrameInput.Button_rightStickIn.Up;
		default:
			return false;
		}
	}
}
