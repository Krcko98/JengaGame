using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JengaGame.CameraNS
{
    public class CameraOrbit : MonoBehaviour
    {
        [SerializeField]
        private Transform focusObject;
        [SerializeField]
        private bool orbitEnabled = true;
        [SerializeField]
        private KeyCode startOrbitingKey = KeyCode.Mouse0;
        [SerializeField]
        private bool isOrbiting = false;
        [SerializeField]
        private float orbitSpeed = 1f;
        [SerializeField]
        private float followSpeed = 1;

        //Clamping
        [SerializeField]
        private float maxVerticalAngle = 90f;
        [SerializeField]
        private float minVerticalAngle = 0f;
        [SerializeField]
        private float maxHorizontalAngle = 360f;
        [SerializeField]
        private float minHorizontalAngle = 0f;

        public bool OrbitEnabled { get => orbitEnabled; set => orbitEnabled = value; }

        [SerializeField]        
        private Vector2 orbitValue;
        private float localRotatitonY = 0;

        private void Update()
        {
            setFocusPosition();

            if (!OrbitEnabled) return;

            getInput();
            orbitCamera();
        }

        private void setFocusPosition()
        {
            if (focusObject == null) return;
            
            transform.position = Vector3.Lerp(transform.position, focusObject.position, Time.deltaTime * followSpeed);
        }

        private void getInput()
        {
            if(Input.GetMouseButtonDown(0))
            {
                isOrbiting = true;
            }
            else if(Input.GetMouseButtonUp(0))
            {
                isOrbiting = false;
            }
        }

        private void orbitCamera()
        {
            if (!isOrbiting) return;

            orbitValue.x = Input.GetAxis("Mouse X");
            orbitValue.y = Input.GetAxis("Mouse Y");

            float localRotationX = transform.localEulerAngles.y + orbitValue.x * orbitSpeed;
            localRotatitonY -= orbitValue.y * orbitSpeed;

            localRotatitonY = Mathf.Clamp(localRotatitonY, minVerticalAngle, maxVerticalAngle);

            transform.localEulerAngles = new Vector3(localRotatitonY, localRotationX, 0);
        }

        public void SetFocusObject(in Transform focusObject)
        {
            this.focusObject = focusObject;
        }
    }
}
