using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class TutorialDialog : Dialog
{
    [SerializeField] Button nextButton;
    [SerializeField] Button endButton;

    [SerializeField] List<TutorialStep> tutorialSteps = new List<TutorialStep>();
    [SerializeField] TMP_Text instructionText;
    [SerializeField] TMP_Text skipText;
    [SerializeField] float timeToNextStep;      // Thời gian sau khi hoàn thành bước hiện tại để chuyển sang bước tiếp theo
    [SerializeField] int stepInSecondOpen;      // Tutorial sẽ mở bước này đầu tiên trừ lần đầu tiên mở tutorial

    Animator animator;
    GraphicRaycaster graphicRaycaster;
    PlayerInputHandler playerInputHandler;

    int currentStepIndex = 0;
    bool isHighlightEffect = false;
    bool isActionCompleted = false;
    Coroutine highlightCoroutine;

    protected override void Awake() {
        base.Awake();
        animator = GetComponent<Animator>();
        graphicRaycaster = GetComponent<GraphicRaycaster>();

        GameObject player = GameManager.Instance.Player;
        if (player != null ) {
            playerInputHandler = player.GetComponent<PlayerInputHandler>();
        }
    }

    void Start() {
        nextButton.onClick.AddListener(NextTutorial);
        endButton.onClick.AddListener(EndTutorial);

        if (GameSettings.Instance.isTutorial) {
            // Lần đầu mở
            StartCoroutine(PrepareOpen());
        }
        else {
            // Các lần mở sau
            currentStepIndex = stepInSecondOpen;
        }
    }

    IEnumerator PrepareOpen() {
        yield return new WaitUntil(() => Time.timeScale == 1);
        Open();
    }

    public override void Open() {
        GameSettings.Instance.isTutorial = false;
        GameManager.Instance.DoTutorial = true;
        animator.enabled = true;
        SetTutorial(currentStepIndex);
        base.Open();
    }

    void RestoreState() {
        isHighlightEffect = false;
        foreach (TutorialStep step in tutorialSteps) {
            if (step.hasHighlight) {
                step.canvasGroupHighlight.alpha = 0f;
            }
        }
    }

    public override void Close() {
        animator.Rebind();
        animator.Update(0f);
        animator.enabled = false;
        currentStepIndex = stepInSecondOpen;
        GameManager.Instance.DoTutorial = false;
        RestoreState();
        base.Close();
    }

    public void SetTutorial(int currentStepIndex) {
        this.currentStepIndex = currentStepIndex;

        if (currentStepIndex == tutorialSteps.Count - 1) {
            EndTutorial();
            return;
        }

        if (tutorialSteps[currentStepIndex].hasAction) {
            StartCoroutine(ActionHandle());
            animator.SetTrigger("Hide Tutorial");
        }
        else {
            animator.SetTrigger("Talk Instruction " + tutorialSteps[currentStepIndex].talkType.ToString());
            GameManager.Instance.gameState = GameState.Pause;
            graphicRaycaster.enabled = true;
        }

        if (isHighlightEffect) {
            isHighlightEffect = false;
            tutorialSteps[currentStepIndex - 1].canvasGroupHighlight.alpha = 0f;
            if (highlightCoroutine != null) {
                StopCoroutine(highlightCoroutine);
                highlightCoroutine = null;
            }
        }
        if (tutorialSteps[currentStepIndex].hasHighlight) {
            isHighlightEffect = true;
            highlightCoroutine = StartCoroutine(VisualHighlight(currentStepIndex, tutorialSteps[currentStepIndex].delayHighlightTime));
        }
    }

    IEnumerator VisualHighlight(int stepIndex, float delay) {
        yield return new WaitForSecondsRealtime(delay);
        tutorialSteps[stepIndex].canvasGroupHighlight.alpha = 1f;
        highlightCoroutine = null;
    }

    public void SetInstructionText() {
        instructionText.text = tutorialSteps[currentStepIndex].instructionText;
    }

    void NextTutorial() {
        SetTutorial(++currentStepIndex);
    }

    public void EndTutorial() {
        currentStepIndex = tutorialSteps.Count - 1;
        animator.SetTrigger("Hide Tutorial");
    }

    public void CloseTutorial() {
        if (currentStepIndex == tutorialSteps.Count - 1) {
            Close();
        }
    }

    public void ContinueGame() {
        GameManager.Instance.gameState = GameState.Play;
        graphicRaycaster.enabled = false;
    }

    IEnumerator ActionHandle() {
        isActionCompleted = false;

        StartAction();

        while (!isActionCompleted) {
            yield return null;
        }

        EndAction();

        yield return new WaitForSeconds(timeToNextStep);

        NextTutorial();
    }

    void StartAction() {
        switch (tutorialSteps[currentStepIndex].actionText) {
            case "Move":
                playerInputHandler.onWalk += WalkHandle;
                animator.SetFloat("Tutorial Remind Index", 0f);
                break;
            case "Fly":
                playerInputHandler.onFly += FlyHandle;
                animator.SetFloat("Tutorial Remind Index", 0.5f);
                break;
            case "Flash":
                playerInputHandler.onFlash += FlashHandle;
                animator.SetFloat("Tutorial Remind Index", 1f);
                break;
        }
    }

    void EndAction() {
        switch (tutorialSteps[currentStepIndex].actionText) {
            case "Move":
                playerInputHandler.onWalk -= WalkHandle;
                break;
            case "Fly":
                playerInputHandler.onFly -= FlyHandle;
                break;
            case "Flash":
                playerInputHandler.onFlash -= FlashHandle;
                break;
        }
    }

    void WalkHandle(bool isWalk, Vector2 dirMove) => CompleteAction();

    void FlyHandle(Vector2 dirMove) => CompleteAction();

    void FlashHandle(Vector2 dirMove) => CompleteAction();

    void CompleteAction() {
        isActionCompleted = true;
        animator.SetTrigger("Idle Tutorial");
    }
}
