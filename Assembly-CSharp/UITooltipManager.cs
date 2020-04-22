using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITooltipManager : UIScene
{
	private class PositioningData
	{
		public RectTransform Transform
		{
			get;
			private set;
		}

		public Canvas Canvas
		{
			get;
			private set;
		}

		public PositioningData(Transform transform)
		{
			Transform = (transform as RectTransform);
			Canvas = transform.GetComponentInParent<Canvas>();
		}

		public void GetCornerBoundsInViewport(out float left, out float width, out float bottom, out float height)
		{
			Vector3[] array = new Vector3[4];
			Transform.GetWorldCorners(array);
			Vector3 vector = Canvas.worldCamera.WorldToViewportPoint(array[0]);
			Vector3 vector2 = Canvas.worldCamera.WorldToViewportPoint(array[2]);
			left = vector.x;
			width = vector2.x - left;
			bottom = vector.y;
			height = vector2.y - bottom;
		}

		public Vector2[] GetViewportAnchors(Vector2[] anchors, float left, float width, float bottom, float height)
		{
			Vector2[] array = new Vector2[anchors.Length];
			for (int i = 0; i < anchors.Length; i++)
			{
				array[i].x = left + anchors[i].x * width;
				array[i].y = bottom + anchors[i].y * height;
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
				return array;
			}
		}

		public void UpdatePosFromViewportPoint(Vector2 newViewportPos)
		{
			Vector3 position = Canvas.worldCamera.ViewportToWorldPoint(newViewportPos);
			Vector3 position2 = Transform.position;
			position.z = position2.z;
			Transform.position = position;
		}
	}

	public UITooltipBase[] m_tooltipPrefabs;

	private static UITooltipManager s_instance;

	private UITooltipBase[] m_tooltips;

	private UITooltipBase m_currentDisplayTooltip;

	private UITooltipHoverObject m_currentHoveredObject;

	private UITooltipBase m_currentMenuTooltip;

	private UITooltipClickObject m_currentClickedObject;

	private StandaloneInputModuleWithEventDataAccess m_inputModule;

	private Vector2[] m_defaultAnchorPoints = new Vector2[4]
	{
		new Vector2(0f, 0f),
		new Vector2(0f, 1f),
		new Vector2(1f, 0f),
		new Vector2(1f, 1f)
	};

	public static UITooltipManager Get()
	{
		return s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.Tooltips;
	}

	public override void Awake()
	{
		s_instance = this;
		base.Awake();
	}

	private void OnDestroy()
	{
		if (!(s_instance == this))
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
			s_instance = null;
			return;
		}
	}

	private void Start()
	{
		m_tooltips = new UITooltipBase[20];
		m_inputModule = EventSystem.current.gameObject.GetComponent<StandaloneInputModuleWithEventDataAccess>();
	}

	private void LateUpdate()
	{
		if (m_currentDisplayTooltip != null)
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
			if (m_currentDisplayTooltip.isActiveAndEnabled)
			{
				GameObject pointerEnter = m_inputModule.GetLastPointerEventDataPublic(-1).pointerEnter;
				if (!(m_currentHoveredObject == null) && !(pointerEnter == null))
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
					if (IsEqualOrParent(m_currentHoveredObject.transform, pointerEnter.transform))
					{
						goto IL_00b4;
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
				HideDisplayTooltip();
				m_currentHoveredObject = null;
			}
			else
			{
				m_currentDisplayTooltip = null;
				m_currentHoveredObject = null;
			}
		}
		goto IL_00b4;
		IL_00b4:
		if (!(m_currentMenuTooltip != null))
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
			if (!m_currentMenuTooltip.isActiveAndEnabled)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						m_currentMenuTooltip = null;
						m_currentClickedObject = null;
						return;
					}
				}
			}
			if (m_currentClickedObject == null || !m_currentClickedObject.isActiveAndEnabled)
			{
				HideMenu();
				return;
			}
			if (!Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
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
				if (!Input.GetMouseButtonDown(2))
				{
					return;
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
			GameObject gameObject = m_inputModule.GetLastPointerEventDataPublic(-1).pointerCurrentRaycast.gameObject;
			if (!(gameObject != null))
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
				if (!IsEqualOrParent(m_currentMenuTooltip.transform, gameObject.transform))
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						HideMenu();
						return;
					}
				}
				return;
			}
		}
	}

	private bool IsEqualOrParent(Transform parent, Transform child)
	{
		do
		{
			if (parent == child)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return true;
					}
				}
			}
			child = child.parent;
		}
		while (child != null);
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			return false;
		}
	}

	public bool IsVisible(TooltipType type)
	{
		UITooltipBase uITooltipBase = m_tooltips[(int)type];
		if (uITooltipBase == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return false;
				}
			}
		}
		if (!(m_currentDisplayTooltip == uITooltipBase))
		{
			if (!(m_currentMenuTooltip == uITooltipBase))
			{
				return false;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return true;
	}

	public void HideDisplayTooltip()
	{
		if (m_currentDisplayTooltip != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_currentHoveredObject.CallDisableTooltip();
			m_currentDisplayTooltip.SetVisible(false);
		}
		m_currentDisplayTooltip = null;
	}

	public void HideDisplayTooltip(TooltipType type)
	{
		if (!(m_tooltips[(int)type] != null))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_tooltips[(int)type] == m_currentDisplayTooltip)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					HideDisplayTooltip();
					return;
				}
			}
			return;
		}
	}

	public void ShowDisplayTooltip(UITooltipHoverObject obj)
	{
		HideDisplayTooltip();
		m_currentDisplayTooltip = GetTooltip(obj.GetTooltipType());
		m_currentHoveredObject = obj;
		if (!m_currentHoveredObject.PopulateTooltip(m_currentDisplayTooltip))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					HideDisplayTooltip();
					return;
				}
			}
		}
		m_currentDisplayTooltip.SetVisible(true);
		PositionTooltip(m_currentHoveredObject, m_currentDisplayTooltip);
	}

	public void HideMenu()
	{
		m_currentMenuTooltip.SetVisible(false);
		m_currentMenuTooltip = null;
		m_currentClickedObject = null;
	}

	public void ShowMenu(UITooltipClickObject obj)
	{
		if (m_currentMenuTooltip != null)
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
			m_currentMenuTooltip.SetVisible(false);
		}
		m_currentMenuTooltip = GetTooltip(obj.GetTooltipType());
		m_currentClickedObject = obj;
		if (!m_currentClickedObject.PopulateTooltip(m_currentMenuTooltip))
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					HideMenu();
					return;
				}
			}
		}
		m_currentMenuTooltip.SetVisible(true);
		PositionTooltip(m_currentClickedObject, m_currentMenuTooltip);
	}

	private UITooltipBase GetTooltip(TooltipType type)
	{
		if (m_tooltips[(int)type] == null)
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
			UITooltipBase uITooltipBase = UnityEngine.Object.Instantiate(m_tooltipPrefabs[(int)type]);
			uITooltipBase.transform.SetParent(base.transform);
			uITooltipBase.transform.localPosition = Vector3.zero;
			uITooltipBase.transform.localScale = Vector3.one;
			m_tooltips[(int)type] = uITooltipBase;
		}
		return m_tooltips[(int)type];
	}

	public void UpdateTooltip(UITooltipObject obj)
	{
		if (obj is UITooltipHoverObject)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (m_currentHoveredObject != obj)
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
					if (m_currentDisplayTooltip == null)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								ShowDisplayTooltip(obj as UITooltipHoverObject);
								return;
							}
						}
					}
					if (!m_currentHoveredObject.PopulateTooltip(m_currentDisplayTooltip))
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								HideDisplayTooltip();
								return;
							}
						}
					}
					return;
				}
			}
		}
		if (!(obj is UITooltipClickObject))
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
			if (!(m_currentMenuTooltip == null) && !(m_currentClickedObject != obj) && !m_currentClickedObject.PopulateTooltip(m_currentMenuTooltip))
			{
				HideMenu();
			}
			return;
		}
	}

	private void PositionTooltip(UITooltipObject obj, UITooltipBase tooltip)
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(tooltip.transform as RectTransform);
		PositioningData positioningData = new PositioningData(obj.transform);
		PositioningData positioningData2 = new PositioningData(tooltip.transform);
		if (positioningData.Canvas.renderMode != RenderMode.WorldSpace)
		{
			while (true)
			{
				float left;
				float width;
				float bottom;
				float height;
				Vector2[] array;
				Vector2 vector;
				Vector2[] anchors;
				int num;
				Vector2 comparee;
				Vector2 closestVector;
				Vector2[] candidates;
				bool flag;
				Vector2[] array2;
				switch (4)
				{
				case 0:
					break;
				default:
					{
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						if (obj is UITooltipHoverObject)
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
							UITooltipHoverObject uITooltipHoverObject = obj as UITooltipHoverObject;
							positioningData.GetCornerBoundsInViewport(out left, out width, out bottom, out height);
							if (uITooltipHoverObject.m_anchorPoints != null)
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
								if (uITooltipHoverObject.m_anchorPoints.Length > 0)
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
									array = uITooltipHoverObject.m_anchorPoints;
									goto IL_00b5;
								}
							}
							array = m_defaultAnchorPoints;
							goto IL_00b5;
						}
						Vector3 mousePosition = Input.mousePosition;
						vector = new Vector2(mousePosition.x / (float)Screen.width, mousePosition.y / (float)Screen.height);
						left = vector.x;
						bottom = vector.x;
						width = (height = 0f);
						goto IL_0125;
					}
					IL_00b5:
					anchors = array;
					anchors = positioningData.GetViewportAnchors(anchors, left, width, bottom, height);
					vector = GetClosestVector(obj.m_gravityWell, anchors);
					goto IL_0125;
					IL_01c5:
					if (num != 0)
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
						comparee *= -1f;
					}
					comparee.x = comparee.x / -2f + 0.5f;
					comparee.y = comparee.y / -2f + 0.5f;
					closestVector = GetClosestVector(comparee, candidates);
					positioningData2.Transform.anchorMin = closestVector;
					positioningData2.Transform.anchorMax = closestVector;
					positioningData2.Transform.pivot = closestVector;
					positioningData2.UpdatePosFromViewportPoint(vector);
					positioningData2.GetCornerBoundsInViewport(out left, out width, out bottom, out height);
					flag = false;
					if (left < 0f)
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
						vector.x -= left;
						flag = true;
					}
					else if (left + width > 1f)
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
						vector.x -= left + width - 1f;
						flag = true;
					}
					if (bottom < 0f)
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
						vector.y -= bottom;
						flag = true;
					}
					else if (bottom + height > 1f)
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
						vector.y -= bottom + height - 1f;
						flag = true;
					}
					if (flag)
					{
						positioningData2.UpdatePosFromViewportPoint(vector);
					}
					return;
					IL_0125:
					comparee = obj.m_gravityWell - vector;
					comparee.Normalize();
					if (tooltip.m_anchorPoints.Length > 0)
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
						array2 = tooltip.m_anchorPoints;
					}
					else
					{
						array2 = m_defaultAnchorPoints;
					}
					candidates = array2;
					if (obj.m_gravityWell.x >= left)
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
						if (obj.m_gravityWell.x <= left + width)
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
							if (obj.m_gravityWell.y >= bottom)
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
								num = ((obj.m_gravityWell.y <= bottom + height) ? 1 : 0);
								goto IL_01c5;
							}
						}
					}
					num = 0;
					goto IL_01c5;
				}
			}
		}
		throw new NotImplementedException();
	}

	private Vector2 GetClosestVector(Vector2 comparee, Vector2[] candidates)
	{
		Vector2 vector = candidates[0];
		float num = Vector2.Distance(comparee, vector);
		for (int i = 1; i < candidates.Length; i++)
		{
			float num2 = Vector2.Distance(comparee, candidates[i]);
			if (num2 < num)
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
				num = num2;
				vector = candidates[i];
			}
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			return vector;
		}
	}
}
