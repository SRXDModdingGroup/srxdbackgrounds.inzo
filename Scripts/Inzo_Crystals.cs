using SRXDBackgrounds.Common;
using UnityEngine;

namespace SRXDBackgrounds.Inzo {
    public class Inzo_Crystals : MonoBehaviour {
        [SerializeField] private Inzo_Crystal[] crystals;
        [SerializeField] private float envelopeDuration;
        [SerializeField] private float maxEnvelopeIntensity;

        private EnvelopeInverted envelope;

        private void Awake() => envelope = new EnvelopeInverted { Duration = envelopeDuration };

        private void LateUpdate() {
            float deltaTime = Time.deltaTime;
            float value = envelope.Update(deltaTime);
            float intensity = maxEnvelopeIntensity * value * value;

            foreach (var crystal in crystals)
                crystal.SetInnerIntensity(intensity);
        }

        public void Trigger() {
            envelope.Trigger();

            foreach (var crystal in crystals)
                crystal.TriggerParticleSystem();
        }

        public void DoReset() {
            envelope.Reset();

            foreach (var crystal in crystals)
                crystal.DoReset();
        }
    }
}