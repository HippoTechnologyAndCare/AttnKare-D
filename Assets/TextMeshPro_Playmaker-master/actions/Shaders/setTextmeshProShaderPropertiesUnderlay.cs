// (c) Eric Vander Wal, 2017 All rights reserved.
// Custom Action by DumbGameDev
// www.dumbgamedev.com

using UnityEngine;
using TMPro;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("TextMesh Pro Shader")]
	[Tooltip("Set Text Mesh Pro underlay shaders.")]

	public class  setTextmeshProShaderPropertiesUnderlay : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(TextMeshPro))]
		[Tooltip("Textmesh Pro component is required.")]
		public FsmOwnerDefault gameObject;

        [TitleAttribute("Enable Underlay Shader")]
        public FsmBool enable;

        [TitleAttribute("Enable Inner Underlay Shader")]
        public FsmBool enableInner;

        public FsmColor underlayColor;

        [HasFloatSlider(-1, 1)]
        public FsmFloat offsetX;

        [HasFloatSlider(-1, 1)]
        public FsmFloat offsetY;

        [HasFloatSlider(-1, 1)]
        public FsmFloat dilate;

        [HasFloatSlider(0, 1)]
        public FsmFloat softness;

        [Tooltip("Check this box to preform this action every frame.")]
		public FsmBool everyFrame;

		TextMeshPro meshproScript;

		public override void Reset()
		{

			gameObject = null;
            underlayColor = null;
            offsetX = null;
            offsetY = null;
            dilate = null;
            softness = null;
            everyFrame = false;
            enable = false;
		}

		public override void OnEnter()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);


			meshproScript = go.GetComponent<TextMeshPro>();

			if (!everyFrame.Value)
			{
				DoMeshChange();
				Finish();
			}

		}

		public override void OnUpdate()
		{
			if (everyFrame.Value)
			{
				DoMeshChange();
			}
		}

		void DoMeshChange()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}

            if (enable.Value == true)
            {
                meshproScript.fontSharedMaterial.EnableKeyword("UNDERLAY_ON");
            }

            if (enableInner.Value == true)
            {
                meshproScript.fontSharedMaterial.EnableKeyword("UNDERLAY_INNER");
            }

            meshproScript.fontSharedMaterial.SetColor("_UnderlayColor", underlayColor.Value);
            meshproScript.fontSharedMaterial.SetFloat("_UnderlayOffsetX", offsetX.Value);
            meshproScript.fontSharedMaterial.SetFloat("_UnderlayOffsetY", offsetY.Value);
            meshproScript.fontSharedMaterial.SetFloat("_UnderlayDilate", dilate.Value);
            meshproScript.fontSharedMaterial.SetFloat("_UnderlaySoftness", softness.Value);

        }

    }
}