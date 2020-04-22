using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitchDetector : MonoBehaviour
{
	public struct GridLayoutStaggerInfo
	{
		public LayoutGroup m_layoutGroup;

		public List<GameObject> m_children;
	}

	public struct UILoadTimeInfo
	{
		public float startTime;

		public string recordString;
	}

	private List<GridLayoutStaggerInfo> m_gridlayoutInfo = new List<GridLayoutStaggerInfo>();

	private List<UILoadTimeInfo> loadTimeRecordings = new List<UILoadTimeInfo>();

	private List<GameObject> m_objectsJustEnabled = new List<GameObject>();

	private int clearObjectsJustEnabledList;

	private const float TimeToLogHitch = 0.1f;

	private float TimeUntilNextLogUpdateHitch;

	private static HitchDetector s_instance;

	private void Awake()
	{
		s_instance = this;
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	public static HitchDetector Get()
	{
		return s_instance;
	}

	public bool IsObjectStaggeringOn(GameObject obj)
	{
		if (obj != null)
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
			for (int i = 0; i < m_gridlayoutInfo.Count; i++)
			{
				GridLayoutStaggerInfo gridLayoutStaggerInfo = m_gridlayoutInfo[i];
				if (!(gridLayoutStaggerInfo.m_layoutGroup != null))
				{
					continue;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				GridLayoutStaggerInfo gridLayoutStaggerInfo2 = m_gridlayoutInfo[i];
				if (!gridLayoutStaggerInfo2.m_layoutGroup.gameObject.activeInHierarchy)
				{
					continue;
				}
				GridLayoutStaggerInfo gridLayoutStaggerInfo3 = m_gridlayoutInfo[i];
				if (!(gridLayoutStaggerInfo3.m_layoutGroup.gameObject == obj.transform.parent.gameObject))
				{
					continue;
				}
				GridLayoutStaggerInfo gridLayoutStaggerInfo4 = m_gridlayoutInfo[i];
				int result;
				if (gridLayoutStaggerInfo4.m_layoutGroup.gameObject.activeInHierarchy)
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
					if (!obj.gameObject.activeInHierarchy)
					{
						result = 1;
						goto IL_0112;
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
				result = (m_objectsJustEnabled.Contains(obj.gameObject) ? 1 : 0);
				goto IL_0112;
				IL_0112:
				return (byte)result != 0;
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
		return false;
	}

	public void RecordFrameTimeForHitch(string recordString)
	{
		if (!HydrogenConfig.Get().EnableHitchDetection)
		{
			return;
		}
		UILoadTimeInfo item = default(UILoadTimeInfo);
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
			if (!Application.isEditor)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					item.startTime = Time.unscaledTime;
					item.recordString = recordString;
					loadTimeRecordings.Add(item);
					return;
				}
			}
			return;
		}
	}

	private void Update()
	{
		if (m_objectsJustEnabled.Count > 0)
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
			if (clearObjectsJustEnabledList <= 0)
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
				m_objectsJustEnabled.Clear();
				goto IL_0051;
			}
		}
		clearObjectsJustEnabledList--;
		goto IL_0051;
		IL_0051:
		for (int i = 0; i < m_gridlayoutInfo.Count; i++)
		{
			GridLayoutStaggerInfo gridLayoutStaggerInfo = m_gridlayoutInfo[i];
			if (gridLayoutStaggerInfo.m_layoutGroup == null)
			{
				m_gridlayoutInfo.RemoveAt(i);
				i--;
				continue;
			}
			GridLayoutStaggerInfo gridLayoutStaggerInfo2 = m_gridlayoutInfo[i];
			if (!gridLayoutStaggerInfo2.m_layoutGroup.gameObject.activeInHierarchy)
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
				TurnOffAllChildren(m_gridlayoutInfo[i]);
				continue;
			}
			int num = 0;
			while (true)
			{
				int num2 = num;
				GridLayoutStaggerInfo gridLayoutStaggerInfo3 = m_gridlayoutInfo[i];
				if (num2 >= gridLayoutStaggerInfo3.m_children.Count)
				{
					break;
				}
				GridLayoutStaggerInfo gridLayoutStaggerInfo4 = m_gridlayoutInfo[i];
				if (gridLayoutStaggerInfo4.m_children[num] != null)
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
					GridLayoutStaggerInfo gridLayoutStaggerInfo5 = m_gridlayoutInfo[i];
					if (!gridLayoutStaggerInfo5.m_children[num].activeSelf)
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
						GridLayoutStaggerInfo gridLayoutStaggerInfo6 = m_gridlayoutInfo[i];
						StaggerComponent component = gridLayoutStaggerInfo6.m_children[num].GetComponent<StaggerComponent>();
						if (!(component == null))
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
							if (!component.DoActivateOnStagger)
							{
								goto IL_0280;
							}
						}
						clearObjectsJustEnabledList = 1;
						List<GameObject> objectsJustEnabled = m_objectsJustEnabled;
						GridLayoutStaggerInfo gridLayoutStaggerInfo7 = m_gridlayoutInfo[i];
						objectsJustEnabled.Add(gridLayoutStaggerInfo7.m_children[num].gameObject);
						GridLayoutStaggerInfo gridLayoutStaggerInfo8 = m_gridlayoutInfo[i];
						UIManager.SetGameObjectActive(gridLayoutStaggerInfo8.m_children[num], true);
						break;
					}
					GridLayoutStaggerInfo gridLayoutStaggerInfo9 = m_gridlayoutInfo[i];
					StaggerComponent component2 = gridLayoutStaggerInfo9.m_children[num].GetComponent<StaggerComponent>();
					if (component2 != null)
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
						if (!component2.DoActivateOnStagger)
						{
							GridLayoutStaggerInfo gridLayoutStaggerInfo10 = m_gridlayoutInfo[i];
							UIManager.SetGameObjectActive(gridLayoutStaggerInfo10.m_children[num], false);
						}
					}
					goto IL_0280;
				}
				GridLayoutStaggerInfo gridLayoutStaggerInfo11 = m_gridlayoutInfo[i];
				RedoLayoutGroup(gridLayoutStaggerInfo11.m_layoutGroup);
				break;
				IL_0280:
				num++;
			}
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (Application.isEditor)
			{
				return;
			}
			int num3 = 0;
			bool flag = false;
			while (num3 < loadTimeRecordings.Count)
			{
				float unscaledTime = Time.unscaledTime;
				UILoadTimeInfo uILoadTimeInfo = loadTimeRecordings[num3];
				if (unscaledTime <= uILoadTimeInfo.startTime)
				{
					num3++;
				}
				else if (num3 < loadTimeRecordings.Count)
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
					float unscaledTime2 = Time.unscaledTime;
					UILoadTimeInfo uILoadTimeInfo2 = loadTimeRecordings[num3];
					float num4 = unscaledTime2 - uILoadTimeInfo2.startTime;
					if (num4 >= 0.1f)
					{
						flag = true;
						UILoadTimeInfo uILoadTimeInfo3 = loadTimeRecordings[num3];
						Log.Error(string.Format("Hitch occurred: {0} took {1} seconds", uILoadTimeInfo3.recordString, num4.ToString("F3")));
					}
					loadTimeRecordings.RemoveAt(num3);
				}
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (!HydrogenConfig.Get().EnableRandomFrameHitchDetection)
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
					if (TimeUntilNextLogUpdateHitch > 0f)
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
						TimeUntilNextLogUpdateHitch -= Time.unscaledDeltaTime;
					}
					if (flag || !(Time.unscaledDeltaTime >= 0.1f))
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
						if (TimeUntilNextLogUpdateHitch <= 0f)
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								TimeUntilNextLogUpdateHitch = 5f;
								Log.Error(string.Format("Unknown cause hitch occurred: Took {0} seconds", Time.unscaledDeltaTime.ToString("F3")));
								return;
							}
						}
						return;
					}
				}
			}
		}
	}

	private void RedoLayoutGroup(LayoutGroup group)
	{
		GridLayoutStaggerInfo value = default(GridLayoutStaggerInfo);
		for (int i = 0; i < m_gridlayoutInfo.Count; i++)
		{
			GridLayoutStaggerInfo gridLayoutStaggerInfo = m_gridlayoutInfo[i];
			if (gridLayoutStaggerInfo.m_layoutGroup == null)
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
				m_gridlayoutInfo.RemoveAt(i);
				continue;
			}
			GridLayoutStaggerInfo gridLayoutStaggerInfo2 = m_gridlayoutInfo[i];
			if (gridLayoutStaggerInfo2.m_layoutGroup == group)
			{
				GridLayoutStaggerInfo gridLayoutStaggerInfo3 = m_gridlayoutInfo[i];
				value.m_layoutGroup = gridLayoutStaggerInfo3.m_layoutGroup;
				GridLayoutStaggerInfo gridLayoutStaggerInfo4 = m_gridlayoutInfo[i];
				value.m_children = gridLayoutStaggerInfo4.m_children;
				value.m_children.Clear();
				IEnumerator enumerator = group.gameObject.transform.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Transform transform = (Transform)enumerator.Current;
						if (transform.parent.gameObject == group.gameObject)
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
							value.m_children.Add(transform.gameObject);
						}
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								disposable.Dispose();
								goto end_IL_0123;
							}
						}
					}
					end_IL_0123:;
				}
				m_gridlayoutInfo[i] = value;
				return;
			}
		}
		while (true)
		{
			switch (1)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public void AddNewLayoutGroup(LayoutGroup group)
	{
		if (group == null)
		{
			while (true)
			{
				switch (1)
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
		for (int i = 0; i < m_gridlayoutInfo.Count; i++)
		{
			GridLayoutStaggerInfo gridLayoutStaggerInfo = m_gridlayoutInfo[i];
			if (!(gridLayoutStaggerInfo.m_layoutGroup == group))
			{
				continue;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				RedoLayoutGroup(group);
				return;
			}
		}
		GridLayoutStaggerInfo item = default(GridLayoutStaggerInfo);
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			item.m_layoutGroup = group;
			item.m_children = new List<GameObject>();
			IEnumerator enumerator = group.gameObject.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.Current;
					if (transform.parent.gameObject == group.gameObject)
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
						item.m_children.Add(transform.gameObject);
					}
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
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							disposable.Dispose();
							goto end_IL_00fd;
						}
					}
				}
				end_IL_00fd:;
			}
			if (item.m_children.Count > 0)
			{
				m_gridlayoutInfo.Add(item);
			}
			return;
		}
	}

	private void TurnOffAllChildren(GridLayoutStaggerInfo info)
	{
		int num = 0;
		while (num < info.m_children.Count)
		{
			if (info.m_children[num] != null)
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
						UIManager.SetGameObjectActive(info.m_children[num], false);
						num++;
						goto IL_0059;
					}
				}
			}
			RedoLayoutGroup(info.m_layoutGroup);
			return;
			IL_0059:;
		}
		while (true)
		{
			switch (1)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}
}
