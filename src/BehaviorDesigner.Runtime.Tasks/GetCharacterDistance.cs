using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("현재 챕터의 Stage Index를 저장합니다.")]
public sealed class GetCharacterDistance : Action
{
	[SerializeField]
	private SharedCharacter _owner;

	[SerializeField]
	private SharedCharacter _target;

	[SerializeField]
	private SharedFloat _storedDistance;

	[SerializeField]
	private bool _compareOnlyXDistance;

	private Character _ownerValue;

	public override void OnAwake()
	{
		_ownerValue = ((SharedVariable<Character>)_owner).Value;
	}

	public override TaskStatus OnUpdate()
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)_ownerValue == (Object)null) && !((Object)(object)((SharedVariable<Character>)_target).Value == (Object)null))
		{
			float num = (_compareOnlyXDistance ? Mathf.Abs(((Component)((SharedVariable<Character>)_target).Value).transform.position.x - ((Component)_ownerValue).transform.position.x) : Vector2.Distance(Vector2.op_Implicit(((Component)((SharedVariable<Character>)_target).Value).transform.position), Vector2.op_Implicit(((Component)_ownerValue).transform.position)));
			((SharedVariable)_storedDistance).SetValue((object)num);
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}
}
