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

	public Rect rect
	{
		get
		{
			return m_rect;
		}
		set
		{
			m_rect = value;
		}
	}

	public float MinValue
	{
		get
		{
			return m_minValue;
		}
		set
		{
			m_minValue = value;
		}
	}

	public float MaxValue
	{
		get
		{
			return m_maxValue;
		}
		set
		{
			m_maxValue = value;
		}
	}

	public bool Horizontal
	{
		get
		{
			return m_horizontal;
		}
		set
		{
			m_horizontal = value;
		}
	}

	public bool ShrinkInward
	{
		get
		{
			return m_shrinkInward;
		}
		set
		{
			m_shrinkInward = value;
		}
	}

	public float ColorShiftValue
	{
		get
		{
			return m_colorShiftValue;
		}
		set
		{
			m_colorShiftValue = value;
		}
	}

	public float CurrentValue
	{
		get
		{
			return m_currentValue;
		}
		set
		{
			m_currentValue = value;
		}
	}

	public bool Visible
	{
		get
		{
			return m_visible;
		}
		set
		{
			m_visible = value;
		}
	}

	public string ProgressText
	{
		get
		{
			return m_progressText;
		}
		set
		{
			m_progressText = value;
		}
	}

	public Color BarColor
	{
		get
		{
			return m_barColor;
		}
		set
		{
			m_barColor = value;
		}
	}

	public Color ShiftColor
	{
		get
		{
			return m_colorShiftColor;
		}
		set
		{
			m_colorShiftColor = value;
		}
	}

	public Color BackgroundColor
	{
		get
		{
			return m_backgroundColor;
		}
		set
		{
			m_backgroundColor = value;
		}
	}

	public bool HasBorder
	{
		get
		{
			return m_hasBorder;
		}
		set
		{
			m_hasBorder = value;
		}
	}

	public bool HasMarkerBorder
	{
		get
		{
			return m_hasMarkerBorder;
		}
		set
		{
			m_hasMarkerBorder = value;
		}
	}

	public ProgressBar(Rect rect, float minValue, float maxValue)
	{
		m_rect = rect;
		m_minValue = minValue;
		m_maxValue = maxValue;
		m_currentValue = m_maxValue;
		m_horizontal = false;
		m_visible = true;
		m_barColor = Color.green;
		m_backgroundColor = Color.white;
		m_textures = new List<Texture2D>();
		m_hasBorder = false;
		m_hasMarkerBorder = false;
	}

	private GUIStyle CreateColorStyle(Color styleColor)
	{
		Texture2D texture2D = new Texture2D(1, 1);
		texture2D.SetPixel(1, 1, styleColor);
		texture2D.wrapMode = TextureWrapMode.Repeat;
		texture2D.Apply();
		m_textures.Add(texture2D);
		GUIStyle gUIStyle = new GUIStyle();
		gUIStyle.normal.background = texture2D;
		gUIStyle.alignment = TextAnchor.MiddleCenter;
		gUIStyle.fontSize = 24;
		return gUIStyle;
	}

	private void ClearTextures()
	{
		using (List<Texture2D>.Enumerator enumerator = m_textures.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Texture2D current = enumerator.Current;
				Object.DestroyImmediate(current);
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					goto end_IL_000e;
				}
			}
			end_IL_000e:;
		}
		m_textures.Clear();
	}

	public void Draw()
	{
		ClearTextures();
		if (!m_visible)
		{
			return;
		}
		GUI.depth = 10;
		GUI.Box(m_rect, string.Empty, CreateColorStyle(m_backgroundColor));
		float value = m_currentValue / (m_maxValue - m_minValue);
		value = Mathf.Clamp(value, 0f, m_maxValue);
		float num = 1f - value;
		Color styleColor = m_barColor;
		if (m_currentValue < m_colorShiftValue)
		{
			styleColor = m_colorShiftColor;
		}
		if (m_horizontal)
		{
			int num2 = (int)(m_rect.width * value);
			Rect position;
			if (!m_shrinkInward)
			{
				position = new Rect(m_rect.x, m_rect.y, num2, m_rect.height);
			}
			else
			{
				int num3 = (int)(m_rect.width * num / 2f);
				position = new Rect(m_rect.x + (float)num3, m_rect.y, num2, m_rect.height);
			}
			GUI.Box(position, string.Empty, CreateColorStyle(styleColor));
		}
		else
		{
			int num4 = (int)(m_rect.height * value);
			Rect position2;
			if (!m_shrinkInward)
			{
				position2 = new Rect(m_rect.x, m_rect.y + (m_rect.height - (float)num4), m_rect.width, num4);
			}
			else
			{
				int num5 = (int)(m_rect.height * num / 2f);
				position2 = new Rect(m_rect.x, m_rect.y + (float)num5, m_rect.width, num4);
			}
			GUI.Box(position2, string.Empty, CreateColorStyle(styleColor));
		}
		GUI.Box(style: CreateColorStyle(new Color(1f, 1f, 1f, 0f)), position: m_rect, text: m_progressText);
		if (m_hasBorder)
		{
			Rect position3 = new Rect(m_rect.x - (float)borderSize, m_rect.y - (float)borderSize, m_rect.width + (float)(2 * borderSize), borderSize);
			GUI.Box(position3, string.Empty, CreateColorStyle(Color.black));
			position3 = new Rect(m_rect.x - (float)borderSize, m_rect.y - (float)borderSize, borderSize, m_rect.height + (float)(2 * borderSize));
			GUI.Box(position3, string.Empty, CreateColorStyle(Color.black));
			position3 = new Rect(m_rect.x - (float)borderSize, m_rect.y + m_rect.height, m_rect.width + (float)(2 * borderSize), borderSize);
			GUI.Box(position3, string.Empty, CreateColorStyle(Color.black));
			position3 = new Rect(m_rect.x + m_rect.width, m_rect.y - (float)borderSize, borderSize, m_rect.height + (float)(2 * borderSize));
			GUI.Box(position3, string.Empty, CreateColorStyle(Color.black));
		}
		if (!m_hasMarkerBorder)
		{
			return;
		}
		while (true)
		{
			float num6 = 1f;
			Color styleColor2 = Color.yellow;
			if (m_currentValue / m_maxValue <= 0.25f)
			{
				num6 = 0.25f;
				styleColor2 = Color.red;
			}
			else if (m_currentValue / m_maxValue <= 0.5f)
			{
				num6 = 0.5f;
				styleColor2 = Color.red;
			}
			if (num6 == 1f)
			{
				return;
			}
			while (true)
			{
				if (m_horizontal)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
						{
							Rect position4 = new Rect(m_rect.x - (float)(2 * borderSize), m_rect.y - (float)(2 * borderSize), m_rect.width * num6 + (float)(2 * borderSize), borderSize);
							GUI.Box(position4, string.Empty, CreateColorStyle(styleColor2));
							position4 = new Rect(m_rect.x - (float)(2 * borderSize), m_rect.y - (float)(2 * borderSize), borderSize, m_rect.height + (float)(4 * borderSize));
							GUI.Box(position4, string.Empty, CreateColorStyle(styleColor2));
							position4 = new Rect(m_rect.x - (float)(2 * borderSize), m_rect.y + m_rect.height + (float)borderSize, m_rect.width * num6 + (float)(2 * borderSize), borderSize);
							GUI.Box(position4, string.Empty, CreateColorStyle(styleColor2));
							return;
						}
						}
					}
				}
				Rect position5 = new Rect(m_rect.x - (float)(2 * borderSize), m_rect.y + m_rect.height * (1f - num6) + (float)(2 * borderSize), borderSize, m_rect.height * num6);
				GUI.Box(position5, string.Empty, CreateColorStyle(styleColor2));
				position5 = new Rect(m_rect.x + m_rect.width + (float)borderSize, m_rect.y + m_rect.height * (1f - num6) + (float)(2 * borderSize), borderSize, m_rect.height * num6);
				GUI.Box(position5, string.Empty, CreateColorStyle(styleColor2));
				position5 = new Rect(m_rect.x - (float)(2 * borderSize), m_rect.y + m_rect.height + (float)borderSize, m_rect.width + (float)(4 * borderSize), borderSize);
				GUI.Box(position5, string.Empty, CreateColorStyle(styleColor2));
				return;
			}
		}
	}
}
