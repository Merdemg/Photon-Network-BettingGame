using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BettingSidePanelController : MonoBehaviour
{
    [SerializeField] TMP_Text chipCountText = null;
    [SerializeField] TMP_Text betAmountText = null;
    [SerializeField] Toggle isGreenToggle = null;
    [SerializeField] Button betButton = null;


    int chipCount = 100;
    int betAmount = 10;

    const int betIncrement = 10;

    private void Start() {
        GameplayManager.Instance.OnRoundEnd += OnRoundEnd;
    }

    private void OnRoundEnd(bool isWin, int newTotalChips) {
        if (newTotalChips == 0) { // Top player off
            newTotalChips = 1000;
        }

        UpdateUI(newTotalChips);
        betButton.interactable = true;
    }

    public void UpdateUI(int newChipCount) {
        chipCount = newChipCount;
        chipCountText.text = chipCount.ToString();
        betAmount = 10;
        betAmountText.text = betAmount.ToString();
    }

    public void ResetUI() {

    }


    public void OnIncreaseBet() {
        if (betAmount + betIncrement <= chipCount) {
            betAmount += betIncrement;
            betAmountText.text = betAmount.ToString();
        }
    }

    public void OnDecreaseBet() {
        if (betAmount - betIncrement > 0) {
            betAmount -= betIncrement;
            betAmountText.text = betAmount.ToString();
        }
    }

    public void OnBet() {
        betButton.interactable = false;
        chipCount -= betAmount;
        chipCountText.text = chipCount.ToString();
        GameplayManager.Instance.OnBetSelected(isGreenToggle.isOn, betAmount, chipCount);
    }
}
