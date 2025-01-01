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
            var indices = new List<uint>();

            // Generate vertices and normals
            for (int y = 0; y <= segments; y++)
            {
                for (int x = 0; x <= segments; x++)
                {
                    float xPos = (x / (float)segments - 0.5f) * width;
                    float yPos = (y / (float)segments - 0.5f) * height;

                    vertices.Add(new Vector3(xPos, yPos, 0.0f));
                    normals.Add(new Vector3(0.0f, 0.0f, 1.0f)); // Normal points up
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
            var flatAttributes = Flatten(vertices, normals);

            return new MeshData(flatAttributes, indices.ToArray());
        }

        public static MeshData CreateSphere(float radius, int latitudeSegments, int longitudeSegments)
        {
            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var indices = new List<uint>();

            // Generate vertices and normals
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
            var flatAttributes = Flatten(vertices, normals);

            return new MeshData(flatAttributes, indices.ToArray());
        }

        private static float[] Flatten(List<Vector3> vectors, List<Vector3> normals)
        {
            var result = new float[vectors.Count * 6];
            for (int i = 0; i < vectors.Count; i++)
            {
                result[i * 6 + 0] = vectors[i].X;
                result[i * 6 + 1] = vectors[i].Y;
                result[i * 6 + 2] = vectors[i].Z;
            }
            for (int i = 0; i < normals.Count; i++)
            {
                result[i * 6 + 3] = normals[i].X;
                result[i * 6 + 4] = normals[i].Y;
                result[i * 6 + 5] = normals[i].Z;
            }
            return result;
        }
    }
}