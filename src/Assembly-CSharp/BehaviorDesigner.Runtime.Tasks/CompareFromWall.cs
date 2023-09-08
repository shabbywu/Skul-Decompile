using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("벽과 캐릭터의 거리 비교")]
public sealed class CompareFromWall : Conditional
{
	private enum Comparer
	{
		GreaterThan,
		LessThan
	}

	[SerializeField]
	private SharedCharacter _character;

	[SerializeField]
	private SharedFloat _distance;

	[SerializeField]
	private Comparer _comparer;

	public override TaskStatus OnUpdate()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		Character value = ((SharedVariable<Character>)_character).Value;
		Collider2D lastStandingCollider = value.movement.controller.collisionState.lastStandingCollider;
		if ((Object)(object)lastStandingCollider == (Object)null)
		{
			return (TaskStatus)1;
		}
		Bounds bounds = lastStandingCollider.bounds;
		float num = ((((Component)value).transform.position.x > ((Bounds)(ref bounds)).center.x) ? Mathf.Abs(((Bounds)(ref bounds)).max.x - ((Component)value).transform.position.x) : Mathf.Abs(((Bounds)(ref bounds)).min.x - ((Component)value).transform.position.x));
		float value2 = ((SharedVariable<float>)_distance).Value;
		switch (_comparer)
		{
		case Comparer.GreaterThan:
			if (!(num >= value2))
			{
				return (TaskStatus)1;
			}
			return (TaskStatus)2;
		case Comparer.LessThan:
			if (!(num <= value2))
			{
				return (TaskStatus)1;
			}
			return (TaskStatus)2;
		default:
			return (TaskStatus)1;
		}
	}
}
