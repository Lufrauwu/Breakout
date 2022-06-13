using System;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Brick : MonoBehaviour
{
    public int _hitPoints = 1;
    private SpriteRenderer _spriteRenderer = default;
    [SerializeField] private ParticleSystem _destroyEffect = default;
    public static event Action<Brick> OnBrickDestroy;
    private AudioSource _audio;

    private void Awake()
    {
        this._spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Ball ball = col.gameObject.GetComponent<Ball>();
        CollisionLogic();
    }

    private void CollisionLogic()
    {
        this._hitPoints--;
        if (this._hitPoints <= 0)
        {
            BrickManager.Instance._remainingBricks.Remove(this);
            OnBrickDestroy?.Invoke(this);
            SpawnDestroyEffect();
            Destroy(this.gameObject);
        }
        else
        {
            this._spriteRenderer.sprite = BrickManager.Instance._sprites[this._hitPoints - 1];
        }
    }

    private void SpawnDestroyEffect()
    {
        Vector3 brickPos = gameObject.transform.position;
        Vector3 spawnPosition = new Vector3(brickPos.x, brickPos.y, brickPos.z - 0.2f);
        GameObject effect = Instantiate(_destroyEffect.gameObject, spawnPosition, Quaternion.identity);
        MainModule mainModule = effect.GetComponent<ParticleSystem>().main;
        mainModule.startColor = this._spriteRenderer.color;
        Destroy(effect, _destroyEffect.main.startLifetime.constant);
    }

    public void Init(Transform brickContainerTransform, Sprite sprite, Color color, int hitpoints)
    {
       this.transform.SetParent(brickContainerTransform);
       this._spriteRenderer.sprite = sprite;
       this._spriteRenderer.color = color;
       this._hitPoints = hitpoints;
    }
}
