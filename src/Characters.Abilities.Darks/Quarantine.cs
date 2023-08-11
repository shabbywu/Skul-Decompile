using System;
using System.Collections;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities.Darks;

[Serializable]
public sealed class Quarantine : Ability
{
	public sealed class Instance : AbilityInstance<Quarantine>
	{
		private float _reaminReadyTime;

		private bool _active;

		public Instance(Character owner, Quarantine ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			_reaminReadyTime = ability._readyTime;
			((MonoBehaviour)owner).StartCoroutine(CLoad());
		}

		protected override void OnDetach()
		{
			ability._quarantineObject.gameObject.SetActive(false);
			ability._operationInfos.Stop();
			ability.soundOnAttach.Dispose();
		}

		private IEnumerator CLoad()
		{
			yield return null;
			ability._operationInfos.Initialize();
			Activate();
		}

		private void Activate()
		{
			if (!_active)
			{
				ability._quarantineObject.SetActive(true);
				((Component)ability._operationInfos).gameObject.SetActive(true);
				if (((Component)ability._operationInfos).gameObject.activeSelf)
				{
					ability._operationInfos.Run(owner);
				}
				_active = true;
			}
		}

		private void Deactivate()
		{
			if (_active)
			{
				ability._quarantineObject.gameObject.SetActive(false);
				ability._operationInfos.Stop();
				_active = false;
			}
		}
	}

	[SerializeField]
	private GameObject _quarantineObject;

	[SerializeField]
	private OperationInfos _operationInfos;

	[SerializeField]
	private float _maxSpeed;

	[SerializeField]
	private float _readyTime;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
