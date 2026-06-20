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
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
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
        UIDebugItem uIDebugItem = Instantiate(m_debugItemPrefab);
        uIDebugItem.Setup(listener, m_scrollRect);
        uIDebugItem.transform.SetParent(m_debugMenuGrid.transform);
        uIDebugItem.transform.localPosition = Vector3.zero;
        uIDebugItem.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private List<string> GetCategoriesToAdd(bool inFrontEnd)
    {
        List<string> list = new List<string>();
        if (DebugCommands.Get() == null)
        {
            return list;
        }

        foreach (DebugCommand cmd in DebugCommands.Get().m_debugCommands)
        {
            if (inFrontEnd && !cmd.AvailableInFrontEnd())
            {
                continue;
            }

            string path = cmd.GetPath();
            if (path == string.Empty)
            {
                continue;
            }

            if (!path.StartsWith(m_currentPath) && m_currentPath != string.Empty)
            {
                continue;
            }

            path = path.Remove(0, m_currentPath.Length);
            string[] parts = path.Split('/');
            if (parts[0] != string.Empty && !list.Contains(parts[0]))
            {
                list.Add(parts[0]);
            }
        }

        return list;
    }

    private bool DebugItemAtCurrentLevel(string path)
    {
        return path == m_currentPath.TrimEnd('/');
    }

    private void ClearDebugItems()
    {
        List<Transform> elements = new List<Transform>();
        for (int i = 0; i < m_debugMenuGrid.transform.childCount; i++)
        {
            elements.Add(m_debugMenuGrid.transform.GetChild(i));
        }

        foreach (Transform elem in elements)
        {
            DestroyImmediate(elem.gameObject);
        }
    }

    private void CreateDebugItems()
    {
        ClearDebugItems();
        if (m_currentPath != string.Empty)
        {
            CreateDebugItem(new DebugCommand_Back { m_debugMenu = this });
        }

        m_inFrontEnd = GameFlowData.Get() == null;
        if (DebugCommands.Get() != null)
        {
            foreach (DebugCommand cmd in DebugCommands.Get().m_debugCommands)
            {
                if ((!m_inFrontEnd || cmd.AvailableInFrontEnd())
                    && DebugItemAtCurrentLevel(cmd.GetPath())
                    && cmd.GetPath().Length > 0)
                {
                    CreateDebugItem(cmd);
                }
            }
        }

        List<string> categoriesToAdd = GetCategoriesToAdd(m_inFrontEnd);
        foreach (string category in categoriesToAdd)
        {
            CreateDebugItem(
                new DebugCommand_Category
                {
                    m_category = category,
                    m_debugMenu = this
                });
        }

        float width = m_debugMenuGrid.GetComponent<RectTransform>().rect.width;
        m_debugMenuGrid.cellSize = new Vector2(width, m_debugMenuGrid.cellSize.y);
        float cellSizeY = m_debugMenuGrid.cellSize.y + m_debugMenuGrid.spacing.y;
        float cellSizeX = m_debugMenuGrid.cellSize.x + m_debugMenuGrid.spacing.x;
        float rowNum = Mathf.Ceil(
            m_debugMenuGrid.transform.childCount
            / Mathf.Floor(m_debugMenuGrid.GetComponent<RectTransform>().rect.width / cellSizeX));
        RectTransform component = m_debugMenuGrid.GetComponent<RectTransform>();
        Vector2 sizeDelta = m_debugMenuGrid.GetComponent<RectTransform>().sizeDelta;
        component.sizeDelta = new Vector2(sizeDelta.x, cellSizeY * rowNum);
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
        if (UISounds.GetUISounds() == null
            || UIUtils.InputFieldHasFocus()
            || !AccountPreferences.DoesApplicationHaveFocus()
            || DebugCommands.Get() == null)
        {
            return;
        }

        bool isInFrontend = GameFlowData.Get() == null;
        foreach (DebugCommand cmd in DebugCommands.Get().m_debugCommands)
        {
            if (isInFrontend && !cmd.AvailableInFrontEnd())
            {
                continue;
            }

            if (cmd.GetKeyCode() != 0 && Input.GetKeyDown(cmd.GetKeyCode()) && HasRequiredModifierKeys(cmd))
            {
                UISounds.GetUISounds().Play("ui_btn_menu_click");
                cmd.OnIncreaseClick();
            }

            if (cmd.CheckGameControllerTrigger())
            {
                UISounds.GetUISounds().Play("ui_btn_menu_click");
                cmd.OnIncreaseClick();
            }
        }
    }

    private bool HasRequiredModifierKeys(DebugCommand listener)
    {
        if (listener == null)
        {
            return false;
        }

        return (!listener.RequireCtrlKey() || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
               && (!listener.RequireAltKey() || Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
               && (!listener.RequireShiftKey() || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
    }

    public void UpPathLevel()
    {
        int index = m_currentPath.TrimEnd('/').LastIndexOf('/');
        m_currentPath = index > 0
            ? m_currentPath.Substring(0, index + 1)
            : string.Empty;
        CreateDebugItems();
        ResetScroll();
    }

    public void ResetScroll()
    {
        (m_debugMenuGrid.transform as RectTransform).anchoredPosition = Vector2.zero;
    }
}