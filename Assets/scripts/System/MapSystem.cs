using System;
using UnityEngine;

public class MapSystem : MonoBehaviour {
    public GameObject player;
    public GameObject wall;
    public GameObject unBreakWall;
    public GameObject pig;
    private int width = 100, height = 100;
    private int seed = 123123123;

    private int[,] map;
    private System.Random prng;

    void Init(float fillPercent) {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                map[j, i] = prng.NextDouble() < fillPercent ? 1 : 0;
            }
        }
    }

    float [,] NewNoise(int scale) {
        float [,] noise = new float[width, height];
        float offsetX = prng.Next(-100000, 100000);
        float offsetY = prng.Next(-100000, 100000);
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                float sampleX = (i + offsetX) / scale;
                float sampleY = (j + offsetY) / scale;

                noise[i, j] = Mathf.PerlinNoise(sampleX, sampleY);
            }
        }

        return noise;
    }

    void AddNoise() {
        float [,] noise = NewNoise(20);
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (noise[i, j] > 0.7) {
                    map[i, j] = 1;
                }
            }
        }
    }

    void Start() {
        prng = new(seed);
        map = new int[width, height];
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                map[i, j] = 0;
            }
        }

        Init(0.5f);
        for (int i = 0; i < 3; i++) {
            Smooth();
        }
        for (int i = 0; i < 2; i++) {
            AddNoise();
        }
        Outline();
        GenerateThings();
        SpawnMobs();
        SpawnPlayer();
    }

    void Smooth() {
        Vector3Int[] neighbor =
        {
            new(0, 0), new(1, 0), new(-1, 0),
            new(0, 1), new(0, -1), new(1, 1),
            new(1, -1), new(-1, 1), new(-1, -1),
        };

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                int count = 0;
                foreach (Vector3Int dir in neighbor) {
                    Vector3Int neighborPos = new Vector3Int(i, j) + dir;
                    if (0 <= neighborPos.x && neighborPos.x < width
                        && 0 <= neighborPos.y && neighborPos.y < height)
                            if (map[neighborPos.x, neighborPos.y] == 0)
                                count++;
                }

                map[i, j] = count >= 5 ? 0 : 1;
            }
        }
    }

    void Outline() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if ((j == 0 || j == height-1 || i == 0 || i == width-1)) {
                    map[j, i] = 2;
                }
            }
        }
    }

    private void GenerateThings()
    {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (map[j, i] == 1) {
                    ThingSystem.Instance.InstantiateThing(wall, new Vector3Int(i, 0, j));
                } else if (map[j, i] == 2) {
                    ThingSystem.Instance.InstantiateThing(unBreakWall, new Vector3Int(i, 0, j));
                }
            }
        }
    }

    private void SpawnMobs()
    {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (map[j, i] == 0) {
                    if (UnityEngine.Random.Range(0, 1f) < 0.02f) {
                        ThingSystem.Instance.InstantiateThing(pig, new Vector3Int(i, 0, j));

                        map[j, i] = 3;
                    }
                }
            }
        }
    }

    private void SpawnPlayer()
    {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (map[j, i] == 0) {
                    ThingSystem.Instance.InstantiateThing(player, new Vector3Int(i, 0, j));

                    return;
                }
            }
        }
    }
}