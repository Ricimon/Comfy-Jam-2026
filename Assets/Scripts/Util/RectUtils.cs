using UnityEngine;

public static class RectUtils
{
    public static Rect CreateCenteredRect(Vector2 center, Vector2 size)
    {
        var extents = size / 2;
        return Rect.MinMaxRect(
            center.x - extents.x, center.y - extents.y,
            center.x + extents.x, center.y + extents.y);
    }
}