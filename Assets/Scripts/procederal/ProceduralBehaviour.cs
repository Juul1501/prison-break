﻿using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class ProceduralBehaviour : MonoBehaviour
{
    public static ProceduralBehaviour instance;
    public float perlinSeedX;
    public float perlinSeedY;
    public int seed = 0;
    public GameObject module;
    public Terrain t;
    public int worldSize = 10;
    public float maxHeight = 600;

    private ProceduralTerrain pt;

    public List<HeightPass> passes;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            setSeed(seed);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        pt = new ProceduralTerrain(worldSize, worldSize, passes);
        //t.terrainData.size = new Vector3(worldSize * 2, maxHeight, worldSize * 2);
        //t.terrainData.heightmapResolution = worldSize;
        Generate();
    }

    void Update()
    {
        //if (Input.GetButtonDown("Fire1"))
        //{
        //    Generate();
        //}
    }

    public void Generate()
    {
        setSeed(seed);

        foreach (var obj in GameObject.FindGameObjectsWithTag("Procedural"))
        {
            Destroy(obj);
        }

        pt.Generate();

        float[,] norm = pt.GetHeightsNormalized();

        t.terrainData.SetHeights(0, 0, norm);
        Texture2D mask = new Texture2D(worldSize, worldSize);
        Color[] colors = new Color[worldSize * worldSize];

        for (int x = 0; x < worldSize; x++)
        {
            for (int z = 0; z < worldSize; z++)
            {
                colors[x + (z * worldSize)] = new Color(norm[x, z], norm[x, z], norm[x, z]);
            }
        }

        mask.SetPixels(colors);
        mask.Apply();

        //t.terrainData.terrainLayers[0].diffuseTexture = mask;
    }

    public void setSeed(int s)
    {
        Random.InitState(s);
        if (seed != s)
        {
            seed = s;
        }
        //perlinSeed = Random.Range(0.0f, 1000000f);
    }

}