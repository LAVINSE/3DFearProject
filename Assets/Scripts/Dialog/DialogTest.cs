using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogTest : MonoBehaviour
{
    #region 변수
    [SerializeField] private DialogSystem dialogSystem01;
    [SerializeField] private TextMeshProUGUI textCountdown;
    [SerializeField] private DialogSystem dialogSystem02;
    #endregion // 변수

    #region 함수
    private IEnumerator Start()
    {
        textCountdown.gameObject.SetActive(false);

        // 첫 번째 대사 분기 시작
        yield return new WaitUntil(() => dialogSystem01.UpdateDialog());

        // 대사 분기 사이에 원하는 행동을 추가할 수 있다.
        // 캐릭터를 움직이거나 아이템을 획득하는 등의.. 현재는 5-4-3-2-1 카운트 다운 실행
        textCountdown.gameObject.SetActive(true);

        int count = 5;

        while(count > 0)
        {
            textCountdown.text = count.ToString();
            count--;

            yield return new WaitForSeconds(1f);
        }

        textCountdown.gameObject.SetActive(false);

        // 두 번째 대사 분기 시작
        yield return new WaitUntil(() => dialogSystem02.UpdateDialog());

        textCountdown.gameObject.SetActive(true);
        textCountdown.text = "The End";

        yield return new WaitForSeconds(2f);


        Debug.Log(" 대화 종료 ");
    }
    #endregion // 함수

    /*
     * using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogTest : MonoBehaviour
{
    [SerializeField] private List<DialogSystem> dialogSystems; // 대화 시스템을 관리하는 List
    [SerializeField] private TextMeshProUGUI textCountdown; // 카운트 다운을 표시하는 Text UI

    private IEnumerator Start()
    {
        textCountdown.gameObject.SetActive(false); // 시작 시 카운트 다운 UI를 비활성화

        // 대화 시스템들을 병렬로 업데이트
        yield return StartCoroutine(UpdateDialogs());

        // 대화 완료 후 추가 행동 수행
        yield return StartCoroutine(AdditionalActions());
    }

    private IEnumerator UpdateDialogs()
    {
        // 모든 대화 시스템이 완료될 때까지 업데이트
        while (!AllDialogsCompleted())
        {
            // 각 대화 시스템을 순회하면서 업데이트
            foreach (var dialogSystem in dialogSystems)
            {
                // 대화 시스템이 완료되면 다음 대화 시스템으로 넘어감
                if (dialogSystem.UpdateDialog())
                    yield return null; // 대화가 완료될 때까지 기다림
            }
        }
    }

    private bool AllDialogsCompleted()
    {
        // 모든 대화 시스템이 완료되었는지 확인
        foreach (var dialogSystem in dialogSystems)
        {
            // 대화 시스템의 업데이트를 체크하여 완료 여부를 확인
            if (!dialogSystem.UpdateDialog())
                return false;
        }
        return true; // 모든 대화 시스템이 완료됨
    }

    private IEnumerator AdditionalActions()
    {
        // 원하는 추가 행동 수행 가능
        textCountdown.gameObject.SetActive(true); // 카운트 다운 UI 활성화

        int count = 5;

        // 카운트 다운 진행
        while (count > 0)
        {
            textCountdown.text = count.ToString();
            count--;

            yield return new WaitForSeconds(1f);
        }

        textCountdown.gameObject.SetActive(false); // 카운트 다운 UI 비활성화
        Debug.Log(" 대화 종료 "); // 대화 종료 로그 출력
    }
}

     */
}
