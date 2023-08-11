using GameResources;
using UnityEngine;

namespace Level;

public class PathNode
{
	[HideInInspector]
	public MapReference reference;

	public MapReward.Type reward;

	public Gate.Type gate;

	public static readonly PathNode none = new PathNode(null, MapReward.Type.None, Gate.Type.None);

	public PathNode()
	{
	}

	public PathNode(MapReference reference, MapReward.Type reward, Gate.Type gate)
	{
		this.reference = reference;
		this.reward = reward;
		this.gate = gate;
	}
}
