using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Customs;

public class FlipByPlayerPosition : Operation
{
	[SerializeField]
	private Transform _body;

	public override void Run()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		Transform transform = ((Component)Singleton<Service>.Instance.levelManager.player).transform;
		if (_body.position.x < transform.position.x)
		{
			_body.localScale = Vector2.op_Implicit(new Vector2(1f, 1f));
		}
		else
		{
			_body.localScale = Vector2.op_Implicit(new Vector2(-1f, 1f));
		}
	}
}
