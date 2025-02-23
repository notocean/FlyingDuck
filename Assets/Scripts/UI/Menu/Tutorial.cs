using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : Dialog
{
    Animator animator;
    [SerializeField] Button nextButton;
    [SerializeField] Button skipButton;
    [SerializeField] int jumpTutorialIndex;

    int tutorialIndex;

    protected override void Awake() {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    private void Start() {
        nextButton.onClick.AddListener(NextTutorial);
        skipButton.onClick.AddListener(SkipTutorial);

        if (GameSettings.Instance.IsTutorial) {
            StartCoroutine(PrepareOpen());
        }
    }

    IEnumerator PrepareOpen() {
        yield return new WaitUntil(() => Time.timeScale == 1);
        Open();
    }

    public override void Open() {
        GameSettings.Instance.IsTutorial = false;
        animator.enabled = true;
        base.Open();
    }

    public override void Close() {
        animator.Rebind();
        animator.Update(0f);
        animator.enabled = false;
        tutorialIndex = 0;
        base.Close();
    }

    public void SetTutorial(int tutorialIndex) {
        this.tutorialIndex = tutorialIndex;
        animator.SetInteger("Tutorial Index", tutorialIndex);
    }

    private void NextTutorial() {
        SetTutorial(++tutorialIndex);
    }

    public void SkipTutorial() {
        animator.SetTrigger("End Tutorial");
    }

    public void EndTutorial() {
        Close();
    }
}
