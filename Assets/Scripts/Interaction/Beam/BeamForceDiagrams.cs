using System.Collections.Generic;
using UnityEngine;

public class ShearForceInfo
{
    public float startPointSegment;
    public float endPointSegment;
    public float shearForce;
}

public class BendingMomentInfo
{
    public float startPointSegment;
    public float endPointSegment;
    public float momentStartPosition;
    public float momentEndPosition;
}

public class BeamForceDiagrams : MonoBehaviour
{
    private BeamForceCalculation _beamForceCalculation;

    public float spacing;

    public MeshFilter sfdMeshFilter;
    public float sfdScale = 1;

    public MeshFilter bmdMeshFilter;
    public float bmdScale = 1;

    private MeshCreator _sfdMeshCreator;
    private MeshCreator _bmdMeshCreator;

    public float FindPeakPoint(List<float> list)
    {
        var peakPoint = 0f;

        for (int i = 0; i < list.Count; i++)
            peakPoint = Mathf.Max(peakPoint, Mathf.Abs(list[i]));

        return peakPoint;
    }

    private void Awake()
    {
        _beamForceCalculation = GetComponent<BeamForces>().beamForceCalculation;
    }

    public void UpdateBeamForceDiagrams()
    {
        CreateSFD();
        CreateBMD();
    }

    private void CreateSFD()
    {
        _sfdMeshCreator = new MeshCreator(MeshTopology.Quads);

        if (_beamForceCalculation.NumForces == 0)
        {
            sfdMeshFilter.sharedMesh = _sfdMeshCreator.CreateMesh();
            return;
        }

        var segments = _beamForceCalculation.NumForces + 1;

        var peakPoint = FindPeakPoint(_beamForceCalculation.shearForces);
        var scale = sfdScale / peakPoint;

        for (int i = 0; i < segments; i++)
        {
            var shearForceInfo = ShearForceInfo(i);

            var depth = Mathf.Min(0, shearForceInfo.shearForce * scale);
            var height = Mathf.Max(0, shearForceInfo.shearForce * scale);

            var cornerStart = new Vector3(shearForceInfo.startPointSegment + spacing, depth, 0);
            var cornerEnd = new Vector3(shearForceInfo.endPointSegment - spacing, height, 0);

            var face = Face.CreateSquareQuad(cornerStart, cornerEnd);

            _sfdMeshCreator.Faces.Add(face);
        }

        sfdMeshFilter.sharedMesh = _sfdMeshCreator.CreateMesh();
    }

    public ShearForceInfo ShearForceInfo(int i)
    {
        var info = new ShearForceInfo();

        info.startPointSegment = i == 0
            ? 0f
            : _beamForceCalculation.forcesAndDistancesToStart[i - 1].y;
        info.endPointSegment = i != _beamForceCalculation.NumForces
            ? _beamForceCalculation.forcesAndDistancesToStart[i].y
            : _beamForceCalculation.beamLength;
        info.shearForce = _beamForceCalculation.shearForces[i];

        return info;
    }

    private void CreateBMD()
    {
        _bmdMeshCreator = new(MeshTopology.Quads);

        if (_beamForceCalculation.NumForces == 0)
        {
            bmdMeshFilter.sharedMesh = _bmdMeshCreator.CreateMesh();
            return;
        }

        var segments = _beamForceCalculation.NumForces + 1;

        var peakPoint = FindPeakPoint(_beamForceCalculation.bendingMoments);
        var scale = bmdScale / peakPoint;

        for (int i = 0; i < segments; i++)
        {
            var bendingMomentInfo = BendingMomentInfo(i);

            var depthStart = Mathf.Min(0, bendingMomentInfo.momentStartPosition * scale);
            var heightStart = Mathf.Max(0, bendingMomentInfo.momentStartPosition * scale);
            var depthEnd = Mathf.Min(0, bendingMomentInfo.momentEndPosition * scale);
            var heightEnd = Mathf.Max(0, bendingMomentInfo.momentEndPosition * scale);

            var cornerStartDepth = new Vector3(bendingMomentInfo.startPointSegment + spacing, depthStart, 0);
            var cornerEndDepth = new Vector3(bendingMomentInfo.endPointSegment - spacing, depthEnd, 0);
            var cornerEndHeight = new Vector3(bendingMomentInfo.endPointSegment - spacing, heightEnd, 0);
            var cornerStartHeight = new Vector3(bendingMomentInfo.startPointSegment + spacing, heightStart, 0);

            var face = Face.CreateQuad(cornerStartDepth, cornerEndDepth, cornerEndHeight, cornerStartHeight);

            _bmdMeshCreator.Faces.Add(face);
        }

        bmdMeshFilter.sharedMesh = _bmdMeshCreator.CreateMesh();
    }

    public BendingMomentInfo BendingMomentInfo(int i)
    {
        var info = new BendingMomentInfo();

        info.startPointSegment = i == 0
            ? 0f
            : _beamForceCalculation.forcesAndDistancesToStart[i - 1].y;
        info.endPointSegment = i != _beamForceCalculation.NumForces
            ? _beamForceCalculation.forcesAndDistancesToStart[i].y
            : _beamForceCalculation.beamLength;

        info.momentStartPosition = _beamForceCalculation.bendingMoments[i];
        info.momentEndPosition = _beamForceCalculation.bendingMoments[i + 1];

        return info;
    }
}