using Characters.Actions;
using Characters.Operations;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public sealed class Spoils : InscriptionInstance
{
	[Header("2세트 효과")]
	[SerializeField]
	private OperationInfos _flagOperations;

	protected override void Initialize()
	{
		_flagOperations.Initialize();
		((Component)_flagOperations).gameObject.SetActive(false);
	}

	public override void Attach()
	{
		Singleton<Service>.Instance.levelManager.onMapLoaded += Disable;
		base.character.onStartAction += OnStartAction;
	}

	private void Disable()
	{
		((Component)_flagOperations).gameObject.SetActive(false);
	}

	public override void Detach()
	{
		Disable();
		Singleton<Service>.Instance.levelManager.onMapLoaded -= Disable;
		base.character.onStartAction -= OnStartAction;
	}

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
	}

	private void OnStartAction(Action action)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		if (action.type == Action.Type.Swap && keyword.step >= 1)
		{
			((Component)_flagOperations).gameObject.SetActive(false);
			((Component)_flagOperations).gameObject.SetActive(true);
			Vector3 val = ((Component)this).transform.position;
			RaycastHit2D val2 = Physics2D.Raycast(Vector2.op_Implicit(val), Vector2.down, 12f, LayerMask.op_Implicit(Layers.groundMask));
			if (RaycastHit2D.op_Implicit(val2))
			{
				val = Vector2.op_Implicit(((RaycastHit2D)(ref val2)).point);
			}
			((Component)_flagOperations).transform.position = val;
			_flagOperations.Run(base.character);
		}
	}
}
