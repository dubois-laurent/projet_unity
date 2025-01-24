using UnityEngine;

public class MonsterMouvement : MonoBehaviour
{
    public Transform shadow; // Référence au joueur
    public float walkSpeed = 3f; // Vitesse de marche
    public float runSpeed = 7f; // Vitesse de course
    public float flySpeed = 10f; // Vitesse en vol
    public float attackRange = 2f; // Portée d'attaque
    public float walkRange = 8f; // Portée pour marcher
    public float detectionRange = 15f; // Portée de détection maximale
    public float screamIntervalMin = 5f; // Intervalle minimum pour crier
    public float screamIntervalMax = 15f; // Intervalle maximum pour crier
    public AudioClip screamSound; // Son de cri du monstre
    public bool isFlying = false; // Indique si le joueur vole (à remplacer selon votre logique)

    private Animator animator; // Référence à l'Animator pour les animations
    private AudioSource audioSource; // Source audio pour les cris
    private float nextScreamTime; // Temps pour le prochain cri

    private void Start()
    {
        // Trouver le joueur automatiquement si non assigné
        if (shadow == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("shadow");
            if (playerObject != null)
            {
                shadow = playerObject.transform;
            }
            else
            {
                Debug.LogError("Aucun objet avec le tag 'Player' trouvé !");
            }
        }

        // Initialisation des composants
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        ScheduleNextScream();
    }

    private void Update()
    {
        if (shadow == null) return;

        // Calculer la distance au joueur
        float distanceToPlayer = Vector3.Distance(transform.position, shadow.position);

        // Comportement en fonction de la distance
        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer <= walkRange)
        {
            WalkTowardsPlayer();
        }
        else if (distanceToPlayer <= detectionRange)
        {
            if (isFlying || shadow.position.y > transform.position.y + 2f) // Vérifie si le joueur vole
            {
                FlyTowardsPlayer();
            }
            else
            {
                RunTowardsPlayer();
            }
        }

        // Faire crier le monstre de temps en temps
        HandleScreaming();
    }

    private void AttackPlayer()
    {
        // Jouer une animation d'attaque
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        Debug.Log("Le monstre attaque le joueur !");
    }

    private void WalkTowardsPlayer()
    {
        Vector3 direction = (shadow.position - transform.position).normalized;
        transform.position += direction * walkSpeed * Time.deltaTime;

        // Faire face au joueur
        transform.LookAt(new Vector3(shadow.position.x, transform.position.y, shadow.position.z));

        // Jouer une animation de marche
        if (animator != null)
        {
            animator.SetBool("Walk", true);
            animator.SetBool("Run", false);
            animator.SetBool("Fly Forward", false);
        }

        Debug.Log("Le monstre marche vers le joueur.");
    }

    private void RunTowardsPlayer()
    {
        Vector3 direction = (shadow.position - transform.position).normalized;
        transform.position += direction * runSpeed * Time.deltaTime;

        // Faire face au joueur
        transform.LookAt(new Vector3(shadow.position.x, transform.position.y, shadow.position.z));

        // Jouer une animation de course
        if (animator != null)
        {
            animator.SetBool("Run", true);
            animator.SetBool("Walk", false);
            animator.SetBool("Fly Forward", false);
        }

        Debug.Log("Le monstre court vers le joueur.");
    }

    private void FlyTowardsPlayer()
    {
        Vector3 direction = (shadow.position - transform.position).normalized;
        transform.position += direction * flySpeed * Time.deltaTime;

        // Faire face au joueur
        transform.LookAt(shadow);

        // Jouer une animation de vol
        if (animator != null)
        {
            animator.SetBool("Fly Forward", true);
            animator.SetBool("Run", false);
            animator.SetBool("Walk", false);
        }

        Debug.Log("Le monstre vole vers le joueur.");
    }

    private void HandleScreaming()
    {
        if (Time.time >= nextScreamTime)
        {
            if (audioSource != null && screamSound != null)
            {
                audioSource.PlayOneShot(screamSound);
            }

            Debug.Log("Le monstre crie !");
            ScheduleNextScream();
        }
    }

    private void ScheduleNextScream()
    {
        nextScreamTime = Time.time + Random.Range(screamIntervalMin, screamIntervalMax);
    }