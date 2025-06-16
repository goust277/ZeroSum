using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;

public class NPCController : MonoBehaviour
{
    public enum NPCState
    {
        Idle,
        Walk,
        Run,
        Teleport,
        Dead,
        Down,
    }
    [Header("NpcHp")]
    [SerializeField] private NPC_Hp npcHp;
    [SerializeField] private GameObject hpBar;

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
    [SerializeField] private NPCDead _dead;
    [SerializeField] private NPCDown _down;

    public bool isDown;
    public bool isDowned = false;
    [Header("Animator")]
    public Animator animator;

    [Header("Sprite")]
    [SerializeField] private SpriteRenderer sprite;

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
        IState<NPCController> Dead = _dead;
        IState<NPCController> Down = _down;

        dicState.Add(NPCState.Idle, Idle);
        dicState.Add(NPCState.Run, Run);
        dicState.Add(NPCState.Walk, Walk);
        dicState.Add(NPCState.Teleport, Teleport);
        dicState.Add(NPCState.Dead, Dead);
        dicState.Add(NPCState.Down, Down);

        sm = new StateMachine<NPCController>(this, dicState[NPCState.Idle]);
    }

    // Update is called once per frame
    void Update()
    {
        if (!npcHp.isDead)
        {
            distanceX = Mathf.Abs(transform.position.x - player.transform.position.x);
            distanceY = Mathf.Abs(transform.position.y - player.transform.position.y);

            moveDirection.x = player.transform.position.x - transform.position.x;

            if(isDown && !isDowned)
            {
                sm.SetState(dicState[NPCState.Down]);
                isDowned = true;
            }

            if (distanceX < walkDistance && !isDown)
            {
                sm.SetState(dicState[NPCState.Idle]);
                isDowned = false;
            }

            else if (walkDistance <= distanceX && distanceX < runDistance)
            {
                sm.SetState(dicState[NPCState.Walk]);
                isDowned = false;
            }

            else if (runDistance <= distanceX)
            {
                sm.SetState(dicState[NPCState.Run]);
                isDowned = false;
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
                isDowned = false;
            }

            
        }
        
        else if (npcHp.isDead) 
        {
            sm.SetState(dicState[NPCState.Dead]);
        }
        sm.DoOperateUpdate();
    }

    private void Flip()
    {
        Debug.Log("Filp");

        moveLeft = !moveLeft;
        //gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);

        sprite.flipX = true;
        //hpBar.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 180, 0);

        if (!moveLeft)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            sprite.flipX = false;
        }
    }

    private void OnEnable()
    {
        hpBar.SetActive(true);
        npcHp.enabled = true;
    }

    public void OnDown(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isDown = true;
        }
        if (context.canceled && isDown)
        {
            isDown = false;
        }
    }
}
