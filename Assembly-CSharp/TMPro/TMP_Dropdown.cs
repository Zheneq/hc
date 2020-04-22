using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TMPro
{
	[AddComponentMenu("UI/TMP Dropdown", 35)]
	[RequireComponent(typeof(RectTransform))]
	public class TMP_Dropdown : Selectable, IPointerClickHandler, ISubmitHandler, ICancelHandler, IEventSystemHandler
	{
		protected internal class DropdownItem : MonoBehaviour, IPointerEnterHandler, ICancelHandler, IEventSystemHandler
		{
			[SerializeField]
			private TMP_Text m_Text;

			[SerializeField]
			private Image m_Image;

			[SerializeField]
			private RectTransform m_RectTransform;

			[SerializeField]
			private Toggle m_Toggle;

			public TMP_Text text
			{
				get
				{
					return m_Text;
				}
				set
				{
					m_Text = value;
				}
			}

			public Image image
			{
				get
				{
					return m_Image;
				}
				set
				{
					m_Image = value;
				}
			}

			public RectTransform rectTransform
			{
				get
				{
					return m_RectTransform;
				}
				set
				{
					m_RectTransform = value;
				}
			}

			public Toggle toggle
			{
				get
				{
					return m_Toggle;
				}
				set
				{
					m_Toggle = value;
				}
			}

			public virtual void OnPointerEnter(PointerEventData eventData)
			{
				EventSystem.current.SetSelectedGameObject(base.gameObject);
			}

			public virtual void OnCancel(BaseEventData eventData)
			{
				TMP_Dropdown componentInParent = GetComponentInParent<TMP_Dropdown>();
				if (!componentInParent)
				{
					return;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					componentInParent.Hide();
					return;
				}
			}
		}

		[Serializable]
		public class OptionData
		{
			[SerializeField]
			private string m_Text;

			[SerializeField]
			private Sprite m_Image;

			public string text
			{
				get
				{
					return m_Text;
				}
				set
				{
					m_Text = value;
				}
			}

			public Sprite image
			{
				get
				{
					return m_Image;
				}
				set
				{
					m_Image = value;
				}
			}

			public OptionData()
			{
			}

			public OptionData(string text)
			{
				this.text = text;
			}

			public OptionData(Sprite image)
			{
				this.image = image;
			}

			public OptionData(string text, Sprite image)
			{
				this.text = text;
				this.image = image;
			}
		}

		[Serializable]
		public class OptionDataList
		{
			[SerializeField]
			private List<OptionData> m_Options;

			public List<OptionData> options
			{
				get
				{
					return m_Options;
				}
				set
				{
					m_Options = value;
				}
			}

			public OptionDataList()
			{
				options = new List<OptionData>();
			}
		}

		[Serializable]
		public class DropdownEvent : UnityEvent<int>
		{
		}

		[SerializeField]
		private RectTransform m_Template;

		[SerializeField]
		private TMP_Text m_CaptionText;

		[SerializeField]
		private Image m_CaptionImage;

		[Space]
		[SerializeField]
		private TMP_Text m_ItemText;

		[SerializeField]
		private Image m_ItemImage;

		[Space]
		[SerializeField]
		private int m_Value;

		[SerializeField]
		[Space]
		private OptionDataList m_Options = new OptionDataList();

		[Space]
		[SerializeField]
		private DropdownEvent m_OnValueChanged = new DropdownEvent();

		private GameObject m_Dropdown;

		private GameObject m_Blocker;

		private List<DropdownItem> m_Items = new List<DropdownItem>();

		private TweenRunner<FloatTween> m_AlphaTweenRunner;

		private bool validTemplate;

		private static OptionData s_NoOptionData = new OptionData();

		public RectTransform template
		{
			get
			{
				return m_Template;
			}
			set
			{
				m_Template = value;
				RefreshShownValue();
			}
		}

		public TMP_Text captionText
		{
			get
			{
				return m_CaptionText;
			}
			set
			{
				m_CaptionText = value;
				RefreshShownValue();
			}
		}

		public Image captionImage
		{
			get
			{
				return m_CaptionImage;
			}
			set
			{
				m_CaptionImage = value;
				RefreshShownValue();
			}
		}

		public TMP_Text itemText
		{
			get
			{
				return m_ItemText;
			}
			set
			{
				m_ItemText = value;
				RefreshShownValue();
			}
		}

		public Image itemImage
		{
			get
			{
				return m_ItemImage;
			}
			set
			{
				m_ItemImage = value;
				RefreshShownValue();
			}
		}

		public List<OptionData> options
		{
			get
			{
				return m_Options.options;
			}
			set
			{
				m_Options.options = value;
				RefreshShownValue();
			}
		}

		public DropdownEvent onValueChanged
		{
			get
			{
				return m_OnValueChanged;
			}
			set
			{
				m_OnValueChanged = value;
			}
		}

		public int value
		{
			get
			{
				return m_Value;
			}
			set
			{
				if (Application.isPlaying)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (value == m_Value)
					{
						return;
					}
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (options.Count == 0)
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
				}
				m_Value = Mathf.Clamp(value, 0, options.Count - 1);
				RefreshShownValue();
				m_OnValueChanged.Invoke(m_Value);
			}
		}

		public bool IsExpanded => m_Dropdown != null;

		protected TMP_Dropdown()
		{
		}

		protected override void Awake()
		{
			m_AlphaTweenRunner = new TweenRunner<FloatTween>();
			m_AlphaTweenRunner.Init(this);
			if ((bool)m_CaptionImage)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_CaptionImage.enabled = (m_CaptionImage.sprite != null);
			}
			if (!m_Template)
			{
				return;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				m_Template.gameObject.SetActive(false);
				return;
			}
		}

		public void RefreshShownValue()
		{
			OptionData optionData = s_NoOptionData;
			if (options.Count > 0)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				optionData = options[Mathf.Clamp(m_Value, 0, options.Count - 1)];
			}
			if ((bool)m_CaptionText)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (optionData != null)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (optionData.text != null)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						m_CaptionText.text = optionData.text;
						goto IL_00b5;
					}
				}
				m_CaptionText.text = string.Empty;
			}
			goto IL_00b5;
			IL_00b5:
			if (!m_CaptionImage)
			{
				return;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				if (optionData != null)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					m_CaptionImage.sprite = optionData.image;
				}
				else
				{
					m_CaptionImage.sprite = null;
				}
				m_CaptionImage.enabled = (m_CaptionImage.sprite != null);
				return;
			}
		}

		public void AddOptions(List<OptionData> options)
		{
			this.options.AddRange(options);
			RefreshShownValue();
		}

		public void AddOptions(List<string> options)
		{
			for (int i = 0; i < options.Count; i++)
			{
				this.options.Add(new OptionData(options[i]));
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				RefreshShownValue();
				return;
			}
		}

		public void AddOptions(List<Sprite> options)
		{
			for (int i = 0; i < options.Count; i++)
			{
				this.options.Add(new OptionData(options[i]));
			}
			RefreshShownValue();
		}

		public void ClearOptions()
		{
			options.Clear();
			RefreshShownValue();
		}

		private void SetupTemplate()
		{
			validTemplate = false;
			if (!m_Template)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogError("The dropdown template is not assigned. The template needs to be assigned and must have a child GameObject with a Toggle component serving as the item.", this);
					return;
				}
			}
			GameObject gameObject = m_Template.gameObject;
			gameObject.SetActive(true);
			Toggle componentInChildren = m_Template.GetComponentInChildren<Toggle>();
			validTemplate = true;
			if ((bool)componentInChildren)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!(componentInChildren.transform == template))
				{
					if (!(componentInChildren.transform.parent is RectTransform))
					{
						validTemplate = false;
						Debug.LogError("The dropdown template is not valid. The child GameObject with a Toggle component (the item) must have a RectTransform on its parent.", template);
					}
					else
					{
						if (itemText != null)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!itemText.transform.IsChildOf(componentInChildren.transform))
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								validTemplate = false;
								Debug.LogError("The dropdown template is not valid. The Item Text must be on the item GameObject or children of it.", template);
								goto IL_018e;
							}
						}
						if (itemImage != null && !itemImage.transform.IsChildOf(componentInChildren.transform))
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							validTemplate = false;
							Debug.LogError("The dropdown template is not valid. The Item Image must be on the item GameObject or children of it.", template);
						}
					}
					goto IL_018e;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			validTemplate = false;
			Debug.LogError("The dropdown template is not valid. The template must have a child GameObject with a Toggle component serving as the item.", template);
			goto IL_018e;
			IL_018e:
			if (!validTemplate)
			{
				gameObject.SetActive(false);
				return;
			}
			DropdownItem dropdownItem = componentInChildren.gameObject.AddComponent<DropdownItem>();
			dropdownItem.text = m_ItemText;
			dropdownItem.image = m_ItemImage;
			dropdownItem.toggle = componentInChildren;
			dropdownItem.rectTransform = (RectTransform)componentInChildren.transform;
			Canvas orAddComponent = GetOrAddComponent<Canvas>(gameObject);
			orAddComponent.overrideSorting = true;
			orAddComponent.sortingOrder = 30000;
			GetOrAddComponent<GraphicRaycaster>(gameObject);
			GetOrAddComponent<CanvasGroup>(gameObject);
			gameObject.SetActive(false);
			validTemplate = true;
		}

		private static T GetOrAddComponent<T>(GameObject go) where T : Component
		{
			T val = go.GetComponent<T>();
			if (!(UnityEngine.Object)val)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				val = go.AddComponent<T>();
			}
			return val;
		}

		public virtual void OnPointerClick(PointerEventData eventData)
		{
			Show();
		}

		public virtual void OnSubmit(BaseEventData eventData)
		{
			Show();
		}

		public virtual void OnCancel(BaseEventData eventData)
		{
			Hide();
		}

		public void Show()
		{
			if (!IsActive())
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (!IsInteractable())
				{
					return;
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					if (m_Dropdown != null)
					{
						while (true)
						{
							switch (7)
							{
							default:
								return;
							case 0:
								break;
							}
						}
					}
					if (!validTemplate)
					{
						SetupTemplate();
						if (!validTemplate)
						{
							while (true)
							{
								switch (3)
								{
								default:
									return;
								case 0:
									break;
								}
							}
						}
					}
					List<Canvas> list = TMP_ListPool<Canvas>.Get();
					base.gameObject.GetComponentsInParent(false, list);
					if (list.Count == 0)
					{
						while (true)
						{
							switch (7)
							{
							default:
								return;
							case 0:
								break;
							}
						}
					}
					Canvas canvas = list[0];
					TMP_ListPool<Canvas>.Release(list);
					m_Template.gameObject.SetActive(true);
					m_Dropdown = CreateDropdownList(m_Template.gameObject);
					m_Dropdown.name = "Dropdown List";
					m_Dropdown.SetActive(true);
					RectTransform rectTransform = m_Dropdown.transform as RectTransform;
					rectTransform.SetParent(m_Template.transform.parent, false);
					DropdownItem componentInChildren = m_Dropdown.GetComponentInChildren<DropdownItem>();
					GameObject gameObject = componentInChildren.rectTransform.parent.gameObject;
					RectTransform rectTransform2 = gameObject.transform as RectTransform;
					componentInChildren.rectTransform.gameObject.SetActive(true);
					Rect rect = rectTransform2.rect;
					Rect rect2 = componentInChildren.rectTransform.rect;
					Vector2 vector = rect2.min - rect.min + (Vector2)componentInChildren.rectTransform.localPosition;
					Vector2 vector2 = rect2.max - rect.max + (Vector2)componentInChildren.rectTransform.localPosition;
					Vector2 size = rect2.size;
					m_Items.Clear();
					Toggle toggle = null;
					for (int i = 0; i < options.Count; i++)
					{
						OptionData data = options[i];
						DropdownItem item = AddItem(data, value == i, componentInChildren, m_Items);
						if (item == null)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							continue;
						}
						item.toggle.isOn = (value == i);
						item.toggle.onValueChanged.AddListener(delegate
						{
							OnSelectItem(item.toggle);
						});
						if (item.toggle.isOn)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							item.toggle.Select();
						}
						if (toggle != null)
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							Navigation navigation = toggle.navigation;
							Navigation navigation2 = item.toggle.navigation;
							navigation.mode = Navigation.Mode.Explicit;
							navigation2.mode = Navigation.Mode.Explicit;
							navigation.selectOnDown = item.toggle;
							navigation.selectOnRight = item.toggle;
							navigation2.selectOnLeft = toggle;
							navigation2.selectOnUp = toggle;
							toggle.navigation = navigation;
							item.toggle.navigation = navigation2;
						}
						toggle = item.toggle;
					}
					Vector2 sizeDelta = rectTransform2.sizeDelta;
					sizeDelta.y = size.y * (float)m_Items.Count + vector.y - vector2.y;
					rectTransform2.sizeDelta = sizeDelta;
					float num = rectTransform.rect.height - rectTransform2.rect.height;
					if (num > 0f)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						Vector2 sizeDelta2 = rectTransform.sizeDelta;
						float x2 = sizeDelta2.x;
						Vector2 sizeDelta3 = rectTransform.sizeDelta;
						rectTransform.sizeDelta = new Vector2(x2, sizeDelta3.y - num);
					}
					Vector3[] array = new Vector3[4];
					rectTransform.GetWorldCorners(array);
					RectTransform rectTransform3 = canvas.transform as RectTransform;
					Rect rect3 = rectTransform3.rect;
					for (int j = 0; j < 2; j++)
					{
						bool flag = false;
						int num2 = 0;
						while (true)
						{
							if (num2 < 4)
							{
								Vector3 vector3 = rectTransform3.InverseTransformPoint(array[num2]);
								if (!(vector3[j] < rect3.min[j]))
								{
									while (true)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										break;
									}
									if (!(vector3[j] > rect3.max[j]))
									{
										num2++;
										continue;
									}
									while (true)
									{
										switch (5)
										{
										case 0:
											continue;
										}
										break;
									}
								}
								flag = true;
							}
							else
							{
								while (true)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
							}
							break;
						}
						if (flag)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							RectTransformUtility.FlipLayoutOnAxis(rectTransform, j, false, false);
						}
					}
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						for (int k = 0; k < m_Items.Count; k++)
						{
							RectTransform rectTransform4 = m_Items[k].rectTransform;
							Vector2 anchorMin = rectTransform4.anchorMin;
							rectTransform4.anchorMin = new Vector2(anchorMin.x, 0f);
							Vector2 anchorMax = rectTransform4.anchorMax;
							rectTransform4.anchorMax = new Vector2(anchorMax.x, 0f);
							Vector2 anchoredPosition = rectTransform4.anchoredPosition;
							float x3 = anchoredPosition.x;
							float num3 = vector.y + size.y * (float)(m_Items.Count - 1 - k);
							float y = size.y;
							Vector2 pivot = rectTransform4.pivot;
							rectTransform4.anchoredPosition = new Vector2(x3, num3 + y * pivot.y);
							Vector2 sizeDelta4 = rectTransform4.sizeDelta;
							rectTransform4.sizeDelta = new Vector2(sizeDelta4.x, size.y);
						}
						AlphaFadeList(0.15f, 0f, 1f);
						m_Template.gameObject.SetActive(false);
						componentInChildren.gameObject.SetActive(false);
						m_Blocker = CreateBlocker(canvas);
						return;
					}
				}
			}
		}

		protected virtual GameObject CreateBlocker(Canvas rootCanvas)
		{
			GameObject gameObject = new GameObject("Blocker");
			RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
			rectTransform.SetParent(rootCanvas.transform, false);
			rectTransform.anchorMin = Vector3.zero;
			rectTransform.anchorMax = Vector3.one;
			rectTransform.sizeDelta = Vector2.zero;
			Canvas canvas = gameObject.AddComponent<Canvas>();
			canvas.overrideSorting = true;
			Canvas component = m_Dropdown.GetComponent<Canvas>();
			canvas.sortingLayerID = component.sortingLayerID;
			canvas.sortingOrder = component.sortingOrder - 1;
			gameObject.AddComponent<GraphicRaycaster>();
			Image image = gameObject.AddComponent<Image>();
			image.color = Color.clear;
			Button button = gameObject.AddComponent<Button>();
			button.onClick.AddListener(Hide);
			return gameObject;
		}

		protected virtual void DestroyBlocker(GameObject blocker)
		{
			UnityEngine.Object.Destroy(blocker);
		}

		protected virtual GameObject CreateDropdownList(GameObject template)
		{
			return UnityEngine.Object.Instantiate(template);
		}

		protected virtual void DestroyDropdownList(GameObject dropdownList)
		{
			UnityEngine.Object.Destroy(dropdownList);
		}

		protected virtual DropdownItem CreateItem(DropdownItem itemTemplate)
		{
			return UnityEngine.Object.Instantiate(itemTemplate);
		}

		protected virtual void DestroyItem(DropdownItem item)
		{
		}

		private DropdownItem AddItem(OptionData data, bool selected, DropdownItem itemTemplate, List<DropdownItem> items)
		{
			DropdownItem dropdownItem = CreateItem(itemTemplate);
			dropdownItem.rectTransform.SetParent(itemTemplate.rectTransform.parent, false);
			dropdownItem.gameObject.SetActive(true);
			dropdownItem.gameObject.name = "Item " + items.Count + ((data.text == null) ? string.Empty : (": " + data.text));
			if (dropdownItem.toggle != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				dropdownItem.toggle.isOn = false;
			}
			if ((bool)dropdownItem.text)
			{
				dropdownItem.text.text = data.text;
			}
			if ((bool)dropdownItem.image)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				dropdownItem.image.sprite = data.image;
				dropdownItem.image.enabled = (dropdownItem.image.sprite != null);
			}
			items.Add(dropdownItem);
			return dropdownItem;
		}

		private void AlphaFadeList(float duration, float alpha)
		{
			CanvasGroup component = m_Dropdown.GetComponent<CanvasGroup>();
			AlphaFadeList(duration, component.alpha, alpha);
		}

		private void AlphaFadeList(float duration, float start, float end)
		{
			if (end.Equals(start))
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return;
					}
				}
			}
			FloatTween floatTween = default(FloatTween);
			floatTween.duration = duration;
			floatTween.startValue = start;
			floatTween.targetValue = end;
			FloatTween info = floatTween;
			info.AddOnChangedCallback(SetAlpha);
			info.ignoreTimeScale = true;
			m_AlphaTweenRunner.StartTween(info);
		}

		private void SetAlpha(float alpha)
		{
			if ((bool)m_Dropdown)
			{
				CanvasGroup component = m_Dropdown.GetComponent<CanvasGroup>();
				component.alpha = alpha;
			}
		}

		public void Hide()
		{
			if (m_Dropdown != null)
			{
				AlphaFadeList(0.15f, 0f);
				if (IsActive())
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					StartCoroutine(DelayedDestroyDropdownList(0.15f));
				}
			}
			if (m_Blocker != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				DestroyBlocker(m_Blocker);
			}
			m_Blocker = null;
			Select();
		}

		private IEnumerator DelayedDestroyDropdownList(float delay)
		{
			yield return new WaitForSecondsRealtime(delay);
			/*Error: Unable to find new state assignment for yield return*/;
		}

		private void OnSelectItem(Toggle toggle)
		{
			if (!toggle.isOn)
			{
				toggle.isOn = true;
			}
			int num = -1;
			Transform transform = toggle.transform;
			Transform parent = transform.parent;
			int num2 = 0;
			while (true)
			{
				if (num2 < parent.childCount)
				{
					if (parent.GetChild(num2) == transform)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						num = num2 - 1;
						break;
					}
					num2++;
					continue;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				break;
			}
			if (num < 0)
			{
				while (true)
				{
					switch (1)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			value = num;
			Hide();
		}
	}
}
