using UnityEditor; // Editor ���� ����� ����ϱ� ���� �ʼ�
using UnityEngine;

// �� ������ ��ũ��Ʈ�� � ������Ʈ�� ������� ���� �����մϴ�.
// �츮�� WeaponController�� �ٸ��� ���Դϴ�.
[CustomEditor(typeof(WeaponBase))]
public class WeaponRangeVisualizer : Editor
{
    // ��(Scene) �信 GUI ��Ҹ� �׸� �� ȣ��Ǵ� �޼����Դϴ�.
    private void OnSceneGUI()
    {
        // ���� �ν����Ϳ��� ���� �ִ� WeaponController ������Ʈ�� �����ɴϴ�.
        // 'target'�� Editor Ŭ������ �⺻���� �����ϴ� �����Դϴ�.
        WeaponBase weaponController = (WeaponBase)target;

        // WeaponController�� ���� �����͸� ������ ���� �ʴٸ� �ƹ��͵� ���� �ʰ� �����մϴ�.
        if (weaponController.itemData == null)
        {
            return;
        }

        // �ð�ȭ�� ���� ��������
        Transform weaponTransform = weaponController.transform; // ������ ��ġ, ȸ��, ũ�� ����
        float attackRange = weaponController.itemData.AttackRange; // ItemData�� ���ǵ� ���� ����

        // ���� �׸� ���� ���� (������)
        Handles.color = Color.red;

        // ���̾������� ������ ���� �׸��ϴ�.
        // Handles.DrawWireDisc(�߽���, ���� �׷��� ����� ���� ����, ������);
        // 2D ����(XY ���)������ ���� ���ͷ� Vector3.forward�� ����մϴ�. (ȭ���� �հ� ������ ����)
        Handles.DrawWireDisc(weaponTransform.position, Vector3.down, attackRange);

        /*
        // ���� �� ���θ� ä���� ǥ���ϰ� �ʹٸ� �Ʒ� �ڵ带 ����ϼ���.
        // �� ���, �������� ������ ����ϴ� ���� �����ϴ�.
        Handles.color = new Color(1f, 0f, 0f, 0.15f); // 15% �������� ������
        Handles.DrawSolidDisc(weaponTransform.position, Vector3.forward, attackRange);
        */
    }
}