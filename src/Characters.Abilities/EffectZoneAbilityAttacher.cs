using System.Collections;
using System.Linq;
using Characters.Usables;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Abilities;

public class EffectZoneAbilityAttacher : AbilityAttacher
{
	private static readonly NonAllocOverlapper _sharedOverlapper = new NonAllocOverlapper(99);

	[SerializeField]
	[Header("Finder")]
	[FrameTime]
	private float _checkInterval = 0.33f;

	[SerializeField]
	private Collider2D _collider;

	[SerializeField]
	private TargetLayer _layer = new TargetLayer(LayerMask.op_Implicit(0), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);

	[SerializeField]
	private string[] _keys;

	[SerializeField]
	[Header("Abilities")]
	[AbilityComponent.Subcomponent]
	private AbilityComponent.Subcomponents _abilityComponents;

	private CoroutineReference _cCheckReference;

	private bool _attached;

	public override void OnIntialize()
	{
		_abilityComponents.Initialize();
	}

	public override void StartAttach()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		((CoroutineReference)(ref _cCheckReference)).Stop();
		_cCheckReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CCheck());
	}

	public override void StopAttach()
	{
		((CoroutineReference)(ref _cCheckReference)).Stop();
		if (!((Object)(object)base.owner == (Object)null))
		{
			AbilityComponent[] components = ((SubcomponentArray<AbilityComponent>)_abilityComponents).components;
			foreach (AbilityComponent abilityComponent in components)
			{
				base.owner.ability.Remove(abilityComponent.ability);
			}
		}
	}

	private IEnumerator CCheck()
	{
		while (true)
		{
			((Behaviour)_collider).enabled = true;
			((ContactFilter2D)(ref _sharedOverlapper.contactFilter)).SetLayerMask(_layer.Evaluate(((Component)base.owner).gameObject));
			if (_sharedOverlapper.OverlapCollider(_collider).GetComponents<EffectZone>(true).Count((EffectZone target) => target.MatchKey(_keys)) > 0)
			{
				if (!_attached)
				{
					for (int i = 0; i < ((SubcomponentArray<AbilityComponent>)_abilityComponents).components.Length; i++)
					{
						base.owner.ability.Add(((SubcomponentArray<AbilityComponent>)_abilityComponents).components[i].ability);
					}
					_attached = true;
				}
			}
			else
			{
				for (int j = 0; j < ((SubcomponentArray<AbilityComponent>)_abilityComponents).components.Length; j++)
				{
					base.owner.ability.Remove(((SubcomponentArray<AbilityComponent>)_abilityComponents).components[j].ability);
				}
				_attached = false;
			}
			yield return (object)new WaitForSecondsRealtime(_checkInterval);
		}
	}
}
