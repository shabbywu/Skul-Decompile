using UnityEngine;

namespace Level;

[CreateAssetMenu]
public class CustomStageInfo : IStageInfo
{
	[SerializeField]
	private ParallaxBackground _background;

	[SerializeField]
	private Gate.Type _lastGate;

	[SerializeField]
	private SerializablePathNode.Reorderable _maps;

	public override (PathNode node1, PathNode node2) currentMapPath => GetPathAt(pathIndex);

	public override (PathNode node1, PathNode node2) nextMapPath => GetPathAt(pathIndex + 1);

	public override ParallaxBackground background => _background;

	private (PathNode node1, PathNode node2) GetPathAt(int pathIndex)
	{
		if (pathIndex >= _maps.values.Length)
		{
			return (new PathNode(null, MapReward.Type.None, _lastGate), PathNode.none);
		}
		return (_maps.values[pathIndex], PathNode.none);
	}

	public override void Reset()
	{
		pathIndex = 0;
	}

	public override void Initialize()
	{
	}

	public override PathNode Next(NodeIndex nodeIndex)
	{
		base.nodeIndex = nodeIndex;
		pathIndex++;
		if (pathIndex >= _maps.values.Length)
		{
			return null;
		}
		return _maps.values[pathIndex];
	}

	public override void UpdateReferences()
	{
	}
}
