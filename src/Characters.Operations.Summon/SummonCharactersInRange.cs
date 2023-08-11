using Characters.Operations.Summon.SummonInRange.Policy;
using Level;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.Summon;

public class SummonCharactersInRange : CharacterOperation
{
	[SerializeField]
	private Character[] _characters;

	[SerializeField]
	private float _summonRange = 8f;

	[SerializeReference]
	[SubclassSelector]
	[Header("소환위치 세팅")]
	private ISummonPosition _positionPolicy;

	[SerializeReference]
	[SubclassSelector]
	[Header("소환체 세팅")]
	private IBDCharacterSetting[] _settings;

	private RayCaster _rayCaster;

	private void Awake()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		ContactFilter2D contactFilter = default(ContactFilter2D);
		((ContactFilter2D)(ref contactFilter)).SetLayerMask(LayerMask.op_Implicit(256));
		_rayCaster = new RayCaster
		{
			contactFilter = contactFilter,
			origin = Vector2.op_Implicit(((Component)this).transform.position),
			distance = _summonRange / 2f
		};
	}

	public override void Run(Character owner)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		if (_characters.Length == 0)
		{
			return;
		}
		((Caster)_rayCaster).direction = Vector2.left;
		RaycastHit2D val = ((Caster)_rayCaster).SingleCast();
		float num = 0f;
		num = ((!RaycastHit2D.op_Implicit(val)) ? (num + _summonRange / 2f) : (num + ((RaycastHit2D)(ref val)).distance));
		((Caster)_rayCaster).direction = Vector2.right;
		val = ((Caster)_rayCaster).SingleCast();
		float num2 = 0f;
		num2 = ((!RaycastHit2D.op_Implicit(val)) ? (num2 + _summonRange / 2f) : (num2 + ((RaycastHit2D)(ref val)).distance));
		Vector2 originPosition = Vector2.op_Implicit(((Component)owner).transform.position);
		originPosition.x += (num2 - num) / 2f;
		for (int i = 0; i < _characters.Length; i++)
		{
			Vector2 position = _positionPolicy.GetPosition(originPosition, num + num2, _characters.Length, i);
			Character character = Object.Instantiate<Character>(_characters[i], Vector2.op_Implicit(position), Quaternion.identity, ((Component)Map.Instance).transform);
			IBDCharacterSetting[] settings = _settings;
			for (int j = 0; j < settings.Length; j++)
			{
				settings[j].ApplyTo(character);
			}
		}
	}
}
