using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoringPlaneScript : MonoBehaviour
{
    public int scoreCount;
    public TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    void Start()
    {
        scoreCount = 0;
        SetScoreText();
    }

    public void SetScoreText()
    {
        scoreText.text = "Score: " + scoreCount.ToString();
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
