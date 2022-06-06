using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipStack : MonoBehaviour
{
    [SerializeField] List<Color> possibleColors;
    [SerializeField] List<Renderer> renderers;



    // Start is called before the first frame update
    void Start()
    {
        PickAColor();
    }

    void PickAColor() {
        Color color = possibleColors[Random.Range(0, possibleColors.Count)];

        foreach (var renderer in renderers) {
            renderer.material.color = color;
        }
    }
}
