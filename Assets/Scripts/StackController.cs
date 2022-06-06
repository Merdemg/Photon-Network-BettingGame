using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackController : ChipStackMover
{
    [SerializeField] ChipStackPooler chipStackPooler;

    new void Start() {
        base.Start();
        GameplayManager.Instance.OnPlayerBet += MoveChipsForPlayerBet;
        GameplayManager.Instance.OnRoundEnd += HandleRoundEnd;
    }

    private void OnDestroy() {
        GameplayManager.Instance.OnPlayerBet -= MoveChipsForPlayerBet;
        GameplayManager.Instance.OnRoundEnd += HandleRoundEnd;
    }

    public void HandleRoundEnd(bool isWin, int totalChips, bool isToppingPlayerUp) {
        if (isWin) {
            MoveChipsForPlayerWin();
        }
        else {
            MoveChipsForPlayerLose();
        }

        if (isToppingPlayerUp) {
            TopPlayerUp(totalChips);
        }
    }

    void MoveChipsForPlayerWin() {
        int betStackCount = betAreaChipStacks.Count;

        for (int i = 0; i < betStackCount; i++) {
            ChipStack stack = betAreaChipStacks.Pop();
            MoveStackToPlayerArea(stack);
        }

        for (int i = 0; i < betStackCount; i++) {
            ChipStack stack = chipStackPooler.GetAStack(transform);
            MoveStackToPlayerArea(stack);
        }
    }

    void MoveChipsForPlayerLose() {
        int count = betAreaChipStacks.Count;
        for (int i = 0; i < count; i++) {
            ChipStack stack = betAreaChipStacks.Pop();
            chipStackPooler.ReturnToPool(stack);
        }
    }

    void TopPlayerUp(int totalChips) {
        while (playerChipStacks.Count < totalChips / 10) {
            ChipStack stack = chipStackPooler.GetAStack(transform);
            MoveStackToPlayerArea(stack);
        }
    }
}
