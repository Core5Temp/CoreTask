using UnityEngine;

public class Player : MonoBehaviour, IPoolable
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _speedBoosterIndicator;

    private Vector3 _targetPosition;

    public Transform Transform { get; private set; }

    private void Awake()
    {
        Transform = transform;

        BoosterController.SpeedModificatorChanged += _speedFactor =>
            _speedBoosterIndicator.gameObject.SetActive(BoosterController.SpeedBoosterActive);

        BoosterController.GodModeBoosterChanged +=
            isGoodMode => _spriteRenderer.color = isGoodMode ? Color.yellow : Color.white;
    }

    public void OnReturnToPool()
    {
        gameObject.SetActive(false);
        
        BoosterController.SpeedModificatorChanged -= SpeedModificatorChanged;
        BoosterController.GodModeBoosterChanged -= GodModeBoosterChanged;
        
        SpeedModificatorChanged(1f);
        GodModeBoosterChanged(false);
    }

    public void OnGetInPool()
    {
        gameObject.SetActive(true);
        
        BoosterController.SpeedModificatorChanged += SpeedModificatorChanged;
        BoosterController.GodModeBoosterChanged += GodModeBoosterChanged;
    }

    private void SpeedModificatorChanged(float speedFactor) =>
        _speedBoosterIndicator.gameObject.SetActive(BoosterController.SpeedBoosterActive);

    private void GodModeBoosterChanged(bool isGoodMode) =>
        _spriteRenderer.color = isGoodMode ? Color.yellow : Color.white;

    private void OnTriggerEnter2D(Collider2D other) =>
        CollisionController.OnTriggerEnter(other);
}