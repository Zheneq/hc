using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScriptableObjectList
{
	public List<ScriptableObject> values;

	public readonly Type SubclassType;

	public readonly string AssetPath;

	public object this[int i]
	{
		get
		{
			return values[i];
		}
		set
		{
			values[i] = (value as ScriptableObject);
		}
	}

	public int Length => values.Count;

	public ScriptableObjectList(Type subclassType, string assetPath)
	{
		values = new List<ScriptableObject>();
		SubclassType = subclassType;
		AssetPath = assetPath;
	}

	public ScriptableObjectList(ScriptableObjectList other)
	{
		values = new List<ScriptableObject>(other.values);
		SubclassType = other.SubclassType;
		AssetPath = other.AssetPath;
	}

	public void AddItem(ScriptableObject newItem)
	{
		values.Add(newItem);
	}

	public void RemoveItem(ScriptableObject item)
	{
		values.Remove(item);
	}
}
