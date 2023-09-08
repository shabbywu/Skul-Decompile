using UnityEngine;

namespace Characters;

public abstract class Laser : MonoBehaviour
{
	[SerializeField]
	protected Transform _originTransform;

	[SerializeField]
	protected LayerMask _terrainLayer;

	[SerializeField]
	protected float _maxLength;

	protected Vector2 _direction;

	protected Character _owner;

	public Vector2 direction => _direction;

	public virtual void Activate(Character owner, Vector2 direction)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		_owner = owner;
		Activate(direction);
	}

	public virtual void Activate(Character owner, float direction)
	{
		_owner = owner;
		Activate(direction);
	}

	public abstract void Activate(Vector2 direction);

	public abstract void Activate(float direction);

	public abstract void Deactivate();
}
