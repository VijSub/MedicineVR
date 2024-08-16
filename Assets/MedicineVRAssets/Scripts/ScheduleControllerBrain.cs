using UnityEngine;
using i5.VirtualAgents;
using i5.VirtualAgents.ScheduleBasedExecution;
using System.Collections;
using i5.VirtualAgents.AgentTasks;

/// <summary>
/// Controls the schedule-based tasks for an agent, focusing on explaining the brain.
/// </summary>
public class ScheduleControllerBrain : ScheduleController
{
    /// <summary>
    /// The agent controlled by this controller, set in the inspector.
    /// </summary>
    [SerializeField]
    private Agent Agent;

    /// <summary>
    /// Reference to the explanation UI to deactivate on explanation start.
    /// </summary>
    private GameObject explUI;

    /// <summary>
    /// Navigation points for the quiz.
    /// </summary>
    [SerializeField] private GameObject QuizNavPoint1;
    [SerializeField] private GameObject QuizNavPoint2;

    /// <summary>
    /// The task system of the agent.
    /// </summary>
    protected ScheduleBasedTaskSystem TaskSystem;

    /// <summary>
    /// Talking speed multiplier.
    /// </summary>
    [Range(0.1f, 10f)] public float TalkingSpeed;

    /// <summary>
    /// Ensures the UI button is only executable once.
    /// </summary>
    private bool isExecuted;

    /// <summary>
    /// Initializes the schedule controller, setting up UI and tooltips.
    /// </summary>
    protected void Start()
    {
        // Initialize
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
    /// Starts the explanation sequence for the brain.
    /// </summary>
    public override void StartExplanation()
    {
        if (!isExecuted)
        {
            int currentToolTipIndex = 1;
            isExecuted = true;
            // Disable UI and wait for 2 seconds to enable user to react and button to play "unpressed" sound
            StartCoroutine(DisableUiCoroutine());

            // Introduction
            TaskSystem.ScheduleTask(new SpeechTask("So, you've chosen to learn more about the brain.", 0.0001f));
            TaskSystem.ScheduleTask(new AgentAnimationTask("WaveRight", 2f, "WaveRight", "Right Arm"));
            TaskSystem.ScheduleTask(new SpeechTask("I'm here to help you understand this incredible organ.", 6f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Let's get started!", 4f / TalkingSpeed));

            // Explanation
            // Introduction and Location
            TaskSystem.ScheduleTask(new SpeechTask("The brain is one of the most complex and vital organs in your body.", 7f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("It is located in the skull, which protects it from injury.", 6f / TalkingSpeed));

            // Anatomy of the Brain
            TaskSystem.ScheduleTask(new SpeechTask("The brain is divided into three main parts: the cerebrum, the cerebellum, and the brainstem.", 8f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("The cerebrum is the largest part, responsible for cognitive functions like thinking, memory, and emotion.", 12f / TalkingSpeed, currentToolTipIndex++)); // show cerebrum
            TaskSystem.ScheduleTask(new SpeechTask("The cerebellum is located under the cerebrum and controls balance and coordination.", 9f / TalkingSpeed, currentToolTipIndex++)); // show cerebrellum
            TaskSystem.ScheduleTask(new SpeechTask("The brainstem connects the brain to the spinal cord and controls vital functions such as breathing, heart rate, and blood pressure.", 12f / TalkingSpeed, currentToolTipIndex++)); // show brainstem

            // Components of the Brain
            TaskSystem.ScheduleTask(new SpeechTask("The brain is made up of billions of nerve cells called neurons.", 7f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Neurons communicate with each other through electrical impulses and chemical signals.", 9f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("The brain is also protected and nourished by cerebrospinal fluid, which surrounds it.", 10f / TalkingSpeed));

            // Functions of the Brain
            TaskSystem.ScheduleTask(new SpeechTask("The brain has many crucial functions, including:", 6f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Cognition: It processes thoughts, memories, and emotions.", 8f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Movement: It controls voluntary movements through the motor cortex.", 8f / TalkingSpeed, currentToolTipIndex)); // show motor cortex
            TaskSystem.ScheduleTask(new SpeechTask("Sensory Processing: It interprets sensory information from the eyes, ears, skin, and other organs.", 10f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Autonomic Functions: It regulates automatic functions like breathing, heart rate, and digestion.", 9f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Homeostasis: It maintains the body's internal balance and regulates sleep cycles.", 8f / TalkingSpeed));

            // System Affiliation
            TaskSystem.ScheduleTask(new SpeechTask("The brain is the central organ of the nervous system.", 6f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("It works closely with the spinal cord and peripheral nerves to control the body.", 10f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("The nervous system is essential for sensing the environment, processing information, and responding to stimuli.", 10f / TalkingSpeed));

            // Conclusion
            TaskSystem.ScheduleTask(new SpeechTask("Now you have a deeper understanding of the brain's structure, location, components, and functions.", 10f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Feel free to explore further or choose another organ to learn about.", 8f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("You can, for example, take a look at our interactible model to my right", 8f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("or explore parts of the brain by looking at the big, annotated brain behind me.", 9f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Remember, knowledge is the key to understanding our amazing bodies!", 8f / TalkingSpeed));

            // Quiz introduction
            TaskSystem.ScheduleTask(new SpeechTask("If you want to take a quiz regarding the brain, follow me!", 7f / TalkingSpeed));
            TaskSystem.Tasks.GoTo(QuizNavPoint1);
            TaskSystem.Tasks.GoTo(QuizNavPoint2);
        }
    }

    /// <summary>
    /// Shakes the agent's head to indicate a wrong answer.
    /// </summary>
    public override void ShakeHead()
    {
        // Only enable headshaking when next to the quiz UI
        if (Vector3.Distance(Agent.transform.position, QuizNavPoint2.transform.position) < 0.5)
        {
            TaskSystem.ScheduleTask(new SpeechTask("Sadly, this is wrong.", 0.1f));
            TaskSystem.ScheduleTask(new AgentAnimationTask("ShakeHead", 2f, "ShakeHead", "Head"));
            TaskSystem.ScheduleTask(new SpeechTask("Next question:", 0.1f));
        }
    }

    /// <summary>
    /// Approves the answer to indicate it is correct.
    /// </summary>
    public override void Approve()
    {
        // Only enable approving when next to the quiz UI
        if (Vector3.Distance(Agent.transform.position, QuizNavPoint2.transform.position) < 0.5)
        {
            TaskSystem.ScheduleTask(new SpeechTask("Correct, very good!", 2f));
            TaskSystem.ScheduleTask(new SpeechTask("Next question:", 0.1f));
        }
    }

    /// <summary>
    /// Coroutine for disabling the UI element starting the explanation.
    /// </summary>
    /// <returns>IEnumerator for coroutine.</returns>
    private IEnumerator DisableUiCoroutine()
    {
        yield return new WaitForSeconds(2f);
        explUI.SetActive(false);
    }
}
