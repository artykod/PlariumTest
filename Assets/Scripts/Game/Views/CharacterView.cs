using Game.Logics;
using Game.Logics.Characters;
using UnityEngine;

public class CharacterView : UnitView {
	private static readonly int ANIM_TRIGGER_IDLE = Animator.StringToHash("idle");
	private static readonly int ANIM_TRIGGER_RUN = Animator.StringToHash("run");
	private static readonly int ANIM_TRIGGER_ATTACK = Animator.StringToHash("attack");

	private Animator animator;
	private bool lastCanMove;
	private Character character;

	protected override float RotationLerpSpeed
	{
		get
		{
			return 0.1f;
		}
	}

	protected override void Awake()
	{
		base.Awake();

		animator = GetComponentInChildren<Animator>();
	}

	public override void FetchLogic(Logic logic)
	{
		base.FetchLogic(logic);

		character = logic as Character;
		character.OnAttack += OnAttack;

		PlayAnimation(ANIM_TRIGGER_IDLE);
	}

	protected override void OnLogicDestroy(Logic logic)
	{
		base.OnLogicDestroy(logic);

		(logic as Character).OnAttack -= OnAttack;
	}

	private void OnAttack(Character self, Unit attackedUnit)
	{
		if (self.Descriptor.IsRangedAttack)
		{
			var shell = PrefabTool.CreateInstance<Shell>("Arrow");
			shell.Fire(new Vector3(self.Position.x, 1f, self.Position.y), new Vector3(attackedUnit.Position.x, 0f, attackedUnit.Position.y), 0.25f);
		}

		PlayAnimation(ANIM_TRIGGER_ATTACK);
	}

	protected override void Update()
	{
		base.Update();

		var canMove = character.CanMove && Core.Instance.GameController.IsBattleStarted;
		if (lastCanMove != canMove)
		{
			lastCanMove = canMove;
			PlayAnimation(lastCanMove ? ANIM_TRIGGER_RUN : ANIM_TRIGGER_IDLE);
		}
	}

	private void PlayAnimation(int trigger)
	{
		if (animator != null && animator.isInitialized)
		{
			animator.ResetTrigger(ANIM_TRIGGER_ATTACK);
			animator.ResetTrigger(ANIM_TRIGGER_IDLE);
			animator.ResetTrigger(ANIM_TRIGGER_RUN);
			animator.SetTrigger(trigger);
		}
	}
}
