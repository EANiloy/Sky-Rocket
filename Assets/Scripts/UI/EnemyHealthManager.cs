using UnityEngine;
using UnityEngine.UI;

namespace SpaceShip.UI
{
    public class EnemyHealthManager : MonoBehaviour
    {
        [SerializeField] private Sprite[] _livesSprite;
        [SerializeField] private Image _livesImage;

        public void UpdateLives(int currentLives)
        {
            if(currentLives>=0)
            {
                _livesImage.sprite = _livesSprite[currentLives];
            }
        }
    }
}
