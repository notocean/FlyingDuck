using System.Collections;
using UnityEngine;

public class Crow : DialogueAnimal, ISaveableObject
{
    [SerializeField] float defaultFlySpeed;
    [SerializeField] float quickFlySpeed;
    [SerializeField] float escapeDistance;
    [SerializeField] float moveTime;

    Animator animator;
    Vector2 defaultPos, escapePos;
    bool isEscape = false;
    bool isMove = false;
    bool isDetech = false;
    float timer;

    protected override void Awake() {
        base.Awake();
        animator = GetComponent<Animator>();
        defaultPos = transform.position;
        escapePos = defaultPos + 4 * Vector2.up;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (canDialogue) {
            canDialogue = false;

            StartCoroutine(dialogueManager.DisplayDialogue(dialogues[dialogueIndex], delayWordTime));
            dialogueIndex = (dialogueIndex + 1) % dialogues.Count;
            StartCoroutine(CoolDown());
        }
        isDetech = true;
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (isMove) return;

        float distance = Vector2.Distance(transform.position, collision.transform.position);
        if (distance <= escapeDistance) {
            timer = 0f;
            isEscape = true;
            StartCoroutine(Move());
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        isDetech = false;
    }

    IEnumerator Move() {
        isMove = true;

        Vector2 startPos, endPos;
        float speed;

        if (isEscape) {
            speed = quickFlySpeed;
            startPos = defaultPos;
            endPos = escapePos;
        }
        else {
            speed = defaultFlySpeed;
            startPos = escapePos;
            endPos = defaultPos;
        }

        animator.SetFloat("FlySpeed", speed);

        while (timer < moveTime) {
            timer += Time.deltaTime;
            transform.position = Vector2.Lerp(startPos, endPos, timer / moveTime);
            yield return null;
        }

        if (isEscape) {
            yield return new WaitWhile(() => isDetech == true);
            timer = 0f;
            isEscape = false;
            yield return StartCoroutine(Move());
        }
        isMove = false;
    }

    public ObjectData GetObjectData() {
        return new CrowData(transform.position);
    }

    public void SetObjectData(ObjectData data) {
        CrowData _data = data as CrowData;
        transform.position = _data.pos;
    }
}

public class CrowData : ObjectData {
    public Vector2 pos { get; set; }

    public CrowData(Vector2 pos) {
        this.pos = pos;
    }

    public override ObjectData Clone() {
        return new CrowData(pos);
    }
}