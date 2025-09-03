using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
// 이 커스텀 에디터가 WeaponBase 클래스를 대상으로 작동하도록 지정합니다.
[CustomEditor(typeof(WeaponBase))]
public class WeaponBoxVisualizer : Editor
{
    // 씬(Scene) 뷰에 GUI 요소를 그릴 때 호출되는 메서드입니다.
    private void OnSceneGUI()
    {
        // 현재 인스펙터에서 선택된 WeaponBase 컴포넌트를 가져옵니다.
        WeaponBase weaponBase = (WeaponBase)target;

        // ItemData가 할당되지 않았다면 오류 방지를 위해 실행을 중단합니다.
        if (weaponBase.itemData == null)
        {
            return;
        }

        // --- SingleAttack 메서드의 로직을 에디터 상에서 재현합니다 ---

        // 1. 공격 방향(attackDir) 계산
        // 씬 뷰의 마우스 위치를 기준으로 Ray를 생성합니다.
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        Vector3 attackDir = Vector3.zero;

        // 마우스 위치에 콜라이더가 있는지 확인합니다.
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue))
        {
            Vector3 targetPoint = hit.point;
            // 무기 위치에서 마우스가 찍은 지점을 바라보는 방향을 계산합니다.
            attackDir = (targetPoint - weaponBase.transform.position).normalized;
            attackDir.y = 0f; // Y축 고정
        }
        else
        {
            // 마우스가 허공을 가리키면, 무기의 전방을 기본 방향으로 사용합니다.
            attackDir = weaponBase.transform.forward;
        }

        // 방향 벡터가 0이 아닌 경우에만 범위를 그립니다 (오류 방지).
        if (attackDir == Vector3.zero) return;

        // 2. OverlapBox의 파라미터 가져오기
        Transform weaponTransform = weaponBase.transform;
        float attackRange = weaponBase.itemData.AttackRange;
        Vector3 boxSize = new Vector3(1f, 1f, attackRange); // 코드에 정의된 박스 크기
        Vector3 boxCenter = weaponTransform.position + attackDir * (attackRange / 2);
        Quaternion boxRotation = Quaternion.LookRotation(attackDir);

        // 3. 핸들(Handles)을 사용하여 박스 그리기
        Handles.color = Color.cyan; // 시각화 색상을 청록색으로 설정

        // Matrix4x4를 사용하여 위치, 회전, 크기를 한 번에 적용합니다.
        // 이렇게 하면 복잡한 계산 없이 로컬 좌표계에서 박스를 쉽게 그릴 수 있습니다.
        Matrix4x4 originalMatrix = Handles.matrix; // 기존 매트릭스 상태 저장
        Handles.matrix = Matrix4x4.TRS(boxCenter, boxRotation, Vector3.one);

        // 로컬 좌표계의 (0,0,0)을 중심으로 박스를 그립니다.
        // 실제 월드 위치는 Handles.matrix에 의해 결정됩니다.
        Handles.DrawWireCube(Vector3.zero, boxSize);

        Handles.matrix = originalMatrix; // 핸들 매트릭스를 원래대로 복원

        // 마우스 움직임에 따라 씬 뷰를 계속해서 다시 그리도록 요청합니다.
        // 이렇게 해야 마우스를 따라 범위 표시가 실시간으로 업데이트됩니다.
        if (Event.current.type == EventType.MouseMove)
        {
            SceneView.RepaintAll();
        }
    }
}
#endif