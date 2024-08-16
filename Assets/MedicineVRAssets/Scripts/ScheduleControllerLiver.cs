using UnityEngine;
using i5.VirtualAgents;
using i5.VirtualAgents.ScheduleBasedExecution;
using System.Collections;
using i5.VirtualAgents.AgentTasks;

/// <summary>
/// Defines a Controller for Agent behaviour in LiverRoom
/// </summary>
public class ScheduleControllerLiver : ScheduleController
{
    ///<summary>The agent which is controlled by this controller, set in the inspector</summary>
    [SerializeField] private Agent Agent;

    ///<summary>Navigation Points</summary>
    [SerializeField] private GameObject QuizNavPoint1;
    [SerializeField] private GameObject QuizNavPoint2;

    // reference to explanation ui to deactivate on explanation start
    private GameObject explUI;

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

        do{
            currentName = "OrganToolTip" + i;
            currentParent = GameObject.Find(currentName);
            if(currentParent == null) break;
            currentToolTip = StaticUtils.FindObject(currentParent, "ToolTip");
            currentToolTip.SetActive(false);
            i++;
        }while(true);
    }

    /// <summary>
    /// Defines the behaviour of the agent, including animations and text
    /// </summary>
    public override void StartExplanation(){
        if(isExecuted == false){
            int currentToolTipIndex = 1;
            isExecuted = true;
            // Disable UI and wait for 2 secs to enable user to react and button to play "unpressed" sound
            StartCoroutine(DisableUiCoroutine());

            // introduction
            TaskSystem.ScheduleTask(new SpeechTask("So, you've chosen to learn more about the liver.", 0.0001f));
            TaskSystem.ScheduleTask(new AgentAnimationTask("WaveRight", 2f, "WaveRight", "Right Arm"));
            TaskSystem.ScheduleTask(new SpeechTask("I'm here to help you understand this incredible organ.", 6f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Let's get started!", 4f / TalkingSpeed));

            // Explanation
            // Introduction and Location
            TaskSystem.ScheduleTask(new SpeechTask("The liver is one of the largest and most vital organs in your body.", 7f / TalkingSpeed)); 
            TaskSystem.ScheduleTask(new SpeechTask("It is located in the upper right quadrant of your abdomen, just beneath the diaphragm.", 8f / TalkingSpeed));

            // Anatomy of the Liver
            TaskSystem.ScheduleTask(new SpeechTask("The liver is divided into two main lobes, the right and left lobes.", 2f / TalkingSpeed, currentToolTipIndex++)); // show left lobe
            TaskSystem.ScheduleTask(new SpeechTask("The liver is divided into two main lobes, the right and left lobes.", 5f / TalkingSpeed, currentToolTipIndex++)); // show right lobe
            TaskSystem.ScheduleTask(new SpeechTask("Each lobe is made up of smaller units called lobules.", 6f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("These lobules contain millions of cells called hepatocytes, which are the functional cells of the liver.", 9f / TalkingSpeed));

            // Components of the Liver
            TaskSystem.ScheduleTask(new SpeechTask("The liver is supplied with blood through two large vessels: the hepatic artery and the portal vein.", 9f / TalkingSpeed, currentToolTipIndex++)); // show inferior vena cava
            TaskSystem.ScheduleTask(new SpeechTask("The hepatic artery carries oxygen-rich blood from the heart.", 7f / TalkingSpeed, currentToolTipIndex++)); // show hepatic artery
            TaskSystem.ScheduleTask(new SpeechTask("The portal vein brings nutrient-rich blood from the intestines.", 7f / TalkingSpeed, currentToolTipIndex++)); // show portal vein
            TaskSystem.ScheduleTask(new SpeechTask("Inside the liver, these blood vessels branch into smaller capillaries, which run through the lobules.", 9f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Here, the hepatocytes filter the blood, removing toxins and storing nutrients.", 8f / TalkingSpeed));

            // Functions of the Liver
            TaskSystem.ScheduleTask(new SpeechTask("The liver has many crucial functions, including:", 6f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Metabolism: It processes carbohydrates, fats, and proteins from the food you eat.", 8f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("It converts them into energy and essential molecules.", 6f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Detoxification: It filters out toxins and harmful substances from the blood.", 8f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("This also includes alcohol and other drugs.", 6f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Production of Bile: It produces bile, which is stored in the gallbladder.", 8f / TalkingSpeed, currentToolTipIndex)); // show Gallbladder
            TaskSystem.ScheduleTask(new SpeechTask("Bile helps digest fats.", 4f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Storage: It stores vitamins and minerals such as vitamins A, D, E, K, and B12.", 9f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("It releases them when needed.", 4f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Synthesis of Blood Proteins: It produces important proteins like albumin.", 8f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Albumin helps maintain blood volume.", 5f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("It also produces clotting factors, which help stop bleeding.", 7f / TalkingSpeed));

            // System Affiliation
            TaskSystem.ScheduleTask(new SpeechTask("The liver is a key part of the digestive system.", 6f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("It works closely with other organs such as the gallbladder, pancreas, and intestines to digest, absorb, and process food.", 11f / TalkingSpeed));

            // Conclusion
            TaskSystem.ScheduleTask(new SpeechTask("Now you have a deeper understanding of the liver's structure, location, components, and functions.", 10f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Feel free to explore further or choose another organ to learn about.", 8f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("You can, for example, take a look at our interactible model to my right", 8f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("or explore parts of the liver by looking at the big, annotated liver behind me.", 9f / TalkingSpeed));
            TaskSystem.ScheduleTask(new SpeechTask("Remember, knowledge is the key to understanding our amazing bodies!", 8f / TalkingSpeed));

            // Quiz introduction
            TaskSystem.ScheduleTask(new SpeechTask("If you want to take a quiz regarding the liver, follow me!", 7f / TalkingSpeed));
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
    private IEnumerator DisableUiCoroutine(){
        yield return new WaitForSeconds(2f);
        explUI.SetActive(false);
    }
}