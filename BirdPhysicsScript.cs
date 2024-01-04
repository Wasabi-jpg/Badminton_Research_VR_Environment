using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BirdPhysicsScript : MonoBehaviour
{
    public float birdSpeed = 10f;
    public Transform birdHead; //for the head of the bird
    public GameObject racketHead; //for the head of the racket
    public GameObject racketHeadFace; //for the part of the racket that will have its velocity calculated
    public bool slowed; //boolean for if we're testing the slowed version of our simulation
    public float slowFactor; //By how much do we slow down the birdie upon launch from the spawner
    public GameObject scoringPlane; //the plane that if the bird intersects, increase score
    public ScoringPlaneScript scoringPlaneScript; //script for the scoring plane
    //public TextMeshPro text; //for displaying score


    Collider racketHeadCollider;
    Rigidbody racketHeadRigidBody;

    private Rigidbody _rB;
    private bool _inTheAir = false; //is the bird currently flying thru the air
    private Vector3 _lastPos = Vector3.zero;

    private Vector3 prevPosOfFace;
    private Vector3 currentVelocityOfFace;
    private Vector3 faceDisplacement;

    public float birdieTimeScale; //time scaling for the bird, where I can either slow down time for the bird if I choose

    //private Vector3 prevBirdPos; //for debugging purposes
    private int frameNum; //for debugging purposes
    //private float prevTime; //for debugging purposes

    // Start is called before the first frame update
    void Start()
    {
        _rB = GetComponent<Rigidbody>();
        _inTheAir = true;
        /*^currently predicting that when the bird
         * spawns in, the script for the bird will run
         * in which this Start() will run for the specifc
         * bird that was spawned. Hence, the assignment
         * and boolean for the bird in the air set to true.
         */
        slowed = false;
        slowFactor = 1.0f;
        StartCoroutine(ArcBirdHead());

        racketHeadCollider = racketHead.GetComponent<BoxCollider>(); //collider for racketHead
        racketHeadRigidBody = racketHead.GetComponent<Rigidbody>();

        prevPosOfFace = racketHeadFace.transform.position;

        if (slowed) //Time scaling for the bird, using Time.timeScale
        {
            birdieTimeScale = slowFactor;
            Time.timeScale = birdieTimeScale;
            Time.fixedDeltaTime = (float)(0.02f * Time.timeScale);
        }
        else
        {
            //Time.timeScale = 1f; //optional, just so we can switch from slowed speed or normal speed
            Time.fixedDeltaTime = (float)((double)0.02 * (double)Time.timeScale);
        }

        

        //print("Fixed Delta Time: " + Time.fixedDeltaTime);

        //Debug.Log("Default Solver Iterations: " + Physics.defaultSolverIterations);
        //Debug.Log("Default Solver Velocity Iterations: " + Physics.defaultSolverVelocityIterations);

        //prevBirdPos = _rB.transform.position;
        frameNum = 0;
        //prevTime = Time.time;

    }

    private IEnumerator ArcBirdHead()
    {
        //yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(.05f); //works, the idea came down to momentum direction from arcing was applied incorrectly
        while (_inTheAir)
        {
            Quaternion newOrientation = Quaternion.LookRotation(_rB.velocity, Vector3.up);
            transform.rotation = newOrientation;
            //yield return null;
            yield return new WaitForFixedUpdate(); //Uncommented to test 7-21-23
        }
    }
    void FixedUpdate()
    {
        if (_inTheAir)
        {
            //print("Debugging Velocity: " + _rB.velocity);
        }
        if(racketHeadFace != null) //7/26/23 addition
        {
            faceDisplacement = racketHeadFace.transform.position - prevPosOfFace;
            float deltaTime = Time.fixedDeltaTime; //Adjusted

            currentVelocityOfFace = faceDisplacement / deltaTime;
            prevPosOfFace = racketHeadFace.transform.position;
        }
        _lastPos = birdHead.position; //updating the last known position of the bird's head 

        //Eliminated Velocity calculation code since that was also commented out and useless
    }

    void Update()
    {
        
            //print("Debugging Velocity from Frame #: " + frameNum + " AND velocity: "+ _rB.velocity);
            frameNum++;
        if (slowed) //Time scaling for the bird, using Time.timeScale
        {
            birdieTimeScale = slowFactor;
            Time.timeScale = birdieTimeScale;
            Time.fixedDeltaTime = (float)(0.02f * Time.timeScale);
        }
        else
        {
            Time.fixedDeltaTime = 0.02f;
        }
        //if (frameNum == 1)
        //{
        //    print("Bird velocity: " + _rB.velocity);
        //}
    }
//Eliminated code regarding Raycast collision checking, since that was commented out and rather useless

    private IEnumerator DisableRacketHeadCollider(float time)
    {
        
        racketHeadCollider.enabled = false;
        yield return new WaitForSeconds(1f);
        racketHeadCollider.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //This function is for if the bird hit the ground
        if(collision.transform.gameObject.layer == 6)
        {
            _inTheAir = false;
        }else if(collision.transform.gameObject.layer == 7)
        {
            //print("HIT");
            //_rB.velocity = racketHeadRigidBody.velocity; //changing velocity of birdie

            //if (slowed)
            //{
            //    Time.timeScale = 1f; //to reset the timeScale upon striking the bird if I'm slowing 
            //}

            _rB.velocity = currentVelocityOfFace;
            //print("called at: " + Time.time);
            StartCoroutine(DisableRacketHeadCollider(1f));

        }
    }

    private void OnTriggerEnter(Collider other) //Upon a bird triggering the trigger of the scoringPlane, increment score and set the text to reflect such
    {
        if(other.transform.gameObject.layer == 8)
        {
            scoringPlaneScript.scoreCount += 1;
            scoringPlaneScript.SetScoreText();
        }
    }
}
