using UnityEngine;
using i5.VirtualAgents;
using i5.VirtualAgents.ScheduleBasedExecution;
using System.Collections;
using i5.VirtualAgents.AgentTasks;

/// <summary>
/// Defines a Controller for Agent behaviour in SelectionRoom
/// </summary>
public class ScheduleControllerSelectionRoom : ScheduleController
{
    ///<summary>The agent which is controlled by this controller, set in the inspector</summary>
    [SerializeField] private Agent Agent;

    ///<summary>Navigation Points</summary>
    [SerializeField] private GameObject HeartNavPoint;
    [SerializeField] private GameObject BrainNavPoint;
    [SerializeField] private GameObject LiverNavPoint;
    [SerializeField] private GameObject LungsNavPoint;
    [SerializeField] private GameObject MiddleNavPoint1;
    [SerializeField] private GameObject MiddleNavPoint2;

    [SerializeField] private GameObject Heart;
    [SerializeField] private GameObject Brain;
    [SerializeField] private GameObject Liver;
    [SerializeField] private GameObject Lungs;

    // reference to explanation ui to deactivate on explanation start
    private GameObject explUI;

    ///<summary>The taskSystem of the agent</summary>
    protected ScheduleBasedTaskSystem TaskSystem;

    ///<summary>talking speed multiplier</summary>
    [Range(0.1f, 10f)] public float TalkingSpeed;
    
    // make UI button only "executable" once
    private bool isExecuted;
    
    protected void Start()
    {
        // initialize
        explUI = GameObject.Find("StartExplanationUI");
        if (explUI == null) Debug.LogError("ExplanationUI for Doc not found!");
        isExecuted = false;
        TaskSystem = (ScheduleBasedTaskSystem)Agent.TaskSystem;
    }

   /// <summary>
    /// Defines the behaviour of the agent, including animations and text
    /// </summary>
    public override void StartExplanation(){
        if(isExecuted == false){

            isExecuted = true;
            // Disable UI and wait for 2 secs to enable user to react and button to play "unpressed" sound
            StartCoroutine(DisableUiCoroutine());
            

            // introduction
            TaskSystem.ScheduleTask(new AgentAdaptiveGazeTask(true));
            TaskSystem.ScheduleTask(new SpeechTask("Hello and Welcome to MedicineVR!", 0.0001f));
            TaskSystem.ScheduleTask(new AgentAnimationTask("WaveRight", 2f, "WaveRight", "Right Arm"));
            TaskSystem.ScheduleTask(new SpeechTask("Firstly, let me introduce myself: I am Doctor Klamma, your guide through this fascinating journey of the human body!", 10f / TalkingSpeed));

            // Heart
            TaskSystem.Tasks.GoTo(HeartNavPoint);            
            TaskSystem.ScheduleTask(new SpeechTask("Let's start with the heart. This incredible organ is like the engine of your body, pumping blood and keeping you alive.", 0.0001f));
            TaskSystem.ScheduleTask(new AgentAnimationTask("PointingRight", 10f / TalkingSpeed, "PointingRight", "Right Arm", Heart));
            TaskSystem.ScheduleTask(new SpeechTask("It works tirelessly to distribute blood throughout your entire system.", 6f / TalkingSpeed)); 

            // Brain
            TaskSystem.Tasks.GoTo(BrainNavPoint);
            TaskSystem.ScheduleTask(new SpeechTask("Next, we have the brain. Think of it as the control center, managing everything from your thoughts to your movements.", 0.0001f));
            TaskSystem.ScheduleTask(new AgentAnimationTask("PointingRight", 10f / TalkingSpeed, "PointingRight", "Right Arm", Brain));
            TaskSystem.ScheduleTask(new SpeechTask("It is part of our nerve system and very important for regulating our body, while also giving us consciousness. How Awesome!", 10f / TalkingSpeed));

            // Liver
            TaskSystem.Tasks.GoTo(LiverNavPoint);
            TaskSystem.ScheduleTask(new SpeechTask("Here we have the liver, a powerhouse of metabolism.", 0.0001f));
            TaskSystem.ScheduleTask(new AgentAnimationTask("PointingRight", 5f / TalkingSpeed, "PointingRight", "Right Arm", Liver));
            TaskSystem.ScheduleTask(new SpeechTask("It processes nutrients and produces essential proteins. It also breaks down and excretes substances. Truly indispensable!", 10f / TalkingSpeed));

            // Lungs
            TaskSystem.Tasks.GoTo(LungsNavPoint);
            TaskSystem.ScheduleTask(new SpeechTask("And finally, the lungs.", 0.0001f / TalkingSpeed));
            TaskSystem.ScheduleTask(new AgentAnimationTask("PointingRight", 4f / TalkingSpeed, "PointingRight", "Right Arm", Lungs));
            TaskSystem.ScheduleTask(new SpeechTask("They draw in oxygen and expel carbon dioxide, keeping your blood oxygenated and your body functioning.", 9f / TalkingSpeed));

            TaskSystem.Tasks.GoTo(MiddleNavPoint1);
            TaskSystem.Tasks.GoTo(MiddleNavPoint2);

            TaskSystem.ScheduleTask(new SpeechTask("You can SELECT any organ by clicking it to learn more about it.", 1f / TalkingSpeed));
        }
    }

    private IEnumerator DisableUiCoroutine(){
        yield return new WaitForSeconds(2f);
        explUI.SetActive(false);
    }

    /// <summary>
    /// do nothing, as it is not used within the SelectionRoom
    /// </summary>
    public override void ShakeHead(){}

    /// <summary>
    /// do nothing, as it is not used within the SelectionRoom
    /// </summary>
    public override void Approve(){} // do nothing, as it is not used within the SelectionRoom
}