using UnityEngine;

namespace Level;

[CreateAssetMenu]
public class SequentialStageInfo : IStageInfo
{
	[SerializeField]
	private ParallaxBackground _background;

	private PathNode[] _path;

	public override (PathNode node1, PathNode node2) currentMapPath => GetPathAt(pathIndex);

	public override (PathNode node1, PathNode node2) nextMapPath => GetPathAt(pathIndex + 1);

	public override ParallaxBackground background => _background;

	private (PathNode node1, PathNode node2) GetPathAt(int pathIndex)
	{
		return (_path[pathIndex], PathNode.none);
	}

	public override void Reset()
	{
		pathIndex = 0;
	}

	public override void Initialize()
	{
		_path = new PathNode[maps.Length + 1];
		for (int i = 0; i < maps.Length; i++)
		{
			_path[i] = new PathNode(maps[i], MapReward.Type.None, Gate.Type.Normal);
		}
		_path[maps.Length] = PathNode.none;
	}

	public override PathNode Next(NodeIndex nodeIndex)
	{
		base.nodeIndex = nodeIndex;
		pathIndex++;
		if (pathIndex >= _path.Length)
		{
			return null;
		}
		return _path[pathIndex];
	}

	public override void UpdateReferences()
	{
	}
}
