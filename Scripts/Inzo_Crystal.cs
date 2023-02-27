using UnityEngine;

namespace SRXDBackgrounds.Inzo {
    public class Inzo_Crystal : MonoBehaviour {
        private static readonly int INTENSITY = Shader.PropertyToID("_Intensity");
        private static readonly int STARS_INTENSITY = Shader.PropertyToID("_Stars_Intensity");
        private static readonly int LIGHT_INTENSITY = Shader.PropertyToID("_Light_Intensity");
        private static readonly int FRESNEL_INTENSITY = Shader.PropertyToID("_Fresnel_Intensity");
        
        [SerializeField] private MeshRenderer innerRenderer;
        [SerializeField] private MeshRenderer outerRenderer;
        [SerializeField] private ParticleSystem particleSystem;

        private Material innerMaterial;
        private Material outerMaterial;

        private void Awake() {
            innerMaterial = innerRenderer.material;
            outerMaterial = outerRenderer.material;
        }

        public void SetInnerIntensity(float intensity) => innerMaterial.SetFloat(INTENSITY, intensity);
        
        public void SetOtherIntensity(float starsIntensity, float lightIntensity, float fresnelIntensity) {
            innerMaterial.SetFloat(STARS_INTENSITY, starsIntensity);
            outerMaterial.SetFloat(LIGHT_INTENSITY, lightIntensity);
            outerMaterial.SetFloat(FRESNEL_INTENSITY, fresnelIntensity);
        }

        public void TriggerParticleSystem() => particleSystem.Play();

        public void DoReset() => particleSystem.Clear();
    }
}