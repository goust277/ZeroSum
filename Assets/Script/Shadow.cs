using UnityEngine;

public class Shadow : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private float maxA;

    private Color _color;
    void Start()
    {
        _color = spriteRenderer.color;
        maxA = spriteRenderer.color.a;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InPlayer(float time)
    {
        if (spriteRenderer.color.a > 0)
        {
            _color.a -= time * Time.deltaTime;

            spriteRenderer.color = _color;
        }
    }

    public void OutPlayer(float time)
    {
        if (spriteRenderer.color.a < maxA)
        {
            _color.a += time * Time.deltaTime;

            spriteRenderer.color = _color;
        }
    }

}
