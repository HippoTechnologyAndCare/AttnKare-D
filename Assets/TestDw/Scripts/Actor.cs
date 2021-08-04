using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

//SimpleMove(lTrans.position);
//speechBubble.SetText("Messagesadnasdkandkandkadnkadnkasndakn");

public class Actor : MonoBehaviour
{
    private const string PARAM_ANIM_IS_WALK = "isWalk";
    SpriteRenderer[] spriteRenderers;
    Animator animator;

    void Start()
    {
        spriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool(PARAM_ANIM_IS_WALK, false);
    }

    public void SimpleMove(Transform trans, System.Action onComplete = null) => SimpleMove(trans.position, onComplete);
    public void SimpleMove(Vector3 position, System.Action onComplete = null)
    {
        var dest = position;
        dest.y = transform.position.y;
        animator.SetBool(PARAM_ANIM_IS_WALK, true);

        var tween = transform.DOMove(dest, 1f).OnComplete(() =>
        {
            animator.SetBool(PARAM_ANIM_IS_WALK, false);
            onComplete?.Invoke();
        });

        tween.OnUpdate(() => 
        {
            var dir = dest - transform.position;

            spriteRenderers.ToList().ForEach(_ =>
            {
                if(dir.x != 0)
                {
                    _.flipX = dir.x < 0;
                }
            });
        });
    }

    void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
