using System.Collections.Generic;
using Hardmode;
using Level;
using Singletons;
using UnityEngine;

namespace Characters.Projectiles.Operations;

public class SpawnObject : HitOperation
{
	[SerializeField]
	private GameObject[] _objectsInHardmode;

	[SerializeField]
	private GameObject[] _objects;

	[SerializeField]
	private GameObject _object;

	[SerializeField]
	private float _lifeTime;

	public override void Run(IProjectile projectile, RaycastHit2D raycastHit)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		if (Singleton<HardmodeManager>.Instance.hardmode)
		{
			if (_objectsInHardmode != null && _objectsInHardmode.Length != 0)
			{
				_object = ExtensionMethods.Random<GameObject>((IEnumerable<GameObject>)_objectsInHardmode);
			}
		}
		else if (_objects != null && _objects.Length != 0)
		{
			_object = ExtensionMethods.Random<GameObject>((IEnumerable<GameObject>)_objects);
		}
		GameObject val = Object.Instantiate<GameObject>(_object, Vector2.op_Implicit(((RaycastHit2D)(ref raycastHit)).point), Quaternion.identity, ((Component)Map.Instance).transform);
		Character component = val.GetComponent<Character>();
		if ((Object)(object)component != (Object)null)
		{
			Map.Instance.waveContainer.Attach(component);
		}
		if (_lifeTime != 0f)
		{
			Object.Destroy((Object)(object)val, _lifeTime);
		}
	}
}
