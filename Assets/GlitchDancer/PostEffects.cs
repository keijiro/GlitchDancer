using UnityEngine;

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

        [SerializeField] Color _lineColor = Color.black;

        public Color backgroundColor {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }

        [SerializeField] Color _backgroundColor = new Color(1, 1, 1, 0);

        public float lowThreshold {
            get { return _lowThreshold; }
            set { _lowThreshold = value; }
        }

        [SerializeField, Range(0, 1)] float _lowThreshold = 0.05f;

        public float highThreshold {
            get { return _highThreshold; }
            set { _highThreshold = Mathf.Max(_lowThreshold, value); }
        }

        [SerializeField, Range(0, 1)] float _highThreshold = 0.5f;

        #endregion

        #region Private members

        [SerializeField, HideInInspector] Shader _shader;
        Material _material;

        #endregion

        #region MonoBehaviour Functions

        void OnDestroy()
        {
            if (_material != null)
                if (Application.isPlaying)
                    Destroy(_material);
                else
                    DestroyImmediate(_material);
        }

        void OnValidate()
        {
            _highThreshold = Mathf.Max(_lowThreshold, _highThreshold);
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (_material == null)
            {
                _material = new Material(_shader);
                _material.hideFlags = HideFlags.DontSave;
            }

            _material.SetColor("_LineColor", _lineColor);
            _material.SetColor("_BGColor", _backgroundColor);
            _material.SetVector("_Threshold", new Vector2(_lowThreshold, _highThreshold));

            Graphics.Blit(source, destination, _material);
        }

        #endregion
    }
}
