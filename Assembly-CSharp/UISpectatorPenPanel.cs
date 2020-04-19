using System;
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

	private int PixelCountRedrawAmount = 0x64;

	private const string LineSizePrefName = "SpectatorLineSize";

	private int screenWidth;

	private int screenHeight;

	private void Awake()
	{
		this.screenWidth = Screen.width;
		this.screenHeight = Screen.height;
		this.m_clearColorArray = new Color32[(int)((float)Screen.width * 1f) * (int)((float)Screen.height * 1f)];
		for (int i = 0; i < this.m_clearColorArray.Length; i++)
		{
			this.m_clearColorArray[i] = this.m_clearColor32;
		}
		this.theTexture = new Texture2D((int)((float)Screen.width * 1f), (int)((float)Screen.height * 1f));
		this.m_drawColor = HUD_UIResources.Get().m_spectatorPenDrawColor;
		this.m_drawMode.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.DrawModeClicked);
		this.m_clearDrawing.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ClearDrawingClicked);
		this.m_increaseLineThickness.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.IncreaseLineThickness);
		this.m_decreaseLineThickness.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.DecreaseLineThickness);
		this.m_toggleOptionsBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ToggleOptionsClicked);
		UIManager.SetGameObjectActive(this.m_drawOptions, false, null);
		UIManager.SetGameObjectActive(this.m_drawhitBox, false, null);
		if (PlayerPrefs.HasKey("SpectatorLineSize"))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISpectatorPenPanel.Awake()).MethodHandle;
			}
			this.LineSize = PlayerPrefs.GetInt("SpectatorLineSize");
		}
		this.m_lineThicknessLabel.text = string.Format(StringUtil.TR("LineWidth", "SpectatorMode"), this.LineSize);
		this.ClearDrawing(true);
	}

	private void ResizeTexture()
	{
		UnityEngine.Object.Destroy(this.theTexture);
		this.m_clearColorArray = new Color32[(int)((float)Screen.width * 1f) * (int)((float)Screen.height * 1f)];
		for (int i = 0; i < this.m_clearColorArray.Length; i++)
		{
			this.m_clearColorArray[i] = this.m_clearColor32;
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(UISpectatorPenPanel.ResizeTexture()).MethodHandle;
		}
		this.theTexture = new Texture2D((int)((float)Screen.width * 1f), (int)((float)Screen.height * 1f));
		this.screenWidth = Screen.width;
		this.screenHeight = Screen.height;
		this.ClearDrawing(true);
	}

	public void ToggleOptionsClicked(BaseEventData data)
	{
		UIManager.SetGameObjectActive(this.m_drawOptions, !this.m_drawOptions.gameObject.activeSelf, null);
		this.m_toggleOptionsBtn.SetSelected(this.m_drawOptions.gameObject.activeSelf, false, string.Empty, string.Empty);
	}

	public void IncreaseLineThickness(BaseEventData data)
	{
		this.LineSize++;
		this.LineSize = Mathf.Clamp(this.LineSize, HUD_UIResources.Get().m_minLineWidth, HUD_UIResources.Get().m_maxLineWidth);
		this.m_lineThicknessLabel.text = string.Format(StringUtil.TR("LineWidth", "SpectatorMode"), this.LineSize);
		PlayerPrefs.SetInt("SpectatorLineSize", this.LineSize);
	}

	public void DecreaseLineThickness(BaseEventData data)
	{
		this.LineSize--;
		this.LineSize = Mathf.Clamp(this.LineSize, HUD_UIResources.Get().m_minLineWidth, HUD_UIResources.Get().m_maxLineWidth);
		this.m_lineThicknessLabel.text = string.Format(StringUtil.TR("LineWidth", "SpectatorMode"), this.LineSize);
		PlayerPrefs.SetInt("SpectatorLineSize", this.LineSize);
	}

	public void DrawModeClicked(BaseEventData data)
	{
		this.m_isDrawing = !this.m_isDrawing;
		this.m_drawMode.SetSelected(this.m_isDrawing, false, string.Empty, string.Empty);
		UIManager.SetGameObjectActive(this.m_drawhitBox, this.m_isDrawing, null);
	}

	public void ClearDrawingClicked(BaseEventData data)
	{
		this.ClearDrawing(false);
	}

	public void ClearDrawing(bool hardClear = false)
	{
		if (hardClear)
		{
			this.theTexture.SetPixels32(this.m_clearColorArray);
			this.theTexture.Apply();
			this.m_drawhitBox.sprite = Sprite.Create(this.theTexture, new Rect(0f, 0f, (float)this.theTexture.width, (float)this.theTexture.height), new Vector2(0.5f, 0.5f));
			this.m_drawhitBox.color = new Color(1f, 1f, 1f, 1f);
			this.modifiedPixels.Clear();
			this.LastNumModifiedPixelsDrawn = 0;
		}
		else
		{
			foreach (KeyValuePair<Vector2, bool> keyValuePair in this.modifiedPixels)
			{
				this.theTexture.SetPixel((int)keyValuePair.Key.x, (int)keyValuePair.Key.y, this.m_clearColor);
			}
			this.theTexture.Apply();
			this.m_drawhitBox.sprite = Sprite.Create(this.theTexture, new Rect(0f, 0f, (float)this.theTexture.width, (float)this.theTexture.height), new Vector2(0.5f, 0.5f));
			this.m_drawhitBox.color = new Color(1f, 1f, 1f, 1f);
			this.modifiedPixels.Clear();
			this.LastNumModifiedPixelsDrawn = 0;
		}
	}

	private void OnDestroy()
	{
		UnityEngine.Object.Destroy(this.theTexture);
	}

	private void PadInMidpointsPoints(Vector2 previousPoint, Vector2 newPoint)
	{
		if (Vector2.Distance(previousPoint, newPoint) < (float)this.LineSize)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISpectatorPenPanel.PadInMidpointsPoints(Vector2, Vector2)).MethodHandle;
			}
			return;
		}
		Vector2 vector = (previousPoint + newPoint) * 0.5f;
		int num = (int)vector.x - this.LineSize;
		while ((float)num < vector.x + (float)this.LineSize)
		{
			int num2 = (int)vector.y - this.LineSize;
			while ((float)num2 < vector.y + (float)this.LineSize)
			{
				Vector2 vector2 = new Vector2((float)num, (float)num2);
				if (0f <= (float)num)
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
					if (num < this.theTexture.width)
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
						if (0f <= (float)num2)
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
							if (num2 < this.theTexture.height && Vector2.Distance(vector2, vector) < (float)this.LineSize)
							{
								this.theTexture.SetPixel(num, num2, this.m_drawColor);
								this.modifiedPixels[vector2] = true;
							}
						}
					}
				}
				num2++;
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
			num++;
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
		this.PadInMidpointsPoints(previousPoint, vector);
		this.PadInMidpointsPoints(vector, newPoint);
	}

	private void CheckRedrawTimeInterval()
	{
		this.RedrawTimeInterval = Mathf.Max(Time.unscaledDeltaTime, this.RedrawTimeInterval);
	}

	private void Update()
	{
		if (this.screenWidth == Screen.width)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISpectatorPenPanel.Update()).MethodHandle;
			}
			if (this.screenHeight == Screen.height)
			{
				goto IL_41;
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
		this.ResizeTexture();
		IL_41:
		bool flag;
		if (ClientGameManager.Get() != null)
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
			if (ClientGameManager.Get().PlayerInfo != null)
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
				if (ClientGameManager.Get().PlayerInfo.TeamId == Team.Spectator)
				{
					flag = true;
					goto IL_D7;
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
		}
		if (GameManager.Get() != null && GameManager.Get().PlayerInfo != null)
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
			flag = (GameManager.Get().PlayerInfo.TeamId == Team.Spectator);
		}
		else
		{
			flag = false;
		}
		IL_D7:
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
			UIManager.SetGameObjectActive(this.m_container, true, null);
			if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.ToggleSpectatorDrawMode))
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
				this.DrawModeClicked(null);
			}
			if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.ClearSpectatorDraw))
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
				this.ClearDrawing(false);
			}
			if (this.m_isDrawing)
			{
				if (Input.GetMouseButton(0))
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
					if (!UIUtils.IsMouseOnGUI())
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
						Vector2 vector = Input.mousePosition * 1f;
						if (0f <= vector.x)
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
							if (vector.x < (float)this.theTexture.width)
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
								if (0f <= vector.y * 1f)
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
									if (vector.y < (float)this.theTexture.height)
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
										int num = (int)vector.x;
										int num2 = (int)vector.y;
										if (this.LastXPos != -1)
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
											if (this.LastYPos != -1)
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
												this.CheckRedrawTimeInterval();
												this.PadInMidpointsPoints(new Vector2((float)this.LastXPos, (float)this.LastYPos), new Vector2((float)num, (float)num2));
											}
										}
										for (int i = num - this.LineSize; i < num + this.LineSize; i++)
										{
											for (int j = num2 - this.LineSize; j < num2 + this.LineSize; j++)
											{
												if (0f <= (float)i)
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
													if (i < this.theTexture.width && 0f <= (float)j)
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
														if (j < this.theTexture.height)
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
															Vector2 vector2 = new Vector2((float)i, (float)j);
															if (Vector2.Distance(vector2, new Vector2((float)num, (float)num2)) < (float)this.LineSize)
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
																this.theTexture.SetPixel(i, j, this.m_drawColor);
																this.modifiedPixels[vector2] = true;
															}
														}
													}
												}
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
										if (this.modifiedPixels.Count - this.LastNumModifiedPixelsDrawn > this.PixelCountRedrawAmount && Time.time > this.LastRedrawTime + this.RedrawTimeInterval)
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
											this.LastRedrawTime = Time.time;
											this.LastNumModifiedPixelsDrawn = this.modifiedPixels.Count;
											this.theTexture.Apply();
											this.m_drawhitBox.sprite = Sprite.Create(this.theTexture, new Rect(0f, 0f, (float)this.theTexture.width, (float)this.theTexture.height), new Vector2(0.5f, 0.5f));
										}
										this.LastXPos = num;
										this.LastYPos = num2;
									}
								}
							}
						}
					}
				}
				if (Input.GetMouseButtonUp(0))
				{
					this.LastNumModifiedPixelsDrawn = this.modifiedPixels.Count;
					this.LastXPos = -1;
					this.LastYPos = -1;
					this.theTexture.Apply();
					this.m_drawhitBox.sprite = Sprite.Create(this.theTexture, new Rect(0f, 0f, (float)this.theTexture.width, (float)this.theTexture.height), new Vector2(0.5f, 0.5f));
				}
			}
		}
	}
}
