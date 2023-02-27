using SRXDBackgrounds.Common;
using UnityEngine;

namespace SRXDBackgrounds.Inzo {
    public class Inzo_Pyramid : MonoBehaviour {
        private static readonly int LIGHT_EFFECT_PHASE_1 = Shader.PropertyToID("_Light_Effect_Phase_1");
        private static readonly int LIGHT_EFFECT_PHASE_2 = Shader.PropertyToID("_Light_Effect_Phase_2");
        private static readonly int INTENSITY = Shader.PropertyToID("_Intensity");
        private static readonly int COLOR = Shader.PropertyToID("_Color");
        private static readonly int LIGHT_BASE_INTENSITY = Shader.PropertyToID("_Light_Base_Intensity");

        [SerializeField] private MeshRenderer pyramidBodyRenderer;
        [SerializeField] private MeshRenderer pyramidRimRenderer;
        [SerializeField] private float defaultLightBaseIntensity;
        [SerializeField] private float maxLightBaseIntensity;
        [SerializeField] private float lightEffectIntensity;
        [SerializeField] private float lightEffectStartPhase;
        [SerializeField] private float lightEffectEndPhase;
        [SerializeField] private float minLightEffectDuration;
        [SerializeField] private float maxLightEffectDuration;
        [SerializeField] private float lightOscillatorSpeed;
        [SerializeField] private float lightOscillatorMaxIntensity;
        [SerializeField] private float defaultRimBaseIntensity;
        [SerializeField] private float maxRimBaseIntensity;
        [SerializeField] private float rimEffectIntensity;
        [SerializeField] private float rimEffectDuration;
        [SerializeField] private Inzo_Terrain terrain;
        [SerializeField] private Color colorToTerrain;

        private ContinuousRotation continuousRotation;
        private EnvelopeBasic lightEffectPhaseEnvelope1;
        private EnvelopeBasic lightEffectPhaseEnvelope2;
        private EnvelopeInverted rimEnvelope;
        private OscillatorSquare lightOscillator;
        private Material pyramidBodyMainMaterial;
        private Material pyramidBodyNotchMaterial;
        private Material pyramidRimMaterial;
        private bool alternateLightEffect;
        private float lightOscillatorIntensity;
        private float lightBaseIntensity;
        private float rimBaseIntensity;

        private void Awake() {
            lightEffectPhaseEnvelope1 = new EnvelopeBasic();
            lightEffectPhaseEnvelope2 = new EnvelopeBasic();
            rimEnvelope = new EnvelopeInverted { Duration = rimEffectDuration };
            lightOscillator = new OscillatorSquare { Speed = lightOscillatorSpeed };
            pyramidBodyMainMaterial = pyramidBodyRenderer.materials[0];
            pyramidBodyNotchMaterial = pyramidBodyRenderer.materials[1];
            pyramidRimMaterial = pyramidRimRenderer.material;
            continuousRotation = GetComponent<ContinuousRotation>();
            lightBaseIntensity = defaultLightBaseIntensity;
            rimBaseIntensity = defaultRimBaseIntensity;
            RandomizeRimColor();
        }

        private void LateUpdate() {
            float deltaTime = Time.deltaTime;
            float envelope1Phase = lightEffectPhaseEnvelope1.Update(deltaTime);
            float envelope2Phase = lightEffectPhaseEnvelope2.Update(deltaTime);
            float totalLightBaseIntensity = lightBaseIntensity + lightOscillatorIntensity * lightOscillator.Update(deltaTime);
            
            pyramidBodyMainMaterial.SetFloat(
                LIGHT_EFFECT_PHASE_1,
                Mathf.Lerp(lightEffectStartPhase, lightEffectEndPhase, envelope1Phase));
            pyramidBodyMainMaterial.SetFloat(
                LIGHT_EFFECT_PHASE_2,
                Mathf.Lerp(lightEffectStartPhase, lightEffectEndPhase, envelope2Phase));
            pyramidBodyMainMaterial.SetFloat(LIGHT_BASE_INTENSITY, totalLightBaseIntensity);

            float rimPhase = rimEnvelope.Update(deltaTime);
            float rimIntensity = rimBaseIntensity + rimEffectIntensity * rimPhase * rimPhase * (3f - 2f * rimPhase);

            pyramidRimMaterial.SetFloat(INTENSITY, rimIntensity);
            pyramidBodyNotchMaterial.SetFloat(INTENSITY, rimIntensity);
            terrain.SetMiddleLightColor(0, (totalLightBaseIntensity + lightEffectIntensity * (Bell(2f * envelope1Phase) + Bell(2f * envelope2Phase))) * colorToTerrain);
        }

        public void LightEffect(float duration) {
            duration = Mathf.Lerp(minLightEffectDuration, maxLightEffectDuration, duration);
            
            if (alternateLightEffect) {
                lightEffectPhaseEnvelope1.Duration = duration;
                lightEffectPhaseEnvelope1.Trigger();
            }
            else {
                lightEffectPhaseEnvelope2.Duration = duration;
                lightEffectPhaseEnvelope2.Trigger();
            }

            alternateLightEffect = !alternateLightEffect;
        }

        public void RimEffect() => rimEnvelope.Trigger();

        public void RandomizeRimColor() => pyramidRimMaterial.SetColor(COLOR, Color.HSVToRGB(Random.value, 0.8f, 1f));

        public void SetLightBaseIntensity(float value) => lightBaseIntensity = maxLightBaseIntensity * value;

        public void SetLightOscillatorIntensity(float value) => lightOscillatorIntensity = lightOscillatorMaxIntensity * value;

        public void SetRimBaseIntensity(float value) => rimBaseIntensity = maxRimBaseIntensity * value;

        public void DoReset() {
            lightEffectPhaseEnvelope1.Reset();
            lightEffectPhaseEnvelope2.Reset();
            rimEnvelope.Reset();
            lightOscillator.SetPhase(0f);
            alternateLightEffect = false;
            lightOscillatorIntensity = 0f;
            lightBaseIntensity = defaultLightBaseIntensity;
            rimBaseIntensity = defaultRimBaseIntensity;
        }

        private static float Bell(float f) => Mathf.Max(0f, 1f - 4f * (f - 0.5f) * (f - 0.5f));
    }
}
