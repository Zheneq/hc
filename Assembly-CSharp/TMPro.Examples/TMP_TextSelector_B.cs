using UnityEngine;
using UnityEngine.EventSystems;

namespace TMPro.Examples
{
	public class TMP_TextSelector_B : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerUpHandler, IEventSystemHandler
	{
		public RectTransform _001D;

		private RectTransform _000E;

		private TextMeshProUGUI _0012;

		private const string _0015 = "You have selected link <#ffff00>";

		private const string _0016 = "Word Index: <#ffff00>";

		private TextMeshProUGUI _0013;

		private Canvas _0018;

		private Camera _0009;

		private bool _0019;

		private int _0011 = -1;

		private int _001A = -1;

		private int _0004 = -1;

		private Matrix4x4 _000B;

		private TMP_MeshInfo[] _0003;

		private void _000F()
		{
			_0013 = base.gameObject.GetComponent<TextMeshProUGUI>();
			_0018 = base.gameObject.GetComponentInParent<Canvas>();
			if (_0018.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				_0009 = null;
			}
			else
			{
				_0009 = _0018.worldCamera;
			}
			_000E = Object.Instantiate(_001D);
			_000E.SetParent(_0018.transform, false);
			_0012 = _000E.GetComponentInChildren<TextMeshProUGUI>();
			_000E.gameObject.SetActive(false);
		}

		private void _0017()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(_000F);
		}

