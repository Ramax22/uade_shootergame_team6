using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> neightbourds;
    public bool isTrap;
    private void Start()
    {
        //GetNeightbourd(Vector3.right);
        //GetNeightbourd(Vector3.left);
        //GetNeightbourd(Vector3.forward);
        //GetNeightbourd(Vector3.back);
    }
    void GetNeightbourd(Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, 2.2f))
        {
            neightbourds.Add(hit.collider.GetComponent<Node>());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (var item in neightbourds)
        {
            Vector3 line_finish = item.transform.position;
            line_finish.y += 2;
            Gizmos.DrawLine(transform.position, line_finish);
            Vector3 direction = (line_finish - transform.position).normalized;
            ForGizmo(line_finish, direction, Color.blue, false, 1f);

            /*Vector3 direction = (item.transform.position - transform.position).normalized;
            float arrowHeadAngle = 50f;
            float arrowHeadLength = 0.5f;
            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(item.transform.position + direction, right * arrowHeadLength);
            Gizmos.DrawRay(item.transform.position + direction, left * arrowHeadLength);*/
        }
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 1);
    }

    public void ForGizmo(Vector3 pos, Vector3 direction, Color? color = null, bool doubled = false, float arrowHeadLength = 0.2f, float arrowHeadAngle = 20.0f)
    {
        Gizmos.color = color ?? Color.white;

        //arrow shaft
        Gizmos.DrawRay(pos, direction);

        if (direction != Vector3.zero)
        {
            //arrow head
            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
            Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
        }
    }
}