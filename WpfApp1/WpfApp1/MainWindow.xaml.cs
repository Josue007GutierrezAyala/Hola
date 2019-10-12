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
using System.Data.OleDb;//Agregamos libreria OleDB
using System.Data; //Agregamos System.Data

namespace WPFDBParte2
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OleDbConnection con; //Agregamos la conexión
        DataTable dt;   //Agregamos la tabla
        public MainWindow()
        {
            InitializeComponent();
            //Conectamos la base de datos a nuestro archivo Access
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\Reservaciones.mdb";
            MostrarDatos();
        }
        //Mostramos los registros de la tabla
        private void MostrarDatos()
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select * from Huespedes";
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            gvDatos.ItemsSource = dt.AsDataView();

            if (dt.Rows.Count > 0)
            {
                lbContenido.Visibility = System.Windows.Visibility.Hidden;
                gvDatos.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                lbContenido.Visibility = System.Windows.Visibility.Visible;
                gvDatos.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        //Limpiamos todos los campos
        private void LimpiaTodo()
        {
            txtNumHuesped.Text = "";
            txtNombre.Text = "";
            cbFormaPago.SelectedIndex = 0;
            cbZonasVIP.SelectedIndex = 0;
            txtNumTelef.Text = "";
            txtNumHabita.Text = "";
            txtFechaIni.Text = "";
            txtFechaSali.Text = "";
            btnNuevo.Content = "Nuevo";
            txtNumHuesped.IsEnabled = true;
        }
        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;

            if (txtNumHuesped.Text != "")
            {
                if (txtNumHuesped.IsEnabled == true)
                {
                    if (cbFormaPago.Text != "Selecciona Forma de Pago")
                    {
                        cmd.CommandText = "insert into Huespedes(NumHuesped,Nombre,NumDias,FormaPago,ZonasVIP,NumTelef,NumHabita,FechaIni,FechaSali) " +
                            "Values(" + txtNumHuesped.Text + "','" + txtNombre.Text + "','" + txtNumDias.Text + "','" + cbFormaPago.Text + "','" + cbZonasVIP.Text + "', '" + txtNumTelef.Text + "','" + txtNumHabita.Text + "''" + txtFechaIni.Text + "''" + txtFechaSali.Text + "')";
                        cmd.ExecuteNonQuery();
                        MostrarDatos();
                        MessageBox.Show("El huesped ha sido agregado correctamente...");
                        LimpiaTodo();

                    }
                    else
                    {
                        MessageBox.Show("Favor de seleccionar la forma de pago....");
                    }
                }
                else
                {
                    cmd.CommandText = "update Huespedes set Nombre='"+ txtNumHuesped.Text + "',NumHuesped='"+ txtNombre.Text + "',NumDias='" + txtNumDias.Text + "',FormaPago='" + cbFormaPago.Text + "',ZonasVIP='" + cbZonasVIP.Text + "',NumTelef=" + txtNumTelef.Text
                        + ",NumHabita='" + txtNumHabita.Text  +"'FechaIni="+ txtFechaIni.Text + "'FechaSali=" + txtFechaSali.Text + "' where NumHuesped=" + txtNumHuesped.Text;
                    cmd.ExecuteNonQuery();
                    MostrarDatos();
                    MessageBox.Show("Datos del huespedes Actualizados...");
                    LimpiaTodo();
                }
            }
            else
            {
                MessageBox.Show("Favor de poner el Numero del cliente porfavor.......");
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)gvDatos.SelectedItems[0];
            txtNumHuesped.Text = row["NumHuesped"].ToString();
            txtNombre.Text = row["Nombre"].ToString();
            txtNumDias.Text = row["NumDias"].ToString();
            cbFormaPago.Text = row["FormaPago"].ToString();
            cbZonasVIP.Text = row["ZonasVIP"].ToString();
            txtNumTelef.Text = row["NumTelef"].ToString();
            txtNumHabita.Text = row["NumHabita"].ToString();
            txtFechaIni.Text = row[" FechaIni"].ToString();
            txtFechaSali.Text = row[" FechaSali"].ToString();
            txtNumHuesped.IsEnabled = false;
            btnNuevo.Content = "Actualizar";
           
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (gvDatos.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0];
                OleDbCommand cmd = new OleDbCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.Connection = con;
                cmd.CommandText = "delete from Huespedes where NumHuesped= " + row["NumHuesped"].ToString();//Es importante poner el where porque si no borraras toda la base de datos
                cmd.ExecuteNonQuery();
                MostrarDatos();
                MessageBox.Show("Alumno eliminado correctamente.....");
                LimpiaTodo();
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiaTodo();
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CbFormaPago_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
         // <TextBlock Text = "Numero de Habitacion :                   " Width="205"/>            Sirven para dar anchura a un textBox
        //  <TextBox Name = "txtNumHabita" TextWrapping="Wrap" AcceptsReturn="True" Height="75" />
    }
}