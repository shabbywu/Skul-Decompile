using System.Collections;
using Characters.Actions;
using FX;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Pope;

public sealed class Grace : Behaviour
{
	[SerializeField]
	private Action _ready;

	[SerializeField]
	private Action _attack;

	[SerializeField]
	private Action _end;

	[SerializeField]
	[UnityEditor.Subcomponent(typeof(MoveHandler))]
	private MoveHandler _moveHandler;

	[MinMaxSlider(0f, 180f)]
	[SerializeField]
	private Vector2 _radianRangeFromPlayerTarget;

	[SerializeField]
	private LineEffect[] _lineEffects;

	private const int _count = 3;

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		yield return _moveHandler.CMove(controller);
		_ready.TryStart();
		while (_ready.running)
		{
			yield return null;
		}
		for (int i = 0; i < 3; i++)
		{
			SetNoneTarget();
			_attack.TryStart();
			while (_attack.running)
			{
				yield return null;
			}
		}
		_end.TryStart();
		while (_end.running)
		{
			yield return null;
		}
		base.result = Result.Success;
	}

	public void SetNoneTarget()
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		int num = _lineEffects.Length;
		int num2 = Mathf.FloorToInt((float)((_lineEffects.Length - 1) / 2));
		float delta = 180 / num;
		float margin = 30f;
		float num3 = 15f;
		SetAngle(num, num2, delta, margin, num3);
		if (num % 2 != 0)
		{
			float num4 = Random.Range(0f, num3);
			float num5 = (MMMaths.RandomBool() ? 1 : (-1));
			((Component)_lineEffects[num2]).transform.rotation = Quaternion.AngleAxis(270f + num5 * num4, Vector3.forward);
		}
	}

	private void SetAngle(int n, int mid, float delta, float margin, float padding)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		float num = 180f + margin;
		float num2 = Random.Range(0f, padding);
		((Component)_lineEffects[0]).transform.rotation = Quaternion.AngleAxis(num + num2, Vector3.forward);
		num += delta;
		for (int i = 1; i <= mid; i++)
		{
			num2 = Random.Range(0f - padding, delta - padding);
			((Component)_lineEffects[i]).transform.rotation = Quaternion.AngleAxis(num + num2, Vector3.forward);
			num += delta;
		}
		num = 360f - margin;
		num2 = Random.Range(0f, padding);
		((Component)_lineEffects[mid + 1]).transform.rotation = Quaternion.AngleAxis(num - num2, Vector3.forward);
		num -= delta;
		for (int j = mid + 2; j < n; j++)
		{
			num2 = Random.Range(0f - padding, delta - padding);
			((Component)_lineEffects[j]).transform.rotation = Quaternion.AngleAxis(num - num2, Vector3.forward);
			num -= delta;
		}
	}
}
