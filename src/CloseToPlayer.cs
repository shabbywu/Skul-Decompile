using Characters;
using Services;
using Singletons;
using UnityEngine;

public class CloseToPlayer : MonoBehaviour
{
	[SerializeField]
	private Character _owner;

	[SerializeField]
	private float _speed;

	private Transform _player;

	private void Start()
	{
		_player = ((Component)Singleton<Service>.Instance.levelManager.player).transform;
	}

	private void Update()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		float num = Mathf.Sign(_player.position.x - ((Component)this).transform.position.x);
		((Component)this).transform.Translate(Vector2.op_Implicit(new Vector2(num * ((ChronometerBase)_owner.chronometer.master).deltaTime * _speed, 0f)));
	}
}
