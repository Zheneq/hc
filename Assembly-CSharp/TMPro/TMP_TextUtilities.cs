using System;
using UnityEngine;

namespace TMPro
{
	public static class TMP_TextUtilities
	{
		private static Vector3[] m_rectWorldCorners = new Vector3[4];

		private const string k_lookupStringL = "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[-]^_`abcdefghijklmnopqrstuvwxyz{|}~-";

		private const string k_lookupStringU = "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-";

		public static int GetCursorIndexFromPosition(TMP_Text textComponent, Vector3 position, Camera camera)
		{
			int num = TMP_TextUtilities.FindNearestCharacter(textComponent, position, camera, false);
			RectTransform rectTransform = textComponent.rectTransform;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			TMP_CharacterInfo tmp_CharacterInfo = textComponent.textInfo.characterInfo[num];
			Vector3 vector = rectTransform.TransformPoint(tmp_CharacterInfo.bottomLeft);
			Vector3 vector2 = rectTransform.TransformPoint(tmp_CharacterInfo.topRight);
			float num2 = (position.x - vector.x) / (vector2.x - vector.x);
			if (num2 < 0.5f)
			{
				return num;
			}
			return num + 1;
		}

		public unsafe static int GetCursorIndexFromPosition(TMP_Text textComponent, Vector3 position, Camera camera, out CaretPosition cursor)
		{
			int num = TMP_TextUtilities.FindNearestLine(textComponent, position, camera);
			int num2 = TMP_TextUtilities.FindNearestCharacterOnLine(textComponent, position, num, camera, false);
			if (textComponent.textInfo.lineInfo[num].characterCount == 1)
			{
				cursor = CaretPosition.Left;
				return num2;
			}
			RectTransform rectTransform = textComponent.rectTransform;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			TMP_CharacterInfo tmp_CharacterInfo = textComponent.textInfo.characterInfo[num2];
			Vector3 vector = rectTransform.TransformPoint(tmp_CharacterInfo.bottomLeft);
			Vector3 vector2 = rectTransform.TransformPoint(tmp_CharacterInfo.topRight);
			float num3 = (position.x - vector.x) / (vector2.x - vector.x);
			if (num3 < 0.5f)
			{
				cursor = CaretPosition.Left;
				return num2;
			}
			cursor = CaretPosition.Right;
			return num2;
		}

		public static int FindNearestLine(TMP_Text text, Vector3 position, Camera camera)
		{
			RectTransform rectTransform = text.rectTransform;
			float num = float.PositiveInfinity;
			int result = -1;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			for (int i = 0; i < text.textInfo.lineCount; i++)
			{
				TMP_LineInfo tmp_LineInfo = text.textInfo.lineInfo[i];
				float y = rectTransform.TransformPoint(new Vector3(0f, tmp_LineInfo.ascender, 0f)).y;
				float y2 = rectTransform.TransformPoint(new Vector3(0f, tmp_LineInfo.descender, 0f)).y;
				if (y > position.y)
				{
					if (y2 < position.y)
					{
						return i;
					}
				}
				float a = Mathf.Abs(y - position.y);
				float b = Mathf.Abs(y2 - position.y);
				float num2 = Mathf.Min(a, b);
				if (num2 < num)
				{
					num = num2;
					result = i;
				}
			}
			return result;
		}

