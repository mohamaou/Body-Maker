using System;
using RootMotion.Dynamics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private new PlayerAnimation animation;
    [SerializeField] private PlayerBody body;
    [SerializeField] private FieldOfView fieldOfView;
    [SerializeField] private BodyInfo info;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private PuppetMaster puppetMaster;
    [HideInInspector] public Image myLogo;
    private Camera cam;
    private float health, maxHealth;
    
    [Header("Movement")]
    [SerializeField] [Range(1,5)] private float range = 5f;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private LayerMask playerLayer;
    private Transform target;
    private float changeDirection;

    public bool IsVisible() => body.IsVisible(cam);
    public void Dance() => animation.Dance();
    
    private void Start()
    {
        body.SetRandomBody();
        RandomAnimation();
        health = maxHealth = info.health;
        agent.speed *= Player.Instance.SetSpeed(info.speed);
        cam = Camera.main;
        agent.speed = SetSpeed();
    }

    private float SetSpeed()
    { 
        if(info.speed < 0.2f ) return 1f; 
        if(info.speed < 0.4f ) return 1.5f; 
        if(info.speed < 0.8f ) return 2f; 

                return 2.5f;
      
    }
    
    
    private void Update()
    {
        if(GameManager.State != GameState.Play) return;
        FindEnemy();
        AiMovement();
    }
    
    private void FindEnemy()
    {
        animation.Attack(fieldOfView.visibleTargets.Count > 0);
        agent.isStopped = fieldOfView.visibleTargets.Count > 0;
        if (fieldOfView.visibleTargets.Count <= 0) return;
        var dir = - transform.position + fieldOfView.visibleTargets[0].position;
        transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(dir),6*Time.deltaTime);
    }

    private void AiMovement()
    {
        var results = new Collider[10];
        var size = Physics.OverlapSphereNonAlloc(transform.position, range, results, playerLayer);
        animation.Run(agent.velocity.magnitude > 0.05f, true);
        if (size == 0)
        {
            if (changeDirection < 0)
            {
                Vector3 randomDirection = Random.insideUnitSphere * 10;
                if(Vector3.Distance(transform.position,randomDirection) < 3) randomDirection = Random.insideUnitSphere * 10;
                agent.destination = randomDirection;
                changeDirection = 3f;
            }

            changeDirection -= Time.deltaTime;
        }
        else
        {
            for (int i = 0; i < results.Length; i++)
            {
                if (results[i] != null)
                {
                    target = results[i].transform;
                }
            }
            agent.destination = target.position;
        }
    }

    public void RandomAnimation()
    {
        animation.SetRandomAnimation();
    }
    

    public void Attack()
    {
        Player.Instance.GetHet(info.damage);
    }

    public void GetHit(float damage)
    {
        health -= damage / 5;
        healthBar.SetValue(health, maxHealth);
        if(health < 0)KillMe();
    }

    private void KillMe()
    {
        puppetMaster.pinWeight = 0.2f;
        puppetMaster.muscleWeight = 0.2f;
        Destroy(gameObject.GetComponent<Rigidbody>());
        gameObject.GetComponent<Collider>().isTrigger = true;
        Destroy(healthBar.gameObject);
        animation.DisableAnimation();
        gameObject.layer = 0;
        agent.enabled = false;
        body.Death();
        GameManager.Instance.enemiesCount--;
        enabled = false;
        myLogo.color = Color.gray;
    }
    
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
