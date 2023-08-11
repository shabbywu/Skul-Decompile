using UnityEngine;

namespace Characters.Projectiles;

public class Rotate : MonoBehaviour
{
	[SerializeField]
	private Projectile _projectile;

	[SerializeField]
	private BouncyProjectile _bouncyProjectile;

	[SerializeField]
	private float _amount;

	private IProjectile _iprojectile;

	private void Awake()
	{
		IProjectile iprojectile;
		if (!((Object)(object)_projectile == (Object)null))
		{
			IProjectile projectile = _projectile;
			iprojectile = projectile;
		}
		else
		{
			IProjectile projectile = _bouncyProjectile;
			iprojectile = projectile;
		}
		_iprojectile = iprojectile;
	}

	private void Update()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		float num = _amount * ((ChronometerBase)_iprojectile.owner.chronometer.projectile).deltaTime;
		num *= Mathf.Sign(_iprojectile.transform.localScale.x);
		((Component)this).transform.Rotate(Vector3.forward, num, (Space)1);
	}
}
