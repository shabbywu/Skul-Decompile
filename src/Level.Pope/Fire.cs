using System.Collections;
using Characters;
using Characters.Abilities;
using Characters.Operations;
using Characters.Operations.Attack;
using Level.Traps;
using UnityEditor;
using UnityEngine;

namespace Level.Pope;

public class Fire : Trap
{
	[SerializeField]
	private Character _character;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _onAppear;

	[Subcomponent(typeof(SweepAttack))]
	[SerializeField]
	private SweepAttack _attack;

	[SerializeField]
	[AbilityAttacher.Subcomponent]
	private AbilityAttacher _abilityAttacher;

	private void Awake()
	{
		_attack.Initialize();
		_attack.Run(_character);
		_onAppear.Initialize();
		_abilityAttacher.Initialize(_character);
		_abilityAttacher.StartAttach();
	}

	public void Appear()
	{
		((Component)this).gameObject.SetActive(true);
		((MonoBehaviour)this).StartCoroutine(_onAppear.CRun(_character));
		_attack.Run(_character);
		((MonoBehaviour)this).StartCoroutine(CAppear());
	}

	public void Disappear()
	{
		_attack.Stop();
		((MonoBehaviour)this).StartCoroutine(CDisappear());
	}

	private IEnumerator CAppear()
	{
		int num = 3;
		int time = 6;
		float elapsed = 0f;
		Vector2 start = Vector2.op_Implicit(((Component)this).transform.position);
		Vector2 end = new Vector2(start.x, start.y + (float)num);
		while (elapsed < (float)time)
		{
			((Component)this).transform.position = Vector2.op_Implicit(Vector2.Lerp(start, end, elapsed / (float)time));
			elapsed += _character.chronometer.master.deltaTime;
			yield return null;
		}
		((Component)this).transform.position = Vector2.op_Implicit(end);
	}

	private IEnumerator CDisappear()
	{
		int num = 4;
		int time = 4;
		float elapsed = 0f;
		Vector2 start = Vector2.op_Implicit(((Component)this).transform.position);
		Vector2 end = new Vector2(start.x, start.y - (float)num);
		while (elapsed < (float)time)
		{
			((Component)this).transform.position = Vector2.op_Implicit(Vector2.Lerp(start, end, elapsed / (float)time));
			elapsed += _character.chronometer.master.deltaTime;
			yield return null;
		}
		((Component)this).transform.position = Vector2.op_Implicit(end);
		((Component)this).gameObject.SetActive(false);
	}
}
