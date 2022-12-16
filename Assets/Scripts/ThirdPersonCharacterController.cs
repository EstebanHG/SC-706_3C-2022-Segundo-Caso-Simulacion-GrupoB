using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StarterAssets;
using UnityEngine;

public class ThirdPersonCharacterController : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera aimVirtualCamera;

    [SerializeField]
    float normalSens = 1.0F;

    [SerializeField]
    float aimSens = 0.5F;

    [SerializeField]
    LayerMask aimMask;

    [SerializeField]
    float crosshairRotationSpeed;

    [SerializeField]
    float aimInterpolation;

    [SerializeField]
    GameObject projectilePrefab;

    [SerializeField]
    Transform spawnPoint;

    Animator animator;

    StarterAssetsInputs inputs;

    ThirdPersonController thirdPersonController;


    void Awake() 
    {
        animator = GetComponent<Animator>();
        inputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
    }

    void Update() 
    {
        bool isAiming = inputs.aim;
        float sensitivity = 
            !isAiming
                ? normalSens
                : aimSens;

        float weight = 
            isAiming
                ? Mathf.Lerp
                    (animator.GetLayerWeight(1), 1.0F, aimInterpolation * Time.deltaTime )
                : Mathf.Lerp 
                    (animator.GetLayerWeight(1), 0.0F, aimInterpolation * Time.deltaTime );

        aimVirtualCamera.gameObject.SetActive(isAiming);
        thirdPersonController.SetSensitivityRate(sensitivity);
        thirdPersonController.SetRotationOnMove(!isAiming);
        animator.SetLayerWeight(1, weight);

        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        Transform hitObject = null;

        if(Physics.Raycast(ray, out RaycastHit raycastHit, 1000.0F, aimMask))
        {
            mouseWorldPosition = raycastHit.point;
            hitObject = raycastHit.transform;
        }

        if(isAiming)
        {
            Vector3 aimTarget = mouseWorldPosition;
            aimTarget.y = transform.position.y;
            Vector3 aimDirection = (aimTarget  - transform.position).normalized;
            transform.forward = 
                Vector3.Lerp(transform.forward, aimDirection, crosshairRotationSpeed * Time.deltaTime);
            
        }

        if(inputs.attack)
        {
            Vector3 projectileDirection = (mouseWorldPosition - spawnPoint.transform.position).normalized;

            Instantiate
                (projectilePrefab, spawnPoint.position, 
                    Quaternion.LookRotation(projectileDirection, Vector3.up));
            
            inputs.attack = false;
        }


    }
}
