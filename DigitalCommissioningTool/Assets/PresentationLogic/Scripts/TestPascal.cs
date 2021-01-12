using ApplicationFacade.Application;
using MMICoSimulation;
using MMICSharp.MMIStandard.Utils;
using MMIStandard;
using MMIUnity.TargetEngine;
using MMIUnity.TargetEngine.Scene;
using System.Collections.Generic;
using UnityEngine;

public class TestPascal : AvatarBehavior
{
    public WalkTrajectory WalkTrajectory;
    public MMISceneObject WalkTrajectoryTarget;
    public List<Transform> WalkTrajectoryPoints = new List<Transform>();
    public QueueMenu queueMenu;

    private string carryID;
    public GameObject StorageRack;

    protected override void GUIBehaviorInput()
    {
        if (GUI.Button(new Rect(290, 10, 120, 50), "Idle"))
        {
            //Create a new idle instruction of type idle
            MInstruction idleInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Idle", "Pose/Idle");

            //Abort all instructions
            this.CoSimulator.Abort();

            //Assign the instruction to the co-simulator
            this.CoSimulator.AssignInstruction(idleInstruction, null);
        }


        //Walkin to a specific point
        if (GUI.Button(new Rect(420, 10, 120, 50), "Walk to"))
        {
            //First create the walk instruction to walk to the specific object
            MInstruction walkInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Walk", "Locomotion/Walk")
            {
                //Write the target id to the properties (the target id is gathered from the scene).
                //An alternative way to get a target would be to directly use the MMISceneObject as editor variable
                //Force path means a straight line path is enfored if no path can be found
                Properties = PropertiesCreator.Create("TargetID", UnitySceneAccess.Instance.GetSceneObjectByName("WalkTarget").ID, "ForcePath", false.ToString(), "Velocity", 2.0f.ToString())//"ReplanningTime", 500.ToString())
            };
            this.CoSimulator.AssignInstruction(walkInstruction, null);


            //Furthermore create an idle instruction
            MInstruction idleInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Idle", "Pose/Idle")
            {
                //Start idle after walk has been finished
                StartCondition = walkInstruction.ID + ":" + mmiConstants.MSimulationEvent_End
            };

            //Abort all current tasks
            this.CoSimulator.Abort();

            //Assign walk and idle instruction
            this.CoSimulator.AssignInstruction(walkInstruction, null);
            this.CoSimulator.AssignInstruction(idleInstruction, null);
            this.CoSimulator.MSimulationEventHandler += this.CoSimulator_MSimulationEventHandler;
        }


        if (GUI.Button(new Rect(550, 10, 150, 50), "Fill Storage Rack"))
        {
            Transform carryTarget = gameObject.transform.Find("CarryTarget");
            MInstruction startCondition = null;
            foreach (Transform t in StorageRack.transform)
            {
                if (t.name.Contains("StorageContainer"))
                {
                    startCondition = PickupOneHand(t.GetComponent<MMISceneObject>(), startCondition);
                }
                else
                {
                    Transform newCarryTarget = Instantiate(gameObject.transform.Find("CarryTarget"));
                    newCarryTarget.parent = carryTarget.parent;
                    newCarryTarget.localPosition += new Vector3(-t.localScale.x / 2.0f, 0, t.localScale.z / 2.0f);
                    //newCarryTarget.localPosition += new Vector3(-t.GetComponent<BoxCollider>().size.x/2.0f, 0, t.GetComponent<BoxCollider>().size.z / 2.0f);
                    startCondition = PickupBothHand(t.GetComponent<MMISceneObject>(), newCarryTarget.GetComponent<MMISceneObject>(), startCondition);
                }

            }
            this.CoSimulator.MSimulationEventHandler += this.CoSimulator_MSimulationEventHandler;
        }
        if (GUI.Button(new Rect(710, 10, 150, 50), "Plan Path"))
        {
            UpdatePath();
        }
    }
    public void UpdatePath()
    {
        this.WalkTrajectory.Points.Clear();
        this.WalkTrajectory.Points.Add(this.transform);
        foreach (DragItem dragItem in queueMenu.QueueItems)
        {
            Transform walkTarget = dragItem.LinkedItem.Object.transform.Find("BoxWalkTarget");
            this.WalkTrajectory.Points.Add(walkTarget);
            this.WalkTrajectory.Points.Add(GameManager.GameContainer.StorageContainer[0].Object.transform);
        }
        //foreach (Transform child in WalkTrajectoryPoints)
        //{
        //    this.WalkTrajectory.Points.Add(child);
        //}
        WalkTrajectoryTarget = this.WalkTrajectory.Points[this.WalkTrajectory.Points.Count - 1].GetComponent<MMISceneObject>();
        MMICSharp.Access.MMUAccess mmuAccess = this.avatar.MMUAccess;
        this.WalkTrajectory.GetPathConstraintCollision(mmuAccess, 1.0f);
    }
    private MInstruction PickupOneHand(MMISceneObject target, MInstruction startCondition)
    {
        MMISceneObject source = target.InitialLocation;
        MSceneObject m_source = source.MSceneObject;
        MSceneObject m_target = target.MSceneObject;
        MSceneObject m_GraspR = source.gameObject.transform.Find("GraspTargetR").GetComponent<MMISceneObject>().MSceneObject;
        MInstruction walkInstruction;
        if (startCondition != null)
        {
            walkInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Walk", "Locomotion/Walk")
            {
                Properties = PropertiesCreator.Create("TargetID", source.IsLocatedAt.MSceneObject.ID, "Velocity", 2.0f.ToString()),
                StartCondition = startCondition.ID + ":" + mmiConstants.MSimulationEvent_End
            };
        }
        else
        {
            walkInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Walk", "Locomotion/Walk")
            {
                Properties = PropertiesCreator.Create("TargetID", source.IsLocatedAt.MSceneObject.ID, "Velocity", 2.0f.ToString())
            };
        }

        MInstruction idleInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Idle", "Pose/Idle")
        {
            StartCondition = walkInstruction.ID + ":" + mmiConstants.MSimulationEvent_End
        };

        MInstruction reachRight = new MInstruction(MInstructionFactory.GenerateID(), "reachRight", "Pose/Reach")
        {
            Properties = PropertiesCreator.Create("TargetID", m_GraspR.ID, "Hand", "Right"),
            StartCondition = walkInstruction.ID + ":" + mmiConstants.MSimulationEvent_End
        };

        carryID = MInstructionFactory.GenerateID();
        MInstruction carryInstruction = new MInstruction(carryID, "carry object", "Object/Carry")
        {
            Properties = PropertiesCreator.Create("TargetID", m_source.ID, "Hand", "Right", "Velocity", 2.0f.ToString()/*, "AddOffset", true.ToString(), "CarryDistance", 0.07f.ToString(), "CarryTarget", UnitySceneAccess.Instance["CarryTarget"].ID*/),
            StartCondition = reachRight.ID + ":" + mmiConstants.MSimulationEvent_End + "+ 0.01"
        };


        MInstruction walkInstructionBack = new MInstruction(MInstructionFactory.GenerateID(), "Walk", "Locomotion/Walk")
        {
            Properties = PropertiesCreator.Create("TargetID", target.IsLocatedAt.MSceneObject.ID, "Velocity", 2.0f.ToString()),
            StartCondition = carryInstruction.ID + ":" + mmiConstants.MSimulationEvent_Start
        };

        MInstruction moveInstruction = new MInstruction(MInstructionFactory.GenerateID(), "move object", "Object/Move")
        {
            Properties = PropertiesCreator.Create("SubjectID", m_source.ID, "Hand", "Right", "TargetID", m_target.ID, "HoldDuration", 1.0f.ToString(), "Velocity", 2.0f.ToString()),

            //Terminate the carry
            Action = CoSimTopic.OnStart + "->" + carryID + ":" + CoSimAction.EndInstruction,
            StartCondition = walkInstructionBack.ID + ":" + mmiConstants.MSimulationEvent_End
        };

        MInstruction releaseRight = new MInstruction(MInstructionFactory.GenerateID(), "release object", "Object/Release")
        {
            Properties = PropertiesCreator.Create("Hand", "Right", CoSimTopic.OnStart, carryID + ":" + CoSimAction.EndInstruction),
            StartCondition = moveInstruction.ID + ":" + mmiConstants.MSimulationEvent_End
        };

        this.CoSimulator.AssignInstruction(walkInstruction, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
        this.CoSimulator.AssignInstruction(idleInstruction, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
        this.CoSimulator.AssignInstruction(reachRight, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
        this.CoSimulator.AssignInstruction(carryInstruction, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
        this.CoSimulator.AssignInstruction(walkInstructionBack, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
        this.CoSimulator.AssignInstruction(moveInstruction, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
        this.CoSimulator.AssignInstruction(releaseRight, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
        return releaseRight;
    }

    private MInstruction PickupBothHand(MMISceneObject target, MMISceneObject carryTarget, MInstruction startCondition)
    {
        MMISceneObject source = target.InitialLocation;
        MSceneObject m_source = source.MSceneObject;
        MSceneObject m_target = target.MSceneObject;
        MSceneObject m_GraspL = source.gameObject.transform.Find("GraspTargetL").GetComponent<MMISceneObject>().MSceneObject;
        MSceneObject m_GraspR = source.gameObject.transform.Find("GraspTargetR").GetComponent<MMISceneObject>().MSceneObject;

        MInstruction walkInstruction;
        if (startCondition != null)
        {
            walkInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Walk", "Locomotion/Walk")
            {
                Properties = PropertiesCreator.Create("TargetID", source.IsLocatedAt.MSceneObject.ID, "Velocity", 2.0f.ToString()),
                StartCondition = startCondition.ID + ":" + mmiConstants.MSimulationEvent_End
            };
        }
        else
        {
            walkInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Walk", "Locomotion/Walk")
            {
                Properties = PropertiesCreator.Create("TargetID", source.IsLocatedAt.MSceneObject.ID, "Velocity", 2.0f.ToString())
            };
        }

        MInstruction idleInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Idle", "Pose/Idle")
        {
            StartCondition = walkInstruction.ID + ":" + mmiConstants.MSimulationEvent_End
        };

        MInstruction reachLeft = new MInstruction(MInstructionFactory.GenerateID(), "reachLeft", "Pose/Reach")
        {
            Properties = PropertiesCreator.Create("TargetID", m_GraspL.ID, "Hand", "Left"),
            StartCondition = walkInstruction.ID + ":" + mmiConstants.MSimulationEvent_End
        };

        MInstruction reachRight = new MInstruction(MInstructionFactory.GenerateID(), "reachRight", "Pose/Reach")
        {
            Properties = PropertiesCreator.Create("TargetID", m_GraspR.ID, "Hand", "Right"),
            StartCondition = walkInstruction.ID + ":" + mmiConstants.MSimulationEvent_End
        };

        carryID = MInstructionFactory.GenerateID();
        MInstruction carryInstruction = new MInstruction(carryID, "carry object", "Object/Carry")
        {
            Properties = PropertiesCreator.Create("TargetID", m_source.ID, "Hand", "Both", "Velocity", 2.0f.ToString(), /*"AddOffset", true.ToString(), "CarryDistance", 0.07f.ToString(),*/ "CarryTarget", carryTarget.MSceneObject.ID),
            StartCondition = reachLeft.ID + ":" + mmiConstants.MSimulationEvent_End + " && " + reachRight.ID + ":" + mmiConstants.MSimulationEvent_End + "+ 0.01"
        };


        MInstruction walkInstructionBack = new MInstruction(MInstructionFactory.GenerateID(), "Walk", "Locomotion/Walk")
        {
            Properties = PropertiesCreator.Create("TargetID", target.IsLocatedAt.MSceneObject.ID, "Velocity", 2.0f.ToString()),
            StartCondition = carryInstruction.ID + ":" + mmiConstants.MSimulationEvent_Start
        };

        MInstruction moveInstruction = new MInstruction(MInstructionFactory.GenerateID(), "move object", "Object/Move")
        {
            Properties = PropertiesCreator.Create("SubjectID", m_source.ID, "Hand", "Both", "TargetID", m_target.ID, "HoldDuration", 1.0f.ToString(), "Velocity", 2.0f.ToString()),

            //Terminate the carry
            Action = CoSimTopic.OnStart + "->" + carryID + ":" + CoSimAction.EndInstruction,
            StartCondition = walkInstructionBack.ID + ":" + mmiConstants.MSimulationEvent_End
        };

        MInstruction releaseRight = new MInstruction(MInstructionFactory.GenerateID(), "release object", "Object/Release")
        {
            Properties = PropertiesCreator.Create("Hand", "Right", CoSimTopic.OnStart, carryID + ":" + CoSimAction.EndInstruction),
            StartCondition = moveInstruction.ID + ":" + mmiConstants.MSimulationEvent_End
        };
        MInstruction releaseLeft = new MInstruction(MInstructionFactory.GenerateID(), "release object", "Object/Release")
        {
            Properties = PropertiesCreator.Create("Hand", "Left", CoSimTopic.OnStart, carryID + ":" + CoSimAction.EndInstruction),
            StartCondition = moveInstruction.ID + ":" + mmiConstants.MSimulationEvent_End
        };


        this.CoSimulator.AssignInstruction(walkInstruction, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
        this.CoSimulator.AssignInstruction(idleInstruction, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
        this.CoSimulator.AssignInstruction(reachLeft, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
        this.CoSimulator.AssignInstruction(reachRight, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
        this.CoSimulator.AssignInstruction(carryInstruction, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
        this.CoSimulator.AssignInstruction(walkInstructionBack, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
        this.CoSimulator.AssignInstruction(moveInstruction, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
        this.CoSimulator.AssignInstruction(releaseLeft, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
        this.CoSimulator.AssignInstruction(releaseRight, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
        return releaseLeft;
    }


    /// <summary>
    /// Callback for the co-simulation event handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CoSimulator_MSimulationEventHandler(object sender, MSimulationEvent e)
    {
        Debug.Log(e.Reference + " " + e.Name + " " + e.Type);
    }


}
