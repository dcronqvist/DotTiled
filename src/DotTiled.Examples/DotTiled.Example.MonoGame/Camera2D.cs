using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DotTiled.Example.MonoGame;

public class Camera2D
{
  public float Zoom { get; set; }
  public Vector2 Position { get; set; }
  public Rectangle Bounds { get; protected set; }
  public Rectangle VisibleArea { get; protected set; }
  public Matrix Transform { get; protected set; }

  public Camera2D(Viewport viewport)
  {
    Bounds = viewport.Bounds;
    Zoom = 1f;
    Position = Vector2.Zero;
  }

  private void UpdateVisibleArea()
  {
    var inverseViewMatrix = Matrix.Invert(Transform);

    var tl = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
    var tr = Vector2.Transform(new Vector2(Bounds.X, 0), inverseViewMatrix);
    var bl = Vector2.Transform(new Vector2(0, Bounds.Y), inverseViewMatrix);
    var br = Vector2.Transform(new Vector2(Bounds.Width, Bounds.Height), inverseViewMatrix);

    var min = new Vector2(
        MathHelper.Min(tl.X, MathHelper.Min(tr.X, MathHelper.Min(bl.X, br.X))),
        MathHelper.Min(tl.Y, MathHelper.Min(tr.Y, MathHelper.Min(bl.Y, br.Y))));
    var max = new Vector2(
        MathHelper.Max(tl.X, MathHelper.Max(tr.X, MathHelper.Max(bl.X, br.X))),
        MathHelper.Max(tl.Y, MathHelper.Max(tr.Y, MathHelper.Max(bl.Y, br.Y))));
    VisibleArea = new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
  }

  private void UpdateMatrix()
  {
    Transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
            Matrix.CreateScale(Zoom) *
            Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));
    UpdateVisibleArea();
  }

  public void UpdateCamera(Viewport bounds)
  {
    Bounds = bounds.Bounds;
    UpdateMatrix();
  }
}
