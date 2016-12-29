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
    public partial class FrmAbonosPrestamo : Form
    {
        static internal FrmAbonosPrestamo frmAbonosPrestamo;

        OleDbConnection conexion;
        internal OleDbDataAdapter adaptador;
        OleDbCommandBuilder builder;
        internal DataSet datos;
        BindingManagerBase bmb; 

        public FrmAbonosPrestamo()
        {
            FrmAbonosPrestamo.frmAbonosPrestamo = this;
            InitializeComponent();
        }

        private void FrmAbonosPrestamo_Load(object sender, EventArgs e)
        {
            string sqlPrestamo = "SELECT prestamo, abono, fechaAbono, proximoPago FROM ABONO WHERE prestamo = '"+FrmPrestamo.frmPrestamo.txtPrestamo.Text+"' ";

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
