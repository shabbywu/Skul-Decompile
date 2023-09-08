using FX;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.Health;

public sealed class RangeHeal : CharacterOperation
{
	private enum Type
	{
		Percent,
		Constnat
	}

	[SerializeField]
	private EffectInfo _targetEffect;

	[SerializeField]
	private TargetLayer _targetLayer;

	[SerializeField]
	private Collider2D _range;

	[SerializeField]
	private int _maxCount;

	[SerializeField]
	private bool _exceptSelf;

	[SerializeField]
	private Type _type;

	[SerializeField]
	private CustomFloat _amount;

	private NonAllocOverlapper _overlapper;

	public override void Initialize()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		_overlapper = new NonAllocOverlapper(_maxCount);
	}

	public override void Run(Character owner)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_targetLayer.Evaluate(((Component)owner).gameObject));
		ReadonlyBoundedList<Collider2D> results = _overlapper.OverlapCollider(_range).results;
		for (int i = 0; i < results.Count; i++)
		{
			Target component = ((Component)results[i]).GetComponent<Target>();
			Character character = ((!((Object)(object)component == (Object)null)) ? component.character : ((Component)results[i]).GetComponent<Character>());
			if (!((Object)(object)character == (Object)null) && (!_exceptSelf || !((Object)(object)character == (Object)(object)owner)))
			{
				_targetEffect.Spawn(((Component)character).transform.position, character);
				character.health.Heal(GetAmount(character));
			}
		}
	}

	private double GetAmount(Character target)
	{
		return _type switch
		{
			Type.Percent => (double)_amount.value * target.health.maximumHealth * 0.01, 
			Type.Constnat => _amount.value, 
			_ => 0.0, 
		};
	}
}