		private void _000D()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(_000F);
		}

		private void _000F(Object _001D)
		{
			if (!(_001D == _0013))
			{
				return;
			}
			while (true)
			{
				_0003 = _0013.textInfo.CopyMeshInfoVertexData();
				return;
			}
		}

		private void _0008()
		{
			if (_0019)
			{
				while (true)
				{
					int num;
					int num2;
					int num4;
					switch (7)
					{
					case 0:
						break;
					default:
						{
							num = TMP_TextUtilities.FindIntersectingCharacter(_0013, Input.mousePosition, _0009, true);
							if (num != -1)
							{
								if (num == _0004)
								{
									goto IL_006c;
								}
							}
							_000F(_0004);
							_0004 = -1;
							goto IL_006c;
						}
						IL_006c:
						if (num != -1)
						{
							if (num != _0004)
							{
								if (!Input.GetKey(KeyCode.LeftShift))
								{
									if (!Input.GetKey(KeyCode.RightShift))
									{
										goto IL_0406;
									}
								}
								_0004 = num;
								int materialReferenceIndex = _0013.textInfo.characterInfo[num].materialReferenceIndex;
								int vertexIndex = _0013.textInfo.characterInfo[num].vertexIndex;
								Vector3[] vertices = _0013.textInfo.meshInfo[materialReferenceIndex].vertices;
								Vector2 v = (vertices[vertexIndex] + vertices[vertexIndex + 2]) / 2f;
								Vector3 b = v;
								vertices[vertexIndex] -= b;
								vertices[vertexIndex + 1] = vertices[vertexIndex + 1] - b;
								vertices[vertexIndex + 2] = vertices[vertexIndex + 2] - b;
								vertices[vertexIndex + 3] = vertices[vertexIndex + 3] - b;
								float d = 1.5f;
								_000B = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * d);
								vertices[vertexIndex] = _000B.MultiplyPoint3x4(vertices[vertexIndex]);
								vertices[vertexIndex + 1] = _000B.MultiplyPoint3x4(vertices[vertexIndex + 1]);
								vertices[vertexIndex + 2] = _000B.MultiplyPoint3x4(vertices[vertexIndex + 2]);
								vertices[vertexIndex + 3] = _000B.MultiplyPoint3x4(vertices[vertexIndex + 3]);
								vertices[vertexIndex] += b;
								vertices[vertexIndex + 1] = vertices[vertexIndex + 1] + b;
								vertices[vertexIndex + 2] = vertices[vertexIndex + 2] + b;
								vertices[vertexIndex + 3] = vertices[vertexIndex + 3] + b;
								Color32 color = new Color32(byte.MaxValue, byte.MaxValue, 192, byte.MaxValue);
								Color32[] colors = _0013.textInfo.meshInfo[materialReferenceIndex].colors32;
								colors[vertexIndex] = color;
								colors[vertexIndex + 1] = color;
								colors[vertexIndex + 2] = color;
								colors[vertexIndex + 3] = color;
								TMP_MeshInfo tMP_MeshInfo = _0013.textInfo.meshInfo[materialReferenceIndex];
								int dst = vertices.Length - 4;
								tMP_MeshInfo.SwapVertexData(vertexIndex, dst);
								_0013.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
							}
						}
						goto IL_0406;
						IL_059e:
						if (num2 != -1 && num2 != _0011)
						{
							if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
							{
								_0011 = num2;
								TMP_WordInfo tMP_WordInfo = _0013.textInfo.wordInfo[num2];
								for (int i = 0; i < tMP_WordInfo.characterCount; i++)
								{
									int num3 = tMP_WordInfo.firstCharacterIndex + i;
									int materialReferenceIndex2 = _0013.textInfo.characterInfo[num3].materialReferenceIndex;
									int vertexIndex2 = _0013.textInfo.characterInfo[num3].vertexIndex;
									Color32[] colors2 = _0013.textInfo.meshInfo[materialReferenceIndex2].colors32;
									Color32 color2 = colors2[vertexIndex2].Tint(0.75f);
									colors2[vertexIndex2] = color2;
									colors2[vertexIndex2 + 1] = color2;
									colors2[vertexIndex2 + 2] = color2;
									colors2[vertexIndex2 + 3] = color2;
								}
								_0013.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
							}
						}
						num4 = TMP_TextUtilities.FindIntersectingLink(_0013, Input.mousePosition, _0009);
						if (num4 == -1)
						{
							if (_001A != -1)
							{
								goto IL_0759;
							}
						}
						if (num4 != _001A)
						{
							goto IL_0759;
						}
						goto IL_0773;
						IL_0759:
						_000E.gameObject.SetActive(false);
						_001A = -1;
						goto IL_0773;
						IL_0773:
						if (num4 != -1 && num4 != _001A)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									break;
								default:
								{
									_001A = num4;
									TMP_LinkInfo tMP_LinkInfo = _0013.textInfo.linkInfo[num4];
									Vector3 worldPoint = Vector3.zero;
									RectTransformUtility.ScreenPointToWorldPointInRectangle(_0013.rectTransform, Input.mousePosition, _0009, out worldPoint);
									string linkID = tMP_LinkInfo.GetLinkID();
									if (linkID != null)
									{
										while (true)
										{
											switch (7)
											{
											case 0:
												break;
											default:
												if (!(linkID == "id_01"))
												{
													if (!(linkID == "id_02"))
													{
														while (true)
														{
															switch (4)
															{
															default:
																return;
															case 0:
																break;
															}
														}
													}
													_000E.position = worldPoint;
													_000E.gameObject.SetActive(true);
													_0012.text = "You have selected link <#ffff00> ID 02";
												}
												else
												{
													_000E.position = worldPoint;
													_000E.gameObject.SetActive(true);
													_0012.text = "You have selected link <#ffff00> ID 01";
												}
												return;
											}
										}
									}
									return;
								}
								}
							}
						}
						return;
						IL_0406:
						num2 = TMP_TextUtilities.FindIntersectingWord(_0013, Input.mousePosition, _0009);
						if (_000E != null)
						{
							if (_0011 != -1)
							{
								if (num2 != -1)
								{
									if (num2 == _0011)
									{
										goto IL_059e;
									}
								}
								TMP_WordInfo tMP_WordInfo2 = _0013.textInfo.wordInfo[_0011];
								for (int j = 0; j < tMP_WordInfo2.characterCount; j++)
								{
									int num5 = tMP_WordInfo2.firstCharacterIndex + j;
									int materialReferenceIndex3 = _0013.textInfo.characterInfo[num5].materialReferenceIndex;
									int vertexIndex3 = _0013.textInfo.characterInfo[num5].vertexIndex;
									Color32[] colors3 = _0013.textInfo.meshInfo[materialReferenceIndex3].colors32;
									Color32 color3 = colors3[vertexIndex3].Tint(1.33333f);
									colors3[vertexIndex3] = color3;
									colors3[vertexIndex3 + 1] = color3;
									colors3[vertexIndex3 + 2] = color3;
									colors3[vertexIndex3 + 3] = color3;
								}
								_0013.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
								_0011 = -1;
							}
						}
						goto IL_059e;
					}
				}
			}
			if (_0004 != -1)
			{
				_000F(_0004);
				_0004 = -1;
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			_0019 = true;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			_0019 = false;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
		}

		public void OnPointerUp(PointerEventData eventData)
		{
		}

		private void _000F(int _001D)
		{
			if (_001D == -1)
			{
				return;
			}
			if (_001D > _0013.textInfo.characterCount - 1)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return;
					}
				}
			}
			int materialReferenceIndex = _0013.textInfo.characterInfo[_001D].materialReferenceIndex;
			int vertexIndex = _0013.textInfo.characterInfo[_001D].vertexIndex;
			Vector3[] vertices = _0003[materialReferenceIndex].vertices;
			Vector3[] vertices2 = _0013.textInfo.meshInfo[materialReferenceIndex].vertices;
			vertices2[vertexIndex] = vertices[vertexIndex];
			vertices2[vertexIndex + 1] = vertices[vertexIndex + 1];
			vertices2[vertexIndex + 2] = vertices[vertexIndex + 2];
			vertices2[vertexIndex + 3] = vertices[vertexIndex + 3];
			Color32[] colors = _0013.textInfo.meshInfo[materialReferenceIndex].colors32;
			Color32[] colors2 = _0003[materialReferenceIndex].colors32;
			colors[vertexIndex] = colors2[vertexIndex];
			colors[vertexIndex + 1] = colors2[vertexIndex + 1];
			colors[vertexIndex + 2] = colors2[vertexIndex + 2];
			colors[vertexIndex + 3] = colors2[vertexIndex + 3];
			Vector2[] uvs = _0003[materialReferenceIndex].uvs0;
			Vector2[] uvs2 = _0013.textInfo.meshInfo[materialReferenceIndex].uvs0;
			uvs2[vertexIndex] = uvs[vertexIndex];
			uvs2[vertexIndex + 1] = uvs[vertexIndex + 1];
			uvs2[vertexIndex + 2] = uvs[vertexIndex + 2];
			uvs2[vertexIndex + 3] = uvs[vertexIndex + 3];
			Vector2[] uvs3 = _0003[materialReferenceIndex].uvs2;
			Vector2[] uvs4 = _0013.textInfo.meshInfo[materialReferenceIndex].uvs2;
			uvs4[vertexIndex] = uvs3[vertexIndex];
			uvs4[vertexIndex + 1] = uvs3[vertexIndex + 1];
			uvs4[vertexIndex + 2] = uvs3[vertexIndex + 2];
			uvs4[vertexIndex + 3] = uvs3[vertexIndex + 3];
			int num = (vertices.Length / 4 - 1) * 4;
			vertices2[num] = vertices[num];
			vertices2[num + 1] = vertices[num + 1];
			vertices2[num + 2] = vertices[num + 2];
			vertices2[num + 3] = vertices[num + 3];
			colors2 = _0003[materialReferenceIndex].colors32;
			colors = _0013.textInfo.meshInfo[materialReferenceIndex].colors32;
			colors[num] = colors2[num];
			colors[num + 1] = colors2[num + 1];
			colors[num + 2] = colors2[num + 2];
			colors[num + 3] = colors2[num + 3];
			uvs = _0003[materialReferenceIndex].uvs0;
			uvs2 = _0013.textInfo.meshInfo[materialReferenceIndex].uvs0;
			uvs2[num] = uvs[num];
			uvs2[num + 1] = uvs[num + 1];
			uvs2[num + 2] = uvs[num + 2];
			uvs2[num + 3] = uvs[num + 3];
			uvs3 = _0003[materialReferenceIndex].uvs2;
			uvs4 = _0013.textInfo.meshInfo[materialReferenceIndex].uvs2;
			uvs4[num] = uvs3[num];
			uvs4[num + 1] = uvs3[num + 1];
			uvs4[num + 2] = uvs3[num + 2];
			uvs4[num + 3] = uvs3[num + 3];
			_0013.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
		}
	}
}
