using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class MouseColor : MonoBehaviour
{
    public Material material;
    public TextMeshProUGUI textoRGB;
    private Camera _cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cam = Camera.main;
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if(Mouse.current == null)
        {
            return;
        }
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = _cam.ScreenPointToRay(mousePos);

        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            Vector3 local = transform.InverseTransformPoint(hit.point);
            float r = (local.x + 0.5f);
            float g = (local.y + 0.5f);
            float b = (local.z + 0.5f);

            Color color = new Color(r, g, b, 1.0f);

            material.SetColor("_Color", color);

            if(textoRGB != null )
            {
                textoRGB.text = $"R:{r:F2} G:{g:F2} B:{b:F2}";
            }
        }
    }
}
