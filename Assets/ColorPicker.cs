using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPicker : MonoBehaviour
{
    [SerializeField] Material greenMat = null;
    [SerializeField] Material redMat = null;

    Renderer spinnerRenderer;

    const float SPIN_LENGTH = 3f;
    const float COLOR_SWITCH_FREQ = 0.35f;

    // Start is called before the first frame update
    void Start()
    {
        spinnerRenderer = GetComponent<Renderer>();

        GameplayManager.Instance.OnNewSpin += OnNewSpin;   
    }

    private void OnDestroy() {
        GameplayManager.Instance.OnNewSpin -= OnNewSpin;
    }

    private void OnNewSpin(bool isGreen) {
        StartCoroutine(SpinCoroutine(isGreen));
    }

    IEnumerator SpinCoroutine(bool isGreen) {
        float counter = 0;

        bool isCurrentlyGreen = Random.value > 0.5f;

        float currentSwitchFreq = COLOR_SWITCH_FREQ;

        while (counter < SPIN_LENGTH) {
            spinnerRenderer.material = isCurrentlyGreen ? greenMat : redMat;
            isCurrentlyGreen = !isCurrentlyGreen;
            yield return new WaitForSeconds(currentSwitchFreq);
            counter += currentSwitchFreq;
            currentSwitchFreq *= 0.95f;
        }

        spinnerRenderer.material = isGreen ? greenMat : redMat;

        GameplayManager.Instance.OnSpinOver();
    }
}
