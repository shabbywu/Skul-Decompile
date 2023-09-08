using UnityEngine;

namespace Level;

public class ThiefGold : MonoBehaviour
{
	public delegate void OnDespawn(double goldAmount, Vector3 position);

	[SerializeField]
	private CurrencyParticle _goldParticle;

	public static event OnDespawn onDespawn;

	private void OnDisable()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		ThiefGold.onDespawn?.Invoke(_goldParticle.currencyAmount, ((Component)this).transform.position);
	}
}
