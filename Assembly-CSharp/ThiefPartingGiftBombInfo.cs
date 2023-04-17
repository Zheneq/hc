using System;
using UnityEngine;

[Serializable]
public class ThiefPartingGiftBombInfo
{
	public int damageDelay = 1;
	public int damageAmount = 3;
	public AbilityAreaShape shape = AbilityAreaShape.Three_x_Three;
	public bool penetrateLos;
	[Header("-- Effect when bomb effect is applied, base effect data")]
	public StandardActorEffectData onHitEffectData;
	[Header("-- Effect when bomb explodes")]
	public StandardEffectInfo onExplodeEffect;
	[Header("-- Sequence for thief bomb.  The sequence is in charge of all phases")]
	public GameObject sequencePrefab;
	public GameObject explosionSequencePrefab;
}
