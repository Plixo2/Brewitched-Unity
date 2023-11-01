using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [ExecuteInEditMode]
public class ConnectionGrid : MonoBehaviour
{
    [SerializeField] private GameObject pointPrefab;

    [SerializeField] [Range(1, 10)] private int rows = 1;
    [SerializeField] [Range(1, 10)] private int collums = 1;
    [SerializeField] private float spacingX = 1;
    [SerializeField] private float spacingY = 1;
    [SerializeField] private bool generate = true;
    [SerializeField] private bool drawGizmos = false;


    void Update()
    {
        if (generate)
        {
            generate = false;
            GeneratePoint();
        }
    }

    private void GeneratePoint()
    {
        if (pointPrefab == null)
        {
            return;
        }
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
            DestroyImmediate(child);
        }

        var startPosition = transform.position;
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < collums; y++)
            {
                var pos = startPosition + new Vector3(x * spacingX, y * spacingY, 0);
                CreateConnection(pos);
            }
        }
    }

    private void CreateConnection(Vector3 position)
    {
        var point = Instantiate(pointPrefab, transform);
        point.transform.position = position;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            OnDrawGizmosSelected();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.7f);
        var startPosition = transform.position;
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < collums; y++)
            {
                var pos = startPosition + new Vector3(x * spacingX, y * spacingY, 0);
                Gizmos.DrawSphere(pos, 0.05f);
            }
        }
    }
}