using System;
using System.Collections.Generic;
using System.Linq;
using Level;
using Unity.Mathematics;
using UnityEngine;

namespace FX.ScreenEffect;

public class MirrorBreakEffect : MonoBehaviour
{
	[Serializable]
	public class Triangle
	{
		public int triangleIndex;

		public int3 vertices;

		public int3 edges;

		public int3 edgePairedTriangle;

		public Vector3 normal;

		public float crossProduct;

		public float randomProbabilities = 1f;

		public float decreaseAmount = 0.3f;

		public int digLevel = 1;

		public void MakeChunk(int chunkIndex, bool inside, float innerCircleIndex, float inAreaLevel, float outAreaLevel, List<Triangle> triangles, ref List<Triangle> tempTriangleList, ref List<int> triangleStoreList)
		{
			tempTriangleList.Remove(this);
			IncludeTriangleToChunk(chunkIndex, this, inside, edgePairedTriangle.x, innerCircleIndex, inAreaLevel, outAreaLevel, triangles, ref tempTriangleList, ref triangleStoreList);
			IncludeTriangleToChunk(chunkIndex, this, inside, edgePairedTriangle.y, innerCircleIndex, inAreaLevel, outAreaLevel, triangles, ref tempTriangleList, ref triangleStoreList);
			IncludeTriangleToChunk(chunkIndex, this, inside, edgePairedTriangle.z, innerCircleIndex, inAreaLevel, outAreaLevel, triangles, ref tempTriangleList, ref triangleStoreList);
		}

		public void IncludeTriangleToChunk(int chunkIndex, Triangle triangle, bool inside, int targetTriangle, float innerCircleIndex, float inAreaLevel, float outAreaLevel, List<Triangle> triangles, ref List<Triangle> tempTriangleList, ref List<int> triangleStoreList)
		{
			if (inside)
			{
				if (!(triangle.crossProduct < inAreaLevel) || !((float)targetTriangle <= innerCircleIndex) || !(Random.Range(0f, 1f) < triangle.randomProbabilities))
				{
					return;
				}
				for (int i = 0; i < tempTriangleList.Count; i++)
				{
					if (tempTriangleList[i].triangleIndex == targetTriangle)
					{
						Triangle triangle2 = tempTriangleList[i];
						triangleStoreList.Add(targetTriangle);
						triangle2.crossProduct += triangle.crossProduct;
						triangle2.randomProbabilities -= triangle.decreaseAmount;
						triangle2.digLevel++;
						triangle2.MakeChunk(chunkIndex, inside, innerCircleIndex, inAreaLevel, outAreaLevel, triangles, ref tempTriangleList, ref triangleStoreList);
						return;
					}
				}
				_ = -1;
				return;
			}
			if (chunkInsideIndex == -1)
			{
				chunkInsideIndex = chunkIndex;
			}
			if (!(crossProduct < outAreaLevel) || !((float)targetTriangle > innerCircleIndex) || !(Random.Range(0f, 1f) < triangle.randomProbabilities))
			{
				return;
			}
			for (int j = 0; j < tempTriangleList.Count; j++)
			{
				if (tempTriangleList[j].triangleIndex == targetTriangle)
				{
					Triangle triangle3 = tempTriangleList[j];
					triangleStoreList.Add(targetTriangle);
					triangle3.crossProduct += triangle.crossProduct;
					triangle3.randomProbabilities -= triangle.decreaseAmount;
					triangle3.digLevel++;
					triangle3.MakeChunk(chunkIndex, inside, innerCircleIndex, inAreaLevel, outAreaLevel, triangles, ref tempTriangleList, ref triangleStoreList);
					return;
				}
			}
			_ = -1;
		}
	}

	public class FinalMesh
	{
		public List<Triangle> meshTriangles;
	}

	[Serializable]
	public class ChunkData
	{
		public int baseTriangleIndex;

		public Vector3 chunkBasePosition;

		public Vector3 moveDirection;

		public float moveSpeed;

		public float InitialRotationDisplacement;

		public Vector3 rotationVector;

		public float rotationSpeed;

		public float scale;

		public List<int> chunkedTriangleIndexes;
	}

	public struct VerticesWithAngle
	{
		public int verticeIndex;

		public float verticeAngle;
	}

	public class ConvexHullLinkedVertices
	{
		public int convexHullverticeIndex;

		public List<VerticesWithAngle> linkedVerticesWithAngles;
	}

	public struct OutterVerticesCandidate
	{
		public int verticeIndex;

		public List<int> relateEdge;
	}

	public enum EdgeDirection
	{
		Up,
		Left,
		Down,
		Right,
		ShortTwo
	}

	private static int chunkInsideIndex = -1;

	[SerializeField]
	private Vector2 _screenSize;

	[SerializeField]
	private int verticesNum = 100;

	[Header("미러 애니메이션")]
	[SerializeField]
	private float _displacement_uv = 0.1f;

	[SerializeField]
	private float _displacement_rotation = 0.1f;

	[SerializeField]
	private bool _forceControlcamera;

	[SerializeField]
	private bool _addRootPosition;

	private float _cameraSize = 5.625f;

	[Range(-10f, 10f)]
	[SerializeField]
	private float _time;

	[Range(0f, 10f)]
	[SerializeField]
	private float _delay;

	private float _accelator;

	[SerializeField]
	private Vector3 _normal3;

	[SerializeField]
	[Header("메쉬 세팅")]
	private float _thickness = 0.2f;

	[SerializeField]
	private float _gap = 0.1f;

	[SerializeField]
	private float _scaleModifier = 1.04f;

	[SerializeField]
	private Material _material;

	[SerializeField]
	private Texture2D _endingSprite;

	[SerializeField]
	private Texture2D _resultSprite;

	[Header("정점 세팅")]
	[Space(10f)]
	[SerializeField]
	[Tooltip("내부,원,외부,최외곽")]
	private Vector4 verticesPositionRatio = new Vector4(40f, 20f, 20f, 20f);

	[SerializeField]
	private Vector2 _centerPoint;

	[Tooltip("도넛 내외부 반지름.")]
	[SerializeField]
	private Vector2 _innerCircleRadius;

	[SerializeField]
	[Tooltip("최외곽 두께.")]
	private float _edgeThickness = 0.5f;

	[Header("청크 메이킹 데이터")]
	[SerializeField]
	private float _innerCircleTriangleAreaLevel = 0.1f;

	[SerializeField]
	[Range(0f, 1f)]
	private float _InnerCircleTriangleRandomProbabilities = 1f;

	[SerializeField]
	[Range(0f, 1f)]
	private float _InnerCircleTriangleRPDecreaseAmount = 1f;

	[SerializeField]
	private float _outterCircleTriangleAreaLevel = 5f;

	[SerializeField]
	[Range(0f, 1f)]
	private float _outterCircleTriangleRandomProbabilities = 1f;

	[SerializeField]
	[Range(0f, 1f)]
	private float _outterCircleTriangleRPDecreaseAmount = 1f;

	[Header("기본 화면 세팅")]
	[SerializeField]
	private Camera _renderTextureCamera;

	[SerializeField]
	private Vector2[] vertices;

	[SerializeField]
	private float2[] verticesUV;

	[SerializeField]
	private List<int2> edges;

	[SerializeField]
	private List<Triangle> triangles;

	[SerializeField]
	private List<ChunkData> chunks;

	[SerializeField]
	private List<GameObject> chunkObject;

	private GameObject rootObject;

	private List<int> deadVertices;

	private List<int2> deadEdges;

	private List<int> convexHullPointsIndex;

	private int closestCenterPointIndex;

	private float halfScreenSizeX;

	private float halfScreenSizeY;

	private Vector2 topLeft;

	private Vector2 topRight;

	private Vector2 downLeft;

	private Vector2 downRight;

	private float topLeftDegree;

	private float topRightDegree;

	private float downLeftDegree;

	private float downRightDegree;

	private float innerVerticesRatio;

	private float circleVerticesRatio;

	private float outerVerticesRatio;

	private float edgeVerticesRatio;

	private bool IsInitialized;

	private bool IsVertexCreated;

	private bool IsEdgeCreated;

	private bool IsTriangleCreated;

	private bool IsPairedTriangleCreated;

	private bool IsChunkCreated;

	private bool IsMeshCreated;

	[Header("Debug")]
	[SerializeField]
	private bool IsMeshCreatedVertexCheck;

	[SerializeField]
	private bool IsMeshCreatedEdgeCheck;

	[SerializeField]
	private bool IsMeshCreatedTriangleCheck;

	[SerializeField]
	private bool IsMeshCreatedTrianglePairedCheck;

	[SerializeField]
	private bool IsMeshCreatedChunkCheck = true;

	private int firstTriangles;

	private int convexHullTriangles;

	private int convexHullToDeadVerticeTriangles;

	private int outterCircleLinkVerticeTriangles;

	private int outterCircleLeftTriangles;

	private int insideConvexHullEdgeNum = 100;

	private int convexHullEdgeToOutterCircleVertice = 1000;

	private int outterCircleBranchVerticeLink = 1000;

	private Camera camera;

	private List<int> deadEdgeOutterCircle = new List<int>();

