using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.SoundManagement;

namespace Game.UI
{
    [RequireComponent(typeof(SquashStretchEffect), typeof(Button))]
    public class SquashStretchButton : MonoBehaviour
    {
        [SerializeField]private SoundData pressSound;
        private Button _button;
        private SquashStretchEffect _squashEffect;
        private void Awake() {
            _button = GetComponent<Button>();
            _squashEffect = GetComponent<SquashStretchEffect>();
        }
        // Start is called before the first frame update
        void Start()
        {
            _button.onClick.AddListener(_squashEffect.PlayEffect);
            _button.onClick.AddListener(()=> SoundManager.Instance.Play(pressSound, Vector3.zero, true));
        }
    }
}

