using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
namespace TestMod
{
    public class DeleteRay : MonoBehaviour
    {
        public PhotonPeer Peer { get; set; }

        public void Start()
        {
            this.Peer = PhotonNetwork.NetworkingClient.LoadBalancingPeer;
        }

        public void Update()
        {

            
            

        }

        Rect windowRect = new Rect(40,40,120,180);
        public void OnGUI()
        {
            Render.DrawString(new Vector2(200, 40), "Lagger");

            windowRect = GUI.Window(3, windowRect, RenderWindow, "Lagger");
        }

        public void RenderWindow(int windowID)
        {

            GUI.DragWindow(new Rect(0, 0, 0x1000, 30));

           
            if (this.Peer == null)
                GUILayout.Label("No peer to communicate with. ");

            GUILayout.Label(string.Format("Rtt:{0,4} +/-{1,3}", this.Peer.RoundTripTime, this.Peer.RoundTripTimeVariance));

            bool simEnabled = this.Peer.IsSimulationEnabled;
            bool newSimEnabled = GUILayout.Toggle(simEnabled, "Simulate");
            if (newSimEnabled != simEnabled)
            {
                this.Peer.IsSimulationEnabled = newSimEnabled;
            }

            float inOutLag = this.Peer.NetworkSimulationSettings.IncomingLag;
            GUILayout.Label("Lag " + inOutLag);
            inOutLag = GUILayout.HorizontalSlider(inOutLag, 0, 500);

            this.Peer.NetworkSimulationSettings.IncomingLag = (int)inOutLag;
            this.Peer.NetworkSimulationSettings.OutgoingLag = (int)inOutLag;

            float inOutJitter = this.Peer.NetworkSimulationSettings.IncomingJitter;
            GUILayout.Label("Jit " + inOutJitter);
            inOutJitter = GUILayout.HorizontalSlider(inOutJitter, 0, 100);

            this.Peer.NetworkSimulationSettings.IncomingJitter = (int)inOutJitter;
            this.Peer.NetworkSimulationSettings.OutgoingJitter = (int)inOutJitter;

            float loss = this.Peer.NetworkSimulationSettings.IncomingLossPercentage;
            GUILayout.Label("Loss " + loss);
            loss = GUILayout.HorizontalSlider(loss, 0, 10);

            this.Peer.NetworkSimulationSettings.IncomingLossPercentage = (int)loss;
            this.Peer.NetworkSimulationSettings.OutgoingLossPercentage = (int)loss;
        }
    }
    
}