	public void Initialize()
	{
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)rootObject != (Object)null)
		{
			Object.Destroy((Object)(object)rootObject);
		}
		IsInitialized = true;
		camera = Camera.main;
		_cameraSize = camera.orthographicSize;
		if (_forceControlcamera)
		{
			((Behaviour)((Component)camera).GetComponent<CameraController>()).enabled = false;
		}
		halfScreenSizeX = _screenSize.x * 0.5f;
		halfScreenSizeY = _screenSize.y * 0.5f;
		vertices = (Vector2[])(object)new Vector2[verticesNum + 4];
		if (verticesPositionRatio.x + verticesPositionRatio.y + verticesPositionRatio.z + verticesPositionRatio.w == 0f)
		{
			verticesPositionRatio.x = (verticesPositionRatio.y = (verticesPositionRatio.z = (verticesPositionRatio.w = 2.5f)));
		}
		if (verticesNum <= 0)
		{
			verticesNum = 1;
		}
		innerVerticesRatio = verticesPositionRatio.x;
		circleVerticesRatio = verticesPositionRatio.y;
		outerVerticesRatio = verticesPositionRatio.z;
		edgeVerticesRatio = verticesPositionRatio.w;
		float num = innerVerticesRatio + circleVerticesRatio + outerVerticesRatio + edgeVerticesRatio;
		innerVerticesRatio = innerVerticesRatio / num * (float)verticesNum;
		circleVerticesRatio = circleVerticesRatio / num * (float)verticesNum + innerVerticesRatio;
		outerVerticesRatio = outerVerticesRatio / num * (float)verticesNum + circleVerticesRatio;
		edgeVerticesRatio = edgeVerticesRatio / num * (float)verticesNum + outerVerticesRatio;
		topLeft = new Vector2(0f - halfScreenSizeX - _centerPoint.x, halfScreenSizeY - _centerPoint.y);
		topRight = new Vector2(halfScreenSizeX + _centerPoint.x, halfScreenSizeY - _centerPoint.y);
		downLeft = new Vector2(0f - halfScreenSizeX - _centerPoint.x, 0f - halfScreenSizeY + _centerPoint.y);
		downRight = new Vector2(halfScreenSizeX + _centerPoint.x, 0f - halfScreenSizeY + _centerPoint.y);
		topLeftDegree = Vector2.SignedAngle(new Vector2(1f, 0f), topLeft);
		topRightDegree = Vector2.SignedAngle(new Vector2(1f, 0f), topRight);
		downLeftDegree = Vector2.SignedAngle(new Vector2(1f, 0f), downLeft);
		downRightDegree = Vector2.SignedAngle(new Vector2(1f, 0f), downRight);
		deadVertices = new List<int>();
		edges = new List<int2>();
		triangles = new List<Triangle>();
		deadEdges = new List<int2>();
		convexHullPointsIndex = new List<int>();
		deadEdgeOutterCircle = new List<int>();
		IsVertexCreated = false;
		IsEdgeCreated = false;
		IsTriangleCreated = false;
		firstTriangles = 0;
		convexHullTriangles = 0;
		convexHullToDeadVerticeTriangles = 0;
		outterCircleLinkVerticeTriangles = 0;
		outterCircleLeftTriangles = 0;
		insideConvexHullEdgeNum = 100;
		convexHullEdgeToOutterCircleVertice = 1000;
		outterCircleBranchVerticeLink = 1000;
		_material.SetTexture("_MainTex", (Texture)(object)_endingSprite);
	}

	private void OnDestroy()
	{
		if ((Object)(object)camera != (Object)null)
		{
			((Behaviour)((Component)camera).GetComponent<CameraController>()).enabled = true;
		}
	}

	public void CreateVertex()
	{
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		Initialize();
		Vector2 val = default(Vector2);
		for (int i = 0; i < verticesNum; i++)
		{
			float num = Random.Range(0f, (float)Math.PI * 2f);
			if ((float)i < innerVerticesRatio)
			{
				float num2 = Random.Range(0f, _innerCircleRadius.x);
				vertices[i] = new Vector2(Mathf.Cos(num) + _centerPoint.x, Mathf.Sin(num) + _centerPoint.y) * num2;
			}
			else if ((float)i < circleVerticesRatio)
			{
				float num2 = Random.Range(_innerCircleRadius.x, _innerCircleRadius.y);
				vertices[i] = new Vector2(Mathf.Cos(num) + _centerPoint.x, Mathf.Sin(num) + _centerPoint.y) * num2;
			}
			else if ((float)i < outerVerticesRatio)
			{
				float num3 = _edgeThickness * 2f;
				((Vector2)(ref val))._002Ector(Random.Range(0f - halfScreenSizeX + num3, halfScreenSizeX - num3), Random.Range(0f - halfScreenSizeY + num3, halfScreenSizeY - num3));
				int num4 = 0;
				while (((Vector2)(ref val)).magnitude <= _innerCircleRadius.y + 1f)
				{
					((Vector2)(ref val))._002Ector(Random.Range(0f - halfScreenSizeX + num3, halfScreenSizeX - num3), Random.Range(0f - halfScreenSizeY + num3, halfScreenSizeY - num3));
					num4++;
					if (num4 > 100)
					{
						break;
					}
				}
				vertices[i] = val;
				CheckOutSidePoints(i);
			}
			else
			{
				float num2 = halfScreenSizeX * 3f;
				vertices[i] = new Vector2(Mathf.Cos(num), Mathf.Sin(num)) * num2;
				CheckOutSidePoints(i);
			}
		}
		vertices[verticesNum - 1] = new Vector2(Random.Range(_centerPoint.x - halfScreenSizeX * 0.5f, _centerPoint.x + _edgeThickness), halfScreenSizeY);
		vertices[verticesNum - 2] = new Vector2(Random.Range(_centerPoint.x + halfScreenSizeX * 0.5f, _centerPoint.x - _edgeThickness), halfScreenSizeY);
		vertices[verticesNum - 3] = new Vector2(Random.Range(_centerPoint.x - halfScreenSizeX * 0.5f, _centerPoint.x + _edgeThickness), 0f - halfScreenSizeY);
		vertices[verticesNum - 4] = new Vector2(Random.Range(_centerPoint.x + halfScreenSizeX * 0.5f, _centerPoint.x - _edgeThickness), 0f - halfScreenSizeY);
		vertices[verticesNum - 5] = new Vector2(halfScreenSizeX, Random.Range(_centerPoint.y - halfScreenSizeY * 0.5f, _centerPoint.y + halfScreenSizeY * 0.5f));
		vertices[verticesNum - 6] = new Vector2(0f - halfScreenSizeX, Random.Range(_centerPoint.y - halfScreenSizeY * 0.5f, _centerPoint.y + halfScreenSizeY * 0.5f));
		vertices[verticesNum] = new Vector2(topLeft.x, topLeft.y);
		vertices[verticesNum + 1] = new Vector2(topRight.x, topRight.y);
		vertices[verticesNum + 2] = new Vector2(downLeft.x, downLeft.y);
		vertices[verticesNum + 3] = new Vector2(downRight.x, downRight.y);
		deadVertices.Add(verticesNum + 1);
		deadVertices.Add(verticesNum + 2);
		deadVertices.Add(verticesNum + 3);
		IsVertexCreated = true;
	}

	public void CheckOutSidePoints(int _currentIndex)
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		if (!(topLeft.x < vertices[_currentIndex].x) || !(vertices[_currentIndex].x < topRight.x) || !(vertices[_currentIndex].y > downRight.y) || !(vertices[_currentIndex].y < topRight.y))
		{
			float num = Vector2.SignedAngle(new Vector2(1f, 0f), vertices[_currentIndex]);
			if (topRightDegree <= num && num <= topLeftDegree)
			{
				GetTwoLineIntersectionPoint(_centerPoint, vertices[_currentIndex], new Vector2(0f - halfScreenSizeX, halfScreenSizeY), new Vector2(halfScreenSizeX, halfScreenSizeY), EdgeDirection.Up, _currentIndex, ref vertices[_currentIndex]);
			}
			else if (downLeftDegree <= num && num <= downRightDegree)
			{
				GetTwoLineIntersectionPoint(_centerPoint, vertices[_currentIndex], new Vector2(0f - halfScreenSizeX, 0f - halfScreenSizeY), new Vector2(halfScreenSizeX, 0f - halfScreenSizeY), EdgeDirection.Down, _currentIndex, ref vertices[_currentIndex]);
			}
			else if (downRightDegree <= num && num <= topRightDegree)
			{
				GetTwoLineIntersectionPoint(_centerPoint, vertices[_currentIndex], new Vector2(halfScreenSizeX, 0f - halfScreenSizeY), new Vector2(halfScreenSizeX, halfScreenSizeY), EdgeDirection.Right, _currentIndex, ref vertices[_currentIndex]);
			}
			else
			{
				GetTwoLineIntersectionPoint(_centerPoint, vertices[_currentIndex], new Vector2(0f - halfScreenSizeX, 0f - halfScreenSizeY), new Vector2(0f - halfScreenSizeX, halfScreenSizeY), EdgeDirection.Left, _currentIndex, ref vertices[_currentIndex]);
			}
		}
	}

	public bool GetTwoLineIntersectionPoint(Vector2 A1, Vector2 A2, Vector2 B1, Vector2 B2, EdgeDirection edgeNum, int currentIndex, ref Vector2 originalVector)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		float num = (B2.x - B1.x) * (A2.y - A1.y) - (B2.y - B1.y) * (A2.x - A1.x);
		if (num == 0f)
		{
			return false;
		}
		float num2 = ((A1.x - B1.x) * (A2.y - A1.y) - (A1.y - B1.y) * (A2.x - A1.x)) / num;
		float num3 = B1.x + (B2.x - B1.x) * num2;
		float num4 = B1.y + (B2.y - B1.y) * num2;
		switch (edgeNum)
		{
		case EdgeDirection.Up:
			if (num3 < B1.x || num3 > B2.x)
			{
				return false;
			}
			break;
		case EdgeDirection.Down:
			if (num3 < B1.x || num3 > B2.x)
			{
				return false;
			}
			break;
		case EdgeDirection.Right:
			if (num4 < B1.y || num4 > B2.y)
			{
				return false;
			}
			break;
		case EdgeDirection.Left:
			if (num4 < B1.y || num4 > B2.y)
			{
				return false;
			}
			break;
		case EdgeDirection.ShortTwo:
			if (math.min(math.min(A1.x, A2.x), math.min(B1.x, B2.x)) < num3 && num3 < math.max(math.max(A1.x, A2.x), math.max(B1.x, B2.x)) && math.min(math.min(A1.y, A2.y), math.min(B1.y, B2.y)) < num4 && num4 < math.max(math.max(A1.y, A2.y), math.max(B1.y, B2.y)))
			{
				return true;
			}
			break;
		}
		originalVector = new Vector2(num3, num4);
		deadVertices.Add(currentIndex);
		return true;
	}

	public bool CheckTwoLineIntersect(Vector2 A1, Vector2 A2, Vector2 B1, Vector2 B2)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		float num = (B2.x - B1.x) * (A2.y - A1.y) - (B2.y - B1.y) * (A2.x - A1.x);
		if (num == 0f)
		{
			return false;
		}
		float num2 = ((A1.x - B1.x) * (A2.y - A1.y) - (A1.y - B1.y) * (A2.x - A1.x)) / num;
		float num3 = B1.x + (B2.x - B1.x) * num2;
		float num4 = B1.y + (B2.y - B1.y) * num2;
		if (math.min(math.min(A1.x, A2.x), math.min(B1.x, B2.x)) < num3 && num3 < math.max(math.max(A1.x, A2.x), math.max(B1.x, B2.x)) && math.min(math.min(A1.y, A2.y), math.min(B1.y, B2.y)) < num4 && num4 < math.max(math.max(A1.y, A2.y), math.max(B1.y, B2.y)))
		{
			return true;
		}
		return false;
	}

	public void SetEdges()
	{
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0acd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0add: Unknown result type (might be due to invalid IL or missing references)
		//IL_0732: Unknown result type (might be due to invalid IL or missing references)
		//IL_073c: Unknown result type (might be due to invalid IL or missing references)
		//IL_074f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0759: Unknown result type (might be due to invalid IL or missing references)
		//IL_075e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0763: Unknown result type (might be due to invalid IL or missing references)
		//IL_076d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06db: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0702: Unknown result type (might be due to invalid IL or missing references)
		//IL_0707: Unknown result type (might be due to invalid IL or missing references)
		//IL_070c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0716: Unknown result type (might be due to invalid IL or missing references)
		//IL_078e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b59: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0809: Unknown result type (might be due to invalid IL or missing references)
		//IL_081c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0826: Unknown result type (might be due to invalid IL or missing references)
		//IL_082b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0830: Unknown result type (might be due to invalid IL or missing references)
		//IL_083a: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0859: Unknown result type (might be due to invalid IL or missing references)
		//IL_086a: Unknown result type (might be due to invalid IL or missing references)
		//IL_086f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0874: Unknown result type (might be due to invalid IL or missing references)
		//IL_0878: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Unknown result type (might be due to invalid IL or missing references)
		//IL_088b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0892: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0904: Unknown result type (might be due to invalid IL or missing references)
		//IL_091c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0921: Unknown result type (might be due to invalid IL or missing references)
		//IL_0926: Unknown result type (might be due to invalid IL or missing references)
		//IL_0930: Unknown result type (might be due to invalid IL or missing references)
		//IL_0948: Unknown result type (might be due to invalid IL or missing references)
		//IL_094d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0952: Unknown result type (might be due to invalid IL or missing references)
		//IL_0955: Unknown result type (might be due to invalid IL or missing references)
		//IL_0957: Unknown result type (might be due to invalid IL or missing references)
		//IL_1486: Unknown result type (might be due to invalid IL or missing references)
		//IL_1497: Unknown result type (might be due to invalid IL or missing references)
		//IL_149c: Unknown result type (might be due to invalid IL or missing references)
		//IL_14a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_14a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_14aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_16db: Unknown result type (might be due to invalid IL or missing references)
		//IL_16e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_16e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_16e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_1390: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_13dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_13fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1400: Unknown result type (might be due to invalid IL or missing references)
		//IL_1407: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dfe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e75: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1210: Unknown result type (might be due to invalid IL or missing references)
		//IL_1215: Unknown result type (might be due to invalid IL or missing references)
		//IL_1226: Unknown result type (might be due to invalid IL or missing references)
		//IL_122b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1230: Unknown result type (might be due to invalid IL or missing references)
		//IL_1234: Unknown result type (might be due to invalid IL or missing references)
		//IL_123b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe9: Unknown result type (might be due to invalid IL or missing references)
		//IL_101c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1035: Unknown result type (might be due to invalid IL or missing references)
		//IL_103a: Unknown result type (might be due to invalid IL or missing references)
		//IL_104b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1050: Unknown result type (might be due to invalid IL or missing references)
		//IL_1055: Unknown result type (might be due to invalid IL or missing references)
		//IL_1059: Unknown result type (might be due to invalid IL or missing references)
		//IL_1060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_18fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1914: Unknown result type (might be due to invalid IL or missing references)
		//IL_1919: Unknown result type (might be due to invalid IL or missing references)
		//IL_192a: Unknown result type (might be due to invalid IL or missing references)
		//IL_192f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1934: Unknown result type (might be due to invalid IL or missing references)
		//IL_1936: Unknown result type (might be due to invalid IL or missing references)
		//IL_193d: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_12dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_12fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_130b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1310: Unknown result type (might be due to invalid IL or missing references)
		//IL_1315: Unknown result type (might be due to invalid IL or missing references)
		//IL_1319: Unknown result type (might be due to invalid IL or missing references)
		//IL_1320: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_1101: Unknown result type (might be due to invalid IL or missing references)
		//IL_111a: Unknown result type (might be due to invalid IL or missing references)
		//IL_111f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1130: Unknown result type (might be due to invalid IL or missing references)
		//IL_1135: Unknown result type (might be due to invalid IL or missing references)
		//IL_113a: Unknown result type (might be due to invalid IL or missing references)
		//IL_113e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1145: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e52: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ae5: Unknown result type (might be due to invalid IL or missing references)
		//IL_19f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1969: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eef: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d48: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c43: Unknown result type (might be due to invalid IL or missing references)
		List<int> list = new List<int>(deadVertices);
		deadEdges = new List<int2>();
		edges = new List<int2>();
		triangles = new List<Triangle>();
		deadVertices.Add(verticesNum);
		int num = verticesNum;
		EdgeDirection edgeDirection = EdgeDirection.Up;
		for (int i = 0; i < deadVertices.Count; i++)
		{
			float num2 = halfScreenSizeX * 2.5f;
			float num3 = num2;
			int index = 0;
			switch (edgeDirection)
			{
			case EdgeDirection.Up:
			{
				for (int l = 0; l < list.Count; l++)
				{
					if (vertices[list[l]].y == topLeft.y)
					{
						num3 = Mathf.Abs(vertices[num].x - vertices[list[l]].x);
						if (num3 < num2)
						{
							num2 = num3;
							index = l;
						}
					}
				}
				int num4 = list[index];
				edges.Add(new int2(num, num4));
				deadEdges.Add(new int2(num, num4));
				list.RemoveAt(index);
				num = num4;
				if (vertices[num].x == topRight.x)
				{
					edgeDirection = EdgeDirection.Right;
					list.Add(verticesNum);
				}
				break;
			}
			case EdgeDirection.Left:
			{
				for (int m = 0; m < list.Count; m++)
				{
					if (vertices[list[m]].x == topLeft.x)
					{
						num3 = Mathf.Abs(vertices[num].y - vertices[list[m]].y);
						if (num3 < num2)
						{
							num2 = num3;
							index = m;
						}
					}
				}
				int num4 = list[index];
				edges.Add(new int2(num, num4));
				deadEdges.Add(new int2(num, num4));
				list.RemoveAt(index);
				num = num4;
				break;
			}
			case EdgeDirection.Down:
			{
				for (int k = 0; k < list.Count; k++)
				{
					if (vertices[list[k]].y == downLeft.y)
					{
						num3 = Mathf.Abs(vertices[num].x - vertices[list[k]].x);
						if (num3 < num2)
						{
							num2 = num3;
							index = k;
						}
					}
				}
				int num4 = list[index];
				edges.Add(new int2(num, num4));
				deadEdges.Add(new int2(num, num4));
				list.RemoveAt(index);
				num = num4;
				if (vertices[num].x == downLeft.x)
				{
					edgeDirection = EdgeDirection.Left;
				}
				break;
			}
			case EdgeDirection.Right:
			{
				for (int j = 0; j < list.Count; j++)
				{
					if (vertices[list[j]].x == topRight.x)
					{
						num3 = Mathf.Abs(vertices[num].y - vertices[list[j]].y);
						if (num3 < num2)
						{
							num2 = num3;
							index = j;
						}
					}
				}
				int num4 = list[index];
				edges.Add(new int2(num, num4));
				deadEdges.Add(new int2(num, num4));
				list.RemoveAt(index);
				num = num4;
				if (vertices[num].y == downLeft.y)
				{
					edgeDirection = EdgeDirection.Down;
				}
				break;
			}
			}
		}
		float num5 = _innerCircleRadius.y;
		for (int n = 0; (float)n < circleVerticesRatio; n++)
		{
			float num6 = Vector2.Distance(Vector2.zero, vertices[n]);
			if (num6 < num5)
			{
				num5 = num6;
				closestCenterPointIndex = n;
			}
		}
		List<VerticesWithAngle> list2 = new List<VerticesWithAngle>();
		for (int num7 = 0; (float)num7 < circleVerticesRatio; num7++)
		{
			if (num7 != closestCenterPointIndex)
			{
				VerticesWithAngle item = default(VerticesWithAngle);
				item.verticeIndex = num7;
				Vector2 val = vertices[num7] - vertices[closestCenterPointIndex];
				item.verticeAngle = Mathf.Atan2(val.y, val.x);
				list2.Add(item);
			}
		}
		List<VerticesWithAngle> list3 = list2.OrderByDescending((VerticesWithAngle x) => x.verticeAngle).ToList();
		for (int num8 = 0; num8 < list3.Count; num8++)
		{
		}
		_ = deadEdges.Count;
		int2 edge = default(int2);
		for (int num9 = 0; num9 < list3.Count; num9++)
		{
			Triangle triangle = new Triangle();
			int index2 = (num9 + 1) % list3.Count;
			((int2)(ref edge))._002Ector(closestCenterPointIndex, list3[num9].verticeIndex);
			EdgeContainsCheck(0, ref edge, ref triangle);
			triangle.vertices.x = closestCenterPointIndex;
			triangle.vertices.y = list3[num9].verticeIndex;
			((int2)(ref edge))._002Ector(list3[num9].verticeIndex, list3[index2].verticeIndex);
			EdgeContainsCheck(1, ref edge, ref triangle);
			triangle.vertices.z = list3[index2].verticeIndex;
			((int2)(ref edge))._002Ector(closestCenterPointIndex, list3[index2].verticeIndex);
			EdgeContainsCheck(2, ref edge, ref triangle);
			triangles.Add(triangle);
			firstTriangles = triangles.Count - 1;
		}
		List<VerticesWithAngle> list4 = new List<VerticesWithAngle>(list3);
		List<int> list5 = new List<int>();
		convexHullPointsIndex = new List<int>();
		for (int num10 = 0; num10 < list3.Count; num10++)
		{
			convexHullPointsIndex.Add(list3[num10].verticeIndex);
		}
		int num11 = 0;
		VerticesWithAngle item3 = default(VerticesWithAngle);
		while (list4.Count > 0)
		{
			int firstEdgesFromTheLast = GetFirstEdgesFromTheLast(list4[0].verticeIndex, closestCenterPointIndex, list5);
			int secondEdgesFromTheLast = GetSecondEdgesFromTheLast(list4[0].verticeIndex, closestCenterPointIndex, list5);
			if (firstEdgesFromTheLast == -1 || secondEdgesFromTheLast == -1)
			{
				list4.RemoveAt(0);
				continue;
			}
			int num12 = 0;
			Vector2 val2;
			int num13;
			if (list4[0].verticeIndex == edges[firstEdgesFromTheLast].x)
			{
				val2 = vertices[edges[firstEdgesFromTheLast].y] - vertices[edges[firstEdgesFromTheLast].x];
				num13 = edges[firstEdgesFromTheLast].y;
			}
			else
			{
				val2 = vertices[edges[firstEdgesFromTheLast].x] - vertices[edges[firstEdgesFromTheLast].y];
				num13 = edges[firstEdgesFromTheLast].x;
			}
			Vector2 val3;
			if (list4[0].verticeIndex == edges[secondEdgesFromTheLast].x)
			{
				val3 = vertices[edges[secondEdgesFromTheLast].y] - vertices[edges[secondEdgesFromTheLast].x];
				num12 = edges[secondEdgesFromTheLast].y;
			}
			else
			{
				val3 = vertices[edges[secondEdgesFromTheLast].x] - vertices[edges[secondEdgesFromTheLast].y];
				num12 = edges[secondEdgesFromTheLast].x;
			}
			Vector2 val4 = vertices[list4[0].verticeIndex] - vertices[closestCenterPointIndex];
			float num14 = Vector2.Dot(((Vector2)(ref val4)).normalized, ((Vector2)(ref val2)).normalized);
			float num15 = Vector2.Dot(((Vector2)(ref val4)).normalized, ((Vector2)(ref val3)).normalized);
			if (num14 + num15 <= 0f)
			{
				list4.RemoveAt(0);
				continue;
			}
			edges.Add(new int2(num13, num12));
			Triangle triangle2 = new Triangle();
			SetEdge(0, firstEdgesFromTheLast, ref triangle2);
			SetEdge(1, edges.Count - 1, ref triangle2);
			SetEdge(2, secondEdgesFromTheLast, ref triangle2);
			Vector2 first = vertices[num13] - vertices[list4[0].verticeIndex];
			Vector2 second = vertices[num12] - vertices[list4[0].verticeIndex];
			if (CrossProduct(first, second) < 0f)
			{
				triangle2.vertices.x = list4[0].verticeIndex;
				triangle2.vertices.y = num13;
				triangle2.vertices.z = num12;
			}
			else
			{
				triangle2.vertices.x = list4[0].verticeIndex;
				triangle2.vertices.y = num12;
				triangle2.vertices.z = num13;
			}
			triangles.Add(triangle2);
			convexHullTriangles = triangles.Count - 1;
			VerticesWithAngle item2 = default(VerticesWithAngle);
			item3.verticeIndex = num13;
			item2.verticeIndex = num12;
			item3.verticeAngle = (item2.verticeAngle = 0f);
			list4.Add(item3);
			list4.Add(item2);
			list5.Add(firstEdgesFromTheLast);
			list5.Add(secondEdgesFromTheLast);
			convexHullPointsIndex.Remove(list4[0].verticeIndex);
			list4.RemoveAt(0);
		}
		List<VerticesWithAngle> list6 = new List<VerticesWithAngle>();
		List<VerticesWithAngle> list7 = new List<VerticesWithAngle>();
		for (int num16 = 0; num16 < convexHullPointsIndex.Count; num16++)
		{
			VerticesWithAngle item4 = default(VerticesWithAngle);
			item4.verticeIndex = convexHullPointsIndex[num16];
			Vector2 val5 = vertices[convexHullPointsIndex[num16]] - vertices[closestCenterPointIndex];
			item4.verticeAngle = Mathf.Atan2(val5.y, val5.x);
			list6.Add(item4);
		}
		for (int num17 = (int)circleVerticesRatio; num17 < vertices.Length; num17++)
		{
			VerticesWithAngle item5 = default(VerticesWithAngle);
			item5.verticeIndex = num17;
			Vector2 val6 = vertices[num17] - vertices[closestCenterPointIndex];
			item5.verticeAngle = Mathf.Atan2(val6.y, val6.x);
			list7.Add(item5);
		}
		List<VerticesWithAngle> list8 = list6.OrderByDescending((VerticesWithAngle x) => x.verticeAngle).ToList();
		List<VerticesWithAngle> list9 = list7.OrderByDescending((VerticesWithAngle x) => x.verticeAngle).ToList();
		insideConvexHullEdgeNum = edges.Count - 1;
		int count = triangles.Count;
		List<ConvexHullLinkedVertices> list10 = new List<ConvexHullLinkedVertices>();
		for (int num18 = 0; num18 < list8.Count; num18++)
		{
			float num20;
			if (num18 == 0)
			{
				float num19 = ((float)Math.PI + list8[list8.Count - 1].verticeAngle) * 0.5f + ((float)Math.PI - list8[num18].verticeAngle) * 0.5f;
				num20 = list8[list8.Count - 1].verticeAngle - num19;
				if (num20 <= -(float)Math.PI)
				{
					num20 = list8[num18].verticeAngle + num19;
				}
			}
			else
			{
				num20 = list8[num18 - 1].verticeAngle * 0.5f + list8[num18].verticeAngle * 0.5f;
			}
			float num22;
			if (num18 == list8.Count - 1)
			{
				float num21 = ((float)Math.PI + list8[num18].verticeAngle) * 0.5f + ((float)Math.PI - list8[0].verticeAngle) * 0.5f;
				num22 = list8[0].verticeAngle + num21;
				if (num22 >= (float)Math.PI)
				{
					num22 = list8[num18].verticeAngle - num21;
				}
			}
			else
			{
				num22 = list8[num18].verticeAngle * 0.5f + list8[num18 + 1].verticeAngle * 0.5f;
			}
			ConvexHullLinkedVertices convexHullLinkedVertices = new ConvexHullLinkedVertices();
			convexHullLinkedVertices.convexHullverticeIndex = list8[num18].verticeIndex;
			convexHullLinkedVertices.linkedVerticesWithAngles = new List<VerticesWithAngle>();
			VerticesWithAngle item6 = default(VerticesWithAngle);
			for (int num23 = 0; num23 < list9.Count; num23++)
			{
				if (num20 * num22 < 0f)
				{
					if (num18 == 0 && num20 < -1f)
					{
						if (num20 >= list9[num23].verticeAngle && list9[num23].verticeAngle > -(float)Math.PI)
						{
							edges.Add(new int2(list8[num18].verticeIndex, list9[num23].verticeIndex));
							item6.verticeIndex = list9[num23].verticeIndex;
							Vector2 val7 = vertices[list9[num23].verticeIndex] - vertices[list8[num18].verticeIndex] - vertices[closestCenterPointIndex];
							item6.verticeAngle = Mathf.Atan2(val7.y, val7.x);
							convexHullLinkedVertices.linkedVerticesWithAngles.Add(item6);
						}
						if ((float)Math.PI >= list9[num23].verticeAngle && list9[num23].verticeAngle > num22)
						{
							edges.Add(new int2(list8[num18].verticeIndex, list9[num23].verticeIndex));
							item6.verticeIndex = list9[num23].verticeIndex;
							Vector2 val8 = vertices[list9[num23].verticeIndex] - vertices[list8[num18].verticeIndex] - vertices[closestCenterPointIndex];
							item6.verticeAngle = Mathf.Atan2(val8.y, val8.x);
							convexHullLinkedVertices.linkedVerticesWithAngles.Add(item6);
						}
					}
					else if (num18 == list8.Count - 1 && num22 > 0f)
					{
						if ((float)Math.PI >= list9[num23].verticeAngle && list9[num23].verticeAngle > num22)
						{
							edges.Add(new int2(list8[num18].verticeIndex, list9[num23].verticeIndex));
							item6.verticeIndex = list9[num23].verticeIndex;
							Vector2 val9 = vertices[list9[num23].verticeIndex] - vertices[list8[num18].verticeIndex] - vertices[closestCenterPointIndex];
							item6.verticeAngle = Mathf.Atan2(val9.y, val9.x);
							convexHullLinkedVertices.linkedVerticesWithAngles.Add(item6);
						}
						if (num20 >= list9[num23].verticeAngle && list9[num23].verticeAngle > -(float)Math.PI)
						{
							edges.Add(new int2(list8[num18].verticeIndex, list9[num23].verticeIndex));
							item6.verticeIndex = list9[num23].verticeIndex;
							Vector2 val10 = vertices[list9[num23].verticeIndex] - vertices[list8[num18].verticeIndex] - vertices[closestCenterPointIndex];
							item6.verticeAngle = Mathf.Atan2(val10.y, val10.x);
							convexHullLinkedVertices.linkedVerticesWithAngles.Add(item6);
						}
					}
					else if (num22 < 0f)
					{
						if (num20 >= list9[num23].verticeAngle && list9[num23].verticeAngle > 0f)
						{
							edges.Add(new int2(list8[num18].verticeIndex, list9[num23].verticeIndex));
							item6.verticeIndex = list9[num23].verticeIndex;
							Vector2 val11 = vertices[list9[num23].verticeIndex] - vertices[list8[num18].verticeIndex] - vertices[closestCenterPointIndex];
							item6.verticeAngle = Mathf.Atan2(val11.y, val11.x);
							convexHullLinkedVertices.linkedVerticesWithAngles.Add(item6);
						}
						if (0f >= list9[num23].verticeAngle && list9[num23].verticeAngle > num22)
						{
							edges.Add(new int2(list8[num18].verticeIndex, list9[num23].verticeIndex));
							item6.verticeIndex = list9[num23].verticeIndex;
							Vector2 val12 = vertices[list9[num23].verticeIndex] - vertices[list8[num18].verticeIndex] - vertices[closestCenterPointIndex];
							item6.verticeAngle = Mathf.Atan2(val12.y, val12.x);
							convexHullLinkedVertices.linkedVerticesWithAngles.Add(item6);
						}
					}
				}
				else if (num20 >= list9[num23].verticeAngle && list9[num23].verticeAngle > num22)
				{
					edges.Add(new int2(list8[num18].verticeIndex, list9[num23].verticeIndex));
					item6.verticeIndex = list9[num23].verticeIndex;
					Vector2 val13 = vertices[list9[num23].verticeIndex] - vertices[list8[num18].verticeIndex] - vertices[closestCenterPointIndex];
					item6.verticeAngle = Mathf.Atan2(val13.y, val13.x);
					convexHullLinkedVertices.linkedVerticesWithAngles.Add(item6);
				}
			}
			list10.Add(convexHullLinkedVertices);
		}
		convexHullEdgeToOutterCircleVertice = edges.Count - 1;
		VerticesWithAngle item7 = default(VerticesWithAngle);
		for (int num24 = 0; num24 < list10.Count; num24++)
		{
			Vector2 val14 = vertices[list8[num24].verticeIndex] - vertices[closestCenterPointIndex];
			float num25 = Mathf.Atan2(val14.y, val14.x);
			_ = "convexHullverticeIndex : [" + list10[num24].convexHullverticeIndex + "    angle : " + num25 + "]";
			List<VerticesWithAngle> list11 = list10[num24].linkedVerticesWithAngles.OrderByDescending((VerticesWithAngle x) => x.verticeAngle).ToList();
			for (int num26 = 0; num26 < list11.Count; num26++)
			{
				if (!(Mathf.Abs(list11[num26].verticeAngle - num25) > 1.5f))
				{
					continue;
				}
				item7.verticeIndex = list11[num26].verticeIndex;
				item7.verticeAngle = list11[num26].verticeAngle;
				if (list11[num26].verticeAngle < 0f)
				{
					item7.verticeAngle = (float)Math.PI + list11[num26].verticeAngle + (float)Math.PI;
					list11.RemoveAt(num26);
					list11.Insert(0, item7);
					list11 = list11.OrderByDescending((VerticesWithAngle x) => x.verticeAngle).ToList();
				}
				else
				{
					item7.verticeAngle = 0f - ((float)Math.PI - list11[num26].verticeAngle) - (float)Math.PI;
					list11.RemoveAt(num26);
					list11.Add(item7);
					list11 = list11.OrderByDescending((VerticesWithAngle x) => x.verticeAngle).ToList();
				}
				num26--;
			}
		}
		int num27 = -1;
		int num28 = -1;
		int num29 = -1;
		int num30 = -1;
		int yIndex = -1;
		bool flag = false;
		List<int> list12 = new List<int>();
		List<int> list13 = new List<int>();
		VerticesWithAngle item8 = default(VerticesWithAngle);
		for (int num31 = 0; num31 < list10.Count; num31++)
		{
			Vector2 val15 = vertices[list8[num31].verticeIndex] - vertices[closestCenterPointIndex];
			float num32 = Mathf.Atan2(val15.y, val15.x);
			List<VerticesWithAngle> list14 = list10[num31].linkedVerticesWithAngles.OrderByDescending((VerticesWithAngle x) => x.verticeAngle).ToList();
			for (int num33 = 0; num33 < list14.Count; num33++)
			{
				if (!(Mathf.Abs(list14[num33].verticeAngle - num32) > 1.5f))
				{
					continue;
				}
				item8.verticeIndex = list14[num33].verticeIndex;
				item8.verticeAngle = list14[num33].verticeAngle;
				if (list14[num33].verticeAngle < 0f)
				{
					item8.verticeAngle = (float)Math.PI + list14[num33].verticeAngle + (float)Math.PI;
					list14.RemoveAt(num33);
					list14.Insert(0, item8);
					list14 = list14.OrderByDescending((VerticesWithAngle x) => x.verticeAngle).ToList();
				}
				else
				{
					item8.verticeAngle = 0f - ((float)Math.PI - list14[num33].verticeAngle) - (float)Math.PI;
					list14.RemoveAt(num33);
					list14.Add(item8);
					list14 = list14.OrderByDescending((VerticesWithAngle x) => x.verticeAngle).ToList();
				}
				num33--;
			}
			if (num30 == -1)
			{
				list12.Add(list10[num31].convexHullverticeIndex);
				list13.Add(list10[num31].convexHullverticeIndex);
			}
			bool flag2 = false;
			if (num28 != -1 && num28 != list10[num31].convexHullverticeIndex)
			{
				list13.Add(list10[num31].convexHullverticeIndex);
				if (list14.Count > 0 && num29 != -1)
				{
					Vector2 val16 = vertices[num29] - vertices[list10[num31].convexHullverticeIndex] - vertices[closestCenterPointIndex];
					if (Mathf.Atan2(val16.y, val16.x) > list14[0].verticeAngle)
					{
						for (int num34 = 0; num34 < list13.Count; num34++)
						{
							EdgeContainsCheck(new int2(list13[num34], num29));
							if (num34 == 0)
							{
								AddTriangle(num29, list13[num34], num28);
							}
							else
							{
								AddTriangle(num29, list13[num34], list13[num34 - 1]);
							}
							convexHullToDeadVerticeTriangles = triangles.Count - 1;
						}
					}
					else
					{
						for (int num35 = list13.Count - 1; num35 >= 0; num35--)
						{
							EdgeContainsCheck(new int2(list13[num35], list14[0].verticeIndex));
							flag2 = true;
							if (num35 == list13.Count - 1)
							{
								AddTriangle(list14[0].verticeIndex, num28, list13[num35]);
							}
							else
							{
								AddTriangle(list14[0].verticeIndex, list13[num35], list13[num35 + 1]);
							}
							convexHullToDeadVerticeTriangles = triangles.Count - 1;
						}
					}
				}
				else if (list14.Count > 0 && num29 == -1 && num31 - 1 >= 0 && list10[num31 - 1].linkedVerticesWithAngles.Count > 0)
				{
					EdgeContainsCheck(new int2(list10[num31 - 1].linkedVerticesWithAngles[0].verticeIndex, list10[num31].convexHullverticeIndex));
					AddTriangle(list10[num31 - 1].convexHullverticeIndex, list10[num31 - 1].linkedVerticesWithAngles[0].verticeIndex, list10[num31].convexHullverticeIndex);
					convexHullToDeadVerticeTriangles = triangles.Count - 1;
				}
			}
			for (int num36 = 0; num36 < list14.Count; num36++)
			{
				if (num27 == -1)
				{
					num27 = list14[num36].verticeIndex;
					num28 = list14[num36].verticeIndex;
					if (num30 == -1)
					{
						num30 = list14[num36].verticeIndex;
						yIndex = list10[num31].convexHullverticeIndex;
					}
				}
				else
				{
					if (num27 == list14[num36].verticeIndex)
					{
						continue;
					}
					EdgeContainsCheck(new int2(num27, list14[num36].verticeIndex));
					if (flag2 && num31 - 1 >= 0 && list13.Count > 0)
					{
						AddTriangle(list13[0], num27, list14[num36].verticeIndex);
						convexHullToDeadVerticeTriangles = triangles.Count - 1;
						flag2 = false;
					}
					if (triangles.Count == count && list13.Count > 0)
					{
						for (int num37 = 0; num37 < list13.Count; num37++)
						{
							EdgeContainsCheck(new int2(num27, list13[num37]));
							if (num37 == 0)
							{
								AddTriangle(num27, list10[num31 - (list13.Count - 1)].convexHullverticeIndex, list13[num37]);
							}
							else
							{
								AddTriangle(num27, list13[num37], list13[num37 - 1]);
							}
						}
					}
					AddTriangle(list10[num31].convexHullverticeIndex, num27, list14[num36].verticeIndex);
					convexHullToDeadVerticeTriangles = triangles.Count - 1;
					num27 = list14[num36].verticeIndex;
					num28 = list10[num31].convexHullverticeIndex;
					num29 = list14[num36].verticeIndex;
					list13.Clear();
					list13.Add(list10[num31].convexHullverticeIndex);
					if (flag)
					{
						continue;
					}
					flag = true;
					for (int num38 = 0; num38 < list13.Count - 1; num38++)
					{
						EdgeContainsCheck(new int2(num29, list13[num38]));
						if (num36 == list13.Count - 1)
						{
							AddTriangle(num29, num28, list13[num36]);
						}
						else if (list13.Count >= 2 && num36 >= 1)
						{
							AddTriangle(num29, list13[num36], list13[num36 + 1]);
						}
						convexHullToDeadVerticeTriangles = triangles.Count - 1;
					}
				}
			}
			if (num31 == 0 && num28 == -1 && list10[num31].linkedVerticesWithAngles.Count != 0)
			{
				num28 = list10[num31].convexHullverticeIndex;
				num27 = list10[num31].linkedVerticesWithAngles[0].verticeIndex;
			}
			if (num31 == list10.Count - 1)
			{
				for (int num39 = list12.Count - 1; num39 >= 0; num39--)
				{
					EdgeContainsCheck(new int2(num30, list12[num39]));
					if (num39 == list12.Count - 1)
					{
						AddTriangle(num30, yIndex, list12[num39]);
					}
					else if (list12.Count >= 2 && num39 < list12.Count - 1)
					{
						AddTriangle(num30, list12[num39 + 1], list12[num39]);
					}
					convexHullToDeadVerticeTriangles = triangles.Count - 1;
				}
				for (int num40 = list13.Count - 1; num40 >= 0; num40--)
				{
					EdgeContainsCheck(new int2(num30, list13[num40]));
					if (list12.Count > 0)
					{
						if (num40 == list13.Count - 1)
						{
							AddTriangle(num30, list12[0], list13[num40]);
						}
						else if (list13.Count >= 2)
						{
							AddTriangle(num30, list13[num40 + 1], list13[num40]);
						}
					}
					else
					{
						if (num40 == list13.Count - 1)
						{
							AddTriangle(num30, yIndex, list13[num40]);
						}
						else if (list13.Count >= 2)
						{
							AddTriangle(num30, list13[num40 + 1], list13[num40]);
						}
						convexHullToDeadVerticeTriangles = triangles.Count - 1;
					}
				}
				EdgeContainsCheck(new int2(num29, num30));
				if (list13.Count > 0)
				{
					AddTriangle(num29, num30, list13[0]);
				}
				else if (list10.Count > num31)
				{
					AddTriangle(num29, num30, list10[num31].convexHullverticeIndex);
				}
				convexHullToDeadVerticeTriangles = triangles.Count - 1;
			}
			num11++;
			if (num11 > 500)
			{
				break;
			}
		}
		outterCircleBranchVerticeLink = edges.Count - 1;
		FindOutterTriangleCandidateVertices();
		IsEdgeCreated = true;
	}

	private void EdgeContainsCheck(int edgeNumber, ref int2 edge, ref Triangle triangle)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < edges.Count; i++)
		{
			if (edges[i].x == edge.x && edges[i].y == edge.y)
			{
				SetEdge(edgeNumber, i, ref triangle);
				return;
			}
			if (edges[i].x == edge.y && edges[i].y == edge.x)
			{
				SetEdge(edgeNumber, i, ref triangle);
				return;
			}
		}
		edges.Add(edge);
		SetEdge(edgeNumber, edges.Count - 1, ref triangle);
	}

	private bool EdgeContainsCheck(int2 edge)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < edges.Count; i++)
		{
			if (edges[i].x == edge.x && edges[i].y == edge.y)
			{
				return true;
			}
			if (edges[i].x == edge.y && edges[i].y == edge.x)
			{
				return true;
			}
		}
		edges.Add(edge);
		return false;
	}

	private bool EdgeContainsCheck(int2 edge, int x, int y, int z)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < edges.Count; i++)
		{
			if (edges[i].x == edge.x && edges[i].y == edge.y)
			{
				return true;
			}
			if (edges[i].x == edge.y && edges[i].y == edge.x)
			{
				return true;
			}
		}
		edges.Add(edge);
		Triangle triangle = new Triangle();
		triangle.vertices.x = x;
		triangle.vertices.y = y;
		triangle.vertices.z = z;
		triangle.edges.x = GetEdgeIndex(x, y);
		triangle.edges.y = GetEdgeIndex(y, z);
		triangle.edges.z = GetEdgeIndex(z, x);
		triangles.Add(triangle);
		return false;
	}

	private int GetEdgeIndex(int x, int y)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < edges.Count; i++)
		{
			if (edges[i].x == x && edges[i].y == y)
			{
				return i;
			}
			if (edges[i].y == x && edges[i].x == y)
			{
				return i;
			}
		}
		return -1;
	}

	private void SetEdge(int edgeNumber, int edgeIndex, ref Triangle triangle)
	{
		switch (edgeNumber)
		{
		case 0:
			triangle.edges.x = edgeIndex;
			break;
		case 1:
			triangle.edges.y = edgeIndex;
			break;
		case 2:
			triangle.edges.z = edgeIndex;
			break;
		}
	}

	private int GetFirstEdgesFromTheLast(int pointIndex, int closestCenterPointIndex, List<int> deadEdgeInConvexHull)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		for (int num = edges.Count - 1; num >= 0; num--)
		{
			bool flag = false;
			for (int i = 0; i < deadEdgeInConvexHull.Count; i++)
			{
				if (num == deadEdgeInConvexHull[i])
				{
					flag = true;
					break;
				}
			}
			if (!flag && edges[num].x != closestCenterPointIndex && edges[num].y != closestCenterPointIndex && (edges[num].x == pointIndex || edges[num].y == pointIndex))
			{
				return num;
			}
		}
		return -1;
	}

	private int GetSecondEdgesFromTheLast(int pointIndex, int closestCenterPointIndex, List<int> deadEdgeInConvexHull)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		for (int num2 = edges.Count - 1; num2 >= 0; num2--)
		{
			bool flag = false;
			for (int i = 0; i < deadEdgeInConvexHull.Count; i++)
			{
				if (num2 == deadEdgeInConvexHull[i])
				{
					flag = true;
					break;
				}
			}
			if (!flag && edges[num2].x != closestCenterPointIndex && edges[num2].y != closestCenterPointIndex && (edges[num2].x == pointIndex || edges[num2].y == pointIndex))
			{
				num++;
				if (num == 2)
				{
					return num2;
				}
			}
		}
		return -1;
	}

	private void FindOutterTriangleCandidateVertices()
	{
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Unknown result type (might be due to invalid IL or missing references)
		//IL_056a: Unknown result type (might be due to invalid IL or missing references)
		//IL_057d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0587: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0591: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_0513: Unknown result type (might be due to invalid IL or missing references)
		//IL_0526: Unknown result type (might be due to invalid IL or missing references)
		//IL_0530: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0544: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_062c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0636: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_0653: Unknown result type (might be due to invalid IL or missing references)
		//IL_0658: Unknown result type (might be due to invalid IL or missing references)
		//IL_065d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0667: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05df: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Unknown result type (might be due to invalid IL or missing references)
		//IL_0606: Unknown result type (might be due to invalid IL or missing references)
		//IL_0610: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0685: Unknown result type (might be due to invalid IL or missing references)
		//IL_0696: Unknown result type (might be due to invalid IL or missing references)
		//IL_069b: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0713: Unknown result type (might be due to invalid IL or missing references)
		//IL_0718: Unknown result type (might be due to invalid IL or missing references)
		//IL_071d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0727: Unknown result type (might be due to invalid IL or missing references)
		//IL_073e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0743: Unknown result type (might be due to invalid IL or missing references)
		//IL_0748: Unknown result type (might be due to invalid IL or missing references)
		//IL_074b: Unknown result type (might be due to invalid IL or missing references)
		//IL_074d: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		List<OutterVerticesCandidate> list = new List<OutterVerticesCandidate>();
		for (int i = (int)circleVerticesRatio; (float)i < outerVerticesRatio; i++)
		{
			OutterVerticesCandidate item = default(OutterVerticesCandidate);
			item.verticeIndex = i;
			item.relateEdge = new List<int>();
			list.Add(item);
		}
		for (int j = 0; j < list.Count; j++)
		{
			List<int> allRelateEdges = GetAllRelateEdges(list[j].verticeIndex);
			if (allRelateEdges.Count <= 0)
			{
				list.RemoveAt(j);
				j--;
				continue;
			}
			for (int k = 0; k < allRelateEdges.Count; k++)
			{
				list[j].relateEdge.Add(allRelateEdges[k]);
			}
		}
		int num = 0;
		while (list.Count > 0)
		{
			if (list[0].relateEdge.Count == 2)
			{
				int num2 = 0;
				for (int l = 0; l < list[0].relateEdge.Count; l++)
				{
					if ((float)edges[list[0].relateEdge[l]].x >= outerVerticesRatio || (float)edges[list[0].relateEdge[l]].y >= outerVerticesRatio)
					{
						num2++;
					}
				}
				int num3 = list[0].relateEdge[0];
				int num4 = list[0].relateEdge[1];
				if (num2 == 1)
				{
					int num5 = 0;
					int num6 = 0;
					Vector2 val;
					if (list[0].verticeIndex == edges[num3].x)
					{
						val = vertices[edges[num3].y] - vertices[edges[num3].x];
						num5 = edges[num3].y;
					}
					else
					{
						val = vertices[edges[num3].x] - vertices[edges[num3].y];
						num5 = edges[num3].x;
					}
					Vector2 val2;
					if (list[0].verticeIndex == edges[num4].x)
					{
						val2 = vertices[edges[num4].y] - vertices[edges[num4].x];
						num6 = edges[num4].y;
					}
					else
					{
						val2 = vertices[edges[num4].x] - vertices[edges[num4].y];
						num6 = edges[num4].x;
					}
					Vector2 val3 = vertices[list[0].verticeIndex] - vertices[closestCenterPointIndex];
					float num7 = Vector2.Dot(((Vector2)(ref val3)).normalized, ((Vector2)(ref val)).normalized);
					float num8 = Vector2.Dot(((Vector2)(ref val3)).normalized, ((Vector2)(ref val2)).normalized);
					if (num7 + num8 <= 0.099f)
					{
						list.RemoveAt(0);
					}
					else
					{
						edges.Add(new int2(num5, num6));
						Vector2 first = vertices[num5] - vertices[list[0].verticeIndex];
						Vector2 second = vertices[num6] - vertices[list[0].verticeIndex];
						if (CrossProduct(first, second) < 0f)
						{
							AddTriangle(list[0].verticeIndex, num5, num6);
						}
						else
						{
							AddTriangle(list[0].verticeIndex, num6, num5);
						}
						deadEdgeOutterCircle.Add(num3);
						deadEdgeOutterCircle.Add(num4);
						OutterVerticesCandidate item2 = default(OutterVerticesCandidate);
						if ((float)num5 >= outerVerticesRatio)
						{
							item2.verticeIndex = num6;
						}
						else
						{
							item2.verticeIndex = num5;
						}
						item2.relateEdge = GetAllRelateEdges(item2.verticeIndex);
						for (int m = 0; m < list.Count; m++)
						{
							if (list[m].verticeIndex == item2.verticeIndex)
							{
								list.RemoveAt(m);
							}
						}
						list.Add(item2);
						list.RemoveAt(0);
					}
				}
				else
				{
					int num9 = 0;
					int num10 = 0;
					Vector2 val4;
					if (list[0].verticeIndex == edges[num3].x)
					{
						val4 = vertices[edges[num3].y] - vertices[edges[num3].x];
						num9 = edges[num3].y;
					}
					else
					{
						val4 = vertices[edges[num3].x] - vertices[edges[num3].y];
						num9 = edges[num3].x;
					}
					Vector2 val5;
					if (list[0].verticeIndex == edges[num4].x)
					{
						val5 = vertices[edges[num4].y] - vertices[edges[num4].x];
						num10 = edges[num4].y;
					}
					else
					{
						val5 = vertices[edges[num4].x] - vertices[edges[num4].y];
						num10 = edges[num4].x;
					}
					Vector2 val6 = vertices[list[0].verticeIndex] - vertices[closestCenterPointIndex];
					float num11 = Vector2.Dot(((Vector2)(ref val6)).normalized, ((Vector2)(ref val4)).normalized);
					float num12 = Vector2.Dot(((Vector2)(ref val6)).normalized, ((Vector2)(ref val5)).normalized);
					if (num11 + num12 <= 0.099f)
					{
						list.RemoveAt(0);
					}
					else
					{
						edges.Add(new int2(num9, num10));
						Vector2 first2 = vertices[num9] - vertices[list[0].verticeIndex];
						Vector2 second2 = vertices[num10] - vertices[list[0].verticeIndex];
						if (CrossProduct(first2, second2) < 0f)
						{
							AddTriangle(list[0].verticeIndex, num9, num10);
						}
						else
						{
							AddTriangle(list[0].verticeIndex, num10, num9);
						}
						deadEdgeOutterCircle.Add(num3);
						deadEdgeOutterCircle.Add(num4);
						OutterVerticesCandidate item3 = default(OutterVerticesCandidate);
						item3.verticeIndex = num9;
						item3.relateEdge = GetAllRelateEdges(item3.verticeIndex);
						for (int n = 0; n < list.Count; n++)
						{
							if (list[n].verticeIndex == item3.verticeIndex)
							{
								list.RemoveAt(n);
							}
						}
						list.Add(item3);
						item3 = default(OutterVerticesCandidate);
						item3.verticeIndex = num10;
						item3.relateEdge = GetAllRelateEdges(item3.verticeIndex);
						for (int num13 = 0; num13 < list.Count; num13++)
						{
							if (list[num13].verticeIndex == item3.verticeIndex)
							{
								list.RemoveAt(num13);
							}
						}
						list.Add(item3);
						list.RemoveAt(0);
					}
				}
			}
			else
			{
				list.RemoveAt(0);
			}
			num++;
			if (num > 100)
			{
				break;
			}
		}
	}

	private List<int> GetAllRelateEdges(int verticeIndex)
	{
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		for (int i = 0; i < edges.Count; i++)
		{
			bool flag = false;
			for (int j = 0; j < deadEdgeOutterCircle.Count; j++)
			{
				if (i == deadEdgeOutterCircle[j])
				{
					flag = true;
					break;
				}
			}
			if (flag || (edges[i].x != verticeIndex && edges[i].y != verticeIndex))
			{
				continue;
			}
			bool flag2 = false;
			if ((float)edges[i].x >= outerVerticesRatio)
			{
				list2.Add(edges[i].x);
			}
			if ((float)edges[i].y >= outerVerticesRatio)
			{
				list2.Add(edges[i].y);
			}
			for (int k = 0; k < convexHullPointsIndex.Count; k++)
			{
				if (edges[i].x == convexHullPointsIndex[k] || edges[i].y == convexHullPointsIndex[k])
				{
					flag2 = true;
				}
			}
			if (!flag2)
			{
				list.Add(i);
			}
		}
		if (list2.Count == 2)
		{
			int num = list2[0];
			int num2 = list2[1];
			Vector2 first = vertices[num] - vertices[verticeIndex];
			Vector2 second = vertices[num2] - vertices[verticeIndex];
			if (CrossProduct(first, second) < 0f)
			{
				AddTriangle(verticeIndex, num, num2);
			}
			else
			{
				AddTriangle(verticeIndex, num2, num);
			}
			list.Clear();
		}
		return list;
	}

	public void SetTriangles()
	{
		IsEdgeCreated = false;
		IsTriangleCreated = true;
		for (int i = 0; i < triangles.Count; i++)
		{
		}
	}

	private float CrossProduct(Vector2 first, Vector2 second)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		return first.x * second.y - first.y * second.x;
	}

	private void AddTriangle(int xIndex, int yIndex, int zIndex)
	{
		Triangle triangle = new Triangle();
		int edgeIndex = GetEdgeIndex(xIndex, yIndex);
		int edgeIndex2 = GetEdgeIndex(yIndex, zIndex);
		int edgeIndex3 = GetEdgeIndex(zIndex, xIndex);
		if (edgeIndex == -1 || edgeIndex2 == -1 || edgeIndex3 == -1)
		{
			return;
		}
		new List<int>();
		for (int i = 0; i < triangles.Count; i++)
		{
			if ((xIndex == triangles[i].vertices.x || xIndex == triangles[i].vertices.y || xIndex == triangles[i].vertices.z) && (yIndex == triangles[i].vertices.x || yIndex == triangles[i].vertices.y || yIndex == triangles[i].vertices.z) && (zIndex == triangles[i].vertices.x || zIndex == triangles[i].vertices.y || zIndex == triangles[i].vertices.z))
			{
				return;
			}
		}
		triangle.vertices.x = xIndex;
		triangle.vertices.y = yIndex;
		triangle.vertices.z = zIndex;
		SetEdge(0, edgeIndex, ref triangle);
		SetEdge(1, edgeIndex2, ref triangle);
		SetEdge(2, edgeIndex3, ref triangle);
		triangles.Add(triangle);
	}

	private int FindPairTriangleWithEdge(int edgeIndex, int triangleIndex)
	{
		int result = -1;
		for (int i = 0; i < triangles.Count; i++)
		{
			if (triangleIndex != i && (triangles[i].edges.x == edgeIndex || triangles[i].edges.y == edgeIndex || triangles[i].edges.z == edgeIndex))
			{
				return i;
			}
		}
		return result;
	}

	public void CreatePairTriangles()
	{
		for (int i = 0; i < triangles.Count; i++)
		{
			triangles[i].edgePairedTriangle.x = FindPairTriangleWithEdge(triangles[i].edges.x, i);
			triangles[i].edgePairedTriangle.y = FindPairTriangleWithEdge(triangles[i].edges.y, i);
			triangles[i].edgePairedTriangle.z = FindPairTriangleWithEdge(triangles[i].edges.z, i);
		}
		IsTriangleCreated = false;
		IsVertexCreated = false;
		IsPairedTriangleCreated = true;
	}

	public void SettingMeshData()
	{
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		verticesUV = (float2[])(object)new float2[vertices.Length];
		for (int i = 0; i < verticesUV.Length; i++)
		{
			verticesUV[i].x = (vertices[i].x + halfScreenSizeX) / _screenSize.x;
			verticesUV[i].y = (vertices[i].y + halfScreenSizeY) / _screenSize.y;
		}
		for (int j = 0; j < triangles.Count; j++)
		{
			triangles[j].normal = new Vector3(0f, 0f, 1f);
		}
		for (int k = 0; k < triangles.Count; k++)
		{
			Vector2 first = vertices[triangles[k].vertices.y] - vertices[triangles[k].vertices.x];
			Vector2 second = vertices[triangles[k].vertices.z] - vertices[triangles[k].vertices.x];
			triangles[k].crossProduct = math.abs(CrossProduct(first, second));
			if (k < convexHullToDeadVerticeTriangles)
			{
				triangles[k].randomProbabilities = _InnerCircleTriangleRandomProbabilities;
				triangles[k].decreaseAmount = _InnerCircleTriangleRPDecreaseAmount;
			}
			else
			{
				triangles[k].randomProbabilities = _outterCircleTriangleRandomProbabilities;
				triangles[k].decreaseAmount = _outterCircleTriangleRPDecreaseAmount;
			}
		}
		for (int l = 0; l < triangles.Count; l++)
		{
			triangles[l].triangleIndex = l;
		}
	}

	public void CreateChunk()
	{
		List<Triangle> tempTriangleList = new List<Triangle>(triangles);
		chunks = new List<ChunkData>();
		int num = 0;
		while (tempTriangleList.Count > 0)
		{
			ChunkData chunkData = new ChunkData();
			chunkData.baseTriangleIndex = tempTriangleList[0].triangleIndex;
			chunkData.chunkedTriangleIndexes = new List<int>();
			chunkData.chunkedTriangleIndexes.Add(tempTriangleList[0].triangleIndex);
			bool inside = true;
			if (tempTriangleList[0].triangleIndex > convexHullTriangles)
			{
				inside = false;
			}
			tempTriangleList[0].MakeChunk(num, inside, convexHullTriangles, _innerCircleTriangleAreaLevel, _outterCircleTriangleAreaLevel, triangles, ref tempTriangleList, ref chunkData.chunkedTriangleIndexes);
			chunks.Add(chunkData);
			num++;
			if (num > 90)
			{
				break;
			}
		}
		IsPairedTriangleCreated = false;
		IsChunkCreated = true;
	}

	public void CreateMeshes()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		//IL_00bc: Expected O, but got Unknown
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1234: Unknown result type (might be due to invalid IL or missing references)
		//IL_1257: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0632: Unknown result type (might be due to invalid IL or missing references)
		//IL_0637: Unknown result type (might be due to invalid IL or missing references)
		//IL_063c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0549: Unknown result type (might be due to invalid IL or missing references)
		//IL_054e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0553: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_046f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_071c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_0722: Unknown result type (might be due to invalid IL or missing references)
		//IL_0727: Unknown result type (might be due to invalid IL or missing references)
		//IL_072c: Unknown result type (might be due to invalid IL or missing references)
		//IL_072d: Unknown result type (might be due to invalid IL or missing references)
		//IL_072f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Unknown result type (might be due to invalid IL or missing references)
		//IL_0736: Unknown result type (might be due to invalid IL or missing references)
		//IL_0737: Unknown result type (might be due to invalid IL or missing references)
		//IL_0739: Unknown result type (might be due to invalid IL or missing references)
		//IL_073e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0740: Unknown result type (might be due to invalid IL or missing references)
		//IL_0741: Unknown result type (might be due to invalid IL or missing references)
		//IL_0743: Unknown result type (might be due to invalid IL or missing references)
		//IL_0748: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0832: Unknown result type (might be due to invalid IL or missing references)
		//IL_0837: Unknown result type (might be due to invalid IL or missing references)
		//IL_0838: Unknown result type (might be due to invalid IL or missing references)
		//IL_083d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0842: Unknown result type (might be due to invalid IL or missing references)
		//IL_0843: Unknown result type (might be due to invalid IL or missing references)
		//IL_0845: Unknown result type (might be due to invalid IL or missing references)
		//IL_084a: Unknown result type (might be due to invalid IL or missing references)
		//IL_084c: Unknown result type (might be due to invalid IL or missing references)
		//IL_084d: Unknown result type (might be due to invalid IL or missing references)
		//IL_084f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0854: Unknown result type (might be due to invalid IL or missing references)
		//IL_0856: Unknown result type (might be due to invalid IL or missing references)
		//IL_0857: Unknown result type (might be due to invalid IL or missing references)
		//IL_0859: Unknown result type (might be due to invalid IL or missing references)
		//IL_085e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0860: Unknown result type (might be due to invalid IL or missing references)
		//IL_0872: Unknown result type (might be due to invalid IL or missing references)
		//IL_0883: Unknown result type (might be due to invalid IL or missing references)
		//IL_088e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0893: Unknown result type (might be due to invalid IL or missing references)
		//IL_0898: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0906: Unknown result type (might be due to invalid IL or missing references)
		//IL_090b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0910: Unknown result type (might be due to invalid IL or missing references)
		//IL_0924: Unknown result type (might be due to invalid IL or missing references)
		//IL_0937: Unknown result type (might be due to invalid IL or missing references)
		//IL_0942: Unknown result type (might be due to invalid IL or missing references)
		//IL_0947: Unknown result type (might be due to invalid IL or missing references)
		//IL_094c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0960: Unknown result type (might be due to invalid IL or missing references)
		//IL_0973: Unknown result type (might be due to invalid IL or missing references)
		//IL_097e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0983: Unknown result type (might be due to invalid IL or missing references)
		//IL_0988: Unknown result type (might be due to invalid IL or missing references)
		//IL_099c: Unknown result type (might be due to invalid IL or missing references)
		//IL_09af: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0adc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e24: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eeb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1004: Unknown result type (might be due to invalid IL or missing references)
		//IL_1005: Unknown result type (might be due to invalid IL or missing references)
		//IL_1007: Unknown result type (might be due to invalid IL or missing references)
		//IL_100c: Unknown result type (might be due to invalid IL or missing references)
		//IL_100e: Unknown result type (might be due to invalid IL or missing references)
		//IL_100f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1011: Unknown result type (might be due to invalid IL or missing references)
		//IL_1016: Unknown result type (might be due to invalid IL or missing references)
		//IL_1018: Unknown result type (might be due to invalid IL or missing references)
		//IL_104a: Unknown result type (might be due to invalid IL or missing references)
		//IL_104f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1050: Unknown result type (might be due to invalid IL or missing references)
		//IL_1052: Unknown result type (might be due to invalid IL or missing references)
		//IL_1057: Unknown result type (might be due to invalid IL or missing references)
		//IL_1059: Unknown result type (might be due to invalid IL or missing references)
		//IL_105a: Unknown result type (might be due to invalid IL or missing references)
		//IL_105c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1061: Unknown result type (might be due to invalid IL or missing references)
		//IL_1063: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_10fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_10fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1101: Unknown result type (might be due to invalid IL or missing references)
		//IL_1103: Unknown result type (might be due to invalid IL or missing references)
		//IL_1104: Unknown result type (might be due to invalid IL or missing references)
		//IL_1106: Unknown result type (might be due to invalid IL or missing references)
		//IL_110b: Unknown result type (might be due to invalid IL or missing references)
		//IL_110d: Unknown result type (might be due to invalid IL or missing references)
		//IL_110e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1110: Unknown result type (might be due to invalid IL or missing references)
		//IL_1115: Unknown result type (might be due to invalid IL or missing references)
		//IL_1117: Unknown result type (might be due to invalid IL or missing references)
		//IL_1141: Unknown result type (might be due to invalid IL or missing references)
		//IL_1146: Unknown result type (might be due to invalid IL or missing references)
		//IL_1147: Unknown result type (might be due to invalid IL or missing references)
		//IL_1149: Unknown result type (might be due to invalid IL or missing references)
		//IL_114e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1150: Unknown result type (might be due to invalid IL or missing references)
		//IL_1151: Unknown result type (might be due to invalid IL or missing references)
		//IL_1153: Unknown result type (might be due to invalid IL or missing references)
		//IL_1158: Unknown result type (might be due to invalid IL or missing references)
		//IL_115a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1187: Unknown result type (might be due to invalid IL or missing references)
		//IL_118c: Unknown result type (might be due to invalid IL or missing references)
		//IL_118d: Unknown result type (might be due to invalid IL or missing references)
		//IL_118f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1194: Unknown result type (might be due to invalid IL or missing references)
		//IL_1196: Unknown result type (might be due to invalid IL or missing references)
		//IL_1197: Unknown result type (might be due to invalid IL or missing references)
		//IL_1199: Unknown result type (might be due to invalid IL or missing references)
		//IL_119e: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a0: Unknown result type (might be due to invalid IL or missing references)
		IsVertexCreated = false;
		IsEdgeCreated = false;
		IsTriangleCreated = false;
		IsPairedTriangleCreated = false;
		IsChunkCreated = false;
		rootObject = new GameObject();
		((Object)rootObject).name = "MirroBreak";
		rootObject.transform.SetParent(((Component)Map.Instance).transform);
		rootObject.SetActive(false);
		chunkObject = new List<GameObject>();
		Vector2 val8 = default(Vector2);
		for (int i = 0; i < chunks.Count; i++)
		{
			GameObject val = new GameObject();
			((Object)val).name = "chunks" + i;
			val.transform.parent = rootObject.transform;
			MeshFilter obj = val.AddComponent<MeshFilter>();
			Mesh val2 = new Mesh();
			Mesh val3 = val2;
			obj.mesh = val2;
			Mesh val4 = val3;
			MeshRenderer obj2 = val.AddComponent<MeshRenderer>();
			((Renderer)obj2).material = _material;
			((Renderer)obj2).sortingLayerName = "UI";
			((Renderer)obj2).sortingOrder = 11;
			Vector2 val5 = default(Vector2);
			Vector3[] array = (Vector3[])(object)new Vector3[3 * chunks[i].chunkedTriangleIndexes.Count * 2 * 2];
			int[] array2 = new int[24 * chunks[i].chunkedTriangleIndexes.Count];
			Vector2[] array3 = (Vector2[])(object)new Vector2[3 * chunks[i].chunkedTriangleIndexes.Count * 2 * 2];
			Vector2[] array4 = (Vector2[])(object)new Vector2[3 * chunks[i].chunkedTriangleIndexes.Count * 2 * 2];
			Vector3[] array5 = (Vector3[])(object)new Vector3[3 * chunks[i].chunkedTriangleIndexes.Count * 2 * 2];
			for (int j = 0; j < chunks[i].chunkedTriangleIndexes.Count; j++)
			{
				val5 += vertices[triangles[chunks[i].chunkedTriangleIndexes[j]].vertices.x];
				val5 += vertices[triangles[chunks[i].chunkedTriangleIndexes[j]].vertices.y];
				val5 += vertices[triangles[chunks[i].chunkedTriangleIndexes[j]].vertices.z];
			}
			val5 /= (float)(3 * chunks[i].chunkedTriangleIndexes.Count);
			Vector2 val6 = val5;
			val5 += new Vector2(((Component)this).transform.position.x, ((Component)this).transform.position.y);
			val.transform.position = Vector2.op_Implicit(val5);
			Vector2 val7;
			if (i < chunkInsideIndex)
			{
				chunks[i].chunkBasePosition = Vector2.op_Implicit(val5);
				chunks[i].rotationSpeed = Random.Range(50, 200);
				ChunkData chunkData = chunks[i];
				val7 = val6 - _centerPoint;
				chunkData.rotationVector = Vector2.op_Implicit(((Vector2)(ref val7)).normalized) + new Vector3(0f, 0f, ((Component)this).transform.position.z);
				if (Random.Range(0, 2) == 0)
				{
					chunks[i].rotationVector = -chunks[i].rotationVector;
				}
				chunks[i].InitialRotationDisplacement = Random.Range(0f - _displacement_rotation, _displacement_rotation);
				chunks[i].moveSpeed = Random.Range(0.8f, 1.5f);
				ChunkData chunkData2 = chunks[i];
				val7 = val6 - _centerPoint;
				chunkData2.moveDirection = Vector2.op_Implicit(((Vector2)(ref val7)).normalized);
				ChunkData chunkData3 = chunks[i];
				chunkData3.moveDirection += new Vector3(Random.Range(0f, chunks[i].moveDirection.x), Random.Range(0f, chunks[i].moveDirection.y), Random.Range(5f, 10f));
				chunks[i].scale = Random.Range(0.01f, 0.5f);
			}
			else
			{
				chunks[i].chunkBasePosition = Vector2.op_Implicit(val5);
				chunks[i].rotationSpeed = Random.Range(50, 200);
				ChunkData chunkData4 = chunks[i];
				val7 = val6 - _centerPoint;
				chunkData4.rotationVector = Vector2.op_Implicit(((Vector2)(ref val7)).normalized) + new Vector3(0f, 0f, ((Component)this).transform.position.z);
				if (Random.Range(0, 2) == 0)
				{
					chunks[i].rotationVector = -chunks[i].rotationVector;
				}
				chunks[i].InitialRotationDisplacement = Random.Range(0f - _displacement_rotation, _displacement_rotation);
				chunks[i].moveSpeed = Random.Range(1f, 2f);
				ChunkData chunkData5 = chunks[i];
				val7 = val6 - _centerPoint;
				chunkData5.moveDirection = Vector2.op_Implicit(((Vector2)(ref val7)).normalized) * 1.5f;
				ChunkData chunkData6 = chunks[i];
				chunkData6.moveDirection += new Vector3(Random.Range(0f, chunks[i].moveDirection.x), Random.Range(0f, chunks[i].moveDirection.y), Random.Range(5f, 10f));
				chunks[i].scale = Random.Range(0.01f, 0.1f);
			}
			float num = Random.Range(0f - _displacement_uv, _displacement_uv * 0.5f);
			((Vector2)(ref val8))._002Ector(Random.Range((0f - _displacement_uv) * 0.5f, _displacement_uv * 0.5f), Random.Range((0f - _displacement_uv) * 0.5f, _displacement_uv * 0.5f));
			for (int k = 0; k < chunks[i].chunkedTriangleIndexes.Count; k++)
			{
				array[k * 12] = (array[k * 12 + 3] = (array[k * 12 + 6] = (array[k * 12 + 9] = Vector2.op_Implicit(vertices[triangles[chunks[i].chunkedTriangleIndexes[k]].vertices.x] - val5))));
				array[k * 12 + 1] = (array[k * 12 + 4] = (array[k * 12 + 7] = (array[k * 12 + 10] = Vector2.op_Implicit(vertices[triangles[chunks[i].chunkedTriangleIndexes[k]].vertices.y] - val5))));
				array[k * 12 + 2] = (array[k * 12 + 5] = (array[k * 12 + 8] = (array[k * 12 + 11] = Vector2.op_Implicit(vertices[triangles[chunks[i].chunkedTriangleIndexes[k]].vertices.z] - val5))));
				ref Vector3 reference = ref array[k * 12];
				reference += ((Vector3)(ref array[k * 12])).normalized * _gap;
				ref Vector3 reference2 = ref array[k * 12 + 1];
				reference2 += ((Vector3)(ref array[k * 12 + 1])).normalized * _gap;
				ref Vector3 reference3 = ref array[k * 12 + 2];
				reference3 += ((Vector3)(ref array[k * 12 + 2])).normalized * _gap;
				ref Vector3 reference4 = ref array[k * 12 + 3];
				reference4 -= ((Vector3)(ref array[k * 12 + 3])).normalized * _gap;
				ref Vector3 reference5 = ref array[k * 12 + 4];
				reference5 -= ((Vector3)(ref array[k * 12 + 4])).normalized * _gap;
				ref Vector3 reference6 = ref array[k * 12 + 5];
				reference6 -= ((Vector3)(ref array[k * 12 + 5])).normalized * _gap;
				array[k * 12].z = (array[k * 12 + 1].z = (array[k * 12 + 2].z = _thickness));
				array[k * 12 + 3].z = (array[k * 12 + 4].z = (array[k * 12 + 5].z = 0f - _thickness));
				array[k * 12 + 6] = array[k * 12];
				array[k * 12 + 7] = array[k * 12 + 1];
				array[k * 12 + 8] = array[k * 12 + 2];
				array[k * 12 + 9] = array[k * 12 + 3];
				array[k * 12 + 10] = array[k * 12 + 4];
				array[k * 12 + 11] = array[k * 12 + 5];
				array2[k * 24] = k * 12;
				array2[k * 24 + 1] = k * 12 + 1;
				array2[k * 24 + 2] = k * 12 + 2;
				array2[k * 24 + 3] = k * 12 + 3;
				array2[k * 24 + 4] = k * 12 + 4;
				array2[k * 24 + 5] = k * 12 + 5;
				array2[k * 24 + 6] = k * 12 + 7;
				array2[k * 24 + 7] = k * 12 + 6;
				array2[k * 24 + 8] = k * 12 + 9;
				array2[k * 24 + 9] = k * 12 + 9;
				array2[k * 24 + 10] = k * 12 + 6;
				array2[k * 24 + 11] = k * 12 + 10;
				array2[k * 24 + 12] = k * 12 + 8;
				array2[k * 24 + 13] = k * 12 + 7;
				array2[k * 24 + 14] = k * 12 + 11;
				array2[k * 24 + 15] = k * 12 + 11;
				array2[k * 24 + 16] = k * 12 + 7;
				array2[k * 24 + 17] = k * 12 + 10;
				array2[k * 24 + 18] = k * 12 + 6;
				array2[k * 24 + 19] = k * 12 + 8;
				array2[k * 24 + 20] = k * 12 + 9;
				array2[k * 24 + 21] = k * 12 + 9;
				array2[k * 24 + 22] = k * 12 + 8;
				array2[k * 24 + 23] = k * 12 + 11;
				val7 = (array3[k * 12] = (array3[k * 12 + 3] = float2.op_Implicit(verticesUV[triangles[chunks[i].chunkedTriangleIndexes[k]].vertices.x])));
				val7 = (array3[k * 12 + 1] = (array3[k * 12 + 4] = float2.op_Implicit(verticesUV[triangles[chunks[i].chunkedTriangleIndexes[k]].vertices.y])));
				val7 = (array3[k * 12 + 2] = (array3[k * 12 + 5] = float2.op_Implicit(verticesUV[triangles[chunks[i].chunkedTriangleIndexes[k]].vertices.z])));
				ref Vector2 reference7 = ref array3[k * 12];
				reference7 += ((Vector2)(ref array3[k * 12])).normalized * num + val8;
				ref Vector2 reference8 = ref array3[k * 12 + 1];
				reference8 += ((Vector2)(ref array3[k * 12 + 1])).normalized * num + val8;
				ref Vector2 reference9 = ref array3[k * 12 + 2];
				reference9 += ((Vector2)(ref array3[k * 12 + 2])).normalized * num + val8;
				int num2 = k * 12 + 3;
				int num3 = k * 12 + 6;
				val7 = (array3[k * 12 + 9] = array3[k * 12]);
				val7 = (array3[num2] = (array3[num3] = val7));
				int num4 = k * 12 + 4;
				int num5 = k * 12 + 7;
				val7 = (array3[k * 12 + 10] = array3[k * 12 + 1]);
				val7 = (array3[num4] = (array3[num5] = val7));
				int num6 = k * 12 + 5;
				int num7 = k * 12 + 8;
				val7 = (array3[k * 12 + 11] = array3[k * 12 + 2]);
				val7 = (array3[num6] = (array3[num7] = val7));
				array5[k * 12] = (array5[k * 12 + 1] = (array5[k * 12 + 2] = new Vector3(1f, 0f, 0f)));
				array5[k * 12 + 3] = (array5[k * 12 + 4] = (array5[k * 12 + 5] = new Vector3(-1f, 0f, 0f)));
				array5[k * 12 + 6] = (array5[k * 12 + 7] = (array5[k * 12 + 8] = new Vector3(0f, 0f, 0f)));
				array5[k * 12 + 9] = (array5[k * 12 + 10] = (array5[k * 12 + 11] = new Vector3(0f, 0f, 0f)));
				_ = ((Vector3)(ref array[k * 12])).magnitude;
				_ = ((Vector3)(ref array[k * 12 + 1])).magnitude;
				_ = ((Vector3)(ref array[k * 12 + 2])).magnitude;
				int num8 = k * 12;
				int num9 = k * 12 + 1;
				int num10 = k * 12 + 2;
				int num11 = k * 12 + 3;
				int num12 = k * 12 + 4;
				val7 = (array4[k * 12 + 5] = new Vector2(0f, 0f));
				val7 = (array4[num12] = val7);
				val7 = (array4[num11] = val7);
				val7 = (array4[num10] = val7);
				val7 = (array4[num8] = (array4[num9] = val7));
				int num13 = k * 12 + 6;
				int num14 = k * 12 + 7;
				val7 = (array4[k * 12 + 8] = new Vector2(1f, 1f));
				val7 = (array4[num13] = (array4[num14] = val7));
				int num15 = k * 12 + 9;
				int num16 = k * 12 + 10;
				val7 = (array4[k * 12 + 11] = new Vector2(1f, 0f));
				val7 = (array4[num15] = (array4[num16] = val7));
			}
			val4.vertices = array;
			val4.triangles = array2;
			val4.uv = array3;
			val4.uv2 = array4;
			val4.normals = array5;
			chunkObject.Add(val);
		}
		IsMeshCreated = true;
		rootObject.transform.localScale = new Vector3(_scaleModifier, _scaleModifier, 1f);
		if (_addRootPosition)
		{
			rootObject.transform.position = ((Component)this).transform.position;
		}
	}

	public void CreateAll()
	{
		CreateVertex();
		SetEdges();
		SetTriangles();
		CreatePairTriangles();
		SettingMeshData();
		CreateChunk();
		CreateMeshes();
	}

	public void CreateAllSomeCracked()
	{
		CreateVertex();
		SetEdges();
		SetTriangles();
		CreatePairTriangles();
		SettingMeshData();
		CreateChunk();
		CreateMeshes();
		CrackedSome(4);
	}

	public void Update()
	{
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		if (!IsInitialized || !IsMeshCreated)
		{
			return;
		}
		for (int i = 0; i < chunks.Count; i++)
		{
			if (_time > 5f)
			{
				_accelator = _time - 4.99f;
				_accelator *= _accelator * _accelator;
				_accelator += 1f;
			}
			else
			{
				_accelator = 1f;
			}
			if (_addRootPosition)
			{
				chunkObject[i].transform.position = chunks[i].chunkBasePosition + rootObject.transform.position;
			}
			else
			{
				chunkObject[i].transform.position = chunks[i].chunkBasePosition;
			}
			Transform transform = chunkObject[i].transform;
			transform.position += chunks[i].moveDirection * chunks[i].moveSpeed * _time * _accelator;
			chunkObject[i].transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
			chunkObject[i].transform.RotateAround(chunks[i].chunkBasePosition + chunks[i].moveDirection * chunks[i].moveSpeed * _time * _accelator, chunks[i].rotationVector, chunks[i].InitialRotationDisplacement + chunks[i].rotationSpeed * _time);
		}
	}

	public void ChangeSprite()
	{
		_material.SetTexture("_MainTex", (Texture)(object)_resultSprite);
		RestoreMeshes();
	}

	public void RestoreMeshes()
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_0505: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_052f: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0544: Unknown result type (might be due to invalid IL or missing references)
		//IL_0557: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_0579: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0591: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0605: Unknown result type (might be due to invalid IL or missing references)
		//IL_0617: Unknown result type (might be due to invalid IL or missing references)
		//IL_0622: Unknown result type (might be due to invalid IL or missing references)
		//IL_0627: Unknown result type (might be due to invalid IL or missing references)
		//IL_062c: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0703: Unknown result type (might be due to invalid IL or missing references)
		//IL_0719: Unknown result type (might be due to invalid IL or missing references)
		//IL_071e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Unknown result type (might be due to invalid IL or missing references)
		//IL_0739: Unknown result type (might be due to invalid IL or missing references)
		//IL_074f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0754: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < chunks.Count; i++)
		{
			chunks[i].InitialRotationDisplacement = 0f;
			Vector2[] array = (Vector2[])(object)new Vector2[3 * chunks[i].chunkedTriangleIndexes.Count * 2 * 2];
			Vector3[] array2 = (Vector3[])(object)new Vector3[3 * chunks[i].chunkedTriangleIndexes.Count * 2 * 2];
			Vector2 val = default(Vector2);
			for (int j = 0; j < chunks[i].chunkedTriangleIndexes.Count; j++)
			{
				val += vertices[triangles[chunks[i].chunkedTriangleIndexes[j]].vertices.x];
				val += vertices[triangles[chunks[i].chunkedTriangleIndexes[j]].vertices.y];
				val += vertices[triangles[chunks[i].chunkedTriangleIndexes[j]].vertices.z];
			}
			val /= (float)(3 * chunks[i].chunkedTriangleIndexes.Count);
			for (int k = 0; k < chunks[i].chunkedTriangleIndexes.Count; k++)
			{
				array[k * 12] = (array[k * 12 + 3] = float2.op_Implicit(verticesUV[triangles[chunks[i].chunkedTriangleIndexes[k]].vertices.x]));
				array[k * 12 + 1] = (array[k * 12 + 4] = float2.op_Implicit(verticesUV[triangles[chunks[i].chunkedTriangleIndexes[k]].vertices.y]));
				array[k * 12 + 2] = (array[k * 12 + 5] = float2.op_Implicit(verticesUV[triangles[chunks[i].chunkedTriangleIndexes[k]].vertices.z]));
				array[k * 12 + 3] = (array[k * 12 + 6] = (array[k * 12 + 9] = array[k * 12]));
				array[k * 12 + 4] = (array[k * 12 + 7] = (array[k * 12 + 10] = array[k * 12 + 1]));
				array[k * 12 + 5] = (array[k * 12 + 8] = (array[k * 12 + 11] = array[k * 12 + 2]));
				array2[k * 12] = (array2[k * 12 + 3] = (array2[k * 12 + 6] = (array2[k * 12 + 9] = Vector2.op_Implicit(vertices[triangles[chunks[i].chunkedTriangleIndexes[k]].vertices.x] - val))));
				array2[k * 12 + 1] = (array2[k * 12 + 4] = (array2[k * 12 + 7] = (array2[k * 12 + 10] = Vector2.op_Implicit(vertices[triangles[chunks[i].chunkedTriangleIndexes[k]].vertices.y] - val))));
				array2[k * 12 + 2] = (array2[k * 12 + 5] = (array2[k * 12 + 8] = (array2[k * 12 + 11] = Vector2.op_Implicit(vertices[triangles[chunks[i].chunkedTriangleIndexes[k]].vertices.z] - val))));
				ref Vector3 reference = ref array2[k * 12];
				reference += ((Vector3)(ref array2[k * 12])).normalized * _gap;
				ref Vector3 reference2 = ref array2[k * 12 + 1];
				reference2 += ((Vector3)(ref array2[k * 12 + 1])).normalized * _gap;
				ref Vector3 reference3 = ref array2[k * 12 + 2];
				reference3 += ((Vector3)(ref array2[k * 12 + 2])).normalized * _gap;
				ref Vector3 reference4 = ref array2[k * 12 + 3];
				reference4 -= ((Vector3)(ref array2[k * 12 + 3])).normalized * _gap;
				ref Vector3 reference5 = ref array2[k * 12 + 4];
				reference5 -= ((Vector3)(ref array2[k * 12 + 4])).normalized * _gap;
				ref Vector3 reference6 = ref array2[k * 12 + 5];
				reference6 -= ((Vector3)(ref array2[k * 12 + 5])).normalized * _gap;
				array2[k * 12].z = (array2[k * 12 + 1].z = (array2[k * 12 + 2].z = _thickness));
				array2[k * 12 + 3].z = (array2[k * 12 + 4].z = (array2[k * 12 + 5].z = 0f - _thickness));
				array2[k * 12 + 6] = array2[k * 12];
				array2[k * 12 + 7] = array2[k * 12 + 1];
				array2[k * 12 + 8] = array2[k * 12 + 2];
				array2[k * 12 + 9] = array2[k * 12 + 3];
				array2[k * 12 + 10] = array2[k * 12 + 4];
				array2[k * 12 + 11] = array2[k * 12 + 5];
			}
			Mesh mesh = chunkObject[i].GetComponent<MeshFilter>().mesh;
			mesh.vertices = array2;
			mesh.uv = array;
		}
	}

	public void CrackedSome(int num)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0617: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_0636: Unknown result type (might be due to invalid IL or missing references)
		//IL_064b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_0666: Unknown result type (might be due to invalid IL or missing references)
		//IL_066b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0681: Unknown result type (might be due to invalid IL or missing references)
		//IL_0686: Unknown result type (might be due to invalid IL or missing references)
		//IL_069c: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_0505: Unknown result type (might be due to invalid IL or missing references)
		//IL_0518: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0552: Unknown result type (might be due to invalid IL or missing references)
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
		//IL_056f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_0579: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < chunks.Count; i++)
		{
			chunks[i].InitialRotationDisplacement = 0f;
			Vector2[] array = (Vector2[])(object)new Vector2[3 * chunks[i].chunkedTriangleIndexes.Count * 2 * 2];
			Vector3[] array2 = (Vector3[])(object)new Vector3[3 * chunks[i].chunkedTriangleIndexes.Count * 2 * 2];
			Vector2 val = default(Vector2);
			for (int j = 0; j < chunks[i].chunkedTriangleIndexes.Count; j++)
			{
				val += vertices[triangles[chunks[i].chunkedTriangleIndexes[j]].vertices.x];
				val += vertices[triangles[chunks[i].chunkedTriangleIndexes[j]].vertices.y];
				val += vertices[triangles[chunks[i].chunkedTriangleIndexes[j]].vertices.z];
			}
			val /= (float)(3 * chunks[i].chunkedTriangleIndexes.Count);
			for (int k = 0; k < chunks[i].chunkedTriangleIndexes.Count; k++)
			{
				array[k * 12] = (array[k * 12 + 3] = float2.op_Implicit(verticesUV[triangles[chunks[i].chunkedTriangleIndexes[k]].vertices.x]));
				array[k * 12 + 1] = (array[k * 12 + 4] = float2.op_Implicit(verticesUV[triangles[chunks[i].chunkedTriangleIndexes[k]].vertices.y]));
				array[k * 12 + 2] = (array[k * 12 + 5] = float2.op_Implicit(verticesUV[triangles[chunks[i].chunkedTriangleIndexes[k]].vertices.z]));
				array2[k * 12] = (array2[k * 12 + 3] = (array2[k * 12 + 6] = (array2[k * 12 + 9] = Vector2.op_Implicit(vertices[triangles[chunks[i].chunkedTriangleIndexes[k]].vertices.x] - val))));
				array2[k * 12 + 1] = (array2[k * 12 + 4] = (array2[k * 12 + 7] = (array2[k * 12 + 10] = Vector2.op_Implicit(vertices[triangles[chunks[i].chunkedTriangleIndexes[k]].vertices.y] - val))));
				array2[k * 12 + 2] = (array2[k * 12 + 5] = (array2[k * 12 + 8] = (array2[k * 12 + 11] = Vector2.op_Implicit(vertices[triangles[chunks[i].chunkedTriangleIndexes[k]].vertices.z] - val))));
				if (i % num == 0)
				{
					ref Vector3 reference = ref array2[k * 12];
					reference += ((Vector3)(ref array2[k * 12])).normalized * _gap;
					ref Vector3 reference2 = ref array2[k * 12 + 1];
					reference2 += ((Vector3)(ref array2[k * 12 + 1])).normalized * _gap;
					ref Vector3 reference3 = ref array2[k * 12 + 2];
					reference3 += ((Vector3)(ref array2[k * 12 + 2])).normalized * _gap;
					ref Vector3 reference4 = ref array2[k * 12 + 3];
					reference4 -= ((Vector3)(ref array2[k * 12 + 3])).normalized * _gap;
					ref Vector3 reference5 = ref array2[k * 12 + 4];
					reference5 -= ((Vector3)(ref array2[k * 12 + 4])).normalized * _gap;
					ref Vector3 reference6 = ref array2[k * 12 + 5];
					reference6 -= ((Vector3)(ref array2[k * 12 + 5])).normalized * _gap;
				}
				array2[k * 12].z = (array2[k * 12 + 1].z = (array2[k * 12 + 2].z = _thickness));
				array2[k * 12 + 3].z = (array2[k * 12 + 4].z = (array2[k * 12 + 5].z = 0f - _thickness));
				array2[k * 12 + 6] = array2[k * 12];
				array2[k * 12 + 7] = array2[k * 12 + 1];
				array2[k * 12 + 8] = array2[k * 12 + 2];
				array2[k * 12 + 9] = array2[k * 12 + 3];
				array2[k * 12 + 10] = array2[k * 12 + 4];
				array2[k * 12 + 11] = array2[k * 12 + 5];
			}
			Mesh mesh = chunkObject[i].GetComponent<MeshFilter>().mesh;
			mesh.vertices = array2;
			mesh.uv = array;
		}
	}

	public void CrackedAll()
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0510: Unknown result type (might be due to invalid IL or missing references)
		//IL_0522: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_0537: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0567: Unknown result type (might be due to invalid IL or missing references)
		//IL_056c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_060f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0614: Unknown result type (might be due to invalid IL or missing references)
		//IL_0629: Unknown result type (might be due to invalid IL or missing references)
		//IL_062e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0643: Unknown result type (might be due to invalid IL or missing references)
		//IL_0648: Unknown result type (might be due to invalid IL or missing references)
		//IL_065e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0663: Unknown result type (might be due to invalid IL or missing references)
		//IL_0679: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0694: Unknown result type (might be due to invalid IL or missing references)
		//IL_0699: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < chunks.Count; i++)
		{
			chunks[i].InitialRotationDisplacement = 0f;
			Vector2[] array = (Vector2[])(object)new Vector2[3 * chunks[i].chunkedTriangleIndexes.Count * 2 * 2];
			Vector3[] array2 = (Vector3[])(object)new Vector3[3 * chunks[i].chunkedTriangleIndexes.Count * 2 * 2];
			Vector2 val = default(Vector2);
			for (int j = 0; j < chunks[i].chunkedTriangleIndexes.Count; j++)
			{
				val += vertices[triangles[chunks[i].chunkedTriangleIndexes[j]].vertices.x];
				val += vertices[triangles[chunks[i].chunkedTriangleIndexes[j]].vertices.y];
				val += vertices[triangles[chunks[i].chunkedTriangleIndexes[j]].vertices.z];
			}
			val /= (float)(3 * chunks[i].chunkedTriangleIndexes.Count);
			for (int k = 0; k < chunks[i].chunkedTriangleIndexes.Count; k++)
			{
				array[k * 12] = (array[k * 12 + 3] = float2.op_Implicit(verticesUV[triangles[chunks[i].chunkedTriangleIndexes[k]].vertices.x]));
				array[k * 12 + 1] = (array[k * 12 + 4] = float2.op_Implicit(verticesUV[triangles[chunks[i].chunkedTriangleIndexes[k]].vertices.y]));
				array[k * 12 + 2] = (array[k * 12 + 5] = float2.op_Implicit(verticesUV[triangles[chunks[i].chunkedTriangleIndexes[k]].vertices.z]));
				array2[k * 12] = (array2[k * 12 + 3] = (array2[k * 12 + 6] = (array2[k * 12 + 9] = Vector2.op_Implicit(vertices[triangles[chunks[i].chunkedTriangleIndexes[k]].vertices.x] - val))));
				array2[k * 12 + 1] = (array2[k * 12 + 4] = (array2[k * 12 + 7] = (array2[k * 12 + 10] = Vector2.op_Implicit(vertices[triangles[chunks[i].chunkedTriangleIndexes[k]].vertices.y] - val))));
				array2[k * 12 + 2] = (array2[k * 12 + 5] = (array2[k * 12 + 8] = (array2[k * 12 + 11] = Vector2.op_Implicit(vertices[triangles[chunks[i].chunkedTriangleIndexes[k]].vertices.z] - val))));
				ref Vector3 reference = ref array2[k * 12];
				reference += ((Vector3)(ref array2[k * 12])).normalized * _gap;
				ref Vector3 reference2 = ref array2[k * 12 + 1];
				reference2 += ((Vector3)(ref array2[k * 12 + 1])).normalized * _gap;
				ref Vector3 reference3 = ref array2[k * 12 + 2];
				reference3 += ((Vector3)(ref array2[k * 12 + 2])).normalized * _gap;
				ref Vector3 reference4 = ref array2[k * 12 + 3];
				reference4 -= ((Vector3)(ref array2[k * 12 + 3])).normalized * _gap;
				ref Vector3 reference5 = ref array2[k * 12 + 4];
				reference5 -= ((Vector3)(ref array2[k * 12 + 4])).normalized * _gap;
				ref Vector3 reference6 = ref array2[k * 12 + 5];
				reference6 -= ((Vector3)(ref array2[k * 12 + 5])).normalized * _gap;
				array2[k * 12].z = (array2[k * 12 + 1].z = (array2[k * 12 + 2].z = _thickness));
				array2[k * 12 + 3].z = (array2[k * 12 + 4].z = (array2[k * 12 + 5].z = 0f - _thickness));
				array2[k * 12 + 6] = array2[k * 12];
				array2[k * 12 + 7] = array2[k * 12 + 1];
				array2[k * 12 + 8] = array2[k * 12 + 2];
				array2[k * 12 + 9] = array2[k * 12 + 3];
				array2[k * 12 + 10] = array2[k * 12 + 4];
				array2[k * 12 + 11] = array2[k * 12 + 5];
			}
			Mesh mesh = chunkObject[i].GetComponent<MeshFilter>().mesh;
			mesh.vertices = array2;
			mesh.uv = array;
		}
	}

	public void RootObjectSetActive()
	{
		rootObject.SetActive(true);
	}

	public void RootObjectSetDeActive()
	{
		rootObject.SetActive(false);
	}

	[ContextMenu("StopRenderTexture")]
	public void StopRenderTexture()
	{
		((Behaviour)_renderTextureCamera).enabled = false;
	}

	[ContextMenu("StartRenderTexture")]
	public void StartRenderTexture()
	{
		((Behaviour)_renderTextureCamera).enabled = true;
	}

	public void StartAnimation()
	{
		GameObject.Find("UI Canvas").SetActive(false);
	}

	public void SetTime(float time)
	{
		_time = time;
	}
}
