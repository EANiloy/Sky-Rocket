using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShip.Core
{
    public class GalaxyController : MonoBehaviour
    {
        SpriteRenderer _spriteRenderer;
        Material _material;
        Vector2 _materialOffset;

        private void Start() {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _material = _spriteRenderer.material;
            _materialOffset = _material.mainTextureOffset;
        }
        void Update()
        {
            _materialOffset.y += Time.deltaTime * .05f;
            if(_materialOffset.y >=1f)
            {
                _materialOffset.y = 0f;
            }
            _material.mainTextureOffset = _materialOffset;
        }
    }

}