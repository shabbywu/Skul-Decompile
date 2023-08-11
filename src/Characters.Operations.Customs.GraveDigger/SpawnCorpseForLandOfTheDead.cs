using System.Collections;
using Characters.Abilities.Customs;
using UnityEngine;

namespace Characters.Operations.Customs.GraveDigger;

public class SpawnCorpseForLandOfTheDead : CharacterOperation
{
	[SerializeField]
	private CustomFloat _summonInterval;

	private GraveDiggerPassiveComponent _passive;

	private Vector2 _left;

	private Vector2 _right;

	public void Set(GraveDiggerPassiveComponent passive, Vector2 left, Vector2 right)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		_passive = passive;
		_left = left;
		_right = right;
	}

	public override void Run(Character owner)
	{
		((MonoBehaviour)this).StartCoroutine(CSummon());
	}

	private IEnumerator CSummon()
	{
		Vector2 val = default(Vector2);
		while (true)
		{
			((Vector2)(ref val))._002Ector(Random.Range(_left.x, _right.x), _left.y);
			_passive.SpawnCorpse(Vector2.op_Implicit(val));
			yield return Chronometer.global.WaitForSeconds(_summonInterval.value);
		}
	}
}
