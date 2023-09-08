using System;
using UnityEditor;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class ColliderSubcomponentAttribute : SubcomponentAttribute
{
	public new static readonly Type[] types = new Type[6]
	{
		typeof(BoxCollider2D),
		typeof(CircleCollider2D),
		typeof(CapsuleCollider2D),
		typeof(PolygonCollider2D),
		typeof(EdgeCollider2D),
		typeof(CompositeCollider2D)
	};

	public readonly int layer;

	public readonly bool isTrigger;

	private static int ignoreRaycast = LayerMask.NameToLayer("Ignore Raycast");

	public ColliderSubcomponentAttribute(bool isTrigger = false)
		: base(allowCustom: true, types)
	{
		layer = ignoreRaycast;
		this.isTrigger = isTrigger;
	}

	public ColliderSubcomponentAttribute(int layer, bool isTrigger = false)
		: base(allowCustom: true, types)
	{
		this.layer = layer;
		this.isTrigger = isTrigger;
	}

	public ColliderSubcomponentAttribute(string layer, bool isTrigger = false)
		: base(allowCustom: true, types)
	{
		this.layer = LayerMask.NameToLayer(layer);
		this.isTrigger = isTrigger;
	}
}
