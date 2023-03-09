using SRXDBackgrounds.Common;
using UnityEngine;

namespace SRXDBackgrounds.Inzo {
    public class Inzo_Crystals : MonoBehaviour {
        [SerializeField] private Inzo_Crystal[] crystals;
        [SerializeField] private float envelopeDuration;
        [SerializeField] private float maxEnvelopeIntensity;

        private EnvelopeBasic envelope;

        private void Awake() => envelope = new EnvelopeBasic {
            Duration = envelopeDuration,
            Invert = true,
            InterpolationType = InterpolationType.EaseOut
        };

        private void LateUpdate() {
            float deltaTime = Time.deltaTime;
            float value = maxEnvelopeIntensity * envelope.Update(deltaTime);

            foreach (var crystal in crystals)
                crystal.SetInnerIntensity(value);
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