using UnityEngine;

public class MultiLayerParallax : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform layer;       // Katman objesi
        public float parallaxFactor;  // Hareket hızı
        public float manualSizeX = 20f; // Manuel genişlik
        public float manualSizeY = 20f; // Manuel yükseklik
        [HideInInspector] public Vector3 startPos;
        [HideInInspector] public float textureUnitSizeX;
        [HideInInspector] public float textureUnitSizeY;
    }

    public Transform player;
    public ParallaxLayer[] layers;
    private Vector3 lastPlayerPosition;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player Transform atanmamış!");
            return;
        }

        lastPlayerPosition = player.position;

        foreach (var l in layers)
        {
            if (l.layer == null)
            {
                Debug.LogError("Layer Transform atanmamış!");
                continue;
            }

            l.startPos = l.layer.position;

            // SpriteRenderer kontrolü (sadece uyarı için)
            SpriteRenderer sr = l.layer.GetComponent<SpriteRenderer>();
            if (sr == null)
            {
                sr = l.layer.GetComponentInChildren<SpriteRenderer>();
            }

            if (sr != null && sr.sprite != null)
            {
                // Otomatik boyut hesaplama
                l.textureUnitSizeX = sr.sprite.texture.width / sr.sprite.pixelsPerUnit;
                l.textureUnitSizeY = sr.sprite.texture.height / sr.sprite.pixelsPerUnit;
            }
            else
            {
                // Manuel boyut kullan
                l.textureUnitSizeX = l.manualSizeX;
                l.textureUnitSizeY = l.manualSizeY;
                Debug.Log($"Layer {l.layer.name} için manuel boyut kullanılıyor: {l.manualSizeX}x{l.manualSizeY}");
            }
        }
    }

    // LateUpdate metodu aynı kalabilir...
}