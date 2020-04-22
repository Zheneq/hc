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
		return s_instance;
	}

	public LoreArticle GetArticleByIndex(int index)
	{
		if (m_loreArticles[index].Index != index)
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
		}
		return m_loreArticles[index];
	}

	public List<LoreArticle> GetArticlesByCharacter(CharacterType charType)
	{
		List<LoreArticle> list = new List<LoreArticle>();
		for (int i = 0; i < m_loreArticles.Count; i++)
		{
			if (m_loreArticles[i].RelatedCharacters.Contains(charType))
			{
				list.Add(m_loreArticles[i]);
			}
		}
		return list;
	}

	private void Awake()
	{
		s_instance = this;
	}

	private void OnDestroy()
	{
		s_instance = null;
	}
}
