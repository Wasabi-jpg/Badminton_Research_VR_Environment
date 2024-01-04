using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserDisplayScript : MonoBehaviour
{
    public int seconds;
    public int birdCount;
    public TextMeshProUGUI userText;
    // Start is called before the first frame update
    void Start()
    {
        userText.text = "";
    }


    public void SetUserBufferText()
    {
        userText.text = "Seconds Left: " + seconds.ToString();
    }

    public void SetRoundFinisherText()
    {
        userText.text = "All finished with this set! Waiting for another if needed.";
    }
    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void SetCountdownText()
    {
        userText.text = "Starting in: " + birdCount.ToString() + " birds.";
    }

    public void GoText()
    {
        userText.text = "START HITTING!";
        userText.text = "";

    }
    public void ResetText()
    {
        userText.text = "";
    }
}
