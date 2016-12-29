using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace pagadiario.NET {
    public partial class FrmMenu : Form {

        static internal FrmMenu frmMenu; 

        public FrmMenu() {
            FrmMenu.frmMenu = this; 
            InitializeComponent();
        }

        #region "configuracion"
        private void cliente_Click(object sender, EventArgs e) {
            FrmCliente frmCliente = new FrmCliente();
            frmCliente.MdiParent = this; 
            frmCliente.Show(); 
        }

        private void cobrador_Click(object sender, EventArgs e) {
            FrmCobrador frmCobrador = new FrmCobrador();
            frmCobrador.MdiParent = this;
            frmCobrador.Show(); 
        }

        private void prestamo_Click(object sender, EventArgs e) {
            FrmPrestamo frmPrestamo = new FrmPrestamo();
            frmPrestamo.MdiParent = this;
            frmPrestamo.Show();
        }
                
        private void salir_Click(object sender, EventArgs e) {
            
        }
        #endregion 

        private void FrmMenu_Load(object sender, EventArgs e)
        {
            FrmLogin frmLogin = new FrmLogin();
            this.toolStripStatusLabel1.Text = "Uuario: " + frmLogin.txtUsuario.Text;
            this.toolStripStatusLabel2.Text = "Hora: " + DateTime.Now;
                  
        }

        private void principalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmPrincipal frmPrincipal = new FrmPrincipal();
            frmPrincipal.MdiParent = this;
            frmPrincipal.Show(); 
        }

        private void ingresosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCobroDiario frmCobroDiario = new FrmCobroDiario();
            frmCobroDiario.MdiParent = this; 
            frmCobroDiario.Show(); 
        }

        private void diarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmBalanceDiario frmBalanceDiario = new FrmBalanceDiario();
            frmBalanceDiario.MdiParent = this;
            frmBalanceDiario.Show();
        }

        private void mensualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmBalanceMensual frmBalanceMensual = new FrmBalanceMensual();
            frmBalanceMensual.MdiParent = this;
            frmBalanceMensual.Show(); 
        }

        private void alDiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmClientesAlDia frmClientesAlDia = new FrmClientesAlDia();
            frmClientesAlDia.MdiParent = this;
            frmClientesAlDia.Show(); 
        }

        private void enMoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmClientesEnMora frmClientesEnMora = new FrmClientesEnMora();
            frmClientesEnMora.MdiParent = this;
            frmClientesEnMora.Show();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Esta seguro que desea salir de la aplicacion", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.ExitThread();
            }
        }

        private void abonosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAbonos frmAbonos = new FrmAbonos();
            frmAbonos.MdiParent = this;
            frmAbonos.Show();
        }

        private void limpiarTodoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Desea eliminar todos los cobradores, clientes, prestamos y abonos", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                OleDbConnection conexion = new OleDbConnection(Conexion.conectar());
                OleDbCommand comandoAbono = new OleDbCommand("DELETE * FROM ABONO", conexion);
                OleDbCommand comandoPrestamo = new OleDbCommand("DELETE * FROM PRESTAMO", conexion);
                OleDbCommand comandoCliente = new OleDbCommand("DELETE * FROM CLIENTE", conexion);
                OleDbCommand comandoCobrador = new OleDbCommand("DELETE * FROM COBRADOR", conexion);

                conexion.Open();
                int abono = comandoAbono.ExecuteNonQuery();
                int prestamo = comandoPrestamo.ExecuteNonQuery();
                int cliente = comandoCliente.ExecuteNonQuery();
                int cobrador = comandoCobrador.ExecuteNonQuery();
                conexion.Close();

                MessageBox.Show(abono + " Abonos eliminados");
                MessageBox.Show(prestamo + " Prestamos eliminados");
                MessageBox.Show(cliente + " Clientes eliminados");
                MessageBox.Show(cobrador + " Cobradores eliminados");
            }

        }
    }//end class
}//end namespace
