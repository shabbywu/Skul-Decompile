using Characters.Operations;
using Level;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Chimera;

public class ChimeraWreck : MonoBehaviour
{
	[SerializeField]
	private ParticleEffectInfo _particle;

	[SerializeField]
	private Transform _emitPosition;

	[SerializeField]
	private Collider2D _range;

	[SerializeField]
	[Subcomponent(typeof(OperationInfos))]
	private OperationInfos _operationInfos;

	private void Awake()
	{
		_operationInfos.Initialize();
	}

	private void Update()
	{
		if (Map.Instance.waveContainer.enemyWaves[0].remains <= 0)
		{
			DestroyProp(Singleton<Service>.Instance.levelManager.player);
		}
	}

	public void DestroyProp(Character chimera)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		_particle.Emit(Vector2.op_Implicit(_emitPosition.position), _range.bounds, Vector2.up * 2f);
		_operationInfos.Run(chimera);
		Object.Destroy((Object)(object)((Component)this).gameObject);
	}
}
