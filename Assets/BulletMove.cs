using System;
using UnityEngine;
using System.Collections;

public class BulletMove : MonoBehaviour {
        
        [SerializeField] private float timeToDestruct = 10;
        [SerializeField] private int startSpeed = 50;
        //[SerializeField] private GameObject particleHit;

        private TrailRenderer _trailRenderer;
        private Rigidbody _rigidbody;
        private Vector3 _previousStep;
        private float _currentDamage;
       
        void Awake ()
        {
                //Destroy(gameObject,TimeToDestruct);
                _trailRenderer = GetComponent<TrailRenderer>();
                _rigidbody = GetComponent<Rigidbody>();
        }
        

        public void Move()
        {
                Invoke(nameof(Disable),timeToDestruct);
                _rigidbody.velocity = transform.TransformDirection(Vector3.forward * startSpeed);
                _previousStep = transform.position;
                _trailRenderer.enabled = true;
        }
        
        void FixedUpdate()
        {
                var currentStep = gameObject.transform.rotation;
               
                transform.LookAt (_previousStep, transform.up);
                var distance = Vector3.Distance (_previousStep, transform.position);
                
                if (distance == 0.0f) 
                        distance = 1e-05f;
                
                if(Physics.Raycast(_previousStep, 
                           transform.TransformDirection(Vector3.back), 
                           out var hit, distance * 0.9999f) 
                   && (hit.transform.gameObject != gameObject))
                {
                        //Instantiate(particleHit, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                        _trailRenderer.enabled = false;
                        hit.transform.gameObject.SendMessage("ApplyDamage", SendMessageOptions.DontRequireReceiver);
                }
               
                gameObject.transform.rotation = currentStep;
                _previousStep = gameObject.transform.position;
        }

        void Disable() => gameObject.SetActive(false);
}