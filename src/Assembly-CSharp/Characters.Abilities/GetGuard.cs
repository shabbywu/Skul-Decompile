using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Characters.Abilities;

[Serializable]
public sealed class GetGuard : Ability
{
	public sealed class Instance : AbilityInstance<GetGuard>
	{
		private Guard _guardInstance;

		public Instance(Character owner, GetGuard ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			_guardInstance = Guard.Create(ability._guardReference);
			_guardInstance.Initialize(owner);
			((Component)_guardInstance).transform.localPosition = Vector3.zero;
			_guardInstance.GuardUp();
		}

		protected override void OnDetach()
		{
			_guardInstance.GuardDown();
			Object.Destroy((Object)(object)_guardInstance);
			_guardInstance = null;
		}
	}

	[SerializeField]
	private AssetReference _guardReference;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
