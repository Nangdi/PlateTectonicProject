using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuidePanel : MonoBehaviour
{
    [SerializeField]
    private LeapMouseCursor playerCursor;
    [SerializeField]
    private TextMeshProUGUI text;

    public void UpdateGuideText()
    {
        switch (playerCursor.actionState)
        {
            case ActionState.Idle:
                text.text = "손을 움직여 버튼을 선택해보세요";

                break;
            case ActionState.playback:
                text.text = "판의 움직임으로 형성된 지형과 현상을 알아보세요";
                break;
            case ActionState.Select:
                text.text = "시뮬레이션을 실행해보세요";
                break;
            case ActionState.Off:
                text.text = "에어제스처에 손을 넣어 체험해보세요";
                break;
        }
    }
}