using UnityEngine;

//[CreateAssetMenu(fileName = "New Muzzle Group", menuName = "Scriptable Object/Muzzle Group")]
public class MuzzleSO : MonoBehaviour
{
    [SerializeField] private MuzzleType muzzleType;

    [SerializeField] private bool needDrawUpMuzzles;
    [SerializeField] private Vector3[] upMuzzleStartingPoints;
    [SerializeField] private Vector3[] upProjectileDirection = new Vector3[] { Vector3.up };
    [SerializeField] private bool needDrawDownMuzzles;
    [SerializeField] private Vector3[] downMuzzleStartingPoints;
    [SerializeField] private Vector3[] downProjectileDirection = new Vector3[] { Vector3.down };
    [SerializeField] private bool needDrawLeftMuzzles;
    [SerializeField] private Vector3[] leftMuzzleStartingPoints;
    [SerializeField] private Vector3[] leftProjectileDirection = new Vector3[] { Vector3.left };
    [SerializeField] private bool needDrawRightMuzzles;
    [SerializeField] private Vector3[] rightMuzzleStartingPoints;
    [SerializeField] private Vector3[] rightProjectileDirection = new Vector3[] { Vector3.right };

    private int currentMuzzleNum;

    public Vector3[] GetNextMuzzles(Vector2 direction)
    {
        currentMuzzleNum = 1 - currentMuzzleNum;
        switch (muzzleType)
        {
            case MuzzleType.Serial:
                if (direction == Vector2.up)
                    return new Vector3[] { upMuzzleStartingPoints[currentMuzzleNum] };
                else if (direction == Vector2.down)
                    return new Vector3[] { downMuzzleStartingPoints[currentMuzzleNum] };
                else if (direction == Vector2.left)
                    return new Vector3[] { leftMuzzleStartingPoints[currentMuzzleNum] };
                else if (direction == Vector2.right)
                    return new Vector3[] { rightMuzzleStartingPoints[currentMuzzleNum] };
                else
                {
                    Debug.LogError($"Fatal direction input: {direction}.");
                    return new Vector3[] { downMuzzleStartingPoints[currentMuzzleNum] };
                }
            case MuzzleType.Paralell:
                if (direction == Vector2.up)
                    return upMuzzleStartingPoints;
                else if (direction == Vector2.down)
                    return downMuzzleStartingPoints;
                else if (direction == Vector2.left)
                    return leftMuzzleStartingPoints;
                else if (direction == Vector2.right)
                    return rightMuzzleStartingPoints;
                else
                {
                    Debug.LogError($"Fatal direction input: {direction}.");
                    return downMuzzleStartingPoints;
                }
            case MuzzleType.Laser:
                break;
            default:
                break;
        }
        return null;
    }

    private void OnValidate()
    {

    }

