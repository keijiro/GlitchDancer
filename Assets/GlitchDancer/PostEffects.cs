using UnityEngine;
using System.Collections;

namespace GlitchDancer
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class PostEffects : MonoBehaviour
    {
        #region Public properties

        public Color lineColor {
            get { return _lineColor; }
            set { _lineColor = value; }
        }

        [SerializeField, ColorUsage(false)] Color _lineColor = Color.black;

        public float lineOpacity {
            get { return _lineOpacity; }
            set { _lineOpacity = value; }
        }

        [SerializeField, Range(0, 1)] float _lineOpacity = 1;

        public Color darkColor {
            get { return _darkColor; }
            set { _darkColor = value; }
        }

        [SerializeField, ColorUsage(false)] Color _darkColor = Color.black;

        public Color lightColor {
            get { return _lightColor; }
            set { _lightColor = value; }
        }

        [SerializeField, ColorUsage(false)] Color _lightColor = Color.white;

        public float fillOpacity {
            get { return _fillOpacity; }
            set { _fillOpacity = value; }
        }

        [SerializeField, Range(0, 1)] float _fillOpacity = 0;

        public float glitch {
            get { return _glitch; }
            set { _glitch = value; }
        }

        [SerializeField, Range(0, 1)] float _glitch;

        #endregion

        #region Private members

        [SerializeField, HideInInspector] Shader _shader;
        Material _material;

        #endregion

        #region MonoBehaviour Functions

        IEnumerator Start()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);

                {
                    _lineColor = Color.black;
                    _fillOpacity = 0;
                    _lineOpacity = 0;
                }

                yield return new WaitForSeconds(1);

                _fillOpacity = 1;
                _lineOpacity = 1;

                {
                    var hue = Random.value;
                    var hue2 = (hue + Random.Range(1.0f / 3, 2.0f / 3)) % 1;
                    _darkColor = Color.HSVToRGB(hue, 1, 0.4f);
                    _lightColor = Color.HSVToRGB(hue2, 1, 1);
                    _lineColor = Color.HSVToRGB(hue2, 1, 0.8f);
                }

                yield return new WaitForSeconds(1);

                {
                    var hue = Random.value;
                    var sat = Random.value;
                    _darkColor = Color.HSVToRGB(hue, sat, 0.4f);
                    _lightColor = Color.HSVToRGB(hue, sat, 1);
                    _lineColor = Color.black;
                }

                yield return new WaitForSeconds(1);

                {
                    var hue = Random.value;
                    var sat = Random.value;
                    _darkColor = Color.HSVToRGB(hue, sat, 1);
                    _lightColor = Color.HSVToRGB(hue, sat, 0.4f);
                    _lineColor = Color.white;
                }
            }
        }

        void OnDestroy()
        {
            if (_material != null)
                if (Application.isPlaying)
                    Destroy(_material);
                else
                    DestroyImmediate(_material);
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (_material == null)
            {
                _material = new Material(_shader);
                _material.hideFlags = HideFlags.DontSave;
            }

            var color = _lineColor;
            color.a = _lineOpacity;
            _material.SetColor("_LineColor", color);

            color = _darkColor;
            color.a = _fillOpacity;
            _material.SetColor("_Color1", color);
            _material.SetColor("_Color2", _lightColor);

            _material.SetFloat("_Glitch", _glitch);

            Graphics.Blit(source, destination, _material);
        }

        #endregion
    }
}
