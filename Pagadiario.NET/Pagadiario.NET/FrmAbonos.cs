using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace pagadiario.NET
{
    public partial class FrmAbonos : Form
    {
        static internal FrmAbonos frmAbonos;

        OleDbConnection conexion;
        internal OleDbDataAdapter adaptador;
        OleDbCommandBuilder builder;
        internal DataSet datos;
        BindingManagerBase bmb;

        public FrmAbonos()
        {
            FrmAbonos.frmAbonos = this;
            InitializeComponent();
        }

        private void FrmAbonos_Load(object sender, EventArgs e)
        {
            string sqlPrestamo = "SELECT prestamo, abono, fechaAbono, proximoPago FROM ABONO";

            conexion = new OleDbConnection(Conexion.conectar());
            adaptador = new OleDbDataAdapter(sqlPrestamo, conexion);
            builder = new OleDbCommandBuilder(adaptador);

            datos = new DataSet();

            conexion.Open();
            adaptador.Fill(datos);
            conexion.Close();

            dgvAbono.DataSource = datos.Tables[0];
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            ExportarExcel exportarExcel = new ExportarExcel();
            exportarExcel.Exportar(dgvAbono);
        }
    }
}
