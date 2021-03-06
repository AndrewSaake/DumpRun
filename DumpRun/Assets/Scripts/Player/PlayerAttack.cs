using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] projectiles;
    private Animator anim;
    private Player player;

    public Transform attackPos;
    public LayerMask enemyMask;
    public LayerMask enemyProj;
    public float meleeRange;
    public float meleeDamage;

    private float meleeTimeBetweenAttack;
    public float meleeStartTimeBetweenAttack;

    private float meleeTime;
    private float timeSinceLastMelee;

    [SerializeField] private float cooldownTimer = Mathf.Infinity;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.isPaused)
        {
            return;
        }
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown)
        {
            Attack();
        }

        //melee attack system
        if (meleeTimeBetweenAttack <= 0)
        {
            player.animator.SetBool("MeleePressed", false);
            if (Input.GetMouseButton(1))
            {
                Debug.Log("MeleePressed");
                player.animator.SetBool("MeleePressed", true);
               
                meleeTimeBetweenAttack = meleeStartTimeBetweenAttack;
            }
            //player.animator.SetBool("MeleePressed", false);


        }
        else
        {
            meleeTimeBetweenAttack -= Time.deltaTime;
        }
        cooldownTimer += Time.deltaTime;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, meleeRange);
    }

    public void PlayerMelee()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, meleeRange, enemyMask);
        for (int i = 0; i < enemies.Length; i++)
        {
            try
            {
                enemies[i].GetComponent<MeleeEnemy>().TakeDamage(meleeDamage);
            }
            catch
            {
                enemies[i].GetComponent<RangedEnemy>().TakeDamage(meleeDamage);
            }
            Debug.Log("MeleeAttack");

        }
        Collider2D[] proj = Physics2D.OverlapCircleAll(attackPos.position, meleeRange, enemyProj);
        for (int i = 0; i < proj.Length; i++)
        {
            try
            {
                proj[i].GetComponent<EnemyProjectile>().Deactivate();
                player.incramentTrash(1);
            }
            catch
            {
               
            }
            Debug.Log("MeleeAttack");

        }
    }

    private void Attack()
    {
        //play animation for attack
        cooldownTimer = 0;
        player.PlayWindex();
        //int bullet = FindFireball();
        projectiles[FindFireball()].transform.position = firePoint.position;
        projectiles[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));

       
    }

    private int FindFireball()
    {
        for (int i =0; i < projectiles.Length; i ++)
        {
            if (!projectiles[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }
}
