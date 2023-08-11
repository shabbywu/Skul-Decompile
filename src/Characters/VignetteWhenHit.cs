using Characters.Operations.Fx;
using UnityEditor;
using UnityEngine;

namespace Characters;

public class VignetteWhenHit : MonoBehaviour
{
	[SerializeField]
	[GetComponent]
	private Character _owner;

	[Subcomponent(typeof(Vignette))]
	[SerializeField]
	private Vignette _vignette;

	private void Awake()
	{
		_owner.health.onTookDamage += onTookDamage;
	}

	private void onTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		if (damageDealt > 0.0 && tookDamage.attackType != Damage.AttackType.Additional)
		{
			_vignette.Run(_owner);
		}
	}
}
