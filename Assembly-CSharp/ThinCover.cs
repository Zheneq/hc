using System;
using UnityEngine;

public class ThinCover : MonoBehaviour, IGameEventListener
{
	public ThinCover.CoverType m_coverType;

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
		if (eventType == GameEventManager.EventType.GameFlowDataStarted)
		{
			if (base.transform == null)
			{
				Debug.LogError("ThinCover recieving GameFlowDataStarted game event, but its transform is null.");
			}
			else if (GameFlowData.Get() == null)
			{
				Debug.LogError("ThinCover recieving GameFlowDataStarted game event, but GameFlowData is null.");
			}
			else if (GameFlowData.Get().GetThinCoverRoot() == null)
			{
				Debug.LogError("ThinCover recieving GameFlowDataStarted game event, but GameFlowData's ThinCoverRoot is null.");
			}
			else
			{
				try
				{
					base.transform.parent = GameFlowData.Get().GetThinCoverRoot().transform;
					this.UpdateBoardSquare();
				}
				catch (NullReferenceException)
				{
					Debug.LogError("Caught System.NullReferenceException for ThinCover receiving GameFlowDataStarted game event.  Highly unexpected!");
				}
			}
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
			if (num5 > 0f)
			{
				board.SetThinCover(num3, num4, ActorCover.CoverDirections.X_POS, this.m_coverType);
				if (num4 + 1 < board.GetMaxY())
				{
					board.SetThinCover(num3 + 1, num4, ActorCover.CoverDirections.X_NEG, this.m_coverType);
				}
			}
			else
			{
				board.SetThinCover(num3, num4, ActorCover.CoverDirections.X_NEG, this.m_coverType);
				if (num3 - 1 >= 0)
				{
					board.SetThinCover(num3 - 1, num4, ActorCover.CoverDirections.X_POS, this.m_coverType);
				}
			}
		}
		else if (num6 > 0f)
		{
			board.SetThinCover(num3, num4, ActorCover.CoverDirections.Y_POS, this.m_coverType);
			if (num4 + 1 < board.GetMaxY())
			{
				board.SetThinCover(num3, num4 + 1, ActorCover.CoverDirections.Y_NEG, this.m_coverType);
			}
		}
		else
		{
			board.SetThinCover(num3, num4, ActorCover.CoverDirections.Y_NEG, this.m_coverType);
			if (num4 - 1 >= 0)
			{
				board.SetThinCover(num3, num4 - 1, ActorCover.CoverDirections.Y_POS, this.m_coverType);
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			return;
		}
		Gizmos.matrix = Matrix4x4.TRS(base.transform.position, base.transform.localRotation, Vector3.one);
		if (this.m_coverType == ThinCover.CoverType.Half)
		{
			Gizmos.DrawWireCube(Vector3.zero, new Vector3(1.5f, 1f, 0.1f));
		}
		else
		{
			Gizmos.DrawWireCube(Vector3.zero, new Vector3(1.5f, 2f, 0.1f));
		}
		Gizmos.matrix = Matrix4x4.identity;
		if (this.m_coverType == ThinCover.CoverType.Half)
		{
			Gizmos.DrawIcon(base.transform.position, "icon_HalfCover.png");
		}
		else
		{
			Gizmos.DrawIcon(base.transform.position, "icon_FullCover.png");
		}
	}

	public enum CoverType
	{
		None,
		Half,
		Full
	}
}
