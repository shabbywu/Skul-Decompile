using Characters;
using UnityEngine;

namespace Level.Objects.DecorationCharacter;

public class DecorationCharacter : MonoBehaviour
{
	public readonly CharacterChronometer chronometer = new CharacterChronometer();

	[SerializeField]
	[GetComponent]
	private DecorationCharacterAnimationController _animationController;

	[SerializeField]
	private BoxCollider2D _collider;

	[SerializeField]
	private GameObject _attach;

	[SerializeField]
	private float _speed = 5f;

	[SerializeField]
	private SpriteRenderer _renderer;

	private Character.LookingDirection _lookingDirection;

	public float speed => _speed;

	public DecorationCharacterAnimationController animationController => _animationController;

	public float deltaTime => ChronometerExtension.DeltaTime((ChronometerBase)(object)chronometer.animation);

	public Character.LookingDirection lookingDirection
	{
		get
		{
			return _lookingDirection;
		}
		set
		{
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			desiringLookingDirection = value;
			_lookingDirection = value;
			if (_lookingDirection == Character.LookingDirection.Right)
			{
				_animationController.parameter.flipX = false;
				attachWithFlip.transform.localScale = Vector3.one;
			}
			else
			{
				_animationController.parameter.flipX = true;
				attachWithFlip.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
		}
	}

	public Character.LookingDirection desiringLookingDirection { get; private set; }

	public BoxCollider2D collider => _collider;

	public GameObject attachWithFlip { get; private set; }

	public void SetRenderSortingOrder(int order)
	{
		((Renderer)_renderer).sortingOrder = order;
	}

	private void Awake()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		_animationController.Initialize(chronometer);
		if ((Object)(object)_attach == (Object)null)
		{
			_attach = new GameObject("_attach");
			_attach.transform.SetParent(((Component)this).transform, false);
		}
		if ((Object)(object)attachWithFlip == (Object)null)
		{
			attachWithFlip = new GameObject("attachWithFlip");
			attachWithFlip.transform.SetParent(_attach.transform, false);
		}
	}
}
