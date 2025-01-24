using UnityEngine;

public class MonsterBehavior : MonoBehaviour
{
    public Transform target; // Référence au joueur ou à la cible
    public float walkSpeed = 2f; // Vitesse de marche
    public float runSpeed = 5f; // Vitesse de course
    public float flySpeed = 7f; // Vitesse en vol
    public float attackRange = 2f; // Portée d'attaque
    public float walkRange = 8f; // Portée pour marcher
    public float detectionRange = 15f; // Portée de détection maximale

    private Animator animator; // Référence à l'Animator
    private bool isFlying = false; // Indique si le monstre est en vol
    private bool isLanding = false; // Indique si le monstre est en train d'atterrir

    void Start()
    {
        animator = GetComponent<Animator>();

        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
            else
            {
                Debug.LogError("Aucun joueur trouvé avec le tag 'Player'.");
            }
        }
    }

    void Update()
    {
        if (target == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget <= attackRange)
        {
            Attack();
        }
        else if (distanceToTarget <= walkRange)
        {
            WalkTowardsTarget();
        }
        else if (distanceToTarget <= detectionRange)
        {
            if (isFlying)
            {
                FlyTowardsTarget();
            }
            else
            {
                RunTowardsTarget();
            }
        }
        else
        {
            Idle();
        }

        HandleLanding();
    }

    private void Idle()
    {
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsFlying", false);

        Debug.Log("Le monstre est en Idle.");
    }

    private void WalkTowardsTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * walkSpeed * Time.deltaTime;

        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));

        animator.SetBool("IsWalking", true);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsFlying", false);

        Debug.Log("Le monstre marche vers le joueur.");
    }

    private void RunTowardsTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * runSpeed * Time.deltaTime;

        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));

        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", true);
        animator.SetBool("IsFlying", false);

        Debug.Log("Le monstre court vers le joueur.");
    }

    private void FlyTowardsTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * flySpeed * Time.deltaTime;

        transform.LookAt(target);

        animator.SetBool("IsFlying", true);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsWalking", false);
        animator.SetTrigger("FlyForward");

        Debug.Log("Le monstre vole vers le joueur.");
    }

    private void Attack()
    {
        int attackType = Random.Range(0, 2); // 0 = Basic Attack, 1 = Claw Attack
        if (attackType == 0)
        {
            animator.SetTrigger("Basic Attack");
            Debug.Log("Le monstre utilise Basic Attack.");
        }
        else
        {
            animator.SetTrigger("Claw Attack");
            Debug.Log("Le monstre utilise Claw Attack.");
        }
    }

    private void HandleLanding()
    {
        if (isFlying && !isLanding && Vector3.Distance(transform.position, target.position) > detectionRange)
        {
            animator.SetTrigger("Land");
            isLanding = true;
            Debug.Log("Le monstre atterrit.");
        }

        if (isLanding && animator.GetCurrentAnimatorStateInfo(0).IsName("Land"))
        {
            isFlying = false;
            isLanding = false;
        }
    }

    public void TakeOff()
    {
        if (!isFlying)
        {
            animator.SetTrigger("Take Off");
            isFlying = true;
            Debug.Log("Le monstre décolle.");
        }
    }

    public void GetHit()
    {
        animator.SetTrigger("Get Hit");
        Debug.Log("Le monstre reçoit des dégâts.");
    }
}