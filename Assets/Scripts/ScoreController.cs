using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    void OnGUI()
    {
        GUI.Label(
           new Rect(
               5,                   // x, left offset
               Screen.height - 150, // y, bottom offset
               300f,                // width
               150f                 // height
           ),
           Score.ToString(),             // the display text
           GUI.skin.textArea        // use a multi-line text area
        );
    }
    // I wonder if this needs synchronization? Not sure if asynchrony requires protections from concurrency in this situaion.
    bool IsAnimatingScoreChange = false;
    IEnumerator AnimateScoreChangeAsync()
    {
        if (!IsAnimatingScoreChange)
        {
            IsAnimatingScoreChange = true;
        }
        while (DisplayScore != ActualScore)
        {
            var diff = ActualScore - DisplayScore;
            // This *should* ensure that we always move at least one tick in the right direciton. 
            DisplayScore += (int)Mathf.Sign(diff)+(diff / 10);
           // Not sure if we can just return null here. We also might want to use a timespan for smoother results.
            yield return new WaitForEndOfFrame();
        }
        IsAnimatingScoreChange = false;

    }
    public void ModifyScoreBy(int amount)
    {
        ActualScore += amount;
        StartCoroutine(AnimateScoreChangeAsync());
    }
    private int DisplayScore = 0;
    private int ActualScore = 0;
    public int Score{
        get { return DisplayScore; }
      
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
