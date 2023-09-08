using System.Collections;
using Characters.AI.Behaviours;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public class PatrolCat : AIController
{
	[SerializeField]
	private bool _justIdle;

	[Subcomponent(typeof(Idle))]
	[SerializeField]
	private Idle _idle;

	[Subcomponent(typeof(MoveToDestination))]
	[SerializeField]
	private MoveToDestination _moveToDestination;

	[SerializeField]
	[MinMaxSlider(5f, 30f)]
	private Vector2 _distanceRange;

	[SerializeField]
	private Transform _minPoint;

	[SerializeField]
	private Transform _maxPoint;

	protected override void OnEnable()
	{
		if (!_justIdle)
		{
			((MonoBehaviour)this).StartCoroutine(CProcess());
		}
	}

	protected override IEnumerator CProcess()
	{
		while (true)
		{
			yield return _idle.CRun(this);
			SetDestination();
			yield return _moveToDestination.CRun(this);
		}
	}

	private void SetDestination()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		float num = Random.Range(_distanceRange.x, _distanceRange.y);
		int num2 = (MMMaths.RandomBool() ? 1 : (-1));
		switch (num2)
		{
		case 1:
			if (((Component)character).transform.position.x + (float)num2 * num >= _maxPoint.position.x)
			{
				num2 *= -1;
			}
			break;
		case -1:
			if (((Component)character).transform.position.x + (float)num2 * num <= _minPoint.position.x)
			{
				num2 *= -1;
			}
			break;
		}
		base.destination = new Vector2(((Component)character).transform.position.x + (float)num2 * num, ((Component)character).transform.position.y);
	}
}
