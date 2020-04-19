using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScriptableObjectList
{
	public List<ScriptableObject> values;

	public readonly Type SubclassType;

	public readonly string AssetPath;

	public ScriptableObjectList(Type subclassType, string assetPath)
	{
		this.values = new List<ScriptableObject>();
		this.SubclassType = subclassType;
		this.AssetPath = assetPath;
	}

	public ScriptableObjectList(ScriptableObjectList other)
	{
		this.values = new List<ScriptableObject>(other.values);
		this.SubclassType = other.SubclassType;
		this.AssetPath = other.AssetPath;
	}

	public object this[int i]
	{
		get
		{
			return this.values[i];
		}
		set
		{
			this.values[i] = (value as ScriptableObject);
		}
	}

	public int Length
	{
		get
		{
			return this.values.Count;
		}
	}

	public void AddItem(ScriptableObject newItem)
	{
		this.values.Add(newItem);
	}

	public void RemoveItem(ScriptableObject item)
	{
		this.values.Remove(item);
	}
}
