using System;

public class SceneVisibilityParameters
{
	public bool TurnOffAllOtherScenesInCanvasLayer;

	public bool PlayTransitions;

	public SceneStateParameters StateParameters;

	public SceneVisibilityParameters()
	{
		this.TurnOffAllOtherScenesInCanvasLayer = false;
		this.PlayTransitions = true;
		this.StateParameters = new SceneStateParameters();
	}
}
