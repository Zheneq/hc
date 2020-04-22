using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ControlPointCoordinator : MonoBehaviour
{
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
			m_controlPoint = controlPoint;
			m_respawnTurn = -1;
		}
	}

	public ControlPointSpawnInfo[] m_controlPointSpawnInfo;

	public int m_initialSpawnDelay;

	public int m_respawnDelay;

	public int m_maxActivePoints = -1;

	public int m_minActivePoints;

	private static List<ControlPointCoordinator> s_controlPointCoordinators;

	private ControlPointLocationInfo[] m_controlPointLocationInfo;

	public static List<ControlPointCoordinator> GetCoordinators()
	{
		if (s_controlPointCoordinators == null)
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
			s_controlPointCoordinators = new List<ControlPointCoordinator>();
		}
		return s_controlPointCoordinators;
	}

	private void Awake()
	{
		if (!NetworkServer.active)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_controlPointLocationInfo = new ControlPointLocationInfo[m_controlPointSpawnInfo.Length];
			if (s_controlPointCoordinators == null)
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
				s_controlPointCoordinators = new List<ControlPointCoordinator>();
			}
			s_controlPointCoordinators.Add(this);
			return;
		}
	}

	private void OnDestroy()
	{
		if (s_controlPointCoordinators == null)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (s_controlPointCoordinators.Contains(this))
			{
				s_controlPointCoordinators.Remove(this);
			}
			return;
		}
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
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
					return;
				}
			}
		}
		if (m_controlPointSpawnInfo == null)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			ControlPointSpawnInfo[] controlPointSpawnInfo = m_controlPointSpawnInfo;
			foreach (ControlPointSpawnInfo controlPointSpawnInfo2 in controlPointSpawnInfo)
			{
				controlPointSpawnInfo2.m_boardRegion.Initialize();
				controlPointSpawnInfo2.m_boardRegion.GizmosDrawRegion(controlPointSpawnInfo2.m_gizmoColor);
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

	private void SpawnControlPoint(int index)
	{
		if (!NetworkServer.active)
		{
			Log.Error(string.Format("Trying to spawn a controlPoint on the client.", base.name, index));
			return;
		}
		if (index >= 0)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (index < m_controlPointSpawnInfo.Length)
			{
				if (m_controlPointSpawnInfo[index] == null)
				{
					Log.Error($"Trying to spawn a controlPoint on coordinator {base.name}, but the spawn location is null (index = {index}).");
					return;
				}
				if (m_controlPointSpawnInfo[index].m_controlPointPrefab == null)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							Log.Error($"Trying to spawn a controlPoint on coordinator {base.name}, but the prefab for spawn location {index} is null.");
							return;
						}
					}
				}
				if (m_maxActivePoints != -1)
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
					if (m_maxActivePoints <= GetNumActivePoints())
					{
						return;
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
				Vector3 center = m_controlPointSpawnInfo[index].m_boardRegion.GetCenter();
				BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(center.x, center.z);
				GameObject gameObject = UnityEngine.Object.Instantiate(m_controlPointSpawnInfo[index].m_controlPointPrefab, boardSquareSafe.ToVector3(), Quaternion.identity);
				ControlPoint component = gameObject.GetComponent<ControlPoint>();
				if (component == null)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							throw new ApplicationException($"Control point prefab {m_controlPointSpawnInfo[index].m_controlPointPrefab.name} does not have a control point component.");
						}
					}
				}
				component.SetRegion(m_controlPointSpawnInfo[index].m_boardRegion);
				ControlPointLocationInfo controlPointLocationInfo = new ControlPointLocationInfo(component);
				m_controlPointLocationInfo[index] = controlPointLocationInfo;
				NetworkServer.Spawn(gameObject);
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
		Log.Error($"Trying to spawn a controlPoint on coordinator {base.name}, but the index ({index}) is invalid.");
	}

	private void SpawnRandomControlPoint()
	{
		int num = m_controlPointLocationInfo.Length - GetNumActivePoints();
		int num2 = GameplayRandom.Range(0, num - 1);
		int num3 = 0;
		int num4 = 0;
		while (num3 < m_controlPointLocationInfo.Length)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				if (num4 < num2)
				{
					if (m_controlPointLocationInfo[num3] != null)
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
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						if (!(m_controlPointLocationInfo[num3].m_controlPoint == null))
						{
							goto IL_0071;
						}
					}
					if (num4 >= num2)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							SpawnControlPoint(num3);
							return;
						}
					}
					num2++;
					goto IL_0071;
				}
				while (true)
				{
					switch (2)
					{
					default:
						return;
					case 0:
						break;
					}
				}
				IL_0071:
				num3++;
				goto IL_0075;
			}
			IL_0075:;
		}
	}

	private int GetNumActivePoints()
	{
		int num = 0;
		for (int i = 0; i < m_controlPointLocationInfo.Length; i++)
		{
			ControlPointLocationInfo controlPointLocationInfo = m_controlPointLocationInfo[i];
			if (controlPointLocationInfo == null)
			{
				continue;
			}
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
			if (controlPointLocationInfo.m_controlPoint != null)
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
				num++;
			}
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			return num;
		}
	}

	public void OnTurnStart()
	{
		int currentTurn = GameFlowData.Get().CurrentTurn;
		if (currentTurn >= m_initialSpawnDelay)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			for (int i = 0; i < m_controlPointSpawnInfo.Length; i++)
			{
				if (m_controlPointLocationInfo[i] == null)
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
					SpawnControlPoint(i);
				}
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		for (int j = 0; j < m_controlPointLocationInfo.Length; j++)
		{
			if (m_controlPointLocationInfo[j] == null)
			{
				continue;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_controlPointLocationInfo[j].m_respawnTurn == -1)
			{
				continue;
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
			if (m_controlPointLocationInfo[j].m_respawnTurn <= currentTurn)
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
				SpawnControlPoint(j);
			}
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			List<ControlPoint> list = new List<ControlPoint>();
			for (int k = 0; k < m_controlPointLocationInfo.Length; k++)
			{
				if (m_controlPointLocationInfo[k] == null)
				{
					continue;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (m_controlPointLocationInfo[k].m_controlPoint != null)
				{
					ControlPoint controlPoint = m_controlPointLocationInfo[k].m_controlPoint;
					controlPoint.OnTurnStart_ControlPoint();
					if (controlPoint.ShouldControlPointEnd())
					{
						list.Add(controlPoint);
						m_controlPointLocationInfo[k].m_controlPoint = null;
					}
				}
			}
			using (List<ControlPoint>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ControlPoint current = enumerator.Current;
					GameObject gameObject = current.gameObject;
					NetworkServer.Destroy(gameObject);
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			int numActivePoints = GetNumActivePoints();
			if (m_minActivePoints <= numActivePoints)
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				int num = m_minActivePoints - numActivePoints;
				for (int l = 0; l < num; l++)
				{
					SpawnRandomControlPoint();
				}
				return;
			}
		}
	}
}
