using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDebugMenu : MonoBehaviour
{
	public GridLayoutGroup m_debugMenuGrid;

	public UIDebugItem m_debugItemPrefab;

	public RectTransform m_container;

	private string m_currentPath = string.Empty;

	private bool m_inFrontEnd;

	private ScrollRect m_scrollRect;

	private static UIDebugMenu s_instance;

	public static UIDebugMenu Get()
	{
		return UIDebugMenu.s_instance;
	}

	public void Awake()
	{
		UIDebugMenu.s_instance = this;
		this.m_scrollRect = base.GetComponentInChildren<ScrollRect>(true);
		if (HydrogenConfig.Get().DevMode)
		{
			this.Init();
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void Init()
	{
		this.CreateDebugItems();
	}

	public void ResetIfNeeded()
	{
		if (this.m_inFrontEnd != (GameFlowData.Get() == null))
		{
			this.CreateDebugItems();
		}
	}

	private void Update()
	{
		this.UpdateHotKeys();
	}

	private void CreateDebugItem(DebugCommand listener)
	{
		UIDebugItem uidebugItem = UnityEngine.Object.Instantiate<UIDebugItem>(this.m_debugItemPrefab);
		uidebugItem.Setup(listener, this.m_scrollRect);
		uidebugItem.transform.SetParent(this.m_debugMenuGrid.transform);
		uidebugItem.transform.localPosition = Vector3.zero;
		uidebugItem.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	private List<string> GetCategoriesToAdd(bool inFrontEnd)
	{
		List<string> list = new List<string>();
		if (DebugCommands.Get() != null)
		{
			using (List<DebugCommand>.Enumerator enumerator = DebugCommands.Get().m_debugCommands.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DebugCommand debugCommand = enumerator.Current;
					if (inFrontEnd && !debugCommand.AvailableInFrontEnd())
					{
					}
					else
					{
						string text = debugCommand.GetPath();
						if (text != string.Empty)
						{
							if (!text.StartsWith(this.m_currentPath))
							{
								if (!(this.m_currentPath == string.Empty))
								{
									continue;
								}
							}
							text = text.Remove(0, this.m_currentPath.Length);
							string[] array = text.Split(new char[]
							{
								'/'
							});
							if (array[0] != string.Empty)
							{
								if (!list.Contains(array[0]))
								{
									list.Add(array[0]);
								}
							}
						}
					}
				}
			}
		}
		return list;
	}

	private bool symbol_001D(string symbol_001D)
	{
		bool result = false;
		if (symbol_001D == this.m_currentPath.TrimEnd(new char[]
		{
			'/'
		}))
		{
			result = true;
		}
		return result;
	}

	private void ClearDebugItems()
	{
		List<Transform> list = new List<Transform>();
		for (int i = 0; i < this.m_debugMenuGrid.transform.childCount; i++)
		{
			list.Add(this.m_debugMenuGrid.transform.GetChild(i));
		}
		using (List<Transform>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Transform transform = enumerator.Current;
				UnityEngine.Object.DestroyImmediate(transform.gameObject);
			}
		}
	}

	private void CreateDebugItems()
	{
		this.ClearDebugItems();
		if (this.m_currentPath != string.Empty)
		{
			this.CreateDebugItem(new DebugCommand_Back
			{
				m_debugMenu = this
			});
		}
		this.m_inFrontEnd = (GameFlowData.Get() == null);
		if (DebugCommands.Get() != null)
		{
			using (List<DebugCommand>.Enumerator enumerator = DebugCommands.Get().m_debugCommands.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DebugCommand debugCommand = enumerator.Current;
					if (this.m_inFrontEnd)
					{
						if (!debugCommand.AvailableInFrontEnd())
						{
							continue;
						}
					}
					if (this.symbol_001D(debugCommand.GetPath()))
					{
						if (debugCommand.GetPath().Length > 0)
						{
							this.CreateDebugItem(debugCommand);
						}
					}
				}
			}
		}
		List<string> categoriesToAdd = this.GetCategoriesToAdd(this.m_inFrontEnd);
		using (List<string>.Enumerator enumerator2 = categoriesToAdd.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				string category = enumerator2.Current;
				this.CreateDebugItem(new DebugCommand_Category
				{
					m_category = category,
					m_debugMenu = this
				});
			}
		}
		this.m_debugMenuGrid.cellSize = new Vector2(this.m_debugMenuGrid.GetComponent<RectTransform>().rect.width, this.m_debugMenuGrid.cellSize.y);
		float num = this.m_debugMenuGrid.cellSize.y + this.m_debugMenuGrid.spacing.y;
		float num2 = this.m_debugMenuGrid.cellSize.x + this.m_debugMenuGrid.spacing.x;
		float num3 = Mathf.Ceil((float)this.m_debugMenuGrid.transform.childCount / Mathf.Floor(this.m_debugMenuGrid.GetComponent<RectTransform>().rect.width / num2));
		this.m_debugMenuGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(this.m_debugMenuGrid.GetComponent<RectTransform>().sizeDelta.x, num * num3);
	}

	public void OnEnable()
	{
		this.ResetIfNeeded();
	}

	public void AddToPath(string category)
	{
		this.m_currentPath = this.m_currentPath + category + "/";
		this.CreateDebugItems();
		this.ResetScroll();
	}

	public void UpdateHotKeys()
	{
		if (UISounds.GetUISounds() != null)
		{
			if (!UIUtils.InputFieldHasFocus() && AccountPreferences.DoesApplicationHaveFocus() && DebugCommands.Get() != null)
			{
				bool flag = GameFlowData.Get() == null;
				using (List<DebugCommand>.Enumerator enumerator = DebugCommands.Get().m_debugCommands.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DebugCommand debugCommand = enumerator.Current;
						if (flag)
						{
							if (!debugCommand.AvailableInFrontEnd())
							{
								continue;
							}
						}
						if (debugCommand.symbol_001D() != KeyCode.None)
						{
							if (Input.GetKeyDown(debugCommand.symbol_001D()))
							{
								if (this.HasRequiredModifierKeys(debugCommand))
								{
									UISounds.GetUISounds().Play("ui_btn_menu_click");
									debugCommand.OnIncreaseClick();
								}
							}
						}
						if (debugCommand.CheckGameControllerTrigger())
						{
							UISounds.GetUISounds().Play("ui_btn_menu_click");
							debugCommand.OnIncreaseClick();
						}
					}
				}
			}
		}
	}

	private bool HasRequiredModifierKeys(DebugCommand listener)
	{
		if (listener != null)
		{
			bool flag;
			if (listener.symbol_000E() && !Input.GetKey(KeyCode.LeftControl))
			{
				flag = Input.GetKey(KeyCode.RightControl);
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			bool flag3;
			if (listener.symbol_0012())
			{
				if (!Input.GetKey(KeyCode.LeftAlt))
				{
					flag3 = Input.GetKey(KeyCode.RightAlt);
					goto IL_86;
				}
			}
			flag3 = true;
			IL_86:
			bool flag4 = flag3;
			bool flag5;
			if (listener.symbol_0015())
			{
				if (!Input.GetKey(KeyCode.LeftShift))
				{
					flag5 = Input.GetKey(KeyCode.RightShift);
					goto IL_C0;
				}
			}
			flag5 = true;
			IL_C0:
			bool result = flag5;
			if (flag2)
			{
				if (flag4)
				{
					return result;
				}
			}
			return false;
		}
		return false;
	}

	public void UpPathLevel()
	{
		int num = this.m_currentPath.TrimEnd(new char[]
		{
			'/'
		}).LastIndexOf('/');
		if (num > 0)
		{
			this.m_currentPath = this.m_currentPath.Substring(0, num + 1);
		}
		else
		{
			this.m_currentPath = string.Empty;
		}
		this.CreateDebugItems();
		this.ResetScroll();
	}

	public void ResetScroll()
	{
		(this.m_debugMenuGrid.transform as RectTransform).anchoredPosition = Vector2.zero;
	}
}
