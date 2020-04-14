using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLaser : WeaponBase
{
    LineRenderer line;
    public Material lineMat;
    private bool isLine = false;

    [SerializeField] private float endSpeed = 10f;
    [SerializeField] private float lineStartWidth = 0.05f;
    [SerializeField] private float lineEndWidth = 0.05f;

    private Vector3 endPoint, bezierCtrlPoint;
    private int numPoints = 50;
    private Vector3[] positions = new Vector3[50];

    public float EndSpeed { get => endSpeed; set => endSpeed = value; }
    public float LineStartWidth { get => lineStartWidth; set => lineStartWidth = value; }
    public float LineEndWidth { get => lineEndWidth; set => lineEndWidth = value; }

    protected override void Start()
    {
        base.Start();

        line = GetComponent<LineRenderer>();
        line.positionCount = numPoints;
        line.material = lineMat;
        line.startWidth = lineStartWidth;
        line.endWidth = lineEndWidth;

    }

    protected override void Update()
    {
        base.Update();
        if (isLine)
        {
            UpdateBezierPoint();
        }
    }
    public override void StopDrawingLaser()
    {
        if (line != null)
        {
            line.enabled = false;
            isLine = false;
        }
    }

    protected override void ShootingBullet(Vector3 direction)
    {
        Ray ray = new Ray(Camera.main.transform.position, direction);

        if (Physics.Raycast(ray, out RaycastHit hit, _range))
        {
            endPoint = hit.point;
            EnemyHitBehaviour(ref hit, ray);
        }
        else
        {
            endPoint = Camera.main.transform.forward * _range;
        }

        if (!isLine)
        {
            isLine = true;
            bezierCtrlPoint = endPoint;
            line.enabled = true;
        }
        DrawQuadBezierCurve();
    }
    private Vector3 CalculateQuadBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        // B(t) = (1-t)^2P0 + 2(1-t)tP1 + t^2P2

        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = (uu * p0) + (2 * u * t * p1) + (tt * p2);

        return p;
    }
    private void DrawQuadBezierCurve()
    {
        for (int i = 0; i < numPoints; i++)
        {
            float t = i / (float)numPoints;
            positions[i] = CalculateQuadBezierPoint(t, Camera.main.transform.position + new Vector3(0.5f, -0.25f), endPoint, bezierCtrlPoint);
        }
        line.SetPositions(positions);
    }
    private void UpdateBezierPoint()
    {
        float actualSpeed = endSpeed * Vector3.Distance(bezierCtrlPoint, endPoint);
        bezierCtrlPoint = Vector3.MoveTowards(bezierCtrlPoint, endPoint, Time.deltaTime * actualSpeed);
    }
}
