using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITooltipManager : UIScene
{
	public UITooltipBase[] m_tooltipPrefabs;

	private static UITooltipManager s_instance;

	private UITooltipBase[] m_tooltips;

	private UITooltipBase m_currentDisplayTooltip;

	private UITooltipHoverObject m_currentHoveredObject;

	private UITooltipBase m_currentMenuTooltip;

	private UITooltipClickObject m_currentClickedObject;

	private StandaloneInputModuleWithEventDataAccess m_inputModule;

	private Vector2[] m_defaultAnchorPoints = new Vector2[]
	{
		new Vector2(0f, 0f),
		new Vector2(0f, 1f),
		new Vector2(1f, 0f),
		new Vector2(1f, 1f)
	};

	public static UITooltipManager Get()
	{
		return UITooltipManager.s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.Tooltips;
	}

	public override void Awake()
	{
		UITooltipManager.s_instance = this;
		base.Awake();
	}

	private void OnDestroy()
	{
		if (UITooltipManager.s_instance == this)
		{
			UITooltipManager.s_instance = null;
		}
	}

	private void Start()
	{
		this.m_tooltips = new UITooltipBase[0x14];
		this.m_inputModule = EventSystem.current.gameObject.GetComponent<StandaloneInputModuleWithEventDataAccess>();
	}

	private void LateUpdate()
	{
		if (this.m_currentDisplayTooltip != null)
		{
			if (this.m_currentDisplayTooltip.isActiveAndEnabled)
			{
				GameObject pointerEnter = this.m_inputModule.GetLastPointerEventDataPublic(-1).pointerEnter;
				if (!(this.m_currentHoveredObject == null) && !(pointerEnter == null))
				{
					if (this.IsEqualOrParent(this.m_currentHoveredObject.transform, pointerEnter.transform))
					{
						goto IL_A4;
					}
				}
				this.HideDisplayTooltip();
				this.m_currentHoveredObject = null;
				IL_A4:;
			}
			else
			{
				this.m_currentDisplayTooltip = null;
				this.m_currentHoveredObject = null;
			}
		}
		if (this.m_currentMenuTooltip != null)
		{
			if (!this.m_currentMenuTooltip.isActiveAndEnabled)
			{
				this.m_currentMenuTooltip = null;
				this.m_currentClickedObject = null;
			}
			else if (this.m_currentClickedObject == null || !this.m_currentClickedObject.isActiveAndEnabled)
			{
				this.HideMenu();
			}
			else
			{
				if (!Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
				{
					if (!Input.GetMouseButtonDown(2))
					{
						return;
					}
				}
				GameObject gameObject = this.m_inputModule.GetLastPointerEventDataPublic(-1).pointerCurrentRaycast.gameObject;
				if (gameObject != null)
				{
					if (!this.IsEqualOrParent(this.m_currentMenuTooltip.transform, gameObject.transform))
					{
						this.HideMenu();
					}
				}
			}
		}
	}

	private bool IsEqualOrParent(Transform parent, Transform child)
	{
		while (!(parent == child))
		{
			child = child.parent;
			if (!(child != null))
			{
				return false;
			}
		}
		return true;
	}

	public bool IsVisible(TooltipType type)
	{
		UITooltipBase uitooltipBase = this.m_tooltips[(int)type];
		if (uitooltipBase == null)
		{
			return false;
		}
		if (!(this.m_currentDisplayTooltip == uitooltipBase))
		{
			if (!(this.m_currentMenuTooltip == uitooltipBase))
			{
				return false;
			}
		}
		return true;
	}

	public void HideDisplayTooltip()
	{
		if (this.m_currentDisplayTooltip != null)
		{
			this.m_currentHoveredObject.CallDisableTooltip();
			this.m_currentDisplayTooltip.SetVisible(false);
		}
		this.m_currentDisplayTooltip = null;
	}

	public void HideDisplayTooltip(TooltipType type)
	{
		if (this.m_tooltips[(int)type] != null)
		{
			if (this.m_tooltips[(int)type] == this.m_currentDisplayTooltip)
			{
				this.HideDisplayTooltip();
			}
		}
	}

	public void ShowDisplayTooltip(UITooltipHoverObject obj)
	{
		this.HideDisplayTooltip();
		this.m_currentDisplayTooltip = this.GetTooltip(obj.GetTooltipType());
		this.m_currentHoveredObject = obj;
		if (!this.m_currentHoveredObject.PopulateTooltip(this.m_currentDisplayTooltip))
		{
			this.HideDisplayTooltip();
			return;
		}
		this.m_currentDisplayTooltip.SetVisible(true);
		this.PositionTooltip(this.m_currentHoveredObject, this.m_currentDisplayTooltip);
	}

	public void HideMenu()
	{
		this.m_currentMenuTooltip.SetVisible(false);
		this.m_currentMenuTooltip = null;
		this.m_currentClickedObject = null;
	}

	public void ShowMenu(UITooltipClickObject obj)
	{
		if (this.m_currentMenuTooltip != null)
		{
			this.m_currentMenuTooltip.SetVisible(false);
		}
		this.m_currentMenuTooltip = this.GetTooltip(obj.GetTooltipType());
		this.m_currentClickedObject = obj;
		if (!this.m_currentClickedObject.PopulateTooltip(this.m_currentMenuTooltip))
		{
			this.HideMenu();
			return;
		}
		this.m_currentMenuTooltip.SetVisible(true);
		this.PositionTooltip(this.m_currentClickedObject, this.m_currentMenuTooltip);
	}

	private UITooltipBase GetTooltip(TooltipType type)
	{
		if (this.m_tooltips[(int)type] == null)
		{
			UITooltipBase uitooltipBase = UnityEngine.Object.Instantiate<UITooltipBase>(this.m_tooltipPrefabs[(int)type]);
			uitooltipBase.transform.SetParent(base.transform);
			uitooltipBase.transform.localPosition = Vector3.zero;
			uitooltipBase.transform.localScale = Vector3.one;
			this.m_tooltips[(int)type] = uitooltipBase;
		}
		return this.m_tooltips[(int)type];
	}

	public void UpdateTooltip(UITooltipObject obj)
	{
		if (obj is UITooltipHoverObject)
		{
			if (this.m_currentHoveredObject != obj)
			{
				return;
			}
			if (this.m_currentDisplayTooltip == null)
			{
				this.ShowDisplayTooltip(obj as UITooltipHoverObject);
				return;
			}
			if (!this.m_currentHoveredObject.PopulateTooltip(this.m_currentDisplayTooltip))
			{
				this.HideDisplayTooltip();
			}
		}
		else if (obj is UITooltipClickObject)
		{
			if (this.m_currentMenuTooltip == null || this.m_currentClickedObject != obj)
			{
				return;
			}
			if (!this.m_currentClickedObject.PopulateTooltip(this.m_currentMenuTooltip))
			{
				this.HideMenu();
			}
		}
	}

	private void PositionTooltip(UITooltipObject obj, UITooltipBase tooltip)
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(tooltip.transform as RectTransform);
		UITooltipManager.PositioningData positioningData = new UITooltipManager.PositioningData(obj.transform);
		UITooltipManager.PositioningData positioningData2 = new UITooltipManager.PositioningData(tooltip.transform);
		if (positioningData.Canvas.renderMode != RenderMode.WorldSpace)
		{
			float x;
			float num;
			float x2;
			float num2;
			Vector2 closestVector;
			if (obj is UITooltipHoverObject)
			{
				UITooltipHoverObject uitooltipHoverObject = obj as UITooltipHoverObject;
				positioningData.GetCornerBoundsInViewport(out x, out num, out x2, out num2);
				Vector2[] array;
				if (uitooltipHoverObject.m_anchorPoints != null)
				{
					if (uitooltipHoverObject.m_anchorPoints.Length > 0)
					{
						array = uitooltipHoverObject.m_anchorPoints;
						goto IL_B5;
					}
				}
				array = this.m_defaultAnchorPoints;
				IL_B5:
				Vector2[] array2 = array;
				array2 = positioningData.GetViewportAnchors(array2, x, num, x2, num2);
				closestVector = this.GetClosestVector(obj.m_gravityWell, array2);
			}
			else
			{
				Vector3 mousePosition = Input.mousePosition;
				closestVector = new Vector2(mousePosition.x / (float)Screen.width, mousePosition.y / (float)Screen.height);
				x = closestVector.x;
				x2 = closestVector.x;
				num2 = (num = 0f);
			}
			Vector2 vector = obj.m_gravityWell - closestVector;
			vector.Normalize();
			Vector2[] array3;
			if (tooltip.m_anchorPoints.Length > 0)
			{
				array3 = tooltip.m_anchorPoints;
			}
			else
			{
				array3 = this.m_defaultAnchorPoints;
			}
			Vector2[] candidates = array3;
			bool flag;
			if (obj.m_gravityWell.x >= x)
			{
				if (obj.m_gravityWell.x <= x + num)
				{
					if (obj.m_gravityWell.y >= x2)
					{
						flag = (obj.m_gravityWell.y <= x2 + num2);
						goto IL_1C5;
					}
				}
			}
			flag = false;
			IL_1C5:
			bool flag2 = flag;
			if (flag2)
			{
				vector *= -1f;
			}
			vector.x = vector.x / -2f + 0.5f;
			vector.y = vector.y / -2f + 0.5f;
			Vector2 closestVector2 = this.GetClosestVector(vector, candidates);
			positioningData2.Transform.anchorMin = closestVector2;
			positioningData2.Transform.anchorMax = closestVector2;
			positioningData2.Transform.pivot = closestVector2;
			positioningData2.UpdatePosFromViewportPoint(closestVector);
			positioningData2.GetCornerBoundsInViewport(out x, out num, out x2, out num2);
			bool flag3 = false;
			if (x < 0f)
			{
				closestVector.x -= x;
				flag3 = true;
			}
			else if (x + num > 1f)
			{
				closestVector.x -= x + num - 1f;
				flag3 = true;
			}
			if (x2 < 0f)
			{
				closestVector.y -= x2;
				flag3 = true;
			}
			else if (x2 + num2 > 1f)
			{
				closestVector.y -= x2 + num2 - 1f;
				flag3 = true;
			}
			if (flag3)
			{
				positioningData2.UpdatePosFromViewportPoint(closestVector);
			}
			return;
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
				num = num2;
				vector = candidates[i];
			}
		}
		return vector;
	}

	private class PositioningData
	{
		public PositioningData(Transform transform)
		{
			this.Transform = (transform as RectTransform);
			this.Canvas = transform.GetComponentInParent<Canvas>();
		}

		public RectTransform Transform { get; private set; }

		public Canvas Canvas { get; private set; }

		public void GetCornerBoundsInViewport(out float left, out float width, out float bottom, out float height)
		{
			Vector3[] array = new Vector3[4];
			this.Transform.GetWorldCorners(array);
			Vector3 vector = this.Canvas.worldCamera.WorldToViewportPoint(array[0]);
			Vector3 vector2 = this.Canvas.worldCamera.WorldToViewportPoint(array[2]);
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
			return array;
		}

		public void UpdatePosFromViewportPoint(Vector2 newViewportPos)
		{
			Vector3 position = this.Canvas.worldCamera.ViewportToWorldPoint(newViewportPos);
			position.z = this.Transform.position.z;
			this.Transform.position = position;
		}
	}
}
