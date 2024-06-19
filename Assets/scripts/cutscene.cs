using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cutscene : MonoBehaviour
{
    public Text year;
    public Text comment;
    public Text comment2;
    Vector2 yearPos = new Vector2(-229.6f, -13.4f);
    Vector2 yearSc = new Vector2(0.16f, 0.16f);

    public Image mm;
    public GameObject mOutline;
    public GameObject logo;
    void Start()
    {
        StartCoroutine(motion());
    }

    IEnumerator motion() {
        year.gameObject.SetActive(false);
        comment.gameObject.SetActive(false);
        comment2.gameObject.SetActive(false);
        mOutline.gameObject.SetActive(false);
        mm.gameObject.SetActive(false);
        logo.gameObject.SetActive(true);
        logo.transform.localScale = Vector2.zero;

        yield return new WaitForSeconds(1);

        year.gameObject.SetActive(true);
        for (int i = 0; i < 20; i++) {
            Color col = year.color;
            col.a = i * 0.05f;

            year.color = col;

            yield return new WaitForSeconds(0.05f);
        }

        for (int i = 1; i <= 6; i++) {
            year.text = (2024 - i).ToString();

            SoundsLab.Instance.Play("click");

            yield return new WaitForSeconds(0.5f);
        }

        Vector2 realVec = year.transform.localScale;

        LeanTween.scale(year.gameObject, new Vector3(realVec.x * 1.5f, realVec.x * 1.5f), 0.1f);
        SoundsLab.Instance.Play("pababa", 0.05f);
        yield return new WaitForSeconds(0.1f);
        LeanTween.scale(year.gameObject, new Vector3(realVec.x * 1.4f, realVec.x * 1.4f), 0.1f);

        yield return new WaitForSeconds(0.5f);

        LeanTween.moveLocal(year.gameObject, yearPos, 1f);
        LeanTween.scale(year.gameObject, yearSc, 1f);

        yield return new WaitForSeconds(1.2f);

        SoundsLab.Instance.Play("touwng", 0.3f);

        year.gameObject.SetActive(false);
        comment.gameObject.SetActive(true);

        yield return new WaitForSeconds(2.6f);
        mm.gameObject.SetActive(true);
        comment.text = "";

        yield return new WaitForSeconds(1f);
        mOutline.gameObject.SetActive(true);
        mOutline.transform.localPosition = new Vector3(0, 100);
        SoundsLab.Instance.Play("click");

        yield return new WaitForSeconds(1f);
        LeanTween.moveLocal(mOutline.gameObject, new Vector3(0, 0), 0.2f).setEase(LeanTweenType.easeOutCubic);
        SoundsLab.Instance.Play("click");

        yield return new WaitForSeconds(1f);
        LeanTween.moveLocal(mOutline.gameObject, new Vector3(0, -100), 0.2f).setEase(LeanTweenType.easeOutCubic);
        SoundsLab.Instance.Play("click");

        yield return new WaitForSeconds(1f);
        LeanTween.moveLocal(mOutline.gameObject, new Vector3(0, 0), 0.2f).setEase(LeanTweenType.easeOutCubic);
        SoundsLab.Instance.Play("click");

        yield return new WaitForSeconds(1.3f);
        LeanTween.moveLocal(mm.gameObject, new Vector3(-225f, -156f), 0.1f).setEase(LeanTweenType.easeOutCubic);
        LeanTween.scale(mm.gameObject, new Vector3(1.5f, 1.5f), 0.05f);
        SoundsLab.Instance.Play("likeVill", 0.3f);

        yield return new WaitForSeconds(0.8f);
        comment2.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.6f);
        comment2.text = "흙";
        SoundsLab.Instance.Play("likeVill", 0.3f);

        yield return new WaitForSeconds(0.3f);
        comment2.text = "흙흙";
        SoundsLab.Instance.Play("likeVill", 0.3f, true);

        yield return new WaitForSeconds(0.3f);
        comment2.text = "흙흙흙";
        SoundsLab.Instance.Play("likeVill", 0.3f, true);

        yield return new WaitForSeconds(0.3f);
        comment2.text = "흙흙흙흙";
        SoundsLab.Instance.Play("likeVill", 0.3f, true);

        yield return new WaitForSeconds(0.3f);
        comment2.text = "흙파기 맵";
        SoundsLab.Instance.Play("likeVill", 0.3f, true);

        yield return new WaitForSeconds(1.4f);
        comment2.text = "";
        mm.gameObject.SetActive(false);

        SoundsLab.Instance.Play("tc", 0.5f);

        yield return new WaitForSeconds(1.2f);
        comment.text = "모두가 흙을 파면서 행복해 하였다.";

        yield return new WaitForSeconds(2f);
        comment.text = "나도!";

        LeanTween.scale(year.gameObject, new Vector3(realVec.x * 2f, realVec.x * 2f), 0.01f);

        yield return new WaitForSeconds(0.8f);

        year.transform.localScale = realVec;
        comment.text = "";

        yield return new WaitForSeconds(1.4f);
        comment.text = "오랜만에 또 하고 싶었다.";

        yield return new WaitForSeconds(1.8f);
        comment.text = "그래서 만들었다.";

        SoundsLab.Instance.Stop("tc");
        yield return new WaitForSeconds(2f);
        SoundsLab.Instance.Play("intro", 0.3f);

        LeanTween.scale(logo, Vector2.one, 1.5f);

        yield return new WaitForSeconds(1.8f);

        LeanTween.scale(gameObject, Vector2.zero, 0.8f);

        yield return new WaitForSeconds(1.2f);

        SoundsLab.Instance.Play("pz");
    }
}
