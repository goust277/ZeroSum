using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;

public class NPCController : MonoBehaviour
{
    public enum NPCState
    {
        Idle,
        Walk,
        Run,
        Teleport,
    }

    [Header("Speed")]
    public float runSpeed;
    public float walkSpeed;
    public Rigidbody2D rb;

    [HideInInspector] public float distanceX;
    [HideInInspector] public float distanceY;
    public Vector2 moveDirection;

    [Header("Distance")]
    [SerializeField] private float runDistance;
    [SerializeField] private float walkDistance;
    [SerializeField] private float teleportDistance;

    [Header("Player")]
    public GameObject player;

    [Header("State")]
    [SerializeField] private NPCIdle _idle;
    [SerializeField] private NPCRun _run;
    [SerializeField] private NPCWalk _walk;
    [SerializeField] private NPCTeleport _teleport;
    
    [Header("Animator")]
    public Animator animator;

    private bool moveLeft = true;
    private enum Direction
    {
        Left = -1,
        Right = 1, 
    }

    private Dictionary<NPCState, IState<NPCController>> dicState = new Dictionary<NPCState, IState<NPCController>>();
    private StateMachine<NPCController> sm;

    void Start()
    {
        IState<NPCController> Idle = _idle;
        IState<NPCController> Run = _run;
        IState<NPCController> Walk = _walk;
        IState<NPCController> Teleport = _teleport;

        dicState.Add(NPCState.Idle, Idle);
        dicState.Add(NPCState.Run, Run);
        dicState.Add(NPCState.Walk, Walk);
        dicState.Add(NPCState.Teleport, Teleport);

        sm = new StateMachine<NPCController>(this, dicState[NPCState.Idle]);
    }

    // Update is called once per frame
    void Update()
    {
        distanceX = Mathf.Abs(transform.position.x - player.transform.position.x);
        distanceY = Mathf.Abs(transform.position.y - player.transform.position.y);

        moveDirection.x = player.transform.position.x - transform.position.x;

        if (distanceX < walkDistance)
        {
            sm.SetState(dicState[NPCState.Idle]);
        }

        else if (walkDistance <= distanceX && distanceX < runDistance)
        {
            sm.SetState(dicState[NPCState.Walk]);
        }

        else if (runDistance <= distanceX)
        {
            sm.SetState(dicState[NPCState.Run]);
        }

        if (moveDirection.x >= 0 && moveLeft)
        {
            Flip();
        }
        else if (moveDirection.x < 0 && !moveLeft)
        {
            Flip();
        }

        if (teleportDistance <= distanceY)
        {
            sm.SetState(dicState[NPCState.Teleport]);
        }
        sm.DoOperateUpdate();
    }

    private void Flip()
    {
        Debug.Log("Filp");

        moveLeft = !moveLeft;
        gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);

        if (!moveLeft)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
