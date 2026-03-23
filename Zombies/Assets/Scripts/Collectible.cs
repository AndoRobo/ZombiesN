using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int points = 1;

    // Detects when a zombie touches the collectible.
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Zombie"))
            return;

        GameManager manager = FindObjectOfType<GameManager>();
        if (manager != null)
            manager.CollectCollectible(gameObject, points);
    }
}
