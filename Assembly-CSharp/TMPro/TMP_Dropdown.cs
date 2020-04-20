using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TMPro
{
	[AddComponentMenu("UI/TMP Dropdown", 0x23)]
	[RequireComponent(typeof(RectTransform))]
	public class TMP_Dropdown : Selectable, IPointerClickHandler, ISubmitHandler, ICancelHandler, IEventSystemHandler
	{
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
		private TMP_Dropdown.OptionDataList m_Options = new TMP_Dropdown.OptionDataList();

		[Space]
		[SerializeField]
		private TMP_Dropdown.DropdownEvent m_OnValueChanged = new TMP_Dropdown.DropdownEvent();

		private GameObject m_Dropdown;

		private GameObject m_Blocker;

		private List<TMP_Dropdown.DropdownItem> m_Items = new List<TMP_Dropdown.DropdownItem>();

		private TweenRunner<FloatTween> m_AlphaTweenRunner;

		private bool validTemplate;

		private static TMP_Dropdown.OptionData s_NoOptionData = new TMP_Dropdown.OptionData();

		protected TMP_Dropdown()
		{
		}

		public RectTransform template
		{
			get
			{
				return this.m_Template;
			}
			set
			{
				this.m_Template = value;
				this.RefreshShownValue();
			}
		}

		public TMP_Text captionText
		{
			get
			{
				return this.m_CaptionText;
			}
			set
			{
				this.m_CaptionText = value;
				this.RefreshShownValue();
			}
		}

		public Image captionImage
		{
			get
			{
				return this.m_CaptionImage;
			}
			set
			{
				this.m_CaptionImage = value;
				this.RefreshShownValue();
			}
		}

		public TMP_Text itemText
		{
			get
			{
				return this.m_ItemText;
			}
			set
			{
				this.m_ItemText = value;
				this.RefreshShownValue();
			}
		}

		public Image itemImage
		{
			get
			{
				return this.m_ItemImage;
			}
			set
			{
				this.m_ItemImage = value;
				this.RefreshShownValue();
			}
		}

		public List<TMP_Dropdown.OptionData> options
		{
			get
			{
				return this.m_Options.options;
			}
			set
			{
				this.m_Options.options = value;
				this.RefreshShownValue();
			}
		}

		public TMP_Dropdown.DropdownEvent onValueChanged
		{
			get
			{
				return this.m_OnValueChanged;
			}
			set
			{
				this.m_OnValueChanged = value;
			}
		}

		public int value
		{
			get
			{
				return this.m_Value;
			}
			set
			{
				if (Application.isPlaying)
				{
					if (value != this.m_Value)
					{
						if (this.options.Count != 0)
						{
							goto IL_45;
						}
					}
					return;
				}
				IL_45:
				this.m_Value = Mathf.Clamp(value, 0, this.options.Count - 1);
				this.RefreshShownValue();
				this.m_OnValueChanged.Invoke(this.m_Value);
			}
		}

		public bool IsExpanded
		{
			get
			{
				return this.m_Dropdown != null;
			}
		}

		protected override void Awake()
		{
			this.m_AlphaTweenRunner = new TweenRunner<FloatTween>();
			this.m_AlphaTweenRunner.Init(this);
			if (this.m_CaptionImage)
			{
				this.m_CaptionImage.enabled = (this.m_CaptionImage.sprite != null);
			}
			if (this.m_Template)
			{
				this.m_Template.gameObject.SetActive(false);
			}
		}

		public void RefreshShownValue()
		{
			TMP_Dropdown.OptionData optionData = TMP_Dropdown.s_NoOptionData;
			if (this.options.Count > 0)
			{
				optionData = this.options[Mathf.Clamp(this.m_Value, 0, this.options.Count - 1)];
			}
			if (this.m_CaptionText)
			{
				if (optionData != null)
				{
					if (optionData.text != null)
					{
						this.m_CaptionText.text = optionData.text;
						goto IL_B5;
					}
				}
				this.m_CaptionText.text = string.Empty;
			}
			IL_B5:
			if (this.m_CaptionImage)
			{
				if (optionData != null)
				{
					this.m_CaptionImage.sprite = optionData.image;
				}
				else
				{
					this.m_CaptionImage.sprite = null;
				}
				this.m_CaptionImage.enabled = (this.m_CaptionImage.sprite != null);
			}
		}

		public void AddOptions(List<TMP_Dropdown.OptionData> options)
		{
			this.options.AddRange(options);
			this.RefreshShownValue();
		}

		public void AddOptions(List<string> options)
		{
			for (int i = 0; i < options.Count; i++)
			{
				this.options.Add(new TMP_Dropdown.OptionData(options[i]));
			}
			this.RefreshShownValue();
		}

		public void AddOptions(List<Sprite> options)
		{
			for (int i = 0; i < options.Count; i++)
			{
				this.options.Add(new TMP_Dropdown.OptionData(options[i]));
			}
			this.RefreshShownValue();
		}

		public void ClearOptions()
		{
			this.options.Clear();
			this.RefreshShownValue();
		}

		private void SetupTemplate()
		{
			this.validTemplate = false;
			if (!this.m_Template)
			{
				Debug.LogError("The dropdown template is not assigned. The template needs to be assigned and must have a child GameObject with a Toggle component serving as the item.", this);
				return;
			}
			GameObject gameObject = this.m_Template.gameObject;
			gameObject.SetActive(true);
			Toggle componentInChildren = this.m_Template.GetComponentInChildren<Toggle>();
			this.validTemplate = true;
			if (componentInChildren)
			{
				if (componentInChildren.transform == this.template)
				{
				}
				else
				{
					if (!(componentInChildren.transform.parent is RectTransform))
					{
						this.validTemplate = false;
						Debug.LogError("The dropdown template is not valid. The child GameObject with a Toggle component (the item) must have a RectTransform on its parent.", this.template);
						goto IL_18E;
					}
					if (this.itemText != null)
					{
						if (!this.itemText.transform.IsChildOf(componentInChildren.transform))
						{
							this.validTemplate = false;
							Debug.LogError("The dropdown template is not valid. The Item Text must be on the item GameObject or children of it.", this.template);
							goto IL_18E;
						}
					}
					if (this.itemImage != null && !this.itemImage.transform.IsChildOf(componentInChildren.transform))
					{
						this.validTemplate = false;
						Debug.LogError("The dropdown template is not valid. The Item Image must be on the item GameObject or children of it.", this.template);
						goto IL_18E;
					}
					goto IL_18E;
				}
			}
			this.validTemplate = false;
			Debug.LogError("The dropdown template is not valid. The template must have a child GameObject with a Toggle component serving as the item.", this.template);
			IL_18E:
			if (!this.validTemplate)
			{
				gameObject.SetActive(false);
				return;
			}
			TMP_Dropdown.DropdownItem dropdownItem = componentInChildren.gameObject.AddComponent<TMP_Dropdown.DropdownItem>();
			dropdownItem.text = this.m_ItemText;
			dropdownItem.image = this.m_ItemImage;
			dropdownItem.toggle = componentInChildren;
			dropdownItem.rectTransform = (RectTransform)componentInChildren.transform;
			Canvas orAddComponent = TMP_Dropdown.GetOrAddComponent<Canvas>(gameObject);
			orAddComponent.overrideSorting = true;
			orAddComponent.sortingOrder = 0x7530;
			TMP_Dropdown.GetOrAddComponent<GraphicRaycaster>(gameObject);
			TMP_Dropdown.GetOrAddComponent<CanvasGroup>(gameObject);
			gameObject.SetActive(false);
			this.validTemplate = true;
		}

		private static T GetOrAddComponent<T>(GameObject go) where T : Component
		{
			T t = go.GetComponent<T>();
			if (!t)
			{
				t = go.AddComponent<T>();
			}
			return t;
		}

		public virtual void OnPointerClick(PointerEventData eventData)
		{
			this.Show();
		}

		public virtual void OnSubmit(BaseEventData eventData)
		{
			this.Show();
		}

		public virtual void OnCancel(BaseEventData eventData)
		{
			this.Hide();
		}

		public void Show()
		{
			if (this.IsActive())
			{
				if (this.IsInteractable())
				{
					if (this.m_Dropdown != null)
					{
					}
					else
					{
						if (!this.validTemplate)
						{
							this.SetupTemplate();
							if (!this.validTemplate)
							{
								return;
							}
						}
						List<Canvas> list = TMP_ListPool<Canvas>.Get();
						base.gameObject.GetComponentsInParent<Canvas>(false, list);
						if (list.Count == 0)
						{
							return;
						}
						Canvas canvas = list[0];
						TMP_ListPool<Canvas>.Release(list);
						this.m_Template.gameObject.SetActive(true);
						this.m_Dropdown = this.CreateDropdownList(this.m_Template.gameObject);
						this.m_Dropdown.name = "Dropdown List";
						this.m_Dropdown.SetActive(true);
						RectTransform rectTransform = this.m_Dropdown.transform as RectTransform;
						rectTransform.SetParent(this.m_Template.transform.parent, false);
						TMP_Dropdown.DropdownItem componentInChildren = this.m_Dropdown.GetComponentInChildren<TMP_Dropdown.DropdownItem>();
						GameObject gameObject = componentInChildren.rectTransform.parent.gameObject;
						RectTransform rectTransform2 = gameObject.transform as RectTransform;
						componentInChildren.rectTransform.gameObject.SetActive(true);
						Rect rect = rectTransform2.rect;
						Rect rect2 = componentInChildren.rectTransform.rect;
						Vector2 vector = rect2.min - rect.min + componentInChildren.rectTransform.localPosition;
						Vector2 vector2 = rect2.max - rect.max + componentInChildren.rectTransform.localPosition;
						Vector2 size = rect2.size;
						this.m_Items.Clear();
						Toggle toggle = null;
						for (int i = 0; i < this.options.Count; i++)
						{
							TMP_Dropdown.OptionData data = this.options[i];
							TMP_Dropdown.DropdownItem item = this.AddItem(data, this.value == i, componentInChildren, this.m_Items);
							if (item == null)
							{
							}
							else
							{
								item.toggle.isOn = (this.value == i);
								item.toggle.onValueChanged.AddListener(delegate(bool x)
								{
									this.OnSelectItem(item.toggle);
								});
								if (item.toggle.isOn)
								{
									item.toggle.Select();
								}
								if (toggle != null)
								{
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
						}
						Vector2 sizeDelta = rectTransform2.sizeDelta;
						sizeDelta.y = size.y * (float)this.m_Items.Count + vector.y - vector2.y;
						rectTransform2.sizeDelta = sizeDelta;
						float num = rectTransform.rect.height - rectTransform2.rect.height;
						if (num > 0f)
						{
							rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y - num);
						}
						Vector3[] array = new Vector3[4];
						rectTransform.GetWorldCorners(array);
						RectTransform rectTransform3 = canvas.transform as RectTransform;
						Rect rect3 = rectTransform3.rect;
						int j = 0;
						IL_531:
						while (j < 2)
						{
							bool flag = false;
							int k = 0;
							while (k < 4)
							{
								Vector3 vector3 = rectTransform3.InverseTransformPoint(array[k]);
								if (vector3[j] >= rect3.min[j])
								{
									if (vector3[j] <= rect3.max[j])
									{
										k++;
										continue;
									}
								}
								flag = true;
								IL_513:
								if (flag)
								{
									RectTransformUtility.FlipLayoutOnAxis(rectTransform, j, false, false);
								}
								j++;
								goto IL_531;
							}
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								goto IL_513;
							}
						}
						for (int l = 0; l < this.m_Items.Count; l++)
						{
							RectTransform rectTransform4 = this.m_Items[l].rectTransform;
							rectTransform4.anchorMin = new Vector2(rectTransform4.anchorMin.x, 0f);
							rectTransform4.anchorMax = new Vector2(rectTransform4.anchorMax.x, 0f);
							rectTransform4.anchoredPosition = new Vector2(rectTransform4.anchoredPosition.x, vector.y + size.y * (float)(this.m_Items.Count - 1 - l) + size.y * rectTransform4.pivot.y);
							rectTransform4.sizeDelta = new Vector2(rectTransform4.sizeDelta.x, size.y);
						}
						this.AlphaFadeList(0.15f, 0f, 1f);
						this.m_Template.gameObject.SetActive(false);
						componentInChildren.gameObject.SetActive(false);
						this.m_Blocker = this.CreateBlocker(canvas);
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
			Canvas component = this.m_Dropdown.GetComponent<Canvas>();
			canvas.sortingLayerID = component.sortingLayerID;
			canvas.sortingOrder = component.sortingOrder - 1;
			gameObject.AddComponent<GraphicRaycaster>();
			Image image = gameObject.AddComponent<Image>();
			image.color = Color.clear;
			Button button = gameObject.AddComponent<Button>();
			button.onClick.AddListener(new UnityAction(this.Hide));
			return gameObject;
		}

		protected virtual void DestroyBlocker(GameObject blocker)
		{
			UnityEngine.Object.Destroy(blocker);
		}

		protected virtual GameObject CreateDropdownList(GameObject template)
		{
			return UnityEngine.Object.Instantiate<GameObject>(template);
		}

		protected virtual void DestroyDropdownList(GameObject dropdownList)
		{
			UnityEngine.Object.Destroy(dropdownList);
		}

		protected virtual TMP_Dropdown.DropdownItem CreateItem(TMP_Dropdown.DropdownItem itemTemplate)
		{
			return UnityEngine.Object.Instantiate<TMP_Dropdown.DropdownItem>(itemTemplate);
		}

		protected virtual void DestroyItem(TMP_Dropdown.DropdownItem item)
		{
		}

		private TMP_Dropdown.DropdownItem AddItem(TMP_Dropdown.OptionData data, bool selected, TMP_Dropdown.DropdownItem itemTemplate, List<TMP_Dropdown.DropdownItem> items)
		{
			TMP_Dropdown.DropdownItem dropdownItem = this.CreateItem(itemTemplate);
			dropdownItem.rectTransform.SetParent(itemTemplate.rectTransform.parent, false);
			dropdownItem.gameObject.SetActive(true);
			dropdownItem.gameObject.name = "Item " + items.Count + ((data.text == null) ? string.Empty : (": " + data.text));
			if (dropdownItem.toggle != null)
			{
				dropdownItem.toggle.isOn = false;
			}
			if (dropdownItem.text)
			{
				dropdownItem.text.text = data.text;
			}
			if (dropdownItem.image)
			{
				dropdownItem.image.sprite = data.image;
				dropdownItem.image.enabled = (dropdownItem.image.sprite != null);
			}
			items.Add(dropdownItem);
			return dropdownItem;
		}

		private void AlphaFadeList(float duration, float alpha)
		{
			CanvasGroup component = this.m_Dropdown.GetComponent<CanvasGroup>();
			this.AlphaFadeList(duration, component.alpha, alpha);
		}

		private void AlphaFadeList(float duration, float start, float end)
		{
			if (end.Equals(start))
			{
				return;
			}
			FloatTween info = new FloatTween
			{
				duration = duration,
				startValue = start,
				targetValue = end
			};
			info.AddOnChangedCallback(new UnityAction<float>(this.SetAlpha));
			info.ignoreTimeScale = true;
			this.m_AlphaTweenRunner.StartTween(info);
		}

		private void SetAlpha(float alpha)
		{
			if (!this.m_Dropdown)
			{
				return;
			}
			CanvasGroup component = this.m_Dropdown.GetComponent<CanvasGroup>();
			component.alpha = alpha;
		}

		public void Hide()
		{
			if (this.m_Dropdown != null)
			{
				this.AlphaFadeList(0.15f, 0f);
				if (this.IsActive())
				{
					base.StartCoroutine(this.DelayedDestroyDropdownList(0.15f));
				}
			}
			if (this.m_Blocker != null)
			{
				this.DestroyBlocker(this.m_Blocker);
			}
			this.m_Blocker = null;
			this.Select();
		}

		private IEnumerator DelayedDestroyDropdownList(float delay)
		{
			yield return new WaitForSecondsRealtime(delay);
			for (int i = 0; i < this.m_Items.Count; i++)
			{
				if (this.m_Items[i] != null)
				{
					this.DestroyItem(this.m_Items[i]);
				}
				this.m_Items.Clear();
			}
			if (this.m_Dropdown != null)
			{
				this.DestroyDropdownList(this.m_Dropdown);
			}
			this.m_Dropdown = null;
			yield break;
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
			int i = 0;
			while (i < parent.childCount)
			{
				if (parent.GetChild(i) == transform)
				{
					num = i - 1;
					IL_6A:
					if (num < 0)
					{
						return;
					}
					this.value = num;
					this.Hide();
					return;
				}
				else
				{
					i++;
				}
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				goto IL_6A;
			}
		}

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
					return this.m_Text;
				}
				set
				{
					this.m_Text = value;
				}
			}

			public Image image
			{
				get
				{
					return this.m_Image;
				}
				set
				{
					this.m_Image = value;
				}
			}

			public RectTransform rectTransform
			{
				get
				{
					return this.m_RectTransform;
				}
				set
				{
					this.m_RectTransform = value;
				}
			}

			public Toggle toggle
			{
				get
				{
					return this.m_Toggle;
				}
				set
				{
					this.m_Toggle = value;
				}
			}

			public virtual void OnPointerEnter(PointerEventData eventData)
			{
				EventSystem.current.SetSelectedGameObject(base.gameObject);
			}

			public virtual void OnCancel(BaseEventData eventData)
			{
				TMP_Dropdown componentInParent = base.GetComponentInParent<TMP_Dropdown>();
				if (componentInParent)
				{
					componentInParent.Hide();
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

			public string text
			{
				get
				{
					return this.m_Text;
				}
				set
				{
					this.m_Text = value;
				}
			}

			public Sprite image
			{
				get
				{
					return this.m_Image;
				}
				set
				{
					this.m_Image = value;
				}
			}
		}

		[Serializable]
		public class OptionDataList
		{
			[SerializeField]
			private List<TMP_Dropdown.OptionData> m_Options;

			public OptionDataList()
			{
				this.options = new List<TMP_Dropdown.OptionData>();
			}

			public List<TMP_Dropdown.OptionData> options
			{
				get
				{
					return this.m_Options;
				}
				set
				{
					this.m_Options = value;
				}
			}
		}

		[Serializable]
		public class DropdownEvent : UnityEvent<int>
		{
		}
	}
}
