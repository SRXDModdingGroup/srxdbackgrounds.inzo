using SRXDBackgrounds.Common;
using UnityEngine;

namespace SRXDBackgrounds.Inzo {
    public class Inzo_Terrain : MonoBehaviour {
        private static readonly int WAVE_PHASE = Shader.PropertyToID("_Wave_Phase");
        private static readonly int MIDDLE_LIGHT_COLOR = Shader.PropertyToID("_Middle_Light_Color");
        private static readonly int BACK_LIGHT_COLOR = Shader.PropertyToID("_Back_Light_Color");
        private static readonly int BACK_LIGHT_DIRECTION = Shader.PropertyToID("_Back_Light_Direction");
        private static readonly int TOP_LIGHT_INTENSITY = Shader.PropertyToID("_Top_Light_Intensity");
        private static readonly int INTENSITY = Shader.PropertyToID("_Intensity");
        
        [SerializeField] private MeshRenderer terrainRenderer;
        [SerializeField] private ParticleSystemRenderer trianglesRenderer;
        [SerializeField] private float waveStartDistance;
        [SerializeField] private float waveEndDistance;
        [SerializeField] private float waveDuration;
        [SerializeField] private int middleLightSourceCount;
        [SerializeField] private float defaultTopLightIntensity;
        [SerializeField] private float maxTopLightIntensity;
        [SerializeField] private float trianglesBaseIntensity;
        [SerializeField] private float trianglesPulseIntensity;
        [SerializeField] private float trianglesPulseDuration;
        
        private EnvelopeBasic wavePhaseEnvelope;
        private EnvelopeInverted trianglePulseEnvelope;
        private Material terrainMaterial;
        private Material trianglesMaterial;
        private Color[] middleLightColors;

        private void Awake() {
            wavePhaseEnvelope = new EnvelopeBasic { Duration = waveDuration };
            trianglePulseEnvelope = new EnvelopeInverted { Duration = trianglesPulseDuration };
            terrainMaterial = terrainRenderer.material;
            trianglesMaterial = trianglesRenderer.material;
            middleLightColors = new Color[middleLightSourceCount];
            terrainMaterial.SetFloat(TOP_LIGHT_INTENSITY, defaultTopLightIntensity);
        }

        private void LateUpdate() {
            float deltaTime = Time.deltaTime;
            
            terrainMaterial.SetFloat(WAVE_PHASE, Mathf.Lerp(waveStartDistance, waveEndDistance, wavePhaseEnvelope.Update(deltaTime)));
            trianglesMaterial.SetFloat(INTENSITY, trianglesBaseIntensity + trianglesPulseIntensity * trianglePulseEnvelope.Update(deltaTime));
        }

        public void Wave() => wavePhaseEnvelope.Trigger();

        public void PulseTriangles() => trianglePulseEnvelope.Trigger();

        public void SetTopLightIntensity(float value) => terrainMaterial.SetFloat(TOP_LIGHT_INTENSITY, maxTopLightIntensity * value);

        public void SetMiddleLightColor(int index, Color color) {
            middleLightColors[index] = color;
            
            var middleLightColor = Color.black;

            foreach (var value in middleLightColors)
                middleLightColor += value;
            
            terrainMaterial.SetColor(MIDDLE_LIGHT_COLOR, middleLightColor);
        }

        public void SetBackLightColorAndDirection(Color color, Vector3 direction) {
            terrainMaterial.SetColor(BACK_LIGHT_COLOR, color);
            terrainMaterial.SetVector(BACK_LIGHT_DIRECTION, direction);
        }

        public void DoReset() {
            wavePhaseEnvelope.Reset();
            trianglePulseEnvelope.Reset();

            for (int i = 0; i < middleLightSourceCount; i++)
                middleLightColors[i] = Color.black;
            
            terrainMaterial.SetColor(MIDDLE_LIGHT_COLOR, Color.black);
            terrainMaterial.SetColor(BACK_LIGHT_COLOR, Color.black);
            terrainMaterial.SetFloat(TOP_LIGHT_INTENSITY, defaultTopLightIntensity);
        }
    }
}
