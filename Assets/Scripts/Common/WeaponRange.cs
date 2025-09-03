using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
// �� Ŀ���� �����Ͱ� WeaponBase Ŭ������ ������� �۵��ϵ��� �����մϴ�.
[CustomEditor(typeof(WeaponBase))]
public class WeaponBoxVisualizer : Editor
{
    // ��(Scene) �信 GUI ��Ҹ� �׸� �� ȣ��Ǵ� �޼����Դϴ�.
    private void OnSceneGUI()
    {
        // ���� �ν����Ϳ��� ���õ� WeaponBase ������Ʈ�� �����ɴϴ�.
        WeaponBase weaponBase = (WeaponBase)target;

        // ItemData�� �Ҵ���� �ʾҴٸ� ���� ������ ���� ������ �ߴ��մϴ�.
        if (weaponBase.itemData == null)
        {
            return;
        }

        // --- SingleAttack �޼����� ������ ������ �󿡼� �����մϴ� ---

        // 1. ���� ����(attackDir) ���
        // �� ���� ���콺 ��ġ�� �������� Ray�� �����մϴ�.
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        Vector3 attackDir = Vector3.zero;

        // ���콺 ��ġ�� �ݶ��̴��� �ִ��� Ȯ���մϴ�.
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue))
        {
            Vector3 targetPoint = hit.point;
            // ���� ��ġ���� ���콺�� ���� ������ �ٶ󺸴� ������ ����մϴ�.
            attackDir = (targetPoint - weaponBase.transform.position).normalized;
            attackDir.y = 0f; // Y�� ����
        }
        else
        {
            // ���콺�� ����� ����Ű��, ������ ������ �⺻ �������� ����մϴ�.
            attackDir = weaponBase.transform.forward;
        }

        // ���� ���Ͱ� 0�� �ƴ� ��쿡�� ������ �׸��ϴ� (���� ����).
        if (attackDir == Vector3.zero) return;

        // 2. OverlapBox�� �Ķ���� ��������
        Transform weaponTransform = weaponBase.transform;
        float attackRange = weaponBase.itemData.AttackRange;
        Vector3 boxSize = new Vector3(1f, 1f, attackRange); // �ڵ忡 ���ǵ� �ڽ� ũ��
        Vector3 boxCenter = weaponTransform.position + attackDir * (attackRange / 2);
        Quaternion boxRotation = Quaternion.LookRotation(attackDir);

        // 3. �ڵ�(Handles)�� ����Ͽ� �ڽ� �׸���
        Handles.color = Color.cyan; // �ð�ȭ ������ û�ϻ����� ����

        // Matrix4x4�� ����Ͽ� ��ġ, ȸ��, ũ�⸦ �� ���� �����մϴ�.
        // �̷��� �ϸ� ������ ��� ���� ���� ��ǥ�迡�� �ڽ��� ���� �׸� �� �ֽ��ϴ�.
        Matrix4x4 originalMatrix = Handles.matrix; // ���� ��Ʈ���� ���� ����
        Handles.matrix = Matrix4x4.TRS(boxCenter, boxRotation, Vector3.one);

        // ���� ��ǥ���� (0,0,0)�� �߽����� �ڽ��� �׸��ϴ�.
        // ���� ���� ��ġ�� Handles.matrix�� ���� �����˴ϴ�.
        Handles.DrawWireCube(Vector3.zero, boxSize);

        Handles.matrix = originalMatrix; // �ڵ� ��Ʈ������ ������� ����

        // ���콺 �����ӿ� ���� �� �並 ����ؼ� �ٽ� �׸����� ��û�մϴ�.
        // �̷��� �ؾ� ���콺�� ���� ���� ǥ�ð� �ǽð����� ������Ʈ�˴ϴ�.
        if (Event.current.type == EventType.MouseMove)
        {
            SceneView.RepaintAll();
        }
    }
}
#endif