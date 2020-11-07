using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnChangePosition : MonoBehaviour
{
    public PolygonCollider2D hole2DCollider;
    public PolygonCollider2D ground2DCollider;
    public MeshCollider generatedMeshCollider;
    public Collider groundCollider;
    Mesh generatedMesh;
    public float initialScale = 0.5f;

    public IEnumerator ScaleHole()
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = 2 * startScale;

        float t = 0;
        while (t < 0.4f)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

    }

    private void Start()
    {
        GameObject[] allGOs = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (var go in allGOs)
        {
            if (go.layer == LayerMask.NameToLayer("Obstacles"))
            {
                Physics.IgnoreCollision(go.GetComponent<Collider>(), generatedMeshCollider, true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Physics.IgnoreCollision(other, groundCollider, true);
        Physics.IgnoreCollision(other, generatedMeshCollider, false);
    }

    private void OnTriggerExit(Collider other)
    {
        Physics.IgnoreCollision(other, groundCollider, false);
        Physics.IgnoreCollision(other, generatedMeshCollider, true);
    }

    private void FixedUpdate()
    {
        if(transform.hasChanged == true)
        {
            transform.hasChanged = false;
            hole2DCollider.transform.position = new Vector2(transform.position.x, transform.position.z);
            hole2DCollider.transform.localScale = transform.localScale * initialScale;
            MakeHole2D();
            Make3DMeshCollider();
        }

        Move();
    }

    private void Move()
    {
        float hMov = Input.GetAxis("Horizontal");
        float vMov = Input.GetAxis("Vertical");
        float speed = 2;

        if(hMov!=0 || vMov != 0)
        {
            transform.Translate(new Vector3(speed * hMov * Time.deltaTime, 0, speed * vMov * Time.deltaTime));
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -4.5f, 4.5f),
                transform.position.y, Mathf.Clamp(transform.position.z, -4.5f, 4.5f));           
        }
    }

    private void MakeHole2D()
    {
        Vector2[] pointPositions = hole2DCollider.GetPath(0);
        for(int i = 0; i<pointPositions.Length; i++)
        {
            pointPositions[i] = hole2DCollider.transform.TransformPoint(pointPositions[i]);
        }

        ground2DCollider.pathCount = 2;
        ground2DCollider.SetPath(1, pointPositions);
    }
    private void Make3DMeshCollider()
    {
        if (generatedMesh != null) Destroy(generatedMesh);
        generatedMesh = ground2DCollider.CreateMesh(true, true);
        generatedMeshCollider.sharedMesh = generatedMesh;
    }
}
