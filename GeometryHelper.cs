using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace opentk_experiment
{
    public static class GeometryHelper
    {
        public class MeshData(float[] attributes, uint[] indices)
        {
            public float[] Attributes { get; } = attributes;
            public uint[] Indices { get; } = indices;
        }

        public static MeshData CreatePlane(float width, float height, int segments = 1)
        {
            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var texCoords = new List<Vector2>();
            var faces = new List<float>();
            var indices = new List<uint>();

            // Generate vertices, normals, and texture coordinates
            for (int y = 0; y <= segments; y++)
            {
                for (int x = 0; x <= segments; x++)
                {
                    float xPos = (x / (float)segments - 0.5f) * width;
                    float yPos = (y / (float)segments - 0.5f) * height;

                    vertices.Add(new Vector3(xPos, yPos, 0.0f));
                    normals.Add(new Vector3(0.0f, 0.0f, 1.0f)); // Normal points up
                    texCoords.Add(new Vector2(x / (float)segments, y / (float)segments));
                    faces.Add(0);
                }
            }

            // Generate indices for triangle strips
            for (int y = 0; y < segments; y++)
            {
                for (int x = 0; x < segments; x++)
                {
                    uint topLeft = (uint)(y * (segments + 1) + x);
                    uint topRight = topLeft + 1;
                    uint bottomLeft = (uint)((y + 1) * (segments + 1) + x);
                    uint bottomRight = bottomLeft + 1;

                    indices.Add(topLeft);
                    indices.Add(bottomLeft);
                    indices.Add(topRight);

                    indices.Add(topRight);
                    indices.Add(bottomLeft);
                    indices.Add(bottomRight);
                }
            }

            // Flatten data for OpenGL
            var flatAttributes = Flatten(vertices, normals, texCoords, faces);

            return new MeshData(flatAttributes, [.. indices]);
        }

        public static MeshData CreateSphere(float radius, int latitudeSegments, int longitudeSegments)
        {
            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var texCoords = new List<Vector2>();
            var faces = new List<float>();
            var indices = new List<uint>();

            // Generate vertices, normals, and texture coordinates
            for (int lat = 0; lat <= latitudeSegments; lat++)
            {
                float theta = MathF.PI * lat / latitudeSegments;
                float sinTheta = MathF.Sin(theta);
                float cosTheta = MathF.Cos(theta);

                for (int lon = 0; lon <= longitudeSegments; lon++)
                {
                    float phi = 2 * MathF.PI * lon / longitudeSegments;
                    float sinPhi = MathF.Sin(phi);
                    float cosPhi = MathF.Cos(phi);

                    float x = cosPhi * sinTheta;
                    float y = cosTheta;
                    float z = sinPhi * sinTheta;

                    Vector3 normal = new Vector3(x, y, z).Normalized();
                    Vector3 position = normal * radius;

                    vertices.Add(position);
                    normals.Add(normal);
                    texCoords.Add(new Vector2(lon / (float)longitudeSegments, lat / (float)latitudeSegments));
                    faces.Add(0);
                }
            }

            // Generate indices
            for (int lat = 0; lat < latitudeSegments; lat++)
            {
                for (int lon = 0; lon < longitudeSegments; lon++)
                {
                    uint first = (uint)(lat * (longitudeSegments + 1) + lon);
                    uint second = (uint)(first + longitudeSegments + 1);

                    indices.Add(first);
                    indices.Add(second);
                    indices.Add(first + 1);

                    indices.Add(second);
                    indices.Add(second + 1);
                    indices.Add(first + 1);
                }
            }

            // Flatten data for OpenGL
            var flatAttributes = Flatten(vertices, normals, texCoords, faces);

            return new MeshData(flatAttributes, [.. indices]);
        }

        public static MeshData CreateCube(float size)
        {
            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var texCoords = new List<Vector2>();
            var faces = new List<float>();
            var indices = new List<uint>();

            float half = size / 2.0f;

            // Cube vertices, normals, and texture coordinates
            Vector3[] positions =
            [
                new Vector3(-half, -half, -half), // 0
                new Vector3(half, -half, -half),  // 1
                new Vector3(half, half, -half),   // 2
                new Vector3(-half, half, -half),  // 3
                new Vector3(-half, -half, half),  // 4
                new Vector3(half, -half, half),   // 5
                new Vector3(half, half, half),    // 6
                new Vector3(-half, half, half),   // 7
            ];

            Vector3[] faceNormals =
            [
                new Vector3(0, 0, -1), // Back
                new Vector3(0, 0, 1),  // Front
                new Vector3(-1, 0, 0), // Left
                new Vector3(1, 0, 0),  // Right
                new Vector3(0, -1, 0), // Bottom
                new Vector3(0, 1, 0),  // Top
            ];

            int[][] faceIndices =
            [
                [0, 1, 2, 3], // Back
                [4, 5, 6, 7], // Front
                [0, 4, 7, 3], // Left
                [1, 5, 6, 2], // Right
                [0, 1, 5, 4], // Bottom
                [3, 2, 6, 7], // Top
            ];

            for (int i = 0; i < faceIndices.Length; i++)
            {
                var normal = faceNormals[i];

                for (int j = 0; j < 4; j++)
                {
                    var vertex = positions[faceIndices[i][j]];
                    vertices.Add(vertex);
                    normals.Add(normal);
                    faces.Add(i);
                    texCoords.Add(new Vector2(j == 1 || j == 2 ? 1 : 0, j >= 2 ? 1 : 0));
                }

                int baseIndex = i * 4;
                indices.Add((uint)baseIndex);
                indices.Add((uint)(baseIndex + 1));
                indices.Add((uint)(baseIndex + 2));
                indices.Add((uint)(baseIndex));
                indices.Add((uint)(baseIndex + 2));
                indices.Add((uint)(baseIndex + 3));
            }

            var flatAttributes = Flatten(vertices, normals, texCoords, faces);
            return new MeshData(flatAttributes, indices.ToArray());
        }

        public static MeshData CreateCylinder(float radius, float height, int segments)
        {
            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var texCoords = new List<Vector2>();
            var faces = new List<float>();
            var indices = new List<uint>();

            float halfHeight = height / 2.0f;

            // Generate vertices for the top and bottom circles
            for (int i = 0; i <= segments; i++)
            {
                float angle = 2 * MathF.PI * i / segments;
                float x = MathF.Cos(angle) * radius;
                float z = MathF.Sin(angle) * radius;

                vertices.Add(new Vector3(x, halfHeight, z)); // Top circle
                normals.Add(new Vector3(0, 1, 0));
                texCoords.Add(new Vector2(i / (float)segments, 0));
                faces.Add(0);

                vertices.Add(new Vector3(x, -halfHeight, z)); // Bottom circle
                normals.Add(new Vector3(0, -1, 0));
                texCoords.Add(new Vector2(i / (float)segments, 1));
                faces.Add(1);
            }

            // Generate side vertices
            for (int i = 0; i <= segments; i++)
            {
                float angle = 2 * MathF.PI * i / segments;
                float x = MathF.Cos(angle) * radius;
                float z = MathF.Sin(angle) * radius;

                vertices.Add(new Vector3(x, halfHeight, z));
                normals.Add(new Vector3(x, 0, z).Normalized());
                texCoords.Add(new Vector2(i / (float)segments, 0));
                faces.Add(2);

                vertices.Add(new Vector3(x, -halfHeight, z));
                normals.Add(new Vector3(x, 0, z).Normalized());
                texCoords.Add(new Vector2(i / (float)segments, 1));
                faces.Add(2);
            }

            // Generate indices for top and bottom
            for (int i = 0; i < segments; i++)
            {
                indices.Add((uint)(i * 2));
                indices.Add((uint)((i + 1) * 2));
                indices.Add((uint)((i + 1) * 2 + 1));

                indices.Add((uint)(i * 2));
                indices.Add((uint)((i + 1) * 2 + 1));
                indices.Add((uint)(i * 2 + 1));
            }

            // Generate indices for sides
            for (int i = 0; i < segments; i++)
            {
                uint topLeft = (uint)(segments * 2 + i * 2);
                uint bottomLeft = topLeft + 1;
                uint topRight = topLeft + 2;
                uint bottomRight = topRight + 1;

                indices.Add(topLeft);
                indices.Add(bottomLeft);
                indices.Add(topRight);

                indices.Add(topRight);
                indices.Add(bottomLeft);
                indices.Add(bottomRight);
            }

            var flatAttributes = Flatten(vertices, normals, texCoords, faces);
            return new MeshData(flatAttributes, indices.ToArray());
        }

        private static float[] Flatten(List<Vector3> vectors, List<Vector3> normals, List<Vector2> texCoords, List<float> faces)
        {
            var result = new float[vectors.Count * 9];
            for (int i = 0; i < vectors.Count; i++)
            {
                result[i * 9 + 0] = vectors[i].X;
                result[i * 9 + 1] = vectors[i].Y;
                result[i * 9 + 2] = vectors[i].Z;
                result[i * 9 + 3] = normals[i].X;
                result[i * 9 + 4] = normals[i].Y;
                result[i * 9 + 5] = normals[i].Z;
                result[i * 9 + 6] = texCoords[i].X;
                result[i * 9 + 7] = texCoords[i].Y;
                result[i * 9 + 8] = faces[i];
            }
            return result;
        }
    }
}
