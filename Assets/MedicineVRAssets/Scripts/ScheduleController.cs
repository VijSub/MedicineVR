using UnityEngine;

/// <summary>
/// Abstract base class for controlling schedule-based tasks of an agent.
/// </summary>
public abstract class ScheduleController : MonoBehaviour
{
    /// <summary>
    /// Starts the explanation sequence.
    /// </summary>
    public abstract void StartExplanation();

    /// <summary>
    /// Shakes the agent's head to indicate a wrong answer.
    /// </summary>
    public abstract void ShakeHead();

    /// <summary>
    /// Approves the answer to indicate it is correct.
    /// </summary>
    public abstract void Approve();
}
