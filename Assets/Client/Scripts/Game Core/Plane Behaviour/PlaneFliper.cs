using UnityEngine;


    [RequireComponent(typeof(SpriteRenderer))]
    public class PlaneFliper : MonoBehaviour
    {
        [SerializeField]
        private Transform targetTransform;

        private SpriteRenderer _planeSkin;

        [SerializeField] 
        private Transform collidersPlane;

        private void Awake()
        {
            _planeSkin = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            TurnChecker();
        }

        private void TurnChecker()
        {
            var rotation = targetTransform.rotation;
            if (rotation.eulerAngles.z > 100 && rotation.eulerAngles.z < 290)
            {
                ChangeFlip(true);
                return;
            }

            ChangeFlip(false);
        }

        private void ChangeFlip(bool newFlipY)
        {
            if(newFlipY == _planeSkin.flipY) return;
            
            if (_planeSkin.flipY == false)
            {
                _planeSkin.flipY = true;
                collidersPlane.localEulerAngles = new Vector3(180,0,0);
            }
            else
            {
                _planeSkin.flipY = false;
                collidersPlane.localEulerAngles = Vector3.zero;
            }
        }
        
    }