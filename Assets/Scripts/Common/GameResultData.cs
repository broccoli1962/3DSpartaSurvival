
// 게임의 결과 상태를 나타냅니다.
public enum EGameResultType { Victory, Defeat }

/// <summary>
/// 게임 결과 정보를 UI에 전달하기 위한 데이터 클래스
/// </summary>
public class GameResultData
{
    public EGameResultType ResultType; // 승리 또는 패배
    public float Playtime;             // 총 플레이 시간
}