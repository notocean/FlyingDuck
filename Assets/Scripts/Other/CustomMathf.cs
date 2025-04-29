using UnityEngine;

public static class CustomMathf 
{
    /// <summary>
    /// Kiểm tra 2 vector có đối nhau hay không
    /// </summary>
    public static bool IsOpposite(Vector2 a, Vector2 b, float threshold = 0.99f) {
        return Vector2.Dot(a.normalized, b.normalized) <= -threshold;
    }

    /// <summary>
    /// Kẹp giá trị value từ [a, b] thành [c, d]
    /// </summary>
    public static float MapValue(float value, float a, float b, float c, float d) {
        return c + (value - a) * (d - c) / (b - a);
    }

    /// <summary>
    /// Kiểm tra điểm point có thuộc tam giác abc hay không
    /// </summary>
    public static bool IsPointInTriangle(Vector2 point, Vector2 a, Vector2 b, Vector2 c) {
        float Sign(Vector2 p1, Vector2 p2, Vector2 p3) {
            return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
        }

        bool b1 = Sign(point, a, b) < 0.0f;
        bool b2 = Sign(point, b, c) < 0.0f;
        bool b3 = Sign(point, c, a) < 0.0f;

        return (b1 == b2) && (b2 == b3);
    }

    /// <summary>
    /// Kiểm tra điểm point có thuộc hình chữ nhật hay không
    /// </summary>
    public static bool IsPointInRectangle(Vector2 point, float top, float bottom, float right, float left) {
        return point.x >= left && point.x <= right && point.y >= bottom && point.y <= top;
    }
}
