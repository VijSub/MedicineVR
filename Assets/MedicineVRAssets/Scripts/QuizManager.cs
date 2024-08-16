using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.MixedReality.Toolkit.UI;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

/// <summary>
/// Manages the quiz functionality including loading questions, displaying them, handling user answers, and calculating scores.
/// </summary>
public class QuizManager : MonoBehaviour
{
    /// <summary>
    /// TextMeshPro object for displaying the question text.
    /// </summary>
    public TextMeshPro questionText;

    /// <summary>
    /// GameObject representing the first answer button.
    /// </summary>
    public GameObject answer1;

    /// <summary>
    /// GameObject representing the second answer button.
    /// </summary>
    public GameObject answer2;

    /// <summary>
    /// GameObject representing the third answer button.
    /// </summary>
    public GameObject answer3;

    /// <summary>
    /// TextMeshPro object for displaying feedback messages (e.g., correct or wrong).
    /// </summary>
    public TextMeshPro feedbackText;

    // Private fields for TextMeshPro components of the answer buttons
    private TextMeshPro answerText1;
    private TextMeshPro answerText2;
    private TextMeshPro answerText3;

    // List to store questions loaded from JSON
    private List<Question> questions;
    private int currentQuestionIndex; // Index of the current question
    private int score; // Score counter

    private ScheduleController SchedCtrl;

    /// <summary>
    /// Represents a single quiz question.
    /// </summary>
    [System.Serializable]
    public class Question
    {
        /// <summary>
        /// The text of the question.
        /// </summary>
        public string question;

        /// <summary>
        /// Array of possible answers.
        /// </summary>
        public string[] answers;

        /// <summary>
        /// Index of the correct answer in the 'answers' array.
        /// </summary>
        public int correctAnswer;
    }

    void Start()
    {
        // Initialize the TextMeshPro components of the answer buttons
        answerText1 = answer1.transform.Find("IconAndText/TextMeshPro").GetComponent<TextMeshPro>();
        answerText2 = answer2.transform.Find("IconAndText/TextMeshPro").GetComponent<TextMeshPro>();
        answerText3 = answer3.transform.Find("IconAndText/TextMeshPro").GetComponent<TextMeshPro>();
        SchedCtrl = GameObject.Find("ScheduleController").GetComponent<ScheduleController>();

        // Debugging: Check if the components are correctly initialized
        Debug.Log("Start method called.");
        if (answerText1 == null) Debug.LogError("answerText1 is null");
        if (answerText2 == null) Debug.LogError("answerText2 is null");
        if (answerText3 == null) Debug.LogError("answerText3 is null");
        if (questionText == null) Debug.LogError("questionText is null");
        if (feedbackText == null) Debug.LogError("feedbackText is null");
        if (SchedCtrl == null) Debug.LogError("ScheduleController is null");
        if (answer1.GetComponent<Interactable>() == null) Debug.LogError("Interactable component on answer1 is null");
        if (answer2.GetComponent<Interactable>() == null) Debug.LogError("Interactable component on answer2 is null");
        if (answer3.GetComponent<Interactable>() == null) Debug.LogError("Interactable component on answer3 is null");

        // Set the start screen
        answerText1.text = "Start the Quiz"; // Set the start button text
        answerText2.text = "";
        answerText3.text = "";
        answer2.SetActive(false); // Disable the other answer buttons
        answer3.SetActive(false);
        feedbackText.text = "";

        // Remove any previous event listeners (if any)
        answer1.GetComponent<Interactable>().OnClick.RemoveAllListeners();
        // Add an event listener for the start button
        answer1.GetComponent<Interactable>().OnClick.AddListener(OnStartButtonClicked);

        // Load the questions at the start
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        LoadQuestions(sceneName);
    }

    /// <summary>
    /// Event handler for when the start button is clicked.
    /// </summary>
    void OnStartButtonClicked()
    {
        answer1.GetComponent<Interactable>().OnClick.RemoveAllListeners(); // Prevent multiple clicks
        StartQuiz();
    }

