using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions;

public class PowerbombChainAction : Action
{
	[SerializeField]
	[Tooltip("이 옵션을 활성화하면 땅에서 사용할 경우 즉시 landing motion이 발동됩니다.")]
	private bool _doLandingMotionIfGrounded;

	[SerializeField]
	[Tooltip("이 시간이 지난 후부터 땅에 있는 지 검사하여 땅에 있을 경우 강제로 landing motion을 실행시킵니다. 넉백이나 프레임 드랍 등으로 인해 landing motion이 실행되지 않는 경우를 방지하기 위해서이며, motion에 의해 캐릭터가 땅에서 떨어져 공중으로 뜨기 위한 시간 정도로 짧게 주는 것이 좋습니다. 보통 기본값인 0.1초로 충분합니다.")]
	private float _motionTimeout = 0.1f;

	[Subcomponent(typeof(Motion))]
	[SerializeField]
	protected Motion.Subcomponents _motions;

	[Subcomponent(typeof(Motion))]
	[SerializeField]
	protected Motion.Subcomponents _landingMotions;

	private Character.LookingDirection _lookingDirection;

	public override Motion[] motions => ((SubcomponentArray<Motion>)_motions).components.Concat(((SubcomponentArray<Motion>)_landingMotions).components).ToArray();

	public override bool canUse
	{
		get
		{
			if (base.cooldown.canUse && !_owner.stunedOrFreezed)
			{
				return PassAllConstraints(motions[0]);
			}
			return false;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		bool blockLookBefore = false;
		JoinMotion(_motions);
		JoinMotion(_landingMotions);
		if (((SubcomponentArray<Motion>)_motions).components.Length != 0)
		{
			Motion obj = ((SubcomponentArray<Motion>)_motions).components[((SubcomponentArray<Motion>)_motions).components.Length - 1];
			obj.onStart += delegate
			{
				base.owner.movement.onGrounded += OnGrounded;
			};
			obj.onEnd += delegate
			{
				base.owner.movement.onGrounded -= OnGrounded;
			};
			obj.onCancel += delegate
			{
				base.owner.movement.onGrounded -= OnGrounded;
			};
		}
		void JoinMotion(Motion.Subcomponents subcomponents)
		{
			for (int i = 0; i < ((SubcomponentArray<Motion>)subcomponents).components.Length; i++)
			{
				Motion motion = ((SubcomponentArray<Motion>)subcomponents).components[i];
				if (motion.blockLook)
				{
					if (blockLookBefore)
					{
						motion.onStart += delegate
						{
							base.owner.lookingDirection = _lookingDirection;
						};
					}
					motion.onStart += delegate
					{
						_lookingDirection = base.owner.lookingDirection;
					};
				}
				blockLookBefore = motion.blockLook;
				if (i + 1 < ((SubcomponentArray<Motion>)subcomponents).components.Length)
				{
					int cached = i + 1;
					((SubcomponentArray<Motion>)subcomponents).components[i].onEnd += delegate
					{
						DoMotion(((SubcomponentArray<Motion>)subcomponents).components[cached]);
					};
				}
			}
		}
	}

	private void OnDisable()
	{
		base.owner.movement.onGrounded -= OnGrounded;
	}

	private void OnGrounded()
	{
		((MonoBehaviour)this).StopAllCoroutines();
		DoMotion(((SubcomponentArray<Motion>)_landingMotions).components[0]);
	}

	public override void Initialize(Character owner)
	{
		base.Initialize(owner);
		for (int i = 0; i < motions.Length; i++)
		{
			motions[i].Initialize(this);
		}
	}

	public override bool TryStart()
	{
		if (!((Component)this).gameObject.activeSelf || !canUse || !ConsumeCooldownIfNeeded())
		{
			return false;
		}
		if (base.owner.movement.isGrounded && _doLandingMotionIfGrounded)
		{
			_lookingDirection = base.owner.lookingDirection;
			DoAction(((SubcomponentArray<Motion>)_landingMotions).components[0]);
		}
		else
		{
			DoAction(((SubcomponentArray<Motion>)_motions).components[0]);
			((MonoBehaviour)this).StopAllCoroutines();
			((MonoBehaviour)this).StartCoroutine(CExtraGroundCheck());
		}
		return true;
	}

	private IEnumerator CExtraGroundCheck()
	{
		float speedMultiplier = GetSpeedMultiplier(((SubcomponentArray<Motion>)_motions).components[0]);
		yield return Chronometer.global.WaitForSeconds(_motionTimeout / speedMultiplier);
		while (((SubcomponentArray<Motion>)_motions).components.Any((Motion m) => m.running))
		{
			if (base.owner.movement.isGrounded)
			{
				DoMotion(((SubcomponentArray<Motion>)_landingMotions).components[0]);
				break;
			}
			yield return null;
		}
	}
}
