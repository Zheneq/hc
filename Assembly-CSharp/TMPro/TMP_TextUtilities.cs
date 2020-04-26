using UnityEngine;

namespace TMPro
{
	public static class TMP_TextUtilities
	{
		private struct LineSegment
		{
			public Vector3 Point1;

			public Vector3 Point2;

			public LineSegment(Vector3 p1, Vector3 p2)
			{
				Point1 = p1;
				Point2 = p2;
			}
		}

		private static Vector3[] m_rectWorldCorners = new Vector3[4];

		private const string k_lookupStringL = "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[-]^_`abcdefghijklmnopqrstuvwxyz{|}~-";

		private const string k_lookupStringU = "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-";

		public static int GetCursorIndexFromPosition(TMP_Text textComponent, Vector3 position, Camera camera)
		{
			int num = FindNearestCharacter(textComponent, position, camera, false);
			RectTransform rectTransform = textComponent.rectTransform;
			ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			TMP_CharacterInfo tMP_CharacterInfo = textComponent.textInfo.characterInfo[num];
			Vector3 vector = rectTransform.TransformPoint(tMP_CharacterInfo.bottomLeft);
			Vector3 vector2 = rectTransform.TransformPoint(tMP_CharacterInfo.topRight);
			float num2 = (position.x - vector.x) / (vector2.x - vector.x);
			if (num2 < 0.5f)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return num;
					}
				}
			}
			return num + 1;
		}

		public static int GetCursorIndexFromPosition(TMP_Text textComponent, Vector3 position, Camera camera, out CaretPosition cursor)
		{
			int num = FindNearestLine(textComponent, position, camera);
			int num2 = FindNearestCharacterOnLine(textComponent, position, num, camera, false);
			if (textComponent.textInfo.lineInfo[num].characterCount == 1)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						cursor = CaretPosition.Left;
						return num2;
					}
				}
			}
			RectTransform rectTransform = textComponent.rectTransform;
			ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			TMP_CharacterInfo tMP_CharacterInfo = textComponent.textInfo.characterInfo[num2];
			Vector3 vector = rectTransform.TransformPoint(tMP_CharacterInfo.bottomLeft);
			Vector3 vector2 = rectTransform.TransformPoint(tMP_CharacterInfo.topRight);
			float num3 = (position.x - vector.x) / (vector2.x - vector.x);
			if (num3 < 0.5f)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						cursor = CaretPosition.Left;
						return num2;
					}
				}
			}
			cursor = CaretPosition.Right;
			return num2;
		}

		public static int FindNearestLine(TMP_Text text, Vector3 position, Camera camera)
		{
			RectTransform rectTransform = text.rectTransform;
			float num = float.PositiveInfinity;
			int result = -1;
			ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			for (int i = 0; i < text.textInfo.lineCount; i++)
			{
				TMP_LineInfo tMP_LineInfo = text.textInfo.lineInfo[i];
				Vector3 vector = rectTransform.TransformPoint(new Vector3(0f, tMP_LineInfo.ascender, 0f));
				float y = vector.y;
				Vector3 vector2 = rectTransform.TransformPoint(new Vector3(0f, tMP_LineInfo.descender, 0f));
				float y2 = vector2.y;
				if (y > position.y)
				{
					if (y2 < position.y)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								return i;
							}
						}
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
			while (true)
			{
				return result;
			}
		}

		public static int FindNearestCharacterOnLine(TMP_Text text, Vector3 position, int line, Camera camera, bool visibleOnly)
		{
			RectTransform rectTransform = text.rectTransform;
			ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			int firstCharacterIndex = text.textInfo.lineInfo[line].firstCharacterIndex;
			int lastCharacterIndex = text.textInfo.lineInfo[line].lastCharacterIndex;
			float num = float.PositiveInfinity;
			int result = lastCharacterIndex;
			int num2 = firstCharacterIndex;
			while (true)
			{
				if (num2 < lastCharacterIndex)
				{
					TMP_CharacterInfo tMP_CharacterInfo = text.textInfo.characterInfo[num2];
					if (visibleOnly && !tMP_CharacterInfo.isVisible)
					{
					}
					else
					{
						Vector3 vector = rectTransform.TransformPoint(tMP_CharacterInfo.bottomLeft);
						Vector3 vector2 = rectTransform.TransformPoint(new Vector3(tMP_CharacterInfo.bottomLeft.x, tMP_CharacterInfo.topRight.y, 0f));
						Vector3 vector3 = rectTransform.TransformPoint(tMP_CharacterInfo.topRight);
						Vector3 vector4 = rectTransform.TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.bottomLeft.y, 0f));
						if (PointIntersectRectangle(position, vector, vector2, vector3, vector4))
						{
							result = num2;
							break;
						}
						float num3 = DistanceToLine(vector, vector2, position);
						float num4 = DistanceToLine(vector2, vector3, position);
						float num5 = DistanceToLine(vector3, vector4, position);
						float num6 = DistanceToLine(vector4, vector, position);
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
							result = num2;
						}
					}
					num2++;
					continue;
				}
				break;
			}
			return result;
		}

		public static bool IsIntersectingRectTransform(RectTransform rectTransform, Vector3 position, Camera camera)
		{
			ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			rectTransform.GetWorldCorners(m_rectWorldCorners);
			if (PointIntersectRectangle(position, m_rectWorldCorners[0], m_rectWorldCorners[1], m_rectWorldCorners[2], m_rectWorldCorners[3]))
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return true;
					}
				}
			}
			return false;
		}

		public static int FindIntersectingCharacter(TMP_Text text, Vector3 position, Camera camera, bool visibleOnly)
		{
			RectTransform rectTransform = text.rectTransform;
			ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			for (int i = 0; i < text.textInfo.characterCount; i++)
			{
				TMP_CharacterInfo tMP_CharacterInfo = text.textInfo.characterInfo[i];
				if (visibleOnly)
				{
					if (!tMP_CharacterInfo.isVisible)
					{
						continue;
					}
				}
				Vector3 a = rectTransform.TransformPoint(tMP_CharacterInfo.bottomLeft);
				Vector3 b = rectTransform.TransformPoint(new Vector3(tMP_CharacterInfo.bottomLeft.x, tMP_CharacterInfo.topRight.y, 0f));
				Vector3 c = rectTransform.TransformPoint(tMP_CharacterInfo.topRight);
				Vector3 d = rectTransform.TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.bottomLeft.y, 0f));
				if (!PointIntersectRectangle(position, a, b, c, d))
				{
					continue;
				}
				while (true)
				{
					return i;
				}
			}
			while (true)
			{
				return -1;
			}
		}

		public static int FindNearestCharacter(TMP_Text text, Vector3 position, Camera camera, bool visibleOnly)
		{
			RectTransform rectTransform = text.rectTransform;
			float num = float.PositiveInfinity;
			int result = 0;
			ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			for (int i = 0; i < text.textInfo.characterCount; i++)
			{
				TMP_CharacterInfo tMP_CharacterInfo = text.textInfo.characterInfo[i];
				if (visibleOnly)
				{
					if (!tMP_CharacterInfo.isVisible)
					{
						continue;
					}
				}
				Vector3 vector = rectTransform.TransformPoint(tMP_CharacterInfo.bottomLeft);
				Vector3 vector2 = rectTransform.TransformPoint(new Vector3(tMP_CharacterInfo.bottomLeft.x, tMP_CharacterInfo.topRight.y, 0f));
				Vector3 vector3 = rectTransform.TransformPoint(tMP_CharacterInfo.topRight);
				Vector3 vector4 = rectTransform.TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.bottomLeft.y, 0f));
				if (PointIntersectRectangle(position, vector, vector2, vector3, vector4))
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							return i;
						}
					}
				}
				float num2 = DistanceToLine(vector, vector2, position);
				float num3 = DistanceToLine(vector2, vector3, position);
				float num4 = DistanceToLine(vector3, vector4, position);
				float num5 = DistanceToLine(vector4, vector, position);
				float num6 = (!(num2 < num3)) ? num3 : num2;
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
				}
			}
			while (true)
			{
				return result;
			}
		}

		public static int FindIntersectingWord(TMP_Text text, Vector3 position, Camera camera)
		{
			RectTransform rectTransform = text.rectTransform;
			ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			for (int i = 0; i < text.textInfo.wordCount; i++)
			{
				TMP_WordInfo tMP_WordInfo = text.textInfo.wordInfo[i];
				bool flag = false;
				Vector3 a = Vector3.zero;
				Vector3 b = Vector3.zero;
				Vector3 zero = Vector3.zero;
				Vector3 zero2 = Vector3.zero;
				float num = float.NegativeInfinity;
				float num2 = float.PositiveInfinity;
				for (int j = 0; j < tMP_WordInfo.characterCount; j++)
				{
					int num3 = tMP_WordInfo.firstCharacterIndex + j;
					TMP_CharacterInfo tMP_CharacterInfo = text.textInfo.characterInfo[num3];
					int lineNumber = tMP_CharacterInfo.lineNumber;
					bool isVisible = tMP_CharacterInfo.isVisible;
					num = Mathf.Max(num, tMP_CharacterInfo.ascender);
					num2 = Mathf.Min(num2, tMP_CharacterInfo.descender);
					if (!flag)
					{
						if (isVisible)
						{
							flag = true;
							a = new Vector3(tMP_CharacterInfo.bottomLeft.x, tMP_CharacterInfo.descender, 0f);
							b = new Vector3(tMP_CharacterInfo.bottomLeft.x, tMP_CharacterInfo.ascender, 0f);
							if (tMP_WordInfo.characterCount == 1)
							{
								flag = false;
								zero = new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.descender, 0f);
								zero2 = new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.ascender, 0f);
								a = rectTransform.TransformPoint(new Vector3(a.x, num2, 0f));
								b = rectTransform.TransformPoint(new Vector3(b.x, num, 0f));
								zero2 = rectTransform.TransformPoint(new Vector3(zero2.x, num, 0f));
								zero = rectTransform.TransformPoint(new Vector3(zero.x, num2, 0f));
								if (PointIntersectRectangle(position, a, b, zero2, zero))
								{
									while (true)
									{
										switch (7)
										{
										case 0:
											break;
										default:
											return i;
										}
									}
								}
							}
						}
					}
					if (flag)
					{
						if (j == tMP_WordInfo.characterCount - 1)
						{
							flag = false;
							zero = new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.descender, 0f);
							zero2 = new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.ascender, 0f);
							a = rectTransform.TransformPoint(new Vector3(a.x, num2, 0f));
							b = rectTransform.TransformPoint(new Vector3(b.x, num, 0f));
							zero2 = rectTransform.TransformPoint(new Vector3(zero2.x, num, 0f));
							zero = rectTransform.TransformPoint(new Vector3(zero.x, num2, 0f));
							if (!PointIntersectRectangle(position, a, b, zero2, zero))
							{
								continue;
							}
							while (true)
							{
								return i;
							}
						}
					}
					if (!flag || lineNumber == text.textInfo.characterInfo[num3 + 1].lineNumber)
					{
						continue;
					}
					flag = false;
					zero = new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.descender, 0f);
					zero2 = new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.ascender, 0f);
					a = rectTransform.TransformPoint(new Vector3(a.x, num2, 0f));
					b = rectTransform.TransformPoint(new Vector3(b.x, num, 0f));
					zero2 = rectTransform.TransformPoint(new Vector3(zero2.x, num, 0f));
					zero = rectTransform.TransformPoint(new Vector3(zero.x, num2, 0f));
					num = float.NegativeInfinity;
					num2 = float.PositiveInfinity;
					if (!PointIntersectRectangle(position, a, b, zero2, zero))
					{
						continue;
					}
					while (true)
					{
						return i;
					}
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						goto end_IL_043a;
					}
					continue;
					end_IL_043a:
					break;
				}
			}
			return -1;
		}

		public static int FindNearestWord(TMP_Text text, Vector3 position, Camera camera)
		{
			RectTransform rectTransform = text.rectTransform;
			float num = float.PositiveInfinity;
			int result = 0;
			ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			for (int i = 0; i < text.textInfo.wordCount; i++)
			{
				TMP_WordInfo tMP_WordInfo = text.textInfo.wordInfo[i];
				bool flag = false;
				Vector3 vector = Vector3.zero;
				Vector3 vector2 = Vector3.zero;
				Vector3 zero = Vector3.zero;
				Vector3 zero2 = Vector3.zero;
				for (int j = 0; j < tMP_WordInfo.characterCount; j++)
				{
					int num2 = tMP_WordInfo.firstCharacterIndex + j;
					TMP_CharacterInfo tMP_CharacterInfo = text.textInfo.characterInfo[num2];
					int lineNumber = tMP_CharacterInfo.lineNumber;
					bool isVisible = tMP_CharacterInfo.isVisible;
					if (!flag && isVisible)
					{
						flag = true;
						vector = rectTransform.TransformPoint(new Vector3(tMP_CharacterInfo.bottomLeft.x, tMP_CharacterInfo.descender, 0f));
						vector2 = rectTransform.TransformPoint(new Vector3(tMP_CharacterInfo.bottomLeft.x, tMP_CharacterInfo.ascender, 0f));
						if (tMP_WordInfo.characterCount == 1)
						{
							flag = false;
							zero = rectTransform.TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.descender, 0f));
							zero2 = rectTransform.TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.ascender, 0f));
							if (PointIntersectRectangle(position, vector, vector2, zero2, zero))
							{
								while (true)
								{
									return i;
								}
							}
							float num3 = DistanceToLine(vector, vector2, position);
							float num4 = DistanceToLine(vector2, zero2, position);
							float num5 = DistanceToLine(zero2, zero, position);
							float num6 = DistanceToLine(zero, vector, position);
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
						if (j == tMP_WordInfo.characterCount - 1)
						{
							flag = false;
							zero = rectTransform.TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.descender, 0f));
							zero2 = rectTransform.TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.ascender, 0f));
							if (PointIntersectRectangle(position, vector, vector2, zero2, zero))
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										break;
									default:
										return i;
									}
								}
							}
							float num11 = DistanceToLine(vector, vector2, position);
							float num12 = DistanceToLine(vector2, zero2, position);
							float num13 = DistanceToLine(zero2, zero, position);
							float num14 = DistanceToLine(zero, vector, position);
							float num15 = (!(num11 < num12)) ? num12 : num11;
							num15 = ((!(num15 < num13)) ? num13 : num15);
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
							continue;
						}
					}
					if (!flag)
					{
						continue;
					}
					if (lineNumber == text.textInfo.characterInfo[num2 + 1].lineNumber)
					{
						continue;
					}
					flag = false;
					zero = rectTransform.TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.descender, 0f));
					zero2 = rectTransform.TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.ascender, 0f));
					if (PointIntersectRectangle(position, vector, vector2, zero2, zero))
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								return i;
							}
						}
					}
					float num17 = DistanceToLine(vector, vector2, position);
					float num18 = DistanceToLine(vector2, zero2, position);
					float num19 = DistanceToLine(zero2, zero, position);
					float num20 = DistanceToLine(zero, vector, position);
					float num21 = (!(num17 < num18)) ? num18 : num17;
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
					}
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						goto end_IL_0485;
					}
					continue;
					end_IL_0485:
					break;
				}
			}
			while (true)
			{
				return result;
			}
		}

		public static int FindIntersectingLine(TMP_Text text, Vector3 position, Camera camera)
		{
			RectTransform rectTransform = text.rectTransform;
			int result = -1;
			ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			for (int i = 0; i < text.textInfo.lineCount; i++)
			{
				TMP_LineInfo tMP_LineInfo = text.textInfo.lineInfo[i];
				Vector3 vector = rectTransform.TransformPoint(new Vector3(0f, tMP_LineInfo.ascender, 0f));
				float y = vector.y;
				Vector3 vector2 = rectTransform.TransformPoint(new Vector3(0f, tMP_LineInfo.descender, 0f));
				float y2 = vector2.y;
				if (!(y > position.y))
				{
					continue;
				}
				if (!(y2 < position.y))
				{
					continue;
				}
				while (true)
				{
					return i;
				}
			}
			return result;
		}

		public static int FindIntersectingLink(TMP_Text text, Vector3 position, Camera camera)
		{
			Transform transform = text.transform;
			ScreenPointToWorldPointInRectangle(transform, position, camera, out position);
			for (int i = 0; i < text.textInfo.linkCount; i++)
			{
				TMP_LinkInfo tMP_LinkInfo = text.textInfo.linkInfo[i];
				bool flag = false;
				Vector3 a = Vector3.zero;
				Vector3 b = Vector3.zero;
				Vector3 zero = Vector3.zero;
				Vector3 zero2 = Vector3.zero;
				for (int j = 0; j < tMP_LinkInfo.linkTextLength; j++)
				{
					int num = tMP_LinkInfo.linkTextfirstCharacterIndex + j;
					TMP_CharacterInfo tMP_CharacterInfo = text.textInfo.characterInfo[num];
					int lineNumber = tMP_CharacterInfo.lineNumber;
					if (text.overflowMode == TextOverflowModes.Page)
					{
						if (tMP_CharacterInfo.pageNumber + 1 != text.pageToDisplay)
						{
							continue;
						}
					}
					if (!flag)
					{
						flag = true;
						a = transform.TransformPoint(new Vector3(tMP_CharacterInfo.bottomLeft.x, tMP_CharacterInfo.descender, 0f));
						b = transform.TransformPoint(new Vector3(tMP_CharacterInfo.bottomLeft.x, tMP_CharacterInfo.ascender, 0f));
						if (tMP_LinkInfo.linkTextLength == 1)
						{
							flag = false;
							zero = transform.TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.descender, 0f));
							zero2 = transform.TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.ascender, 0f));
							if (PointIntersectRectangle(position, a, b, zero2, zero))
							{
								return i;
							}
						}
					}
					if (flag)
					{
						if (j == tMP_LinkInfo.linkTextLength - 1)
						{
							flag = false;
							zero = transform.TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.descender, 0f));
							zero2 = transform.TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.ascender, 0f));
							if (!PointIntersectRectangle(position, a, b, zero2, zero))
							{
								continue;
							}
							while (true)
							{
								return i;
							}
						}
					}
					if (!flag)
					{
						continue;
					}
					if (lineNumber == text.textInfo.characterInfo[num + 1].lineNumber)
					{
						continue;
					}
					flag = false;
					zero = transform.TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.descender, 0f));
					zero2 = transform.TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.ascender, 0f));
					if (!PointIntersectRectangle(position, a, b, zero2, zero))
					{
						continue;
					}
					while (true)
					{
						return i;
					}
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						goto end_IL_02fd;
					}
					continue;
					end_IL_02fd:
					break;
				}
			}
			while (true)
			{
				return -1;
			}
		}

		public static int FindNearestLink(TMP_Text text, Vector3 position, Camera camera)
		{
			RectTransform rectTransform = text.rectTransform;
			ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			float num = float.PositiveInfinity;
			int result = 0;
			for (int i = 0; i < text.textInfo.linkCount; i++)
			{
				TMP_LinkInfo tMP_LinkInfo = text.textInfo.linkInfo[i];
				bool flag = false;
				Vector3 vector = Vector3.zero;
				Vector3 vector2 = Vector3.zero;
				Vector3 zero = Vector3.zero;
				Vector3 zero2 = Vector3.zero;
				for (int j = 0; j < tMP_LinkInfo.linkTextLength; j++)
				{
					int num2 = tMP_LinkInfo.linkTextfirstCharacterIndex + j;
					TMP_CharacterInfo tMP_CharacterInfo = text.textInfo.characterInfo[num2];
					int lineNumber = tMP_CharacterInfo.lineNumber;
					if (text.overflowMode == TextOverflowModes.Page)
					{
						if (tMP_CharacterInfo.pageNumber + 1 != text.pageToDisplay)
						{
							continue;
						}
					}
					if (!flag)
					{
						flag = true;
						vector = rectTransform.TransformPoint(new Vector3(tMP_CharacterInfo.bottomLeft.x, tMP_CharacterInfo.descender, 0f));
						vector2 = rectTransform.TransformPoint(new Vector3(tMP_CharacterInfo.bottomLeft.x, tMP_CharacterInfo.ascender, 0f));
						if (tMP_LinkInfo.linkTextLength == 1)
						{
							flag = false;
							zero = rectTransform.TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.descender, 0f));
							zero2 = rectTransform.TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.ascender, 0f));
							if (PointIntersectRectangle(position, vector, vector2, zero2, zero))
							{
								while (true)
								{
									return i;
								}
							}
							float num3 = DistanceToLine(vector, vector2, position);
							float num4 = DistanceToLine(vector2, zero2, position);
							float num5 = DistanceToLine(zero2, zero, position);
							float num6 = DistanceToLine(zero, vector, position);
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
						if (j == tMP_LinkInfo.linkTextLength - 1)
						{
							flag = false;
							zero = rectTransform.TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.descender, 0f));
							zero2 = rectTransform.TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.ascender, 0f));
							if (PointIntersectRectangle(position, vector, vector2, zero2, zero))
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										break;
									default:
										return i;
									}
								}
							}
							float num11 = DistanceToLine(vector, vector2, position);
							float num12 = DistanceToLine(vector2, zero2, position);
							float num13 = DistanceToLine(zero2, zero, position);
							float num14 = DistanceToLine(zero, vector, position);
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
							continue;
						}
					}
					if (!flag)
					{
						continue;
					}
					if (lineNumber == text.textInfo.characterInfo[num2 + 1].lineNumber)
					{
						continue;
					}
					flag = false;
					zero = rectTransform.TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.descender, 0f));
					zero2 = rectTransform.TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.ascender, 0f));
					if (PointIntersectRectangle(position, vector, vector2, zero2, zero))
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								return i;
							}
						}
					}
					float num19 = DistanceToLine(vector, vector2, position);
					float num20 = DistanceToLine(vector2, zero2, position);
					float num21 = DistanceToLine(zero2, zero, position);
					float num22 = DistanceToLine(zero, vector, position);
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
					num24 = ((!(num24 < num21)) ? num21 : num24);
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
					}
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						goto end_IL_04ba;
					}
					continue;
					end_IL_04ba:
					break;
				}
			}
			while (true)
			{
				return result;
			}
		}

		private static bool PointIntersectRectangle(Vector3 m, Vector3 a, Vector3 b, Vector3 c, Vector3 d)
		{
			Vector3 vector = b - a;
			Vector3 rhs = m - a;
			Vector3 vector2 = c - b;
			Vector3 rhs2 = m - b;
			float num = Vector3.Dot(vector, rhs);
			float num2 = Vector3.Dot(vector2, rhs2);
			int result;
			if (0f <= num)
			{
				if (num <= Vector3.Dot(vector, vector))
				{
					if (0f <= num2)
					{
						result = ((num2 <= Vector3.Dot(vector2, vector2)) ? 1 : 0);
						goto IL_0095;
					}
				}
			}
			result = 0;
			goto IL_0095;
			IL_0095:
			return (byte)result != 0;
		}

		public static bool ScreenPointToWorldPointInRectangle(Transform transform, Vector2 screenPoint, Camera cam, out Vector3 worldPoint)
		{
			worldPoint = Vector2.zero;
			Ray ray = RectTransformUtility.ScreenPointToRay(cam, screenPoint);
			if (!new Plane(transform.rotation * Vector3.back, transform.position).Raycast(ray, out float enter))
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return false;
					}
				}
			}
			worldPoint = ray.GetPoint(enter);
			return true;
		}

		private static bool IntersectLinePlane(LineSegment line, Vector3 point, Vector3 normal, out Vector3 intersectingPoint)
		{
			intersectingPoint = Vector3.zero;
			Vector3 vector = line.Point2 - line.Point1;
			Vector3 rhs = line.Point1 - point;
			float num = Vector3.Dot(normal, vector);
			float num2 = 0f - Vector3.Dot(normal, rhs);
			if (Mathf.Abs(num) < Mathf.Epsilon)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						if (num2 == 0f)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									break;
								default:
									return true;
								}
							}
						}
						return false;
					}
				}
			}
			float num3 = num2 / num;
			if (!(num3 < 0f))
			{
				if (!(num3 > 1f))
				{
					intersectingPoint = line.Point1 + num3 * vector;
					return true;
				}
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
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return Vector3.Dot(vector3, vector3);
					}
				}
			}
			Vector3 vector4 = vector2 - vector * (num / Vector3.Dot(vector, vector));
			return Vector3.Dot(vector4, vector4);
		}

		public static char ToLowerFast(char c)
		{
			if (c > "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[-]^_`abcdefghijklmnopqrstuvwxyz{|}~-".Length - 1)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return c;
					}
				}
			}
			return "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[-]^_`abcdefghijklmnopqrstuvwxyz{|}~-"[c];
		}

		public static char ToUpperFast(char c)
		{
			if (c > "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-".Length - 1)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return c;
					}
				}
			}
			return "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-"[c];
		}

		public static int GetSimpleHashCode(string s)
		{
			int num = 0;
			for (int i = 0; i < s.Length; i++)
			{
				num = (((num << 5) + num) ^ s[i]);
			}
			return num;
		}

		public static uint GetSimpleHashCodeLowercase(string s)
		{
			uint num = 5381u;
			for (int i = 0; i < s.Length; i++)
			{
				num = (((num << 5) + num) ^ ToLowerFast(s[i]));
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
			case 'A':
				return 10;
			case 'B':
				return 11;
			case 'C':
				return 12;
			case 'D':
				return 13;
			case 'E':
				return 14;
			case 'F':
				return 15;
			case 'a':
				return 10;
			case 'b':
				return 11;
			case 'c':
				return 12;
			case 'd':
				return 13;
			case 'e':
				return 14;
			case 'f':
				return 15;
			default:
				return 15;
			}
		}

		public static int StringToInt(string s)
		{
			int num = 0;
			for (int i = 0; i < s.Length; i++)
			{
				num += HexToInt(s[i]) * (int)Mathf.Pow(16f, s.Length - 1 - i);
			}
			return num;
		}
	}
}
