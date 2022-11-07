using System;
using System.Collections;
using System.Collections.Generic;
using GentleCat.ScriptableObjects.Properties;
using UnityEngine;
using UnityEngine.Serialization;

public class Lantern : MonoBehaviour
{
    // Start is called before the first frame update
    public LanternVariable lanternReference;
    public Transform light;
    public Transform playerModel;
    public bool pickedUpLanternLightUpgrade = false;
    public float currentTime;
    public float maxTime = 10f;
    public float startBlinkingTime = 3f;
    public LayerMask groundMask;

    private Material lanternMat;
    private static readonly int TimeLeft = Shader.PropertyToID("_Timeleft");

    public bool LanternOn => currentTime > 0;

    private void Awake()
    {
        lanternReference.CurrentValue = this;
    }

    private void Start()
    {
        currentTime = maxTime;
        lanternMat = light.GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, float.MaxValue,
            groundMask))
        {
            Vector3 dir = hitInfo.point - playerModel.position;
            dir.y = 0;
            dir.Normalize();
            //transform.localPosition = dir * lanternDistance;
            playerModel.forward = dir;
        }


        currentTime -= Time.deltaTime;
        lanternMat.SetFloat(TimeLeft,currentTime / startBlinkingTime);
        light.gameObject.SetActive(currentTime > 0);

        if (Input.GetKeyDown(KeyCode.L))
        {
            currentTime = maxTime;
        }
        if (pickedUpLanternLightUpgrade)
        {
            light.localScale = new Vector3(8f, 1f, 12f);
        }
    }


    public bool InTriangle(Vector2 p, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        var a = .5f * (-p1.y * p2.x + p0.y * (-p1.x + p2.x) + p0.x * (p1.y - p2.y) + p1.x * p2.y);
        var sign = a < 0 ? -1 : 1;
        var s = (p0.y * p2.x - p0.x * p2.y + (p2.y - p0.y) * p.x + (p0.x - p2.x) * p.y) * sign;
        var t = (p0.x * p1.y - p0.y * p1.x + (p0.y - p1.y) * p.x + (p1.x - p0.x) * p.y) * sign;

        return s > 0 && t > 0 && (s + t) < 2 * a * sign;
    }

    public bool IsInside(Vector3 point)
    {
        if (!LanternOn)
            return false;
        Camera cam = Camera.main;
        Mesh triangle = light.GetComponent<MeshFilter>().mesh;
        List<Vector3> vertices = new List<Vector3>();
        triangle.GetVertices(vertices);
        Vector2 a = cam.WorldToScreenPoint(light.TransformPoint(vertices[0]));
        Vector2 b = cam.WorldToScreenPoint(light.TransformPoint(vertices[1]));
        Vector2 c = cam.WorldToScreenPoint(light.TransformPoint(vertices[2]));

        bool inTriangle = InTriangle(cam.WorldToScreenPoint(point), a, b, c);
        return inTriangle;
    }
}