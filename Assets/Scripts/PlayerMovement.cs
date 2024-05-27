using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    public float forwardSpeed = 5f;
    
    [Inject]
    public PathManager pathManager;
    
    public Transform currentTarget;
    
    private bool isMoving = true;

    private bool isMovingToFinish = false;
    
    [Inject]
    private GameManager gameManager;
    
    private Rigidbody rigidBody;
    
    private Vector3 targetPosition;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        SetNextTarget();

        InvokeRepeating(nameof(CheckAcceleration), 0f, 1f);

    }
    
    void CheckAcceleration()
    {
        if(rigidBody.velocity.y<-5) 
            gameManager.GameOver();
        
    }

    private void IsMovingHandler(bool t)
    {
        isMoving = t;
    }
    
    private void IsMovingToFinishHandler(bool t)
    {
        isMovingToFinish = t;
    }
    
    private bool CanMove()
    {
        if (isMoving) return true;

            return false;
    }
    
    void FixedUpdate()
    {
        if (!CanMove()) return;

        if(!isMovingToFinish)
            MoveToPlatform();
        else
        {
            MoveToFinish();
        }
    }

    private void MoveToPlatform()
    {
        if (currentTarget == null)
        {
            SetNextTarget();
        }

        if (currentTarget != null)
        {
            targetPosition = new Vector3(currentTarget.position.x, transform.position.y, currentTarget.position.z);

            if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, forwardSpeed * Time.deltaTime);
            }
            else
            {
                
                    SetNextTarget();
                    
            } 
        }
        else
        {
            // Hedef yoksa z ekseninde ilerlemeye devam et
            transform.position += Vector3.forward * forwardSpeed * Time.deltaTime;

        }
    }
    
    
    private void MoveToFinish()
    {
        if (currentTarget == null)
        {
            SetNextTarget();
        }
        
        targetPosition = new Vector3(currentTarget.position.x, transform.position.y, currentTarget.position.z);
        if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, forwardSpeed * Time.deltaTime);
            }
            else
            {
                
                    IsMovingHandler(false);
                    gameManager.GameFinished();
            }
                
    }
    
    void SetNextTarget()
    {
        currentTarget = pathManager.GetNextPlatform();
        if (currentTarget != null)
        {
            pathManager.RemoveFirstPlatform();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            pathManager.AddPlatform(collision.transform);
            
            IsMovingToFinishHandler(true);
            
            pathManager.SetLastPlatform(collision.gameObject);
        }
    }


    public void StartNewGame()
    {
        IsMovingHandler(true);
        IsMovingToFinishHandler(false);

    }

    public void GameFinished()
    {
        SetNextTarget();
    }
    
}