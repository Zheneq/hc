using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

public class ImageFilledSloped : Image
{
	private const float MAX_SLOPE = 10f;

	private const float MIN_SLOPE = 0.05f;

	[SerializeField]
	[Range(0.05f, 10f)]
	private float m_FillSlope = 2.5f;

	[Range(0f, 1f)]
	[SerializeField]
	private float m_FillStart;

	public float m_FillMin = 0.067f;

	public float m_FillMax = 0.941f;

	public bool m_StartIsSloped;

	public bool m_IncludeTransparency;

	private static readonly Vector3[] s_Xy = new Vector3[4];

	private static readonly Vector3[] s_Uv = new Vector3[4];

	public float fillSlope
	{
		get
		{
			return m_FillSlope;
		}
		set
		{
			float num = Mathf.Clamp(value, 0.05f, 10f);
			if (num != m_FillSlope)
			{
				m_FillSlope = num;
				SetVerticesDirty();
			}
		}
	}

	public float fillStart
	{
		get
		{
			return m_FillStart;
		}
		set
		{
			float num = Mathf.Clamp(value, 0f, 1f);
			if (num != m_FillStart)
			{
				m_FillStart = num;
				SetVerticesDirty();
			}
		}
	}

	public float fillAmountTrimmed => Mathf.Lerp(m_FillMin, m_FillMax, base.fillAmount);

	public float fillStartTrimmed => Mathf.Lerp(m_FillMin, m_FillMax, fillStart);

