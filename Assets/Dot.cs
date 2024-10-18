using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Dot : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    private Tween fadeTween;

    private BackgroundTile _backgroundTile;
    private ID _id;
    private bool _matched;
    public BackgroundTile BackgroundTile { get => _backgroundTile; set => _backgroundTile = value; }
    public ID Id { get => _id; set => _id = value; }
    public bool Matched { get => _matched; set => _matched = value; }
    public Tween FadeIn(float duration)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(1, duration));
        sequence.Join(Fade(1f, duration, () =>
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }));
        return sequence;
    }
    public Tween FadeOut(float duration)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(0, duration));
        sequence.Join(Fade(0, duration, () =>
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }));
        return sequence;
    }

    private Tween Fade(float endValue, float duration, TweenCallback onEnd)
    {
        if(fadeTween != null)
        {
            fadeTween.Kill(false); // false : off tween
        }
        
        fadeTween = canvasGroup.DOFade(endValue, duration).OnComplete(onEnd);
        return fadeTween;
    }
}
public enum ID
{
    Element0,
    Element1,
    Element2,
    Element3,
    Element4,
    Element5,
    Element6,
    Element7,
    None
}
