using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogTest : MonoBehaviour
{
    #region ����
    [SerializeField] private DialogSystem dialogSystem01;
    [SerializeField] private TextMeshProUGUI textCountdown;
    [SerializeField] private DialogSystem dialogSystem02;
    #endregion // ����

    #region �Լ�
    private IEnumerator Start()
    {
        textCountdown.gameObject.SetActive(false);

        // ù ��° ��� �б� ����
        yield return new WaitUntil(() => dialogSystem01.UpdateDialog());

        // ��� �б� ���̿� ���ϴ� �ൿ�� �߰��� �� �ִ�.
        // ĳ���͸� �����̰ų� �������� ȹ���ϴ� ����.. ����� 5-4-3-2-1 ī��Ʈ �ٿ� ����
        textCountdown.gameObject.SetActive(true);

        int count = 5;

        while(count > 0)
        {
            textCountdown.text = count.ToString();
            count--;

            yield return new WaitForSeconds(1f);
        }

        textCountdown.gameObject.SetActive(false);

        // �� ��° ��� �б� ����
        yield return new WaitUntil(() => dialogSystem02.UpdateDialog());

        textCountdown.gameObject.SetActive(true);
        textCountdown.text = "The End";

        yield return new WaitForSeconds(2f);


        Debug.Log(" ��ȭ ���� ");
    }
    #endregion // �Լ�

    /*
     * using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogTest : MonoBehaviour
{
    [SerializeField] private List<DialogSystem> dialogSystems; // ��ȭ �ý����� �����ϴ� List
    [SerializeField] private TextMeshProUGUI textCountdown; // ī��Ʈ �ٿ��� ǥ���ϴ� Text UI

    private IEnumerator Start()
    {
        textCountdown.gameObject.SetActive(false); // ���� �� ī��Ʈ �ٿ� UI�� ��Ȱ��ȭ

        // ��ȭ �ý��۵��� ���ķ� ������Ʈ
        yield return StartCoroutine(UpdateDialogs());

        // ��ȭ �Ϸ� �� �߰� �ൿ ����
        yield return StartCoroutine(AdditionalActions());
    }

    private IEnumerator UpdateDialogs()
    {
        // ��� ��ȭ �ý����� �Ϸ�� ������ ������Ʈ
        while (!AllDialogsCompleted())
        {
            // �� ��ȭ �ý����� ��ȸ�ϸ鼭 ������Ʈ
            foreach (var dialogSystem in dialogSystems)
            {
                // ��ȭ �ý����� �Ϸ�Ǹ� ���� ��ȭ �ý������� �Ѿ
                if (dialogSystem.UpdateDialog())
                    yield return null; // ��ȭ�� �Ϸ�� ������ ��ٸ�
            }
        }
    }

    private bool AllDialogsCompleted()
    {
        // ��� ��ȭ �ý����� �Ϸ�Ǿ����� Ȯ��
        foreach (var dialogSystem in dialogSystems)
        {
            // ��ȭ �ý����� ������Ʈ�� üũ�Ͽ� �Ϸ� ���θ� Ȯ��
            if (!dialogSystem.UpdateDialog())
                return false;
        }
        return true; // ��� ��ȭ �ý����� �Ϸ��
    }

    private IEnumerator AdditionalActions()
    {
        // ���ϴ� �߰� �ൿ ���� ����
        textCountdown.gameObject.SetActive(true); // ī��Ʈ �ٿ� UI Ȱ��ȭ

        int count = 5;

        // ī��Ʈ �ٿ� ����
        while (count > 0)
        {
            textCountdown.text = count.ToString();
            count--;

            yield return new WaitForSeconds(1f);
        }

        textCountdown.gameObject.SetActive(false); // ī��Ʈ �ٿ� UI ��Ȱ��ȭ
        Debug.Log(" ��ȭ ���� "); // ��ȭ ���� �α� ���
    }
}

     */
}
