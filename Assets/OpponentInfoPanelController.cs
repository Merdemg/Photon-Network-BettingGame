using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OpponentInfoPanelController : MonoBehaviour
{
    [SerializeField] TMP_Text opponentChipsText;
    [SerializeField] GameObject beforeBetTextHolder;
    [SerializeField] GameObject afterBetTextHolder;
    [SerializeField] TMP_Text opponentBetText;
    [SerializeField] Color greenTextColor, redTextColor;

    // Start is called before the first frame update
    void Start()
    {
        GameplayManager.Instance.OnOpponentUpdate += OnOpponentUpdate;
    }


    private void OnDestroy() {
        GameplayManager.Instance.OnOpponentUpdate -= OnOpponentUpdate;
    }

    private void OnOpponentUpdate(int betAmount, int leftChips, bool isGreen) {
        opponentChipsText.text = leftChips.ToString();
        opponentBetText.text = betAmount.ToString() + " for ";

        if (isGreen) {
            opponentBetText.text += "Green";
            opponentBetText.color = greenTextColor;
        }
        else {
            opponentBetText.text += "Red";
            opponentBetText.color = redTextColor;
        }

        if (betAmount == 0) {
            beforeBetTextHolder.SetActive(true);
            afterBetTextHolder.SetActive(false);
        }
        else {
            beforeBetTextHolder.SetActive(false);
            afterBetTextHolder.SetActive(true);
        }
    }
}
