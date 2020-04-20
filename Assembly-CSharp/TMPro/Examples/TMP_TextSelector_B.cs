using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TMPro.Examples
{
	public class TMP_TextSelector_B : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerUpHandler, IEventSystemHandler
	{
		public RectTransform symbol_001D;

		private RectTransform symbol_000E;

		private TextMeshProUGUI symbol_0012;

		private const string symbol_0015 = "You have selected link <#ffff00>";

		private const string symbol_0016 = "Word Index: <#ffff00>";

		private TextMeshProUGUI symbol_0013;

		private Canvas symbol_0018;

		private Camera symbol_0009;

		private bool symbol_0019;

		private int symbol_0011 = -1;

		private int symbol_001A = -1;

		private int symbol_0004 = -1;

		private Matrix4x4 symbol_000B;

		private TMP_MeshInfo[] symbol_0003;

		private void symbol_000F()
		{
			this.symbol_0013 = base.gameObject.GetComponent<TextMeshProUGUI>();
			this.symbol_0018 = base.gameObject.GetComponentInParent<Canvas>();
			if (this.symbol_0018.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				this.symbol_0009 = null;
			}
			else
			{
				this.symbol_0009 = this.symbol_0018.worldCamera;
			}
			this.symbol_000E = UnityEngine.Object.Instantiate<RectTransform>(this.symbol_001D);
			this.symbol_000E.SetParent(this.symbol_0018.transform, false);
			this.symbol_0012 = this.symbol_000E.GetComponentInChildren<TextMeshProUGUI>();
			this.symbol_000E.gameObject.SetActive(false);
		}

		private void symbol_0017()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this.symbol_000F));
		}

		private void symbol_000D()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this.symbol_000F));
		}

		private void symbol_000F(UnityEngine.Object symbol_001D)
		{
			if (symbol_001D == this.symbol_0013)
			{
				this.symbol_0003 = this.symbol_0013.textInfo.CopyMeshInfoVertexData();
			}
		}

		private void symbol_0008()
		{
			if (this.symbol_0019)
			{
				int num = TMP_TextUtilities.FindIntersectingCharacter(this.symbol_0013, Input.mousePosition, this.symbol_0009, true);
				if (num != -1)
				{
					if (num == this.symbol_0004)
					{
						goto IL_6C;
					}
				}
				this.symbol_000F(this.symbol_0004);
				this.symbol_0004 = -1;
				IL_6C:
				if (num != -1)
				{
					if (num != this.symbol_0004)
					{
						if (!Input.GetKey(KeyCode.LeftShift))
						{
							if (!Input.GetKey(KeyCode.RightShift))
							{
								goto IL_406;
							}
						}
						this.symbol_0004 = num;
						int materialReferenceIndex = this.symbol_0013.textInfo.characterInfo[num].materialReferenceIndex;
						int vertexIndex = this.symbol_0013.textInfo.characterInfo[num].vertexIndex;
						Vector3[] vertices = this.symbol_0013.textInfo.meshInfo[materialReferenceIndex].vertices;
						Vector2 v = (vertices[vertexIndex] + vertices[vertexIndex + 2]) / 2f;
						Vector3 b = v;
						vertices[vertexIndex] -= b;
						vertices[vertexIndex + 1] = vertices[vertexIndex + 1] - b;
						vertices[vertexIndex + 2] = vertices[vertexIndex + 2] - b;
						vertices[vertexIndex + 3] = vertices[vertexIndex + 3] - b;
						float d = 1.5f;
						this.symbol_000B = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * d);
						vertices[vertexIndex] = this.symbol_000B.MultiplyPoint3x4(vertices[vertexIndex]);
						vertices[vertexIndex + 1] = this.symbol_000B.MultiplyPoint3x4(vertices[vertexIndex + 1]);
						vertices[vertexIndex + 2] = this.symbol_000B.MultiplyPoint3x4(vertices[vertexIndex + 2]);
						vertices[vertexIndex + 3] = this.symbol_000B.MultiplyPoint3x4(vertices[vertexIndex + 3]);
						vertices[vertexIndex] += b;
						vertices[vertexIndex + 1] = vertices[vertexIndex + 1] + b;
						vertices[vertexIndex + 2] = vertices[vertexIndex + 2] + b;
						vertices[vertexIndex + 3] = vertices[vertexIndex + 3] + b;
						Color32 color = new Color32(byte.MaxValue, byte.MaxValue, 0xC0, byte.MaxValue);
						Color32[] colors = this.symbol_0013.textInfo.meshInfo[materialReferenceIndex].colors32;
						colors[vertexIndex] = color;
						colors[vertexIndex + 1] = color;
						colors[vertexIndex + 2] = color;
						colors[vertexIndex + 3] = color;
						TMP_MeshInfo tmp_MeshInfo = this.symbol_0013.textInfo.meshInfo[materialReferenceIndex];
						int dst = vertices.Length - 4;
						tmp_MeshInfo.SwapVertexData(vertexIndex, dst);
						this.symbol_0013.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
					}
				}
				IL_406:
				int num2 = TMP_TextUtilities.FindIntersectingWord(this.symbol_0013, Input.mousePosition, this.symbol_0009);
				if (this.symbol_000E != null)
				{
					if (this.symbol_0011 != -1)
					{
						if (num2 != -1)
						{
							if (num2 == this.symbol_0011)
							{
								goto IL_59E;
							}
						}
						TMP_WordInfo tmp_WordInfo = this.symbol_0013.textInfo.wordInfo[this.symbol_0011];
						for (int i = 0; i < tmp_WordInfo.characterCount; i++)
						{
							int num3 = tmp_WordInfo.firstCharacterIndex + i;
							int materialReferenceIndex2 = this.symbol_0013.textInfo.characterInfo[num3].materialReferenceIndex;
							int vertexIndex2 = this.symbol_0013.textInfo.characterInfo[num3].vertexIndex;
							Color32[] colors2 = this.symbol_0013.textInfo.meshInfo[materialReferenceIndex2].colors32;
							Color32 color2 = colors2[vertexIndex2].Tint(1.33333f);
							colors2[vertexIndex2] = color2;
							colors2[vertexIndex2 + 1] = color2;
							colors2[vertexIndex2 + 2] = color2;
							colors2[vertexIndex2 + 3] = color2;
						}
						this.symbol_0013.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
						this.symbol_0011 = -1;
					}
				}
				IL_59E:
				if (num2 != -1 && num2 != this.symbol_0011)
				{
					if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
					{
						this.symbol_0011 = num2;
						TMP_WordInfo tmp_WordInfo2 = this.symbol_0013.textInfo.wordInfo[num2];
						for (int j = 0; j < tmp_WordInfo2.characterCount; j++)
						{
							int num4 = tmp_WordInfo2.firstCharacterIndex + j;
							int materialReferenceIndex3 = this.symbol_0013.textInfo.characterInfo[num4].materialReferenceIndex;
							int vertexIndex3 = this.symbol_0013.textInfo.characterInfo[num4].vertexIndex;
							Color32[] colors3 = this.symbol_0013.textInfo.meshInfo[materialReferenceIndex3].colors32;
							Color32 color3 = colors3[vertexIndex3].Tint(0.75f);
							colors3[vertexIndex3] = color3;
							colors3[vertexIndex3 + 1] = color3;
							colors3[vertexIndex3 + 2] = color3;
							colors3[vertexIndex3 + 3] = color3;
						}
						this.symbol_0013.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
					}
				}
				int num5 = TMP_TextUtilities.FindIntersectingLink(this.symbol_0013, Input.mousePosition, this.symbol_0009);
				if (num5 == -1)
				{
					if (this.symbol_001A != -1)
					{
						goto IL_759;
					}
				}
				if (num5 == this.symbol_001A)
				{
					goto IL_773;
				}
				IL_759:
				this.symbol_000E.gameObject.SetActive(false);
				this.symbol_001A = -1;
				IL_773:
				if (num5 != -1 && num5 != this.symbol_001A)
				{
					this.symbol_001A = num5;
					TMP_LinkInfo tmp_LinkInfo = this.symbol_0013.textInfo.linkInfo[num5];
					Vector3 zero = Vector3.zero;
					RectTransformUtility.ScreenPointToWorldPointInRectangle(this.symbol_0013.rectTransform, Input.mousePosition, this.symbol_0009, out zero);
					string linkID = tmp_LinkInfo.GetLinkID();
					if (linkID != null)
					{
						if (!(linkID == "id_01"))
						{
							if (!(linkID == "id_02"))
							{
							}
							else
							{
								this.symbol_000E.position = zero;
								this.symbol_000E.gameObject.SetActive(true);
								this.symbol_0012.text = "You have selected link <#ffff00> ID 02";
							}
						}
						else
						{
							this.symbol_000E.position = zero;
							this.symbol_000E.gameObject.SetActive(true);
							this.symbol_0012.text = "You have selected link <#ffff00> ID 01";
						}
					}
				}
			}
			else if (this.symbol_0004 != -1)
			{
				this.symbol_000F(this.symbol_0004);
				this.symbol_0004 = -1;
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			this.symbol_0019 = true;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			this.symbol_0019 = false;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
		}

		public void OnPointerUp(PointerEventData eventData)
		{
		}

		private void symbol_000F(int symbol_001D)
		{
			if (symbol_001D != -1)
			{
				if (symbol_001D <= this.symbol_0013.textInfo.characterCount - 1)
				{
					int materialReferenceIndex = this.symbol_0013.textInfo.characterInfo[symbol_001D].materialReferenceIndex;
					int vertexIndex = this.symbol_0013.textInfo.characterInfo[symbol_001D].vertexIndex;
					Vector3[] vertices = this.symbol_0003[materialReferenceIndex].vertices;
					Vector3[] vertices2 = this.symbol_0013.textInfo.meshInfo[materialReferenceIndex].vertices;
					vertices2[vertexIndex] = vertices[vertexIndex];
					vertices2[vertexIndex + 1] = vertices[vertexIndex + 1];
					vertices2[vertexIndex + 2] = vertices[vertexIndex + 2];
					vertices2[vertexIndex + 3] = vertices[vertexIndex + 3];
					Color32[] colors = this.symbol_0013.textInfo.meshInfo[materialReferenceIndex].colors32;
					Color32[] colors2 = this.symbol_0003[materialReferenceIndex].colors32;
					colors[vertexIndex] = colors2[vertexIndex];
					colors[vertexIndex + 1] = colors2[vertexIndex + 1];
					colors[vertexIndex + 2] = colors2[vertexIndex + 2];
					colors[vertexIndex + 3] = colors2[vertexIndex + 3];
					Vector2[] uvs = this.symbol_0003[materialReferenceIndex].uvs0;
					Vector2[] uvs2 = this.symbol_0013.textInfo.meshInfo[materialReferenceIndex].uvs0;
					uvs2[vertexIndex] = uvs[vertexIndex];
					uvs2[vertexIndex + 1] = uvs[vertexIndex + 1];
					uvs2[vertexIndex + 2] = uvs[vertexIndex + 2];
					uvs2[vertexIndex + 3] = uvs[vertexIndex + 3];
					Vector2[] uvs3 = this.symbol_0003[materialReferenceIndex].uvs2;
					Vector2[] uvs4 = this.symbol_0013.textInfo.meshInfo[materialReferenceIndex].uvs2;
					uvs4[vertexIndex] = uvs3[vertexIndex];
					uvs4[vertexIndex + 1] = uvs3[vertexIndex + 1];
					uvs4[vertexIndex + 2] = uvs3[vertexIndex + 2];
					uvs4[vertexIndex + 3] = uvs3[vertexIndex + 3];
					int num = (vertices.Length / 4 - 1) * 4;
					vertices2[num] = vertices[num];
					vertices2[num + 1] = vertices[num + 1];
					vertices2[num + 2] = vertices[num + 2];
					vertices2[num + 3] = vertices[num + 3];
					colors2 = this.symbol_0003[materialReferenceIndex].colors32;
					colors = this.symbol_0013.textInfo.meshInfo[materialReferenceIndex].colors32;
					colors[num] = colors2[num];
					colors[num + 1] = colors2[num + 1];
					colors[num + 2] = colors2[num + 2];
					colors[num + 3] = colors2[num + 3];
					uvs = this.symbol_0003[materialReferenceIndex].uvs0;
					uvs2 = this.symbol_0013.textInfo.meshInfo[materialReferenceIndex].uvs0;
					uvs2[num] = uvs[num];
					uvs2[num + 1] = uvs[num + 1];
					uvs2[num + 2] = uvs[num + 2];
					uvs2[num + 3] = uvs[num + 3];
					uvs3 = this.symbol_0003[materialReferenceIndex].uvs2;
					uvs4 = this.symbol_0013.textInfo.meshInfo[materialReferenceIndex].uvs2;
					uvs4[num] = uvs3[num];
					uvs4[num + 1] = uvs3[num + 1];
					uvs4[num + 2] = uvs3[num + 2];
					uvs4[num + 3] = uvs3[num + 3];
					this.symbol_0013.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
					return;
				}
			}
		}
	}
}
