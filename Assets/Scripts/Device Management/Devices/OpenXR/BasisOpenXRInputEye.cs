using System.Collections.Generic;
using UnityEngine.XR;

namespace Assets.Scripts.Device_Management.Devices.OpenXR
{
    public class BasisOpenXRInputEye : BasisInputEye
    {
        private XRNodeState leftEyeState;
        private XRNodeState rightEyeState;

        public override void Initalize()
        {
            List<XRNodeState> nodeStates = new List<XRNodeState>();
            InputTracking.GetNodeStates(nodeStates);

            foreach (XRNodeState nodeState in nodeStates)
            {
                if (nodeState.nodeType == XRNode.LeftEye)
                {
                    leftEyeState = nodeState;
                }
                else if (nodeState.nodeType == XRNode.RightEye)
                {
                    rightEyeState = nodeState;
                }
            }
        }

        public override void Simulate()
        {
            if (leftEyeState.TryGetPosition(out LeftPosition))
            {
            }
            if (rightEyeState.TryGetPosition(out RightPosition))
            {
            }
        }
    }
}
