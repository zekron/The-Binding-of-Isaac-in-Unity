using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PhysicalObject : MonoBehaviour
{
    private const float MIN_MOVE_DISTANCE = 0.001f;

    private new Collider2D collider2D;
    private new Rigidbody2D rigidbody2D;
    private ContactFilter2D contactFilter2D;
    private readonly List<RaycastHit2D> raycastHit2DList = new List<RaycastHit2D>();
    private readonly List<RaycastHit2D> tangentRaycastHit2DList = new List<RaycastHit2D>();

    public LayerMask layerMask;
    [HideInInspector]
    public Vector2 velocity;


    void Start()
    {
        collider2D = GetComponent<Collider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        if (rigidbody2D == null)
            rigidbody2D = gameObject.AddComponent<Rigidbody2D>();

        rigidbody2D.hideFlags = HideFlags.NotEditable;
        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        rigidbody2D.simulated = true;
        rigidbody2D.useFullKinematicContacts = false;
        rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rigidbody2D.sleepMode = RigidbodySleepMode2D.NeverSleep;
        rigidbody2D.interpolation = RigidbodyInterpolation2D.Interpolate;
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        rigidbody2D.gravityScale = 0;

        contactFilter2D = new ContactFilter2D
        {
            useLayerMask = true,
            useTriggers = false,
            layerMask = layerMask
        };
    }

    private void OnValidate()
    {
        contactFilter2D.layerMask = layerMask;
    }

    private void Update()
    {
        velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void FixedUpdate()
    {
        Movement(velocity * Time.deltaTime * 5f);
    }

    private void Movement(Vector2 deltaPosition)
    {
        if (deltaPosition == Vector2.zero)
            return;

        Vector2 updateDeltaPosition = Vector2.zero;

        float distance = deltaPosition.magnitude;       //�����ƶ�����ģ��
        Vector2 direction = deltaPosition.normalized;   //�����ƶ�������һ��

        distance = Mathf.Max(distance, MIN_MOVE_DISTANCE);

        rigidbody2D.Cast(direction, contactFilter2D, raycastHit2DList, distance);

        Vector2 finalDirection = direction;             //����������ƶ�����
        float finalDistance = distance;                 //����������ƶ������С

        foreach (var hit in raycastHit2DList)
        {
            float moveDistance = hit.distance;

            Debug.DrawLine(hit.point, hit.point + hit.normal, Color.white); //��ײ�㷨������
            Debug.DrawLine(hit.point, hit.point + direction, Color.yellow); //�ƶ���������

            float projection = Vector2.Dot(hit.normal, direction);

            if (projection >= 0)    //�н�С�ڵ���90��
            {
                moveDistance = distance;    //������ײ������
            }
            else
            {
                //Vector2 tangentDirection = new Vector2(hit.normal.y, -hit.normal.x);

                //float tangentDot = Vector2.Dot(tangentDirection, direction);

                //if (tangentDot < 0)
                //{
                //    tangentDirection = -tangentDirection;
                //    tangentDot = -tangentDot;
                //}

                Vector2 tangentDirection = direction;
                float tangentDot = 0;
                if (hit.normal.x != 0)
                {
                    tangentDirection.x = 0;
                    if (direction.y == 0) tangentDirection.y = 0;
                    else tangentDirection.y = direction.y > 0 ? 1 : -1;
                    tangentDot =direction.y;
                }
                if (hit.normal.y != 0)
                {
                    tangentDirection.y = 0;
                    if (direction.x == 0) tangentDirection.x = 0;
                    else tangentDirection.x = direction.x > 0 ? 1 : -1;
                    tangentDot =direction.x;
                }

                float tangentDistance = Mathf.Abs(tangentDot * distance);
                if (tangentDot != 0)
                {
                    rigidbody2D.Cast(tangentDirection, contactFilter2D, tangentRaycastHit2DList, tangentDistance);

                    foreach (var tangentHit in tangentRaycastHit2DList)
                    {
                        Debug.DrawLine(tangentHit.point, tangentHit.point + tangentDirection, Color.magenta);

                        if (Vector2.Dot(tangentHit.normal, tangentDirection) >= 0)
                            continue;

                        tangentDistance = Mathf.Min(tangentDistance, tangentHit.distance);
                    }

                    updateDeltaPosition += tangentDirection * tangentDistance;
                }
            }

            finalDistance = Mathf.Min(finalDistance, moveDistance);
        }

        updateDeltaPosition += finalDirection * finalDistance;
        rigidbody2D.position += updateDeltaPosition;
    }
}
