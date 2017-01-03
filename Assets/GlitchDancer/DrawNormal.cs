using UnityEngine;

public class DrawNormal : MonoBehaviour
{
    [SerializeField] Shader _shader;
    void Start()
    {
        GetComponent<Camera>().SetReplacementShader(_shader, "NormalDrawer");
    }
}
