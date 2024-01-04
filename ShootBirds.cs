using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShootBirds : MonoBehaviour
{

    //public variables
    //variable for Rigidbody
    public Rigidbody bird;
    //variable for amount of force a bird has coming out of the shooter
    public float shotForce = 1000f;
    float currTime; //current Time since start of simulation
    bool fired = false;
    public int timeLimit = 60;
    public int modValue = 3;
    private int birdNum = 1; //Debugging purposes
    public GameObject birdie;
    public float gravitationalConstant = -9.8f; //7-19-23 change
    public int method = 1; //within subject design, A == 1, B == 0

    public UserDisplayScript UserInfoScript; //for dealing with the script meant to show the VR user
    //public StaticSceneSwitcher SceneSwitchingScript; //script meant for if I'm to switch scenes or not.

    //Maybe have some variable for printing if the simulation is over after a set amount of time
    //bool simulationFinished = false;

    public GameObject Racket;
    public MeshRenderer ScoringPlaneMesh;

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = new Vector3(0f, gravitationalConstant, 0f); //new change 7-21-23
        currTime = 0f; //7-25-23 change
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //currTime = Time.time;
        currTime += Time.fixedDeltaTime; //7-25-23 change
        if (currTime > timeLimit)
        {
            //if (Time.timeScale == 0.5f)
            //{
            //    print("Time Scale Print Reached");
            //    Time.timeScale = 1.0f;
            //    Time.fixedDeltaTime = (float)(0.02f * Time.timeScale);
            //}
            ////This point of the code, stop shooting birds and log data for number of shots made over the net
            ////print("Time's up");
            //Time.timeScale = 1.0f;
            //Time.fixedDeltaTime = (float)(0.02f * Time.timeScale);
            ScoringPlaneMesh.enabled = true;
            UserInfoScript.seconds = 10-((int)currTime - timeLimit);
            UserInfoScript.SetUserBufferText();
            if(currTime > (timeLimit + 10))
            {
                Destroy(Racket, 0f);
                if(method == 1 && StaticSceneSwitcher.SceneSwitcher == 0)
                {
                    StaticSceneSwitcher.SceneSwitcher += 1;
                    SceneManager.LoadScene("A_2");
                }
                else if(method == 1 && StaticSceneSwitcher.SceneSwitcher == 1)
                {
                    UserInfoScript.SetRoundFinisherText();
                }
                else if(method == 0 && StaticSceneSwitcher.SceneSwitcher == 0)
                {
                    StaticSceneSwitcher.SceneSwitcher += 1;
                    SceneManager.LoadScene("B_2");
                }
                else if (method == 0 && StaticSceneSwitcher.SceneSwitcher == 1)
                {
                    UserInfoScript.SetRoundFinisherText();
                }
            }
            return;
        }else if((int)currTime % modValue == 0 && fired == false)
        {
            /*At this point of the code, if the modulus works out, shoot a bird,
            but establish the boolean to say we fired a bird and we need to wait for the next second
            to reset the boolean
            */
            //print("Shot taken at: " + currTime);
            if (birdNum <= 20 && birdNum >= 15)
            {
                UserInfoScript.birdCount = 20 - birdNum;
                UserInfoScript.SetCountdownText();
            }
            if(birdNum == 21)
            {
                UserInfoScript.GoText();
                ScoringPlaneMesh.enabled = false;
            }
            if(birdNum == 22)
            {
                UserInfoScript.ResetText();
            }
            fired = true;
            Rigidbody shot = Instantiate(bird, transform.position, transform.rotation) as Rigidbody;
            shot.AddForce(transform.up * (shotForce /Time.timeScale));
            Physics.gravity = new Vector3(0f, gravitationalConstant, 0f); //new change 7-19-23, commented out so that I can just set gravity once
            //print("Time Scale according to spawn: " + Time.timeScale);
            print("Bird Number: " + birdNum);
            //print("Bird velocity: " + shot.velocity);
            birdNum++;

            GameObject birdieShot = shot.gameObject;
            Destroy(birdieShot, 10f);
        }
        else if((int)currTime % modValue == 0 && fired == true)
        {
            /*At this point in the code, if the modulus works out,
             * but we've already fired a bird, ignore the fact
             * that the modulus worked out and wait for the next second
             */
            //print("No shot at: " + currTime);
        }
        else
        {
            /*At this point in the code, 
             * the modulus didn't work out, so 
             * we know not to fire a bird, and to reset the fired
             * boolean
             */
            //print("CurrTime: " + currTime);
            fired = false;
        }
        
    }

    private void Update()
    {
        if (currTime > timeLimit)
        {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = (float)(0.02f * Time.timeScale);
        }
    }
}


