using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bulletPrefab;

    private BulletPool _bulletsBulletPool;
    private Camera _camera;
    private bool _canShoot;

    public void EnableShoot() => _canShoot = true;
    
    public void DisableShoot() => _canShoot = false;
    
    private void Awake()
    {
        _camera = Camera.main;
        _bulletsBulletPool = new BulletPool(bulletPrefab, 30);
    }

    private void Update()
    {
        if (_canShoot && Input.GetMouseButtonDown(0))
        {
            var rayDir = _camera.ScreenPointToRay(Input.mousePosition).direction;
            var r = new Ray(_camera.transform.position, rayDir);
            
            if (Physics.Raycast(r, out var hit))
            {
                var spPos = shootPoint.position;
                var diff = hit.point - spPos;
                var rotateY = Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg;
                var rotateX = Mathf.Atan2(diff.y, diff.z) * Mathf.Rad2Deg;
                //var bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.Euler(rotateX * -1f, rotateY, 0f));
                var bullet = _bulletsBulletPool.GetFreeElement();
                bullet.transform.position = spPos;
                bullet.transform.rotation = Quaternion.Euler(rotateX * -1f, rotateY, 0f);
                bullet.Move();
            }
        }
    }
}
