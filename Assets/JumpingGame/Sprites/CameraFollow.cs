using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.EndlessGame
{
    public class CameraFollow : MonoBehaviour
    {
        public static CameraFollow Ins;

        public Transform target;
        public Vector3 offset;
        [Range(15, 30)]
        public float smoothFactor;
        public float fastSmoothFactor = 2f;
        public float fastSmoothFactor1 = 4f;

        private void Awake()
        {
            Ins = this;
        }

        private void FixedUpdate()
        {
            Follow();
        }

        private void Follow()
        {
            if (target == null || target.transform.position.y < transform.position.y) return;

            Vector3 targetPos = new Vector3(0, target.transform.position.y, 0f) + offset;
            float currentSmoothFactor = smoothFactor;

            // Kiểm tra điểm số và tăng tốc độ di chuyển của camera nếu điểm trên 200
            if (GameManager.Ins && GameManager.Ins.Score > 200)
            {
                currentSmoothFactor = fastSmoothFactor;
            }
            float currentSmoothFactor1 = smoothFactor;
            if (GameManager.Ins && GameManager.Ins.Score > 400)
            {
                currentSmoothFactor1 = fastSmoothFactor1;
            }

            Vector3 smoothedPos = Vector3.Lerp(transform.position, targetPos, smoothFactor * Time.deltaTime);
            transform.position = new Vector3(
                Mathf.Clamp(smoothedPos.x, 0, smoothedPos.x),
                Mathf.Clamp(smoothedPos.y, 0, smoothedPos.y),
                -10f);
        }
    }
}
