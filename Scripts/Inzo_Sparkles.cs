using SRXDBackgrounds.Common;
using UnityEngine;

namespace SRXDBackgrounds.Inzo {
    public class Inzo_Sparkles : MonoBehaviour {
        [SerializeField] private ParticleSystem particleSystem;
        [SerializeField] private Inzo_Terrain terrain;
        [SerializeField] private Color colorToTerrain;
        [SerializeField] private float terrainOscillatorSpeed;
        [SerializeField] private float terrainOscillatorStartPhase;
        [SerializeField] private float terrainOscillatorMin;
        [SerializeField] private float terrainSmootherUpward;
        [SerializeField] private float terrainSmootherDownward;

        private bool playing;
        private Oscillator terrainLightOscillator;
        private Filter terrainLightFilter;

        private void Awake() {
            terrainLightOscillator = new Oscillator {
                Speed = terrainOscillatorSpeed,
                Type = OscillatorType.Sine
            };
            terrainLightFilter = new Filter {
                RateUpward = terrainSmootherUpward,
                RateDownward = terrainSmootherDownward
            };
        }

        private void LateUpdate() {
            float deltaTime = Time.deltaTime;
            float oscillatorValue = terrainLightOscillator.Update(deltaTime);
            float target = playing ? Mathf.Lerp(terrainOscillatorMin, 1f, oscillatorValue) : 0f;
            
            terrain.SetMiddleLightColor(1, terrainLightFilter.Update(target, deltaTime) * colorToTerrain);
        }

        public void Play() {
            var emission = particleSystem.emission;

            emission.enabled = true;
            playing = true;
            terrainLightOscillator.SetPhase(terrainOscillatorStartPhase);
        }

        public void Stop() {
            var emission = particleSystem.emission;

            emission.enabled = false;
            playing = false;
        }

        public void DoReset() {
            var emission = particleSystem.emission;

            emission.enabled = false;
            playing = false;
            particleSystem.Clear();
        }
    }
}
