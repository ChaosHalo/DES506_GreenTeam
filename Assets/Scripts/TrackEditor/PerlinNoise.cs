using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PerlinNoise : MonoBehaviour
{
    private int heightPower = 2;
    private int heightOctave = 2;
    private int waterPower = 2;
    private int waterOctave = 2;

    internal const int sizeX = 10;
    internal const int sizeY = 10;

    float[,] heightPerlinNoise = new float[sizeX, sizeY];
    float[,] waterPerlinNoise = new float[sizeX, sizeY];

    int[,] tileIDs = new int[sizeX, sizeY];

    public void BeginGenerate()
    {
        heightPerlinNoise = GeneratePerlinNoise(heightPower, heightOctave, GenerateBaseNoise());
        waterPerlinNoise = GeneratePerlinNoise(waterPower, waterOctave, GenerateBaseNoise());
        GenerateTileIDs();
    }

    private void GenerateTileIDs()
    {
        // normal terrain
        for(int x =0; x < sizeX; x++)
        {
            for(int y =0; y < sizeY; y++)
            {
                int ID = 3;
                // 3 = grass
                // 4 = desert
                // 5 = snow
                // 6 = water
                // 7 = mountain

                // height-based biomes
                if (heightPerlinNoise[x, y] < 0.16f)
                    ID = 4; // desert
                else if (heightPerlinNoise[x, y] < 0.3f)
                    ID = 3; // grass
                else if (heightPerlinNoise[x, y] < 0.4f)
                    ID = 5; // snow

                // water noise is separate
                if (waterPerlinNoise[x, y] < 0.1)
                    ID = 6;

                tileIDs[x, y] = ID;
            }
        }

        // obstacles (mountains)
        int difficulty = MyGameManager.instance.gameDifficulty;
        WorldgenData worldgenData = MyGameManager.instance.GetPlacementSystem().worldgenDatabase.worldgenData[difficulty];
        int mountainCount = Mathf.Clamp(worldgenData.mountainCount, 0, 12);
        for (int i = 0; i < mountainCount; i++)
            tileIDs[Random.Range(0, sizeX), Random.Range(0, sizeY)] = 7;
    }

    internal int GetTileID(int x, int y)
    {
        return tileIDs[x, y];
    }


    float[,] GenerateBaseNoise()
    {
        float[,] newArray = new float[sizeX, sizeY];

        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                float roll = (Random.Range(0, 100)) / 100.0f;
                newArray[x, y] = roll;
            }
        }
        return newArray;
    }

    float[,] GenerateSmoothNoise(int basePower, int octave, float[,] baseArray)
    {
        float[,] newArray = new float[sizeX, sizeY];
        int wavelength = (int)Mathf.Pow(basePower, octave);
        float frequency = 1.0f / wavelength;

        for (int x = 0; x < sizeX; x++)
        {
            int sample_x0 = (x / wavelength) * wavelength;
            int sample_x1 = (sample_x0 + wavelength) % sizeX;
            float horizontal_blend = (x - sample_x0) * frequency;

            for (int y = 0; y < sizeY; y++)
            {
                int sample_y0 = (y / wavelength) * wavelength;
                int sample_y1 = (sample_y0 + wavelength) % sizeY;
                float vertical_blend = (y - sample_y0) * frequency;

                float top = Mathf.Lerp(baseArray[sample_x0, sample_y0], baseArray[sample_x1, sample_y0], horizontal_blend);

                float bottom = Mathf.Lerp(baseArray[sample_x0, sample_y1], baseArray[sample_x1, sample_y1], horizontal_blend);

                newArray[x, y] = Mathf.Lerp(top, bottom, vertical_blend);
            }
        }

        return newArray;
    }
    float[,] GeneratePerlinNoise(int basePower, int octaveCount, float[,] baseArray)
    {
        float[,] newArray = new float[sizeX, sizeY];
        float[][,] smoothArrays = new float[octaveCount][,];

        float totalAmplitude = 0.0f;
        float persistance = 0.65f;
        float amplitude = 0.5f;

        for (int i = 0; i < octaveCount; i++)
        {
            smoothArrays[i] = GenerateSmoothNoise(basePower, i, baseArray);
        }

        // blend noise together
        for (int octave = octaveCount - 1; octave > 0; octave--)
        {
            amplitude *= persistance;
            totalAmplitude += persistance;

            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    newArray[x, y] += smoothArrays[octave][x, y] * amplitude;
                }
            }
        }

        // normalization
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                newArray[x, y] /= totalAmplitude;
            }
        }

        return newArray;
    }
}