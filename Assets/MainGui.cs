using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGui : MonoBehaviour
{
    private Canvas canvas;
    private RectTransform fillRect;
    private int maxFillHeight = 54; // Not sure how to make this dynamic.
    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        fillRect = GameObject.Find("Fill").GetComponent<RectTransform>();
    }
    public void SetScoreGauge(float percentage)
    {
        fillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Lerp(0, maxFillHeight, percentage));
       
    }
    // Update is called once per frame
    void Update()
    {
        
        
    }
}
