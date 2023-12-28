using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    [System.Serializable]
    public class Part
    {
        [SerializeField]
        public Image Target; 

        [SerializeField]
        public Sprite DamagedSprite;
    }

    [SerializeField]
    private Shader shader;

    [SerializeField]
    private List<Part> parts;

    private void Start()
    {
        foreach (var part in parts)
        {
            var material = new Material(shader);
            material.SetTexture("_DamagedTex", part.DamagedSprite.texture);

            part.Target.material = material;
        }
    }
}
