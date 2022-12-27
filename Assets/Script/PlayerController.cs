using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UGG.Move
{


   public class PlayerController : CharacterMovementBase
   {
      private Transform characterCamera;
      private TpsCamera _tpsCamera;

      [SerializeField] private Transform standCameraLook;
      [SerializeField] private Transform CrouchCamerLook;

      //Ref value
      private float targetRotation;
      private float rotationVelocity;

      //LerpTime
      [SerializeField, Header("Rotation speed")]
      private float rotationLerpTime;

      [SerializeField] private float moveDirectionSlerpTime;

      //Move Speed
      [SerializeField, Header("move Speed")] private float walkSpeed;
      [SerializeField, Header("move speed")] private float runSpeed;
      [SerializeField, Header("move speed")] private float crouchMoveSpeed;

      [SerializeField, Header("character 胶囊控制（下蹲）")]
      private Vector3 crouchCenter;

      [SerializeField] private Vector3 originCenter;
      [SerializeField] private Vector3 cameraLookPositionOnCrouch;
      [SerializeField] private Vector3 cameraLookPositionOrigin;
      [SerializeField] private Vector3 crouchHeight;
      [SerializeField] private float originHeight;
      [SerializeField] private bool isOnCrouch;
      [SerializeField] private Transform crouchDetectionPosition;
      [SerializeField] private Transform CameraLook;
      [SerializeField] private LayerMask crouchDetectionLayer;

      //animationID
      private int crouchID = Animator.StringToHash("Crouch");

      protected override void Awake()
      {
         base.Awake();
         characterCamera = Camera.main.transform.root.transform;
         _tpsCamera = characterCamera.GetComponent<TpsCamera>();
      }

      protected override void Start()
      {
         base.Start();
         cameraLookPositionOrigin = CameraLook.position;
      }

      protected override void Update()
      {
         base.Update();

         PlayerMoveDirection();

      }

      private bool CanMoveContro()
      {
         return true;
         // return isOnGround && characterAnimator.CheckAnimationTag("Motion") || characterAnimator.CheckAnimationTag("CrouchMotion");
      }
      
      private void PlayerMoveDirection()
      {
         //not input
         if (isOnGround && inputSystem.PlayerMovement == Vector2.zero)
         {
            movementDirection = Vector3.zero;
         }

         if (CanMoveContro())
         {
            if (inputSystem.PlayerMovement != Vector2.zero)
            {
               targetRotation =
                  Mathf.Atan2(inputSystem.PlayerMovement.x, inputSystem.PlayerMovement.y) * Mathf.Rad2Deg +
                  characterCamera.localEulerAngles.y;
               
               transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationLerpTime);

               var direction = Quaternion.Euler(0f, targetRotation, 0f) * Vector3.forward;
               direction = direction.normalized;

               movementDirection = Vector3.Slerp(movementDirection, ResetMoveDirectionOnSlop(direction), moveDirectionSlerpTime * Time.deltaTime );
            }
         }
         else
         {
            movementDirection = Vector3.zero;
         }

         control.Move((characterCurrentMoveSpeed * Time.deltaTime) * movementDirection.normalized + new Vector3(0,verticalSpeed,0) * Time.deltaTime);
         
      }



   }
}
