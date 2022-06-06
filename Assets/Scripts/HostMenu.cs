using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HostMenu : MonoBehaviour
{
    [SerializeField] TextFadeOffController nameRequiredWarning = null;
    [SerializeField] TMP_InputField nameInputField = null;

    public void OnHostButton() {
        if (string.IsNullOrWhiteSpace(nameInputField.text)) {
            nameRequiredWarning.ShowTextThenFadeOff();
            return;
        }
        else {
            nameRequiredWarning.FadeOffImmediate();
        }

        PhotonManager.Instance.StartHosting(nameInputField.text);
    }
}
