using System;
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
			if (this.m_enabled)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(CameraControls.get_Enabled()).MethodHandle;
				}
				if (AccountPreferences.DoesApplicationHaveFocus())
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
					if (HUD_UI.Get() != null)
					{
						if (!(HUD_UI.Get().m_textConsole == null))
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
							if (HUD_UI.Get().m_textConsole.IsTextInputFocused(false))
							{
								goto IL_B0;
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
						if (!(UIDialogPopupManager.Get() == null))
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
							result = ((!UIDialogPopupManager.Get().IsDialogBoxOpen()) ? 1 : 0);
						}
						else
						{
							result = 1;
						}
						return result != 0;
					}
				}
			}
			IL_B0:
			result = 0;
			return result != 0;
		}
		set
		{
			this.m_enabled = value;
		}
	}

	internal static CameraControls Get()
	{
		return CameraControls.s_instance;
	}

	private void Awake()
	{
		CameraControls.s_instance = this;
	}

	private void OnDestroy()
	{
		CameraControls.s_instance = null;
	}

	internal bool IsTiltUserControlled()
	{
		return DebugParameters.Get() != null && DebugParameters.Get().GetParameterAsBool("CameraTiltControl");
	}

	internal bool IsMouseDragRotationRequested()
	{
		bool result;
		if (this.IsTiltUserControlled())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraControls.IsMouseDragRotationRequested()).MethodHandle;
			}
			result = Input.GetKey(KeyCode.G);
		}
		else
		{
			result = false;
		}
		return result;
	}

	internal bool IsMouseDragMoveRequested()
	{
		bool result;
		if (!this.IsMouseDragRotationRequested())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraControls.IsMouseDragMoveRequested()).MethodHandle;
			}
			result = Input.GetMouseButton(2);
		}
		else
		{
			result = false;
		}
		return result;
	}

	internal unsafe void CalcDesiredTransform(Transform currentTransform, out Vector3 positionDelta, out Quaternion rotationThisFrame, out float zoomDelta)
	{
		bool flag;
		Vector3 vector = this.CalcDesiredEulerAngles(out flag);
		zoomDelta = this.CalcZoomVelocity() * Mathf.Min(Time.unscaledDeltaTime, 0.06f);
		if (zoomDelta == 0f)
		{
			if (!Mathf.Approximately(vector.x, 0f))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(CameraControls.CalcDesiredTransform(Transform, Vector3*, Quaternion*, float*)).MethodHandle;
				}
				this.m_rotationChangedTime = Time.time;
				this.m_prevRotation = currentTransform.rotation;
				this.m_desiredRotationEulerAngles.x = this.m_desiredRotationEulerAngles.x + vector.x;
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
					float num = Mathf.Abs(vector.x);
					this.m_desiredRotationEulerAngles.x = (float)((int)(this.m_desiredRotationEulerAngles.x / num)) * num;
				}
				this.m_desiredRotationEulerAngles.x = Mathf.Clamp(this.m_desiredRotationEulerAngles.x, this.m_minPitch, this.m_maxPitch);
			}
		}
		if (!Mathf.Approximately(vector.y, 0f))
		{
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
				if (Time.time - this.m_keyboardRotationTime < this.m_keyboardRotationRepeatDelay)
				{
					goto IL_1C6;
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
			this.m_rotationChangedTime = Time.time;
			this.m_prevRotation = currentTransform.rotation;
			this.m_keyboardRotationTime = Time.time;
			this.m_desiredRotationEulerAngles.y = this.m_desiredRotationEulerAngles.y + vector.y;
			if (flag)
			{
				float num2 = Mathf.Abs(vector.y);
				this.m_desiredRotationEulerAngles.y = (float)((int)(this.m_desiredRotationEulerAngles.y / num2)) * num2;
			}
			this.m_desiredRotationEulerAngles.y = this.m_desiredRotationEulerAngles.y % 360f;
		}
		IL_1C6:
		rotationThisFrame = Quaternion.Euler(this.m_desiredRotationEulerAngles);
		if (!this.IsMouseDragRotationRequested())
		{
			float num3 = Easing.ExpoEaseOut(Time.time - this.m_rotationChangedTime, 0f, 1f, this.m_keyboardRotationDuration);
			if (num3 < 1f)
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
				Vector3 eulerAngles = rotationThisFrame.eulerAngles;
				rotationThisFrame = Quaternion.Euler(eulerAngles.x, Mathf.LerpAngle(this.m_prevRotation.eulerAngles.y, eulerAngles.y, num3), eulerAngles.z);
			}
		}
		positionDelta = this.CalcVelocity() * Mathf.Min(Time.unscaledDeltaTime, 0.03333333f);
		if (positionDelta.sqrMagnitude > 1.401298E-45f)
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
			Quaternion rotation = Quaternion.Euler(0f, rotationThisFrame.eulerAngles.y, 0f);
			Vector3 a = rotation * -Vector3.forward;
			Vector3 a2 = rotation * -Vector3.right;
			positionDelta = positionDelta.x * a2 + positionDelta.z * a;
		}
	}

	private float CalcZoomVelocity()
	{
		bool flag;
		if (ControlpadGameplay.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraControls.CalcZoomVelocity()).MethodHandle;
			}
			if (ControlpadGameplay.Get().UsingControllerInput)
			{
				flag = false;
				goto IL_34;
			}
		}
		flag = true;
		IL_34:
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
			if (!Input.GetMouseButton(2))
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
				if (this.Enabled)
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
					if (!UIUtils.IsMouseOnGUI())
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
						if (!(KeyBinding_UI.Get() == null))
						{
							if (KeyBinding_UI.Get().IsVisible())
							{
								goto IL_E2;
							}
							for (;;)
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
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							return -Input.GetAxis("Mouse ScrollWheel") * 3f * this.m_mouseWheelZoomSpeed * 80f;
						}
					}
				}
			}
			IL_E2:;
		}
		else if (this.Enabled)
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
			if (!UIUtils.IsMouseOnGUI())
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
				if (!(KeyBinding_UI.Get() == null))
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
					if (KeyBinding_UI.Get().IsVisible())
					{
						goto IL_163;
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
				if (!EmoticonPanel.IsMouseOverEmoticonPanel())
				{
					float axisValue = ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.DpadY);
					return -axisValue * 3f;
				}
			}
		}
		IL_163:
		return 0f;
	}

	private unsafe Vector3 CalcDesiredEulerAngles(out bool toggleInput)
	{
		toggleInput = false;
		Vector3 zero = Vector3.zero;
		bool flag = false;
		if (GameFlowData.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraControls.CalcDesiredEulerAngles(bool*)).MethodHandle;
			}
			if (GameFlowData.Get().gameState == GameState.EndingGame)
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
				flag = true;
			}
		}
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
			if (SinglePlayerManager.Get().HasPendingCameraUpdate())
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
				Vector3 position;
				if (GameFlowData.Get().activeOwnedActorData == null)
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
					position = base.transform.position;
				}
				else
				{
					position = GameFlowData.Get().activeOwnedActorData.transform.position;
				}
				Vector3 b = position;
				Vector3 a = CameraManager.Get().CameraPositionBounds.center;
				a = SinglePlayerManager.Get().GetCurrentState().m_cameraRotationTarget.transform.position;
				Vector3 vector = a - b;
				vector.y = 0f;
				float magnitude = vector.magnitude;
				float num = 0f;
				if (!Mathf.Approximately(magnitude, 0f))
				{
					vector /= magnitude;
					num = (float)((int)Quaternion.LookRotation(vector).eulerAngles.y / 0x2D * 0x2D);
				}
				zero.y = num - this.m_desiredRotationEulerAngles.y;
				return zero;
			}
		}
		if (this.Enabled)
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
			if (!flag)
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
				bool flag2;
				if (!this.CameraRotateClockwiseToggled)
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
					flag2 = InputManager.Get().IsKeyBindingHeld(KeyPreference.CameraRotateClockwise);
				}
				else
				{
					flag2 = true;
				}
				bool flag3 = flag2;
				bool flag4 = this.CameraRotateCounterClockwiseToggled || InputManager.Get().IsKeyBindingHeld(KeyPreference.CameraRotateCounterClockwise);
				if (Input.GetKeyDown(KeyCode.V) && DebugParameters.Get() != null)
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
					if (DebugParameters.Get().GetParameterAsBool("CameraTiltControl"))
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
						zero.x = this.m_keyboardPitchIncrement;
						toggleInput = true;
						return zero;
					}
				}
				if (Input.GetKeyDown(KeyCode.B))
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
					if (DebugParameters.Get() != null)
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
						if (DebugParameters.Get().GetParameterAsBool("CameraTiltControl"))
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
							zero.x = -this.m_keyboardPitchIncrement;
							toggleInput = true;
							return zero;
						}
					}
				}
				if (flag3 ^ flag4)
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
					bool @bool = AccountPreferences.Get().GetBool(BoolPreference.InvertCameraRotationKbd);
					bool flag5;
					if (flag3)
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
						if (!@bool)
						{
							flag5 = true;
							goto IL_2F0;
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
					if (flag4)
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
						flag5 = @bool;
					}
					else
					{
						flag5 = false;
					}
					IL_2F0:
					bool flag6 = flag5;
					zero.y = this.m_keyboardRotationIncrement * ((!flag6) ? -1f : 1f);
					toggleInput = true;
					CameraManager.Get().OnPlayerMovedCamera();
				}
				else if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.DpadX) != 0f)
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
					bool flag7;
					if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.DpadX) < 0f)
					{
						if (!AccountPreferences.Get().GetBool(BoolPreference.InvertCameraRotationKbd))
						{
							flag7 = true;
							goto IL_3A9;
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
					if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.DpadX) > 0f)
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
						flag7 = AccountPreferences.Get().GetBool(BoolPreference.InvertCameraRotationKbd);
					}
					else
					{
						flag7 = false;
					}
					IL_3A9:
					bool flag8 = flag7;
					float keyboardRotationIncrement = this.m_keyboardRotationIncrement;
					float num2;
					if (flag8)
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
						num2 = 1f;
					}
					else
					{
						num2 = -1f;
					}
					zero.y = keyboardRotationIncrement * num2;
					toggleInput = true;
					CameraManager.Get().OnPlayerMovedCamera();
				}
				else if (this.IsMouseDragRotationRequested())
				{
					zero.y = Input.GetAxis("Mouse X") * this.m_mouseRotationSpeed;
					zero.x = -Input.GetAxis("Mouse Y") * this.m_mousePitchSpeed;
				}
			}
		}
		return zero;
	}

	private Vector3 CalcVelocity()
	{
		Vector3 a = Vector3.zero;
		bool flag = false;
		if (GameFlowData.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraControls.CalcVelocity()).MethodHandle;
			}
			if (GameFlowData.Get().gameState == GameState.EndingGame)
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
				flag = true;
			}
		}
		if (this.Enabled)
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
			if (!flag)
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
				float num = 0f;
				float num2 = 0f;
				bool flag2;
				if (ControlpadGameplay.Get() != null)
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
					if (ControlpadGameplay.Get().UsingControllerInput)
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
						flag2 = false;
						goto IL_B7;
					}
				}
				flag2 = true;
				IL_B7:
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
					if (InputManager.Get().IsKeyBindingHeld(KeyPreference.CameraPanUp))
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
				HighlightUtils.ScrollCursorDirection scrollCursorDirection = HighlightUtils.ScrollCursorDirection.Undefined;
				Vector2 vector = new Vector2(Input.mousePosition.x / (float)Screen.width, Input.mousePosition.y / (float)Screen.height);
				Vector3 zero = Vector3.zero;
				if (Cursor.lockState == CursorLockMode.Confined)
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
					if (!this.m_mouseMoveFringeInEditor)
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
						if (Application.isEditor)
						{
							goto IL_2AD;
						}
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					if (vector.x <= this.m_mouseMoveFringeLeft)
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
						zero.x = this.m_mouseMoveFringeSpeed;
						scrollCursorDirection = HighlightUtils.ScrollCursorDirection.W;
					}
					else if (vector.x >= 1f - this.m_mouseMoveFringeRight)
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
						zero.x = -this.m_mouseMoveFringeSpeed;
						scrollCursorDirection = HighlightUtils.ScrollCursorDirection.E;
					}
					if (vector.y <= this.m_mouseMoveFringeBottom)
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
						zero.z = this.m_mouseMoveFringeSpeed;
						HighlightUtils.ScrollCursorDirection scrollCursorDirection2;
						if (scrollCursorDirection == HighlightUtils.ScrollCursorDirection.W)
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
							scrollCursorDirection2 = HighlightUtils.ScrollCursorDirection.SW;
						}
						else if (scrollCursorDirection == HighlightUtils.ScrollCursorDirection.E)
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
							scrollCursorDirection2 = HighlightUtils.ScrollCursorDirection.SE;
						}
						else
						{
							scrollCursorDirection2 = HighlightUtils.ScrollCursorDirection.S;
						}
						scrollCursorDirection = scrollCursorDirection2;
					}
					else if (vector.y >= 1f - this.m_mouseMoveFringeTop)
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
						zero.z = -this.m_mouseMoveFringeSpeed;
						scrollCursorDirection = ((scrollCursorDirection != HighlightUtils.ScrollCursorDirection.W) ? ((scrollCursorDirection != HighlightUtils.ScrollCursorDirection.E) ? HighlightUtils.ScrollCursorDirection.N : HighlightUtils.ScrollCursorDirection.NE) : HighlightUtils.ScrollCursorDirection.NW);
					}
				}
				IL_2AD:
				bool flag3 = zero != Vector3.zero && !UIUtils.IsMouseOnGUI();
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
					this.m_mouseMoveFringeSpeedEased.EaseTo(zero, this.m_mouseMoveFringeEaseIn, this.m_mouseMoveFringeEaseInTime);
					HighlightUtils.Get().SetScrollCursor(scrollCursorDirection);
					if (scrollCursorDirection != this.m_lastScrollCursorDirection)
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
						this.m_lastScrollCursorDirection = scrollCursorDirection;
						this.m_timeLastScrollCursorStarted = Time.unscaledTime;
					}
					else if (Time.unscaledTime > this.m_timeLastScrollCursorStarted + this.m_secondsUntilScrollCursorStop)
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
						WinUtils.User32.POINT point = default(WinUtils.User32.POINT);
						WinUtils.User32.GetCursorPos(out point);
						if (this.m_lastScrollCursorDirection != HighlightUtils.ScrollCursorDirection.NE)
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
							if (this.m_lastScrollCursorDirection != HighlightUtils.ScrollCursorDirection.E)
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
								if (this.m_lastScrollCursorDirection != HighlightUtils.ScrollCursorDirection.SE)
								{
									goto IL_3AD;
								}
								for (;;)
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
						point.X -= 2;
						IL_3AD:
						if (this.m_lastScrollCursorDirection != HighlightUtils.ScrollCursorDirection.NW && this.m_lastScrollCursorDirection != HighlightUtils.ScrollCursorDirection.W)
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
							if (this.m_lastScrollCursorDirection != HighlightUtils.ScrollCursorDirection.SW)
							{
								goto IL_3EB;
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
						point.X += 2;
						IL_3EB:
						if (this.m_lastScrollCursorDirection != HighlightUtils.ScrollCursorDirection.NE && this.m_lastScrollCursorDirection != HighlightUtils.ScrollCursorDirection.N)
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
							if (this.m_lastScrollCursorDirection != HighlightUtils.ScrollCursorDirection.NW)
							{
								goto IL_41E;
							}
						}
						point.Y += 2;
						IL_41E:
						if (this.m_lastScrollCursorDirection != HighlightUtils.ScrollCursorDirection.SE)
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
							if (this.m_lastScrollCursorDirection != HighlightUtils.ScrollCursorDirection.S)
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
								if (this.m_lastScrollCursorDirection != HighlightUtils.ScrollCursorDirection.SW)
								{
									goto IL_466;
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
						point.Y -= 2;
						IL_466:
						WinUtils.User32.SetCursorPos(point.X, point.Y);
					}
				}
				else
				{
					this.m_mouseMoveFringeSpeedEased.EaseTo(Vector3.zero, this.m_mouseMoveFringeEaseOut, this.m_mouseMoveFringeEaseOutTime);
					HighlightUtils.Get().SetScrollCursor(HighlightUtils.ScrollCursorDirection.Undefined);
					this.m_lastScrollCursorDirection = HighlightUtils.ScrollCursorDirection.Undefined;
					this.m_timeLastScrollCursorStarted = Time.unscaledTime;
				}
				if (this.IsMouseDragMoveRequested())
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
					a.x = Input.GetAxis("Mouse X") * this.m_mouseMoveSpeed;
					a.z = Input.GetAxis("Mouse Y") * this.m_mouseMoveSpeed;
				}
				else
				{
					if (num == 0f)
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
						if (num2 != 0f)
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
							if (flag3)
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
								Vector3 vector2;
								if (this.m_mouseMoveFringeEaseInTime > 0f)
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
									vector2 = this.m_mouseMoveFringeSpeedEased;
								}
								else
								{
									vector2 = zero;
								}
								a = vector2;
								goto IL_5D7;
							}
							Vector3 vector3;
							if (this.m_mouseMoveFringeEaseOutTime > 0f)
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
								vector3 = this.m_mouseMoveFringeSpeedEased;
							}
							else
							{
								vector3 = Vector3.zero;
							}
							a = vector3;
							goto IL_5D7;
						}
					}
					if (num2 != 0f)
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
						a.z += -(this.m_keyboardMoveSpeed * num2);
					}
					if (num != 0f)
					{
						a.x += -(this.m_keyboardMoveSpeed * num);
					}
				}
				IL_5D7:
				goto IL_5F0;
			}
		}
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
			HighlightUtils.Get().ResetCursor();
		}
		IL_5F0:
		return a * 80f;
	}
}
