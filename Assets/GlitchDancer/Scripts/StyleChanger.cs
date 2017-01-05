using UnityEngine;

namespace GlitchDancer
{
    public class StyleChanger : MonoBehaviour
    {
        #region Public properties

        public float scheme {
            set { _scheme = value; _modified = true; }
        }

        [SerializeField, Range(0, 1)] float _scheme;

        public float hue {
            set { _hue = value ; _modified = true; }
        }

        [SerializeField, Range(0, 1)] float _hue;

        bool _modified = true;

        #endregion

        #region Editor-only settings

        [Space]
        [SerializeField] PostEffects _postEffects;
        [SerializeField] Renderer _floor;
        [SerializeField] Renderer _body;
        [SerializeField] Renderer[] _effects;

        #endregion

        #region MonoBehaviour functions

        void OnValidate()
        {
            _modified = true;
        }

        void Update()
        {
            if (!_modified) return;

            // Triad hue set.
            var hue1 = (_hue + 0.113f) % 1;
            var hue2 = (_hue + 0.380f) % 1;
            var hue3 = (_hue + 0.750f) % 1;

            // Update material colors.
            if (_scheme < 0.25f)
            {
                // Set colors based on the hue set.
                Camera.main.backgroundColor = Color.HSVToRGB(hue1, 0.86f, 0.95f);

                _floor.material.color = Color.HSVToRGB(hue2, 0.54f, 0.49f);
                _body.material.color = Color.HSVToRGB(hue2, 0.80f, 0.51f);

                foreach (var renderer in _effects)
                    renderer.material.SetFloat("_BaseHue", hue3);
            }
            else
            {
                // Set placeholder colors.
                Camera.main.backgroundColor = new Color(1, 0.6f, 0);

                _floor.material.color = new Color(0, 0.5f, 0);
                _body.material.color = new Color(0, 0, 0.5f);

                foreach (var renderer in _effects)
                    renderer.material.SetFloat("_BaseHue", 0);
            }

            // Set the post effects settings.
            var fx = _postEffects;

            if (_scheme < 0.25f)
            {
                // Turn off filling.
                fx.fillOpacity = 0;
                fx.lineOpacity = 0.2f;
                fx.lineColor = Color.black;
            }
            else
            {
                // Make the effect opaque.
                fx.fillOpacity = 1;
                fx.lineOpacity = 1;

                if (_scheme < 0.5f)
                {
                    // Tone 1
                    fx.darkColor = Color.HSVToRGB(hue1, 1, 0.3f);
                    fx.lightColor = Color.HSVToRGB(hue2, 0.7f, 1);
                    fx.lineColor = Color.HSVToRGB(hue1, 1, 0.3f);
                }
                else if (_scheme < 0.75f)
                {
                    // Tone 2
                    fx.darkColor = Color.HSVToRGB(hue2, 1, 0.3f);
                    fx.lightColor = Color.HSVToRGB(hue3, 0.7f, 1);
                    fx.lineColor = Color.HSVToRGB(hue3, 0.7f, 1);
                }
                else
                {
                    // Black and white.
                    fx.darkColor = Color.white * Mathf.Lerp(0.7f, 0.3f, _hue);
                    fx.lightColor = Color.white * (1 - _hue);
                    fx.lineColor = Color.white * Mathf.Lerp(0.1f, 0.8f, _hue);
                }
            }

            _modified = false;
        }

        #endregion
    }
}
