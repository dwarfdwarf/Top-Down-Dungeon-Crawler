using UnityEngine;
using Pathfinding;
 
public class EnemyAI : MonoBehaviour
{
 
    Transform target;
 
    public float speed = 1.2f;
    public float nextWaypointDistance = 0.8f;
    public float aggroRange;
 
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath;
 
    Seeker seeker;
    Rigidbody2D rb;

    public Animator animator;
    public Transform enemy;
 
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
 
    private void Awake()
    {
        target = GameObject.FindWithTag("Player").transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
    }
 
    private void Start()
    {
        InvokeRepeating("CheckDist", 0, 1f);
    }
 
    void FixedUpdate()
    {
        if (path == null)
            return;
 
        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }
 
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
 
        rb.MovePosition(rb.position + force);
 
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
 
        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        animator.SetFloat("Speed", Mathf.Abs(speed));
        if(force.x >= 0.01f)
        {
            enemy.localScale = new Vector3(-1, 1, 1);
        }
        else if(force.x <= -0.01f)
        {
            enemy.localScale = new Vector3(1, 1, 1);
        }
    }
 
    void CheckDist()
    {
        float dist = Vector2.Distance(rb.position, target.position);
 
        if (dist <= aggroRange)
        {
            InvokeRepeating("UpdatePath", 0, .5f);
        }
        else
        {
            CancelInvoke("UpdatePath");
            path = null;
        }
    }
 
    void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }
 
    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}