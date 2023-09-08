using System.Collections.Generic;
using Characters;
using PhysicsUtils;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("콜라이더 범위 내에서 설정된 캐릭터들을 찾습니다.아무것도 찾지 못하면 Fail를 반환하고 하나 이상이라도 찾으면 Success를 반환 합니다.")]
public sealed class IsFoundCharactersInRange : Conditional
{
	[SerializeField]
	private SharedCharacter _owner;

	[SerializeField]
	private SharedCollider _range;

	[SerializeField]
	private TargetLayer _targetLayer = new TargetLayer(LayerMask.op_Implicit(0), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);

	[SerializeField]
	private bool _inculdeOwner;

	[SerializeField]
	private SharedCharacterList _targetsList;

	private NonAllocOverlapper _overlapper;

	private Character _ownerValue;

	private Collider2D _rangeValue;

	public override void OnAwake()
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		_overlapper = new NonAllocOverlapper(31);
		_ownerValue = ((SharedVariable<Character>)_owner).Value;
		_rangeValue = ((SharedVariable<Collider2D>)_range).Value;
	}

	public override TaskStatus OnUpdate()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		((Behaviour)_rangeValue).enabled = true;
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_targetLayer.Evaluate(((Component)_ownerValue).gameObject));
		List<Character> components = _overlapper.OverlapCollider(_rangeValue).GetComponents<Character>(true);
		if (!_inculdeOwner)
		{
			components.Remove(_ownerValue);
		}
		if (components.Count != 0)
		{
			if (_targetsList != null)
			{
				((SharedVariable)_targetsList).SetValue((object)components);
			}
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}
}
