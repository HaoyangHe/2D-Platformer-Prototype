using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BashableBridge : MonoBehaviour, BashableObjectOwner
{
    public bool IsTriggered { get; set; }
    public BashableObject bashableObject { get; set; }
    
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    [SerializeField] private int precise = 100;
    [SerializeField] private int segmentLength = 35;
    [SerializeField] private float lineWidth = 0.1f;
    [SerializeField] private float ropeSegLen = 0.25f;
    [SerializeField] private float forceDuration = 0.05f;
    [SerializeField] Vector2 forceGravity = new Vector2(0f, -1f);
    
    private LineRenderer lineRenderer;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    private Vector2 bashImpulse;
    private float bashStartTime;
    private int bashableObjectIndex;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        bashableObject = transform.Find("BashableObject").GetComponent<BashableObject>();
        
        bashableObjectIndex = segmentLength / 2;

        Vector3 ropeStartPoint = startPoint.position;
        for (int i = 0; i < segmentLength; i++)
        {
            ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= ropeSegLen;
        }

        RopeSegment bashableSegment = ropeSegments[bashableObjectIndex];
        bashableObject.Transform.position = bashableSegment.posNow;
        IsTriggered = false;
    }

    void Update()
    {
        DrawRope();
    }

    private void FixedUpdate()
    {
        CheckIfAddForce();
        Simulate();
    }

    private void Simulate()
    {
        // SIMULATION
        for (int i = 1; i < this.segmentLength; i++)
        {
            RopeSegment firstSegment = ropeSegments[i];
            Vector2 velocity = Vector2.zero;
            if (i == bashableObjectIndex && IsTriggered)
            {
                velocity = bashImpulse / bashableObject.Mass;
            }
            else
            {
                velocity = firstSegment.posNow - firstSegment.posOld;
            }
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow += forceGravity * Time.fixedDeltaTime;
            this.ropeSegments[i] = firstSegment;
        }

        // CONSTRAINTS
        for (int i = 0; i < precise; i++)
        {
            ApplyConstraint();
            MoveBashAbleObject();
        }
    }

    private void ApplyConstraint()
    {
        //Constrant to First Point 
        RopeSegment firstSegment = this.ropeSegments[0];
        firstSegment.posNow = startPoint.position;
        this.ropeSegments[0] = firstSegment;


        //Constrant to Second Point 
        RopeSegment endSegment = this.ropeSegments[this.ropeSegments.Count - 1];
        endSegment.posNow = endPoint.position;
        this.ropeSegments[this.ropeSegments.Count - 1] = endSegment;

        for (int i = 0; i < this.segmentLength - 1; i++)
        {
            RopeSegment firstSeg = this.ropeSegments[i];
            RopeSegment secondSeg = this.ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - this.ropeSegLen);
            Vector2 changeDir = Vector2.zero;

            if (dist > ropeSegLen)
            {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            }
            else if (dist < ropeSegLen)
            {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }

            Vector2 changeAmount = changeDir * error;
            if (i != 0)
            {
                firstSeg.posNow -= changeAmount * 0.5f;
                this.ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * 0.5f;
                this.ropeSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += changeAmount;
                this.ropeSegments[i + 1] = secondSeg;
            }
        }
    }

    private void MoveBashAbleObject()
    {
        RopeSegment bashableSegment = ropeSegments[bashableObjectIndex];
        bashableObject.Transform.position = bashableSegment.posNow;
    }

    public void OnTriggered(Vector2 impulse)
    {
        IsTriggered = true;
        bashStartTime = Time.time;
        bashImpulse = impulse;
    }

    public void CheckIfAddForce()
    {
        if (IsTriggered && Time.time >= bashStartTime + forceDuration)
        {
            IsTriggered = false;
        }
    }

    private void DrawRope()
    {
        float lineWidth = this.lineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[this.segmentLength];
        for (int i = 0; i < this.segmentLength; i++)
        {
            ropePositions[i] = this.ropeSegments[i].posNow;
        }

        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);
    }

    public struct RopeSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;

        public RopeSegment(Vector2 pos)
        {
            this.posNow = pos;
            this.posOld = pos;
        }
    }
}
