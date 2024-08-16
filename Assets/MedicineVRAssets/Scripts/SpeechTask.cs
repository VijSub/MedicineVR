using System.Collections;
using System.Collections.Generic;
using i5.VirtualAgents;
using i5.VirtualAgents.AgentTasks;
using Microsoft.MixedReality.Toolkit.UI;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Custom Virtual Agent task, using a ToolTip to make the agent have a "speechbubble", talk, 
/// and be able to activate ToolTips on big organs when saying a certain sentence
/// </summary>
public class SpeechTask : AgentWaitTask
{
    private string SpeechContent;
    private int OrganTooltipIndex;

    /// <summary>
    /// constructs a SpeechTask without referring to an organ tooltip and calls the constructor of AgentWaitTask
    /// </summary>
    /// <param name="SpeechContent">Text, which shall be displayed by agent</param>
    /// <param name="WaitTimeInSeconds">How long the agent should wait</param>
    public SpeechTask(string SpeechContent, float WaitTimeInSeconds) : base(WaitTimeInSeconds){
        this.SpeechContent = SpeechContent;
        this.OrganTooltipIndex = 0;
    }

    /// <summary>
    /// constructs a SpeechTask while enabling the ToolTip with given index in the parameters
    /// </summary>
    /// <param name="SpeechContent">Text, which shall be displayed by agent</param>
    /// <param name="WaitTimeInSeconds">How long the agent should wait</param>
    /// <param name="OrganTooltipIndex">Index of the ToolTip to be activated (1 means OrganToolTip1 will be activated)</param>
    // 
    public SpeechTask(string SpeechContent, float WaitTimeInSeconds, int OrganTooltipIndex) : base(WaitTimeInSeconds){
        this.SpeechContent = SpeechContent;
        this.OrganTooltipIndex = OrganTooltipIndex;
    }

    /// <summary>
    /// defines the behaviour on execution. Sets SpeechBubble (Bezier ToolTip) text
    /// </summary>
    /// <param name="agent">Agent on which the task should be executed</param>
    public override void StartExecution(Agent agent)
    {
        ToolTip toolTip = agent.GetComponentInChildren<ToolTip>();
        if (toolTip != null){
            toolTip.ToolTipText = SpeechContent;
            EnableToolTip();
            base.StartExecution(agent);
        }else{
            Debug.LogWarning("Agent has no Tooltip Component. SpeechTask will be skipped! (SpeechTask.StartExecution)");
        }
    }

    // Enables the organ tooltip corresponding to a speechtask
    private void EnableToolTip(){

        if(OrganTooltipIndex > 0){

            string toolTipName = "OrganToolTip" + OrganTooltipIndex;

            GameObject currToolTip = StaticUtils.FindObject(GameObject.Find(toolTipName), "ToolTip"); // find disabled tooltip child

            if(currToolTip == null){
                Debug.LogWarning(toolTipName + " does not exist!");

            }else currToolTip.SetActive(true);

        }else if(OrganTooltipIndex < 0) Debug.LogWarning("OrganToolTipIndex cannot have a negative Index. Index: " + OrganTooltipIndex);
    }
}
