using System.Collections;
using UnityEngine;

public class ShooterFlowerBullet : MonoBehaviour, IHidableObject, ISaveableObject, ITeleportable
{
    [SerializeField] NoControlEffect effect;
    [SerializeField] float speed;
    [SerializeField] float force;
    [SerializeField] float forceEffectTime;
    [SerializeField] float lifeTime;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] MapSideTeleportEvent mapSideTeleportEvent;
    [SerializeField] AudioClip shotAudioClip;

    Rigidbody2D rb2D;
    SpriteRenderer spriteRenderer;
    Collider2D col2D;
    bool isVisible = true;

    private void Awake() {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col2D = GetComponent<Collider2D>();
    }

    private void Start() {
        LevelDataManager.Instance.RegisterSaveableObject(name, this);
        mapSideTeleportEvent.RaiseRegisterEvent(transform);
    }

    public void Launch(Vector2 shootDir) {
        rb2D.velocity = speed * shootDir;
        StartCoroutine(CountToDestroy());
    }

    IEnumerator CountToDestroy() {
        yield return new WaitForSeconds(lifeTime);
        if (isVisible) {
            SetVisible(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if ((playerLayer.value & (1 << collision.gameObject.layer)) != 0) {
            Rigidbody2D playerRb = collision.GetComponentInParent<Rigidbody2D>();
            PlayerEffectHandler playerEffectHandler = collision.GetComponentInParent<PlayerEffectHandler>();
            StartCoroutine(ApplyForce(playerRb, playerEffectHandler));
        }
        else ObjectPoolingManager.Instance.GetPool("ShooterFlowerBullet").ReturnObject(gameObject);
        SoundFXManager.Instance.PlaySoundFX(shotAudioClip);
    }

    IEnumerator ApplyForce(Rigidbody2D playerRb, PlayerEffectHandler playerEffectHandler) {
        PlayerInfor playerInfor = playerEffectHandler.playerInfor;

        if (!playerInfor.IsImmune) {
            if (playerEffectHandler.playerInfor.OnGround)
                playerRb.AddForce(4 * force * new Vector2(rb2D.velocity.x < 0 ? -1 : 1, 0.1f));
            else playerRb.AddForce(force * rb2D.velocity);

            SetVisible(false);
            playerEffectHandler.AddEffect(effect);
            playerEffectHandler.GetComponent<PlayerController>().TakeDamage();

            yield return new WaitForSeconds(forceEffectTime);

            playerEffectHandler.RemoveEffect(effect);
            ObjectPoolingManager.Instance.GetPool("ShooterFlowerBullet").ReturnObject(gameObject);
        }
    }

    public void SetVisible(bool isVisible) {
        if (this.isVisible != isVisible) {
            this.isVisible = isVisible;
            spriteRenderer.enabled = isVisible;
            col2D.enabled = isVisible;
            if (!isVisible) rb2D.velocity = Vector2.zero;
        }
    }

    public ObjectData GetObjectData() {
        return new ShooterFlowerBulletData(isVisible, rb2D.transform.position, rb2D.velocity);
    }

    public void SetObjectData(ObjectData data) {
        ShooterFlowerBulletData _data = data as ShooterFlowerBulletData;

        if (_data.isVisible) {
            SetVisible(true);
            rb2D.transform.position = _data.pos;
            rb2D.velocity = _data.velocity;
        }
    }

    public void Teleport(Vector2 newPos) {
        rb2D.MovePosition(newPos);
    }
}

public class ShooterFlowerBulletData : ObjectData {
    public bool isVisible { get; }
    public Vector2 pos { get; }
    public Vector2 velocity { get; }

    public ShooterFlowerBulletData(bool isVisible, Vector2 pos, Vector2 velocity) {
        this.isVisible = isVisible;
        this.pos = pos;
        this.velocity = velocity;
    }
}