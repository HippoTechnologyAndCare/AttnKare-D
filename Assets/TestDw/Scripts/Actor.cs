using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using Pool;
using System;

//SimpleMove(lTrans.position);
//speechBubble.SetText("Messagesadnasdkandkandkadnkadnkasndakn");

public partial class Actor : MonoBehaviour
{
    private const string PARAM_ANIM_IS_WALK = "isWalk";
    private const string PARAM_ANIM_SPEED = "AnimationSpeed";

    SpriteRenderer[] spriteRenderers;
    Animator animator;
    Quaternion originalRotation;

    public float moveTime = 3f;
    void Start()
    {
        originalRotation = transform.rotation;
        spriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();

        InitAnimation();
        TestMove(Vector3.left * 3f);
        //SimpleMove(Vector3.left );
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

            if (dir.magnitude <= 0.25f)
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
                if (dir.x != 0)
                {
                    _.flipX = dir.x > 0;
                }
            });
        });
    }
}

public partial class Actor//+Ani,Fx
{
    Pool<FxObject> pool_footStep;

    void InitAnimation()
    {
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool(PARAM_ANIM_IS_WALK, false);
        pool_footStep = new Pool<FxObject>(new PrefabFactory<FxObject>(Resources.Load<GameObject>("eff_footStep")), 5);
    }

    public void OnFootStep()
    {
        var fx = pool_footStep.Allocate();
        fx.transform.position = transform.position + Vector3.up * -0.1f;
        fx.actEnded = () => 
        { 
            pool_footStep.Release(fx); 
        };

        fx.Init();
    }
}

public partial class Actor//CameraFacing
{
    Camera referenceCamera;
    public enum Axis { up, down, left, right, forward, back };
    public bool reverseFace = false;
    public Axis axis = Axis.up;
    void Awake()
    {
        // if no camera referenced, grab the main camera
        if (!referenceCamera)
            referenceCamera = Camera.main;
    }
    //Orient the camera after all movement is completed this frame to avoid jittering
    void LateUpdate()
    {/*
        var a = Camera.main.transform.forward;
        a.y = 0f;

        transform.forward = a;
        */
    }
    public Vector3 GetAxis(Axis refAxis)
    {
        switch (refAxis)
        {
            case Axis.down:
                return Vector3.down;
            case Axis.forward:
                return Vector3.forward;
            case Axis.back:
                return Vector3.back;
            case Axis.left:
                return Vector3.left;
            case Axis.right:
                return Vector3.right;
        }

        // default is Vector3.up
        return Vector3.up;
    }
}

