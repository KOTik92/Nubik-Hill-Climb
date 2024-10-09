using UnityEngine;

namespace Game.Car
{
    public class ColliderIsGround : MonoBehaviour
    {
        internal bool IsGround;
    
        private void OnCollisionStay2D(Collision2D col)
        {
            if (col.gameObject.TryGetComponent(out LandSector landSector))
                IsGround = true;
        }

        private void OnCollisionExit2D(Collision2D col)
        {
            if (col.gameObject.TryGetComponent(out LandSector landSector))
                IsGround = false;
        }
    }
}
