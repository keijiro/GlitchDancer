using UnityEngine;
using System.Collections;

namespace GlitchDancer
{
    public class StyleChanger : MonoBehaviour
    {
        [SerializeField] PostEffects _postEffects;
        [SerializeField] Renderer[] _floor;
        [SerializeField] Renderer[] _body;
        [SerializeField] Renderer[] _effects;

        IEnumerator Start()
        {
            var fx = _postEffects;
            var cam = Camera.main;
            const float interval = 2;

            while (true)
            {
                var hue = Random.value;
                var hue2 = (hue + 0.18f) % 1;
                var hue3 = (hue + 0.42f) % 1;

                fx.lineColor = Color.black;
                fx.fillOpacity = 0;
                fx.lineOpacity = 0.2f;

                foreach (var o in _floor)
                    o.material.color = Color.HSVToRGB(hue, 0.8f, 0.5f);

                foreach (var o in _body)
                    o.material.color = Color.HSVToRGB(hue, 0.5f, 0.5f);

                cam.backgroundColor = Color.HSVToRGB(hue2, 0.9f, 0.8f);

                foreach (var o in _effects)
                    o.material.SetFloat("_BaseHue", hue3);

                yield return new WaitForSeconds(interval);

                fx.fillOpacity = 1;
                fx.lineOpacity = 1;

                fx.darkColor = Color.HSVToRGB(hue, 1, 0.3f);
                fx.lightColor = Color.HSVToRGB(hue3, 0.7f, 1);
                fx.lineColor = Color.HSVToRGB(hue3, 0.7f, 1);

                yield return new WaitForSeconds(interval);

                fx.darkColor = Color.HSVToRGB(hue2, 1, 0.3f);
                fx.lightColor = Color.HSVToRGB(hue, 0.7f, 1);
                fx.lineColor = Color.HSVToRGB(hue2, 1, 0.3f);

                yield return new WaitForSeconds(interval);
            }
        }
    }
}
