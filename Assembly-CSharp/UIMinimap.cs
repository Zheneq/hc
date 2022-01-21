using CameraManagerInternal;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMinimap : MonoBehaviour
{
	public struct MinimapActor
	{
		public ActorData m_actorData;

		public UIMinimapPlayerIcon m_uiPlayerIcon;
	}

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

	private List<MinimapActor> m_minimapActors;

	private void Awake()
	{
		m_minimapActors = new List<MinimapActor>();
		UIEventTriggerUtils.AddListener(m_buttonHitBox.gameObject, EventTriggerType.PointerDown, OnMiniMapPointerDown);
		UIEventTriggerUtils.AddListener(m_buttonHitBox.gameObject, EventTriggerType.PointerExit, OnMiniMapPointerExit);
		UIEventTriggerUtils.AddListener(m_buttonHitBox.gameObject, EventTriggerType.PointerUp, OnMiniMapPointerUp);
		RectTransform rectTransform = m_gridLayout.transform as RectTransform;
		fogXPixelSize = (int)rectTransform.rect.width;
		fogYPixelSize = (int)rectTransform.rect.height;
		fogTexture = new Texture2D(fogXPixelSize, fogYPixelSize, TextureFormat.RGBA32, false);
		m_fogImage.transform.localEulerAngles = new Vector3(0f, 0f, -90f);
		CanvasGroup canvasGroup = m_gridLayout.gameObject.AddComponent<CanvasGroup>();
		canvasGroup.alpha = 0f;
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
	}

	private void OnDestroy()
	{
		if (!(fogTexture != null))
		{
			return;
		}
		while (true)
		{
			UnityEngine.Object.Destroy(fogTexture);
			return;
		}
	}

	public void UpdateFogOfWar()
	{
		if (!m_enableMinimap)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (fogTexture == null)
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
		Color[] pixels = fogTexture.GetPixels(0, 0, fogXPixelSize, fogYPixelSize);
		float num = (float)fogXPixelSize / (float)Board.Get().GetMaxX();
		float num2 = (float)fogYPixelSize / (float)Board.Get().GetMaxY();
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		for (int i = 0; i < pixels.Length; i++)
		{
			int x = Mathf.FloorToInt((float)(i % fogXPixelSize) / num);
			int y = Mathf.FloorToInt((float)(i / fogXPixelSize) / num2);
			GridPos gridPos = default(GridPos);
			gridPos.x = x;
			gridPos.y = y;
			BoardSquare boardSquareSafe = Board.Get().GetSquare(gridPos);
			if (boardSquareSafe != null)
			{
				if (boardSquareSafe.IsValidForGameplay())
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
		fogTexture.SetPixels(0, 0, fogXPixelSize, fogYPixelSize, pixels);
		fogTexture.Apply();
		if (!(fogSprite == null))
		{
			return;
		}
		while (true)
		{
			fogSprite = Sprite.Create(fogTexture, new Rect(0f, 0f, fogXPixelSize, fogYPixelSize), new Vector2(0.5f, 0.5f));
			m_fogImage.sprite = fogSprite;
			Image fogImage = m_fogImage;
			Color color = m_fogImage.color;
			float r = color.r;
			Color color2 = m_fogImage.color;
			float g = color2.g;
			Color color3 = m_fogImage.color;
			fogImage.color = new Color(r, g, color3.b, 0.5f);
			return;
		}
	}

	public void OnMiniMapPointerDown(BaseEventData data)
	{
		m_mouseDown = true;
	}

	public void OnMiniMapPointerExit(BaseEventData data)
	{
		MinimapUnclicked();
	}

	private Vector3 GetMinimapPositionFromWorldPosition(Vector3 worldPosition)
	{
		GridPos gridPos = default(GridPos);
		gridPos.x = 0;
		gridPos.y = 0;
		Vector3 b = Board.Get().GetSquare(gridPos).ToVector3();
		Vector3 vector = worldPosition - b;
		gridPos.x = m_maxX - 1;
		gridPos.y = m_maxY - 1;
		Vector3 a = Board.Get().GetSquare(gridPos).ToVector3();
		Vector3 vector2 = a - b;
		float num = vector.x / vector2.x;
		float num2 = vector.z / vector2.z;
		Vector3 position = m_tiles[0, 0].transform.position;
		Vector3 vector3 = m_tiles[m_maxX - 1, 0].transform.position - position;
		Vector3 vector4 = m_tiles[0, m_maxY - 1].transform.position - position;
		return position + vector3.normalized * (num * vector3.magnitude) + vector4.normalized * (num2 * vector4.magnitude);
	}

	public void DisplayMinimapPing(Vector3 worldPosition)
	{
		if (!m_enableMinimap || !base.gameObject.activeSelf)
		{
			return;
		}
		while (true)
		{
			Image image = UnityEngine.Object.Instantiate(m_pingPrefab);
			image.transform.SetParent(base.gameObject.transform);
			Vector3 mousePosition = Input.mousePosition;
			mousePosition = GetMinimapPositionFromWorldPosition(worldPosition);
			image.transform.position = mousePosition;
			image.transform.localScale = Vector3.one;
			m_buttonHitBox.transform.SetAsLastSibling();
			return;
		}
	}

	public void SendMiniMapPing(Vector3 worldPosition, ActorController.PingType pingType)
	{
		if (HUD_UI.Get().m_tauntPlayerBanner.gameObject.activeSelf)
		{
			return;
		}
		while (true)
		{
			if (!(GameFlowData.Get().activeOwnedActorData != null))
			{
				return;
			}
			while (true)
			{
				if (!(GameFlowData.Get().activeOwnedActorData.GetActorController() != null))
				{
					return;
				}
				while (true)
				{
					if (Time.time - m_lastPingSendTime > HUD_UIResources.Get().m_mapPingCooldown)
					{
						while (true)
						{
							m_lastPingSendTime = Time.time;
							GameFlowData.Get().activeOwnedActorData.SendPingRequestToServer((int)GameFlowData.Get().activeOwnedActorData.GetTeam(), worldPosition, pingType);
							return;
						}
					}
					return;
				}
			}
		}
	}

	public void OnMiniMapPointerUp(BaseEventData data)
	{
		if (m_mouseDown)
		{
			if (InputManager.Get().IsKeyBindingHeld(KeyPreference.MinimapPing))
			{
				SendMiniMapPing(m_lastWorldPositionClicked, ActorController.PingType.Default);
			}
		}
		MinimapUnclicked();
	}

	private void MinimapUnclicked()
	{
		m_mouseDown = false;
	}

	public void AddControlPoint(ControlPoint cp)
	{
		if (!m_controlPoints.Contains(cp))
		{
			m_controlPoints.Add(cp);
		}
		SetupMinimap();
	}

	public void RemoveControlPoint(ControlPoint cp)
	{
		m_controlPoints.Remove(cp);
		SetupMinimap();
	}

	private ControlPoint GetControlPointAtSquare(int x, int y)
	{
		using (List<ControlPoint>.Enumerator enumerator = m_controlPoints.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ControlPoint current = enumerator.Current;
				if (current.GetRegion().Contains(x, y))
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return current;
						}
					}
				}
			}
		}
		return null;
	}

	public void SetupMinimap()
	{
		if (!m_enableMinimap)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (Board.Get() == null)
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
		m_maxX = Board.Get().GetMaxX();
		m_maxY = Board.Get().GetMaxY();
		Transform[] componentsInChildren = m_gridLayout.GetComponentsInChildren<Transform>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i] != m_gridLayout.transform)
			{
				UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
			}
		}
		while (true)
		{
			MapData mapData = GameWideData.Get().GetMapData(GameManager.Get().GameInfo.GameConfig.Map);
			if (mapData != null)
			{
				Sprite sprite = Resources.Load(mapData.MinimapImageLocation, typeof(Sprite)) as Sprite;
				if (sprite != null)
				{
					m_minimapImage.sprite = sprite;
				}
				m_minimapImage.transform.localScale = new Vector3(mapData.MinimapImageXScale, mapData.MinimapImageYScale, 1f);
			}
			UIManager.SetGameObjectActive(m_minimapImage, m_minimapImage.sprite != null);
			int num = (int)(m_gridLayout.transform as RectTransform).rect.width;
			m_gridLayout.cellSize = new Vector2(num / m_maxX, num / m_maxY);
			m_tiles = new RectTransform[m_maxX, m_maxY];
			for (int j = 0; j < m_maxX; j++)
			{
				for (int k = 0; k < m_maxY; k++)
				{
					GridPos gridPos = default(GridPos);
					gridPos.x = j;
					gridPos.y = k;
					BoardSquare boardSquareSafe = Board.Get().GetSquare(gridPos);
					RectTransform rectTransform = UnityEngine.Object.Instantiate(m_tilePrefab);
					m_tiles[j, k] = rectTransform;
					rectTransform.transform.SetParent(m_gridLayout.transform, false);
					ControlPoint controlPointAtSquare = GetControlPointAtSquare(j, k);
					if (controlPointAtSquare != null)
					{
						rectTransform.gameObject.name = "ControlPoint minimap image";
					}
					else if (boardSquareSafe.IsValidForGameplay())
					{
					}
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						goto end_IL_0293;
					}
					continue;
					end_IL_0293:
					break;
				}
			}
			while (true)
			{
				UpdateFogOfWar();
				return;
			}
		}
	}

	private void CheckPlayerActorPositions()
	{
		if (!m_enableMinimap)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		bool flag = false;
		List<ActorData> actors = GameFlowData.Get().GetActors();
		foreach (ActorData item2 in actors)
		{
			bool flag2 = false;
			using (List<MinimapActor>.Enumerator enumerator2 = m_minimapActors.GetEnumerator())
			{
				while (true)
				{
					if (!enumerator2.MoveNext())
					{
						break;
					}
					MinimapActor current2 = enumerator2.Current;
					if (current2.m_actorData == item2)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								flag2 = true;
								goto end_IL_0057;
							}
						}
					}
				}
				end_IL_0057:;
			}
			if (!flag2)
			{
				MinimapActor item = default(MinimapActor);
				item.m_actorData = item2;
				item.m_uiPlayerIcon = UnityEngine.Object.Instantiate(m_playerIconPrefab);
				item.m_uiPlayerIcon.transform.SetParent(base.gameObject.transform);
				item.m_uiPlayerIcon.transform.SetAsLastSibling();
				item.m_uiPlayerIcon.transform.localEulerAngles = Vector3.zero;
				item.m_uiPlayerIcon.transform.localScale = Vector3.one;
				item.m_uiPlayerIcon.transform.position = m_tiles[item2.GetGridPosWithIncrementedHeight().x, item2.GetGridPosWithIncrementedHeight().y].transform.position;
				item.m_uiPlayerIcon.Setup(item.m_actorData);
				m_minimapActors.Add(item);
				m_buttonHitBox.transform.SetAsLastSibling();
				flag = true;
			}
		}
		for (int i = 0; i < m_minimapActors.Count; i++)
		{
			MinimapActor minimapActor = m_minimapActors[i];
			if (!(minimapActor.m_actorData == null))
			{
				MinimapActor minimapActor2 = m_minimapActors[i];
				if (actors.Contains(minimapActor2.m_actorData))
				{
					continue;
				}
			}
			MinimapActor minimapActor3 = m_minimapActors[i];
			UnityEngine.Object.Destroy(minimapActor3.m_uiPlayerIcon.gameObject);
			m_minimapActors.RemoveAt(i);
			flag = true;
			i--;
		}
		while (true)
		{
			for (int j = 0; j < m_minimapActors.Count; j++)
			{
				MinimapActor minimapActor4 = m_minimapActors[j];
				Vector3 minimapPositionFromWorldPosition = GetMinimapPositionFromWorldPosition(minimapActor4.m_actorData.GetNameplatePosition(0f));
				if (minimapPositionFromWorldPosition != minimapActor4.m_uiPlayerIcon.transform.position)
				{
					minimapActor4.m_uiPlayerIcon.transform.position = minimapPositionFromWorldPosition;
					flag = true;
				}
			}
			while (true)
			{
				if (flag)
				{
					while (true)
					{
						UpdateFogOfWar();
						return;
					}
				}
				return;
			}
		}
	}

	private void CheckMinimapOrientation()
	{
		GridPos gridPos = default(GridPos);
		gridPos.x = 0;
		gridPos.y = 0;
		BoardSquare boardSquareSafe = Board.Get().GetSquare(gridPos);
		GridPos gridPos2 = default(GridPos);
		gridPos2.x = 0;
		gridPos2.y = m_maxY - 1;
		BoardSquare boardSquareSafe2 = Board.Get().GetSquare(gridPos2);
		Vector3 vector = boardSquareSafe2.gameObject.transform.position - boardSquareSafe.gameObject.transform.position;
		Vector3 forward = Camera.main.transform.forward;
		forward.y = 0f;
		vector.y = 0f;
		forward.Normalize();
		vector.Normalize();
		Vector3 vector2 = Vector3.Cross(forward, vector);
		float y = vector2.y;
		float num = (Vector3.Angle(forward, vector) - 90f) * -1f;
		if (y < 0f)
		{
			num = Vector3.Angle(forward, vector) + 90f;
		}
		base.gameObject.transform.localEulerAngles = new Vector3(0f, 0f, num);
		using (List<MinimapActor>.Enumerator enumerator = m_minimapActors.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				MinimapActor current = enumerator.Current;
				current.m_uiPlayerIcon.gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f - num);
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
	}

	private void Update()
	{
		if (!m_enableMinimap || !(Board.Get() != null))
		{
			return;
		}
		while (true)
		{
			CheckMinimapOrientation();
			CheckPlayerActorPositions();
			if (!m_mouseDown)
			{
				return;
			}
			while (true)
			{
				Vector3 position = m_tiles[0, 0].transform.position;
				Vector3 from = GetComponentInParent<Canvas>().worldCamera.ScreenToWorldPoint(Input.mousePosition) - position;
				Vector3 to = m_tiles[m_maxX - 1, 0].transform.position - position;
				Vector3 to2 = m_tiles[0, m_maxY - 1].transform.position - position;
				float value = from.magnitude * Mathf.Cos(Vector3.Angle(from, to) * ((float)Math.PI / 180f)) / to.magnitude;
				float value2 = from.magnitude * Mathf.Cos(Vector3.Angle(from, to2) * ((float)Math.PI / 180f)) / to2.magnitude;
				value = Mathf.Clamp01(value);
				value2 = Mathf.Clamp01(value2);
				int num = Mathf.FloorToInt((float)m_maxX * value);
				int num2 = Mathf.FloorToInt((float)m_maxY * value2);
				GridPos gridPos = default(GridPos);
				gridPos.x = num;
				gridPos.y = num2;
				BoardSquare boardSquareSafe = Board.Get().GetSquare(gridPos);
				IsometricCamera component = Camera.main.GetComponent<IsometricCamera>();
				if (!(boardSquareSafe != null))
				{
					return;
				}
				Vector3 a = boardSquareSafe.ToVector3();
				a = (m_lastWorldPositionClicked = a + new Vector3(((float)(m_maxX - 1) * value - (float)num) * Board.Get().squareSize, 0f, ((float)(m_maxY - 1) * value2 - (float)num2) * Board.Get().squareSize));
				if (!component)
				{
					return;
				}
				while (true)
				{
					if (component.enabled && !InputManager.Get().IsKeyBindingHeld(KeyPreference.MinimapPing))
					{
						Vector3 targetPos = a;
						Vector3 eulerAngles = component.transform.rotation.eulerAngles;
						component.ForceTransformAtDefaultAngle(targetPos, eulerAngles.y);
					}
					return;
				}
			}
		}
	}

	private void OnEnable()
	{
		SetupMinimap();
		Update();
	}
}
