using System;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar
{
	private Rect m_rect;

	private float m_minValue;

	private float m_maxValue;

	private bool m_horizontal;

	private bool m_shrinkInward;

	private float m_colorShiftValue;

	private float m_currentValue;

	private bool m_visible;

	private string m_progressText;

	private Color m_barColor;

	private Color m_colorShiftColor;

	private Color m_backgroundColor;

	private List<Texture2D> m_textures;

	private bool m_hasBorder;

	private bool m_hasMarkerBorder;

	private int borderSize = 1;

	public ProgressBar(Rect rect, float minValue, float maxValue)
	{
		this.m_rect = rect;
		this.m_minValue = minValue;
		this.m_maxValue = maxValue;
		this.m_currentValue = this.m_maxValue;
		this.m_horizontal = false;
		this.m_visible = true;
		this.m_barColor = Color.green;
		this.m_backgroundColor = Color.white;
		this.m_textures = new List<Texture2D>();
		this.m_hasBorder = false;
		this.m_hasMarkerBorder = false;
	}

	public Rect rect
	{
		get
		{
			return this.m_rect;
		}
		set
		{
			this.m_rect = value;
		}
	}

	public float MinValue
	{
		get
		{
			return this.m_minValue;
		}
		set
		{
			this.m_minValue = value;
		}
	}

	public float MaxValue
	{
		get
		{
			return this.m_maxValue;
		}
		set
		{
			this.m_maxValue = value;
		}
	}

	public bool Horizontal
	{
		get
		{
			return this.m_horizontal;
		}
		set
		{
			this.m_horizontal = value;
		}
	}

	public bool ShrinkInward
	{
		get
		{
			return this.m_shrinkInward;
		}
		set
		{
			this.m_shrinkInward = value;
		}
	}

	public float ColorShiftValue
	{
		get
		{
			return this.m_colorShiftValue;
		}
		set
		{
			this.m_colorShiftValue = value;
		}
	}

	public float CurrentValue
	{
		get
		{
			return this.m_currentValue;
		}
		set
		{
			this.m_currentValue = value;
		}
	}

	public bool Visible
	{
		get
		{
			return this.m_visible;
		}
		set
		{
			this.m_visible = value;
		}
	}

	public string ProgressText
	{
		get
		{
			return this.m_progressText;
		}
		set
		{
			this.m_progressText = value;
		}
	}

	public Color BarColor
	{
		get
		{
			return this.m_barColor;
		}
		set
		{
			this.m_barColor = value;
		}
	}

	public Color ShiftColor
	{
		get
		{
			return this.m_colorShiftColor;
		}
		set
		{
			this.m_colorShiftColor = value;
		}
	}

	public Color BackgroundColor
	{
		get
		{
			return this.m_backgroundColor;
		}
		set
		{
			this.m_backgroundColor = value;
		}
	}

	public bool HasBorder
	{
		get
		{
			return this.m_hasBorder;
		}
		set
		{
			this.m_hasBorder = value;
		}
	}

	public bool HasMarkerBorder
	{
		get
		{
			return this.m_hasMarkerBorder;
		}
		set
		{
			this.m_hasMarkerBorder = value;
		}
	}

	private GUIStyle CreateColorStyle(Color styleColor)
	{
		Texture2D texture2D = new Texture2D(1, 1);
		texture2D.SetPixel(1, 1, styleColor);
		texture2D.wrapMode = TextureWrapMode.Repeat;
		texture2D.Apply();
		this.m_textures.Add(texture2D);
		return new GUIStyle
		{
			normal = 
			{
				background = texture2D
			},
			alignment = TextAnchor.MiddleCenter,
			fontSize = 0x18
		};
	}

	private void ClearTextures()
	{
		using (List<Texture2D>.Enumerator enumerator = this.m_textures.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Texture2D obj = enumerator.Current;
				UnityEngine.Object.DestroyImmediate(obj);
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ProgressBar.ClearTextures()).MethodHandle;
			}
		}
		this.m_textures.Clear();
	}

	public void Draw()
	{
		this.ClearTextures();
		if (this.m_visible)
		{
			GUI.depth = 0xA;
			GUI.Box(this.m_rect, string.Empty, this.CreateColorStyle(this.m_backgroundColor));
			float num = this.m_currentValue / (this.m_maxValue - this.m_minValue);
			num = Mathf.Clamp(num, 0f, this.m_maxValue);
			float num2 = 1f - num;
			Color styleColor = this.m_barColor;
			if (this.m_currentValue < this.m_colorShiftValue)
			{
				styleColor = this.m_colorShiftColor;
			}
			if (this.m_horizontal)
			{
				int num3 = (int)(this.m_rect.width * num);
				Rect position;
				if (this.m_shrinkInward)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(ProgressBar.Draw()).MethodHandle;
					}
					int num4 = (int)(this.m_rect.width * num2 / 2f);
					position = new Rect(this.m_rect.x + (float)num4, this.m_rect.y, (float)num3, this.m_rect.height);
				}
				else
				{
					position = new Rect(this.m_rect.x, this.m_rect.y, (float)num3, this.m_rect.height);
				}
				GUI.Box(position, string.Empty, this.CreateColorStyle(styleColor));
			}
			else
			{
				int num5 = (int)(this.m_rect.height * num);
				Rect position2;
				if (this.m_shrinkInward)
				{
					int num6 = (int)(this.m_rect.height * num2 / 2f);
					position2 = new Rect(this.m_rect.x, this.m_rect.y + (float)num6, this.m_rect.width, (float)num5);
				}
				else
				{
					position2 = new Rect(this.m_rect.x, this.m_rect.y + (this.m_rect.height - (float)num5), this.m_rect.width, (float)num5);
				}
				GUI.Box(position2, string.Empty, this.CreateColorStyle(styleColor));
			}
			Color styleColor2 = new Color(1f, 1f, 1f, 0f);
			GUI.Box(this.m_rect, this.m_progressText, this.CreateColorStyle(styleColor2));
			if (this.m_hasBorder)
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
				Rect position3 = new Rect(this.m_rect.x - (float)this.borderSize, this.m_rect.y - (float)this.borderSize, this.m_rect.width + (float)(2 * this.borderSize), (float)this.borderSize);
				GUI.Box(position3, string.Empty, this.CreateColorStyle(Color.black));
				position3 = new Rect(this.m_rect.x - (float)this.borderSize, this.m_rect.y - (float)this.borderSize, (float)this.borderSize, this.m_rect.height + (float)(2 * this.borderSize));
				GUI.Box(position3, string.Empty, this.CreateColorStyle(Color.black));
				position3 = new Rect(this.m_rect.x - (float)this.borderSize, this.m_rect.y + this.m_rect.height, this.m_rect.width + (float)(2 * this.borderSize), (float)this.borderSize);
				GUI.Box(position3, string.Empty, this.CreateColorStyle(Color.black));
				position3 = new Rect(this.m_rect.x + this.m_rect.width, this.m_rect.y - (float)this.borderSize, (float)this.borderSize, this.m_rect.height + (float)(2 * this.borderSize));
				GUI.Box(position3, string.Empty, this.CreateColorStyle(Color.black));
			}
			if (this.m_hasMarkerBorder)
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
				float num7 = 1f;
				Color styleColor3 = Color.yellow;
				if (this.m_currentValue / this.m_maxValue <= 0.25f)
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
					num7 = 0.25f;
					styleColor3 = Color.red;
				}
				else if (this.m_currentValue / this.m_maxValue <= 0.5f)
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
					num7 = 0.5f;
					styleColor3 = Color.red;
				}
				if (num7 != 1f)
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
					if (this.m_horizontal)
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
						Rect position4 = new Rect(this.m_rect.x - (float)(2 * this.borderSize), this.m_rect.y - (float)(2 * this.borderSize), this.m_rect.width * num7 + (float)(2 * this.borderSize), (float)this.borderSize);
						GUI.Box(position4, string.Empty, this.CreateColorStyle(styleColor3));
						position4 = new Rect(this.m_rect.x - (float)(2 * this.borderSize), this.m_rect.y - (float)(2 * this.borderSize), (float)this.borderSize, this.m_rect.height + (float)(4 * this.borderSize));
						GUI.Box(position4, string.Empty, this.CreateColorStyle(styleColor3));
						position4 = new Rect(this.m_rect.x - (float)(2 * this.borderSize), this.m_rect.y + this.m_rect.height + (float)this.borderSize, this.m_rect.width * num7 + (float)(2 * this.borderSize), (float)this.borderSize);
						GUI.Box(position4, string.Empty, this.CreateColorStyle(styleColor3));
					}
					else
					{
						Rect position5 = new Rect(this.m_rect.x - (float)(2 * this.borderSize), this.m_rect.y + this.m_rect.height * (1f - num7) + (float)(2 * this.borderSize), (float)this.borderSize, this.m_rect.height * num7);
						GUI.Box(position5, string.Empty, this.CreateColorStyle(styleColor3));
						position5 = new Rect(this.m_rect.x + this.m_rect.width + (float)this.borderSize, this.m_rect.y + this.m_rect.height * (1f - num7) + (float)(2 * this.borderSize), (float)this.borderSize, this.m_rect.height * num7);
						GUI.Box(position5, string.Empty, this.CreateColorStyle(styleColor3));
						position5 = new Rect(this.m_rect.x - (float)(2 * this.borderSize), this.m_rect.y + this.m_rect.height + (float)this.borderSize, this.m_rect.width + (float)(4 * this.borderSize), (float)this.borderSize);
						GUI.Box(position5, string.Empty, this.CreateColorStyle(styleColor3));
					}
				}
			}
		}
	}
}
