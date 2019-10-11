﻿using System;
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
            cmd.CommandText = "select * from Huesped";
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
            cbGenero.SelectedIndex = 0;
            txtTelefono.Text = "";
            txtDireccion.Text = "";
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
                    if (cbGenero.Text != "Selecciona Genero")
                    {
                        cmd.CommandText = "insert into Progra(NumHuesped,Nombre,Genero,Telefono,Direccion) " +
                            "Values(" + txtNumHuesped.Text + ",'" + txtNombre.Text + "','" + cbGenero.Text + "'," + txtTelefono.Text + ",'" + txtDireccion.Text + "')";
                        cmd.ExecuteNonQuery();
                        MostrarDatos();
                        MessageBox.Show("Alumno agregado correctamente...");
                        LimpiaTodo();

                    }
                    else
                    {
                        MessageBox.Show("Favor de seleccionar el genero....");
                    }
                }
                else
                {
                    cmd.CommandText = "update Progra set Nombre='" + txtNombre.Text + "',Genero='" + cbGenero.Text + "',Telefono=" + txtTelefono.Text
                        + ",Direccion='" + txtDireccion.Text + "' where NumHuesped=" + txtNumHuesped.Text;
                    cmd.ExecuteNonQuery();
                    MostrarDatos();
                    MessageBox.Show("Datos del alumno Actualizados...");
                    LimpiaTodo();
                }
            }
            else
            {
                MessageBox.Show("Favor de poner el ID de un Alumno.......");
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)gvDatos.SelectedItems[0];
            txtNumHuesped.Text = row["Id"].ToString();
            txtNombre.Text = row["Nombre"].ToString();
            cbGenero.Text = row["Genero"].ToString();
            txtDireccion.Text = row["Direccion"].ToString();
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
                cmd.CommandText = "delete from Progra where Id= " + row["Id"].ToString();//Es importante poner el where porque si no borraras toda la base de datos
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
    }
}