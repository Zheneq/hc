// ROGUES
// SERVER
using UnityEngine;

public class MatchObjective : MonoBehaviour //, IMissionTagged in rogues
{
	// rogues
	// public string RequiredMissionTag;

	public virtual void Client_OnActorDeath(ActorData actor)
	{
	}

	// rogues
	// public virtual void Server_OnActorRevived(ActorData actor)
	// {
	// }

	public virtual void Server_OnActorDeath(ActorData actor)
	{
	}

	// rogues
	// public List<string> GetRelevantTags()
	// {
	// 	if (!RequiredMissionTag.IsNullOrEmpty())
	// 	{
	// 		return new List<string>
	// 		{
	// 			RequiredMissionTag
	// 		};
	// 	}
	// 	return new List<string>();
	// }

	// rogues
	// public bool MatchesTag(string tag)
	// {
	// 	return !RequiredMissionTag.IsNullOrEmpty() && RequiredMissionTag.EqualsIgnoreCase(tag);
	// }
}
