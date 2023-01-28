using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalGague : MonoBehaviour
{
    private int targetMax;
    private int currentMax;
    private int targetValue;
    private int currentValue;

    // There has to be a less error prone way to accomplish this.
    private bool isAnimatingMax = false;
    private bool isAnimatingValue = false;

    IEnumerator animateValueChangeAsync()
    {
        if (!isAnimatingValue)
        {
            isAnimatingMax = true;
            while (currentValue != targetValue)
            {
                var diff = targetValue - currentValue;
                // This *should* ensure that we always move at least one tick in the right direciton. 
                currentMax += (int)Mathf.Sign(diff) + (diff / 10);
                // Not sure if we can just return null here. We also might want to use a timespan for smoother results.
                yield return new WaitForEndOfFrame();
            }
        }
    }
    IEnumerator animateMaxChangeAsync()
    {
        if (!isAnimatingMax)
        {
            isAnimatingMax = true;
            while (currentMax != targetMax)
            {
                var diff = targetMax - currentMax;
                // This *should* ensure that we always move at least one tick in the right direciton. 
                currentMax += (int)Mathf.Sign(diff) + (diff / 10);
                // Not sure if we can just return null here. We also might want to use a timespan for smoother results.
                yield return new WaitForEndOfFrame();
            }
            isAnimatingMax = false;
        }
       
    }
    public void SetMax(int maximum)
    {
        targetMax = maximum;
    }
    public void SetValue(int value)
    {
        if (value > targetMax)
        {
            targetValue = targetMax;
        }
        if (value < 0)
        {
            targetValue = 0;
        }
        else
        {
            targetValue = value;
        }
    }
    public int TargetValue { get { return targetValue; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
