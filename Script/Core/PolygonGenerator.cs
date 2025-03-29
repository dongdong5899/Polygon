using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public static class PolygonGenerator
{
    public static Sprite GeneratePolygonSprite(float sides, float radius, Color color)
    {
        if (sides < 3)
        {
            Debug.LogWarning("변의 개수가 넘 적으세요...");
            return null;
        }
        else if (sides > 25)
            sides = 25;

        // 1. 정점 생성 (정규화된 좌표로 설정)
        int vertexCount = Mathf.CeilToInt(sides);
        Vector2[] vertices2D = new Vector2[vertexCount + 1];
        vertices2D[0] = Vector2.one * 5; // 중심점을 Sprite Rect의 중앙으로 설정

        float angleStep = 360f / sides;
        for (int i = 0; i < vertexCount; i++)
        {
            float angle = Mathf.Deg2Rad * (i * angleStep + 90);
            vertices2D[i + 1] = new Vector2(
                5f + Mathf.Cos(angle) * radius,
                5f + Mathf.Sin(angle) * radius
            );
        }

        // 2. 삼각형 인덱스 설정
        ushort[] triangles = new ushort[vertexCount * 3];
        for (int i = 0; i < vertexCount; i++)
        {
            triangles[i * 3] = 0; // 중심점
            triangles[i * 3 + 1] = (ushort)(i + 1); // 현재 점
            triangles[i * 3 + 2] = (ushort)((i + 1) % sides + 1); // 다음 점
        }

        // 3. 텍스처 생성 (단색)
        Texture2D texture = new Texture2D(10, 10);
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                texture.SetPixel(i, j, color);
            }
        }
        texture.Apply();

        // 4. Sprite 생성
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), 1f);
        sprite.OverrideGeometry(vertices2D, triangles);


        return sprite;
    }
}
