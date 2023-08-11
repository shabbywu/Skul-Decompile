using GameResources;
using UnityEngine;

namespace Level;

public class TestStageInfo : IStageInfo
{
	[SerializeField]
	private ParallaxBackground _background;

	private PathNode _path;

	public override (PathNode node1, PathNode node2) currentMapPath => (_path, PathNode.none);

	public override (PathNode node1, PathNode node2) nextMapPath => (_path, PathNode.none);

	public override ParallaxBackground background => _background;

	public override void Reset()
	{
	}

	public override void Initialize()
	{
		_path = new PathNode(MapReference.FromPath("Assets/Level/Test/mapToTest.prefab"), MapReward.Type.Head, Gate.Type.Grave);
	}

	public override PathNode Next(NodeIndex nodeIndex)
	{
		return _path;
	}

	public override void UpdateReferences()
	{
	}
}
