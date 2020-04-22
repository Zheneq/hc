using UnityEngine;

public class CameraControls : MonoBehaviour
{
	public float m_mouseWheelZoomSpeed = 10f;

	[Tooltip("Speed the camera moves in response to directional input from the keyboard")]
	public float m_keyboardMoveSpeed = 0.4f;

	[Tooltip("Speed the camera moves in response to mouse click and drag")]
	public float m_mouseMoveSpeed = 0.45f;

	[Tooltip("Speed the camera rotates in response to free rotation control with the mouse")]
	public float m_mouseRotationSpeed = 6f;

	[Tooltip("Speed the camera moves when hovering in a fringe of the screen.")]
	public float m_mouseMoveFringeSpeed = 0.2f;

	public float m_mouseMoveFringeEaseInTime = 0.4f;

	public float m_mouseMoveFringeEaseOutTime = 0.2f;

	public AnimationCurve m_mouseMoveFringeEaseIn = new AnimationCurve();

	public AnimationCurve m_mouseMoveFringeEaseOut = new AnimationCurve();

	[Tooltip("Percentage of screen where hovering the mouse should move the camera.")]
	public float m_mouseMoveFringeTop = 0.025f;

	[Tooltip("Percentage of screen where hovering the mouse should move the camera.")]
	public float m_mouseMoveFringeBottom = 0.025f;

	[Tooltip("Percentage of screen where hovering the mouse should move the camera.")]
	public float m_mouseMoveFringeLeft = 0.025f;

	[Tooltip("Percentage of screen where hovering the mouse should move the camera.")]
	public float m_mouseMoveFringeRight = 0.025f;

	public bool m_mouseMoveFringeInEditor;

	public float m_keyboardRotationIncrement = 45f;

	public float m_keyboardRotationDuration = 0.5f;

	public float m_keyboardPitchIncrement = 20f;

	public float m_keyboardRotationRepeatDelay = 0.15f;

	public float m_mousePitchSpeed = 6f;

	public float m_maxPitch = 90f;

	public float m_minPitch = 10f;

	internal Vector3 m_desiredRotationEulerAngles;

	[HideInInspector]
	public bool CameraRotateClockwiseToggled;

	[HideInInspector]
	public bool CameraRotateCounterClockwiseToggled;

	private bool m_enabled = true;

	private EasedVector3AnimationCurve m_mouseMoveFringeSpeedEased = new EasedVector3AnimationCurve(Vector3.zero);

	private float m_rotationChangedTime = -100f;

	private float m_keyboardRotationTime;

	private Quaternion m_prevRotation;

	private const float FPS_TUNED_AT = 80f;

	private static CameraControls s_instance;

	private HighlightUtils.ScrollCursorDirection m_lastScrollCursorDirection = HighlightUtils.ScrollCursorDirection.Undefined;

	private float m_timeLastScrollCursorStarted;

	public float m_secondsUntilScrollCursorStop = 2f;

