using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterMovementBase : MonoBehaviour
    {
        protected Animator characterAnimator;
        protected CharacterInputSystem inputSystem;
        protected CharacterController control;

        //MoveDirection 
        protected Vector3 movementDirection;
        protected Vector3 veriticalDirection;

        [SerializeField, Header("Move speed")] protected float characterGravity;
        [SerializeField] protected float characterCurrentMoveSpeed;
        protected float characterFallTime = 0.15f;
        protected float characterFallOutDeltaTime;
        protected float verticalSpeed;
        protected float maxVerticalSpeed = 53f;

        [SerializeField, Header("Ground Detect")]
        protected float groundDetectionRang;

        [SerializeField] protected float groundDetectionOffset;
        [SerializeField] protected float slopRayExtent;
        [SerializeField] protected LayerMask whatIsGround;

        [SerializeField, Tooltip("角色动画移动时检测障碍物的层级")]
        protected LayerMask whatIsObstacle;

        [SerializeField] protected bool isOnGround;


        protected virtual void Awake()
        {
            characterAnimator = GetComponentInChildren<Animator>();
            inputSystem = GetComponent<CharacterInputSystem>();
            control = GetComponent<CharacterController>();
            
        }

        protected virtual void Start()
        {
            characterFallOutDeltaTime = characterFallTime;
        }

        protected virtual void Update()
        {
            CheckOnGround();
        }

        /// <summary>
        /// 角色重力
        /// </summary>
        private void CharacterGravity()
        {
            if (isOnGround)
            {
                characterFallOutDeltaTime = characterFallTime;

                if (verticalSpeed < 0.0f)
                {
                    verticalSpeed = -2f;
                }
            }
            else
            {
                if (characterFallOutDeltaTime >= 0.0f)
                {
                    characterFallOutDeltaTime -= Time.deltaTime;
                    characterFallOutDeltaTime = Mathf.Clamp(characterFallOutDeltaTime, 0, characterFallTime);
                }
            }

            if (verticalSpeed < maxVerticalSpeed)
            {
                verticalSpeed += characterGravity * Time.deltaTime;
            }
        }
        
        /// <summary>
        /// 地面检测
        /// </summary>
        private void CheckOnGround()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundDetectionOffset, transform.position.z);
            isOnGround = Physics.CheckSphere(spherePosition, groundDetectionRang, whatIsGround, QueryTriggerInteraction.Ignore);
            
        }
        
        private void OnDrawGizmosSelected()
        {
            
            if (isOnGround) 
                Gizmos.color = Color.green;
            else 
                Gizmos.color = Color.red;

            Vector3 position = Vector3.zero;
            
            position.Set(transform.position.x, transform.position.y - groundDetectionOffset,
                transform.position.z);

            Gizmos.DrawWireSphere(position, groundDetectionRang);
        }

        protected Vector3 ResetMoveDirectionOnSlop(Vector3 dir)
        {
            if (Physics.Raycast(transform.position, -Vector3.up, out var hit, slopRayExtent))
            {
                float newAngle = Vector3.Dot(Vector3.up, hit.normal);
                if (newAngle != 0 && verticalSpeed <= 0)
                {
                    return OnPlaneProject(dir, hit.normal);
                }
            }
            return dir;
        }

        
        public Vector3 OnPlaneProject(Vector3 dir, Vector3 onNormal)
        {
            var dot = Vector3.Dot(dir, onNormal);
            return dir - (onNormal * dot);
        }

        /// <summary>
        /// Character movement 
        /// </summary>
        /// <param name="moveDir">move dir vector</param>
        /// <param name="speed">move speed</param>
        /// <param name="useGravity">use gravity?</param>
        public virtual void CharacterMovementInterface(Vector3 moveDir, float speed, bool useGravity)
        {
            moveDir = moveDir.normalized;
            moveDir = ResetMoveDirectionOnSlop(moveDir);
            
            //使用重力
            if (useGravity)
            {
                veriticalDirection.Set(0,verticalSpeed,0);
            }
            else
            {
                veriticalDirection = Vector3.zero;
            }

            control.Move(moveDir.normalized *(speed *Time.deltaTime) + veriticalDirection * Time.deltaTime);
        }
        
    }
