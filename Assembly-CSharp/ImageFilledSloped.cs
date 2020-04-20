using System;
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
			return this.m_FillSlope;
		}
		set
		{
			float num = Mathf.Clamp(value, 0.05f, 10f);
			if (num != this.m_FillSlope)
			{
				this.m_FillSlope = num;
				this.SetVerticesDirty();
			}
		}
	}

	public float fillStart
	{
		get
		{
			return this.m_FillStart;
		}
		set
		{
			float num = Mathf.Clamp(value, 0f, 1f);
			if (num != this.m_FillStart)
			{
				this.m_FillStart = num;
				this.SetVerticesDirty();
			}
		}
	}

	public float fillAmountTrimmed
	{
		get
		{
			return Mathf.Lerp(this.m_FillMin, this.m_FillMax, base.fillAmount);
		}
	}

	public float fillStartTrimmed
	{
		get
		{
			return Mathf.Lerp(this.m_FillMin, this.m_FillMax, this.fillStart);
		}
	}

	protected override void OnPopulateMesh(VertexHelper toFill)
	{
		if (base.overrideSprite == null)
		{
			base.OnPopulateMesh(toFill);
			return;
		}
		Image.Type type = base.type;
		if (type != Image.Type.Simple)
		{
			if (type == Image.Type.Filled)
			{
				this.GenerateFilledSprite(toFill, base.preserveAspect);
			}
		}
		else
		{
			Debug.LogError(string.Format("{0}: Image Filled Sloped only supports Image Type: Filled", this.ToString()));
		}
	}

	private void GenerateFilledSprite(VertexHelper toFill, bool preserveAspect)
	{
		if (this.fillAmountTrimmed < 0.001f)
		{
			toFill.Clear();
			return;
		}
		UIVertex simpleVert = UIVertex.simpleVert;
		simpleVert.color = this.color;
		Vector4 drawingDimensions;
		Vector4 vector;
		if (this.m_IncludeTransparency)
		{
			Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
			drawingDimensions = new Vector4(pixelAdjustedRect.x, pixelAdjustedRect.y, pixelAdjustedRect.x + pixelAdjustedRect.width, pixelAdjustedRect.y + pixelAdjustedRect.height);
			vector = new Vector4(0f, 0f, 1f, 1f);
		}
		else
		{
			drawingDimensions = this.GetDrawingDimensions(preserveAspect);
			Vector4 vector2;
			if (base.overrideSprite != null)
			{
				vector2 = DataUtility.GetOuterUV(base.overrideSprite);
			}
			else
			{
				vector2 = Vector4.zero;
			}
			vector = vector2;
		}
		float num = vector.x;
		float num2 = vector.y;
		float num3 = vector.z;
		float num4 = vector.w;
		float num5 = drawingDimensions.x;
		float num6 = drawingDimensions.y;
		if (base.fillMethod != Image.FillMethod.Horizontal)
		{
			if (base.fillMethod != Image.FillMethod.Vertical)
			{
				goto IL_ABF;
			}
		}
		if (base.fillMethod == Image.FillMethod.Horizontal)
		{
			if (Mathf.Approximately(this.fillSlope, 0f))
			{
				this.fillSlope = 0.1f;
			}
			if (base.fillOrigin == 1)
			{
				float num7 = (drawingDimensions.w - drawingDimensions.y) / this.fillSlope;
				float x = drawingDimensions.x;
				float num8 = x - num7 + (1f - this.fillAmountTrimmed) * (drawingDimensions.z - x + num7);
				num5 = Mathf.Min(drawingDimensions.z, num8 + num7);
				drawingDimensions.x = num8;
				num6 = Mathf.Min(drawingDimensions.w, drawingDimensions.y + this.fillSlope * (drawingDimensions.z - drawingDimensions.x));
				float num9 = drawingDimensions.z - x;
				float num10 = num3 - num;
				num = num3 - num10 * (drawingDimensions.z - drawingDimensions.x) / num9;
				float x2 = num3 + num10 * (num5 - drawingDimensions.z) / num9;
				float y = num2 + (num4 - num2) * (num6 - drawingDimensions.y) / (drawingDimensions.w - drawingDimensions.y);
				drawingDimensions.z -= this.fillStartTrimmed * num9;
				num3 -= this.fillStartTrimmed * num10;
				if (num5 < drawingDimensions.z)
				{
					float x3 = drawingDimensions.z - ((!this.m_StartIsSloped) ? 0f : num7);
					ImageFilledSloped.s_Xy[0] = new Vector2(drawingDimensions.x, drawingDimensions.y);
					ImageFilledSloped.s_Xy[1] = new Vector2(num5, drawingDimensions.w);
					ImageFilledSloped.s_Xy[2] = new Vector2(drawingDimensions.z, drawingDimensions.w);
					ImageFilledSloped.s_Xy[3] = new Vector2(x3, drawingDimensions.y);
					float num11 = num3;
					float num12;
					if (this.m_StartIsSloped)
					{
						num12 = num7 / num9 * num10;
					}
					else
					{
						num12 = 0f;
					}
					x3 = num11 - num12;
					ImageFilledSloped.s_Uv[0] = new Vector2(num, num2);
					ImageFilledSloped.s_Uv[1] = new Vector2(x2, num4);
					ImageFilledSloped.s_Uv[2] = new Vector2(num3, num4);
					ImageFilledSloped.s_Uv[3] = new Vector2(x3, num2);
				}
				else
				{
					ImageFilledSloped.s_Xy[0] = new Vector2(drawingDimensions.x, drawingDimensions.y);
					ImageFilledSloped.s_Xy[1] = new Vector2(drawingDimensions.z, num6);
					ImageFilledSloped.s_Xy[2] = new Vector2(drawingDimensions.z, drawingDimensions.y);
					ImageFilledSloped.s_Xy[3] = new Vector2(drawingDimensions.z, drawingDimensions.y);
					ImageFilledSloped.s_Uv[0] = new Vector2(num, num2);
					ImageFilledSloped.s_Uv[1] = new Vector2(num3, y);
					ImageFilledSloped.s_Uv[2] = new Vector2(num3, num2);
					ImageFilledSloped.s_Uv[3] = new Vector2(num3, num2);
				}
			}
			else
			{
				float num13 = (drawingDimensions.w - drawingDimensions.y) / this.fillSlope;
				float z = drawingDimensions.z;
				float num14 = drawingDimensions.x - num13 + this.fillAmountTrimmed * (z - drawingDimensions.x + num13);
				num5 = Mathf.Max(drawingDimensions.x, num14);
				drawingDimensions.z = num14 + num13;
				num6 = Mathf.Min(drawingDimensions.w, drawingDimensions.w - this.fillSlope * (drawingDimensions.z - drawingDimensions.x));
				float num15 = z - drawingDimensions.x;
				float num16 = num3 - num;
				num3 = num + num16 * (drawingDimensions.z - drawingDimensions.x) / num15;
				float x2 = num + num16 * (num5 - drawingDimensions.x) / num15;
				float y = num2 + (num4 - num2) * (num6 - drawingDimensions.y) / (drawingDimensions.w - drawingDimensions.y);
				drawingDimensions.x += this.fillStartTrimmed * num15 - num13;
				num += this.fillStartTrimmed * num16 - num13 / num15 * num16;
				if (num5 > drawingDimensions.x)
				{
					float x4 = drawingDimensions.x;
					float num17;
					if (this.m_StartIsSloped)
					{
						num17 = num13;
					}
					else
					{
						num17 = 0f;
					}
					float x5 = x4 + num17;
					ImageFilledSloped.s_Xy[0] = new Vector2(drawingDimensions.x, drawingDimensions.y);
					ImageFilledSloped.s_Xy[1] = new Vector2(x5, drawingDimensions.w);
					ImageFilledSloped.s_Xy[2] = new Vector2(drawingDimensions.z, drawingDimensions.w);
					ImageFilledSloped.s_Xy[3] = new Vector2(num5, drawingDimensions.y);
					x5 = num + ((!this.m_StartIsSloped) ? 0f : (num13 / num15 * num16));
					ImageFilledSloped.s_Uv[0] = new Vector2(num, num2);
					ImageFilledSloped.s_Uv[1] = new Vector2(x5, num4);
					ImageFilledSloped.s_Uv[2] = new Vector2(num3, num4);
					ImageFilledSloped.s_Uv[3] = new Vector2(x2, num2);
				}
				else
				{
					ImageFilledSloped.s_Xy[0] = new Vector2(drawingDimensions.x, num6);
					ImageFilledSloped.s_Xy[1] = new Vector2(drawingDimensions.x, drawingDimensions.w);
					ImageFilledSloped.s_Xy[2] = new Vector2(drawingDimensions.z, drawingDimensions.w);
					ImageFilledSloped.s_Xy[3] = new Vector2(drawingDimensions.z, drawingDimensions.w);
					ImageFilledSloped.s_Uv[0] = new Vector2(num, y);
					ImageFilledSloped.s_Uv[1] = new Vector2(num, num4);
					ImageFilledSloped.s_Uv[2] = new Vector2(num3, num4);
					ImageFilledSloped.s_Uv[3] = new Vector2(num3, num4);
				}
			}
		}
		else if (base.fillMethod == Image.FillMethod.Vertical)
		{
			float num18 = (num4 - num2) * this.fillAmountTrimmed;
			if (base.fillOrigin == 1)
			{
				drawingDimensions.y = drawingDimensions.w - (drawingDimensions.w - drawingDimensions.y) * this.fillAmountTrimmed;
				num2 = num4 - num18;
			}
			else
			{
				drawingDimensions.w = drawingDimensions.y + (drawingDimensions.w - drawingDimensions.y) * this.fillAmountTrimmed;
				num4 = num2 + num18;
			}
			ImageFilledSloped.s_Xy[0] = new Vector2(drawingDimensions.x, drawingDimensions.y);
			ImageFilledSloped.s_Xy[1] = new Vector2(drawingDimensions.x, drawingDimensions.w);
			ImageFilledSloped.s_Xy[2] = new Vector2(drawingDimensions.z, drawingDimensions.w);
			ImageFilledSloped.s_Xy[3] = new Vector2(drawingDimensions.z, drawingDimensions.y);
			ImageFilledSloped.s_Uv[0] = new Vector2(num, num2);
			ImageFilledSloped.s_Uv[1] = new Vector2(num, num4);
			ImageFilledSloped.s_Uv[2] = new Vector2(num3, num4);
			ImageFilledSloped.s_Uv[3] = new Vector2(num3, num2);
		}
		IL_ABF:
		if (toFill.currentVertCount == 4 && toFill.currentIndexCount == 6)
		{
			ImageFilledSloped.SetQuad(toFill, ImageFilledSloped.s_Xy, this.color, ImageFilledSloped.s_Uv);
		}
		else
		{
			toFill.Clear();
			ImageFilledSloped.AddQuad(toFill, ImageFilledSloped.s_Xy, this.color, ImageFilledSloped.s_Uv);
		}
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
			vector3 = Vector2.zero;
		}
		else
		{
			vector3 = new Vector2(base.overrideSprite.rect.width, base.overrideSprite.rect.height);
		}
		Vector2 vector4 = vector3;
		Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
		int num = Mathf.RoundToInt(vector4.x);
		int num2 = Mathf.RoundToInt(vector4.y);
		Vector4 result = new Vector4(vector2.x / (float)num, vector2.y / (float)num2, ((float)num - vector2.z) / (float)num, ((float)num2 - vector2.w) / (float)num2);
		if (shouldPreserveAspect)
		{
			if (vector4.sqrMagnitude > 0f)
			{
				float num3 = vector4.x / vector4.y;
				float num4 = pixelAdjustedRect.width / pixelAdjustedRect.height;
				if (num3 > num4)
				{
					float height = pixelAdjustedRect.height;
					pixelAdjustedRect.height = pixelAdjustedRect.width * (1f / num3);
					pixelAdjustedRect.y += (height - pixelAdjustedRect.height) * base.rectTransform.pivot.y;
				}
				else
				{
					float width = pixelAdjustedRect.width;
					pixelAdjustedRect.width = pixelAdjustedRect.height * num3;
					pixelAdjustedRect.x += (width - pixelAdjustedRect.width) * base.rectTransform.pivot.x;
				}
			}
		}
		result = new Vector4(pixelAdjustedRect.x + pixelAdjustedRect.width * result.x, pixelAdjustedRect.y + pixelAdjustedRect.height * result.y, pixelAdjustedRect.x + pixelAdjustedRect.width * result.z, pixelAdjustedRect.y + pixelAdjustedRect.height * result.w);
		return result;
	}
}
