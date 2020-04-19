using System;
using System.Collections.Generic;
using UnityEngine;

public class ActorTag : MonoBehaviour
{
	[SerializeField]
	private List<string> m_tags = new List<string>();

	public bool HasTag(string tag)
	{
		return this.m_tags.Contains(tag);
	}

	public void AddTag(string tag)
	{
		this.m_tags.Add(tag);
	}

	public void RemoveTag(string tag)
	{
		this.m_tags.Remove(tag);
	}
}
