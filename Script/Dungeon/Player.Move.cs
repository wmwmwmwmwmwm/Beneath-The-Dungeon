using DG.Tweening;
using UnityEngine;
using static SingletonLoader;

public partial class Player : Singleton<Player>
{
	public bool UsePendingMove;
	Vector2Int PendingMoveDirection;
	public void Move(Vector2Int Direction)
	{
		if (IsMoving)
		{
			if (UsePendingMove) PendingMoveDirection = Direction;
			return;
		}
		if (Direction == Vector2Int.up && CanMoveForward())
		{
			MoveAnimation(LookAt);
		}
		else if (Direction == Vector2Int.down && CanMoveBackward())
		{
			MoveAnimation(-LookAt);
		}
		else if (Direction == Vector2Int.left)
		{
			am.PlaySfx(AudioManager.SfxTypeEnum.Walk);
			IsMoving = true;
			transform.DORotate(transform.eulerAngles + new Vector3(0f, -90f, 0f), 0.2f).OnComplete(() => OnMoveComplete(false));
			SetRotation(GridHelper.TurnLeft(LookAt), false);
		}
		else if (Direction == Vector2Int.right)
		{
			am.PlaySfx(AudioManager.SfxTypeEnum.Walk);
			IsMoving = true;
			transform.DORotate(transform.eulerAngles + new Vector3(0f, 90f, 0f), 0.2f).OnComplete(() => OnMoveComplete(false));
			SetRotation(GridHelper.TurnRight(LookAt), false);
		}
		void MoveAnimation(Vector2Int Direction)
		{
			IsMoving = true;
			DungeonController.Instance.OpenDoor(GridPosition, Direction);
			GridPosition += Direction;
			DungeonController.Instance.RevealTile(GridPosition);
			Vector3 MoveVector = DungeonController.Instance.Map[GridPosition].transform.position;
			MoveVector.y += DungeonController.Instance.CurrentDungeonData.PlayerHeight;
			MoveVector -= transform.localPosition;
			float AnimationDuration = 0.3f;
			Tweener MoveTween = transform.DOBlendableLocalMoveBy(MoveVector, AnimationDuration).SetEase(Ease.Linear).OnComplete(() => OnMoveComplete(true));
			bool Footstep1 = false, Footstep2 = false;
			MoveTween.OnUpdate(() =>
			{
				if (MoveTween.position / AnimationDuration > 0.25f && !Footstep1)
				{
					am.PlaySfx(AudioManager.SfxTypeEnum.Walk);
					Footstep1 = true;
				}
				else if (MoveTween.position / AnimationDuration > 0.75f && !Footstep2)
				{
					am.PlaySfx(AudioManager.SfxTypeEnum.Walk);
					Footstep2 = true;
				}
			});
			Sequence MoveYSequence = DOTween.Sequence();
			MoveYSequence.Append(transform.DOBlendableLocalMoveBy(Vector3.up * 0.03f, 0.21f));
			MoveYSequence.Append(transform.DOBlendableLocalMoveBy(Vector3.down * 0.03f, 0.09f));
			UICanvas.Instance.UpdateMinimapPosition();
		}
	}

	public bool CanMoveForward() => !DungeonController.Instance.Map[GridPosition].WorldWalls[GridHelper.DirectionToIndex(LookAt)];
	public bool CanMoveBackward() => !DungeonController.Instance.Map[GridPosition].WorldWalls[GridHelper.DirectionToIndex(-LookAt)];

	void OnMoveComplete(bool IsForward)
	{
		IsMoving = false;
		if (IsForward)
		{
			DungeonController.Instance.OnPlayerTurnComplete(true);
		}
		if (PendingMoveDirection != Vector2Int.zero)
		{
			CommonUI.Instance.CloseAllAlert();
			Move(PendingMoveDirection);
			PendingMoveDirection = Vector2Int.zero;
		}
	}
}
