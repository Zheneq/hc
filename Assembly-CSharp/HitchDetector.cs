using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitchDetector : MonoBehaviour
{
	private List<HitchDetector.GridLayoutStaggerInfo> m_gridlayoutInfo = new List<HitchDetector.GridLayoutStaggerInfo>();

	private List<HitchDetector.UILoadTimeInfo> loadTimeRecordings = new List<HitchDetector.UILoadTimeInfo>();

	private List<GameObject> m_objectsJustEnabled = new List<GameObject>();

	private int clearObjectsJustEnabledList;

	private const float TimeToLogHitch = 0.1f;

	private float TimeUntilNextLogUpdateHitch;

	private static HitchDetector s_instance;

	private void Awake()
	{
		HitchDetector.s_instance = this;
	}

	private void OnDestroy()
	{
		HitchDetector.s_instance = null;
	}

	public static HitchDetector Get()
	{
		return HitchDetector.s_instance;
	}

	public bool IsObjectStaggeringOn(GameObject obj)
	{
		if (obj != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HitchDetector.IsObjectStaggeringOn(GameObject)).MethodHandle;
			}
			for (int i = 0; i < this.m_gridlayoutInfo.Count; i++)
			{
				if (this.m_gridlayoutInfo[i].m_layoutGroup != null)
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
					if (this.m_gridlayoutInfo[i].m_layoutGroup.gameObject.activeInHierarchy && this.m_gridlayoutInfo[i].m_layoutGroup.gameObject == obj.transform.parent.gameObject)
					{
						if (this.m_gridlayoutInfo[i].m_layoutGroup.gameObject.activeInHierarchy)
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
							if (!obj.gameObject.activeInHierarchy)
							{
								return true;
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
						return this.m_objectsJustEnabled.Contains(obj.gameObject);
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
		}
		return false;
	}

	public void RecordFrameTimeForHitch(string recordString)
	{
		if (HydrogenConfig.Get().EnableHitchDetection)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(HitchDetector.RecordFrameTimeForHitch(string)).MethodHandle;
			}
			if (!Application.isEditor)
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
				HitchDetector.UILoadTimeInfo item;
				item.startTime = Time.unscaledTime;
				item.recordString = recordString;
				this.loadTimeRecordings.Add(item);
			}
		}
	}

	private void Update()
	{
		if (this.m_objectsJustEnabled.Count > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HitchDetector.Update()).MethodHandle;
			}
			if (this.clearObjectsJustEnabledList <= 0)
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
				this.m_objectsJustEnabled.Clear();
				goto IL_51;
			}
		}
		this.clearObjectsJustEnabledList--;
		IL_51:
		for (int i = 0; i < this.m_gridlayoutInfo.Count; i++)
		{
			if (this.m_gridlayoutInfo[i].m_layoutGroup == null)
			{
				this.m_gridlayoutInfo.RemoveAt(i);
				i--;
			}
			else if (!this.m_gridlayoutInfo[i].m_layoutGroup.gameObject.activeInHierarchy)
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
				this.TurnOffAllChildren(this.m_gridlayoutInfo[i]);
			}
			else
			{
				for (int j = 0; j < this.m_gridlayoutInfo[i].m_children.Count; j++)
				{
					if (!(this.m_gridlayoutInfo[i].m_children[j] != null))
					{
						this.RedoLayoutGroup(this.m_gridlayoutInfo[i].m_layoutGroup);
						break;
					}
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!this.m_gridlayoutInfo[i].m_children[j].activeSelf)
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
						StaggerComponent component = this.m_gridlayoutInfo[i].m_children[j].GetComponent<StaggerComponent>();
						if (!(component == null))
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
							if (!component.DoActivateOnStagger)
							{
								goto IL_261;
							}
						}
						this.clearObjectsJustEnabledList = 1;
						this.m_objectsJustEnabled.Add(this.m_gridlayoutInfo[i].m_children[j].gameObject);
						UIManager.SetGameObjectActive(this.m_gridlayoutInfo[i].m_children[j], true, null);
						break;
					}
					StaggerComponent component2 = this.m_gridlayoutInfo[i].m_children[j].GetComponent<StaggerComponent>();
					if (component2 != null)
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
						if (!component2.DoActivateOnStagger)
						{
							UIManager.SetGameObjectActive(this.m_gridlayoutInfo[i].m_children[j], false, null);
						}
					}
					IL_261:;
				}
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!Application.isEditor)
		{
			int k = 0;
			bool flag = false;
			while (k < this.loadTimeRecordings.Count)
			{
				if (Time.unscaledTime <= this.loadTimeRecordings[k].startTime)
				{
					k++;
				}
				else if (k < this.loadTimeRecordings.Count)
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
					float num = Time.unscaledTime - this.loadTimeRecordings[k].startTime;
					if (num >= 0.1f)
					{
						flag = true;
						Log.Error(string.Format("Hitch occurred: {0} took {1} seconds", this.loadTimeRecordings[k].recordString, num.ToString("F3")), new object[0]);
					}
					this.loadTimeRecordings.RemoveAt(k);
				}
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (HydrogenConfig.Get().EnableRandomFrameHitchDetection)
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
				if (this.TimeUntilNextLogUpdateHitch > 0f)
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
					this.TimeUntilNextLogUpdateHitch -= Time.unscaledDeltaTime;
				}
				if (!flag && Time.unscaledDeltaTime >= 0.1f)
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
					if (this.TimeUntilNextLogUpdateHitch <= 0f)
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
						this.TimeUntilNextLogUpdateHitch = 5f;
						Log.Error(string.Format("Unknown cause hitch occurred: Took {0} seconds", Time.unscaledDeltaTime.ToString("F3")), new object[0]);
					}
				}
			}
		}
	}

	private void RedoLayoutGroup(LayoutGroup group)
	{
		for (int i = 0; i < this.m_gridlayoutInfo.Count; i++)
		{
			if (this.m_gridlayoutInfo[i].m_layoutGroup == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(HitchDetector.RedoLayoutGroup(LayoutGroup)).MethodHandle;
				}
				this.m_gridlayoutInfo.RemoveAt(i);
			}
			else if (this.m_gridlayoutInfo[i].m_layoutGroup == group)
			{
				HitchDetector.GridLayoutStaggerInfo value;
				value.m_layoutGroup = this.m_gridlayoutInfo[i].m_layoutGroup;
				value.m_children = this.m_gridlayoutInfo[i].m_children;
				value.m_children.Clear();
				IEnumerator enumerator = group.gameObject.transform.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						Transform transform = (Transform)obj;
						if (transform.parent.gameObject == group.gameObject)
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
							value.m_children.Add(transform.gameObject);
						}
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
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
						disposable.Dispose();
					}
				}
				this.m_gridlayoutInfo[i] = value;
				return;
			}
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			return;
		}
	}

	public void AddNewLayoutGroup(LayoutGroup group)
	{
		if (group == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HitchDetector.AddNewLayoutGroup(LayoutGroup)).MethodHandle;
			}
			return;
		}
		for (int i = 0; i < this.m_gridlayoutInfo.Count; i++)
		{
			if (this.m_gridlayoutInfo[i].m_layoutGroup == group)
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
				this.RedoLayoutGroup(group);
				return;
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			break;
		}
		HitchDetector.GridLayoutStaggerInfo item;
		item.m_layoutGroup = group;
		item.m_children = new List<GameObject>();
		IEnumerator enumerator = group.gameObject.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				if (transform.parent.gameObject == group.gameObject)
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
					item.m_children.Add(transform.gameObject);
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
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
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
				disposable.Dispose();
			}
		}
		if (item.m_children.Count > 0)
		{
			this.m_gridlayoutInfo.Add(item);
		}
	}

	private void TurnOffAllChildren(HitchDetector.GridLayoutStaggerInfo info)
	{
		for (int i = 0; i < info.m_children.Count; i++)
		{
			if (!(info.m_children[i] != null))
			{
				this.RedoLayoutGroup(info.m_layoutGroup);
				return;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(HitchDetector.TurnOffAllChildren(HitchDetector.GridLayoutStaggerInfo)).MethodHandle;
			}
			UIManager.SetGameObjectActive(info.m_children[i], false, null);
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			return;
		}
	}

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
}
