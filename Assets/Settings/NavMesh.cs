using UnityEngine;

public class MonsterFollow : MonoBehaviour
{
    public Transform shadow; // Référence au joueur
    public float speed = 3f; // Vitesse de déplacement du monstre
    public float detectionRange = 100f; // Portée de détection du joueur

    private void Update()
    {
        if (shadow != null)
        {
            // Calculer la distance entre le monstre et le joueur
            float distanceToPlayer = Vector3.Distance(transform.position, shadow.position);

            // Si le joueur est dans la portée de détection
            if (distanceToPlayer <= detectionRange)
            {
                // Déplacer le monstre vers le joueur
                Vector3 direction = (shadow.position - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;

                // Optionnel : Faire face au joueur
                transform.LookAt(shadow);
            }
        }
    }

    private void Start()
    {
        // Si le joueur n'est pas assigné dans l'inspecteur, chercher automatiquement par tag
        if (shadow == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                shadow = playerObject.transform;
            }
            else
            {
                Debug.LogError("Aucun objet avec le tag 'Player' trouvé !");
            }
        }
    }
}