	internal bool Enabled
	{
		get
		{
			int result;
			if (m_enabled)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (AccountPreferences.DoesApplicationHaveFocus())
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
					if (HUD_UI.Get() != null)
					{
						if (!(HUD_UI.Get().m_textConsole == null))
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
							if (HUD_UI.Get().m_textConsole.IsTextInputFocused())
							{
								goto IL_00b0;
							}
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						if (!(UIDialogPopupManager.Get() == null))
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
							result = ((!UIDialogPopupManager.Get().IsDialogBoxOpen()) ? 1 : 0);
						}
						else
						{
							result = 1;
						}
						goto IL_00b1;
					}
				}
			}
			goto IL_00b0;
			IL_00b1:
			return (byte)result != 0;
			IL_00b0:
			result = 0;
			goto IL_00b1;
		}
		set
		{
			m_enabled = value;
		}
	}

	internal static CameraControls Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	internal bool IsTiltUserControlled()
	{
		return DebugParameters.Get() != null && DebugParameters.Get().GetParameterAsBool("CameraTiltControl");
	}

	internal bool IsMouseDragRotationRequested()
	{
		int result;
		if (IsTiltUserControlled())
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = (Input.GetKey(KeyCode.G) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	internal bool IsMouseDragMoveRequested()
	{
		int result;
		if (!IsMouseDragRotationRequested())
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = (Input.GetMouseButton(2) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	internal void CalcDesiredTransform(Transform currentTransform, out Vector3 positionDelta, out Quaternion rotationThisFrame, out float zoomDelta)
	{
		bool toggleInput;
		Vector3 vector = CalcDesiredEulerAngles(out toggleInput);
		zoomDelta = CalcZoomVelocity() * Mathf.Min(Time.unscaledDeltaTime, 0.06f);
		if (zoomDelta == 0f && !Mathf.Approximately(vector.x, 0f))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_rotationChangedTime = Time.time;
			m_prevRotation = currentTransform.rotation;
			m_desiredRotationEulerAngles.x += vector.x;
			if (toggleInput)
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
				float num = Mathf.Abs(vector.x);
				m_desiredRotationEulerAngles.x = (float)(int)(m_desiredRotationEulerAngles.x / num) * num;
			}
			m_desiredRotationEulerAngles.x = Mathf.Clamp(m_desiredRotationEulerAngles.x, m_minPitch, m_maxPitch);
		}
		if (!Mathf.Approximately(vector.y, 0f))
		{
			if (toggleInput)
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
				if (!(Time.time - m_keyboardRotationTime >= m_keyboardRotationRepeatDelay))
				{
					goto IL_01c6;
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			m_rotationChangedTime = Time.time;
			m_prevRotation = currentTransform.rotation;
			m_keyboardRotationTime = Time.time;
			m_desiredRotationEulerAngles.y += vector.y;
			if (toggleInput)
			{
				float num2 = Mathf.Abs(vector.y);
				m_desiredRotationEulerAngles.y = (float)(int)(m_desiredRotationEulerAngles.y / num2) * num2;
			}
			m_desiredRotationEulerAngles.y %= 360f;
		}
		goto IL_01c6;
		IL_01c6:
		rotationThisFrame = Quaternion.Euler(m_desiredRotationEulerAngles);
		if (!IsMouseDragRotationRequested())
		{
			float num3 = Easing.ExpoEaseOut(Time.time - m_rotationChangedTime, 0f, 1f, m_keyboardRotationDuration);
			if (num3 < 1f)
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
				Vector3 eulerAngles = rotationThisFrame.eulerAngles;
				float x = eulerAngles.x;
				Vector3 eulerAngles2 = m_prevRotation.eulerAngles;
				rotationThisFrame = Quaternion.Euler(x, Mathf.LerpAngle(eulerAngles2.y, eulerAngles.y, num3), eulerAngles.z);
			}
		}
		positionDelta = CalcVelocity() * Mathf.Min(Time.unscaledDeltaTime, 0.03333333f);
		if (!(positionDelta.sqrMagnitude > float.Epsilon))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			Vector3 eulerAngles3 = rotationThisFrame.eulerAngles;
			Quaternion rotation = Quaternion.Euler(0f, eulerAngles3.y, 0f);
			Vector3 a = rotation * -Vector3.forward;
			Vector3 a2 = rotation * -Vector3.right;
			positionDelta = positionDelta.x * a2 + positionDelta.z * a;
			return;
		}
	}

	private float CalcZoomVelocity()
	{
		bool flag;
		if (ControlpadGameplay.Get() != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (ControlpadGameplay.Get().UsingControllerInput)
			{
				flag = false;
				goto IL_0034;
			}
		}
		flag = true;
		goto IL_0034;
		IL_0034:
		if (flag)
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
			if (!Input.GetMouseButton(2))
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
				if (Enabled)
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
					if (!UIUtils.IsMouseOnGUI())
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
						if (!(KeyBinding_UI.Get() == null))
						{
							if (KeyBinding_UI.Get().IsVisible())
							{
								goto IL_0163;
							}
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						if (!EmoticonPanel.IsMouseOverEmoticonPanel())
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									break;
								default:
									return (0f - Input.GetAxis("Mouse ScrollWheel")) * 3f * m_mouseWheelZoomSpeed * 80f;
								}
							}
						}
					}
				}
			}
		}
		else if (Enabled)
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
			if (!UIUtils.IsMouseOnGUI())
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
				if (!(KeyBinding_UI.Get() == null))
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
					if (KeyBinding_UI.Get().IsVisible())
					{
						goto IL_0163;
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				if (!EmoticonPanel.IsMouseOverEmoticonPanel())
				{
					float axisValue = ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.DpadY);
					return (0f - axisValue) * 3f;
				}
			}
		}
		goto IL_0163;
		IL_0163:
		return 0f;
	}

	private Vector3 CalcDesiredEulerAngles(out bool toggleInput)
	{
		toggleInput = false;
		Vector3 zero = Vector3.zero;
		bool flag = false;
		if (GameFlowData.Get() != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (GameFlowData.Get().gameState == GameState.EndingGame)
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
				flag = true;
			}
		}
		if ((bool)SinglePlayerManager.Get())
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
			if (SinglePlayerManager.Get().HasPendingCameraUpdate())
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
				Vector3 position;
				if (GameFlowData.Get().activeOwnedActorData == null)
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
					position = base.transform.position;
				}
				else
				{
					position = GameFlowData.Get().activeOwnedActorData.transform.position;
				}
				Vector3 b = position;
				Vector3 center = CameraManager.Get().CameraPositionBounds.center;
				center = SinglePlayerManager.Get().GetCurrentState().m_cameraRotationTarget.transform.position;
				Vector3 forward = center - b;
				forward.y = 0f;
				float magnitude = forward.magnitude;
				float num = 0f;
				if (!Mathf.Approximately(magnitude, 0f))
				{
					forward /= magnitude;
					Vector3 eulerAngles = Quaternion.LookRotation(forward).eulerAngles;
					num = (int)eulerAngles.y / 45 * 45;
				}
				zero.y = num - m_desiredRotationEulerAngles.y;
				goto IL_041f;
			}
		}
		int num3;
		int num4;
		if (Enabled)
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
			if (!flag)
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
				int num2;
				if (!CameraRotateClockwiseToggled)
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
					num2 = (InputManager.Get().IsKeyBindingHeld(KeyPreference.CameraRotateClockwise) ? 1 : 0);
				}
				else
				{
					num2 = 1;
				}
				bool flag2 = (byte)num2 != 0;
				bool flag3 = CameraRotateCounterClockwiseToggled || InputManager.Get().IsKeyBindingHeld(KeyPreference.CameraRotateCounterClockwise);
				if (Input.GetKeyDown(KeyCode.V) && DebugParameters.Get() != null)
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
					if (DebugParameters.Get().GetParameterAsBool("CameraTiltControl"))
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
						zero.x = m_keyboardPitchIncrement;
						toggleInput = true;
						goto IL_041f;
					}
				}
				if (Input.GetKeyDown(KeyCode.B))
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
					if (DebugParameters.Get() != null)
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
						if (DebugParameters.Get().GetParameterAsBool("CameraTiltControl"))
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
							zero.x = 0f - m_keyboardPitchIncrement;
							toggleInput = true;
							goto IL_041f;
						}
					}
				}
				if (flag2 ^ flag3)
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
					bool @bool = AccountPreferences.Get().GetBool(BoolPreference.InvertCameraRotationKbd);
					if (flag2)
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
						if (!@bool)
						{
							num3 = 1;
							goto IL_02f0;
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
					if (flag3)
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
						num3 = (@bool ? 1 : 0);
					}
					else
					{
						num3 = 0;
					}
					goto IL_02f0;
				}
				if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.DpadX) != 0f)
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
					if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.DpadX) < 0f)
					{
						if (!AccountPreferences.Get().GetBool(BoolPreference.InvertCameraRotationKbd))
						{
							num4 = 1;
							goto IL_03a9;
						}
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.DpadX) > 0f)
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
						num4 = (AccountPreferences.Get().GetBool(BoolPreference.InvertCameraRotationKbd) ? 1 : 0);
					}
					else
					{
						num4 = 0;
					}
					goto IL_03a9;
				}
				if (IsMouseDragRotationRequested())
				{
					zero.y = Input.GetAxis("Mouse X") * m_mouseRotationSpeed;
					zero.x = (0f - Input.GetAxis("Mouse Y")) * m_mousePitchSpeed;
				}
			}
		}
		goto IL_041f;
		IL_02f0:
		bool flag4 = (byte)num3 != 0;
		zero.y = m_keyboardRotationIncrement * ((!flag4) ? (-1f) : 1f);
		toggleInput = true;
		CameraManager.Get().OnPlayerMovedCamera();
		goto IL_041f;
		IL_041f:
		return zero;
		IL_03a9:
		bool flag5 = (byte)num4 != 0;
		float keyboardRotationIncrement = m_keyboardRotationIncrement;
		float num5;
		if (flag5)
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
			num5 = 1f;
		}
		else
		{
			num5 = -1f;
		}
		zero.y = keyboardRotationIncrement * num5;
		toggleInput = true;
		CameraManager.Get().OnPlayerMovedCamera();
		goto IL_041f;
	}

	private Vector3 CalcVelocity()
	{
		Vector3 a = Vector3.zero;
		bool flag = false;
		if (GameFlowData.Get() != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (GameFlowData.Get().gameState == GameState.EndingGame)
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
				flag = true;
			}
		}
		float num;
		float num2;
		bool flag2;
		if (Enabled)
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
			if (!flag)
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
				num = 0f;
				num2 = 0f;
				if (ControlpadGameplay.Get() != null)
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
					if (ControlpadGameplay.Get().UsingControllerInput)
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
						flag2 = false;
						goto IL_00b7;
					}
				}
				flag2 = true;
				goto IL_00b7;
			}
		}
		if (flag)
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
			HighlightUtils.Get().ResetCursor();
		}
		goto IL_05f0;
		IL_02ad:
		Vector3 zero;
		bool flag3 = zero != Vector3.zero && !UIUtils.IsMouseOnGUI();
		HighlightUtils.ScrollCursorDirection scrollCursorDirection;
		WinUtils.User32.POINT lpPoint;
		if (flag3)
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
			m_mouseMoveFringeSpeedEased.EaseTo(zero, m_mouseMoveFringeEaseIn, m_mouseMoveFringeEaseInTime);
			HighlightUtils.Get().SetScrollCursor(scrollCursorDirection);
			if (scrollCursorDirection != m_lastScrollCursorDirection)
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
				m_lastScrollCursorDirection = scrollCursorDirection;
				m_timeLastScrollCursorStarted = Time.unscaledTime;
			}
			else if (Time.unscaledTime > m_timeLastScrollCursorStarted + m_secondsUntilScrollCursorStop)
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
				lpPoint = default(WinUtils.User32.POINT);
				WinUtils.User32.GetCursorPos(out lpPoint);
				if (m_lastScrollCursorDirection != HighlightUtils.ScrollCursorDirection.NE)
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
					if (m_lastScrollCursorDirection != HighlightUtils.ScrollCursorDirection.E)
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
						if (m_lastScrollCursorDirection != HighlightUtils.ScrollCursorDirection.SE)
						{
							goto IL_03ad;
						}
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
					}
				}
				lpPoint.X -= 2;
				goto IL_03ad;
			}
		}
		else
		{
			m_mouseMoveFringeSpeedEased.EaseTo(Vector3.zero, m_mouseMoveFringeEaseOut, m_mouseMoveFringeEaseOutTime);
			HighlightUtils.Get().SetScrollCursor(HighlightUtils.ScrollCursorDirection.Undefined);
			m_lastScrollCursorDirection = HighlightUtils.ScrollCursorDirection.Undefined;
			m_timeLastScrollCursorStarted = Time.unscaledTime;
		}
		goto IL_04bb;
		IL_03ad:
		if (m_lastScrollCursorDirection != HighlightUtils.ScrollCursorDirection.NW && m_lastScrollCursorDirection != HighlightUtils.ScrollCursorDirection.W)
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
			if (m_lastScrollCursorDirection != HighlightUtils.ScrollCursorDirection.SW)
			{
				goto IL_03eb;
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
		lpPoint.X += 2;
		goto IL_03eb;
		IL_041e:
		if (m_lastScrollCursorDirection != HighlightUtils.ScrollCursorDirection.SE)
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
			if (m_lastScrollCursorDirection != HighlightUtils.ScrollCursorDirection.S)
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
				if (m_lastScrollCursorDirection != HighlightUtils.ScrollCursorDirection.SW)
				{
					goto IL_0466;
				}
				while (true)
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
		lpPoint.Y -= 2;
		goto IL_0466;
		IL_04bb:
		if (IsMouseDragMoveRequested())
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
			a.x = Input.GetAxis("Mouse X") * m_mouseMoveSpeed;
			a.z = Input.GetAxis("Mouse Y") * m_mouseMoveSpeed;
		}
		else
		{
			if (num == 0f)
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
				if (num2 == 0f)
				{
					if (flag3)
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
						Vector3 vector;
						if (m_mouseMoveFringeEaseInTime > 0f)
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
							vector = m_mouseMoveFringeSpeedEased;
						}
						else
						{
							vector = zero;
						}
						a = vector;
					}
					else
					{
						Vector3 vector2;
						if (m_mouseMoveFringeEaseOutTime > 0f)
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
							vector2 = m_mouseMoveFringeSpeedEased;
						}
						else
						{
							vector2 = Vector3.zero;
						}
						a = vector2;
					}
					goto IL_05f0;
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (num2 != 0f)
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
				a.z += 0f - m_keyboardMoveSpeed * num2;
			}
			if (num != 0f)
			{
				a.x += 0f - m_keyboardMoveSpeed * num;
			}
		}
		goto IL_05f0;
		IL_0466:
		WinUtils.User32.SetCursorPos(lpPoint.X, lpPoint.Y);
		goto IL_04bb;
		IL_05f0:
		return a * 80f;
		IL_03eb:
		if (m_lastScrollCursorDirection != HighlightUtils.ScrollCursorDirection.NE && m_lastScrollCursorDirection != 0)
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
			if (m_lastScrollCursorDirection != HighlightUtils.ScrollCursorDirection.NW)
			{
				goto IL_041e;
			}
		}
		lpPoint.Y += 2;
		goto IL_041e;
		IL_00b7:
		if (flag2)
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
			if (InputManager.Get().IsKeyBindingHeld(KeyPreference.CameraPanUp))
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
				num2 = 1f;
			}
			if (InputManager.Get().IsKeyBindingHeld(KeyPreference.CameraPanLeft))
			{
				num = -1f;
			}
			if (InputManager.Get().IsKeyBindingHeld(KeyPreference.CameraPanDown))
			{
				num2 = -1f;
			}
			if (InputManager.Get().IsKeyBindingHeld(KeyPreference.CameraPanRight))
			{
				num = 1f;
			}
		}
		else
		{
			num = ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.RightStickX);
			num2 = ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.RightStickY);
		}
		scrollCursorDirection = HighlightUtils.ScrollCursorDirection.Undefined;
		Vector3 mousePosition = Input.mousePosition;
		float x = mousePosition.x / (float)Screen.width;
		Vector3 mousePosition2 = Input.mousePosition;
		Vector2 vector3 = new Vector2(x, mousePosition2.y / (float)Screen.height);
		zero = Vector3.zero;
		if (Cursor.lockState == CursorLockMode.Confined)
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
			if (!m_mouseMoveFringeInEditor)
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
				if (Application.isEditor)
				{
					goto IL_02ad;
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (vector3.x <= m_mouseMoveFringeLeft)
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
				zero.x = m_mouseMoveFringeSpeed;
				scrollCursorDirection = HighlightUtils.ScrollCursorDirection.W;
			}
			else if (vector3.x >= 1f - m_mouseMoveFringeRight)
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
				zero.x = 0f - m_mouseMoveFringeSpeed;
				scrollCursorDirection = HighlightUtils.ScrollCursorDirection.E;
			}
			if (vector3.y <= m_mouseMoveFringeBottom)
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
				zero.z = m_mouseMoveFringeSpeed;
				int num3;
				if (scrollCursorDirection == HighlightUtils.ScrollCursorDirection.W)
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
					num3 = 5;
				}
				else if (scrollCursorDirection == HighlightUtils.ScrollCursorDirection.E)
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
					num3 = 3;
				}
				else
				{
					num3 = 4;
				}
				scrollCursorDirection = (HighlightUtils.ScrollCursorDirection)num3;
			}
			else if (vector3.y >= 1f - m_mouseMoveFringeTop)
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
				zero.z = 0f - m_mouseMoveFringeSpeed;
				int num4;
				switch (scrollCursorDirection)
				{
				case HighlightUtils.ScrollCursorDirection.W:
					num4 = 7;
					break;
				case HighlightUtils.ScrollCursorDirection.E:
					num4 = 1;
					break;
				default:
					num4 = 0;
					break;
				}
				scrollCursorDirection = (HighlightUtils.ScrollCursorDirection)num4;
			}
		}
		goto IL_02ad;
	}
}
