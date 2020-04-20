using System;
using System.Collections.Generic;
using CameraManagerInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMinimap : MonoBehaviour
{
	public Image m_minimapImage;

	public RectTransform m_tilePrefab;

	public Image m_pingPrefab;

	public Image m_fogImage;

	public Button m_buttonHitBox;

	public GridLayoutGroup m_gridLayout;

	public UIMinimapPlayerIcon m_playerIconPrefab;

	public UIWorldPing m_worldPingPrefab;

	public UIWorldPing m_worldPingAssistPrefab;

	public UIWorldPing m_worldPingDefendPrefab;

	public UIWorldPing m_worldPingEnemyPrefab;

	public UIWorldPing m_worldPingMovePrefab;

	public RectTransform[,] m_tiles;

	public int m_blurRadius = 5;

	public int m_iterations = 1;

	private bool m_enableMinimap;

	private int m_maxX;

	private int m_maxY;

	private bool m_mouseDown;

	private Vector3 m_lastWorldPositionClicked;

	private float m_lastPingSendTime;

	private List<ControlPoint> m_controlPoints = new List<ControlPoint>();

	private Texture2D fogTexture;

	private Sprite fogSprite;

	private int fogXPixelSize;

	private int fogYPixelSize;

	private List<UIMinimap.MinimapActor> m_minimapActors;

	private void Awake()
	{
		this.m_minimapActors = new List<UIMinimap.MinimapActor>();
		UIEventTriggerUtils.AddListener(this.m_buttonHitBox.gameObject, EventTriggerType.PointerDown, new UIEventTriggerUtils.EventDelegate(this.OnMiniMapPointerDown));
		UIEventTriggerUtils.AddListener(this.m_buttonHitBox.gameObject, EventTriggerType.PointerExit, new UIEventTriggerUtils.EventDelegate(this.OnMiniMapPointerExit));
		UIEventTriggerUtils.AddListener(this.m_buttonHitBox.gameObject, EventTriggerType.PointerUp, new UIEventTriggerUtils.EventDelegate(this.OnMiniMapPointerUp));
		RectTransform rectTransform = this.m_gridLayout.transform as RectTransform;
		this.fogXPixelSize = (int)rectTransform.rect.width;
		this.fogYPixelSize = (int)rectTransform.rect.height;
		this.fogTexture = new Texture2D(this.fogXPixelSize, this.fogYPixelSize, TextureFormat.RGBA32, false);
		this.m_fogImage.transform.localEulerAngles = new Vector3(0f, 0f, -90f);
		CanvasGroup canvasGroup = this.m_gridLayout.gameObject.AddComponent<CanvasGroup>();
		canvasGroup.alpha = 0f;
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
	}

	private void OnDestroy()
	{
		if (this.fogTexture != null)
		{
			UnityEngine.Object.Destroy(this.fogTexture);
		}
	}

	public void UpdateFogOfWar()
	{
		if (!this.m_enableMinimap)
		{
			return;
		}
		if (this.fogTexture == null)
		{
			return;
		}
		Color[] pixels = this.fogTexture.GetPixels(0, 0, this.fogXPixelSize, this.fogYPixelSize);
		float num = (float)this.fogXPixelSize / (float)Board.Get().GetMaxX();
		float num2 = (float)this.fogYPixelSize / (float)Board.Get().GetMaxY();
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		for (int i = 0; i < pixels.Length; i++)
		{
			int x = Mathf.FloorToInt((float)(i % this.fogXPixelSize) / num);
			int y = Mathf.FloorToInt((float)(i / this.fogXPixelSize) / num2);
			GridPos gridPos = default(GridPos);
			gridPos.x = x;
			gridPos.y = y;
			BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(gridPos);
			if (boardSquareSafe != null)
			{
				if (boardSquareSafe.IsBaselineHeight())
				{
					if (activeOwnedActorData.GetFogOfWar().IsVisible(boardSquareSafe))
					{
						pixels[i] = HUD_UIResources.Get().m_fogValidPlaySquareVisible;
					}
					else
					{
						pixels[i] = HUD_UIResources.Get().m_fogValidPlaySquareNonVisible;
					}
				}
				else
				{
					pixels[i] = HUD_UIResources.Get().m_fogInvalidPlaySquare;
				}
			}
			else
			{
				pixels[i] = new Color(0f, 1f, 0f, 0.5f);
			}
		}
		this.fogTexture.SetPixels(0, 0, this.fogXPixelSize, this.fogYPixelSize, pixels);
		this.fogTexture.Apply();
		if (this.fogSprite == null)
		{
			this.fogSprite = Sprite.Create(this.fogTexture, new Rect(0f, 0f, (float)this.fogXPixelSize, (float)this.fogYPixelSize), new Vector2(0.5f, 0.5f));
			this.m_fogImage.sprite = this.fogSprite;
			this.m_fogImage.color = new Color(this.m_fogImage.color.r, this.m_fogImage.color.g, this.m_fogImage.color.b, 0.5f);
		}
	}

	public void OnMiniMapPointerDown(BaseEventData data)
	{
		this.m_mouseDown = true;
	}

	public void OnMiniMapPointerExit(BaseEventData data)
	{
		this.MinimapUnclicked();
	}

	private Vector3 GetMinimapPositionFromWorldPosition(Vector3 worldPosition)
	{
		GridPos gridPos = default(GridPos);
		gridPos.x = 0;
		gridPos.y = 0;
		Vector3 b = Board.Get().GetBoardSquareSafe(gridPos).ToVector3();
		Vector3 vector = worldPosition - b;
		gridPos.x = this.m_maxX - 1;
		gridPos.y = this.m_maxY - 1;
		Vector3 a = Board.Get().GetBoardSquareSafe(gridPos).ToVector3();
		Vector3 vector2 = a - b;
		float num = vector.x / vector2.x;
		float num2 = vector.z / vector2.z;
		Vector3 position = this.m_tiles[0, 0].transform.position;
		Vector3 vector3 = this.m_tiles[this.m_maxX - 1, 0].transform.position - position;
		Vector3 vector4 = this.m_tiles[0, this.m_maxY - 1].transform.position - position;
		return position + vector3.normalized * (num * vector3.magnitude) + vector4.normalized * (num2 * vector4.magnitude);
	}

	public void DisplayMinimapPing(Vector3 worldPosition)
	{
		if (!this.m_enableMinimap)
		{
			return;
		}
		if (base.gameObject.activeSelf)
		{
			Image image = UnityEngine.Object.Instantiate<Image>(this.m_pingPrefab);
			image.transform.SetParent(base.gameObject.transform);
			Vector3 position = Input.mousePosition;
			position = this.GetMinimapPositionFromWorldPosition(worldPosition);
			image.transform.position = position;
			image.transform.localScale = Vector3.one;
			this.m_buttonHitBox.transform.SetAsLastSibling();
		}
	}

	public void SendMiniMapPing(Vector3 worldPosition, ActorController.PingType pingType)
	{
		if (!HUD_UI.Get().m_tauntPlayerBanner.gameObject.activeSelf)
		{
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				if (GameFlowData.Get().activeOwnedActorData.GetActorController() != null)
				{
					if (Time.time - this.m_lastPingSendTime > HUD_UIResources.Get().m_mapPingCooldown)
					{
						this.m_lastPingSendTime = Time.time;
						GameFlowData.Get().activeOwnedActorData.SendPingRequestToServer((int)GameFlowData.Get().activeOwnedActorData.GetTeam(), worldPosition, pingType);
					}
				}
			}
		}
	}

	public void OnMiniMapPointerUp(BaseEventData data)
	{
		if (this.m_mouseDown)
		{
			if (InputManager.Get().IsKeyBindingHeld(KeyPreference.MinimapPing))
			{
				this.SendMiniMapPing(this.m_lastWorldPositionClicked, ActorController.PingType.Default);
			}
		}
		this.MinimapUnclicked();
	}

	private void MinimapUnclicked()
	{
		this.m_mouseDown = false;
	}

	public void AddControlPoint(ControlPoint cp)
	{
		if (!this.m_controlPoints.Contains(cp))
		{
			this.m_controlPoints.Add(cp);
		}
		this.SetupMinimap();
	}

	public void RemoveControlPoint(ControlPoint cp)
	{
		this.m_controlPoints.Remove(cp);
		this.SetupMinimap();
	}

	private ControlPoint GetControlPointAtSquare(int x, int y)
	{
		using (List<ControlPoint>.Enumerator enumerator = this.m_controlPoints.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ControlPoint controlPoint = enumerator.Current;
				if (controlPoint.GetRegion().Contains(x, y))
				{
					return controlPoint;
				}
			}
		}
		return null;
	}

	public void SetupMinimap()
	{
		if (!this.m_enableMinimap)
		{
			return;
		}
		if (Board.Get() == null)
		{
			return;
		}
		this.m_maxX = Board.Get().GetMaxX();
		this.m_maxY = Board.Get().GetMaxY();
		Transform[] componentsInChildren = this.m_gridLayout.GetComponentsInChildren<Transform>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i] != this.m_gridLayout.transform)
			{
				UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
			}
		}
		MapData mapData = GameWideData.Get().GetMapData(GameManager.Get().GameInfo.GameConfig.Map);
		if (mapData != null)
		{
			Sprite sprite = Resources.Load(mapData.MinimapImageLocation, typeof(Sprite)) as Sprite;
			if (sprite != null)
			{
				this.m_minimapImage.sprite = sprite;
			}
			this.m_minimapImage.transform.localScale = new Vector3(mapData.MinimapImageXScale, mapData.MinimapImageYScale, 1f);
		}
		UIManager.SetGameObjectActive(this.m_minimapImage, this.m_minimapImage.sprite != null, null);
		int num = (int)(this.m_gridLayout.transform as RectTransform).rect.width;
		this.m_gridLayout.cellSize = new Vector2((float)(num / this.m_maxX), (float)(num / this.m_maxY));
		this.m_tiles = new RectTransform[this.m_maxX, this.m_maxY];
		for (int j = 0; j < this.m_maxX; j++)
		{
			for (int k = 0; k < this.m_maxY; k++)
			{
				GridPos gridPos = default(GridPos);
				gridPos.x = j;
				gridPos.y = k;
				BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(gridPos);
				RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(this.m_tilePrefab);
				this.m_tiles[j, k] = rectTransform;
				rectTransform.transform.SetParent(this.m_gridLayout.transform, false);
				ControlPoint controlPointAtSquare = this.GetControlPointAtSquare(j, k);
				if (controlPointAtSquare != null)
				{
					rectTransform.gameObject.name = "ControlPoint minimap image";
				}
				else if (boardSquareSafe.IsBaselineHeight())
				{
				}
			}
		}
		this.UpdateFogOfWar();
	}

	private void CheckPlayerActorPositions()
	{
		if (!this.m_enableMinimap)
		{
			return;
		}
		bool flag = false;
		List<ActorData> actors = GameFlowData.Get().GetActors();
		foreach (ActorData actorData in actors)
		{
			bool flag2 = false;
			using (List<UIMinimap.MinimapActor>.Enumerator enumerator2 = this.m_minimapActors.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					UIMinimap.MinimapActor minimapActor = enumerator2.Current;
					if (minimapActor.m_actorData == actorData)
					{
						flag2 = true;
						goto IL_A9;
					}
				}
			}
			IL_A9:
			if (!flag2)
			{
				UIMinimap.MinimapActor item = default(UIMinimap.MinimapActor);
				item.m_actorData = actorData;
				item.m_uiPlayerIcon = UnityEngine.Object.Instantiate<UIMinimapPlayerIcon>(this.m_playerIconPrefab);
				item.m_uiPlayerIcon.transform.SetParent(base.gameObject.transform);
				item.m_uiPlayerIcon.transform.SetAsLastSibling();
				item.m_uiPlayerIcon.transform.localEulerAngles = Vector3.zero;
				item.m_uiPlayerIcon.transform.localScale = Vector3.one;
				item.m_uiPlayerIcon.transform.position = this.m_tiles[actorData.GetGridPosWithIncrementedHeight().x, actorData.GetGridPosWithIncrementedHeight().y].transform.position;
				item.m_uiPlayerIcon.Setup(item.m_actorData);
				this.m_minimapActors.Add(item);
				this.m_buttonHitBox.transform.SetAsLastSibling();
				flag = true;
			}
		}
		int i = 0;
		while (i < this.m_minimapActors.Count)
		{
			if (this.m_minimapActors[i].m_actorData == null)
			{
				goto IL_237;
			}
			if (!actors.Contains(this.m_minimapActors[i].m_actorData))
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					goto IL_237;
				}
			}
			IL_26E:
			i++;
			continue;
			IL_237:
			UnityEngine.Object.Destroy(this.m_minimapActors[i].m_uiPlayerIcon.gameObject);
			this.m_minimapActors.RemoveAt(i);
			flag = true;
			i--;
			goto IL_26E;
		}
		for (int j = 0; j < this.m_minimapActors.Count; j++)
		{
			UIMinimap.MinimapActor minimapActor2 = this.m_minimapActors[j];
			Vector3 minimapPositionFromWorldPosition = this.GetMinimapPositionFromWorldPosition(minimapActor2.m_actorData.GetNameplatePosition(0f));
			if (minimapPositionFromWorldPosition != minimapActor2.m_uiPlayerIcon.transform.position)
			{
				minimapActor2.m_uiPlayerIcon.transform.position = minimapPositionFromWorldPosition;
				flag = true;
			}
		}
		if (flag)
		{
			this.UpdateFogOfWar();
		}
	}

	private void CheckMinimapOrientation()
	{
		GridPos gridPos = default(GridPos);
		gridPos.x = 0;
		gridPos.y = 0;
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(gridPos);
		GridPos gridPos2 = default(GridPos);
		gridPos2.x = 0;
		gridPos2.y = this.m_maxY - 1;
		BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(gridPos2);
		Vector3 vector = boardSquareSafe2.gameObject.transform.position - boardSquareSafe.gameObject.transform.position;
		Vector3 forward = Camera.main.transform.forward;
		forward.y = 0f;
		vector.y = 0f;
		forward.Normalize();
		vector.Normalize();
		float y = Vector3.Cross(forward, vector).y;
		float num = (Vector3.Angle(forward, vector) - 90f) * -1f;
		if (y < 0f)
		{
			num = Vector3.Angle(forward, vector) + 90f;
		}
		base.gameObject.transform.localEulerAngles = new Vector3(0f, 0f, num);
		using (List<UIMinimap.MinimapActor>.Enumerator enumerator = this.m_minimapActors.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIMinimap.MinimapActor minimapActor = enumerator.Current;
				minimapActor.m_uiPlayerIcon.gameObject.transform.localEulerAngles = new Vector3(0f, 0f, -num);
			}
		}
	}

	private void Update()
	{
		if (!this.m_enableMinimap)
		{
			return;
		}
		if (Board.Get() != null)
		{
			this.CheckMinimapOrientation();
			this.CheckPlayerActorPositions();
			if (this.m_mouseDown)
			{
				Vector3 position = this.m_tiles[0, 0].transform.position;
				Vector3 from = base.GetComponentInParent<Canvas>().worldCamera.ScreenToWorldPoint(Input.mousePosition) - position;
				Vector3 to = this.m_tiles[this.m_maxX - 1, 0].transform.position - position;
				Vector3 to2 = this.m_tiles[0, this.m_maxY - 1].transform.position - position;
				float num = from.magnitude * Mathf.Cos(Vector3.Angle(from, to) * 0.0174532924f) / to.magnitude;
				float num2 = from.magnitude * Mathf.Cos(Vector3.Angle(from, to2) * 0.0174532924f) / to2.magnitude;
				num = Mathf.Clamp01(num);
				num2 = Mathf.Clamp01(num2);
				int num3 = Mathf.FloorToInt((float)this.m_maxX * num);
				int num4 = Mathf.FloorToInt((float)this.m_maxY * num2);
				GridPos gridPos = default(GridPos);
				gridPos.x = num3;
				gridPos.y = num4;
				BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(gridPos);
				IsometricCamera component = Camera.main.GetComponent<IsometricCamera>();
				if (boardSquareSafe != null)
				{
					Vector3 vector = boardSquareSafe.ToVector3();
					vector += new Vector3(((float)(this.m_maxX - 1) * num - (float)num3) * Board.Get().squareSize, 0f, ((float)(this.m_maxY - 1) * num2 - (float)num4) * Board.Get().squareSize);
					this.m_lastWorldPositionClicked = vector;
					if (component)
					{
						if (component.enabled && !InputManager.Get().IsKeyBindingHeld(KeyPreference.MinimapPing))
						{
							component.ForceTransformAtDefaultAngle(vector, component.transform.rotation.eulerAngles.y);
						}
					}
				}
			}
		}
	}

	private void OnEnable()
	{
		this.SetupMinimap();
		this.Update();
	}

	public struct MinimapActor
	{
		public ActorData m_actorData;

		public UIMinimapPlayerIcon m_uiPlayerIcon;
	}
}
