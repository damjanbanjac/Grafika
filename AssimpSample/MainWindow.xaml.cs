using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using SharpGL.SceneGraph;
using SharpGL;
using Microsoft.Win32;


namespace AssimpSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Atributi

        /// <summary>
        ///	 Instanca OpenGL "sveta" - klase koja je zaduzena za iscrtavanje koriscenjem OpenGL-a.
        /// </summary>
        World m_world = null;

        #endregion Atributi

        #region Konstruktori

        public MainWindow()
        {
            // Inicijalizacija komponenti
            InitializeComponent();

            // Kreiranje OpenGL sveta
            try
            {
                m_world = new World(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "3D Models\\Balloon"), "11809_Hot_air_balloon_l2.obj ", (int)openGLControl.Width, (int)openGLControl.Height, openGLControl.OpenGL,this);
            }
            catch (Exception e)
            {
                MessageBox.Show("Neuspesno kreirana instanca OpenGL sveta. Poruka greške: " + e.Message, "Poruka", MessageBoxButton.OK);
                this.Close();
            }
        }

        #endregion Konstruktori

        /// <summary>
        /// Handles the OpenGLDraw event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLDraw(object sender, OpenGLEventArgs args)
        {
            m_world.Draw(args.OpenGL);
        }

        /// <summary>
        /// Handles the OpenGLInitialized event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLInitialized(object sender, OpenGLEventArgs args)
        {
            m_world.Initialize(args.OpenGL);
        }

        /// <summary>
        /// Handles the Resized event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        private void openGLControl_Resized(object sender, OpenGLEventArgs args)
        {
            m_world.Resize(args.OpenGL, (int)openGLControl.Width, (int)openGLControl.Height);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (m_world.animation) //AKO JE ANIMACIJA U TOKU, IGNORISTI AKCIJE KORISNIKA
                return;
            switch (e.Key)
            {
                
                case Key.F2: this.Close(); break; // IZLAZ IZ APP
                case Key.T: m_world.RotationX -= 5.0f; break;//ROTACIJE
                case Key.G: m_world.RotationX += 5.0f; break;
                case Key.F: m_world.RotationY -= 5.0f; break;
                case Key.H: m_world.RotationY += 5.0f; break;
                case Key.X: toggleInerface(false); m_world.startAnimation(); break;//ONEMOGUCI INTERFEJS, STARTUJ ANIMACIJU
                case Key.Add: m_world.SceneDistance -= 200; break; //ZOOM IN I OUT
                case Key.Subtract: m_world.SceneDistance += 200.0f; break;
                
            }
        }

        //STAVI CEO INTERFEJS NA ENABLE ILI DISABLE
        public void toggleInerface(bool boolean) {
            sliderBlue.IsEnabled = boolean;
            sliderRed.IsEnabled = boolean;
            sliderGreen.IsEnabled = boolean;
            sliderDoor.IsEnabled = boolean;
            sliderHangar.IsEnabled = boolean;
        }

        //OBRADJUJE PROMENU SLAJDERA ZA VRATA
        private void sliderDoor_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (m_world != null)
                m_world.doorSpeed = (float)e.NewValue;
        }

        //OBRADJUJE PROMENU SLAJDERA ZA HANGAR
        private void sliderHangar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (m_world != null)
                m_world.hangarHeight = (float)e.NewValue;
        }


        //RGB KOMPONENTE SVETLA
        private void sliderRed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (m_world != null)
                m_world.red = (float)e.NewValue;
        }

        private void sliderGreen_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (m_world != null)
                m_world.green = (float)e.NewValue;
        }

        private void sliderBlue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (m_world != null)
                m_world.blue = (float)e.NewValue;
        }
    }
}
