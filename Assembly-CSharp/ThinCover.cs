using System;
using UnityEngine;

public class ThinCover : MonoBehaviour, IGameEventListener
{
	public enum CoverType
	{
		None,
		Half,
		Full
	}

	public CoverType m_coverType;

	private void Awake()
	{
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.GameFlowDataStarted);
	}

	private void OnDestroy()
	{
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GameFlowDataStarted);
	}

	void IGameEventListener.OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType != GameEventManager.EventType.GameFlowDataStarted)
		{
			return;
		}
		while (true)
		{
			if (base.transform == null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						Debug.LogError("ThinCover recieving GameFlowDataStarted game event, but its transform is null.");
						return;
					}
				}
			}
			if (GameFlowData.Get() == null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						Debug.LogError("ThinCover recieving GameFlowDataStarted game event, but GameFlowData is null.");
						return;
					}
				}
			}
			if (GameFlowData.Get().GetThinCoverRoot() == null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						Debug.LogError("ThinCover recieving GameFlowDataStarted game event, but GameFlowData's ThinCoverRoot is null.");
						return;
					}
				}
			}
			try
			{
				base.transform.parent = GameFlowData.Get().GetThinCoverRoot().transform;
				UpdateBoardSquare();
			}
			catch (NullReferenceException)
			{
				Debug.LogError("Caught System.NullReferenceException for ThinCover receiving GameFlowDataStarted game event.  Highly unexpected!");
			}
			return;
		}
	}

	private void UpdateBoardSquare()
	{
		Vector3 position = base.transform.position;
		float squareSize = Board.Get().squareSize;
		float num = position.x / squareSize;
		float num2 = position.z / squareSize;
		int num3 = Mathf.RoundToInt(num);
		int num4 = Mathf.RoundToInt(num2);
		float num5 = num - (float)num3;
		float num6 = num2 - (float)num4;
		Board board = Board.Get();
		if (Mathf.Abs(num5) > Mathf.Abs(num6))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (num5 > 0f)
					{
						board.SetThinCover(num3, num4, ActorCover.CoverDirections.X_POS, m_coverType);
						if (num4 + 1 < board.GetMaxY())
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									break;
								default:
									board.SetThinCover(num3 + 1, num4, ActorCover.CoverDirections.X_NEG, m_coverType);
									return;
								}
							}
						}
					}
					else
					{
						board.SetThinCover(num3, num4, ActorCover.CoverDirections.X_NEG, m_coverType);
						if (num3 - 1 >= 0)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									break;
								default:
									board.SetThinCover(num3 - 1, num4, ActorCover.CoverDirections.X_POS, m_coverType);
									return;
								}
							}
						}
					}
					return;
				}
			}
		}
		if (num6 > 0f)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					board.SetThinCover(num3, num4, ActorCover.CoverDirections.Y_POS, m_coverType);
					if (num4 + 1 < board.GetMaxY())
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								board.SetThinCover(num3, num4 + 1, ActorCover.CoverDirections.Y_NEG, m_coverType);
								return;
							}
						}
					}
					return;
				}
			}
		}
		board.SetThinCover(num3, num4, ActorCover.CoverDirections.Y_NEG, m_coverType);
		if (num4 - 1 < 0)
		{
			return;
		}
		while (true)
		{
			board.SetThinCover(num3, num4 - 1, ActorCover.CoverDirections.Y_POS, m_coverType);
			return;
		}
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
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
		Gizmos.matrix = Matrix4x4.TRS(base.transform.position, base.transform.localRotation, Vector3.one);
		if (m_coverType == CoverType.Half)
		{
			Gizmos.DrawWireCube(Vector3.zero, new Vector3(1.5f, 1f, 0.1f));
		}
		else
		{
			Gizmos.DrawWireCube(Vector3.zero, new Vector3(1.5f, 2f, 0.1f));
		}
		Gizmos.matrix = Matrix4x4.identity;
		if (m_coverType == CoverType.Half)
		{
			Gizmos.DrawIcon(base.transform.position, "icon_HalfCover.png");
		}
		else
		{
			Gizmos.DrawIcon(base.transform.position, "icon_FullCover.png");
		}
	}
}
