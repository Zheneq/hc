using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TMPro.Examples
{
	public class TMP_TextSelector_A : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
	{
		private TextMeshPro symbol_001D;

		private Camera symbol_000E;

		private bool symbol_0012;

		private int symbol_0015 = -1;

		private int symbol_0016 = -1;

		private int symbol_0013 = -1;

		private void symbol_0018()
		{
			this.symbol_001D = base.gameObject.GetComponent<TextMeshPro>();
			this.symbol_000E = Camera.main;
			this.symbol_001D.ForceMeshUpdate();
		}

		private void symbol_0009()
		{
			this.symbol_0012 = false;
			if (TMP_TextUtilities.IsIntersectingRectTransform(this.symbol_001D.rectTransform, Input.mousePosition, Camera.main))
			{
				this.symbol_0012 = true;
			}
			if (this.symbol_0012)
			{
				int num = TMP_TextUtilities.FindIntersectingCharacter(this.symbol_001D, Input.mousePosition, Camera.main, true);
				if (num != -1)
				{
					if (num != this.symbol_0016)
					{
						if (!Input.GetKey(KeyCode.LeftShift))
						{
							if (!Input.GetKey(KeyCode.RightShift))
							{
								goto IL_1B9;
							}
						}
						this.symbol_0016 = num;
						int materialReferenceIndex = this.symbol_001D.textInfo.characterInfo[num].materialReferenceIndex;
						int vertexIndex = this.symbol_001D.textInfo.characterInfo[num].vertexIndex;
						Color32 color = new Color32((byte)UnityEngine.Random.Range(0, 0xFF), (byte)UnityEngine.Random.Range(0, 0xFF), (byte)UnityEngine.Random.Range(0, 0xFF), byte.MaxValue);
						Color32[] colors = this.symbol_001D.textInfo.meshInfo[materialReferenceIndex].colors32;
						colors[vertexIndex] = color;
						colors[vertexIndex + 1] = color;
						colors[vertexIndex + 2] = color;
						colors[vertexIndex + 3] = color;
						this.symbol_001D.textInfo.meshInfo[materialReferenceIndex].mesh.colors32 = colors;
					}
				}
				IL_1B9:
				int num2 = TMP_TextUtilities.FindIntersectingLink(this.symbol_001D, Input.mousePosition, this.symbol_000E);
				if (num2 != -1 || this.symbol_0015 == -1)
				{
					if (num2 == this.symbol_0015)
					{
						goto IL_1FE;
					}
				}
				this.symbol_0015 = -1;
				IL_1FE:
				if (num2 != -1)
				{
					if (num2 != this.symbol_0015)
					{
						this.symbol_0015 = num2;
						TMP_LinkInfo tmp_LinkInfo = this.symbol_001D.textInfo.linkInfo[num2];
						Debug.Log(string.Concat(new string[]
						{
							"Link ID: \"",
							tmp_LinkInfo.GetLinkID(),
							"\"   Link Text: \"",
							tmp_LinkInfo.GetLinkText(),
							"\""
						}));
						Vector3 zero = Vector3.zero;
						RectTransformUtility.ScreenPointToWorldPointInRectangle(this.symbol_001D.rectTransform, Input.mousePosition, this.symbol_000E, out zero);
						string linkID = tmp_LinkInfo.GetLinkID();
						if (linkID != null)
						{
							if (!(linkID == "id_01"))
							{
								if (!(linkID == "id_02"))
								{
								}
							}
						}
					}
				}
				int num3 = TMP_TextUtilities.FindIntersectingWord(this.symbol_001D, Input.mousePosition, Camera.main);
				if (num3 != -1 && num3 != this.symbol_0013)
				{
					this.symbol_0013 = num3;
					TMP_WordInfo tmp_WordInfo = this.symbol_001D.textInfo.wordInfo[num3];
					Vector3 position = this.symbol_001D.transform.TransformPoint(this.symbol_001D.textInfo.characterInfo[tmp_WordInfo.firstCharacterIndex].bottomLeft);
					position = Camera.main.WorldToScreenPoint(position);
					Color32[] colors2 = this.symbol_001D.textInfo.meshInfo[0].colors32;
					Color32 color2 = new Color32((byte)UnityEngine.Random.Range(0, 0xFF), (byte)UnityEngine.Random.Range(0, 0xFF), (byte)UnityEngine.Random.Range(0, 0xFF), byte.MaxValue);
					for (int i = 0; i < tmp_WordInfo.characterCount; i++)
					{
						int vertexIndex2 = this.symbol_001D.textInfo.characterInfo[tmp_WordInfo.firstCharacterIndex + i].vertexIndex;
						colors2[vertexIndex2] = color2;
						colors2[vertexIndex2 + 1] = color2;
						colors2[vertexIndex2 + 2] = color2;
						colors2[vertexIndex2 + 3] = color2;
					}
					this.symbol_001D.mesh.colors32 = colors2;
				}
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			Debug.Log("OnPointerEnter()");
			this.symbol_0012 = true;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			Debug.Log("OnPointerExit()");
			this.symbol_0012 = false;
		}
	}
}
