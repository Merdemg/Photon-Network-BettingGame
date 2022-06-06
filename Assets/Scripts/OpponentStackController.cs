using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentStackController : ChipStackMover
{
    [SerializeField] ChipStackPooler chipStackPooler;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        GameplayManager.Instance.OnOpponentUpdate += OnOpponentUpdate;
    }

    private void OnDestroy() {
        GameplayManager.Instance.OnOpponentUpdate -= OnOpponentUpdate;
    }

    private void OnOpponentUpdate(int betAmount, int leftChips, bool isGreen) {
        if (betAmount > 0) { // Opponent made a bet
            MoveChipsForPlayerBet(betAmount);
        }
        else {
            while ((leftChips > playerChipStacks.Count * 10)) { // Opponent gets chips, if needed

                if (betAreaChipStacks.Count > 0) {
                    ChipStack stack = betAreaChipStacks.Pop();
                    MoveStackToPlayerArea(stack);
                }
                else {
                    ChipStack stack = chipStackPooler.GetAStack(transform);
                    MoveStackToPlayerArea(stack);
                }
            }

            int count = betAreaChipStacks.Count; // Clean up and left over chips in bet area
            for (int i = 0; i < count; i++) {
                ChipStack stack = betAreaChipStacks.Pop();
                chipStackPooler.ReturnToPool(stack);
            }
        }
    }
}
