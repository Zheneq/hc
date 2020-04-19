using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TMPro.Examples
{
	public class TMP_TextSelector_A : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
	{
		private TextMeshPro \u001D;

		private Camera \u000E;

		private bool \u0012;

		private int \u0015 = -1;

		private int \u0016 = -1;

		private int \u0013 = -1;

		private void \u0018()
		{
			this.\u001D = base.gameObject.GetComponent<TextMeshPro>();
			this.\u000E = Camera.main;
			this.\u001D.ForceMeshUpdate();
		}

		private void \u0009()
		{
			this.\u0012 = false;
			if (TMP_TextUtilities.IsIntersectingRectTransform(this.\u001D.rectTransform, Input.mousePosition, Camera.main))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_TextSelector_A.\u0009()).MethodHandle;
				}
				this.\u0012 = true;
			}
			if (this.\u0012)
			{
				int num = TMP_TextUtilities.FindIntersectingCharacter(this.\u001D, Input.mousePosition, Camera.main, true);
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
					if (num != this.\u0016)
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
						if (!Input.GetKey(KeyCode.LeftShift))
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
							if (!Input.GetKey(KeyCode.RightShift))
							{
								goto IL_1B9;
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
						this.\u0016 = num;
						int materialReferenceIndex = this.\u001D.textInfo.characterInfo[num].materialReferenceIndex;
						int vertexIndex = this.\u001D.textInfo.characterInfo[num].vertexIndex;
						Color32 color = new Color32((byte)UnityEngine.Random.Range(0, 0xFF), (byte)UnityEngine.Random.Range(0, 0xFF), (byte)UnityEngine.Random.Range(0, 0xFF), byte.MaxValue);
						Color32[] colors = this.\u001D.textInfo.meshInfo[materialReferenceIndex].colors32;
						colors[vertexIndex] = color;
						colors[vertexIndex + 1] = color;
						colors[vertexIndex + 2] = color;
						colors[vertexIndex + 3] = color;
						this.\u001D.textInfo.meshInfo[materialReferenceIndex].mesh.colors32 = colors;
					}
				}
				IL_1B9:
				int num2 = TMP_TextUtilities.FindIntersectingLink(this.\u001D, Input.mousePosition, this.\u000E);
				if (num2 != -1 || this.\u0015 == -1)
				{
					if (num2 == this.\u0015)
					{
						goto IL_1FE;
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
				this.\u0015 = -1;
				IL_1FE:
				if (num2 != -1)
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
					if (num2 != this.\u0015)
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
						this.\u0015 = num2;
						TMP_LinkInfo tmp_LinkInfo = this.\u001D.textInfo.linkInfo[num2];
						Debug.Log(string.Concat(new string[]
						{
							"Link ID: \"",
							tmp_LinkInfo.GetLinkID(),
							"\"   Link Text: \"",
							tmp_LinkInfo.GetLinkText(),
							"\""
						}));
						Vector3 zero = Vector3.zero;
						RectTransformUtility.ScreenPointToWorldPointInRectangle(this.\u001D.rectTransform, Input.mousePosition, this.\u000E, out zero);
						string linkID = tmp_LinkInfo.GetLinkID();
						if (linkID != null)
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
							if (!(linkID == "id_01"))
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
								if (!(linkID == "id_02"))
								{
								}
							}
						}
					}
				}
				int num3 = TMP_TextUtilities.FindIntersectingWord(this.\u001D, Input.mousePosition, Camera.main);
				if (num3 != -1 && num3 != this.\u0013)
				{
					this.\u0013 = num3;
					TMP_WordInfo tmp_WordInfo = this.\u001D.textInfo.wordInfo[num3];
					Vector3 position = this.\u001D.transform.TransformPoint(this.\u001D.textInfo.characterInfo[tmp_WordInfo.firstCharacterIndex].bottomLeft);
					position = Camera.main.WorldToScreenPoint(position);
					Color32[] colors2 = this.\u001D.textInfo.meshInfo[0].colors32;
					Color32 color2 = new Color32((byte)UnityEngine.Random.Range(0, 0xFF), (byte)UnityEngine.Random.Range(0, 0xFF), (byte)UnityEngine.Random.Range(0, 0xFF), byte.MaxValue);
					for (int i = 0; i < tmp_WordInfo.characterCount; i++)
					{
						int vertexIndex2 = this.\u001D.textInfo.characterInfo[tmp_WordInfo.firstCharacterIndex + i].vertexIndex;
						colors2[vertexIndex2] = color2;
						colors2[vertexIndex2 + 1] = color2;
						colors2[vertexIndex2 + 2] = color2;
						colors2[vertexIndex2 + 3] = color2;
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
					this.\u001D.mesh.colors32 = colors2;
				}
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			Debug.Log("OnPointerEnter()");
			this.\u0012 = true;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			Debug.Log("OnPointerExit()");
			this.\u0012 = false;
		}
	}
}
