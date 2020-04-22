using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISpectatorPenPanel : MonoBehaviour
{
	public RectTransform m_container;

	public Image m_drawhitBox;

	public _SelectableBtn m_drawMode;

	public _SelectableBtn m_clearDrawing;

	public RectTransform m_drawOptions;

	public _SelectableBtn m_increaseLineThickness;

	public _SelectableBtn m_decreaseLineThickness;

	public _SelectableBtn m_toggleOptionsBtn;

	public TextMeshProUGUI m_lineThicknessLabel;

	private Color m_clearColor = new Color(1f, 1f, 1f, 0.1f);

	private Color32 m_clearColor32 = new Color(1f, 1f, 1f, 0.1f);

	private Color32[] m_clearColorArray;

	private Color m_drawColor = Color.red;

	private Texture2D theTexture;

	private Dictionary<Vector2, bool> modifiedPixels = new Dictionary<Vector2, bool>();

	private const float ScaleDown = 1f;

	private int LineSize = 5;

	private bool m_isDrawing;

	private int LastXPos;

	private int LastYPos = -1;

	private int LastNumModifiedPixelsDrawn;

	private float LastRedrawTime = -1f;

	private float RedrawTimeInterval = 0.1f;

	private int PixelCountRedrawAmount = 100;

	private const string LineSizePrefName = "SpectatorLineSize";

	private int screenWidth;

	private int screenHeight;

	private void Awake()
	{
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		m_clearColorArray = new Color32[(int)((float)Screen.width * 1f) * (int)((float)Screen.height * 1f)];
		for (int i = 0; i < m_clearColorArray.Length; i++)
		{
			m_clearColorArray[i] = m_clearColor32;
		}
		theTexture = new Texture2D((int)((float)Screen.width * 1f), (int)((float)Screen.height * 1f));
		m_drawColor = HUD_UIResources.Get().m_spectatorPenDrawColor;
		m_drawMode.spriteController.callback = DrawModeClicked;
		m_clearDrawing.spriteController.callback = ClearDrawingClicked;
		m_increaseLineThickness.spriteController.callback = IncreaseLineThickness;
		m_decreaseLineThickness.spriteController.callback = DecreaseLineThickness;
		m_toggleOptionsBtn.spriteController.callback = ToggleOptionsClicked;
		UIManager.SetGameObjectActive(m_drawOptions, false);
		UIManager.SetGameObjectActive(m_drawhitBox, false);
		if (PlayerPrefs.HasKey("SpectatorLineSize"))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			LineSize = PlayerPrefs.GetInt("SpectatorLineSize");
		}
		m_lineThicknessLabel.text = string.Format(StringUtil.TR("LineWidth", "SpectatorMode"), LineSize);
		ClearDrawing(true);
	}

	private void ResizeTexture()
	{
		Object.Destroy(theTexture);
		m_clearColorArray = new Color32[(int)((float)Screen.width * 1f) * (int)((float)Screen.height * 1f)];
		for (int i = 0; i < m_clearColorArray.Length; i++)
		{
			m_clearColorArray[i] = m_clearColor32;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			theTexture = new Texture2D((int)((float)Screen.width * 1f), (int)((float)Screen.height * 1f));
			screenWidth = Screen.width;
			screenHeight = Screen.height;
			ClearDrawing(true);
			return;
		}
	}

	public void ToggleOptionsClicked(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_drawOptions, !m_drawOptions.gameObject.activeSelf);
		m_toggleOptionsBtn.SetSelected(m_drawOptions.gameObject.activeSelf, false, string.Empty, string.Empty);
	}

	public void IncreaseLineThickness(BaseEventData data)
	{
		LineSize++;
		LineSize = Mathf.Clamp(LineSize, HUD_UIResources.Get().m_minLineWidth, HUD_UIResources.Get().m_maxLineWidth);
		m_lineThicknessLabel.text = string.Format(StringUtil.TR("LineWidth", "SpectatorMode"), LineSize);
		PlayerPrefs.SetInt("SpectatorLineSize", LineSize);
	}

	public void DecreaseLineThickness(BaseEventData data)
	{
		LineSize--;
		LineSize = Mathf.Clamp(LineSize, HUD_UIResources.Get().m_minLineWidth, HUD_UIResources.Get().m_maxLineWidth);
		m_lineThicknessLabel.text = string.Format(StringUtil.TR("LineWidth", "SpectatorMode"), LineSize);
		PlayerPrefs.SetInt("SpectatorLineSize", LineSize);
	}

	public void DrawModeClicked(BaseEventData data)
	{
		m_isDrawing = !m_isDrawing;
		m_drawMode.SetSelected(m_isDrawing, false, string.Empty, string.Empty);
		UIManager.SetGameObjectActive(m_drawhitBox, m_isDrawing);
	}

	public void ClearDrawingClicked(BaseEventData data)
	{
		ClearDrawing();
	}

	public void ClearDrawing(bool hardClear = false)
	{
		if (hardClear)
		{
			theTexture.SetPixels32(m_clearColorArray);
			theTexture.Apply();
			m_drawhitBox.sprite = Sprite.Create(theTexture, new Rect(0f, 0f, theTexture.width, theTexture.height), new Vector2(0.5f, 0.5f));
			m_drawhitBox.color = new Color(1f, 1f, 1f, 1f);
			modifiedPixels.Clear();
			LastNumModifiedPixelsDrawn = 0;
		}
		else
		{
			foreach (KeyValuePair<Vector2, bool> modifiedPixel in modifiedPixels)
			{
				Texture2D texture2D = theTexture;
				Vector2 key = modifiedPixel.Key;
				int x = (int)key.x;
				Vector2 key2 = modifiedPixel.Key;
				texture2D.SetPixel(x, (int)key2.y, m_clearColor);
			}
			theTexture.Apply();
			m_drawhitBox.sprite = Sprite.Create(theTexture, new Rect(0f, 0f, theTexture.width, theTexture.height), new Vector2(0.5f, 0.5f));
			m_drawhitBox.color = new Color(1f, 1f, 1f, 1f);
			modifiedPixels.Clear();
			LastNumModifiedPixelsDrawn = 0;
		}
	}

	private void OnDestroy()
	{
		Object.Destroy(theTexture);
	}

	private void PadInMidpointsPoints(Vector2 previousPoint, Vector2 newPoint)
	{
		if (Vector2.Distance(previousPoint, newPoint) < (float)LineSize)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		Vector2 vector = (previousPoint + newPoint) * 0.5f;
		for (int i = (int)vector.x - LineSize; (float)i < vector.x + (float)LineSize; i++)
		{
			for (int j = (int)vector.y - LineSize; (float)j < vector.y + (float)LineSize; j++)
			{
				Vector2 vector2 = new Vector2(i, j);
				if (!(0f <= (float)i))
				{
					continue;
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
				if (i >= theTexture.width)
				{
					continue;
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
				if (0f <= (float)j)
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
					if (j < theTexture.height && Vector2.Distance(vector2, vector) < (float)LineSize)
					{
						theTexture.SetPixel(i, j, m_drawColor);
						modifiedPixels[vector2] = true;
					}
				}
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					goto end_IL_0110;
				}
				continue;
				end_IL_0110:
				break;
			}
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			PadInMidpointsPoints(previousPoint, vector);
			PadInMidpointsPoints(vector, newPoint);
			return;
		}
	}

	private void CheckRedrawTimeInterval()
	{
		RedrawTimeInterval = Mathf.Max(Time.unscaledDeltaTime, RedrawTimeInterval);
	}

	private void Update()
	{
		if (screenWidth == Screen.width)
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
			if (screenHeight == Screen.height)
			{
				goto IL_0041;
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
		ResizeTexture();
		goto IL_0041;
		IL_0041:
		int num;
		if (ClientGameManager.Get() != null)
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
			if (ClientGameManager.Get().PlayerInfo != null)
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
				if (ClientGameManager.Get().PlayerInfo.TeamId == Team.Spectator)
				{
					num = 1;
					goto IL_00d7;
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
		}
		if (GameManager.Get() != null && GameManager.Get().PlayerInfo != null)
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
			num = ((GameManager.Get().PlayerInfo.TeamId == Team.Spectator) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		goto IL_00d7;
		IL_00d7:
		if (num == 0)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			UIManager.SetGameObjectActive(m_container, true);
			if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.ToggleSpectatorDrawMode))
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
				DrawModeClicked(null);
			}
			if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.ClearSpectatorDraw))
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
				ClearDrawing();
			}
			if (!m_isDrawing)
			{
				return;
			}
			if (Input.GetMouseButton(0))
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
				if (!UIUtils.IsMouseOnGUI())
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
					Vector2 vector = Input.mousePosition * 1f;
					if (0f <= vector.x)
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
						if (vector.x < (float)theTexture.width)
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
							if (0f <= vector.y * 1f)
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
								if (vector.y < (float)theTexture.height)
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
									int num2 = (int)vector.x;
									int num3 = (int)vector.y;
									if (LastXPos != -1)
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
										if (LastYPos != -1)
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
											CheckRedrawTimeInterval();
											PadInMidpointsPoints(new Vector2(LastXPos, LastYPos), new Vector2(num2, num3));
										}
									}
									for (int i = num2 - LineSize; i < num2 + LineSize; i++)
									{
										for (int j = num3 - LineSize; j < num3 + LineSize; j++)
										{
											if (!(0f <= (float)i))
											{
												continue;
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
											if (i >= theTexture.width || !(0f <= (float)j))
											{
												continue;
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
											if (j >= theTexture.height)
											{
												continue;
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
											Vector2 vector2 = new Vector2(i, j);
											if (Vector2.Distance(vector2, new Vector2(num2, num3)) < (float)LineSize)
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
												theTexture.SetPixel(i, j, m_drawColor);
												modifiedPixels[vector2] = true;
											}
										}
										while (true)
										{
											switch (3)
											{
											case 0:
												break;
											default:
												goto end_IL_0355;
											}
											continue;
											end_IL_0355:
											break;
										}
									}
									if (modifiedPixels.Count - LastNumModifiedPixelsDrawn > PixelCountRedrawAmount && Time.time > LastRedrawTime + RedrawTimeInterval)
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
										LastRedrawTime = Time.time;
										LastNumModifiedPixelsDrawn = modifiedPixels.Count;
										theTexture.Apply();
										m_drawhitBox.sprite = Sprite.Create(theTexture, new Rect(0f, 0f, theTexture.width, theTexture.height), new Vector2(0.5f, 0.5f));
									}
									LastXPos = num2;
									LastYPos = num3;
								}
							}
						}
					}
				}
			}
			if (Input.GetMouseButtonUp(0))
			{
				LastNumModifiedPixelsDrawn = modifiedPixels.Count;
				LastXPos = -1;
				LastYPos = -1;
				theTexture.Apply();
				m_drawhitBox.sprite = Sprite.Create(theTexture, new Rect(0f, 0f, theTexture.width, theTexture.height), new Vector2(0.5f, 0.5f));
			}
			return;
		}
	}
}
