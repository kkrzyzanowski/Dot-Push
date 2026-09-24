using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private Line _linePrefab;

    private Line[] lines;
    void Awake()
    {
      
    }
    public void AddPoints(Vector3[] points)
    {
        lines = new Line[points.Length];
        for(int i = 0; i < points.Length; i++)
        {
            lines[i] = Instantiate(_linePrefab, transform);
            if(i+1 < points.Length)
                lines[i].SetPosition(points[i], points[i+1]);
            else
                lines[i].SetPosition(points[i], points[i]);
        }
    }

    public void DrawLines()
    {
    }

    public void ClearLines()
    {
        lines = null;
    }
}

