using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions.FairyTaleSummon;

public sealed class OberonBombOrb : MonoBehaviour
{
	[SerializeField]
	private GameObject _body;

	[SerializeField]
	private float _noise;

	[Subcomponent(typeof(OperationInfos))]
	[SerializeField]
	private OperationInfos _activateInfo;

	[Subcomponent(typeof(OperationInfos))]
	[SerializeField]
	private OperationInfos _deactivateInfo;

	private Vector2 _originPoistion;

	private void Awake()
	{
		_activateInfo.Initialize();
		_deactivateInfo.Initialize();
	}

	public void Activate(Character owner)
	{
		Show();
		MoveRandom();
		((Component)_activateInfo).gameObject.SetActive(true);
		_activateInfo.Run(owner);
	}

	public void Deactivate(Character owner)
	{
		Hide();
		((Component)_deactivateInfo).gameObject.SetActive(true);
		_deactivateInfo.Run(owner);
		Restore();
	}

	private void MoveRandom()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		_originPoistion = Vector2.op_Implicit(((Component)this).transform.position);
		((Component)this).transform.Translate(Random.insideUnitSphere * _noise);
	}

	private void Restore()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.position = Vector2.op_Implicit(_originPoistion);
	}

	private void Show()
	{
		_body.SetActive(true);
	}

	private void Hide()
	{
		_body.SetActive(false);
	}
}
