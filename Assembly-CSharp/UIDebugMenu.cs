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
		return s_instance;
	}

	public void Awake()
	{
		s_instance = this;
		m_scrollRect = GetComponentInChildren<ScrollRect>(true);
		if (HydrogenConfig.Get().DevMode)
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
					Init();
					return;
				}
			}
		}
		Object.Destroy(base.gameObject);
	}

	public void Init()
	{
		CreateDebugItems();
	}

	public void ResetIfNeeded()
	{
		if (m_inFrontEnd != (GameFlowData.Get() == null))
		{
			CreateDebugItems();
		}
	}

	private void Update()
	{
		UpdateHotKeys();
	}

	private void CreateDebugItem(DebugCommand listener)
	{
		UIDebugItem uIDebugItem = Object.Instantiate(m_debugItemPrefab);
		uIDebugItem.Setup(listener, m_scrollRect);
		uIDebugItem.transform.SetParent(m_debugMenuGrid.transform);
		uIDebugItem.transform.localPosition = Vector3.zero;
		uIDebugItem.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	private List<string> GetCategoriesToAdd(bool inFrontEnd)
	{
		List<string> list = new List<string>();
		if (DebugCommands.Get() != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					using (List<DebugCommand>.Enumerator enumerator = DebugCommands.Get().m_debugCommands.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							DebugCommand current = enumerator.Current;
							if (inFrontEnd && !current.AvailableInFrontEnd())
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
							else
							{
								string path = current.GetPath();
								if (path != string.Empty)
								{
									if (!path.StartsWith(m_currentPath))
									{
										if (!(m_currentPath == string.Empty))
										{
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
									path = path.Remove(0, m_currentPath.Length);
									string[] array = path.Split('/');
									if (array[0] != string.Empty)
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
										if (!list.Contains(array[0]))
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
											list.Add(array[0]);
										}
									}
								}
							}
						}
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								return list;
							}
						}
					}
				}
				}
			}
		}
		return list;
	}

	private bool _001D(string _001D)
	{
		bool result = false;
		if (_001D == m_currentPath.TrimEnd('/'))
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
			result = true;
		}
		return result;
	}

	private void ClearDebugItems()
	{
		List<Transform> list = new List<Transform>();
		for (int i = 0; i < m_debugMenuGrid.transform.childCount; i++)
		{
			list.Add(m_debugMenuGrid.transform.GetChild(i));
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
			using (List<Transform>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Transform current = enumerator.Current;
					Object.DestroyImmediate(current.gameObject);
				}
				while (true)
				{
					switch (5)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
	}

	private void CreateDebugItems()
	{
		ClearDebugItems();
		if (m_currentPath != string.Empty)
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
			DebugCommand_Back debugCommand_Back = new DebugCommand_Back();
			debugCommand_Back.m_debugMenu = this;
			CreateDebugItem(debugCommand_Back);
		}
		m_inFrontEnd = (GameFlowData.Get() == null);
		if (DebugCommands.Get() != null)
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
			using (List<DebugCommand>.Enumerator enumerator = DebugCommands.Get().m_debugCommands.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DebugCommand current = enumerator.Current;
					if (m_inFrontEnd)
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
						if (!current.AvailableInFrontEnd())
						{
							continue;
						}
					}
					if (_001D(current.GetPath()))
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
						if (current.GetPath().Length > 0)
						{
							CreateDebugItem(current);
						}
					}
				}
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
		}
		List<string> categoriesToAdd = GetCategoriesToAdd(m_inFrontEnd);
		using (List<string>.Enumerator enumerator2 = categoriesToAdd.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				string current2 = enumerator2.Current;
				DebugCommand_Category debugCommand_Category = new DebugCommand_Category();
				debugCommand_Category.m_category = current2;
				debugCommand_Category.m_debugMenu = this;
				CreateDebugItem(debugCommand_Category);
			}
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
		GridLayoutGroup debugMenuGrid = m_debugMenuGrid;
		float width = m_debugMenuGrid.GetComponent<RectTransform>().rect.width;
		Vector2 cellSize = m_debugMenuGrid.cellSize;
		debugMenuGrid.cellSize = new Vector2(width, cellSize.y);
		Vector2 cellSize2 = m_debugMenuGrid.cellSize;
		float y = cellSize2.y;
		Vector2 spacing = m_debugMenuGrid.spacing;
		float num = y + spacing.y;
		Vector2 cellSize3 = m_debugMenuGrid.cellSize;
		float x = cellSize3.x;
		Vector2 spacing2 = m_debugMenuGrid.spacing;
		float num2 = x + spacing2.x;
		float num3 = Mathf.Ceil((float)m_debugMenuGrid.transform.childCount / Mathf.Floor(m_debugMenuGrid.GetComponent<RectTransform>().rect.width / num2));
		RectTransform component = m_debugMenuGrid.GetComponent<RectTransform>();
		Vector2 sizeDelta = m_debugMenuGrid.GetComponent<RectTransform>().sizeDelta;
		component.sizeDelta = new Vector2(sizeDelta.x, num * num3);
	}

	public void OnEnable()
	{
		ResetIfNeeded();
	}

	public void AddToPath(string category)
	{
		m_currentPath = m_currentPath + category + "/";
		CreateDebugItems();
		ResetScroll();
	}

	public void UpdateHotKeys()
	{
		if (!(UISounds.GetUISounds() != null))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!UIUtils.InputFieldHasFocus() && AccountPreferences.DoesApplicationHaveFocus() && DebugCommands.Get() != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					bool flag = GameFlowData.Get() == null;
					using (List<DebugCommand>.Enumerator enumerator = DebugCommands.Get().m_debugCommands.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							DebugCommand current = enumerator.Current;
							if (flag)
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
								if (!current.AvailableInFrontEnd())
								{
									continue;
								}
							}
							if (current._001D() != 0)
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
								if (Input.GetKeyDown(current._001D()))
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
									if (HasRequiredModifierKeys(current))
									{
										UISounds.GetUISounds().Play("ui_btn_menu_click");
										current.OnIncreaseClick();
									}
								}
							}
							if (current.CheckGameControllerTrigger())
							{
								UISounds.GetUISounds().Play("ui_btn_menu_click");
								current.OnIncreaseClick();
							}
						}
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
				}
			}
			return;
		}
	}

	private bool HasRequiredModifierKeys(DebugCommand listener)
	{
		if (listener != null)
		{
			while (true)
			{
				bool flag;
				int num2;
				int result;
				bool flag2;
				int num3;
				bool flag3;
				switch (6)
				{
				case 0:
					break;
				default:
					{
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						int num;
						if (listener._000E() && !Input.GetKey(KeyCode.LeftControl))
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
							num = (Input.GetKey(KeyCode.RightControl) ? 1 : 0);
						}
						else
						{
							num = 1;
						}
						flag = ((byte)num != 0);
						if (listener._0012())
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
							if (!Input.GetKey(KeyCode.LeftAlt))
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
								num2 = (Input.GetKey(KeyCode.RightAlt) ? 1 : 0);
								goto IL_0086;
							}
						}
						num2 = 1;
						goto IL_0086;
					}
					IL_00df:
					return (byte)result != 0;
					IL_0086:
					flag2 = ((byte)num2 != 0);
					if (listener._0015())
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
						if (!Input.GetKey(KeyCode.LeftShift))
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
							num3 = (Input.GetKey(KeyCode.RightShift) ? 1 : 0);
							goto IL_00c0;
						}
					}
					num3 = 1;
					goto IL_00c0;
					IL_00c0:
					flag3 = ((byte)num3 != 0);
					if (flag)
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
						if (flag2)
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
							result = (flag3 ? 1 : 0);
							goto IL_00df;
						}
					}
					result = 0;
					goto IL_00df;
				}
			}
		}
		return false;
	}

	public void UpPathLevel()
	{
		int num = m_currentPath.TrimEnd('/').LastIndexOf('/');
		if (num > 0)
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
			m_currentPath = m_currentPath.Substring(0, num + 1);
		}
		else
		{
			m_currentPath = string.Empty;
		}
		CreateDebugItems();
		ResetScroll();
	}

	public void ResetScroll()
	{
		(m_debugMenuGrid.transform as RectTransform).anchoredPosition = Vector2.zero;
	}
}
