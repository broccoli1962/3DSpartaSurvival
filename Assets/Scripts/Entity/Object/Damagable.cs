// DamageZoneController.cs (디버깅용 버전)

using UnityEngine;

public class Damagable : MonoBehaviour
{
    public float damagePerSecond = 10f;
    public float damageInterval = 0.5f;
    private float lastDamageTime;

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Trigger 감지됨! 들어온 오브젝트: " + other.name);

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