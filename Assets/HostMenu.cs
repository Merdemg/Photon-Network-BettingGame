using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HostMenu : MonoBehaviour
{
    [SerializeField] TextFadeOffController nameRequiredWarning = null;
    [SerializeField] TMP_InputField nameInputField = null;
    [SerializeField] GameObject hostingView = null;


    public void OnHostButton() {
        if (string.IsNullOrWhiteSpace(nameInputField.text)) {
            nameRequiredWarning.ShowTextThenFadeOff();
            return;
        }
        else {
            nameRequiredWarning.FadeOffImmediate();
        }


        PhotonManager.Instance.StartHosting(nameInputField.text);
        hostingView.SetActive(true);
    }

    public void OnBackButton() {
        hostingView.SetActive(false);
        PhotonManager.Instance.LeaveRoomIfInRoom();
    }
}
