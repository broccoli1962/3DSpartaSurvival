using UnityEditor; // Editor 관련 기능을 사용하기 위해 필수
using UnityEngine;

// 이 에디터 스크립트가 어떤 컴포넌트를 대상으로 할지 지정합니다.
// 우리는 WeaponController를 꾸며줄 것입니다.
[CustomEditor(typeof(WeaponBase))]
public class WeaponRangeVisualizer : Editor
{
    // 씬(Scene) 뷰에 GUI 요소를 그릴 때 호출되는 메서드입니다.
    private void OnSceneGUI()
    {
        // 현재 인스펙터에서 보고 있는 WeaponController 컴포넌트를 가져옵니다.
        // 'target'은 Editor 클래스가 기본으로 제공하는 변수입니다.
        WeaponBase weaponController = (WeaponBase)target;

        // WeaponController가 무기 데이터를 가지고 있지 않다면 아무것도 하지 않고 종료합니다.
        if (weaponController.itemData == null)
        {
            return;
        }

        // 시각화할 정보 가져오기
        Transform weaponTransform = weaponController.transform; // 무기의 위치, 회전, 크기 정보
        float attackRange = weaponController.itemData.AttackRange; // ItemData에 정의된 공격 범위

        // 원을 그릴 색상 설정 (빨간색)
        Handles.color = Color.red;

        // 와이어프레임 형태의 원을 그립니다.
        // Handles.DrawWireDisc(중심점, 원이 그려질 평면의 법선 벡터, 반지름);
        // 2D 게임(XY 평면)에서는 법선 벡터로 Vector3.forward를 사용합니다. (화면을 뚫고 나오는 방향)
        Handles.DrawWireDisc(weaponTransform.position, Vector3.down, attackRange);

        /*
        // 만약 원 내부를 채워서 표시하고 싶다면 아래 코드를 사용하세요.
        // 이 경우, 반투명한 색상을 사용하는 것이 좋습니다.
        Handles.color = new Color(1f, 0f, 0f, 0.15f); // 15% 불투명도의 빨간색
        Handles.DrawSolidDisc(weaponTransform.position, Vector3.forward, attackRange);
        */
    }
}