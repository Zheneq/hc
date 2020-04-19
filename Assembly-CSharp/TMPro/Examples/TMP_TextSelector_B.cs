using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TMPro.Examples
{
	public class TMP_TextSelector_B : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerUpHandler, IEventSystemHandler
	{
		public RectTransform \u001D;

		private RectTransform \u000E;

		private TextMeshProUGUI \u0012;

		private const string \u0015 = "You have selected link <#ffff00>";

		private const string \u0016 = "Word Index: <#ffff00>";

		private TextMeshProUGUI \u0013;

		private Canvas \u0018;

		private Camera \u0009;

		private bool \u0019;

		private int \u0011 = -1;

		private int \u001A = -1;

		private int \u0004 = -1;

		private Matrix4x4 \u000B;

		private TMP_MeshInfo[] \u0003;

		private void \u000F()
		{
			this.\u0013 = base.gameObject.GetComponent<TextMeshProUGUI>();
			this.\u0018 = base.gameObject.GetComponentInParent<Canvas>();
			if (this.\u0018.renderMode == RenderMode.ScreenSpaceOverlay)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_TextSelector_B.\u000F()).MethodHandle;
				}
				this.\u0009 = null;
			}
			else
			{
				this.\u0009 = this.\u0018.worldCamera;
			}
			this.\u000E = UnityEngine.Object.Instantiate<RectTransform>(this.\u001D);
			this.\u000E.SetParent(this.\u0018.transform, false);
			this.\u0012 = this.\u000E.GetComponentInChildren<TextMeshProUGUI>();
			this.\u000E.gameObject.SetActive(false);
		}

		private void \u0017()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this.\u000F));
		}

		private void \u000D()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this.\u000F));
		}

		private void \u000F(UnityEngine.Object \u001D)
		{
			if (\u001D == this.\u0013)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_TextSelector_B.\u000F(UnityEngine.Object)).MethodHandle;
				}
				this.\u0003 = this.\u0013.textInfo.CopyMeshInfoVertexData();
			}
		}

		private void \u0008()
		{
			if (this.\u0019)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_TextSelector_B.\u0008()).MethodHandle;
				}
				int num = TMP_TextUtilities.FindIntersectingCharacter(this.\u0013, Input.mousePosition, this.\u0009, true);
				if (num != -1)
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
					if (num == this.\u0004)
					{
						goto IL_6C;
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
				this.\u000F(this.\u0004);
				this.\u0004 = -1;
				IL_6C:
				if (num != -1)
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
					if (num != this.\u0004)
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
						if (!Input.GetKey(KeyCode.LeftShift))
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
							if (!Input.GetKey(KeyCode.RightShift))
							{
								goto IL_406;
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
						this.\u0004 = num;
						int materialReferenceIndex = this.\u0013.textInfo.characterInfo[num].materialReferenceIndex;
						int vertexIndex = this.\u0013.textInfo.characterInfo[num].vertexIndex;
						Vector3[] vertices = this.\u0013.textInfo.meshInfo[materialReferenceIndex].vertices;
						Vector2 v = (vertices[vertexIndex] + vertices[vertexIndex + 2]) / 2f;
						Vector3 b = v;
						vertices[vertexIndex] -= b;
						vertices[vertexIndex + 1] = vertices[vertexIndex + 1] - b;
						vertices[vertexIndex + 2] = vertices[vertexIndex + 2] - b;
						vertices[vertexIndex + 3] = vertices[vertexIndex + 3] - b;
						float d = 1.5f;
						this.\u000B = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * d);
						vertices[vertexIndex] = this.\u000B.MultiplyPoint3x4(vertices[vertexIndex]);
						vertices[vertexIndex + 1] = this.\u000B.MultiplyPoint3x4(vertices[vertexIndex + 1]);
						vertices[vertexIndex + 2] = this.\u000B.MultiplyPoint3x4(vertices[vertexIndex + 2]);
						vertices[vertexIndex + 3] = this.\u000B.MultiplyPoint3x4(vertices[vertexIndex + 3]);
						vertices[vertexIndex] += b;
						vertices[vertexIndex + 1] = vertices[vertexIndex + 1] + b;
						vertices[vertexIndex + 2] = vertices[vertexIndex + 2] + b;
						vertices[vertexIndex + 3] = vertices[vertexIndex + 3] + b;
						Color32 color = new Color32(byte.MaxValue, byte.MaxValue, 0xC0, byte.MaxValue);
						Color32[] colors = this.\u0013.textInfo.meshInfo[materialReferenceIndex].colors32;
						colors[vertexIndex] = color;
						colors[vertexIndex + 1] = color;
						colors[vertexIndex + 2] = color;
						colors[vertexIndex + 3] = color;
						TMP_MeshInfo tmp_MeshInfo = this.\u0013.textInfo.meshInfo[materialReferenceIndex];
						int dst = vertices.Length - 4;
						tmp_MeshInfo.SwapVertexData(vertexIndex, dst);
						this.\u0013.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
					}
				}
				IL_406:
				int num2 = TMP_TextUtilities.FindIntersectingWord(this.\u0013, Input.mousePosition, this.\u0009);
				if (this.\u000E != null)
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
					if (this.\u0011 != -1)
					{
						if (num2 != -1)
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
							if (num2 == this.\u0011)
							{
								goto IL_59E;
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
						TMP_WordInfo tmp_WordInfo = this.\u0013.textInfo.wordInfo[this.\u0011];
						for (int i = 0; i < tmp_WordInfo.characterCount; i++)
						{
							int num3 = tmp_WordInfo.firstCharacterIndex + i;
							int materialReferenceIndex2 = this.\u0013.textInfo.characterInfo[num3].materialReferenceIndex;
							int vertexIndex2 = this.\u0013.textInfo.characterInfo[num3].vertexIndex;
							Color32[] colors2 = this.\u0013.textInfo.meshInfo[materialReferenceIndex2].colors32;
							Color32 color2 = colors2[vertexIndex2].Tint(1.33333f);
							colors2[vertexIndex2] = color2;
							colors2[vertexIndex2 + 1] = color2;
							colors2[vertexIndex2 + 2] = color2;
							colors2[vertexIndex2 + 3] = color2;
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
						this.\u0013.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
						this.\u0011 = -1;
					}
				}
				IL_59E:
				if (num2 != -1 && num2 != this.\u0011)
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
					if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
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
						this.\u0011 = num2;
						TMP_WordInfo tmp_WordInfo2 = this.\u0013.textInfo.wordInfo[num2];
						for (int j = 0; j < tmp_WordInfo2.characterCount; j++)
						{
							int num4 = tmp_WordInfo2.firstCharacterIndex + j;
							int materialReferenceIndex3 = this.\u0013.textInfo.characterInfo[num4].materialReferenceIndex;
							int vertexIndex3 = this.\u0013.textInfo.characterInfo[num4].vertexIndex;
							Color32[] colors3 = this.\u0013.textInfo.meshInfo[materialReferenceIndex3].colors32;
							Color32 color3 = colors3[vertexIndex3].Tint(0.75f);
							colors3[vertexIndex3] = color3;
							colors3[vertexIndex3 + 1] = color3;
							colors3[vertexIndex3 + 2] = color3;
							colors3[vertexIndex3 + 3] = color3;
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
						this.\u0013.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
					}
				}
				int num5 = TMP_TextUtilities.FindIntersectingLink(this.\u0013, Input.mousePosition, this.\u0009);
				if (num5 == -1)
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
					if (this.\u001A != -1)
					{
						goto IL_759;
					}
				}
				if (num5 == this.\u001A)
				{
					goto IL_773;
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
				IL_759:
				this.\u000E.gameObject.SetActive(false);
				this.\u001A = -1;
				IL_773:
				if (num5 != -1 && num5 != this.\u001A)
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
					this.\u001A = num5;
					TMP_LinkInfo tmp_LinkInfo = this.\u0013.textInfo.linkInfo[num5];
					Vector3 zero = Vector3.zero;
					RectTransformUtility.ScreenPointToWorldPointInRectangle(this.\u0013.rectTransform, Input.mousePosition, this.\u0009, out zero);
					string linkID = tmp_LinkInfo.GetLinkID();
					if (linkID != null)
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
						if (!(linkID == "id_01"))
						{
							if (!(linkID == "id_02"))
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
								this.\u000E.position = zero;
								this.\u000E.gameObject.SetActive(true);
								this.\u0012.text = "You have selected link <#ffff00> ID 02";
							}
						}
						else
						{
							this.\u000E.position = zero;
							this.\u000E.gameObject.SetActive(true);
							this.\u0012.text = "You have selected link <#ffff00> ID 01";
						}
					}
				}
			}
			else if (this.\u0004 != -1)
			{
				this.\u000F(this.\u0004);
				this.\u0004 = -1;
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			this.\u0019 = true;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			this.\u0019 = false;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
		}

		public void OnPointerUp(PointerEventData eventData)
		{
		}

		private void \u000F(int \u001D)
		{
			if (\u001D != -1)
			{
				if (\u001D <= this.\u0013.textInfo.characterCount - 1)
				{
					int materialReferenceIndex = this.\u0013.textInfo.characterInfo[\u001D].materialReferenceIndex;
					int vertexIndex = this.\u0013.textInfo.characterInfo[\u001D].vertexIndex;
					Vector3[] vertices = this.\u0003[materialReferenceIndex].vertices;
					Vector3[] vertices2 = this.\u0013.textInfo.meshInfo[materialReferenceIndex].vertices;
					vertices2[vertexIndex] = vertices[vertexIndex];
					vertices2[vertexIndex + 1] = vertices[vertexIndex + 1];
					vertices2[vertexIndex + 2] = vertices[vertexIndex + 2];
					vertices2[vertexIndex + 3] = vertices[vertexIndex + 3];
					Color32[] colors = this.\u0013.textInfo.meshInfo[materialReferenceIndex].colors32;
					Color32[] colors2 = this.\u0003[materialReferenceIndex].colors32;
					colors[vertexIndex] = colors2[vertexIndex];
					colors[vertexIndex + 1] = colors2[vertexIndex + 1];
					colors[vertexIndex + 2] = colors2[vertexIndex + 2];
					colors[vertexIndex + 3] = colors2[vertexIndex + 3];
					Vector2[] uvs = this.\u0003[materialReferenceIndex].uvs0;
					Vector2[] uvs2 = this.\u0013.textInfo.meshInfo[materialReferenceIndex].uvs0;
					uvs2[vertexIndex] = uvs[vertexIndex];
					uvs2[vertexIndex + 1] = uvs[vertexIndex + 1];
					uvs2[vertexIndex + 2] = uvs[vertexIndex + 2];
					uvs2[vertexIndex + 3] = uvs[vertexIndex + 3];
					Vector2[] uvs3 = this.\u0003[materialReferenceIndex].uvs2;
					Vector2[] uvs4 = this.\u0013.textInfo.meshInfo[materialReferenceIndex].uvs2;
					uvs4[vertexIndex] = uvs3[vertexIndex];
					uvs4[vertexIndex + 1] = uvs3[vertexIndex + 1];
					uvs4[vertexIndex + 2] = uvs3[vertexIndex + 2];
					uvs4[vertexIndex + 3] = uvs3[vertexIndex + 3];
					int num = (vertices.Length / 4 - 1) * 4;
					vertices2[num] = vertices[num];
					vertices2[num + 1] = vertices[num + 1];
					vertices2[num + 2] = vertices[num + 2];
					vertices2[num + 3] = vertices[num + 3];
					colors2 = this.\u0003[materialReferenceIndex].colors32;
					colors = this.\u0013.textInfo.meshInfo[materialReferenceIndex].colors32;
					colors[num] = colors2[num];
					colors[num + 1] = colors2[num + 1];
					colors[num + 2] = colors2[num + 2];
					colors[num + 3] = colors2[num + 3];
					uvs = this.\u0003[materialReferenceIndex].uvs0;
					uvs2 = this.\u0013.textInfo.meshInfo[materialReferenceIndex].uvs0;
					uvs2[num] = uvs[num];
					uvs2[num + 1] = uvs[num + 1];
					uvs2[num + 2] = uvs[num + 2];
					uvs2[num + 3] = uvs[num + 3];
					uvs3 = this.\u0003[materialReferenceIndex].uvs2;
					uvs4 = this.\u0013.textInfo.meshInfo[materialReferenceIndex].uvs2;
					uvs4[num] = uvs3[num];
					uvs4[num + 1] = uvs3[num + 1];
					uvs4[num + 2] = uvs3[num + 2];
					uvs4[num + 3] = uvs3[num + 3];
					this.\u0013.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
					return;
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_TextSelector_B.\u000F(int)).MethodHandle;
				}
			}
		}
	}
}
