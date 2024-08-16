using UnityEngine;
using i5.VirtualAgents;
using i5.VirtualAgents.ScheduleBasedExecution;
using System.Collections;
using i5.VirtualAgents.AgentTasks;

/// <summary>
/// Defines a Controller for Agent behaviour in HeartRoom
/// </summary>
public class ScheduleControllerHeart : ScheduleController
{
    ///<summary>The agent which is controlled by this controller, set in the inspector</summary>
    [SerializeField]
    private Agent Agent;

    //reference to explanation ui to deactivate on explanation start
    private GameObject explUI;

    ///<summary>Navigation Points</summary>
    [SerializeField] private GameObject QuizNavPoint1;
    [SerializeField] private GameObject QuizNavPoint2;

    ///<summary>The taskSystem of the agent</summary>
    protected ScheduleBasedTaskSystem TaskSystem;

    ///<summary>talking speed multiplier</summary>
    [Range(0.1f, 10f)] public float TalkingSpeed;

    // make UI button only "executable" once
    private bool isExecuted;

    // initializes the script
    protected void Start()
    {
        // initialize
        explUI = GameObject.Find("StartExplanationUI");
        if (explUI == null) Debug.LogError("ExplanationUI for Doc not found!");
        isExecuted = false;
        TaskSystem = (ScheduleBasedTaskSystem)Agent.TaskSystem;

        int i = 1;
        GameObject currentToolTip;
        GameObject currentParent;
        string currentName;

        do
        {
            currentName = "OrganToolTip" + i;
            currentParent = GameObject.Find(currentName);
            if (currentParent == null) break;
            currentToolTip = StaticUtils.FindObject(currentParent, "ToolTip");
            currentToolTip.SetActive(false);
            i++;
        } while (true);
    }

