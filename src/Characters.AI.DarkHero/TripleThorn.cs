using System.Collections;
using Characters.Operations;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.AI.DarkHero;

public sealed class TripleThorn : MonoBehaviour
{
	[SerializeField]
	private Character _owner;

	[SerializeField]
	private Animator[] _thorns;

	[SerializeField]
	private OperationInfos _operationInfos;

	[SerializeField]
	private AnimationClip _endClip;

	[SerializeField]
	protected SoundInfo _endSound;

	private readonly int attackHash = Animator.StringToHash("Start");

	private readonly int returnHash = Animator.StringToHash("End");

	private void Awake()
	{
		_operationInfos.Initialize();
	}

	private void OnDestroy()
	{
		_endClip = null;
	}

	public void Attack()
	{
		((Component)_operationInfos).gameObject.SetActive(true);
		_operationInfos.Run(_owner);
		Animator[] thorns = _thorns;
		for (int i = 0; i < thorns.Length; i++)
		{
			thorns[i].Play(attackHash, 0, 0f);
		}
	}

	public void Return()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		_operationInfos.Stop();
		Animator[] thorns = _thorns;
		for (int i = 0; i < thorns.Length; i++)
		{
			thorns[i].Play(returnHash);
		}
		PersistentSingleton<SoundManager>.Instance.PlaySound(_endSound, ((Component)this).transform.position);
		((MonoBehaviour)this).StartCoroutine(CReturn());
	}

	private IEnumerator CReturn()
	{
		yield return Chronometer.global.WaitForSeconds(_endClip.length);
		Animator[] thorns = _thorns;
		for (int i = 0; i < thorns.Length; i++)
		{
			((Component)thorns[i]).gameObject.SetActive(false);
		}
	}
}
