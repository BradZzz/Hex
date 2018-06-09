using UnityEngine;

[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour {
  public Color defaultColor = Color.clear;
  public Color enemyColor = Color.clear;

  private Color color;
  private SpriteRenderer spriteRenderer;

  void Awake()
  {
    color = defaultColor;
    //color = Color.red;
  }

  public void setColor(bool enemy) {
    if (enemy) {
      color = enemyColor;
    } else {
      color = defaultColor;
    }
  }

  public void setColor(Color banner) {
    color = banner;
  }

  public Color getColor() {
    return color;
  }

  void OnEnable() {
    spriteRenderer = GetComponent<SpriteRenderer>();
    UpdateOutline(true);
  }

  void OnDisable() {
    UpdateOutline(false);
  }

  void Update() {
    UpdateOutline(true);
  }

  void UpdateOutline(bool outline) {
    MaterialPropertyBlock mpb = new MaterialPropertyBlock();
    spriteRenderer.GetPropertyBlock(mpb);
    mpb.SetFloat("_Outline", outline ? 1f : 0);
    mpb.SetColor("_OutlineColor", Color.red);
    spriteRenderer.SetPropertyBlock(mpb);

  }
}