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
using System.Diagnostics;

namespace MRobot {
   public partial class MainWindow : Window {
      public MainWindow () {
         InitializeComponent ();
         mArduino = new SerialPort ("COM5", 9600);
         try {
            mArduino.Open ();
            StatusLabel.Content = "Connected";
         } catch (Exception ex) {
            StatusLabel.Content = "Not connected";
            MessageBox.Show ("Could not open COM3: " + ex.Message);
         }
      }

      #region Implementations ---------------------------------------

      void ToggleState (object sender, RoutedEventArgs e) {
         mArduino.WriteLine ($"{mS1},{mS2},{mS3}");
      }

      void Slide (object sender, RoutedPropertyChangedEventArgs<double> e) {
         if (sender is Slider btn) {
            string name = btn.Name;
            Debug.WriteLine ($"Name of the Sender is {name}");
            int steps = (int)e.NewValue;
            Debug.WriteLine ($"The Updated value is {steps}");
            switch (name) {
               case "Sliderx1":
                  mS1 = steps;
                  SliderLabel1.Content = $"To Move : {steps}";
                  break;
               case "Sliderx2":
                  mS2 = steps;
                  SliderLabel2.Content = $"To Move : {steps}";
                  break;
               default:
                  mS3 = steps;
                  SliderLabel3.Content = $"To Move : {steps}";
                  break;
            }
         }
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
      #endregion

      #region Fields ------------------------------------------------
      SerialPort mArduino;
      int mS1,mS2,mS3;
      #endregion
   }
}