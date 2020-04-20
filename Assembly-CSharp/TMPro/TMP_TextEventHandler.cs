using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TMPro
{
	public class TMP_TextEventHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
	{
		[SerializeField]
		private TMP_TextEventHandler.CharacterSelectionEvent m_OnCharacterSelection = new TMP_TextEventHandler.CharacterSelectionEvent();

		[SerializeField]
		private TMP_TextEventHandler.WordSelectionEvent m_OnWordSelection = new TMP_TextEventHandler.WordSelectionEvent();

		[SerializeField]
		private TMP_TextEventHandler.LineSelectionEvent m_OnLineSelection = new TMP_TextEventHandler.LineSelectionEvent();

		[SerializeField]
		private TMP_TextEventHandler.LinkSelectionEvent m_OnLinkSelection = new TMP_TextEventHandler.LinkSelectionEvent();

		private TMP_Text m_TextComponent;

		private Camera m_Camera;

		private Canvas m_Canvas;

		private int m_selectedLink = -1;

		private int m_lastCharIndex = -1;

		private int m_lastWordIndex = -1;

		private int m_lastLineIndex = -1;

		public TMP_TextEventHandler.CharacterSelectionEvent onCharacterSelection
		{
			get
			{
				return this.m_OnCharacterSelection;
			}
			set
			{
				this.m_OnCharacterSelection = value;
			}
		}

		public TMP_TextEventHandler.WordSelectionEvent onWordSelection
		{
			get
			{
				return this.m_OnWordSelection;
			}
			set
			{
				this.m_OnWordSelection = value;
			}
		}

		public TMP_TextEventHandler.LineSelectionEvent onLineSelection
		{
			get
			{
				return this.m_OnLineSelection;
			}
			set
			{
				this.m_OnLineSelection = value;
			}
		}

		public TMP_TextEventHandler.LinkSelectionEvent onLinkSelection
		{
			get
			{
				return this.m_OnLinkSelection;
			}
			set
			{
				this.m_OnLinkSelection = value;
			}
		}

		private void Awake()
		{
			this.m_TextComponent = base.gameObject.GetComponent<TMP_Text>();
			if (this.m_TextComponent.GetType() == typeof(TextMeshProUGUI))
			{
				this.m_Canvas = base.gameObject.GetComponentInParent<Canvas>();
				if (this.m_Canvas != null)
				{
					if (this.m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
					{
						this.m_Camera = null;
					}
					else
					{
						this.m_Camera = this.m_Canvas.worldCamera;
					}
				}
			}
			else
			{
				this.m_Camera = Camera.main;
			}
		}

		private void LateUpdate()
		{
			if (TMP_TextUtilities.IsIntersectingRectTransform(this.m_TextComponent.rectTransform, Input.mousePosition, this.m_Camera))
			{
				int num = TMP_TextUtilities.FindIntersectingCharacter(this.m_TextComponent, Input.mousePosition, this.m_Camera, true);
				if (num != -1)
				{
					if (num != this.m_lastCharIndex)
					{
						this.m_lastCharIndex = num;
						this.SendOnCharacterSelection(this.m_TextComponent.textInfo.characterInfo[num].character, num);
					}
				}
				int num2 = TMP_TextUtilities.FindIntersectingWord(this.m_TextComponent, Input.mousePosition, this.m_Camera);
				if (num2 != -1)
				{
					if (num2 != this.m_lastWordIndex)
					{
						this.m_lastWordIndex = num2;
						TMP_WordInfo tmp_WordInfo = this.m_TextComponent.textInfo.wordInfo[num2];
						this.SendOnWordSelection(tmp_WordInfo.GetWord(), tmp_WordInfo.firstCharacterIndex, tmp_WordInfo.characterCount);
					}
				}
				int num3 = TMP_TextUtilities.FindIntersectingLine(this.m_TextComponent, Input.mousePosition, this.m_Camera);
				if (num3 != -1)
				{
					if (num3 != this.m_lastLineIndex)
					{
						this.m_lastLineIndex = num3;
						TMP_LineInfo tmp_LineInfo = this.m_TextComponent.textInfo.lineInfo[num3];
						char[] array = new char[tmp_LineInfo.characterCount];
						for (int i = 0; i < tmp_LineInfo.characterCount; i++)
						{
							if (i >= this.m_TextComponent.textInfo.characterInfo.Length)
							{
								break;
							}
							array[i] = this.m_TextComponent.textInfo.characterInfo[i + tmp_LineInfo.firstCharacterIndex].character;
						}
						string line = new string(array);
						this.SendOnLineSelection(line, tmp_LineInfo.firstCharacterIndex, tmp_LineInfo.characterCount);
					}
				}
				int num4 = TMP_TextUtilities.FindIntersectingLink(this.m_TextComponent, Input.mousePosition, this.m_Camera);
				if (num4 != -1)
				{
					if (num4 != this.m_selectedLink)
					{
						this.m_selectedLink = num4;
						TMP_LinkInfo tmp_LinkInfo = this.m_TextComponent.textInfo.linkInfo[num4];
						this.SendOnLinkSelection(tmp_LinkInfo.GetLinkID(), tmp_LinkInfo.GetLinkText(), num4);
					}
				}
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
		}

		public void OnPointerExit(PointerEventData eventData)
		{
		}

		private void SendOnCharacterSelection(char character, int characterIndex)
		{
			if (this.onCharacterSelection != null)
			{
				this.onCharacterSelection.Invoke(character, characterIndex);
			}
		}

		private void SendOnWordSelection(string word, int charIndex, int length)
		{
			if (this.onWordSelection != null)
			{
				this.onWordSelection.Invoke(word, charIndex, length);
			}
		}

		private void SendOnLineSelection(string line, int charIndex, int length)
		{
			if (this.onLineSelection != null)
			{
				this.onLineSelection.Invoke(line, charIndex, length);
			}
		}

		private void SendOnLinkSelection(string linkID, string linkText, int linkIndex)
		{
			if (this.onLinkSelection != null)
			{
				this.onLinkSelection.Invoke(linkID, linkText, linkIndex);
			}
		}

		[Serializable]
		public class CharacterSelectionEvent : UnityEvent<char, int>
		{
		}

		[Serializable]
		public class WordSelectionEvent : UnityEvent<string, int, int>
		{
		}

		[Serializable]
		public class LineSelectionEvent : UnityEvent<string, int, int>
		{
		}

		[Serializable]
		public class LinkSelectionEvent : UnityEvent<string, string, int>
		{
		}
	}
}
