using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileChase : MonoBehaviour
{
    public float shootSpeed;
    public float lifeTime = 3.0f;
    public float power;
    public float delayShoot = 2.0f;
    public float turnSpeed = 5.0f;
    public LayerMask hitMask;

    private Transform target;
    private Vector3 direction;
    private float hormingStartTime;

    public void Init(Transform target, float power, float speed, LayerMask hitMask)
    {
        this.target = target;
        shootSpeed = speed;
        this.power = power;
        this.hitMask = hitMask;
        hormingStartTime = Time.time + delayShoot;
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (target != null && Time.time >= hormingStartTime)
        {
            direction = (target.position - transform.position).normalized;

            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = rotation;
            //transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, Time.deltaTime * turnSpeed); //추적이 제대로 안됨
        }
        transform.position += transform.forward * shootSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamagable>(out IDamagable hitObj))
        {
            hitObj.ValueChanged(-power);
            Destroy(gameObject);
        }
    }
}
