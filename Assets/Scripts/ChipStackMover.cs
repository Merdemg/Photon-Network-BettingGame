using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipStackMover : MonoBehaviour
{
    [SerializeField] protected List<ChipStack> playerChipStacksList;

    protected Stack<ChipStack> playerChipStacks = new Stack<ChipStack>();
    protected Stack<ChipStack> betAreaChipStacks = new Stack<ChipStack>();

    [SerializeField] protected Transform playerStacksStartPos;
    [SerializeField] protected Transform betAreaStartPos;

    float gapBetweenStacks = 0.15f;

    protected void Start() {
        foreach (var item in playerChipStacksList) {
            playerChipStacks.Push(item);
        }
    }

    public void MoveChipsForPlayerBet(int betAmount) {
        int numOfStacks = betAmount / 10;

        for (int i = 0; i < numOfStacks; i++) {
            ChipStack stack = playerChipStacks.Pop();
            MoveStackToBetArea(stack);
        }
    }

    protected void MoveStackToBetArea(ChipStack stackToMove) {
        int count = betAreaChipStacks.Count;
        int row = count % 3;
        int column = count / 3;

        Vector3 pos = betAreaStartPos.position + new Vector3(column * gapBetweenStacks, 0, -1f * gapBetweenStacks * row);
        stackToMove.transform.position = pos;

        betAreaChipStacks.Push(stackToMove);
    }

    protected void MoveStackToPlayerArea(ChipStack stackToMove) {
        int count = playerChipStacks.Count;
        int row = count % 3;
        int column = count / 3;

        Vector3 pos = playerStacksStartPos.position + new Vector3(column * gapBetweenStacks, 0, -1f * gapBetweenStacks * row);
        stackToMove.transform.position = pos;

        playerChipStacks.Push(stackToMove);
    }
}
