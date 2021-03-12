using System;
using System.IO;
using System.Linq;
using System.Windows;
namespace IPv4
{
    public partial class MainWindow : Window
    {
        string msgERR, CIDR_ou_MASQUE = "MASQUE", CLASSEdeMasque = "";
        public MainWindow()
        {
            InitializeComponent();
            lblslash.Visibility = Visibility.Hidden;
            txtCIDRtoRun.Visibility = Visibility.Hidden;
        }
        //-----------------------------------------------------------------------
        // RADIOBOX CLASSE MASQUE /CIDR / CHECKBOX SOUS RESEAU
        //-----------------------------------------------------------------------
        private void Rdb_classeC_Checked(object sender, RoutedEventArgs e)
        {
            CLASSEdeMasque = "C";
            CIDR_ou_MASQUE = "MASQUE";
            txtCIDRtoRun.Text = "";
            txtMasquedeSR.Text = "255.255.255.0";
            txtCIDRtoRun.Visibility = Visibility.Hidden;
            lblslash.Visibility = Visibility.Hidden;
            txtMasquedeSR.Visibility = Visibility.Visible;
            lblMasque.Visibility = Visibility.Visible;
        }
        private void Rdb_classeB_Checked(object sender, RoutedEventArgs e)
        {
            CLASSEdeMasque = "B";
            CIDR_ou_MASQUE = "MASQUE";
            txtCIDRtoRun.Text = "";
            txtMasquedeSR.Text = "255.255.0.0";
            txtCIDRtoRun.Visibility = Visibility.Hidden;
            lblslash.Visibility = Visibility.Hidden;
            txtMasquedeSR.Visibility = Visibility.Visible;
            lblMasque.Visibility = Visibility.Visible;
        }
        private void Rdb_classeA_Checked(object sender, RoutedEventArgs e)
        {
            CLASSEdeMasque = "A";
            CIDR_ou_MASQUE = "MASQUE";
            txtCIDRtoRun.Text = "";
            txtMasquedeSR.Text = "255.0.0.0";
            txtCIDRtoRun.Visibility = Visibility.Hidden;
            lblslash.Visibility = Visibility.Hidden;
            txtMasquedeSR.Visibility = Visibility.Visible;
            lblMasque.Visibility = Visibility.Visible;
        }
        private void Rdb_classLess_Checked(object sender, RoutedEventArgs e)
        {
            CLASSEdeMasque = "CLASSLESS";
            CIDR_ou_MASQUE = "MASQUE";
            txtCIDRtoRun.Text = "";
            txtMasquedeSR.Text = "255.255.0.0";
            txtCIDRtoRun.Visibility = Visibility.Hidden;
            lblslash.Visibility = Visibility.Hidden;
            txtMasquedeSR.Visibility = Visibility.Visible;
            lblMasque.Visibility = Visibility.Visible;
        }
        private void Rdb_CIDR_Checked(object sender, RoutedEventArgs e)
        {
            CLASSEdeMasque = "CIDR";
            CIDR_ou_MASQUE = "CIDR";
            txtCIDRtoRun.Visibility = Visibility.Visible;
            lblslash.Visibility = Visibility.Visible;
            txtMasquedeSR.Visibility = Visibility.Hidden;
            lblMasque.Visibility = Visibility.Hidden;
        }
        //-----------------------------------------------------------------------
        // BOUTON CALCULER
        //-----------------------------------------------------------------------
        private void BtnCalculer_Click(object sender, RoutedEventArgs e)
        {
            //-----------------------------------------------------------------------
            // CIDR 0 ET 32
            //-----------------------------------------------------------------------
            msgERR = "";
            if (txtCIDRtoRun.Text == "0")
            {
                txtAdr.Text = txtIP.Text;
                txtBroadCast.Text = "255.255.255.255";
                txtCIDR.Text = "0";
                txtDernierHote.Text = "255.255.255.254";
                txtMasque.Text = "0.0.0.0";
                txtNbHote.Text = "4294967294";
                txtPremierHote.Text = "0.0.0.1";
                txtReseau.Text = "0.0.0.0";
                txtBinAdresse.Text = "";
                txtBinMasque.Text = "";
                txtBinReseaux.Text = "";
                txtBinBroadCast.Text = "";
            }
            else if (txtCIDRtoRun.Text == "32")
            {
                txtBroadCast.Text = "";
                txtAdr.Text = txtIP.Text;
                txtCIDR.Text = "32";
                txtDernierHote.Text = "";
                txtMasque.Text = "255.255.255.255";
                txtNbHote.Text = "1";
                txtPremierHote.Text = txtIP.Text;
                txtReseau.Text = "";
                txtBinAdresse.Text = "";
                txtBinMasque.Text = "";
                txtBinReseaux.Text = "";
                txtBinBroadCast.Text = "";
            }
            else if (CLASSEdeMasque == "" && txtCIDRtoRun.Text == "")
            {
                MessageBox.Show("Sélectionez une classe", "Message d'erreur", MessageBoxButton.OK, MessageBoxImage.Stop);
                msgERR = "ERR!!";
            }
            else
            {
                //-----------------------------------------------------------------------
                // ADRESSE IP
                //-----------------------------------------------------------------------
                string IP = txtIP.Text;
                int dotLOC;
                int @IPoc1 = 0, @IPoc2 = 0, @IPoc3 = 0, @IPoc4 = 0;
                int nbDOTip = txtIP.Text.Count(ch => ch == '.');
                if (nbDOTip >= 4 || IP == "" || nbDOTip < 3)
                {
                    MessageBox.Show("Adresse IP invalide", "Message d'erreur", MessageBoxButton.OK, MessageBoxImage.Stop);
                    msgERR = "ERR!!";
                }//ERR!!
                else
                {
                    dotLOC = IP.IndexOf(".");
                    @IPoc1 = Convert.ToInt32(IP.Substring(0, dotLOC));
                    IP = IP.Remove(0, dotLOC + 1);
                    dotLOC = IP.IndexOf(".");
                    @IPoc2 = Convert.ToInt32(IP.Substring(0, dotLOC));
                    IP = IP.Remove(0, dotLOC + 1);
                    dotLOC = IP.IndexOf(".");
                    @IPoc3 = Convert.ToInt32(IP.Substring(0, dotLOC));
                    IP = IP.Remove(0, dotLOC + 1);
                    @IPoc4 = Convert.ToInt32(IP.Substring(0));
                    if (@IPoc1 > 255 || @IPoc2 > 255 || @IPoc3 > 255 || @IPoc4 > 255)
                    {
                        MessageBox.Show("Adresse IP invalide", "Message d'erreur", MessageBoxButton.OK, MessageBoxImage.Stop);
                        msgERR = "ERR!!";
                    }//ERR!!
                }
                //-----------------------------------------------------------------------
                // IF CIDR ELSE MASQUE
                //-----------------------------------------------------------------------
                int @m1 = 0, @m2 = 0, @m3 = 0, @m4 = 0;
                string @m1_BIN = "", @m2_BIN = "", @m3_BIN = "", @m4_BIN = "";
                if (CIDR_ou_MASQUE == "CIDR")
                {
                    string CIDR = "";
                    if (txtCIDRtoRun.Text == "")
                    {
                        MessageBox.Show("CIDR invalide", "Message d'erreur", MessageBoxButton.OK, MessageBoxImage.Stop);
                        msgERR = "ERR!!";
                    }//ERR!!
                    else
                    {
                        int nbCIDR = Convert.ToInt32(txtCIDRtoRun.Text);
                        if (nbCIDR > 32)
                        {
                            MessageBox.Show("CIDR invalide", "Message d'erreur", MessageBoxButton.OK, MessageBoxImage.Stop);
                            msgERR = "ERR!!";
                        }//ERR!!
                        else
                        {
                            do
                            {
                                nbCIDR -= 1;
                                CIDR = CIDR.Insert(0, "1");
                            } while (nbCIDR != 0);
                            do
                            {
                                CIDR = CIDR.Insert(CIDR.Length, "0");
                            } while (CIDR.Length != 32);
                            string CIDR1_BIN = CIDR.Substring(0, 8);
                            string CIDR2_BIN = CIDR.Substring(8, 8);
                            string CIDR3_BIN = CIDR.Substring(16, 8);
                            string CIDR4_BIN = CIDR.Substring(24, 8);
                            @m1_BIN = CIDR1_BIN;
                            @m2_BIN = CIDR2_BIN;
                            @m3_BIN = CIDR3_BIN;
                            @m4_BIN = CIDR4_BIN;
                            @m1 = Convert.ToInt32(@m1_BIN, 2);
                            @m2 = Convert.ToInt32(@m2_BIN, 2);
                            @m3 = Convert.ToInt32(@m3_BIN, 2);
                            @m4 = Convert.ToInt32(@m4_BIN, 2);
                            txtMasque.Text = @m1 + "." + @m2 + "." + @m3 + "." + @m4;
                        }
                    }
                }//CIDR
                else
                {
                    string Masque = txtMasquedeSR.Text;
                    int nbDOTm = txtMasquedeSR.Text.Count(ch => ch == '.');
                    if (nbDOTm >= 4 || Masque == "" || nbDOTm < 3)
                    {
                        MessageBox.Show("Adresse du Masque invalide", "Message d'erreur", MessageBoxButton.OK, MessageBoxImage.Stop);
                        msgERR = "ERR!!";
                    }
                    else
                    {
                        int dotLOC2;
                        dotLOC2 = Masque.IndexOf(".");
                        @m1 = Convert.ToInt32(Masque.Substring(0, dotLOC2));
                        Masque = Masque.Remove(0, dotLOC2 + 1);
                        dotLOC2 = Masque.IndexOf(".");
                        @m2 = Convert.ToInt32(Masque.Substring(0, dotLOC2));
                        Masque = Masque.Remove(0, dotLOC2 + 1);
                        dotLOC2 = Masque.IndexOf(".");
                        @m3 = Convert.ToInt32(Masque.Substring(0, dotLOC2));
                        Masque = Masque.Remove(0, dotLOC2 + 1);
                        @m4 = Convert.ToInt32(Masque.Substring(0));
                        if (@m1 > 255 || @m2 > 255 || @m3 > 255 || @m4 > 255)
                        {
                            MessageBox.Show("Adresse du Masque invalide", "Message d'erreur", MessageBoxButton.OK, MessageBoxImage.Stop);
                            msgERR = "ERR!!";
                        }//ERR!!
                        else if (CLASSEdeMasque == "A" && @m1 < 255)
                        {
                            MessageBox.Show("Adresse du Masque invalide", "Message d'erreur", MessageBoxButton.OK, MessageBoxImage.Stop);
                            msgERR = "ERR!!";
                        }//ERR!!
                        else if (CLASSEdeMasque == "B" && @m1 < 255 && @m2 < 255)
                        {
                            MessageBox.Show("Adresse du Masque invalide", "Message d'erreur", MessageBoxButton.OK, MessageBoxImage.Stop);
                            msgERR = "ERR!!";
                        }//ERR!!
                        else if (CLASSEdeMasque == "C" && @m1 < 255 && @m2 < 255 && @m3 < 255)
                        {
                            MessageBox.Show("Adresse du Masque invalide", "Message d'erreur", MessageBoxButton.OK, MessageBoxImage.Stop);
                            msgERR = "ERR!!";
                        }//ERR!!
                        else
                        {
                            txtMasque.Text = @m1 + "." + @m2 + "." + @m3 + "." + @m4;
                            @m1_BIN = Convert.ToString(@m1, 2);
                            @m2_BIN = Convert.ToString(@m2, 2);
                            @m3_BIN = Convert.ToString(@m3, 2);
                            @m4_BIN = Convert.ToString(@m4, 2);
                        }
                    }
                }//MASQUE
                 //-----------------------------------------------------------------------
                 // IF ERR!!
                 //-----------------------------------------------------------------------
                if (msgERR == "ERR!!")
                {
                    txtSR.Text = "";
                    txtAdr.Text = "";
                    txtBinAdresse.Text = "";
                    txtBinMasque.Text = "";
                    txtBinReseaux.Text = "";
                    txtBinBroadCast.Text = "";
                    txtCIDR.Text = "";
                    txtBroadCast.Text = "";
                    txtDernierHote.Text = "";
                    txtMasque.Text = "";
                    txtNbHote.Text = "";
                    txtPremierHote.Text = "";
                    txtReseau.Text = "";
                }
                else
                {
                    //-----------------------------------------------------------------------
                    // BINAIRE
                    //-----------------------------------------------------------------------
                    string @IPoc1_BIN = Convert.ToString(@IPoc1, 2);
                    string @IPoc2_BIN = Convert.ToString(@IPoc2, 2);
                    string @IPoc3_BIN = Convert.ToString(@IPoc3, 2);
                    string @IPoc4_BIN = Convert.ToString(@IPoc4, 2);
                    //-----------------------------------------------------------------------
                    // RAJOUT DES 0 AUX @
                    //-----------------------------------------------------------------------
                    if (@IPoc1_BIN.Length < 8)
                    {
                        do
                        {
                            @IPoc1_BIN = @IPoc1_BIN.Insert(0, "0");
                        } while (@IPoc1_BIN.Length != 8);
                    }
                    if (@IPoc2_BIN.Length < 8)
                    {
                        do
                        {
                            @IPoc2_BIN = @IPoc2_BIN.Insert(0, "0");
                        } while (@IPoc2_BIN.Length != 8);
                    }
                    if (@IPoc3_BIN.Length < 8)
                    {
                        do
                        {
                            @IPoc3_BIN = @IPoc3_BIN.Insert(0, "0");
                        } while (@IPoc3_BIN.Length != 8);
                    }
                    if (@IPoc4_BIN.Length < 8)
                    {
                        do
                        {
                            @IPoc4_BIN = @IPoc4_BIN.Insert(0, "0");
                        } while (@IPoc4_BIN.Length != 8);
                    }
                    if (CIDR_ou_MASQUE == "MASQUE")
                    {
                        if (@m1_BIN.Length < 8)
                        {
                            do
                            {
                                @m1_BIN = @m1_BIN.Insert(0, "0");
                            } while (@m1_BIN.Length != 8);
                        }
                        if (@m2_BIN.Length < 8)
                        {
                            do
                            {
                                @m2_BIN = @m2_BIN.Insert(0, "0");
                            } while (@m2_BIN.Length != 8);
                        }
                        if (@m3_BIN.Length < 8)
                        {
                            do
                            {
                                @m3_BIN = @m3_BIN.Insert(0, "0");
                            } while (@m3_BIN.Length != 8);
                        }
                        if (@m4_BIN.Length < 8)
                        {
                            do
                            {
                                @m4_BIN = @m4_BIN.Insert(0, "0");
                            } while (@m4_BIN.Length != 8);
                        }
                    }
                    //-----------------------------------------------------------------------
                    // ET LOGIQUE @ DE RESEAU
                    //-----------------------------------------------------------------------
                    string @REoc1_BIN = "", @REoc2_BIN = "", @REoc3_BIN = "", @REoc4_BIN = "";
                    if (IPoc1_BIN != "")
                    {
                        string OCbitR1, OCbitR2, OCbitR3, OCbitR4, OCbitR5, OCbitR6, OCbitR7, OCbitR8;
                        string OCbitM1, OCbitM2, OCbitM3, OCbitM4, OCbitM5, OCbitM6, OCbitM7, OCbitM8;
                        OCbitM1 = @m1_BIN.Substring(0, 1);
                        OCbitM2 = @m1_BIN.Substring(1, 1);
                        OCbitM3 = @m1_BIN.Substring(2, 1);
                        OCbitM4 = @m1_BIN.Substring(3, 1);
                        OCbitM5 = @m1_BIN.Substring(4, 1);
                        OCbitM6 = @m1_BIN.Substring(5, 1);
                        OCbitM7 = @m1_BIN.Substring(6, 1);
                        OCbitM8 = @m1_BIN.Substring(7, 1);
                        string OCbitIP1, OCbitIP2, OCbitIP3, OCbitIP4, OCbitIP5, OCbitIP6, OCbitIP7, OCbitIP8;
                        OCbitIP1 = @IPoc1_BIN.Substring(0, 1);
                        OCbitIP2 = @IPoc1_BIN.Substring(1, 1);
                        OCbitIP3 = @IPoc1_BIN.Substring(2, 1);
                        OCbitIP4 = @IPoc1_BIN.Substring(3, 1);
                        OCbitIP5 = @IPoc1_BIN.Substring(4, 1);
                        OCbitIP6 = @IPoc1_BIN.Substring(5, 1);
                        OCbitIP7 = @IPoc1_BIN.Substring(6, 1);
                        OCbitIP8 = @IPoc1_BIN.Substring(7, 1);
                        if (OCbitIP1 == "1" && OCbitM1 == "1")
                        {
                            OCbitR1 = "1";
                        }
                        else
                        {
                            OCbitR1 = "0";
                        }
                        if (OCbitIP2 == "1" && OCbitM2 == "1")
                        {
                            OCbitR2 = "1";
                        }
                        else
                        {
                            OCbitR2 = "0";
                        }
                        if (OCbitIP3 == "1" && OCbitM3 == "1")
                        {
                            OCbitR3 = "1";
                        }
                        else
                        {
                            OCbitR3 = "0";
                        }
                        if (OCbitIP4 == "1" && OCbitM4 == "1")
                        {
                            OCbitR4 = "1";
                        }
                        else
                        {
                            OCbitR4 = "0";
                        }
                        if (OCbitIP5 == "1" && OCbitM5 == "1")
                        {
                            OCbitR5 = "1";
                        }
                        else
                        {
                            OCbitR5 = "0";
                        }
                        if (OCbitIP6 == "1" && OCbitM6 == "1")
                        {
                            OCbitR6 = "1";
                        }
                        else
                        {
                            OCbitR6 = "0";
                        }
                        if (OCbitIP7 == "1" && OCbitM7 == "1")
                        {
                            OCbitR7 = "1";
                        }
                        else
                        {
                            OCbitR7 = "0";
                        }
                        if (OCbitIP8 == "1" && OCbitM8 == "1")
                        {
                            OCbitR8 = "1";
                        }
                        else
                        {
                            OCbitR8 = "0";
                        }
                        @REoc1_BIN = @REoc1_BIN.Insert(0, OCbitR8);
                        @REoc1_BIN = @REoc1_BIN.Insert(0, OCbitR7);
                        @REoc1_BIN = @REoc1_BIN.Insert(0, OCbitR6);
                        @REoc1_BIN = @REoc1_BIN.Insert(0, OCbitR5);
                        @REoc1_BIN = @REoc1_BIN.Insert(0, OCbitR4);
                        @REoc1_BIN = @REoc1_BIN.Insert(0, OCbitR3);
                        @REoc1_BIN = @REoc1_BIN.Insert(0, OCbitR2);
                        @REoc1_BIN = @REoc1_BIN.Insert(0, OCbitR1);
                    }
                    if (IPoc2_BIN != "")
                    {
                        string OCbitR1, OCbitR2, OCbitR3, OCbitR4, OCbitR5, OCbitR6, OCbitR7, OCbitR8;
                        string OCbitM1, OCbitM2, OCbitM3, OCbitM4, OCbitM5, OCbitM6, OCbitM7, OCbitM8;
                        OCbitM1 = @m2_BIN.Substring(0, 1);
                        OCbitM2 = @m2_BIN.Substring(1, 1);
                        OCbitM3 = @m2_BIN.Substring(2, 1);
                        OCbitM4 = @m2_BIN.Substring(3, 1);
                        OCbitM5 = @m2_BIN.Substring(4, 1);
                        OCbitM6 = @m2_BIN.Substring(5, 1);
                        OCbitM7 = @m2_BIN.Substring(6, 1);
                        OCbitM8 = @m2_BIN.Substring(7, 1);
                        string OCbitIP1, OCbitIP2, OCbitIP3, OCbitIP4, OCbitIP5, OCbitIP6, OCbitIP7, OCbitIP8;
                        OCbitIP1 = @IPoc2_BIN.Substring(0, 1);
                        OCbitIP2 = @IPoc2_BIN.Substring(1, 1);
                        OCbitIP3 = @IPoc2_BIN.Substring(2, 1);
                        OCbitIP4 = @IPoc2_BIN.Substring(3, 1);
                        OCbitIP5 = @IPoc2_BIN.Substring(4, 1);
                        OCbitIP6 = @IPoc2_BIN.Substring(5, 1);
                        OCbitIP7 = @IPoc2_BIN.Substring(6, 1);
                        OCbitIP8 = @IPoc2_BIN.Substring(7, 1);
                        if (OCbitIP1 == "1" && OCbitM1 == "1")
                        {
                            OCbitR1 = "1";
                        }
                        else
                        {
                            OCbitR1 = "0";
                        }
                        if (OCbitIP2 == "1" && OCbitM2 == "1")
                        {
                            OCbitR2 = "1";
                        }
                        else
                        {
                            OCbitR2 = "0";
                        }
                        if (OCbitIP3 == "1" && OCbitM3 == "1")
                        {
                            OCbitR3 = "1";
                        }
                        else
                        {
                            OCbitR3 = "0";
                        }
                        if (OCbitIP4 == "1" && OCbitM4 == "1")
                        {
                            OCbitR4 = "1";
                        }
                        else
                        {
                            OCbitR4 = "0";
                        }
                        if (OCbitIP5 == "1" && OCbitM5 == "1")
                        {
                            OCbitR5 = "1";
                        }
                        else
                        {
                            OCbitR5 = "0";
                        }
                        if (OCbitIP6 == "1" && OCbitM6 == "1")
                        {
                            OCbitR6 = "1";
                        }
                        else
                        {
                            OCbitR6 = "0";
                        }
                        if (OCbitIP7 == "1" && OCbitM7 == "1")
                        {
                            OCbitR7 = "1";
                        }
                        else
                        {
                            OCbitR7 = "0";
                        }
                        if (OCbitIP8 == "1" && OCbitM8 == "1")
                        {
                            OCbitR8 = "1";
                        }
                        else
                        {
                            OCbitR8 = "0";
                        }
                        @REoc2_BIN = @REoc2_BIN.Insert(0, OCbitR8);
                        @REoc2_BIN = @REoc2_BIN.Insert(0, OCbitR7);
                        @REoc2_BIN = @REoc2_BIN.Insert(0, OCbitR6);
                        @REoc2_BIN = @REoc2_BIN.Insert(0, OCbitR5);
                        @REoc2_BIN = @REoc2_BIN.Insert(0, OCbitR4);
                        @REoc2_BIN = @REoc2_BIN.Insert(0, OCbitR3);
                        @REoc2_BIN = @REoc2_BIN.Insert(0, OCbitR2);
                        @REoc2_BIN = @REoc2_BIN.Insert(0, OCbitR1);
                    }
                    if (IPoc3_BIN != "")
                    {
                        string OCbitR1, OCbitR2, OCbitR3, OCbitR4, OCbitR5, OCbitR6, OCbitR7, OCbitR8;
                        string OCbitM1, OCbitM2, OCbitM3, OCbitM4, OCbitM5, OCbitM6, OCbitM7, OCbitM8;
                        OCbitM1 = @m3_BIN.Substring(0, 1);
                        OCbitM2 = @m3_BIN.Substring(1, 1);
                        OCbitM3 = @m3_BIN.Substring(2, 1);
                        OCbitM4 = @m3_BIN.Substring(3, 1);
                        OCbitM5 = @m3_BIN.Substring(4, 1);
                        OCbitM6 = @m3_BIN.Substring(5, 1);
                        OCbitM7 = @m3_BIN.Substring(6, 1);
                        OCbitM8 = @m3_BIN.Substring(7, 1);
                        string OCbitIP1, OCbitIP2, OCbitIP3, OCbitIP4, OCbitIP5, OCbitIP6, OCbitIP7, OCbitIP8;
                        OCbitIP1 = @IPoc3_BIN.Substring(0, 1);
                        OCbitIP2 = @IPoc3_BIN.Substring(1, 1);
                        OCbitIP3 = @IPoc3_BIN.Substring(2, 1);
                        OCbitIP4 = @IPoc3_BIN.Substring(3, 1);
                        OCbitIP5 = @IPoc3_BIN.Substring(4, 1);
                        OCbitIP6 = @IPoc3_BIN.Substring(5, 1);
                        OCbitIP7 = @IPoc3_BIN.Substring(6, 1);
                        OCbitIP8 = @IPoc3_BIN.Substring(7, 1);
                        if (OCbitIP1 == "1" && OCbitM1 == "1")
                        {
                            OCbitR1 = "1";
                        }
                        else
                        {
                            OCbitR1 = "0";
                        }
                        if (OCbitIP2 == "1" && OCbitM2 == "1")
                        {
                            OCbitR2 = "1";
                        }
                        else
                        {
                            OCbitR2 = "0";
                        }
                        if (OCbitIP3 == "1" && OCbitM3 == "1")
                        {
                            OCbitR3 = "1";
                        }
                        else
                        {
                            OCbitR3 = "0";
                        }
                        if (OCbitIP4 == "1" && OCbitM4 == "1")
                        {
                            OCbitR4 = "1";
                        }
                        else
                        {
                            OCbitR4 = "0";
                        }
                        if (OCbitIP5 == "1" && OCbitM5 == "1")
                        {
                            OCbitR5 = "1";
                        }
                        else
                        {
                            OCbitR5 = "0";
                        }
                        if (OCbitIP6 == "1" && OCbitM6 == "1")
                        {
                            OCbitR6 = "1";
                        }
                        else
                        {
                            OCbitR6 = "0";
                        }
                        if (OCbitIP7 == "1" && OCbitM7 == "1")
                        {
                            OCbitR7 = "1";
                        }
                        else
                        {
                            OCbitR7 = "0";
                        }
                        if (OCbitIP8 == "1" && OCbitM8 == "1")
                        {
                            OCbitR8 = "1";
                        }
                        else
                        {
                            OCbitR8 = "0";
                        }
                        @REoc3_BIN = @REoc3_BIN.Insert(0, OCbitR8);
                        @REoc3_BIN = @REoc3_BIN.Insert(0, OCbitR7);
                        @REoc3_BIN = @REoc3_BIN.Insert(0, OCbitR6);
                        @REoc3_BIN = @REoc3_BIN.Insert(0, OCbitR5);
                        @REoc3_BIN = @REoc3_BIN.Insert(0, OCbitR4);
                        @REoc3_BIN = @REoc3_BIN.Insert(0, OCbitR3);
                        @REoc3_BIN = @REoc3_BIN.Insert(0, OCbitR2);
                        @REoc3_BIN = @REoc3_BIN.Insert(0, OCbitR1);
                    }
                    if (IPoc4_BIN != "")
                    {
                        string OCbitR1, OCbitR2, OCbitR3, OCbitR4, OCbitR5, OCbitR6, OCbitR7, OCbitR8;
                        string OCbitM1, OCbitM2, OCbitM3, OCbitM4, OCbitM5, OCbitM6, OCbitM7, OCbitM8;
                        OCbitM1 = @m4_BIN.Substring(0, 1);
                        OCbitM2 = @m4_BIN.Substring(1, 1);
                        OCbitM3 = @m4_BIN.Substring(2, 1);
                        OCbitM4 = @m4_BIN.Substring(3, 1);
                        OCbitM5 = @m4_BIN.Substring(4, 1);
                        OCbitM6 = @m4_BIN.Substring(5, 1);
                        OCbitM7 = @m4_BIN.Substring(6, 1);
                        OCbitM8 = @m4_BIN.Substring(7, 1);
                        string OCbitIP1, OCbitIP2, OCbitIP3, OCbitIP4, OCbitIP5, OCbitIP6, OCbitIP7, OCbitIP8;
                        OCbitIP1 = @IPoc4_BIN.Substring(0, 1);
                        OCbitIP2 = @IPoc4_BIN.Substring(1, 1);
                        OCbitIP3 = @IPoc4_BIN.Substring(2, 1);
                        OCbitIP4 = @IPoc4_BIN.Substring(3, 1);
                        OCbitIP5 = @IPoc4_BIN.Substring(4, 1);
                        OCbitIP6 = @IPoc4_BIN.Substring(5, 1);
                        OCbitIP7 = @IPoc4_BIN.Substring(6, 1);
                        OCbitIP8 = @IPoc4_BIN.Substring(7, 1);
                        if (OCbitIP1 == "1" && OCbitM1 == "1")
                        {
                            OCbitR1 = "1";
                        }
                        else
                        {
                            OCbitR1 = "0";
                        }
                        if (OCbitIP2 == "1" && OCbitM2 == "1")
                        {
                            OCbitR2 = "1";
                        }
                        else
                        {
                            OCbitR2 = "0";
                        }
                        if (OCbitIP3 == "1" && OCbitM3 == "1")
                        {
                            OCbitR3 = "1";
                        }
                        else
                        {
                            OCbitR3 = "0";
                        }
                        if (OCbitIP4 == "1" && OCbitM4 == "1")
                        {
                            OCbitR4 = "1";
                        }
                        else
                        {
                            OCbitR4 = "0";
                        }
                        if (OCbitIP5 == "1" && OCbitM5 == "1")
                        {
                            OCbitR5 = "1";
                        }
                        else
                        {
                            OCbitR5 = "0";
                        }
                        if (OCbitIP6 == "1" && OCbitM6 == "1")
                        {
                            OCbitR6 = "1";
                        }
                        else
                        {
                            OCbitR6 = "0";
                        }
                        if (OCbitIP7 == "1" && OCbitM7 == "1")
                        {
                            OCbitR7 = "1";
                        }
                        else
                        {
                            OCbitR7 = "0";
                        }
                        if (OCbitIP8 == "1" && OCbitM8 == "1")
                        {
                            OCbitR8 = "1";
                        }
                        else
                        {
                            OCbitR8 = "0";
                        }
                        @REoc4_BIN = @REoc4_BIN.Insert(0, OCbitR8);
                        @REoc4_BIN = @REoc4_BIN.Insert(0, OCbitR7);
                        @REoc4_BIN = @REoc4_BIN.Insert(0, OCbitR6);
                        @REoc4_BIN = @REoc4_BIN.Insert(0, OCbitR5);
                        @REoc4_BIN = @REoc4_BIN.Insert(0, OCbitR4);
                        @REoc4_BIN = @REoc4_BIN.Insert(0, OCbitR3);
                        @REoc4_BIN = @REoc4_BIN.Insert(0, OCbitR2);
                        @REoc4_BIN = @REoc4_BIN.Insert(0, OCbitR1);
                    }
                    //-----------------------------------------------------------------------
                    // AFFICHAGE @ DE RESEAU
                    //-----------------------------------------------------------------------
                    string @REoc1 = Convert.ToInt32(@REoc1_BIN, 2).ToString();
                    string @REoc2 = Convert.ToInt32(@REoc2_BIN, 2).ToString();
                    string @REoc3 = Convert.ToInt32(@REoc3_BIN, 2).ToString();
                    string @REoc4 = Convert.ToInt32(@REoc4_BIN, 2).ToString();
                    string @ReseauxEntier = @REoc1 + "." + @REoc2 + "." + @REoc3 + "." + @REoc4;
                    string @IPentier = @IPoc1 + "." + @IPoc2 + "." + @IPoc3 + "." + @IPoc4;
                    txtReseau.Text = @ReseauxEntier;
                    txtAdr.Text = @IPentier;
                    //-----------------------------------------------------------------------
                    // AFFICHAGE DU ET LOGIQUE
                    //-----------------------------------------------------------------------
                    string @ReseauxEntierEnBIN = @REoc1_BIN + "." + @REoc2_BIN + "." + @REoc3_BIN + "." + @REoc4_BIN;
                    string @IPEnBIN = @IPoc1_BIN + "." + @IPoc2_BIN + "." + @IPoc3_BIN + "." + @IPoc4_BIN;
                    string @MasqueEnBIN = @m1_BIN + "." + @m2_BIN + "." + @m3_BIN + "." + @m4_BIN;
                    txtBinAdresse.Text = @IPEnBIN;
                    txtBinMasque.Text = @MasqueEnBIN;
                    txtBinReseaux.Text = @ReseauxEntierEnBIN;
                    //-----------------------------------------------------------------------
                    // NOMBRE D'HOTES
                    //-----------------------------------------------------------------------
                    string @MasqueEntier_BIN = @m1_BIN + @m2_BIN + @m3_BIN + @m4_BIN;
                    int @M_NombreDe1 = 0, @M_NombreDe0 = 0, nombreHotes = 0;
                    @M_NombreDe0 = MasqueEntier_BIN.Count(ch => ch == '0');
                    @M_NombreDe1 = @MasqueEntier_BIN.Count(ch => ch == '1');
                    if (@M_NombreDe1 == 1)
                    {
                        txtNbHote.Text = "2147483646";
                    }
                    else if (@M_NombreDe1 == 32)
                    {
                        txtNbHote.Text = "0";
                    }
                    else
                    {
                        @M_NombreDe0 = 32 - @M_NombreDe1;
                        nombreHotes = Convert.ToInt32(Math.Pow(2, @M_NombreDe0)) - 2;
                        txtNbHote.Text = nombreHotes.ToString();
                    }
                    //-----------------------------------------------------------------------
                    // @ CIDR
                    //-----------------------------------------------------------------------
                    txtCIDR.Text = @M_NombreDe1.ToString();
                    //-----------------------------------------------------------------------
                    // @ DE BROADCAST
                    //-----------------------------------------------------------------------
                    string @ReseauxEntier_BIN = @REoc1_BIN + @REoc2_BIN + @REoc3_BIN + @REoc4_BIN;
                    string BitHote;
                    string @DeBroadCast_BIN = "";
                    string BitRE = @ReseauxEntier_BIN.Substring(0, @M_NombreDe1);
                    BitHote = @MasqueEntier_BIN.Replace("1", "");
                    BitHote = BitHote.Replace("0", "1");
                    @DeBroadCast_BIN = @DeBroadCast_BIN.Insert(0, BitHote);
                    @DeBroadCast_BIN = @DeBroadCast_BIN.Insert(0, BitRE);
                    string @BCoc1_BIN = @DeBroadCast_BIN.Substring(0, 8);
                    string @BCoc2_BIN = @DeBroadCast_BIN.Substring(8, 8);
                    string @BCoc3_BIN = @DeBroadCast_BIN.Substring(16, 8);
                    string @BCoc4_BIN = @DeBroadCast_BIN.Substring(24, 8);
                    string @BCoc1 = Convert.ToInt32(@BCoc1_BIN, 2).ToString();
                    string @BCoc2 = Convert.ToInt32(@BCoc2_BIN, 2).ToString();
                    string @BCoc3 = Convert.ToInt32(@BCoc3_BIN, 2).ToString();
                    string @BCoc4 = Convert.ToInt32(@BCoc4_BIN, 2).ToString();
                    string @BroadCastEntiere = @BCoc1 + "." + @BCoc2 + "." + @BCoc3 + "." + @BCoc4;
                    txtBroadCast.Text = @BroadCastEntiere;
                    txtBinBroadCast.Text = @BCoc1_BIN + "." + @BCoc2_BIN + "." + @BCoc3_BIN + "." + @BCoc4_BIN;
                    //-----------------------------------------------------------------------
                    // PLAGE D'@
                    //-----------------------------------------------------------------------
                    string plageMin_BIN = @REoc4_BIN.Remove(@REoc4_BIN.Length - 1);
                    string plageMax_BIN = @BCoc4_BIN.Remove(@BCoc4_BIN.Length - 1);
                    plageMin_BIN = plageMin_BIN.Insert(7, "1");
                    plageMax_BIN = plageMax_BIN.Insert(7, "0");
                    string premierHote = Convert.ToInt32(plageMin_BIN, 2).ToString();
                    string dernierHote = Convert.ToInt32(plageMax_BIN, 2).ToString();
                    string @premierHoteduR = @REoc1 + "." + @REoc2 + "." + @REoc3 + "." + premierHote;
                    string @dernierHoteduR = @BCoc1 + "." + @BCoc2 + "." + @BCoc3 + "." + dernierHote;
                    txtDernierHote.Text = @dernierHoteduR;
                    txtPremierHote.Text = @premierHoteduR;
                    //-----------------------------------------------------------------------
                    // LES SOUS RESEAUX
                    //-----------------------------------------------------------------------
                    if (CLASSEdeMasque == "CIDR"||CLASSEdeMasque =="CLASSLESS")
                    {
                        if (@M_NombreDe1 == 8 || @M_NombreDe1 == 16 || @M_NombreDe1 == 24 || @M_NombreDe1 == 32)
                        {
                            txtSR.Text = "0";
                        }
                        else
                        {
                            int nbDe1SR;
                            if (@M_NombreDe1 < 32 && @M_NombreDe1 > 24)
                            {
                                nbDe1SR = @M_NombreDe1 - 24;
                            }
                            else if (@M_NombreDe1 < 24 && @M_NombreDe1 > 16)
                            {
                                nbDe1SR = @M_NombreDe1 - 16;
                            }
                            else if (@M_NombreDe1 < 16 && @M_NombreDe1 > 8)
                            {
                                nbDe1SR = @M_NombreDe1 - 8;
                            }
                            else
                            {
                                nbDe1SR = @M_NombreDe1;
                            }
                            int nombreDeSR = Convert.ToInt32(Math.Pow(2, nbDe1SR));
                            txtSR.Text = nombreDeSR.ToString();
                        }
                    }
                    else if (CLASSEdeMasque == "B")
                    {
                        if (@M_NombreDe1 == 16)
                        {
                            txtSR.Text = "0";
                        }
                        else if (@M_NombreDe0 == 0)
                        {
                            txtSR.Text = "65536";
                        }
                        else
                        {
                            int nbDe1SR;
                            nbDe1SR = (@M_NombreDe1 - 16);
                            int nombreDeSR = Convert.ToInt32(Math.Pow(2, nbDe1SR));
                            txtSR.Text = nombreDeSR.ToString();
                        }
                    }
                    else if (CLASSEdeMasque == "C")
                    {
                        if (@M_NombreDe1 == 24)
                        {
                            txtSR.Text = "0";
                        }
                        else if (@M_NombreDe0 == 0)
                        {
                            txtSR.Text = "256";
                        }
                        else
                        {
                            int nbDe1SR;
                            nbDe1SR = (@M_NombreDe1 - 24);
                            int nombreDeSR = Convert.ToInt32(Math.Pow(2, nbDe1SR));
                            txtSR.Text = nombreDeSR.ToString();
                        }
                    }
                    else if (CLASSEdeMasque == "A")
                    {
                        if (@M_NombreDe1 == 8)
                        {
                            txtSR.Text = "0";
                        }
                        else if (@M_NombreDe0 == 0)
                        {
                            txtSR.Text = "16777216‬";
                        }
                        else
                        {
                            int nbDe1SR;
                            nbDe1SR = (@M_NombreDe1 - 8);
                            int nombreDeSR = Convert.ToInt32(Math.Pow(2, nbDe1SR));
                            txtSR.Text = nombreDeSR.ToString();
                        }
                    }
                }
            }
        }
        //-----------------------------------------------------------------------
        // BOUTON QUITTER
        //-----------------------------------------------------------------------
        private void BtnQuitter_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        //-----------------------------------------------------------------------
        // BOUTON EXPORT
        //-----------------------------------------------------------------------
        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            string nomtxt = "A_" + txtAdr.Text + " R_" + txtReseau.Text + " CIDR_" + txtCIDR.Text;
            StreamWriter file = new StreamWriter(@"D:\Travail\IPv4\" + nomtxt + ".txt");
            MessageBox.Show("Le fichier a bien été créé dans l'emplacement spécifié!", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
            file.Write("IP : " + txtAdr.Text + '\n' + "Masque : " + txtMasque.Text + '\n' + "Réseau : " + txtReseau.Text + " / " + txtCIDR.Text + '\n' + "BroadCast : " + txtBroadCast.Text + '\n' + "Premiere machine : " + txtPremierHote.Text + '\n' + "Derniere machine : " + txtDernierHote.Text + '\n' + "Nb Hotes : " + txtNbHote.Text + " x " + txtSR.Text);
            file.Close();
        }
        //-----------------------------------------------------------------------
        // BOUTON RESET
        //-----------------------------------------------------------------------
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            txtSR.Text = "";
            msgERR = "";
            txtIP.Text = "";
            txtAdr.Text = "";
            txtBinAdresse.Text = "";
            txtBinMasque.Text = "";
            txtBinReseaux.Text = "";
            txtBinBroadCast.Text = "";
            txtBroadCast.Text = "";
            txtCIDR.Text = "";
            txtCIDRtoRun.Text = "";
            txtDernierHote.Text = "";
            txtMasque.Text = "";
            txtNbHote.Text = "";
            txtPremierHote.Text = "";
            txtReseau.Text = "";
        }
    }
}