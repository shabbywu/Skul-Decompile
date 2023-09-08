using System.Collections;
using BehaviorDesigner.Runtime;
using Characters.AI;
using UnityEngine;

namespace Characters.Operations.Customs;

public class ChaseTarget : CharacterOperation
{
	[SerializeField]
	private AIController _ai;

	[SerializeField]
	private BehaviorDesignerCommunicator _communicator;

	[SerializeField]
	private string _targetKey = "Target";

	[SerializeField]
	private float _duration;

	[SerializeField]
	private bool _lookTarget;

	private Coroutine _cExpire;

	private const float epsilon = 1f;

	public override void Run(Character owner)
	{
		Character target = ((!((Object)(object)_ai != (Object)null)) ? ((SharedVariable<Character>)_communicator.GetVariable<SharedCharacter>(_targetKey)).Value : _ai.target);
		_cExpire = ((MonoBehaviour)this).StartCoroutine(CRun(owner, target));
	}

	private IEnumerator CRun(Character owner, Character target)
	{
		float elpased = 0f;
		while (elpased <= _duration)
		{
			float num = ((Component)owner).transform.position.x - ((Component)target).transform.position.x;
			if (Mathf.Abs(num) > 1f)
			{
				owner.movement.MoveHorizontal((num > 0f) ? Vector2.left : Vector2.right);
			}
			if (_lookTarget)
			{
				owner.DesireToLookAt(((Component)target).transform.position.x);
			}
			yield return null;
		}
	}

	public override void Stop()
	{
		base.Stop();
		if (_cExpire != null)
		{
			((MonoBehaviour)this).StopCoroutine(_cExpire);
		}
	}
}
