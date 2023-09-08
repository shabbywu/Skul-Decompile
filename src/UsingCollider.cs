using System;
using UnityEngine;

public struct UsingCollider : IDisposable
{
	public const string optimizeTooltip = "콜라이더 최적화 여부, Composite Collider등 특별한 경우가 아니면 true로 유지";

	private readonly Collider2D _collider;

	private readonly bool _optimize;

	public UsingCollider(Collider2D collider)
	{
		_optimize = true;
		_collider = collider;
		((Behaviour)_collider).enabled = true;
	}

	public UsingCollider(Collider2D collider, bool optimize)
	{
		_optimize = optimize;
		if (!_optimize)
		{
			_collider = null;
			return;
		}
		_collider = collider;
		((Behaviour)_collider).enabled = true;
	}

	public void Dispose()
	{
		if (_optimize)
		{
			((Behaviour)_collider).enabled = false;
		}
	}
}
