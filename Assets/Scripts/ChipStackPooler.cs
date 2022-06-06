using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipStackPooler : MonoBehaviour
{
    [SerializeField] ChipStack chipStackPrefab;

    Queue<ChipStack> chipStackPool = new Queue<ChipStack>();


    public ChipStack GetAStack(Transform parent) {
        ChipStack stack;
        if (chipStackPool.Count > 0) {
            stack = chipStackPool.Dequeue();
            stack.transform.parent = parent;
            stack.gameObject.SetActive(true);
        }
        else {
            stack = GameObject.Instantiate(chipStackPrefab, parent);
        }
        return stack;
    }

    public void ReturnToPool(ChipStack stack) {
        stack.transform.parent = transform;
        stack.gameObject.SetActive(false);
        chipStackPool.Enqueue(stack);
    }
}