	protected override void OnPopulateMesh(VertexHelper toFill)
	{
		if (base.overrideSprite == null)
		{
			base.OnPopulateMesh(toFill);
			return;
		}
		Type type = base.type;
		if (type != 0)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (type == Type.Filled)
					{
						GenerateFilledSprite(toFill, base.preserveAspect);
					}
					return;
				}
			}
		}
		Debug.LogError($"{ToString()}: Image Filled Sloped only supports Image Type: Filled");
	}

	private void GenerateFilledSprite(VertexHelper toFill, bool preserveAspect)
	{
		if (fillAmountTrimmed < 0.001f)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				toFill.Clear();
				return;
			}
		}
		UIVertex simpleVert = UIVertex.simpleVert;
		simpleVert.color = color;
		Vector4 vector;
		Vector4 vector2;
		if (m_IncludeTransparency)
		{
			Rect pixelAdjustedRect = GetPixelAdjustedRect();
			vector = new Vector4(pixelAdjustedRect.x, pixelAdjustedRect.y, pixelAdjustedRect.x + pixelAdjustedRect.width, pixelAdjustedRect.y + pixelAdjustedRect.height);
			vector2 = new Vector4(0f, 0f, 1f, 1f);
		}
		else
		{
			vector = GetDrawingDimensions(preserveAspect);
			Vector4 vector3;
			if (base.overrideSprite != null)
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
				vector3 = DataUtility.GetOuterUV(base.overrideSprite);
			}
			else
			{
				vector3 = Vector4.zero;
			}
			vector2 = vector3;
		}
		float x = vector2.x;
		float num = vector2.y;
		float z = vector2.z;
		float num2 = vector2.w;
		float x2 = vector.x;
		float y = vector.y;
		float num3 = x;
		float num4 = num;
		if (base.fillMethod != 0)
		{
			if (base.fillMethod != FillMethod.Vertical)
			{
				goto IL_0abf;
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
		if (base.fillMethod == FillMethod.Horizontal)
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
			if (Mathf.Approximately(fillSlope, 0f))
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
				fillSlope = 0.1f;
			}
			if (base.fillOrigin == 1)
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
				float num5 = (vector.w - vector.y) / fillSlope;
				float x3 = vector.x;
				float num6 = x3 - num5 + (1f - fillAmountTrimmed) * (vector.z - x3 + num5);
				x2 = Mathf.Min(vector.z, num6 + num5);
				vector.x = num6;
				y = Mathf.Min(vector.w, vector.y + fillSlope * (vector.z - vector.x));
				float num7 = vector.z - x3;
				float num8 = z - x;
				x = z - num8 * (vector.z - vector.x) / num7;
				num3 = z + num8 * (x2 - vector.z) / num7;
				num4 = num + (num2 - num) * (y - vector.y) / (vector.w - vector.y);
				vector.z -= fillStartTrimmed * num7;
				z -= fillStartTrimmed * num8;
				if (x2 < vector.z)
				{
					float x4 = vector.z - ((!m_StartIsSloped) ? 0f : num5);
					s_Xy[0] = new Vector2(vector.x, vector.y);
					s_Xy[1] = new Vector2(x2, vector.w);
					s_Xy[2] = new Vector2(vector.z, vector.w);
					s_Xy[3] = new Vector2(x4, vector.y);
					float num9 = z;
					float num10;
					if (m_StartIsSloped)
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
						num10 = num5 / num7 * num8;
					}
					else
					{
						num10 = 0f;
					}
					x4 = num9 - num10;
					s_Uv[0] = new Vector2(x, num);
					s_Uv[1] = new Vector2(num3, num2);
					s_Uv[2] = new Vector2(z, num2);
					s_Uv[3] = new Vector2(x4, num);
				}
				else
				{
					s_Xy[0] = new Vector2(vector.x, vector.y);
					s_Xy[1] = new Vector2(vector.z, y);
					s_Xy[2] = new Vector2(vector.z, vector.y);
					s_Xy[3] = new Vector2(vector.z, vector.y);
					s_Uv[0] = new Vector2(x, num);
					s_Uv[1] = new Vector2(z, num4);
					s_Uv[2] = new Vector2(z, num);
					s_Uv[3] = new Vector2(z, num);
				}
			}
			else
			{
				float num11 = (vector.w - vector.y) / fillSlope;
				float z2 = vector.z;
				float num12 = vector.x - num11 + fillAmountTrimmed * (z2 - vector.x + num11);
				x2 = Mathf.Max(vector.x, num12);
				vector.z = num12 + num11;
				y = Mathf.Min(vector.w, vector.w - fillSlope * (vector.z - vector.x));
				float num13 = z2 - vector.x;
				float num14 = z - x;
				z = x + num14 * (vector.z - vector.x) / num13;
				num3 = x + num14 * (x2 - vector.x) / num13;
				num4 = num + (num2 - num) * (y - vector.y) / (vector.w - vector.y);
				vector.x += fillStartTrimmed * num13 - num11;
				x += fillStartTrimmed * num14 - num11 / num13 * num14;
				if (x2 > vector.x)
				{
					float x5 = vector.x;
					float num15;
					if (m_StartIsSloped)
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
						num15 = num11;
					}
					else
					{
						num15 = 0f;
					}
					float x6 = x5 + num15;
					s_Xy[0] = new Vector2(vector.x, vector.y);
					s_Xy[1] = new Vector2(x6, vector.w);
					s_Xy[2] = new Vector2(vector.z, vector.w);
					s_Xy[3] = new Vector2(x2, vector.y);
					x6 = x + ((!m_StartIsSloped) ? 0f : (num11 / num13 * num14));
					s_Uv[0] = new Vector2(x, num);
					s_Uv[1] = new Vector2(x6, num2);
					s_Uv[2] = new Vector2(z, num2);
					s_Uv[3] = new Vector2(num3, num);
				}
				else
				{
					s_Xy[0] = new Vector2(vector.x, y);
					s_Xy[1] = new Vector2(vector.x, vector.w);
					s_Xy[2] = new Vector2(vector.z, vector.w);
					s_Xy[3] = new Vector2(vector.z, vector.w);
					s_Uv[0] = new Vector2(x, num4);
					s_Uv[1] = new Vector2(x, num2);
					s_Uv[2] = new Vector2(z, num2);
					s_Uv[3] = new Vector2(z, num2);
				}
			}
		}
		else if (base.fillMethod == FillMethod.Vertical)
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
			float num16 = (num2 - num) * fillAmountTrimmed;
			if (base.fillOrigin == 1)
			{
				vector.y = vector.w - (vector.w - vector.y) * fillAmountTrimmed;
				num = num2 - num16;
			}
			else
			{
				vector.w = vector.y + (vector.w - vector.y) * fillAmountTrimmed;
				num2 = num + num16;
			}
			s_Xy[0] = new Vector2(vector.x, vector.y);
			s_Xy[1] = new Vector2(vector.x, vector.w);
			s_Xy[2] = new Vector2(vector.z, vector.w);
			s_Xy[3] = new Vector2(vector.z, vector.y);
			s_Uv[0] = new Vector2(x, num);
			s_Uv[1] = new Vector2(x, num2);
			s_Uv[2] = new Vector2(z, num2);
			s_Uv[3] = new Vector2(z, num);
		}
		goto IL_0abf;
		IL_0abf:
		if (toFill.currentVertCount == 4 && toFill.currentIndexCount == 6)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					SetQuad(toFill, s_Xy, color, s_Uv);
					return;
				}
			}
		}
		toFill.Clear();
		AddQuad(toFill, s_Xy, color, s_Uv);
	}

	private static void AddQuad(VertexHelper vertexHelper, Vector3[] quadPositions, Color32 color, Vector3[] quadUVs)
	{
		int currentVertCount = vertexHelper.currentVertCount;
		for (int i = 0; i < 4; i++)
		{
			vertexHelper.AddVert(quadPositions[i], color, quadUVs[i]);
		}
		vertexHelper.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
		vertexHelper.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
	}

	private static void SetQuad(VertexHelper vertexHelper, Vector3[] quadPositions, Color32 color, Vector3[] quadUVs)
	{
		UIVertex simpleVert = UIVertex.simpleVert;
		for (int i = 0; i < 4; i++)
		{
			simpleVert.position = quadPositions[i];
			simpleVert.color = color;
			simpleVert.uv0 = quadUVs[i];
			vertexHelper.SetUIVertex(simpleVert, i);
		}
	}

	private Vector4 GetDrawingDimensions(bool shouldPreserveAspect)
	{
		Vector4 vector;
		if (base.overrideSprite == null)
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
			vector = Vector4.zero;
		}
		else
		{
			vector = DataUtility.GetPadding(base.overrideSprite);
		}
		Vector4 vector2 = vector;
		Vector2 vector3;
		if (base.overrideSprite == null)
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
			vector3 = Vector2.zero;
		}
		else
		{
			vector3 = new Vector2(base.overrideSprite.rect.width, base.overrideSprite.rect.height);
		}
		Vector2 vector4 = vector3;
		Rect pixelAdjustedRect = GetPixelAdjustedRect();
		int num = Mathf.RoundToInt(vector4.x);
		int num2 = Mathf.RoundToInt(vector4.y);
		Vector4 vector5 = new Vector4(vector2.x / (float)num, vector2.y / (float)num2, ((float)num - vector2.z) / (float)num, ((float)num2 - vector2.w) / (float)num2);
		if (shouldPreserveAspect)
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
			if (vector4.sqrMagnitude > 0f)
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
				float num3 = vector4.x / vector4.y;
				float num4 = pixelAdjustedRect.width / pixelAdjustedRect.height;
				if (num3 > num4)
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
					float height = pixelAdjustedRect.height;
					pixelAdjustedRect.height = pixelAdjustedRect.width * (1f / num3);
					float y = pixelAdjustedRect.y;
					float num5 = height - pixelAdjustedRect.height;
					Vector2 pivot = base.rectTransform.pivot;
					pixelAdjustedRect.y = y + num5 * pivot.y;
				}
				else
				{
					float width = pixelAdjustedRect.width;
					pixelAdjustedRect.width = pixelAdjustedRect.height * num3;
					float x = pixelAdjustedRect.x;
					float num6 = width - pixelAdjustedRect.width;
					Vector2 pivot2 = base.rectTransform.pivot;
					pixelAdjustedRect.x = x + num6 * pivot2.x;
				}
			}
		}
		return new Vector4(pixelAdjustedRect.x + pixelAdjustedRect.width * vector5.x, pixelAdjustedRect.y + pixelAdjustedRect.height * vector5.y, pixelAdjustedRect.x + pixelAdjustedRect.width * vector5.z, pixelAdjustedRect.y + pixelAdjustedRect.height * vector5.w);
	}
}
