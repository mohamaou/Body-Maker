using System;
using System.Collections.Generic;
using RootMotion.Dynamics;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class PlayerAnimation
{
    private static readonly int RunId = Animator.StringToHash("Run");
    private static readonly int SpeedId = Animator.StringToHash("Speed");

    [SerializeField] private Animator anim;

    private float runSpeed;
    private static readonly int AttackId = Animator.StringToHash("Attack");
    private static readonly int RandomAttackId = Animator.StringToHash("RandomAttack");
    


    public void Run(bool run, bool runFast = false)
    {
        anim.SetBool(RunId,run);
        if(runFast) anim.SetFloat(SpeedId,1);
    }

    public void DisableAnimation() => anim.enabled = false;
    
    public void Attack(bool attack)
    {
        anim.SetBool(AttackId, attack);
    }

    public void SetRandomAnimation()
    {
        var r = Random.Range(0, 6);
        anim.SetFloat(RandomAttackId,r/5f);
    }

    public void Player()
    {
        if(Input.GetMouseButtonDown(0))Run(true);
        if(Input.GetMouseButtonUp(0))Run(false);

        runSpeed = anim.GetFloat(SpeedId);
        var target = Movement.Run()? 1f:0f;
        runSpeed = Mathf.Lerp(runSpeed, target, 6 * Time.deltaTime);
        anim.SetFloat(SpeedId,runSpeed);
    }

    public void Dance()
    {
        anim.SetTrigger("Dance");
    }
}

[Serializable]
public class AnimalParts
{
    public string animalName;
    public AnimalType animal;
    public Material material;
    [SerializeField] private Mesh head, hips, leftArm, rightArm, leftLeg, rightLeg;

    public Mesh GetMesh(AnimalType animalType, PartType bodyParts)
    {
        if (animalType != animal) return null;
        switch(bodyParts)
        {
            case PartType.Head:
                return head;
            case PartType.Hips:
                return hips;
            case PartType.LeftArm:
                return leftArm;
            case PartType.RightArm:
                return rightArm;
            case PartType.LeftLeg:
                return leftLeg;
            case PartType.RightLeg:
                return rightLeg;
        }
        return null;
    }
    
    public Mesh GetBodyPartMesh(PartType bodyParts)
    {
        switch(bodyParts)
        {
            case PartType.Head:
                return head;
            case PartType.Hips:
                return hips;
            case PartType.LeftArm:
                return leftArm;
            case PartType.RightArm:
                return rightArm;
            case PartType.LeftLeg:
                return leftLeg;
            case PartType.RightLeg:
                return rightLeg;
        }
        return null;
    }
}

[Serializable]
public class PlayerBody
{
    [SerializeField] private SkinnedMeshRenderer head, hips, leftArm, rightArm, leftLeg, rightLeg;
    [SerializeField] private AnimalParts[] animals;
    [SerializeField] private Renderer render;
    public void SetBody(Body data)
    {
        for (int i = 0; i < animals.Length; i++)
        {
            var headMesh = animals[i].GetMesh(data.head, PartType.Head);
            var hipsMesh = animals[i].GetMesh(data.hips, PartType.Hips);
            var lefArmMesh = animals[i].GetMesh(data.leftArm, PartType.LeftArm);
            var rightArmMesh = animals[i].GetMesh(data.rightArm, PartType.RightArm);
            var leftLegMesh = animals[i].GetMesh(data.leftLeg, PartType.LeftLeg);
            var rightLegMesh = animals[i].GetMesh(data.rightLeg, PartType.RightLeg);

            if (headMesh != null)
            {
                head.sharedMesh = headMesh;
                head.material = animals[i].material;
            }
            if (hipsMesh != null)
            {
                hips.material = animals[i].material;
                hips.sharedMesh = hipsMesh;
            }
            if (lefArmMesh != null)
            {
                leftArm.material = animals[i].material;
                leftArm.sharedMesh = lefArmMesh;
                
            }
            if (rightArmMesh != null)
            {
                rightArm.material = animals[i].material;
                rightArm.sharedMesh = rightArmMesh;
            }
            if (leftLegMesh != null)
            {
                leftLeg.material = animals[i].material;
                leftLeg.sharedMesh = leftLegMesh;
            }
            if (rightLegMesh != null)
            {
                rightLeg.material = animals[i].material;
                rightLeg.sharedMesh = rightLegMesh;
            }
        }
    }

    public void SetRandomBody()
    {
        var headAnimal = animals[Random.Range(0, animals.Length)];
        var hipsAnimal = animals[Random.Range(0,animals.Length)];
        var lefArmAnimal = animals[Random.Range(0,animals.Length)];
        var rightArmAnimal = animals[Random.Range(0,animals.Length)];
        var leftLegAnimal = animals[Random.Range(0,animals.Length)];
        var rightLegAnimal = animals[Random.Range(0,animals.Length)]; 
        
        head.sharedMesh = headAnimal.GetBodyPartMesh(PartType.Head);
        head.material = headAnimal.material;
        hips.sharedMesh = hipsAnimal.GetBodyPartMesh( PartType.Hips);
        hips.material = hipsAnimal.material;
        leftArm.sharedMesh = lefArmAnimal.GetBodyPartMesh(PartType.LeftArm);
        leftArm.material = lefArmAnimal.material;
        rightArm.sharedMesh = rightArmAnimal.GetBodyPartMesh(PartType.RightArm);
        rightArm.material = rightArmAnimal.material;
        leftLeg.sharedMesh = leftLegAnimal.GetBodyPartMesh(PartType.LeftLeg);
        leftLeg.material = leftLegAnimal.material;
        rightLeg.sharedMesh = rightLegAnimal.GetBodyPartMesh(PartType.RightLeg);
        rightLeg.material = rightLegAnimal.material;
    }
    
