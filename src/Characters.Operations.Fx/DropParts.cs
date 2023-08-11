using UnityEngine;

namespace Characters.Operations.Fx;

public class DropParts : CharacterOperation
{
	[SerializeField]
	private Transform _spawnPoint;

	[SerializeField]
	private Collider2D _range;

	[SerializeField]
	private ParticleEffectInfo _particleEffectInfo;

	[SerializeField]
	private Vector2 _direction;

	[SerializeField]
	private float _power = 3f;

	[SerializeField]
	private bool _interpolation;

	[SerializeField]
	private bool _pickRandomOne;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_particleEffectInfo = null;
	}

	public override void Run(Character owner)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		if (_pickRandomOne)
		{
			_particleEffectInfo.EmitRandom(Vector2.op_Implicit(_spawnPoint.position), _range.bounds, _direction * _power, _interpolation);
		}
		else
		{
			_particleEffectInfo.Emit(Vector2.op_Implicit(_spawnPoint.position), _range.bounds, _direction * _power, _interpolation);
		}
	}
}
