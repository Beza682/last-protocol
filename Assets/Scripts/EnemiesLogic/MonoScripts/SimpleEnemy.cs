using UnityEngine;

public class SimpleEnemy : BaseEnemy
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && gameObject.activeSelf)
        {
            UnityEngine.Debug.Log($"Attack");
            Action?.Invoke();
        }
    }
}
