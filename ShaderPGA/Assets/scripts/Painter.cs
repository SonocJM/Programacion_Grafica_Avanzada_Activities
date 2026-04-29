using UnityEngine;
using UnityEngine.InputSystem;

public class Painter : MonoBehaviour
{
    public Material displayMaterial;
    public Material paintMaterial;
    public RenderTexture renderTexture;

    private Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        displayMaterial.SetTexture("_MainTex", renderTexture);

        RenderTexture.active = renderTexture;
        GL.Clear(true,true, Color.white);
        RenderTexture.active = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(Mouse.current == null)
        {
            return;
        }
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    Vector2 uv = hit.textureCoord;
                    if (uv == Vector2.zero)
                    {
                        Vector3 local = transform.InverseTransformPoint(hit.point);
                        uv = new Vector2(local.x + 0.5f, local.y + 0.5f);
                    }
                    paintMaterial.SetVector("_BrushPos", new Vector4(uv.x, uv.y, 0, 0));
                    RenderTexture temp = RenderTexture.GetTemporary(renderTexture.width, renderTexture.height);
                    Graphics.Blit(renderTexture, temp);
                    Graphics.Blit(temp, renderTexture, paintMaterial);
                    RenderTexture.ReleaseTemporary(temp);
                }
            }
        }
    }
}
