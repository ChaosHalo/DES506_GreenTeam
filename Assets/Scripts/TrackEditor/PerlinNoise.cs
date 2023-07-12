using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PerlinNoise : MonoBehaviour
{
    public enum Type { PRESET_1, PRESET_2, PRESET_3 };
    public Type generationType;

    private int heightPower = 2;
    private int heightOctave = 2;
    private int desertPower = 1;
    private int desertOctave = 2;
    private int waterPower = 2;
    private int waterOctave = 2;

    internal const int sizeX = 10;
    internal const int sizeY = 10;

    float[,] heightPerlinNoise = new float[sizeX, sizeY];
    float[,] desertPerlinNoise = new float[sizeX, sizeY];
    float[,] waterPerlinNoise = new float[sizeX, sizeY];

    public void BeginGenerate()
    {
        heightPerlinNoise = GeneratePerlinNoise(heightPower, heightOctave, GenerateBaseNoise());
        desertPerlinNoise = GeneratePerlinNoise(desertPower, desertOctave, GenerateBaseNoise());
        waterPerlinNoise = GeneratePerlinNoise(waterPower, waterOctave, GenerateBaseNoise());
    }

    internal int GetTileID(int x, int y)
    {
        int ID = 3;
        // 3 = grass
        // 4 = desert
        // 5 = snow
        // 6 = water
        // 7 = mountain

        // preset 1
        if (generationType == Type.PRESET_1)
        {
            if (desertPerlinNoise[x, y] < 0.16f)
                ID = 4;
            else if (heightPerlinNoise[x, y] > 0.1f)
                ID = 3;
            else if (heightPerlinNoise[x, y] > 0.075f)
                ID = 5;
            else
                ID = 7;
            if (waterPerlinNoise[x, y] < 0.1)
                ID = 6;
        }

        // preset 2
        if (generationType == Type.PRESET_2)
        {
            if (desertPerlinNoise[x, y] < 0.18f)
                ID = 4;
            else if (desertPerlinNoise[x, y] < 0.2f)
                ID = 6;
            else if (desertPerlinNoise[x, y] < 0.3f)
                ID = 3;
            else if (desertPerlinNoise[x, y] > 0.45f)
                ID = 5;
            else if (desertPerlinNoise[x, y] > 0.4f)
                ID = 7;
        }

        // preset 3
        if (generationType == Type.PRESET_3)
        {
            // bool mountainCondition1 = desertPerlinNoise[x, y] > 0.9975f || desertPerlinNoise[x, y] < 0.025f;
            //bool mountainCondition2 = desertPerlinNoise[x, y] > 0.365f || desertPerlinNoise[x, y] < 0.375f;
            if (heightPerlinNoise[x, y] < 0.16f)
                ID = 4;
            else if (heightPerlinNoise[x, y] < 0.3f)
                ID = 3;
            else if (heightPerlinNoise[x, y] < 0.4f)
                ID = 5;
            // else if (mountainCondition1||mountainCondition2)
            //  ID = 7;

            //else if (desertPerlinNoise[x, y] < 0.4f)
               // ID = 7;


            //if (desertPerlinNoise[x, y] < 0.1f)
            //   ID = 7;
            if (waterPerlinNoise[x, y] < 0.1)
                ID = 6;

            if (Random.Range(0, 100) < 5)
                ID = 7;
        }

        return ID;
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