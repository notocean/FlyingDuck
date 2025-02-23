using UnityEngine;
using UnityEngine.Events;

[ExecuteAlways]
public class DuckInfor : MonoBehaviour
{
    [SerializeField] float maxEnergy = 4;
    private float currEnergy = 0;
    [SerializeField] float defaultEnergySpeed;
    private float currEnergySpeed;

    [HideInInspector] public UnityEvent<float> energyChanged = new UnityEvent<float>();
    public bool canIncreaseEnergy { get; private set; }

    private Rigidbody2D rb2d;
    bool isImmune = false;

    private void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        currEnergySpeed = defaultEnergySpeed;
        canIncreaseEnergy = true;
    }

    private void Update() {
        ChangeEnergy();
    }

    public float GetMaxEnergy() {
        return maxEnergy;
    }

    public bool EnoughEnergy(float value) {
        return currEnergy >= value;
    }

    public void UseEnergy(float value) {
        currEnergy -= value;
        energyChanged.Invoke(currEnergy);
    }

    public void ChangeEnergySpeed(float factor) {
        currEnergySpeed = defaultEnergySpeed * factor;
    }

    public void ResetEnergySpeed() {
        currEnergySpeed = defaultEnergySpeed;
    }

    void ChangeEnergy() {
        if (!canIncreaseEnergy)
            return;
        currEnergy = Mathf.Clamp(currEnergy + currEnergySpeed * Time.deltaTime, 0, maxEnergy);
        energyChanged.Invoke(currEnergy);
    }

    public void SetDuckData(DuckData duckData) {
        transform.position = duckData.pos;
        transform.localScale = new Vector3((int)duckData.viewDir, 1, 1);
        rb2d.velocity = duckData.velocity;
        currEnergy = duckData.energy;
    }

    public DuckData GetDuckData() {
        DuckData duckData = new DuckData();
        duckData.pos = transform.position;
        duckData.viewDir = (DuckDir)transform.localScale.x;
        duckData.velocity = rb2d.velocity;
        duckData.energy = currEnergy;
        
        return duckData;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision != null) {
            Animal animal = collision.GetComponent<Animal>();
            if (animal != null) {
                int food = animal.Collected();
                PlayerData.Instance.Food += food;
            }
        }
    }

    public void SetImmune(bool immune) {
        this.isImmune = immune;
    }
}
