using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum State { PATROL, SUSPECT, CHASE };
public class Chef : MonoBehaviour
{
    [SerializeField] public State state;

    [Header("Patrol")]
    [SerializeField] Transform[] waypoints;
    [SerializeField] bool isActive = true;
    [SerializeField] bool isLooping = true;
    [SerializeField] float waitTime = 0.5f;
    [SerializeField] float maxDistance = 0;
    //[SerializeField] float rotationSpeed = 720;
    Vector3 rotPatrol;

    [Header("Detect")]
    [SerializeField] LayerMask whatItDetects;
    [SerializeField] Transform eyeOrigin;
    [SerializeField] Transform eyeOrigin2;
    [SerializeField] float eyeRadius;
    [SerializeField] float eyeDistance;
    [SerializeField] float suspectDistance;
    float eyeDistanceCurrent;

    [Header("Speeds")]
    [SerializeField] float speedPatrol;
    [SerializeField] float speedSuspect;
    [SerializeField] float speedAttack;


    [Header("Hands")]
    [SerializeField] Transform rightHand;
    [SerializeField] Transform leftHand;

    bool isWaiting;
    float distance;
    int current;

    [Header("Suspect")]

    [Header("Chase")]
    Transform targetCurrent;

    NavMeshAgent agent;
    Animator animator;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rotPatrol = new Vector3(0, 270, 0);
        current = 0;
        ChangeState(State.PATROL, null);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.PATROL:
                if (!isWaiting && isActive)
                    Move();
                break;
            case State.SUSPECT:
                agent.updateRotation = false;
                agent.SetDestination(GameManager.instance.player.transform.position);

                Debug.Log(Vector3.Distance(transform.position, targetCurrent.position));

                if (Vector3.Distance(transform.position, targetCurrent.position) <= suspectDistance)
                    ChangeState(State.PATROL, null);
                    
                break;

            case State.CHASE:
                agent.updateRotation = false;
                agent.SetDestination(GameManager.instance.player.transform.position);
                break;
        }

        Detect();

        animator.SetBool("atk", state == State.CHASE);
        animator.SetBool("walk", state == State.PATROL);
    }
    void Move()
    {
        Vector3 wp = waypoints[current].transform.position;

        agent.SetDestination(wp);
        agent.updateRotation = false;
        //transform.eulerAngles = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(rotPatrol, Vector3.up), rotationSpeed * Time.deltaTime).eulerAngles;
        transform.eulerAngles = rotPatrol;

        distance = Mathf.Abs(Vector3.Distance(transform.position, wp));

        if (distance <= maxDistance)
        {
            isWaiting = true;
            Invoke("NextWayPoint", waitTime);
        }

    }
    void NextWayPoint()
    {
        isWaiting = false;

        current++;

        if (current >= waypoints.Length)
        {
            if (isLooping)
                current = 0;
            else
            {
                current = waypoints.Length;
                isWaiting = true;
            }
        }

    }

    void Detect()
    {
        //Detectar al jugador
        if (Physics.SphereCast(eyeOrigin.position, eyeRadius, eyeOrigin.forward, out RaycastHit hit, eyeDistance, whatItDetects))
        {
            if (hit.collider.CompareTag("Player") && state != State.CHASE)
            {
                Debug.Log("CHEF persigue jugador");
                ChangeState(State.CHASE, hit.transform);
            }
        }
        else
        {
            eyeDistanceCurrent = eyeDistance;
        }

        //Comprobar que el jugador se escapa
        if (!isPlayerInside() && state == State.CHASE)
        {
            Debug.Log("CHEF pierde de vista a jugador");
            ChangeState(State.PATROL, null);
        }
    }

    bool isPlayerInside()
    {
        RaycastHit[] hits = Physics.SphereCastAll(eyeOrigin2.position, eyeRadius, eyeOrigin.forward, eyeDistance, whatItDetects);
        foreach (var item in hits)
        {
            eyeDistanceCurrent = item.distance;

            if (item.collider.CompareTag("Player"))
                return true;
        }

        return false;
    }

    public void ChangeState(State stateNew, Transform targetNew)
    {
        state = stateNew;

        switch (state)
        {
            case State.PATROL:
                targetCurrent = null;
                agent.speed = speedPatrol;
                break;
            case State.SUSPECT:
                targetCurrent = targetNew;
                agent.speed = speedSuspect;
                break;
            case State.CHASE:
                agent.speed = speedAttack;
                targetCurrent = targetNew;
                break;
        }
    }

    public void BackToPatrol()
    {
        state = State.PATROL;
        targetCurrent = null;
        agent.speed = speedPatrol;

    }

    public void ThrowKnifeRightHand()
    {
        ObjectPooler.instance.SpawnFromPool("Knife", rightHand.position, Quaternion.LookRotation(transform.forward));
    }
    public void ThrowKnifeLeftHand()
    {
        ObjectPooler.instance.SpawnFromPool("Knife", leftHand.position, Quaternion.LookRotation(transform.forward));
    }

    public void ChangeWaypoints(Transform[] wps, Vector3 rot)
    {
        waypoints = wps;
        current = 0;
        rotPatrol = rot;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(eyeOrigin.position, eyeOrigin.position + eyeOrigin.forward * eyeDistance);
        Gizmos.DrawWireSphere(eyeOrigin.position + eyeOrigin.forward * eyeDistance, eyeRadius);

        //Gizmos.DrawWireSphere(eyeOrigin2.position, eyeRadius);

        if (waypoints != null)
        {
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (i != waypoints.Length - 1)
                    Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                else
                    Gizmos.DrawLine(waypoints[i].position, waypoints[0].position);
            }
        }

    }

}
