using MMICSharp.Access;
using MMICSharp.Adapter;
using MMICSharp.Common.Communication;
using MMIStandard;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityLocomotionMMU;

public class WalkTrajectory : MonoBehaviour
{
    public List<Transform> Points = new List<Transform>();
    public MotionTrajectory2D ComputedPath;
    public Color Color = Color.red;



    /// <summary>
    /// Returns a trajectory constraint describing the 2D positions (x-z)
    /// </summary>
    /// <returns></returns>
    public MConstraint GetPathConstraint()
    {
        MConstraint constraint = new MConstraint(System.Guid.NewGuid().ToString());

        MPathConstraint pathConstraint = new MPathConstraint()
        {
            PolygonPoints = new List<MGeometryConstraint>()
        };

        foreach (Transform t in this.Points)
        {
            pathConstraint.PolygonPoints.Add(new MGeometryConstraint()
            {
                ParentObjectID = "",
                ParentToConstraint = new MTransform("", new MVector3(t.position.x, 0, t.position.z), new MQuaternion(0, 0, 0, 1))
            });
        }

        constraint.PathConstraint = pathConstraint;

        return constraint;
    }
    public MotionTrajectory2D GetPathConstraintCollision(MMUAccess MMUAccess, float Velocity)
    {
        List<MTransform> computedPath = new List<MTransform>();
        MPathPlanningService.Iface pathPlanningService = MMUAccess.ServiceAccess.PathPlanningService;


        List<MSceneObject> sceneObjects = MMUAccess.SceneAccess.GetSceneObjects();
        //Remove everything except StorageRacks
        //for (int i = sceneObjects.Count - 1; i >= 0; i--)
        //{
        //    if (!sceneObjects[i].Name.Contains("StorageRack"))
        //    {
        //        sceneObjects.RemoveAt(i);
        //    }
        //}
        for (int i = 0; i < Points.Count - 1; i++)
        {
            MVector start = new MVector() { Values = new List<double>() { Points[i].position.x, Points[i].position.z } };
            MVector goal = new MVector() { Values = new List<double>() { Points[i + 1].position.x, Points[i + 1].position.z } };
            MPathConstraint result = pathPlanningService.ComputePath(start, goal, sceneObjects, new Dictionary<string, string>()
                        {
                            { "mode", "2D"},
                            { "time", Serialization.ToJsonString(1.0f)},
                            { "radius", Serialization.ToJsonString(0.3f)},
                            { "height", Serialization.ToJsonString(0.5f)},
                        });
            if (result.PolygonPoints.Count > 0)
            {
                if (result.PolygonPoints[0].ParentToConstraint != null)
                {
                    computedPath.AddRange(result.PolygonPoints.Select(s => new MTransform() { Position = new MVector3(s.ParentToConstraint.Position.X, 0, s.ParentToConstraint.Position.Z) }).ToList());
                }
                else
                {
                    computedPath.AddRange(result.PolygonPoints.Select(s => new MTransform() { Position = new MVector3(s.TranslationConstraint.X(), 0, s.TranslationConstraint.Z()) }).ToList());
                }
            }
            else
            {
                Debug.Log("No path found");
            }
        }
        ComputedPath = new MotionTrajectory2D(computedPath, Velocity);
        drawLine();
        return ComputedPath;
    }
    private void drawLine() 
    {
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.positionCount = (ComputedPath.Poses.Count - 1) * 2;
        lineRenderer.SetPosition(0, new Vector3(ComputedPath.Poses[0].Position.x, 0, ComputedPath.Poses[0].Position.y));
        for (int i = 0; i < ComputedPath.Poses.Count - 1; i++)
        {
            Vector2 from = ComputedPath.Poses[i].Position;
            Vector2 to = ComputedPath.Poses[i + 1].Position;
            lineRenderer.SetPosition((i * 2), new Vector3(from.x, 0, from.y));
            lineRenderer.SetPosition((i * 2) + 1, new Vector3(to.x, 0, to.y));
            Debug.DrawLine(new Vector3(from.x, 0, from.y), new Vector3(to.x, 0, to.y), this.Color);
        }
    }
    private void OnDrawGizmos()
    {
        if (this.Points.Count >= 1)
        {
            for (int i = 0; i < this.Points.Count; i++)
            {
                if (this.Points[i] == null)
                    break;

                if (i < this.Points.Count - 1)
                {
                    if (this.Points[i + 1] != null)
                        Debug.DrawLine(this.Points[i].position, this.Points[i + 1].position, this.Color);
                }

                else
                {
                    if (this.Points[i] != null)
                        Debug.DrawLine(this.Points[i].position, this.transform.position, this.Color);
                }
            }


        }
    }

}
