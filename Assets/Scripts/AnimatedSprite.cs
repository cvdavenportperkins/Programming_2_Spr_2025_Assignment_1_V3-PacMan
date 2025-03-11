using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class AnimatedSprite : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; } //set public read access, but private parameter setting

    public Sprite[] sprites;

    public float animationTime = 0.25f; //set delay between sprites 
    public int animationFrame { get; private set; } //set frame duration with public getter/private setter

    public bool loop = true;

    private void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void Start()
    {
        InvokeRepeating(nameof(Animate), this.animationTime, this.animationTime);

    }

    private void Animate()
    {
        if (!this.spriteRenderer.enabled)
        {
            return;
        }

        this.animationFrame++;

        if (this.animationFrame >= this.sprites.Length && this.loop)
        {
            this.animationFrame = 0;
        }

        if (this.animationFrame >= 0 && this.animationFrame < this.sprites.Length)
        {
            this.spriteRenderer.sprite = this.sprites[this.animationFrame]; 
        }
    }

    public void Restart()
    {
        this.animationFrame = -1;

        Animate();
    }

}
