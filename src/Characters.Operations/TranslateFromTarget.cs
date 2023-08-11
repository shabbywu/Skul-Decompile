using Characters.AI;
using UnityEngine;

namespace Characters.Operations;

public class TranslateFromTarget : CharacterOperation
{
	[SerializeField]
	private Transform _transform;

	[SerializeField]
	private AIController _aIController;

	[SerializeField]
	[Range(0f, 10f)]
	private float _offsetY;

	[Range(0f, 10f)]
	[SerializeField]
	private float _distributionX;

	public override void Run(Character owner)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		Vector3 position = ((Component)_aIController.target).transform.position;
		bool flag = ((Component)owner).transform.position.x < position.x;
		RaycastHit2D val = Physics2D.Raycast(Vector2.op_Implicit(((Component)this).transform.position), flag ? Vector2.left : Vector2.right, 7f, 256);
		Vector3 zero = Vector3.zero;
		if (!RaycastHit2D.op_Implicit(val))
		{
			((Vector3)(ref zero))._002Ector(flag ? (position.x - _distributionX) : (position.x + _distributionX), position.y - _offsetY, 0f);
		}
		else
		{
			RaycastHit2D val2 = Physics2D.Raycast(Vector2.op_Implicit(((Component)this).transform.position), (!flag) ? Vector2.left : Vector2.right, 7f, 256);
			if (!RaycastHit2D.op_Implicit(val2))
			{
				((Vector3)(ref zero))._002Ector((!flag) ? (position.x - _distributionX) : (position.x + _distributionX), position.y - _offsetY, 0f);
			}
			else if (((RaycastHit2D)(ref val)).distance > ((RaycastHit2D)(ref val2)).distance)
			{
				((Vector3)(ref zero))._002Ector(flag ? (position.x - ((RaycastHit2D)(ref val)).distance) : (position.x + ((RaycastHit2D)(ref val)).distance), position.y - _offsetY, 0f);
			}
			else
			{
				((Vector3)(ref zero))._002Ector(flag ? (position.x - ((RaycastHit2D)(ref val2)).distance) : (position.x + ((RaycastHit2D)(ref val2)).distance), position.y - _offsetY, 0f);
			}
		}
		_transform.position = zero;
		if (position.x > _transform.position.x)
		{
			_transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else
		{
			_transform.localScale = new Vector3(-1f, 1f, 1f);
		}
	}
}
