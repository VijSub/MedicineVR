using UnityEngine;
using i5.VirtualAgents;
using i5.VirtualAgents.ScheduleBasedExecution;
using System.Collections;
using i5.VirtualAgents.AgentTasks;

/// <summary>
/// Defines a Controller for Agent behaviour in LungsRoom
/// </summary>
public class ScheduleControllerLungs : ScheduleController
{
    ///<summary>The agent which is controlled by this controller, set in the inspector</summary>
    [SerializeField]
    private Agent Agent;

    // reference to explanation ui to deactivate on explanation start
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

    // defines the sequence of tasks which the agent executes when started.
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
            TaskSystem.ScheduleTask(new SpeechTask("So, you've chosen to learn more about the lungs.", 0.0001f));
            TaskSystem.ScheduleTask(new AgentAnimationTask("WaveRight", 2f, "WaveRight", "Right Arm"));
            TaskSystem.ScheduleTask(new SpeechTask("I'm here to help you understand these incredible organs.", 6f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Let's get started!", 4f / TalkingSpeed));

            // Explanation
            // Introduction and Location
            TaskSystem.ScheduleTask(new SpeechTask("The lungs are essential organs for respiration.", 7f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("They are located in the chest, protected by the ribcage.", 6f / TalkingSpeed));

            // Anatomy of the Lungs
            TaskSystem.ScheduleTask(new SpeechTask("The lungs are divided into lobes: three on the right lung and two on the left lung.", 4f / TalkingSpeed, currentToolTipIndex++)); // show right lung
            TaskSystem.ScheduleTask(new SpeechTask("The lungs are divided into lobes: three on the right lung and two on the left lung.", 4f / TalkingSpeed, currentToolTipIndex++)); // show left lung
            TaskSystem.ScheduleTask(new SpeechTask("The right lung is larger than the left lung.", 6f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Each lung is made up of smaller units called alveoli, which are tiny air sacs where gas exchange occurs.", 10f / TalkingSpeed));

            // Components of the Lungs
            TaskSystem.ScheduleTask(new SpeechTask("The lungs are connected to the trachea (windpipe) via the bronchi.", 3.5f / TalkingSpeed, currentToolTipIndex++)); // show trachea
            TaskSystem.ScheduleTask(new SpeechTask("The lungs are connected to the trachea (windpipe) via the bronchi.", 1.75f / TalkingSpeed, currentToolTipIndex++)); // show bronchi
            TaskSystem.ScheduleTask(new SpeechTask("The lungs are connected to the trachea (windpipe) via the bronchi.", 1.75f / TalkingSpeed, currentToolTipIndex++)); // show bronchi
            TaskSystem.ScheduleTask(new SpeechTask("The bronchi branch into smaller tubes called bronchioles, which lead to the alveoli.", 8f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("The diaphragm, a muscle located below the lungs, plays a key role in breathing by contracting and relaxing to allow air in and out.", 12f / TalkingSpeed));

            // Functions of the Lungs
            TaskSystem.ScheduleTask(new SpeechTask("The lungs have several crucial functions, including:", 6f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Gas Exchange: They take in oxygen from the air and expel carbon dioxide from the blood.", 10f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("pH Balance: They help maintain the body's pH balance by regulating carbon dioxide levels.", 10f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Voice Production: The flow of air through the vocal cords allows us to speak.", 7f / TalkingSpeed, currentToolTipIndex)); // show vocal cords
            TaskSystem.ScheduleTask(new SpeechTask("Protection: They filter out small blood clots and air bubbles from the bloodstream.", 8f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Immune Defense: They contain immune cells that help protect against infections.", 7f / TalkingSpeed));

            // System Affiliation
            TaskSystem.ScheduleTask(new SpeechTask("The lungs are the primary organs of the respiratory system.", 6f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("They work closely with the cardiovascular system to circulate oxygenated blood throughout the body.", 11f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("The respiratory system is essential for providing oxygen to the body's tissues and removing waste gases.", 10f / TalkingSpeed));

            // Conclusion
            TaskSystem.ScheduleTask(new SpeechTask("Now you have a deeper understanding of the lungs' structure, location, components, and functions.", 10f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Feel free to explore further or choose another organ to learn about.", 8f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("You can, for example, take a look at our interactible model to my right", 8f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("or explore parts of the lungs by looking at the big, annotated lungs behind me.", 9f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Remember, knowledge is the key to understanding our amazing bodies!", 8f / TalkingSpeed));

            // Quiz introduction
            TaskSystem.ScheduleTask(new SpeechTask("If you want to take a quiz regarding the heart, follow me!", 7f / TalkingSpeed));
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
