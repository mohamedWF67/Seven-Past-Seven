using System;
using System.Collections;
using UnityEngine;

public class EnemyShootingScript : MonoBehaviour
{

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawn;
    [SerializeField] float fireRate;
    [SerializeField] float bulletSpeed;
    [SerializeField] int damage;
    [SerializeField] private float range;

    private LayerMask playerLayer;
    private Coroutine shootingCoroutine;

    private void Start()
    {
        playerLayer = LayerMask.GetMask("Player");
        shootingCoroutine = StartCoroutine(Shoot());
    }

    private void Update()
    {
        bool isPlayerInRange = Physics2D.OverlapCircle(bulletSpawn.position, range, playerLayer);
        if (isPlayerInRange && shootingCoroutine == null)
        {
            shootingCoroutine = StartCoroutine(Shoot());
        }else if (!isPlayerInRange && shootingCoroutine != null)
        {
            StopCoroutine(Shoot());
            shootingCoroutine = null;
        }
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            var bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
            var player = GameObject.FindGameObjectWithTag("Player");
            Vector2 dir = player.transform.position - bulletSpawn.position;
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = dir.normalized * bulletSpeed;
            bullet.GetComponent<EnemyBulletScript>().damage = damage;
            yield return new WaitForSeconds(fireRate);
        }
    }
}
