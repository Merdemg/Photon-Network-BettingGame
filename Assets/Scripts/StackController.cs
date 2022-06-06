using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackController : MonoBehaviour
{
    [SerializeField] ChipStackPooler chipStackPooler;
    [SerializeField] List<ChipStack> playerChipStacksList;

    Stack<ChipStack> playerChipStacks = new Stack<ChipStack>();
    Stack<ChipStack> betAreaChipStacks = new Stack<ChipStack>();

    [SerializeField] Transform playerStacksStartPos;
    [SerializeField] Transform betAreaStartPos;

    float gapBetweenStacks = 0.15f;

    private void Start() {
        foreach (var item in playerChipStacksList) {
            playerChipStacks.Push(item);
        }

        GameplayManager.Instance.OnPlayerBet += MoveChipsForPlayerBet;
        GameplayManager.Instance.OnRoundEnd += HandleRoundEnd;
    }

    private void OnDestroy() {
        GameplayManager.Instance.OnPlayerBet -= MoveChipsForPlayerBet;
        GameplayManager.Instance.OnRoundEnd += HandleRoundEnd;
    }

    public void MoveChipsForPlayerBet(int betAmount) {
        int numOfStacks = betAmount / 10;

        for (int i = 0; i < numOfStacks; i++) {
            ChipStack stack = playerChipStacks.Pop();
            MoveStackToBetArea(stack);
        }
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


    void MoveStackToBetArea(ChipStack stackToMove) {
        int count = betAreaChipStacks.Count;
        int row = count % 3;
        int column = count / 3;

        Vector3 pos = betAreaStartPos.position + new Vector3( column * gapBetweenStacks, 0, -1f * gapBetweenStacks * row);
        stackToMove.transform.position = pos;

        betAreaChipStacks.Push(stackToMove);
    }

    void MoveStackToPlayerArea(ChipStack stackToMove) {
        int count = playerChipStacks.Count;
        int row = count % 3;
        int column = count / 3;

        Vector3 pos = playerStacksStartPos.position + new Vector3(column * gapBetweenStacks, 0, -1f * gapBetweenStacks * row);
        stackToMove.transform.position = pos;

        playerChipStacks.Push(stackToMove);
    }
}
