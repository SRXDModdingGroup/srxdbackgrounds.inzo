using SRXDCustomVisuals.Core;
using UnityEngine;

namespace SRXDBackgrounds.Inzo {
    public class Inzo_Scene : MonoBehaviour {
        [SerializeField] private Inzo_Spotlights spotlights;
        [SerializeField] private Inzo_Pyramid pyramid;
        [SerializeField] private Inzo_Sparkles sparkles;
        [SerializeField] private Inzo_Crystals crystals;
        [SerializeField] private Inzo_Terrain terrain;
        [SerializeField] private Inzo_Background background;
        
        private VisualsEventReceiver eventReceiver;
        
        private void Awake() {
            eventReceiver = GetComponent<VisualsEventReceiver>();
            eventReceiver.On += EventOn;
            eventReceiver.Off += EventOff;
            eventReceiver.ControlChange += EventControlChange;
            eventReceiver.Reset += EventReset;
        }

        private void EventOn(VisualsEvent visualsEvent) {
            int index = visualsEvent.Index;
            float value = visualsEvent.Value;
            float valueNormalized = value / 255f;
            
            switch (index) {
                case < 8:
                    spotlights.Trigger(index);
                    break;
                case 8:
                    pyramid.LightEffect(valueNormalized);
                    break;
                case 9:
                    pyramid.RimEffect();
                    break;
                case 10:
                    pyramid.RandomizeRimColor();
                    break;
                case 11:
                    sparkles.Play();
                    break;
                case 12:
                    crystals.Trigger();
                    break;
                case 13:
                    terrain.Wave();
                    break;
                case 14:
                    terrain.PulseTriangles();
                    break;
            }
        }

        private void EventOff(VisualsEvent visualsEvent) {
            int index = visualsEvent.Index;
            
            switch (index) {
                case < 8:
                    spotlights.EndSustain(index);
                    break;
                case 11:
                    sparkles.Stop();
                    break;
            }
        }

        private void EventControlChange(VisualsEvent visualsEvent) {
            int index = visualsEvent.Index;
            float value = visualsEvent.Value;
            float valueNormalized = value / 255f;
            
            switch (index) {
                case 0:
                    spotlights.SetIntensity(valueNormalized);
                    break;
                case 1:
                    spotlights.SetDecay(valueNormalized);
                    break;
                case 2:
                    spotlights.SetSustain(valueNormalized);
                    break;
                case 3:
                    spotlights.SetRelease(valueNormalized);
                    break;
                case 4:
                    spotlights.SetOscillatorAmount(valueNormalized);
                    break;
                case 5:
                    spotlights.SetAngle(valueNormalized);
                    break;
                case 6:
                    pyramid.SetLightBaseIntensity(valueNormalized);
                    break;
                case 7:
                    pyramid.SetLightOscillatorIntensity(valueNormalized);
                    break;
                case 8:
                    pyramid.SetRimBaseIntensity(valueNormalized);
                    break;
                case 9:
                    terrain.SetTopLightIntensity(valueNormalized);
                    break;
                case 10:
                    background.SetIntensity(valueNormalized);
                    break;
            }
        }

        private void EventReset() {
            spotlights.DoReset();
            pyramid.DoReset();
            sparkles.DoReset();
            crystals.DoReset();
            terrain.DoReset();
            background.DoReset();
        }
    }
}
