using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.SoundManagement;
using Game.Core;

namespace Game.Testing
{
    public class SoundTest : MonoBehaviour
    {
        [SerializeField]private Slider musicVolSlider;
        [SerializeField]private Slider sfxVolSlider;

        [SerializeField]private SoundData musicSoundData;
        [SerializeField]private SoundData sfxSoundData;
        [SerializeField]private Camera lookCamera;

        private Plane plane;

        private void Awake() {
            plane = new Plane(Vector3.forward, transform.position);
        }
        // Start is called before the first frame update
        void Start()
        {
            float musicVolume = 0.0f;
            musicSoundData.mixerGroup.audioMixer.GetFloat(Constants.MUSIC_VOLUME, out musicVolume);
            musicVolSlider.value = musicVolume;

            float sfxVolume = 0.0f;
            sfxSoundData.mixerGroup.audioMixer.GetFloat(Constants.SFX_VOLUME, out sfxVolume);
            sfxVolSlider.value = sfxVolume;

            musicVolSlider.onValueChanged.AddListener(SoundManager.Instance.SetMusicVolume);
            sfxVolSlider.onValueChanged.AddListener(SoundManager.Instance.SetSFXVolume);

            SoundManager.Instance.Play(musicSoundData, transform.position);
        }

        // Update is called once per frame
        void Update()
        {
            if(UnityEngine.Input.GetMouseButtonDown(0))
            {
                Ray ray = lookCamera.ScreenPointToRay(UnityEngine.Input.mousePosition);
                if(plane.Raycast(ray, out float enter))
                {
                    Vector3 playSoundPos = ray.origin + ray.direction * enter;
                    SoundManager.Instance.Play(sfxSoundData, playSoundPos, true);
                }
            }
        }
    }
}
