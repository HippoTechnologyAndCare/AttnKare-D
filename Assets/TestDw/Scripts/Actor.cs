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
    private const string PARAM_ANIM_SPEED = "AnimationSpeed";

    SpriteRenderer[] spriteRenderers;
    Animator animator;
    Quaternion originalRotation;

    private float moveTime = 3f;
    void Start()
    {
        originalRotation = transform.rotation;
        spriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool(PARAM_ANIM_IS_WALK, false);

        //Test
        TestMove(Vector3.left * 3f);
    }

    void TestMove(Vector3 dir) 
    {
        SimpleMove(dir, () =>
        {
            dir = 3f * (dir.x > 0 ? Vector3.left : Vector3.right);
            TestMove(dir);
        });
    }

    public void SimpleMove(Transform trans, System.Action onComplete = null) => SimpleMove(trans.position, onComplete);
    public void SimpleMove(Vector3 position, System.Action onComplete = null)
    {
        var dest = position;
        dest.y = transform.position.y;
        animator.SetBool(PARAM_ANIM_IS_WALK, true);

        var tween = transform.DOMove(dest, moveTime).OnComplete(() =>
        {
            onComplete?.Invoke();
        });

        tween.OnUpdate(() => 
        {
            var dir = dest - transform.position;

            if(dir.magnitude <= 0.25f)
            {
                animator.SetFloat(PARAM_ANIM_SPEED, 1f);
                animator.SetBool(PARAM_ANIM_IS_WALK, false);
            }
            else 
            {
                animator.SetFloat(PARAM_ANIM_SPEED, 1f + dir.magnitude * 0.5f);
            }

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
        transform.rotation = Camera.main.transform.rotation * originalRotation;
        //transform.forward = Camera.main.transform.forward;

        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    SimpleMove(Vector3.left * 3f);
        //}
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    SimpleMove(Vector3.right * 3f);
        //}

        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    animator.SetBool(PARAM_ANIM_IS_WALK, false);
        //    transform.DOPause();
        //}
    }
}
