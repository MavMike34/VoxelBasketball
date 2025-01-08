using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Polycrime;

public class PlayerControls : MonoBehaviour
{

    public PlayerMovement pM;
    public GameObject bBall;
    public GameObject finalFrameShootBall;
    public GameObject shotOBJ;

    public bool hasBall, isShooting;
    public float jumpHeight;

    public GameObject shootHoop;
    public bool ballInAir;
    public float ballTime;
    public float duration; //Time it takes for ball to go through the net. Make this time relative to Propulsion's Reach Time with some delay.

    public float shootingTimer;
    public float shotTime;
    public float ballArc;
    public float ballSmoothness;
    public Image shotMeter;

    // Start is called before the first frame update
    void Start()
    {
        pM = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isShooting)
        {
            shootingTimer += Time.deltaTime;

            if(shootingTimer >= shotTime)
            {
                ReleaseTheBall();
                hasBall = false;
                isShooting = false;
                shotOBJ.SetActive(true);
            }
        }

        if (ballInAir && bBall.activeSelf)
        {
            ballTime += Time.deltaTime;

            if (ballTime >= duration) //Determines when the ball is done going through the hoop.
            {
                shootingTimer = 0;
                ballInAir = false;
                isShooting = false;
                ballTime = 0;
                shotOBJ.SetActive(false);
                bBall.layer = 0;
            }
        }
    }

    public void ReleaseTheBall()
    {
        ballInAir = true;
        bBall.transform.position = finalFrameShootBall.transform.position;
        bBall.transform.rotation = finalFrameShootBall.transform.rotation;
        bBall.transform.parent = null;
        bBall.SetActive(true);
        isShooting = false;
        hasBall = false;
        bBall.layer = 3;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Basketball" && !ballInAir)
        {
            bBall.transform.parent = this.gameObject.transform;
            bBall.SetActive(false);
            hasBall = true;
        }
    }

    public void StartShooting()
    {
        if (hasBall && !isShooting)
        {
            isShooting = true;
            //pM.StartShotAnim();
            pM.rB.AddForce(0, jumpHeight, 0);
            Vector3 targetPos = new Vector3(shootHoop.transform.position.x, transform.position.y, shootHoop.transform.position.z);
            transform.LookAt(targetPos);
        }
    }
}
