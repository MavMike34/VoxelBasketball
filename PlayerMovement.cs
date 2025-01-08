using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public PlayerControls pC;
    public Rigidbody rB;
    public FixedJoystick joyStick;

    public float animSpeed;
    public float dribbleAnimSpeed;
    public float attackAnimSpeed;
    public float movementSpeed;
    public float turnSpeed;
    public bool isMoving;
    public GameObject idleFrame;
    public GameObject dribbleIdleLegs;
    public GameObject[] walkFrames;

    public GameObject[] idleBallFrames;
    public GameObject[] walkBallFrames;

    public GameObject[] shootBallFrames;

    int walkFrame = 1;
    float walkTimer;

    int idleBallFrame = 1;
    float idleBallTimer;

    int walkBallFrame = 1;
    float walkBallFrameTimer;

    int shootBallFrame = 1;
    float shootBallTimer;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        pC = GetComponent<PlayerControls>();
        rB = GetComponent<Rigidbody>();
        joyStick = FindObjectOfType<FixedJoystick>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pC.isShooting)
        {
            if (joyStick.Horizontal != 0 || joyStick.Vertical != 0)
            {
                isMoving = true;
            }
            else if (joyStick.Horizontal == 0 && joyStick.Vertical == 0)
            {
                isMoving = false;
            }

            shootBallFrame = 1;
            for (int i = 1; i <= 8; i++)
            {
                shootBallFrames[i - 1].SetActive(false);
            }
        }

        if (!pC.hasBall && !pC.isShooting)
        {
            if (isMoving)
            {
                Walking();
                idleFrame.SetActive(false);
            }

            if (!isMoving)
            {
                idleFrame.SetActive(true);
                walkFrame = 1;

                for (int i = 1; i <= 8; i++)
                {
                    walkFrames[i - 1].SetActive(false);
                }
            }

            //Disable The Walking and Idle w/ Ball
            for (int i = 1; i <= 8; i++)
            {
                walkBallFrames[i - 1].SetActive(false);
            }

            for (int i = 1; i <= 7; i++)
            {
                idleBallFrames[i - 1].SetActive(false);
            }

            dribbleIdleLegs.SetActive(false);

        }

        if(pC.hasBall && !pC.isShooting)
        {
            DribbleIdle();

            if (isMoving)
            {
                DribbleWalking();
                dribbleIdleLegs.SetActive(false);
            }

            if (!isMoving)
            {
                dribbleIdleLegs.SetActive(true);

                for (int i = 1; i <= 8; i++)
                {
                    walkBallFrames[i - 1].SetActive(false);
                }
            }

            //Disable The Walking and Idle w/o Ball
            for (int i = 1; i <= 8; i++)
            {
                walkFrames[i - 1].SetActive(false);
            }

            idleFrame.SetActive(false);
        }

        if (pC.isShooting)
        {
            StartShotAnim();

            for (int i = 1; i <= 8; i++)
            {
                walkBallFrames[i - 1].SetActive(false);
            }

            for (int i = 1; i <= 7; i++)
            {
                idleBallFrames[i - 1].SetActive(false);
            }

            dribbleIdleLegs.SetActive(false);
            idleFrame.SetActive(false);

            for (int i = 1; i <= 8; i++)
            {
                walkFrames[i - 1].SetActive(false);
            }
        }
    }

    public void Walking()
    {
        float hor = joyStick.Horizontal;
        float ver = joyStick.Vertical;

        Vector3 moveDir = new Vector3(hor, 0, ver);
        moveDir.Normalize();

        transform.Translate(moveDir * movementSpeed * Time.deltaTime, Space.World);

        if (moveDir != Vector3.zero)
        {
            Quaternion charRo = Quaternion.LookRotation(moveDir, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, charRo, turnSpeed * Time.deltaTime);
        }

        if (isMoving)
        {

            idleFrame.SetActive(false);

            walkTimer += Time.deltaTime;

            if (walkTimer >= animSpeed)
            {
                if (walkFrame < 8)
                {
                    walkFrame += 1;
                }

                else if (walkFrame == 8)
                {
                    walkFrame = 1;
                }

                walkTimer = 0.0f;
            }

            for (int i = 1; i <= 8; i++)
            {
                if (i == walkFrame)
                {
                    walkFrames[i - 1].SetActive(true);
                }
                else
                {
                    walkFrames[i - 1].SetActive(false);
                }
            }
        }

    }

    public void DribbleIdle()
    {
        idleFrame.SetActive(false);

        idleBallTimer += Time.deltaTime;

        if (idleBallTimer >= dribbleAnimSpeed)
        {
            if (idleBallFrame < 7)
            {
                idleBallFrame += 1;
            }

            else if (idleBallFrame == 7)
            {
                idleBallFrame = 1;
            }

            idleBallTimer = 0.0f;
        }

        for (int i = 1; i <= 7; i++)
        {
            if (i == idleBallFrame)
            {
                idleBallFrames[i - 1].SetActive(true);
            }
            else
            {
                idleBallFrames[i - 1].SetActive(false);
            }
        }

    }

    public void DribbleWalking()
    {
        float hor = joyStick.Horizontal;
        float ver = joyStick.Vertical;

        Vector3 moveDir = new Vector3(hor, 0, ver);
        moveDir.Normalize();

        transform.Translate(moveDir * movementSpeed * Time.deltaTime, Space.World);

        if (moveDir != Vector3.zero)
        {
            Quaternion charRo = Quaternion.LookRotation(moveDir, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, charRo, turnSpeed * Time.deltaTime);
        }

        if (isMoving)
        {

            idleFrame.SetActive(false);

            walkBallFrameTimer += Time.deltaTime;

            if (walkBallFrameTimer >= animSpeed)
            {
                if (walkBallFrame < 8)
                {
                    walkBallFrame += 1;
                }

                else if (walkBallFrame == 8)
                {
                    walkBallFrame = 1;
                }

                walkBallFrameTimer = 0.0f;
            }

            for (int i = 1; i <= 8; i++)
            {
                if (i == walkBallFrame)
                {
                    walkBallFrames[i - 1].SetActive(true);
                }
                else
                {
                    walkBallFrames[i - 1].SetActive(false);
                }
            }
        }

    }

    public void StartShotAnim()
    {
        idleFrame.SetActive(false);

        shootBallTimer += Time.deltaTime;

        if (shootBallTimer >= dribbleAnimSpeed)
        {
            if (shootBallFrame < 4)
            {
                shootBallFrame += 1;
            }

            shootBallTimer = 0.0f;
        }

        for (int i = 1; i <= 4; i++)
        {
            if (i == shootBallFrame)
            {
                shootBallFrames[i - 1].SetActive(true);
            }
            else
            {
                shootBallFrames[i - 1].SetActive(false);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void OnTriggerExit(Collider other)
    {

    }
}

