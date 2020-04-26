using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TMPro
{
	public class TMP_TextEventHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
	{
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

		[SerializeField]
		private CharacterSelectionEvent m_OnCharacterSelection = new CharacterSelectionEvent();

		[SerializeField]
		private WordSelectionEvent m_OnWordSelection = new WordSelectionEvent();

		[SerializeField]
		private LineSelectionEvent m_OnLineSelection = new LineSelectionEvent();

		[SerializeField]
		private LinkSelectionEvent m_OnLinkSelection = new LinkSelectionEvent();

		private TMP_Text m_TextComponent;

		private Camera m_Camera;

		private Canvas m_Canvas;

		private int m_selectedLink = -1;

		private int m_lastCharIndex = -1;

		private int m_lastWordIndex = -1;

		private int m_lastLineIndex = -1;

		public CharacterSelectionEvent onCharacterSelection
		{
			get
			{
				return m_OnCharacterSelection;
			}
			set
			{
				m_OnCharacterSelection = value;
			}
		}

		public WordSelectionEvent onWordSelection
		{
			get
			{
				return m_OnWordSelection;
			}
			set
			{
				m_OnWordSelection = value;
			}
		}

		public LineSelectionEvent onLineSelection
		{
			get
			{
				return m_OnLineSelection;
			}
			set
			{
				m_OnLineSelection = value;
			}
		}

		public LinkSelectionEvent onLinkSelection
		{
			get
			{
				return m_OnLinkSelection;
			}
			set
			{
				m_OnLinkSelection = value;
			}
		}

		private void Awake()
		{
			m_TextComponent = base.gameObject.GetComponent<TMP_Text>();
			if (m_TextComponent.GetType() == typeof(TextMeshProUGUI))
			{
				m_Canvas = base.gameObject.GetComponentInParent<Canvas>();
				if (!(m_Canvas != null))
				{
					return;
				}
				while (true)
				{
					if (m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								m_Camera = null;
								return;
							}
						}
					}
					m_Camera = m_Canvas.worldCamera;
					return;
				}
			}
			m_Camera = Camera.main;
		}

		private void LateUpdate()
		{
			if (!TMP_TextUtilities.IsIntersectingRectTransform(m_TextComponent.rectTransform, Input.mousePosition, m_Camera))
			{
				return;
			}
			while (true)
			{
				int num = TMP_TextUtilities.FindIntersectingCharacter(m_TextComponent, Input.mousePosition, m_Camera, true);
				if (num != -1)
				{
					if (num != m_lastCharIndex)
					{
						m_lastCharIndex = num;
						SendOnCharacterSelection(m_TextComponent.textInfo.characterInfo[num].character, num);
					}
				}
				int num2 = TMP_TextUtilities.FindIntersectingWord(m_TextComponent, Input.mousePosition, m_Camera);
				if (num2 != -1)
				{
					if (num2 != m_lastWordIndex)
					{
						m_lastWordIndex = num2;
						TMP_WordInfo tMP_WordInfo = m_TextComponent.textInfo.wordInfo[num2];
						SendOnWordSelection(tMP_WordInfo.GetWord(), tMP_WordInfo.firstCharacterIndex, tMP_WordInfo.characterCount);
					}
				}
				int num3 = TMP_TextUtilities.FindIntersectingLine(m_TextComponent, Input.mousePosition, m_Camera);
				if (num3 != -1)
				{
					if (num3 != m_lastLineIndex)
					{
						m_lastLineIndex = num3;
						TMP_LineInfo tMP_LineInfo = m_TextComponent.textInfo.lineInfo[num3];
						char[] array = new char[tMP_LineInfo.characterCount];
						for (int i = 0; i < tMP_LineInfo.characterCount; i++)
						{
							if (i >= m_TextComponent.textInfo.characterInfo.Length)
							{
								break;
							}
							array[i] = m_TextComponent.textInfo.characterInfo[i + tMP_LineInfo.firstCharacterIndex].character;
						}
						string line = new string(array);
						SendOnLineSelection(line, tMP_LineInfo.firstCharacterIndex, tMP_LineInfo.characterCount);
					}
				}
				int num4 = TMP_TextUtilities.FindIntersectingLink(m_TextComponent, Input.mousePosition, m_Camera);
				if (num4 == -1)
				{
					return;
				}
				while (true)
				{
					if (num4 != m_selectedLink)
					{
						while (true)
						{
							m_selectedLink = num4;
							TMP_LinkInfo tMP_LinkInfo = m_TextComponent.textInfo.linkInfo[num4];
							SendOnLinkSelection(tMP_LinkInfo.GetLinkID(), tMP_LinkInfo.GetLinkText(), num4);
							return;
						}
					}
					return;
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
			if (onCharacterSelection == null)
			{
				return;
			}
			while (true)
			{
				onCharacterSelection.Invoke(character, characterIndex);
				return;
			}
		}

		private void SendOnWordSelection(string word, int charIndex, int length)
		{
			if (onWordSelection == null)
			{
				return;
			}
			while (true)
			{
				onWordSelection.Invoke(word, charIndex, length);
				return;
			}
		}

		private void SendOnLineSelection(string line, int charIndex, int length)
		{
			if (onLineSelection != null)
			{
				onLineSelection.Invoke(line, charIndex, length);
			}
		}

		private void SendOnLinkSelection(string linkID, string linkText, int linkIndex)
		{
			if (onLinkSelection == null)
			{
				return;
			}
			while (true)
			{
				onLinkSelection.Invoke(linkID, linkText, linkIndex);
				return;
			}
		}
	}
}
