using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

namespace LeapWindowsController
{
    class LeapControl
    {
        public static void Main()
        {
            // Create a sample listener and controller
            SampleListener listener = new SampleListener();
            Controller controller = new Controller();

            // Have the sample listener receive events from the controller
            controller.AddListener(listener);

            // Keep this process running until Enter is pressed
            Console.WriteLine("Press Enter to quit...");
            Console.ReadLine();

            //used to keep things running when set to windows application build mode.
            while (listener != null)
            {

            }
            // Remove the sample listener when done
            controller.RemoveListener(listener);
            controller.Dispose();
        }
    }
}