    private void OnDrawGizmosSelected()
    {
        if (upMuzzleStartingPoints == null || downMuzzleStartingPoints == null || leftMuzzleStartingPoints == null || rightMuzzleStartingPoints == null) return;

        var defaultColor = Gizmos.color;
        if (muzzleType == MuzzleType.Paralell)
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < Mathf.Max(upMuzzleStartingPoints.Length, downMuzzleStartingPoints.Length, leftMuzzleStartingPoints.Length, rightMuzzleStartingPoints.Length); i++)
            {
                if (upMuzzleStartingPoints.Length > i && needDrawUpMuzzles)
                {
                    Gizmos.DrawSphere(upMuzzleStartingPoints[i].ToWorldPosition(transform.parent), 0.1f);
                    Gizmos.DrawLine(upMuzzleStartingPoints[i].ToWorldPosition(transform.parent),
                                    (upMuzzleStartingPoints[i] + upProjectileDirection[i]).ToWorldPosition(transform.parent));
                }
                if (downMuzzleStartingPoints.Length > i && needDrawDownMuzzles)
                {
                    Gizmos.DrawSphere(downMuzzleStartingPoints[i].ToWorldPosition(transform.parent), 0.1f);
                    Gizmos.DrawLine(downMuzzleStartingPoints[i].ToWorldPosition(transform.parent),
                                    (downMuzzleStartingPoints[i] + downProjectileDirection[i]).ToWorldPosition(transform.parent));
                }
                if (leftMuzzleStartingPoints.Length > i && needDrawLeftMuzzles)
                {
                    Gizmos.DrawSphere(leftMuzzleStartingPoints[i].ToWorldPosition(transform.parent), 0.1f);
                    Gizmos.DrawLine(leftMuzzleStartingPoints[i].ToWorldPosition(transform.parent),
                                    (leftMuzzleStartingPoints[i] + leftProjectileDirection[i]).ToWorldPosition(transform.parent));
                }
                if (rightMuzzleStartingPoints.Length > i && needDrawRightMuzzles)
                {
                    Gizmos.DrawSphere(rightMuzzleStartingPoints[i].ToWorldPosition(transform.parent), 0.1f);
                    Gizmos.DrawLine(rightMuzzleStartingPoints[i].ToWorldPosition(transform.parent),
                                    (rightMuzzleStartingPoints[i] + rightProjectileDirection[i]).ToWorldPosition(transform.parent));
                }
            }
        }
        else if (muzzleType == MuzzleType.Serial)
        {
            Gizmos.color = Color.green;
            var size = new Vector3(0.1f, 0.1f);
            for (int i = 0; i < Mathf.Max(upMuzzleStartingPoints.Length, downMuzzleStartingPoints.Length, leftMuzzleStartingPoints.Length, rightMuzzleStartingPoints.Length); i++)
            {
                if (upMuzzleStartingPoints.Length > i && needDrawUpMuzzles)
                {
                    Gizmos.DrawCube(upMuzzleStartingPoints[i].ToWorldPosition(transform.parent), size);
                    Gizmos.DrawLine(upMuzzleStartingPoints[i].ToWorldPosition(transform.parent),
                                    (upMuzzleStartingPoints[i] + upProjectileDirection[i]).ToWorldPosition(transform.parent));
                }
                if (downMuzzleStartingPoints.Length > i && needDrawDownMuzzles)
                {
                    Gizmos.DrawCube(downMuzzleStartingPoints[i].ToWorldPosition(transform.parent), size);
                    Gizmos.DrawLine(downMuzzleStartingPoints[i].ToWorldPosition(transform.parent),
                                    (downMuzzleStartingPoints[i] + downProjectileDirection[i]).ToWorldPosition(transform.parent));
                }
                if (leftMuzzleStartingPoints.Length > i && needDrawLeftMuzzles)
                {
                    Gizmos.DrawCube(leftMuzzleStartingPoints[i].ToWorldPosition(transform.parent), size);
                    Gizmos.DrawLine(leftMuzzleStartingPoints[i].ToWorldPosition(transform.parent),
                                    (leftMuzzleStartingPoints[i] + leftProjectileDirection[i]).ToWorldPosition(transform.parent));
                }
                if (rightMuzzleStartingPoints.Length > i && needDrawRightMuzzles)
                {
                    Gizmos.DrawCube(rightMuzzleStartingPoints[i].ToWorldPosition(transform.parent), size);
                    Gizmos.DrawLine(rightMuzzleStartingPoints[i].ToWorldPosition(transform.parent),
                                    (rightMuzzleStartingPoints[i] + rightProjectileDirection[i]).ToWorldPosition(transform.parent));
                }
            }
        }
        else if (muzzleType == MuzzleType.Laser)
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < Mathf.Max(upMuzzleStartingPoints.Length, downMuzzleStartingPoints.Length, leftMuzzleStartingPoints.Length, rightMuzzleStartingPoints.Length); i++)
            {
                if (upMuzzleStartingPoints.Length > i && needDrawUpMuzzles)
                {
                    Gizmos.DrawRay(upMuzzleStartingPoints[i].ToWorldPosition(transform.parent), Vector3.up);
                    Gizmos.DrawLine(upMuzzleStartingPoints[i].ToWorldPosition(transform.parent),
                                    (upMuzzleStartingPoints[i] + upProjectileDirection[i]).ToWorldPosition(transform.parent));
                }
                if (downMuzzleStartingPoints.Length > i && needDrawDownMuzzles)
                    Gizmos.DrawRay(downMuzzleStartingPoints[i].ToWorldPosition(transform.parent), Vector3.down);
                if (leftMuzzleStartingPoints.Length > i && needDrawLeftMuzzles)
                    Gizmos.DrawRay(leftMuzzleStartingPoints[i].ToWorldPosition(transform.parent), Vector3.left);
                if (rightMuzzleStartingPoints.Length > i && needDrawRightMuzzles)
                    Gizmos.DrawRay(rightMuzzleStartingPoints[i].ToWorldPosition(transform.parent), Vector3.right);
            }
        }

        Gizmos.color = defaultColor;
    }

    enum MuzzleType
    {
        Serial,
        Paralell,
        Laser,
    }
}
