using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private Transform[] wayPoints;

    public delegate void PlayerMoverEventHandler();
    public static event PlayerMoverEventHandler OnMovementDone;
    
    private NavMeshAgent _agent;
    private Animator _animator;
    private Vector3 _playerStartPoint;
    private Quaternion _playerStartRotation;
    private readonly Queue<Vector3> _wpQueue = new Queue<Vector3>();
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _playerStartPoint = transform.position;
        _playerStartRotation = transform.rotation;
        RestoreWayPoints();
    }

    private void Update()
    {
        if(!_animator.GetBool(IsRunning) && _agent.hasPath)
            _animator.SetBool(IsRunning, true);

        if(_animator.GetBool(IsRunning) && !_agent.hasPath)
        {
            _animator.SetBool(IsRunning, false);
            OnMovementDone?.Invoke();
        }
    }

    public void MoveForward() => _agent.SetDestination(_wpQueue.Dequeue());

    public void RestartPlayer()
    {
        _agent.ResetPath();
        _agent.enabled = false;
        
        var tf = transform;
        tf.position = _playerStartPoint;
        tf.rotation = _playerStartRotation;
        
        RestoreWayPoints();
        _agent.enabled = true;
    }
    
    private void RestoreWayPoints()
    {
        foreach (var wp in wayPoints)
            _wpQueue.Enqueue(wp.position);
    }
}
