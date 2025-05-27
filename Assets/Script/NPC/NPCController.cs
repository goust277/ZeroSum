using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public enum NPCState
    {
        Idle,
        Walk,
        Run,
        Teleport,
        Dead,
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

        dicState.Add(NPCState.Idle, Idle);
        dicState.Add(NPCState.Run, Run);
        dicState.Add(NPCState.Walk, Walk);
        dicState.Add(NPCState.Teleport, Teleport);
        dicState.Add(NPCState.Dead, Dead);

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
}
