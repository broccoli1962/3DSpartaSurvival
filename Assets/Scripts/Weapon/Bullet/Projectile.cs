using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float shootSpeed;
    public float lifeTime = 3.0f;
    public float power;
    public LayerMask hitMask;

    private Vector3 direction;

    public void Init(Vector3 dir, float power, float speed, LayerMask hitMask)
    {
        this.direction = dir;
        shootSpeed = speed;
        this.power = power;
        this.hitMask = hitMask;
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += direction * shootSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IDamagable>(out IDamagable hitObj))
        {
            hitObj.ValueChanged(-power);
        }
        Destroy(gameObject);
    }
}
