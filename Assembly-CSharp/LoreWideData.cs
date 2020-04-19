using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LoreWideData : MonoBehaviour
{
	[Header("Articles")]
	public List<LoreArticle> m_loreArticles;

	private static LoreWideData s_instance;

	public static LoreWideData Get()
	{
		return LoreWideData.s_instance;
	}

	public LoreArticle GetArticleByIndex(int index)
	{
		if (this.m_loreArticles[index].Index != index)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LoreWideData.GetArticleByIndex(int)).MethodHandle;
			}
		}
		return this.m_loreArticles[index];
	}

	public List<LoreArticle> GetArticlesByCharacter(CharacterType charType)
	{
		List<LoreArticle> list = new List<LoreArticle>();
		for (int i = 0; i < this.m_loreArticles.Count; i++)
		{
			if (this.m_loreArticles[i].RelatedCharacters.Contains(charType))
			{
				list.Add(this.m_loreArticles[i]);
			}
		}
		return list;
	}

	private void Awake()
	{
		LoreWideData.s_instance = this;
	}

	private void OnDestroy()
	{
		LoreWideData.s_instance = null;
	}
}
