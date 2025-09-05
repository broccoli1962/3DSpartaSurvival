using UnityEngine;

public class Damagable : MonoBehaviour
{
    public float damagePerSecond = 10f;
    public float damageInterval = 0.5f;
    private float lastDamageTime;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            if (Time.time > lastDamageTime + damageInterval)
            {
                Player player = other.GetComponent<Player>();
                if (player != null)
                {
                    float damageToDeal = damagePerSecond * damageInterval;
                    player.TakeDamage((int)damageToDeal);
                }
                lastDamageTime = Time.time;
            }
        }
    }
}