    /// <summary>
    /// Defines the behaviour of the agent, including animations and text
    /// </summary>
    public override void StartExplanation()
    {
        if (isExecuted == false)
        {
            int currentToolTipIndex = 1;
            isExecuted = true;
            // Disable UI and wait for 2 secs to enable user to react and button to play "unpressed" sound
            StartCoroutine(DisableUiCoroutine());

            // introduction
            TaskSystem.ScheduleTask(new SpeechTask("So, you've chosen to learn more about the heart.", 0.0001f));
            TaskSystem.ScheduleTask(new AgentAnimationTask("WaveRight", 2f, "WaveRight", "Right Arm"));
            TaskSystem.ScheduleTask(new SpeechTask("I'm here to help you understand this incredible organ.", 6f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Let's get started!", 4f / TalkingSpeed));

            // Explanation
            // Introduction and Location
            TaskSystem.ScheduleTask(new SpeechTask("The heart is one of the most vital organs in your body.", 7f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("It is located in the center of your chest, slightly to the left.", 6f / TalkingSpeed));

            // Anatomy of the Heart
            TaskSystem.ScheduleTask(new SpeechTask("The heart is divided into four chambers: two atria and two ventricles.", 2f / TalkingSpeed, currentToolTipIndex++)); // show right atrium
            TaskSystem.ScheduleTask(new SpeechTask("The heart is divided into four chambers: two atria and two ventricles.", 2f / TalkingSpeed, currentToolTipIndex++)); // show left atrium
            TaskSystem.ScheduleTask(new SpeechTask("The heart is divided into four chambers: two atria and two ventricles.", 2f / TalkingSpeed, currentToolTipIndex++)); // right ventricle
            TaskSystem.ScheduleTask(new SpeechTask("The heart is divided into four chambers: two atria and two ventricles.", 2f / TalkingSpeed, currentToolTipIndex++)); // left ventricle
            TaskSystem.ScheduleTask(new SpeechTask("The right atrium and right ventricle form the right side of the heart, while the left atrium and left ventricle form the left side.", 12f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("These chambers are separated by valves that ensure blood flows in the correct direction.", 9f / TalkingSpeed));

            // Components of the Heart
            TaskSystem.ScheduleTask(new SpeechTask("The heart's walls are made up of three layers: the epicardium, myocardium, and endocardium.", 10f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("The myocardium, the thick middle layer, is composed of cardiac muscle tissue that contracts to pump blood.", 12f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("The heart is supplied with oxygen-rich blood through the coronary arteries.", 4f / TalkingSpeed, currentToolTipIndex++)); // show righ coronary artery
            TaskSystem.ScheduleTask(new SpeechTask("The heart is supplied with oxygen-rich blood through the coronary arteries.", 4f / TalkingSpeed, currentToolTipIndex++)); // show left coronary artery

            TaskSystem.ScheduleTask(new SpeechTask("The electrical conduction system of the heart includes the sinoatrial (SA) node, the atrioventricular (AV) node, and the His-Purkinje network, which coordinate the heartbeat.", 14f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("The SA node is the pacemaker of the heart, initiating an electrical signal which is traveling trough the AV node to the His-Purkinke network.", 13f / TalkingSpeed));

            // Functions of the Heart
            TaskSystem.ScheduleTask(new SpeechTask("The heart has several crucial functions, including:", 6f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Pumping Blood: It circulates blood throughout the body, delivering oxygen and nutrients to tissues and removing waste products.", 12f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Maintaining Blood Pressure: It helps regulate blood pressure by adjusting the force and rate of contractions.", 10f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Oxygenation: The heart pumps deoxygenated blood to the lungs where it receives oxygen and releases carbon dioxide.", 12f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Circulating Hormones: It helps circulate hormones and other important substances throughout the body.", 10f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Homeostasis: It plays a key role in maintaining homeostasis, ensuring stable internal conditions.", 10f / TalkingSpeed));

            // System Affiliation
            TaskSystem.ScheduleTask(new SpeechTask("The heart is the central organ of the circulatory system.", 6f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("It works closely with blood vessels, such as arteries, veins, and capillaries, to ensure efficient blood flow.", 12f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("The circulatory system is essential for delivering oxygen and nutrients while removing waste products from the body.", 10f / TalkingSpeed));

            // Conclusion
            TaskSystem.ScheduleTask(new SpeechTask("Now you have a deeper understanding of the heart's structure, location, components, and functions.", 10f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Feel free to explore further or choose another organ to learn about.", 8f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("You can, for example, take a look at our interactible model to my right", 8f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("or explore parts of the heart by looking at the big, annotated heart behind me.", 9f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Remember, knowledge is the key to understanding our amazing bodies!", 8f / TalkingSpeed));

            // Quiz introduction
            TaskSystem.ScheduleTask(new SpeechTask("If you want to take a quiz regarding the Heart, follow me!", 7f / TalkingSpeed));
            TaskSystem.Tasks.GoTo(QuizNavPoint1);
            TaskSystem.Tasks.GoTo(QuizNavPoint2);
        }
    }

    /// <summary>
    /// Defines the reaction of the agent when a quiz question is answered wrongly
    /// </summary>
    public override void ShakeHead(){
        // only enable headshaking when next to the quiz ui
        if(Vector3.Distance(Agent.transform.position, QuizNavPoint2.transform.position) < 0.5){
            TaskSystem.ScheduleTask(new SpeechTask("Sadly, this is wrong.", 0.1f));
            TaskSystem.ScheduleTask(new AgentAnimationTask("ShakeHead", 2f, "ShakeHead", "Head"));
            TaskSystem.ScheduleTask(new SpeechTask("Next question:", 0.1f));
        }
    }

    /// <summary>
    /// Defines the reaction of the agent when a quiz question is answered correctly
    /// </summary>
    public override void Approve(){
        // only enable approving, when next to the quiz ui
        if(Vector3.Distance(Agent.transform.position, QuizNavPoint2.transform.position) < 0.5){
            TaskSystem.ScheduleTask(new SpeechTask("Correct, very good!", 2f));
            TaskSystem.ScheduleTask(new SpeechTask("Next question:", 0.1f));
        }
    }

    // Coroutine for disabling the UI element starting the explanation
    private IEnumerator DisableUiCoroutine()
    {
        yield return new WaitForSeconds(2f);
        explUI.SetActive(false);
    }
}
