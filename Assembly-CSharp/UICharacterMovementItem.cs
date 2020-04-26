using System.Collections.Generic;
using UnityEngine;

public class UICharacterMovementItem : MonoBehaviour
{
	public UICharacterMovementMarker[] m_redMarkers;

	public UICharacterMovementMarker[] m_blueMarkers;

	public float m_heightOffset;

	private Canvas myCanvas;

	public RectTransform CanvasRect;

	private BoardSquare boardSquareRef;

	private List<ActorData> actorDataRef = new List<ActorData>();

	public List<ActorData> Actors => actorDataRef;

	public BoardSquare BoardPosition => boardSquareRef;

	public void Setup(BoardSquare square, ActorData data)
	{
		boardSquareRef = square;
		AddActor(data);
	}

	public void AddActor(ActorData data)
	{
		if (actorDataRef.Contains(data))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		actorDataRef.Add(data);
		UpdateIndicators();
	}

	public bool RemoveActor(ActorData data)
	{
		if (actorDataRef.Contains(data))
		{
			actorDataRef.Remove(data);
			UpdateIndicators();
		}
		return actorDataRef.Count == 0;
	}

	private void UpdateIndicators()
	{
		int num = 0;
		int num2 = 0;
		if (GameFlowData.Get() != null)
		{
			for (int i = 0; i < actorDataRef.Count; i++)
			{
				if (actorDataRef[i] == null)
				{
					continue;
				}
				int num3;
				if (ClientGameManager.Get() != null && ClientGameManager.Get().PlayerInfo != null)
				{
					if (ClientGameManager.Get().PlayerInfo.TeamId == Team.Spectator)
					{
						num3 = 1;
						goto IL_00db;
					}
				}
				if (GameManager.Get() != null && GameManager.Get().PlayerInfo != null)
				{
					num3 = ((GameManager.Get().PlayerInfo.TeamId == Team.Spectator) ? 1 : 0);
				}
				else
				{
					num3 = 0;
				}
				goto IL_00db;
				IL_00db:
				int num4;
				if (num3 != 0)
				{
					num4 = ((actorDataRef[i].GetTeam() == Team.TeamA) ? 1 : 0);
				}
				else if (GameFlowData.Get().activeOwnedActorData != null)
				{
					num4 = ((actorDataRef[i].GetTeam() == GameFlowData.Get().activeOwnedActorData.GetTeam()) ? 1 : 0);
				}
				else
				{
					num4 = 0;
				}
				if (num4 != 0)
				{
					UICharacterMovementMarker uICharacterMovementMarker = m_blueMarkers[num];
					if (uICharacterMovementMarker != null)
					{
						if (uICharacterMovementMarker.gameObject != null)
						{
							UIManager.SetGameObjectActive(uICharacterMovementMarker, true);
							uICharacterMovementMarker.m_characterImage.sprite = actorDataRef[i].GetScreenIndicatorIcon();
							num++;
						}
					}
					continue;
				}
				UICharacterMovementMarker uICharacterMovementMarker2 = m_redMarkers[num2];
				if (uICharacterMovementMarker2 != null && uICharacterMovementMarker2.gameObject != null)
				{
					UIManager.SetGameObjectActive(uICharacterMovementMarker2, true);
					uICharacterMovementMarker2.m_characterImage.sprite = actorDataRef[i].GetScreenIndicatorIcon();
					num2++;
				}
			}
		}
		for (int j = num; j < m_blueMarkers.Length; j++)
		{
			if (m_blueMarkers[j] != null)
			{
				UIManager.SetGameObjectActive(m_blueMarkers[j], false);
			}
		}
		while (true)
		{
			for (int k = num2; k < m_redMarkers.Length; k++)
			{
				if (m_redMarkers[k] != null)
				{
					UIManager.SetGameObjectActive(m_redMarkers[k], false);
				}
			}
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

	private void LateUpdate()
	{
		if (Camera.main == null)
		{
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
		if (myCanvas == null)
		{
			myCanvas = HUD_UI.Get().GetTopLevelCanvas();
		}
		if (myCanvas != null)
		{
			if (CanvasRect == null)
			{
				CanvasRect = (myCanvas.transform as RectTransform);
			}
		}
		if (!(CanvasRect != null))
		{
			return;
		}
		while (true)
		{
			Vector3 vector = boardSquareRef.gameObject.transform.position + Vector3.up * m_heightOffset;
			Vector3 b = Camera.main.WorldToScreenPoint(vector);
			Vector3 a = Camera.main.WorldToScreenPoint(vector + Camera.main.transform.up);
			Vector3 vector2 = a - b;
			vector2.z = 0f;
			float d = 1f / vector2.magnitude;
			Vector3 vector3 = Camera.main.transform.up * d;
			vector += vector3;
			Vector2 vector4 = Camera.main.WorldToViewportPoint(vector);
			float x = vector4.x;
			Vector2 sizeDelta = CanvasRect.sizeDelta;
			float x2 = x * sizeDelta.x;
			float y = vector4.y;
			Vector2 sizeDelta2 = CanvasRect.sizeDelta;
			Vector2 anchoredPosition = new Vector2(x2, y * sizeDelta2.y);
			(base.gameObject.transform as RectTransform).anchoredPosition = anchoredPosition;
			return;
		}
	}
}
