using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public float lerpSpeed = 2.0f;
        public float teleportThreshold = 6.1f; // DEBUG sprint speed = 5.0f

        private Vector3 offset;

        private Vector3 targetPos;

        private void Start()
        {
            if (target == null) return;

            offset = transform.position - target.position;
        }

        private void Update()
        {
            if (target == null) return;
            targetPos = target.position + offset;
            if (Vector3.Distance(transform.position, targetPos) > teleportThreshold)
            {
                transform.position = targetPos; 
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
            }
        }

    }
}
