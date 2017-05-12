﻿using System;
using UnityEngine;
using Mapzen;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainTile : MonoBehaviour
{
    public int TileZ = 14;

    public int Resolution = 32;

    public bool UseNormalMap = true;

    private int mResolution;

    private bool mUseNormalMap;

    public TileAddress Address;
    public Texture2D ElevationTexture;
    public Texture2D NormalTexture;
        
    public void Start()
    {
        this.Address = new TileAddress(0, 0, TileZ);
        this.data = new TerrainTileData(Address);
        this.data.ElevationTexture = this.ElevationTexture;
        this.data.NormalTexture = this.NormalTexture;

        ApplyElevation();
    }

    private void ApplyElevation()
    {
        var mesh = new Mesh();

        GetComponent<MeshFilter>().mesh = mesh;

        this.data.GenerateElevationGrid(mesh, Resolution, new Vector3(-0.5f, 0.0f, -0.5f));
        // this.data.GenerateElevationGrid(mesh, 32, Vector3.zero);

        this.data.ApplyElevation(mesh, 0.5f);

        var material = GetComponent<MeshRenderer>().material;

        if (UseNormalMap)
        {
            this.data.ApplyNormalTexture(material);
        }
        else
        {
            this.data.RemoveNormalTexture(material);
            mesh.RecalculateNormals();
        }

        mResolution = Resolution;
        mUseNormalMap = UseNormalMap;
    }

    public void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * 12.0f);

        if (mResolution != Resolution || mUseNormalMap != UseNormalMap)
        {
            ApplyElevation();
        }
    }

    private TerrainTileData data;
}

