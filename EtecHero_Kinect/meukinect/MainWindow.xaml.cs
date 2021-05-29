using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using AuxiliarKinect.FuncoesBasicas;
using System.Threading;

namespace meukinect
{
    public partial class MainWindow : Window
    {
        bool MaoDireitaAcimaCabeca = false;
        bool MaoEsquerdaAcimaCabeca = false;
        bool peDireito = false;
        bool quadrilDireito = false;
        bool pe_direito_longe_quadril = false;

        public MainWindow()
        {
            InitializeComponent();
            InicializarSensor();           
        }

        private void InicializarSensor()
        {
            KinectSensor kinect = InicializadorKinect.InicializarPrimeiroSensor(0);
            kinect.SkeletonStream.Enable();
            kinect.SkeletonFrameReady += KinectEvent;
        }

        private void KinectEvent(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame quadroAtual = e.OpenSkeletonFrame())
            {
                if (quadroAtual != null)
                {
                    ExecutarRegraMaoDireitaAcimaDaCabeca(quadroAtual);
                }
            }
        }

        private void ExecutarRegraMaoDireitaAcimaDaCabeca(SkeletonFrame quadroAtual)
        {
            Skeleton[] esqueletos = new Skeleton[6];
            quadroAtual.CopySkeletonDataTo(esqueletos);
            Skeleton usuario = esqueletos.FirstOrDefault(esqueleto => esqueleto.TrackingState == SkeletonTrackingState.Tracked);

            if (usuario != null)
            {
                //Console.WriteLine("ACHOU O USUÁRIO");
                Joint maoDireita = usuario.Joints[JointType.HandRight];
                Joint maoEsquerda = usuario.Joints[JointType.HandLeft];
                Joint quadrilDireito = usuario.Joints[JointType.HipRight];
                Joint peDireito = usuario.Joints[JointType.FootRight];
                Joint cabeca = usuario.Joints[JointType.Head];
                Joint tornozelo = usuario.Joints[JointType.ElbowRight];
                Joint ombroDireito = usuario.Joints[JointType.ShoulderRight];
                
                /*
                float valor_z_mao_direita = peDireito.Position.Z;
                Console.WriteLine("valor mao direita = " + valor_z_mao_direita.ToString());
                bool novoTestepeDireitoFechar = peDireito.Position.Y > ombroDireito.Position.Y;*/

                Console.WriteLine("Posição Ombro : " + peDireito.Position.Y.ToString());
                if(peDireito.Position.Y > 0)
                {
                    MessageBox.Show("LEVANTOU PÉ DIREITO");
                }

                /*if (pe_direito_longe_quadril != novoTestepeDireitoFechar)
                {
                    pe_direito_longe_quadril = novoTestepeDireitoFechar;
                    if (novoTestepeDireitoFechar)
                        MessageBox.Show("PÉ DIREITO LONGE DO OMBRO");
                }*/

                bool novoTesteMaoDireitaAcimaCabeca = maoDireita.Position.Y > cabeca.Position.Y;
                if (MaoDireitaAcimaCabeca != novoTesteMaoDireitaAcimaCabeca)
                {
                    MaoDireitaAcimaCabeca = novoTesteMaoDireitaAcimaCabeca;
                    if (MaoDireitaAcimaCabeca)
                        MessageBox.Show("A mão DIREITA está acima da cabeça!");
                }

                bool novoTesteMaoEsquerdaAcimaCabeca = maoEsquerda.Position.Y > cabeca.Position.Y;
                if (MaoEsquerdaAcimaCabeca != novoTesteMaoEsquerdaAcimaCabeca)
                {
                    MaoEsquerdaAcimaCabeca = novoTesteMaoEsquerdaAcimaCabeca;

                    if (MaoEsquerdaAcimaCabeca)
                        MessageBox.Show("A mão ESQUERDA está acima da cabeça!");
                }
            } else {
                Console.WriteLine("Cadê o usuário?");
            }
         }
       }
    }
