using SRXDBackgrounds.Common;
using UnityEngine;

namespace SRXDBackgrounds.Inzo {
    public class Inzo_Spotlight : MonoBehaviour {
        private static readonly int INTENSITY = Shader.PropertyToID("_Intensity");

        [SerializeField] private Transform rotationRoot;
        [SerializeField] private MeshRenderer spotlightRenderer;
        [SerializeField] private MeshRenderer auraRenderer;
        
        public EnvelopeADSR Envelope { get; set; }

        private Material spotlightMaterial;
        private Material auraMaterial;

        private void Awake() {
            spotlightMaterial = spotlightRenderer.material;
            auraMaterial = auraRenderer.material;
            Envelope = new EnvelopeADSR();
        }

        public void SetIntensity(float intensity) {
            spotlightMaterial.SetFloat(INTENSITY, intensity);
            auraMaterial.SetFloat(INTENSITY, intensity);
        }

        public void SetRotation(float rotation) => rotationRoot.localRotation = Quaternion.AngleAxis(rotation, Vector3.back);
    }
}