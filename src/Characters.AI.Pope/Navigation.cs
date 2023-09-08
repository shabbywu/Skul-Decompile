using System.Collections.Generic;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.AI.Pope;

public sealed class Navigation : MonoBehaviour
{
	[SerializeField]
	private Transform _pointContainer;

	[SerializeField]
	private Collider2D _platformArea;

	private Point.Tag _destinationTag;

	private Point _top;

	private Point _center;

	private List<Point> _inners = new List<Point>();

	public Transform destination { get; set; }

	public Point.Tag destinationTag
	{
		get
		{
			return _destinationTag;
		}
		set
		{
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			switch (value)
			{
			case Point.Tag.Center:
				destination = ((Component)_center).transform;
				break;
			case Point.Tag.Top:
				destination = ((Component)_top).transform;
				break;
			case Point.Tag.None:
				destination = _pointContainer.GetChild(Random.Range(0, _pointContainer.childCount - 1));
				break;
			case Point.Tag.Opposition:
			{
				Character player = Singleton<Service>.Instance.levelManager.player;
				int floor = GetFloor(((Component)player).transform.position.y);
				Point[] componentsInChildren = ((Component)_pointContainer).GetComponentsInChildren<Point>();
				foreach (Point point in componentsInChildren)
				{
					if (point.tag == Point.Tag.Opposition && point.floor == floor)
					{
						Bounds bounds = Map.Instance.bounds;
						float num = Mathf.Sign(((Bounds)(ref bounds)).center.x - ((Component)player).transform.position.x);
						bounds = Map.Instance.bounds;
						if (num != Mathf.Sign(((Bounds)(ref bounds)).center.x - ((Component)point).transform.position.x))
						{
							destination = ((Component)point).transform;
							break;
						}
					}
				}
				break;
			}
			case Point.Tag.Inner:
				destination = ((Component)_inners.Random()).transform;
				break;
			}
		}
	}

	private int GetFloor(float target)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = _platformArea.bounds;
		float num = (((Bounds)(ref bounds)).max.y - ((Bounds)(ref bounds)).min.y) / 5f;
		if (target < ((Bounds)(ref bounds)).min.y + num)
		{
			return 1;
		}
		if (target < ((Bounds)(ref bounds)).min.y + num * 2f)
		{
			return 2;
		}
		if (target < ((Bounds)(ref bounds)).min.y + num * 3f)
		{
			return 3;
		}
		if (target < ((Bounds)(ref bounds)).min.y + num * 4f)
		{
			return 4;
		}
		return 5;
	}

	private void Awake()
	{
		Point[] componentsInChildren = ((Component)_pointContainer).GetComponentsInChildren<Point>();
		foreach (Point point in componentsInChildren)
		{
			if (point.tag == Point.Tag.Top)
			{
				_top = point;
			}
			else if (point.tag == Point.Tag.Center)
			{
				_center = point;
			}
			else if (point.tag == Point.Tag.Inner)
			{
				_inners.Add(point);
			}
		}
	}
}
