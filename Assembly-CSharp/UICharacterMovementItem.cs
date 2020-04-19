using System;
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

	public List<ActorData> Actors
	{
		get
		{
			return this.actorDataRef;
		}
	}

	public BoardSquare BoardPosition
	{
		get
		{
			return this.boardSquareRef;
		}
	}

	public void Setup(BoardSquare square, ActorData data)
	{
		this.boardSquareRef = square;
		this.AddActor(data);
	}

	public void AddActor(ActorData data)
	{
		if (this.actorDataRef.Contains(data))
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterMovementItem.AddActor(ActorData)).MethodHandle;
			}
			return;
		}
		this.actorDataRef.Add(data);
		this.UpdateIndicators();
	}

	public bool RemoveActor(ActorData data)
	{
		if (this.actorDataRef.Contains(data))
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterMovementItem.RemoveActor(ActorData)).MethodHandle;
			}
			this.actorDataRef.Remove(data);
			this.UpdateIndicators();
		}
		return this.actorDataRef.Count == 0;
	}

	private void UpdateIndicators()
	{
		int num = 0;
		int num2 = 0;
		if (GameFlowData.Get() != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterMovementItem.UpdateIndicators()).MethodHandle;
			}
			for (int i = 0; i < this.actorDataRef.Count; i++)
			{
				if (!(this.actorDataRef[i] == null))
				{
					if (!(ClientGameManager.Get() != null) || ClientGameManager.Get().PlayerInfo == null)
					{
						goto IL_98;
					}
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					bool flag;
					if (ClientGameManager.Get().PlayerInfo.TeamId != Team.Spectator)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							goto IL_98;
						}
					}
					else
					{
						flag = true;
					}
					IL_DB:
					bool flag2 = flag;
					bool flag3;
					if (flag2)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						flag3 = (this.actorDataRef[i].\u000E() == Team.TeamA);
					}
					else if (GameFlowData.Get().activeOwnedActorData != null)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						flag3 = (this.actorDataRef[i].\u000E() == GameFlowData.Get().activeOwnedActorData.\u000E());
					}
					else
					{
						flag3 = false;
					}
					bool flag4 = flag3;
					if (flag4)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						UICharacterMovementMarker uicharacterMovementMarker = this.m_blueMarkers[num];
						if (uicharacterMovementMarker != null)
						{
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							if (uicharacterMovementMarker.gameObject != null)
							{
								UIManager.SetGameObjectActive(uicharacterMovementMarker, true, null);
								uicharacterMovementMarker.m_characterImage.sprite = this.actorDataRef[i].\u0015();
								num++;
							}
						}
						goto IL_220;
					}
					UICharacterMovementMarker uicharacterMovementMarker2 = this.m_redMarkers[num2];
					if (uicharacterMovementMarker2 != null && uicharacterMovementMarker2.gameObject != null)
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						UIManager.SetGameObjectActive(uicharacterMovementMarker2, true, null);
						uicharacterMovementMarker2.m_characterImage.sprite = this.actorDataRef[i].\u0015();
						num2++;
						goto IL_220;
					}
					goto IL_220;
					IL_98:
					if (GameManager.Get() != null && GameManager.Get().PlayerInfo != null)
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						flag = (GameManager.Get().PlayerInfo.TeamId == Team.Spectator);
					}
					else
					{
						flag = false;
					}
					goto IL_DB;
				}
				IL_220:;
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		for (int j = num; j < this.m_blueMarkers.Length; j++)
		{
			if (this.m_blueMarkers[j] != null)
			{
				UIManager.SetGameObjectActive(this.m_blueMarkers[j], false, null);
			}
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		for (int k = num2; k < this.m_redMarkers.Length; k++)
		{
			if (this.m_redMarkers[k] != null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				UIManager.SetGameObjectActive(this.m_redMarkers[k], false, null);
			}
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	private void LateUpdate()
	{
		if (Camera.main == null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterMovementItem.LateUpdate()).MethodHandle;
			}
			return;
		}
		if (this.myCanvas == null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			this.myCanvas = HUD_UI.Get().GetTopLevelCanvas();
		}
		if (this.myCanvas != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.CanvasRect == null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				this.CanvasRect = (this.myCanvas.transform as RectTransform);
			}
		}
		if (this.CanvasRect != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			Vector3 vector = this.boardSquareRef.gameObject.transform.position + Vector3.up * this.m_heightOffset;
			Vector3 b = Camera.main.WorldToScreenPoint(vector);
			Vector3 a = Camera.main.WorldToScreenPoint(vector + Camera.main.transform.up);
			Vector3 vector2 = a - b;
			vector2.z = 0f;
			float d = 1f / vector2.magnitude;
			Vector3 b2 = Camera.main.transform.up * d;
			vector += b2;
			Vector2 vector3 = Camera.main.WorldToViewportPoint(vector);
			Vector2 anchoredPosition = new Vector2(vector3.x * this.CanvasRect.sizeDelta.x, vector3.y * this.CanvasRect.sizeDelta.y);
			(base.gameObject.transform as RectTransform).anchoredPosition = anchoredPosition;
		}
	}
}
