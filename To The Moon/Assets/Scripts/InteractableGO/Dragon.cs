using UnityEngine;

public class Dragon : Interactive, IEnemy
{
    [SerializeField] private GameObject _explode;
    [SerializeField] float _speed;
    public void EnemyAction()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * _speed);
    }

    protected override void Init()
    {
        _explodeParticles = _explode;
        Destroy(gameObject, 20f);
    }

    private void OnDestroy()
    {
        GameManager.Instance.RemoveEnemy(GetComponent<Interactive>());
    }

    protected override void InteractWithPlayer(GameObject player)
    {
        player.GetComponent<Rocket>().ReactOnObstacle();
    }
}