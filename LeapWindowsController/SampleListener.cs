using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Leap;

namespace LeapWindowsController
{
    /******************************************************************************\
* Copyright (C) 2012-2014 Leap Motion, Inc. All rights reserved.               *
* Leap Motion proprietary and confidential. Not for distribution.              *
* Use subject to the terms of the Leap Motion SDK Agreement available at       *
* https://developer.leapmotion.com/sdk_agreement, or another agreement         *
* between Leap Motion and you, your company or other organization.             *
\******************************************************************************/
    class SampleListener : Listener
    {
        private Object thisLock = new Object();
        private bool mouseDown = false;

        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        // constants for the mouse_input() API function
        private const int MOUSEEVENTF_MOVE = 0x0001;
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const int MOUSEEVENTF_LEFTUP = 0x0004;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const int MOUSEEVENTF_RIGHTUP = 0x0010;
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        private const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;


        private void SafeWriteLine(String line)
        {
            lock (thisLock)
            {
                Console.WriteLine(line);
            }
        }

        public override void OnInit(Controller controller)
        {
            
        }

        public override void OnConnect(Controller controller)
        {
            
            controller.EnableGesture(Gesture.GestureType.TYPE_CIRCLE);
            controller.EnableGesture(Gesture.GestureType.TYPE_KEY_TAP);
            controller.EnableGesture(Gesture.GestureType.TYPE_SCREEN_TAP);
            controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
        }

        public override void OnDisconnect(Controller controller)
        {
            
        }

        public override void OnExit(Controller controller)
        {
            
        }

        public override void OnFrame(Controller controller)
        {
            // Get the most recent frame and report some basic information
            Frame frame = controller.Frame();
            int x = Cursor.Position.X;
            int y = Cursor.Position.Y;
            foreach (Hand hand in frame.Hands)
            {
                if (hand.Fingers[1].IsExtended && !hand.Fingers[3].IsExtended && !hand.Fingers[4].IsExtended)
                { 
                    int newXInt = (int)hand.Fingers[1].TipPosition.x;
                    int newYInt = (int)hand.Fingers[1].TipPosition.y;

                    //X range is -200 to 200
                    //Y range is 100 to 300
                    newXInt += 200; //set range from 0 to 400
                    newYInt -= 100; //set range from 0 to 300
                    //make sure values in the correct range
                    if (newXInt < 0)
                        newXInt = 0;
                    if (newXInt > 400)
                        newXInt = 400;
                    
                    if (newYInt < 0)
                        newYInt = 0;
                    if (newYInt > 300)
                        newYInt = 300;

                    //values from 0 to 1
                    float newX = (float)newXInt / 400;
                    float newY = 1 - ((float)newYInt / 300);

                    //scale values to screen size
                    newX *= System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                    newY *= System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

                    //set the cursor to the corect point
                    Cursor.Position = new Point((int)newX, (int)newY);
                }
                //left mouse down when thumb in
                if (!hand.Fingers[0].IsExtended)
                {
                    if (!mouseDown)
                    {
                        mouseDown = true;
                        mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
                    }
                }
                    //mouse up when thumb out
                else
                {
                    if (mouseDown)
                    {
                        mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
                        mouseDown = false;
                    }
                }
            }

            if (!frame.Hands.IsEmpty || !frame.Gestures().IsEmpty)
            {
                SafeWriteLine("");
            }
        }
    }

}
