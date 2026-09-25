using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO.Ports;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MRobot {
   public partial class MainWindow : Window {
      public MainWindow () {
         InitializeComponent ();
         mArduino = new SerialPort ("COM3", 9600);
         try {
            mArduino.Open ();
            StatusLabel.Content = "Connected";
         } catch (Exception ex) {
            StatusLabel.Content = "Not connected";
            MessageBox.Show ("Could not open COM3: " + ex.Message);
         }
      }

      void ToggleState (object sender, RoutedEventArgs e) {
         var nState = mMotorState is EState.Running ? EState.Stopped : EState.Running;
         mMotorState = nState;
         if (mMotorState is EState.Running) mArduino.WriteLine ("1");
         else mArduino.WriteLine ("0");
      }

      void DisconnectPort () {
         if (mArduino != null && mArduino.IsOpen) {
            mArduino.Close (); mArduino.Dispose ();
         }
      }

      protected override void OnClosed (EventArgs e) {
         DisconnectPort ();
         base.OnClosed (e);
      }

      #region Fields ------------------------------------------------
      SerialPort mArduino;
      EState mMotorState = EState.Running;
      #endregion
   }
}

public enum EState { Running, Stopped };