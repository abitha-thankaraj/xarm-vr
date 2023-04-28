
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3; // Add this line to alias UnityEngine.Vector3


public class WorldAxes : MonoBehaviour
{
    public float axisLength = 5f; // the length of the axis lines
    public float axisThickness = 0.03f; // the thickness of the axis lines
    public Color xAxisColor = Color.red; // the color of the x axis line
    public Color yAxisColor = Color.green; // the color of the y axis line
    public Color zAxisColor = Color.blue; // the color of the z axis line

    private GameObject xAxis; // the x axis line object
    private GameObject yAxis; // the y axis line object
    private GameObject zAxis; // the z axis line object

    void Start()
    {
        // create the x axis line
        xAxis = new GameObject("X Axis");
        xAxis.transform.SetParent(transform);
        xAxis.transform.localPosition = Vector3.zero;
        xAxis.AddComponent<LineRenderer>();
        LineRenderer xLineRenderer = xAxis.GetComponent<LineRenderer>();
        xLineRenderer.startColor = xAxisColor;
        xLineRenderer.endColor = xAxisColor;
        xLineRenderer.startWidth = axisThickness;
        xLineRenderer.endWidth = axisThickness;
        xLineRenderer.SetPositions(new Vector3[] {
            Vector3.zero,
            Vector3.right * axisLength
        });

        // create the y axis line
        yAxis = new GameObject("Y Axis");
        yAxis.transform.SetParent(transform);
        yAxis.transform.localPosition = Vector3.zero;
        yAxis.AddComponent<LineRenderer>();
        LineRenderer yLineRenderer = yAxis.GetComponent<LineRenderer>();
        yLineRenderer.startColor = yAxisColor;
        yLineRenderer.endColor = yAxisColor;
        yLineRenderer.startWidth = axisThickness;
        yLineRenderer.endWidth = axisThickness;
        yLineRenderer.SetPositions(new Vector3[] {
            Vector3.zero,
            Vector3.up * axisLength
        });

        // create the z axis line
        zAxis = new GameObject("Z Axis");
        zAxis.transform.SetParent(transform);
        zAxis.transform.localPosition = Vector3.zero;
        zAxis.AddComponent<LineRenderer>();
        LineRenderer zLineRenderer = zAxis.GetComponent<LineRenderer>();
        zLineRenderer.startColor = zAxisColor;
        zLineRenderer.endColor = zAxisColor;
        zLineRenderer.startWidth = axisThickness;
        zLineRenderer.endWidth = axisThickness;
        zLineRenderer.SetPositions(new Vector3[] {
            Vector3.zero,
            Vector3.forward * axisLength
        });
    }
}
