using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private Rigidbody[] ragdoll;
    [SerializeField] private Slider healthBar;
    [SerializeField] private int health;
    
    public delegate void EnemyHealthEventHandler();
    public static event EnemyHealthEventHandler OnEnemyKilled;

    private Animator _animator;
    private CapsuleCollider _capsuleCollider;
    private Vector3 _defaultPosition;
    private int _currentHealth;
    
    private void Awake()
    {
        _defaultPosition = transform.position;
        _animator = GetComponent<Animator>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        healthBar.value = healthBar.maxValue = _currentHealth = health;
        DisableRagdoll();
    }

    private void ApplyDamage()
    {
        _currentHealth--;
        if (_currentHealth > 0)
        {
            healthBar.value = _currentHealth;
        }
        else
        {
            healthBar.gameObject.SetActive(false);
            OnEnemyKilled?.Invoke();
            _animator.enabled = false;
            _capsuleCollider.enabled = false;
            EnableRagdoll();
            Invoke(nameof(Disable), 3f);
        }
    }

    private void Disable() => gameObject.SetActive(false);

    public void Revive()
    {
        DisableRagdoll();
        healthBar.gameObject.SetActive(true);
        _animator.enabled = true;
        _capsuleCollider.enabled = true;
        transform.position = _defaultPosition;
        healthBar.value = healthBar.maxValue = _currentHealth = health;
        gameObject.SetActive(true);
    }

    private void DisableRagdoll()
    {
        foreach (var rag in ragdoll)
            rag.isKinematic = true;
    }
    
    private void EnableRagdoll()
    {
        foreach (var rag in ragdoll)
            rag.isKinematic = false;
    }
}
