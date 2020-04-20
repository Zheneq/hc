using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ControlPointCoordinator : MonoBehaviour
{
	public ControlPointCoordinator.ControlPointSpawnInfo[] m_controlPointSpawnInfo;

	public int m_initialSpawnDelay;

	public int m_respawnDelay;

	public int m_maxActivePoints = -1;

	public int m_minActivePoints;

	private static List<ControlPointCoordinator> s_controlPointCoordinators;

	private ControlPointCoordinator.ControlPointLocationInfo[] m_controlPointLocationInfo;

	public static List<ControlPointCoordinator> GetCoordinators()
	{
		if (ControlPointCoordinator.s_controlPointCoordinators == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPointCoordinator.GetCoordinators()).MethodHandle;
			}
			ControlPointCoordinator.s_controlPointCoordinators = new List<ControlPointCoordinator>();
		}
		return ControlPointCoordinator.s_controlPointCoordinators;
	}

	private void Awake()
	{
		if (NetworkServer.active)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPointCoordinator.Awake()).MethodHandle;
			}
			this.m_controlPointLocationInfo = new ControlPointCoordinator.ControlPointLocationInfo[this.m_controlPointSpawnInfo.Length];
			if (ControlPointCoordinator.s_controlPointCoordinators == null)
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
				ControlPointCoordinator.s_controlPointCoordinators = new List<ControlPointCoordinator>();
			}
			ControlPointCoordinator.s_controlPointCoordinators.Add(this);
		}
	}

	private void OnDestroy()
	{
		if (ControlPointCoordinator.s_controlPointCoordinators != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPointCoordinator.OnDestroy()).MethodHandle;
			}
			if (ControlPointCoordinator.s_controlPointCoordinators.Contains(this))
			{
				ControlPointCoordinator.s_controlPointCoordinators.Remove(this);
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPointCoordinator.OnDrawGizmos()).MethodHandle;
			}
			return;
		}
		if (this.m_controlPointSpawnInfo != null)
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
			foreach (ControlPointCoordinator.ControlPointSpawnInfo controlPointSpawnInfo2 in this.m_controlPointSpawnInfo)
			{
				controlPointSpawnInfo2.m_boardRegion.Initialize();
				controlPointSpawnInfo2.m_boardRegion.GizmosDrawRegion(controlPointSpawnInfo2.m_gizmoColor);
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
	}

	private void SpawnControlPoint(int index)
	{
		if (!NetworkServer.active)
		{
			Log.Error(string.Format("Trying to spawn a controlPoint on the client.", base.name, index), new object[0]);
		}
		else
		{
			if (index >= 0)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPointCoordinator.SpawnControlPoint(int)).MethodHandle;
				}
				if (index >= this.m_controlPointSpawnInfo.Length)
				{
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
				else
				{
					if (this.m_controlPointSpawnInfo[index] == null)
					{
						Log.Error(string.Format("Trying to spawn a controlPoint on coordinator {0}, but the spawn location is null (index = {1}).", base.name, index), new object[0]);
						return;
					}
					if (this.m_controlPointSpawnInfo[index].m_controlPointPrefab == null)
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
						Log.Error(string.Format("Trying to spawn a controlPoint on coordinator {0}, but the prefab for spawn location {1} is null.", base.name, index), new object[0]);
						return;
					}
					if (this.m_maxActivePoints != -1)
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
						if (this.m_maxActivePoints <= this.GetNumActivePoints())
						{
							return;
						}
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					Vector3 center = this.m_controlPointSpawnInfo[index].m_boardRegion.GetCenter();
					BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(center.x, center.z);
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_controlPointSpawnInfo[index].m_controlPointPrefab, boardSquareSafe.ToVector3(), Quaternion.identity);
					ControlPoint component = gameObject.GetComponent<ControlPoint>();
					if (component == null)
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
						throw new ApplicationException(string.Format("Control point prefab {0} does not have a control point component.", this.m_controlPointSpawnInfo[index].m_controlPointPrefab.name));
					}
					component.SetRegion(this.m_controlPointSpawnInfo[index].m_boardRegion);
					ControlPointCoordinator.ControlPointLocationInfo controlPointLocationInfo = new ControlPointCoordinator.ControlPointLocationInfo(component);
					this.m_controlPointLocationInfo[index] = controlPointLocationInfo;
					NetworkServer.Spawn(gameObject);
					return;
				}
			}
			Log.Error(string.Format("Trying to spawn a controlPoint on coordinator {0}, but the index ({1}) is invalid.", base.name, index), new object[0]);
		}
	}

	private void SpawnRandomControlPoint()
	{
		int num = this.m_controlPointLocationInfo.Length - this.GetNumActivePoints();
		int num2 = GameplayRandom.Range(0, num - 1);
		int i = 0;
		int num3 = 0;
		while (i < this.m_controlPointLocationInfo.Length)
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
			if (num3 >= num2)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					return;
				}
			}
			else
			{
				if (this.m_controlPointLocationInfo[i] == null)
				{
					goto IL_56;
				}
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPointCoordinator.SpawnRandomControlPoint()).MethodHandle;
				}
				if (this.m_controlPointLocationInfo[i].m_controlPoint == null)
				{
					goto IL_56;
				}
				IL_71:
				i++;
				continue;
				IL_56:
				if (num3 >= num2)
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
					this.SpawnControlPoint(i);
					break;
				}
				num2++;
				goto IL_71;
			}
		}
	}

	private int GetNumActivePoints()
	{
		int num = 0;
		for (int i = 0; i < this.m_controlPointLocationInfo.Length; i++)
		{
			ControlPointCoordinator.ControlPointLocationInfo controlPointLocationInfo = this.m_controlPointLocationInfo[i];
			if (controlPointLocationInfo != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPointCoordinator.GetNumActivePoints()).MethodHandle;
				}
				if (controlPointLocationInfo.m_controlPoint != null)
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
					num++;
				}
			}
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
		return num;
	}

	public void OnTurnStart()
	{
		int currentTurn = GameFlowData.Get().CurrentTurn;
		if (currentTurn >= this.m_initialSpawnDelay)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPointCoordinator.OnTurnStart()).MethodHandle;
			}
			for (int i = 0; i < this.m_controlPointSpawnInfo.Length; i++)
			{
				if (this.m_controlPointLocationInfo[i] == null)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					this.SpawnControlPoint(i);
				}
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		for (int j = 0; j < this.m_controlPointLocationInfo.Length; j++)
		{
			if (this.m_controlPointLocationInfo[j] != null)
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
				if (this.m_controlPointLocationInfo[j].m_respawnTurn != -1)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.m_controlPointLocationInfo[j].m_respawnTurn <= currentTurn)
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
						this.SpawnControlPoint(j);
					}
				}
			}
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
		List<ControlPoint> list = new List<ControlPoint>();
		for (int k = 0; k < this.m_controlPointLocationInfo.Length; k++)
		{
			if (this.m_controlPointLocationInfo[k] != null)
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
				if (this.m_controlPointLocationInfo[k].m_controlPoint != null)
				{
					ControlPoint controlPoint = this.m_controlPointLocationInfo[k].m_controlPoint;
					controlPoint.OnTurnStart_ControlPoint();
					if (controlPoint.ShouldControlPointEnd())
					{
						list.Add(controlPoint);
						this.m_controlPointLocationInfo[k].m_controlPoint = null;
					}
				}
			}
		}
		using (List<ControlPoint>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ControlPoint controlPoint2 = enumerator.Current;
				GameObject gameObject = controlPoint2.gameObject;
				NetworkServer.Destroy(gameObject);
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
		}
		int numActivePoints = this.GetNumActivePoints();
		if (this.m_minActivePoints > numActivePoints)
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
			int num = this.m_minActivePoints - numActivePoints;
			for (int l = 0; l < num; l++)
			{
				this.SpawnRandomControlPoint();
			}
		}
	}

	[Serializable]
	public class ControlPointSpawnInfo
	{
		public BoardRegion m_boardRegion;

		public GameObject m_controlPointPrefab;

		public Color m_gizmoColor = Color.magenta;
	}

	private class ControlPointLocationInfo
	{
		public ControlPoint m_controlPoint;

		public int m_respawnTurn;

		public ControlPointLocationInfo(ControlPoint controlPoint)
		{
			this.m_controlPoint = controlPoint;
			this.m_respawnTurn = -1;
		}
	}
}
