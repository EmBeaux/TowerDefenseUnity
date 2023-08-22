using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tower))]
public class TowerEditor : Editor
{
    private Material circleMaterial;

    private void OnEnable()
    {
        circleMaterial = new Material(Shader.Find("Transparent/Diffuse"));
        circleMaterial.color = new Color(0.0f, 0.0f, 1.0f, 0.2f); // Blue with 20% opacity
    }

    private void OnSceneGUI()
    {
        Tower tower = (Tower)target;

        Handles.color = Color.blue;
        Handles.DrawWireDisc(tower.transform.position, Vector3.up, tower.range);
    }

    private void OnPostRender()
    {
        Tower tower = (Tower)target;
        if (circleMaterial != null && tower.drawRangeInGame)
        {
            GL.PushMatrix();
            circleMaterial.SetPass(0);
            GL.LoadOrtho();
            GL.Begin(GL.QUADS);
            GL.Vertex(new Vector3(0, 0, 0));
            GL.Vertex(new Vector3(1, 0, 0));
            GL.Vertex(new Vector3(1, 1, 0));
            GL.Vertex(new Vector3(0, 1, 0));
            GL.End();
            GL.PopMatrix();
        }
    }
}