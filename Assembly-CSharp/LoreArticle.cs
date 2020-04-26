using System;

[Serializable]
public class LoreArticle
{
	public int Index;

	public string Title;

	public string ImagePath;

	public string DatePublished;

	public string ArticleText;

	public CharacterType[] RelatedCharacters;

	public string GetTitle()
	{
		return StringUtil.TR_LoreTitle(Index);
	}

	public string GetArticleText()
	{
		return StringUtil.TR_LoreArticleText(Index);
	}
}