    /// <summary>
    /// Initializes and starts the quiz.
    /// </summary>
    void StartQuiz()
    {
        Debug.Log("StartQuiz method called.");

        // Shuffle the questions and answers for randomness
        ShuffleQuestions();
        currentQuestionIndex = 0; // Reset the question index
        score = 0; // Reset the score

        // Remove the start event listener
        answer1.GetComponent<Interactable>().OnClick.RemoveAllListeners();
        answer2.SetActive(true); // Enable the other answer buttons
        answer3.SetActive(true);

        // Display the first question
        DisplayQuestion();
    }

    /// <summary>
    /// Loads quiz questions from a JSON file based on the scene name.
    /// </summary>
    /// <param name="sceneName">The name of the scene to determine the quiz file.</param>
    void LoadQuestions(string sceneName)
    {
        Debug.Log("LoadQuestions method called.");

        // Use the standard StreamingAssets path
        string fileName = sceneName + "Quiz.json"; // Construct the file name based on the scene
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName); // Construct the full file path

        Debug.Log("Loading questions from: " + filePath);

        if (Application.platform == RuntimePlatform.Android)
        {
            StartCoroutine(LoadQuestionsFromStreamingAssets(filePath));
        }
        else
        {
            LoadQuestionsFromFile(filePath);
        }
    }

    /// <summary>
    /// Loads quiz questions from a local file.
    /// </summary>
    /// <param name="filePath">The full path to the quiz JSON file.</param>
    void LoadQuestionsFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath); // Read the file contents
            ParseQuestions(jsonData);
        }
        else
        {
            Debug.LogError("Quiz file not found: " + filePath);
            questions = new List<Question>(); // Initialize an empty list if the file is not found
        }
    }

    /// <summary>
    /// Loads quiz questions asynchronously from the StreamingAssets folder.
    /// </summary>
    /// <param name="filePath">The full path to the quiz JSON file.</param>
    /// <returns>An IEnumerator for the coroutine.</returns>
    private IEnumerator LoadQuestionsFromStreamingAssets(string filePath)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(filePath))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string jsonData = www.downloadHandler.text;
                ParseQuestions(jsonData);
            }
            else
            {
                Debug.LogError("Error loading JSON file: " + www.error);
                questions = new List<Question>(); // Initialize an empty list if the file is not found
            }
        }
    }

    /// <summary>
    /// Parses JSON data into Question objects and validates them.
    /// </summary>
    /// <param name="jsonData">The JSON data to parse.</param>
    private void ParseQuestions(string jsonData)
    {
        var tempQuestions = JArray.Parse(jsonData).ToObject<List<Question>>(); // Parse JSON data into a list of Question objects
        questions = new List<Question>();

        // Validate each question
        foreach (var question in tempQuestions)
        {
            if (question.answers.Length == 3)
            {
                questions.Add(question);
            }
            else
            {
                Debug.LogError($"Invalid question format: {question.question}. It must have exactly 3 answers.");
            }
        }

        if (questions.Count == 0)
        {
            Debug.LogError("No valid questions found in the quiz file.");
        }
        else
        {
            Debug.Log("Questions loaded successfully.");
        }
    }

    /// <summary>
    /// Shuffles the order of questions and answers for randomness.
    /// </summary>
    void ShuffleQuestions()
    {
        Debug.Log("ShuffleQuestions method called.");

        // Shuffle the order of the questions using Fisher-Yates shuffle algorithm
        for (int i = 0; i < questions.Count; i++)
        {
            Question temp = questions[i];
            int randomIndex = Random.Range(i, questions.Count);
            questions[i] = questions[randomIndex];
            questions[randomIndex] = temp;
        }

        // Shuffle the order of the answers within each question
        foreach (var question in questions)
        {
            List<string> answers = new List<string>(question.answers);
            string correctAnswer = answers[question.correctAnswer];
            for (int i = answers.Count - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                string temp = answers[i];
                answers[i] = answers[randomIndex];
                answers[randomIndex] = temp;
            }
            question.answers = answers.ToArray();
            question.correctAnswer = answers.IndexOf(correctAnswer);
        }
    }

    /// <summary>
    /// Displays the current question and sets up event listeners for answer buttons.
    /// </summary>
    void DisplayQuestion()
    {
        Debug.Log("DisplayQuestion method called.");
        if (currentQuestionIndex < questions.Count)
        {
            Question currentQuestion = questions[currentQuestionIndex]; // Get the current question
            questionText.text = currentQuestion.question; // Display the question text
            answerText1.text = currentQuestion.answers[0]; // Display the first answer
            answerText2.text = currentQuestion.answers[1]; // Display the second answer
            answerText3.text = currentQuestion.answers[2]; // Display the third answer

            Debug.Log("Current question: " + currentQuestion.question);
            Debug.Log("Answer1: " + currentQuestion.answers[0]);
            Debug.Log("Answer2: " + currentQuestion.answers[1]);
            Debug.Log("Answer3: " + currentQuestion.answers[2]);

            // Add new event listeners for answer buttons
            answer1.GetComponent<Interactable>().OnClick.AddListener(() => OnAnswerSelected(0));
            answer2.GetComponent<Interactable>().OnClick.AddListener(() => OnAnswerSelected(1));
            answer3.GetComponent<Interactable>().OnClick.AddListener(() => OnAnswerSelected(2));
        }
        else
        {
            EndQuiz(); // End the quiz if there are no more questions
        }
    }

    /// <summary>
    /// Handles the user's answer selection and provides feedback.
    /// </summary>
    /// <param name="index">The index of the selected answer.</param>
    void OnAnswerSelected(int index)
    {
        Debug.Log("OnAnswerSelected method called with index: " + index);

        // Remove the listeners immediately after a click to prevent multiple clicks
        answer1.GetComponent<Interactable>().OnClick.RemoveAllListeners();
        answer2.GetComponent<Interactable>().OnClick.RemoveAllListeners();
        answer3.GetComponent<Interactable>().OnClick.RemoveAllListeners();

        Question currentQuestion = questions[currentQuestionIndex]; // Get the current question
        if (index == currentQuestion.correctAnswer)
        {
            feedbackText.text = "Correct!"; // Display correct feedback
            score++; // Increment the score
            SchedCtrl.Approve();
        }
        else
        {
            feedbackText.text = "Wrong!"; // Display wrong feedback
            SchedCtrl.ShakeHead();
        }

        StartCoroutine(NextQuestion()); // Wait for a moment before displaying the next question
    }

    /// <summary>
    /// Waits for a moment before displaying the next question.
    /// </summary>
    IEnumerator NextQuestion()
    {
        yield return new WaitForSeconds(2); // Wait for 2 seconds
        feedbackText.text = ""; // Clear the feedback text
        currentQuestionIndex++; // Move to the next question
        DisplayQuestion(); // Display the next question
    }

    /// <summary>
    /// Ends the quiz and displays the final score.
    /// </summary>
    void EndQuiz()
    {
        Debug.Log("EndQuiz method called.");

        // Display the final score
        questionText.text = $"Quiz over! You scored {score} out of {questions.Count}.";
        answerText1.text = "Restart the Quiz"; // Set the restart button text
        answerText2.text = "";
        answerText3.text = "";
        answer2.SetActive(false); // Disable the other answer buttons
        answer3.SetActive(false);

        // Remove any previous event listeners (if any)
        answer1.GetComponent<Interactable>().OnClick.RemoveAllListeners();
        // Add an event listener for the restart button
        answer1.GetComponent<Interactable>().OnClick.AddListener(OnRestartButtonClicked);
    }

    /// <summary>
    /// Restarts the quiz from the beginning.
    /// </summary>
    void OnRestartButtonClicked()
    {
        answer1.GetComponent<Interactable>().OnClick.RemoveAllListeners(); // Prevent multiple clicks
        StartQuiz();
    }
}
