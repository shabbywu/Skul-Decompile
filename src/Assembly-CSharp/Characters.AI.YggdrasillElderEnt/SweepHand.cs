using Characters.Operations;
using Hardmode;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.YggdrasillElderEnt;

public class SweepHand : MonoBehaviour
{
	[SerializeField]
	private Character _owner;

	[GetComponent]
	[SerializeField]
	private Collider2D _collider;

	[SerializeField]
	private GameObject _monterBody;

	[SerializeField]
	private GameObject _effects;

	[SerializeField]
	private YggdrasillElderEntCollisionDetector _collisionDetector;

	[SerializeField]
	[Subcomponent(typeof(OperationInfos))]
	private OperationInfos _onAttackInHardmode;

	private void Awake()
	{
		if (Singleton<HardmodeManager>.Instance.hardmode)
		{
			_onAttackInHardmode.Initialize();
		}
	}

	public void Attack()
	{
		_collisionDetector.Initialize(_monterBody, _collider);
		((MonoBehaviour)this).StartCoroutine(_collisionDetector.CRun(((Component)this).transform));
		if (Singleton<HardmodeManager>.Instance.hardmode)
		{
			((Component)_onAttackInHardmode).gameObject.SetActive(true);
			_onAttackInHardmode.Run(_owner);
		}
		_effects.gameObject.SetActive(true);
	}

	public void Stop()
	{
		_collisionDetector.Stop();
		if (Singleton<HardmodeManager>.Instance.hardmode)
		{
			_onAttackInHardmode.Stop();
		}
		_effects.gameObject.SetActive(false);
	}
}
