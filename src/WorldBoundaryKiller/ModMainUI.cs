using Modding.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WorldBoundaryKiller
{
    class ModMainUI : SafeUIBehaviour
    {
        Vector3 pivotPosition = Vector3.zero;
        Quaternion pivotRotation = Quaternion.identity;
        public override string InitialWindowName() { return "World Boundary Killer"; }

        public override Rect InitialWindowRect()
        {
            return new Rect(550, 100, 200, 10);
        }

        public override bool ShouldShowGUI()
        {
            return !StatMaster.hudHidden && !StatMaster.levelSimulating && this.IsPlaying();
        }

        bool enabled;
        bool lastEnabled;
        protected override void WindowContent(int windowID)
        {
            enabled = GUILayout.Toggle(enabled, "enabled");
            GUI.DragWindow();
        }

        void Update()
        {
            //enabled = !StatMaster.Bounding.Enabled;
            GameObject boundaryObject = GameObject.Find("WORLD BOUNDARIES");
            if (boundaryObject == null) boundaryObject = GameObject.Find("LEVEL BARREN EXPANSE/WORLD BOUNDARIES_LARGE");
            if (boundaryObject == null) boundaryObject = GameObject.Find("LEVEL SANDBOX/WORLD BOUNDARIES_LARGE (1)");
            if (enabled)
            {
                if (!StatMaster.Bounding.Enabled)
                {
                    PlayerMachine.GetLocal().InternalObject.boundingBoxController.addPiece.outOfBounds = false;
                }
                if (boundaryObject != null) SetChildrenActive(boundaryObject, false);
            }
            else
            {
                if (boundaryObject != null) SetChildrenActive(boundaryObject, true);
                if (lastEnabled)
                {
                    PlayerMachine.GetLocal().InternalObject.CheckBounds();
                }
            }
            lastEnabled = enabled;
        }
        void SetChildrenActive(GameObject go, bool active)
        {
            for(int i=0; i< go.transform.childCount; i++)
            {
                go.transform.GetChild(i).gameObject.SetActive(active);
            }
        }
    }
}