		public static int FindNearestCharacterOnLine(TMP_Text text, Vector3 position, int line, Camera camera, bool visibleOnly)
		{
			RectTransform rectTransform = text.rectTransform;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			int firstCharacterIndex = text.textInfo.lineInfo[line].firstCharacterIndex;
			int lastCharacterIndex = text.textInfo.lineInfo[line].lastCharacterIndex;
			float num = float.PositiveInfinity;
			int result = lastCharacterIndex;
			for (int i = firstCharacterIndex; i < lastCharacterIndex; i++)
			{
				TMP_CharacterInfo tmp_CharacterInfo = text.textInfo.characterInfo[i];
				if (visibleOnly && !tmp_CharacterInfo.isVisible)
				{
				}
				else
				{
					Vector3 vector = rectTransform.TransformPoint(tmp_CharacterInfo.bottomLeft);
					Vector3 vector2 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.topRight.y, 0f));
					Vector3 vector3 = rectTransform.TransformPoint(tmp_CharacterInfo.topRight);
					Vector3 vector4 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.bottomLeft.y, 0f));
					if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector3, vector4))
					{
						result = i;
						return result;
					}
					float num2 = TMP_TextUtilities.DistanceToLine(vector, vector2, position);
					float num3 = TMP_TextUtilities.DistanceToLine(vector2, vector3, position);
					float num4 = TMP_TextUtilities.DistanceToLine(vector3, vector4, position);
					float num5 = TMP_TextUtilities.DistanceToLine(vector4, vector, position);
					float num6;
					if (num2 < num3)
					{
						num6 = num2;
					}
					else
					{
						num6 = num3;
					}
					float num7 = num6;
					float num8;
					if (num7 < num4)
					{
						num8 = num7;
					}
					else
					{
						num8 = num4;
					}
					num7 = num8;
					float num9;
					if (num7 < num5)
					{
						num9 = num7;
					}
					else
					{
						num9 = num5;
					}
					num7 = num9;
					if (num > num7)
					{
						num = num7;
						result = i;
					}
				}
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				return result;
			}
		}

		public static bool IsIntersectingRectTransform(RectTransform rectTransform, Vector3 position, Camera camera)
		{
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			rectTransform.GetWorldCorners(TMP_TextUtilities.m_rectWorldCorners);
			if (TMP_TextUtilities.PointIntersectRectangle(position, TMP_TextUtilities.m_rectWorldCorners[0], TMP_TextUtilities.m_rectWorldCorners[1], TMP_TextUtilities.m_rectWorldCorners[2], TMP_TextUtilities.m_rectWorldCorners[3]))
			{
				return true;
			}
			return false;
		}

		public static int FindIntersectingCharacter(TMP_Text text, Vector3 position, Camera camera, bool visibleOnly)
		{
			RectTransform rectTransform = text.rectTransform;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			int i = 0;
			while (i < text.textInfo.characterCount)
			{
				TMP_CharacterInfo tmp_CharacterInfo = text.textInfo.characterInfo[i];
				if (!visibleOnly)
				{
					goto IL_6B;
				}
				if (tmp_CharacterInfo.isVisible)
				{
					goto IL_6B;
				}
				IL_101:
				i++;
				continue;
				IL_6B:
				Vector3 a = rectTransform.TransformPoint(tmp_CharacterInfo.bottomLeft);
				Vector3 b = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.topRight.y, 0f));
				Vector3 c = rectTransform.TransformPoint(tmp_CharacterInfo.topRight);
				Vector3 d = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.bottomLeft.y, 0f));
				if (TMP_TextUtilities.PointIntersectRectangle(position, a, b, c, d))
				{
					return i;
				}
				goto IL_101;
			}
			return -1;
		}

		public static int FindNearestCharacter(TMP_Text text, Vector3 position, Camera camera, bool visibleOnly)
		{
			RectTransform rectTransform = text.rectTransform;
			float num = float.PositiveInfinity;
			int result = 0;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			int i = 0;
			while (i < text.textInfo.characterCount)
			{
				TMP_CharacterInfo tmp_CharacterInfo = text.textInfo.characterInfo[i];
				if (!visibleOnly)
				{
					goto IL_70;
				}
				if (tmp_CharacterInfo.isVisible)
				{
					goto IL_70;
				}
				IL_188:
				i++;
				continue;
				IL_70:
				Vector3 vector = rectTransform.TransformPoint(tmp_CharacterInfo.bottomLeft);
				Vector3 vector2 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.topRight.y, 0f));
				Vector3 vector3 = rectTransform.TransformPoint(tmp_CharacterInfo.topRight);
				Vector3 vector4 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.bottomLeft.y, 0f));
				if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector3, vector4))
				{
					return i;
				}
				float num2 = TMP_TextUtilities.DistanceToLine(vector, vector2, position);
				float num3 = TMP_TextUtilities.DistanceToLine(vector2, vector3, position);
				float num4 = TMP_TextUtilities.DistanceToLine(vector3, vector4, position);
				float num5 = TMP_TextUtilities.DistanceToLine(vector4, vector, position);
				float num6 = (num2 >= num3) ? num3 : num2;
				float num7;
				if (num6 < num4)
				{
					num7 = num6;
				}
				else
				{
					num7 = num4;
				}
				num6 = num7;
				float num8;
				if (num6 < num5)
				{
					num8 = num6;
				}
				else
				{
					num8 = num5;
				}
				num6 = num8;
				if (num > num6)
				{
					num = num6;
					result = i;
					goto IL_188;
				}
				goto IL_188;
			}
			return result;
		}

		public static int FindIntersectingWord(TMP_Text text, Vector3 position, Camera camera)
		{
			RectTransform rectTransform = text.rectTransform;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			for (int i = 0; i < text.textInfo.wordCount; i++)
			{
				TMP_WordInfo tmp_WordInfo = text.textInfo.wordInfo[i];
				bool flag = false;
				Vector3 a = Vector3.zero;
				Vector3 b = Vector3.zero;
				Vector3 d = Vector3.zero;
				Vector3 c = Vector3.zero;
				float num = float.NegativeInfinity;
				float num2 = float.PositiveInfinity;
				int j = 0;
				while (j < tmp_WordInfo.characterCount)
				{
					int num3 = tmp_WordInfo.firstCharacterIndex + j;
					TMP_CharacterInfo tmp_CharacterInfo = text.textInfo.characterInfo[num3];
					int lineNumber = (int)tmp_CharacterInfo.lineNumber;
					bool isVisible = tmp_CharacterInfo.isVisible;
					num = Mathf.Max(num, tmp_CharacterInfo.ascender);
					num2 = Mathf.Min(num2, tmp_CharacterInfo.descender);
					if (!flag)
					{
						if (isVisible)
						{
							flag = true;
							a = new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.descender, 0f);
							b = new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.ascender, 0f);
							if (tmp_WordInfo.characterCount == 1)
							{
								flag = false;
								d = new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f);
								c = new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f);
								a = rectTransform.TransformPoint(new Vector3(a.x, num2, 0f));
								b = rectTransform.TransformPoint(new Vector3(b.x, num, 0f));
								c = rectTransform.TransformPoint(new Vector3(c.x, num, 0f));
								d = rectTransform.TransformPoint(new Vector3(d.x, num2, 0f));
								if (TMP_TextUtilities.PointIntersectRectangle(position, a, b, c, d))
								{
									return i;
								}
							}
						}
					}
					if (!flag)
					{
						goto IL_322;
					}
					if (j != tmp_WordInfo.characterCount - 1)
					{
						goto IL_322;
					}
					flag = false;
					d = new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f);
					c = new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f);
					a = rectTransform.TransformPoint(new Vector3(a.x, num2, 0f));
					b = rectTransform.TransformPoint(new Vector3(b.x, num, 0f));
					c = rectTransform.TransformPoint(new Vector3(c.x, num, 0f));
					d = rectTransform.TransformPoint(new Vector3(d.x, num2, 0f));
					if (TMP_TextUtilities.PointIntersectRectangle(position, a, b, c, d))
					{
						return i;
					}
					IL_426:
					j++;
					continue;
					IL_322:
					if (!flag || lineNumber == (int)text.textInfo.characterInfo[num3 + 1].lineNumber)
					{
						goto IL_426;
					}
					flag = false;
					d = new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f);
					c = new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f);
					a = rectTransform.TransformPoint(new Vector3(a.x, num2, 0f));
					b = rectTransform.TransformPoint(new Vector3(b.x, num, 0f));
					c = rectTransform.TransformPoint(new Vector3(c.x, num, 0f));
					d = rectTransform.TransformPoint(new Vector3(d.x, num2, 0f));
					num = float.NegativeInfinity;
					num2 = float.PositiveInfinity;
					if (TMP_TextUtilities.PointIntersectRectangle(position, a, b, c, d))
					{
						return i;
					}
					goto IL_426;
				}
			}
			return -1;
		}

		public static int FindNearestWord(TMP_Text text, Vector3 position, Camera camera)
		{
			RectTransform rectTransform = text.rectTransform;
			float num = float.PositiveInfinity;
			int result = 0;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			for (int i = 0; i < text.textInfo.wordCount; i++)
			{
				TMP_WordInfo tmp_WordInfo = text.textInfo.wordInfo[i];
				bool flag = false;
				Vector3 vector = Vector3.zero;
				Vector3 vector2 = Vector3.zero;
				Vector3 vector3 = Vector3.zero;
				Vector3 vector4 = Vector3.zero;
				int j = 0;
				while (j < tmp_WordInfo.characterCount)
				{
					int num2 = tmp_WordInfo.firstCharacterIndex + j;
					TMP_CharacterInfo tmp_CharacterInfo = text.textInfo.characterInfo[num2];
					int lineNumber = (int)tmp_CharacterInfo.lineNumber;
					bool isVisible = tmp_CharacterInfo.isVisible;
					if (!flag && isVisible)
					{
						flag = true;
						vector = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.descender, 0f));
						vector2 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.ascender, 0f));
						if (tmp_WordInfo.characterCount == 1)
						{
							flag = false;
							vector3 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
							vector4 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
							if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
							{
								return i;
							}
							float num3 = TMP_TextUtilities.DistanceToLine(vector, vector2, position);
							float num4 = TMP_TextUtilities.DistanceToLine(vector2, vector4, position);
							float num5 = TMP_TextUtilities.DistanceToLine(vector4, vector3, position);
							float num6 = TMP_TextUtilities.DistanceToLine(vector3, vector, position);
							float num7;
							if (num3 < num4)
							{
								num7 = num3;
							}
							else
							{
								num7 = num4;
							}
							float num8 = num7;
							float num9;
							if (num8 < num5)
							{
								num9 = num8;
							}
							else
							{
								num9 = num5;
							}
							num8 = num9;
							float num10;
							if (num8 < num6)
							{
								num10 = num8;
							}
							else
							{
								num10 = num6;
							}
							num8 = num10;
							if (num > num8)
							{
								num = num8;
								result = i;
							}
						}
					}
					if (!flag)
					{
						goto IL_33F;
					}
					if (j != tmp_WordInfo.characterCount - 1)
					{
						goto IL_33F;
					}
					flag = false;
					vector3 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
					vector4 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
					if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
					{
						return i;
					}
					float num11 = TMP_TextUtilities.DistanceToLine(vector, vector2, position);
					float num12 = TMP_TextUtilities.DistanceToLine(vector2, vector4, position);
					float num13 = TMP_TextUtilities.DistanceToLine(vector4, vector3, position);
					float num14 = TMP_TextUtilities.DistanceToLine(vector3, vector, position);
					float num15 = (num11 >= num12) ? num12 : num11;
					num15 = ((num15 >= num13) ? num13 : num15);
					float num16;
					if (num15 < num14)
					{
						num16 = num15;
					}
					else
					{
						num16 = num14;
					}
					num15 = num16;
					if (num > num15)
					{
						num = num15;
						result = i;
					}
					IL_471:
					j++;
					continue;
					IL_33F:
					if (!flag)
					{
						goto IL_471;
					}
					if (lineNumber == (int)text.textInfo.characterInfo[num2 + 1].lineNumber)
					{
						goto IL_471;
					}
					flag = false;
					vector3 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
					vector4 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
					if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
					{
						return i;
					}
					float num17 = TMP_TextUtilities.DistanceToLine(vector, vector2, position);
					float num18 = TMP_TextUtilities.DistanceToLine(vector2, vector4, position);
					float num19 = TMP_TextUtilities.DistanceToLine(vector4, vector3, position);
					float num20 = TMP_TextUtilities.DistanceToLine(vector3, vector, position);
					float num21 = (num17 >= num18) ? num18 : num17;
					float num22;
					if (num21 < num19)
					{
						num22 = num21;
					}
					else
					{
						num22 = num19;
					}
					num21 = num22;
					float num23;
					if (num21 < num20)
					{
						num23 = num21;
					}
					else
					{
						num23 = num20;
					}
					num21 = num23;
					if (num > num21)
					{
						num = num21;
						result = i;
						goto IL_471;
					}
					goto IL_471;
				}
			}
			return result;
		}

		public static int FindIntersectingLine(TMP_Text text, Vector3 position, Camera camera)
		{
			RectTransform rectTransform = text.rectTransform;
			int result = -1;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			for (int i = 0; i < text.textInfo.lineCount; i++)
			{
				TMP_LineInfo tmp_LineInfo = text.textInfo.lineInfo[i];
				float y = rectTransform.TransformPoint(new Vector3(0f, tmp_LineInfo.ascender, 0f)).y;
				float y2 = rectTransform.TransformPoint(new Vector3(0f, tmp_LineInfo.descender, 0f)).y;
				if (y > position.y)
				{
					if (y2 < position.y)
					{
						return i;
					}
				}
			}
			return result;
		}

		public static int FindIntersectingLink(TMP_Text text, Vector3 position, Camera camera)
		{
			Transform transform = text.transform;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(transform, position, camera, out position);
			for (int i = 0; i < text.textInfo.linkCount; i++)
			{
				TMP_LinkInfo tmp_LinkInfo = text.textInfo.linkInfo[i];
				bool flag = false;
				Vector3 a = Vector3.zero;
				Vector3 b = Vector3.zero;
				Vector3 d = Vector3.zero;
				Vector3 c = Vector3.zero;
				int j = 0;
				while (j < tmp_LinkInfo.linkTextLength)
				{
					int num = tmp_LinkInfo.linkTextfirstCharacterIndex + j;
					TMP_CharacterInfo tmp_CharacterInfo = text.textInfo.characterInfo[num];
					int lineNumber = (int)tmp_CharacterInfo.lineNumber;
					if (text.overflowMode != TextOverflowModes.Page)
					{
						goto IL_D5;
					}
					if ((int)(tmp_CharacterInfo.pageNumber + 1) == text.pageToDisplay)
					{
						goto IL_D5;
					}
					IL_2E9:
					j++;
					continue;
					IL_D5:
					if (!flag)
					{
						flag = true;
						a = transform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.descender, 0f));
						b = transform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.ascender, 0f));
						if (tmp_LinkInfo.linkTextLength == 1)
						{
							flag = false;
							d = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
							c = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
							if (TMP_TextUtilities.PointIntersectRectangle(position, a, b, c, d))
							{
								return i;
							}
						}
					}
					if (flag)
					{
						if (j == tmp_LinkInfo.linkTextLength - 1)
						{
							flag = false;
							d = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
							c = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
							if (TMP_TextUtilities.PointIntersectRectangle(position, a, b, c, d))
							{
								return i;
							}
							goto IL_2E9;
						}
					}
					if (!flag)
					{
						goto IL_2E9;
					}
					if (lineNumber == (int)text.textInfo.characterInfo[num + 1].lineNumber)
					{
						goto IL_2E9;
					}
					flag = false;
					d = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
					c = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
					if (TMP_TextUtilities.PointIntersectRectangle(position, a, b, c, d))
					{
						return i;
					}
					goto IL_2E9;
				}
			}
			return -1;
		}

		public static int FindNearestLink(TMP_Text text, Vector3 position, Camera camera)
		{
			RectTransform rectTransform = text.rectTransform;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			float num = float.PositiveInfinity;
			int result = 0;
			for (int i = 0; i < text.textInfo.linkCount; i++)
			{
				TMP_LinkInfo tmp_LinkInfo = text.textInfo.linkInfo[i];
				bool flag = false;
				Vector3 vector = Vector3.zero;
				Vector3 vector2 = Vector3.zero;
				Vector3 vector3 = Vector3.zero;
				Vector3 vector4 = Vector3.zero;
				int j = 0;
				while (j < tmp_LinkInfo.linkTextLength)
				{
					int num2 = tmp_LinkInfo.linkTextfirstCharacterIndex + j;
					TMP_CharacterInfo tmp_CharacterInfo = text.textInfo.characterInfo[num2];
					int lineNumber = (int)tmp_CharacterInfo.lineNumber;
					if (text.overflowMode != TextOverflowModes.Page)
					{
						goto IL_DB;
					}
					if ((int)(tmp_CharacterInfo.pageNumber + 1) == text.pageToDisplay)
					{
						goto IL_DB;
					}
					IL_4A6:
					j++;
					continue;
					IL_DB:
					if (!flag)
					{
						flag = true;
						vector = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.descender, 0f));
						vector2 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.ascender, 0f));
						if (tmp_LinkInfo.linkTextLength == 1)
						{
							flag = false;
							vector3 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
							vector4 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
							if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
							{
								return i;
							}
							float num3 = TMP_TextUtilities.DistanceToLine(vector, vector2, position);
							float num4 = TMP_TextUtilities.DistanceToLine(vector2, vector4, position);
							float num5 = TMP_TextUtilities.DistanceToLine(vector4, vector3, position);
							float num6 = TMP_TextUtilities.DistanceToLine(vector3, vector, position);
							float num7;
							if (num3 < num4)
							{
								num7 = num3;
							}
							else
							{
								num7 = num4;
							}
							float num8 = num7;
							float num9;
							if (num8 < num5)
							{
								num9 = num8;
							}
							else
							{
								num9 = num5;
							}
							num8 = num9;
							float num10;
							if (num8 < num6)
							{
								num10 = num8;
							}
							else
							{
								num10 = num6;
							}
							num8 = num10;
							if (num > num8)
							{
								num = num8;
								result = i;
							}
						}
					}
					if (flag)
					{
						if (j == tmp_LinkInfo.linkTextLength - 1)
						{
							flag = false;
							vector3 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
							vector4 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
							if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
							{
								return i;
							}
							float num11 = TMP_TextUtilities.DistanceToLine(vector, vector2, position);
							float num12 = TMP_TextUtilities.DistanceToLine(vector2, vector4, position);
							float num13 = TMP_TextUtilities.DistanceToLine(vector4, vector3, position);
							float num14 = TMP_TextUtilities.DistanceToLine(vector3, vector, position);
							float num15;
							if (num11 < num12)
							{
								num15 = num11;
							}
							else
							{
								num15 = num12;
							}
							float num16 = num15;
							float num17;
							if (num16 < num13)
							{
								num17 = num16;
							}
							else
							{
								num17 = num13;
							}
							num16 = num17;
							float num18;
							if (num16 < num14)
							{
								num18 = num16;
							}
							else
							{
								num18 = num14;
							}
							num16 = num18;
							if (num > num16)
							{
								num = num16;
								result = i;
							}
							goto IL_4A6;
						}
					}
					if (!flag)
					{
						goto IL_4A6;
					}
					if (lineNumber == (int)text.textInfo.characterInfo[num2 + 1].lineNumber)
					{
						goto IL_4A6;
					}
					flag = false;
					vector3 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
					vector4 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
					if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
					{
						return i;
					}
					float num19 = TMP_TextUtilities.DistanceToLine(vector, vector2, position);
					float num20 = TMP_TextUtilities.DistanceToLine(vector2, vector4, position);
					float num21 = TMP_TextUtilities.DistanceToLine(vector4, vector3, position);
					float num22 = TMP_TextUtilities.DistanceToLine(vector3, vector, position);
					float num23;
					if (num19 < num20)
					{
						num23 = num19;
					}
					else
					{
						num23 = num20;
					}
					float num24 = num23;
					num24 = ((num24 >= num21) ? num21 : num24);
					float num25;
					if (num24 < num22)
					{
						num25 = num24;
					}
					else
					{
						num25 = num22;
					}
					num24 = num25;
					if (num > num24)
					{
						num = num24;
						result = i;
						goto IL_4A6;
					}
					goto IL_4A6;
				}
			}
			return result;
		}

		private static bool PointIntersectRectangle(Vector3 m, Vector3 a, Vector3 b, Vector3 c, Vector3 d)
		{
			Vector3 vector = b - a;
			Vector3 rhs = m - a;
			Vector3 vector2 = c - b;
			Vector3 rhs2 = m - b;
			float num = Vector3.Dot(vector, rhs);
			float num2 = Vector3.Dot(vector2, rhs2);
			if (0f <= num)
			{
				if (num <= Vector3.Dot(vector, vector))
				{
					if (0f <= num2)
					{
						return num2 <= Vector3.Dot(vector2, vector2);
					}
				}
			}
			return false;
		}

		public unsafe static bool ScreenPointToWorldPointInRectangle(Transform transform, Vector2 screenPoint, Camera cam, out Vector3 worldPoint)
		{
			worldPoint = Vector2.zero;
			Ray ray = RectTransformUtility.ScreenPointToRay(cam, screenPoint);
			Plane plane = new Plane(transform.rotation * Vector3.back, transform.position);
			float distance;
			if (!plane.Raycast(ray, out distance))
			{
				return false;
			}
			worldPoint = ray.GetPoint(distance);
			return true;
		}

		private unsafe static bool IntersectLinePlane(TMP_TextUtilities.LineSegment line, Vector3 point, Vector3 normal, out Vector3 intersectingPoint)
		{
			intersectingPoint = Vector3.zero;
			Vector3 vector = line.Point2 - line.Point1;
			Vector3 rhs = line.Point1 - point;
			float num = Vector3.Dot(normal, vector);
			float num2 = -Vector3.Dot(normal, rhs);
			if (Mathf.Abs(num) >= Mathf.Epsilon)
			{
				float num3 = num2 / num;
				if (num3 >= 0f)
				{
					if (num3 <= 1f)
					{
						intersectingPoint = line.Point1 + num3 * vector;
						return true;
					}
				}
				return false;
			}
			if (num2 == 0f)
			{
				return true;
			}
			return false;
		}

		public static float DistanceToLine(Vector3 a, Vector3 b, Vector3 point)
		{
			Vector3 vector = b - a;
			Vector3 vector2 = a - point;
			float num = Vector3.Dot(vector, vector2);
			if (num > 0f)
			{
				return Vector3.Dot(vector2, vector2);
			}
			Vector3 vector3 = point - b;
			if (Vector3.Dot(vector, vector3) > 0f)
			{
				return Vector3.Dot(vector3, vector3);
			}
			Vector3 vector4 = vector2 - vector * (num / Vector3.Dot(vector, vector));
			return Vector3.Dot(vector4, vector4);
		}

		public static char ToLowerFast(char c)
		{
			if ((int)c > "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[-]^_`abcdefghijklmnopqrstuvwxyz{|}~-".Length - 1)
			{
				return c;
			}
			return "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[-]^_`abcdefghijklmnopqrstuvwxyz{|}~-"[(int)c];
		}

		public static char ToUpperFast(char c)
		{
			if ((int)c > "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-".Length - 1)
			{
				return c;
			}
			return "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-"[(int)c];
		}

		public static int GetSimpleHashCode(string s)
		{
			int num = 0;
			for (int i = 0; i < s.Length; i++)
			{
				num = ((num << 5) + num ^ (int)s[i]);
			}
			return num;
		}

		public static uint GetSimpleHashCodeLowercase(string s)
		{
			uint num = 0x1505U;
			for (int i = 0; i < s.Length; i++)
			{
				num = ((num << 5) + num ^ (uint)TMP_TextUtilities.ToLowerFast(s[i]));
			}
			return num;
		}

		public static int HexToInt(char hex)
		{
			switch (hex)
			{
			case '0':
				return 0;
			case '1':
				return 1;
			case '2':
				return 2;
			case '3':
				return 3;
			case '4':
				return 4;
			case '5':
				return 5;
			case '6':
				return 6;
			case '7':
				return 7;
			case '8':
				return 8;
			case '9':
				return 9;
			default:
				switch (hex)
				{
				case 'a':
					return 0xA;
				case 'b':
					return 0xB;
				case 'c':
					return 0xC;
				case 'd':
					return 0xD;
				case 'e':
					return 0xE;
				case 'f':
					return 0xF;
				default:
					return 0xF;
				}
				break;
			case 'A':
				return 0xA;
			case 'B':
				return 0xB;
			case 'C':
				return 0xC;
			case 'D':
				return 0xD;
			case 'E':
				return 0xE;
			case 'F':
				return 0xF;
			}
		}

		public static int StringToInt(string s)
		{
			int num = 0;
			for (int i = 0; i < s.Length; i++)
			{
				num += TMP_TextUtilities.HexToInt(s[i]) * (int)Mathf.Pow(16f, (float)(s.Length - 1 - i));
			}
			return num;
		}

		private struct LineSegment
		{
			public Vector3 Point1;

			public Vector3 Point2;

			public LineSegment(Vector3 p1, Vector3 p2)
			{
				this.Point1 = p1;
				this.Point2 = p2;
			}
		}
	}
}
