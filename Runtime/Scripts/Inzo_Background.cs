using UnityEngine;

namespace SRXDBackgrounds.Inzo {
    public class Inzo_Background : MonoBehaviour {
        private static readonly int INTENSITY = Shader.PropertyToID("_Intensity");

        [SerializeField] private MeshRenderer renderer;
        [SerializeField] private ParticleSystemRenderer rainRenderer;
        [SerializeField] private float defaultIntensity;
        [SerializeField] private float maxIntensity;
        [SerializeField] private float maxRainIntensity;

        private Material material;
        private Material rainMaterial;

        private void Awake() {
            material = renderer.material;
            rainMaterial = rainRenderer.trailMaterial;
            SetIntensity(defaultIntensity);
        }

        public void SetIntensity(float value) {
            material.SetFloat(INTENSITY, maxIntensity * value);
            rainMaterial.SetFloat(INTENSITY, maxRainIntensity * value);
        }

        public void DoReset() => SetIntensity(defaultIntensity);
    }
}