    public void Death()
    {
       head.material.color = Color.gray;
       hips.material.color = Color.gray;
       leftArm.material.color = Color.gray;
       rightArm.material.color = Color.gray;
       leftLeg.material.color = Color.gray;
       rightLeg.material.color = Color.gray;
    }

    public bool IsVisible(Camera cam)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(cam);
        var point = hips.transform.position;

        for (int i = 0; i < planes.Length; i++)
        {
            if (planes[i].GetDistanceToPoint(point) < 0)
            {
                return false;
            }
        }
        
        return true;
    }
}



public class Player : MonoBehaviour
{
    public static Player Instance {get; protected set;}
    [SerializeField] private new PlayerAnimation animation;
    [SerializeField] private PlayerBody body;
    [SerializeField] private Body bodyData;
    [SerializeField] private FieldOfView fieldOfView;
    [SerializeField] private Movement movement;
    [SerializeField] private PuppetMaster puppetMaster;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private Transform arrow;
    public ParticleSystem winEffect;
    private bool death;
    private List<Enemy> enemies = new List<Enemy>();
    private List<Transform> arrows = new List<Transform>();
    private BodyInfo info;
    private float healingTime;
    private float health, maxHealth;



    public void RandomAnimation() => animation.SetRandomAnimation();
    public void Dance() => animation.Dance();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        body.SetBody(bodyData);
        info = bodyData.powerInfo;
        health = maxHealth = info.health;
        movement.speed *= SetSpeed(info.speed); 
        RandomAnimation();
        var e = FindObjectsOfType<Enemy>();
        for (int i = 0; i < e.Length; i++)
        {
            enemies.Add(e[i]);
            arrows.Add(Instantiate(arrow,transform));
        }
        arrow.gameObject.SetActive(false);
    }

    private void Update()
    {
        animation.Player();
        if (GameManager.State != GameState.Play) return;
        FindEnemy();
        Healing();
        Arrows();
    }


    private void Healing()
    {
        healingTime -= Time.deltaTime;
        if (healingTime > 0) return;
        health = Mathf.Lerp(health, maxHealth, Time.deltaTime);
        healthBar.SetValue(health, maxHealth);
    }
    public float SetSpeed(float speed)
    {
        if (speed < 0.2) return 0.6f;
        if (speed < 0.4) return 0.7f;
        if (speed < 0.6) return 0.8f;
        if (speed < 0.8) return 0.9f;
        return 1;
    }
    
    
    private void FindEnemy()
    {
        animation.Attack(fieldOfView.visibleTargets.Count > 0 && !Input.GetMouseButton(0));
        if(Input.GetMouseButton(0)) return;
        if (fieldOfView.visibleTargets.Count <= 0) return;
        var target = new Vector3(fieldOfView.visibleTargets[0].position.x,transform.position.y,fieldOfView.visibleTargets[0].position.z);
        var dir = - transform.position + target;
        if (dir == Vector3.zero) return;
        transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(dir),6*Time.deltaTime);
    }

    public void GetHet(float damage)
    {
        healingTime = 5f;
        health -= damage / 5;
        healthBar.SetValue(health, maxHealth);
        if(health < 0)KillMe();
    }

    public void Attack()
    {
        if (fieldOfView.visibleTargets.Count <= 0) return;
        fieldOfView.visibleTargets[0].GetComponent<Enemy>().GetHit(info.damage);
    }

    private void KillMe()
    {
        if (death) return;
        death = true;
        puppetMaster.pinWeight = puppetMaster.muscleWeight = 0.2f;
        Destroy(gameObject.GetComponent<Rigidbody>());
        gameObject.GetComponent<Collider>().isTrigger = true;
        Destroy(healthBar.gameObject);
        animation.DisableAnimation();
        gameObject.layer = 0;
        movement.enabled = false;
        body.Death();
        for (int i = 0; i < enemies.Count; i++)
        {
            arrows[i].gameObject.SetActive(false);
        }
        GameManager.Instance.EndGame(false);
        enabled = false;
    }

    private void Arrows()
    {
        if (death) return;
        for (int i = 0; i < enemies.Count; i++)
        {
            arrows[i].gameObject.SetActive(!enemies[i].IsVisible() && enemies[i].enabled);
            var targetPoint = new Vector3(enemies[i].transform.position.x,arrows[i].position.y,enemies[i].transform.position.z);
            arrows[i].LookAt(targetPoint);
        }
    }
}
