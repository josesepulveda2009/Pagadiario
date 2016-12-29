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
    public partial class FrmPrestamo : Form
    {
        static internal FrmPrestamo frmPrestamo;

        OleDbConnection conexion;
        internal OleDbDataAdapter adaptador;
        OleDbCommandBuilder builder;
        internal DataSet datos;
        BindingManagerBase bmb; 

        public FrmPrestamo()
        {
            FrmPrestamo.frmPrestamo = this;
            InitializeComponent();
        }

        private void FrmPrestamo_Load(object sender, EventArgs e)
        {
            string sqlPrestamo = "SELECT cedulaCobrador, cedulaCliente, nombres, apellidos, telefono, celular, direccion, P.prestamo, fecha, fechaLimite, DATEDIFF('d',fecha,fechalimite) AS tiempo, cantidad, recargo, formaPago, total / tiempo AS cuota_diaria, total, SUM(abono) AS totalAbono, (total-totalAbono) AS saldo FROM (CLIENTE AS C LEFT JOIN PRESTAMO AS P ON C.cedula=P.cedulaCliente) LEFT JOIN ABONO AS A ON P.prestamo=A.prestamo GROUP BY cedulaCobrador, cedulaCliente, nombres, apellidos, telefono, celular, direccion, P.prestamo, fecha, cantidad, recargo, formaPago, fechaLimite, total HAVING total> 0; ";

            conexion = new OleDbConnection(Conexion.conectar());
            adaptador = new OleDbDataAdapter(sqlPrestamo, conexion);
            builder = new OleDbCommandBuilder(adaptador);

            OleDbCommand insertar = new OleDbCommand("INSERT INTO PRESTAMO(prestamo, fecha, cantidad, recargo, formaPago, fechaLimite, cedulaCliente, cedulaCobrador, total) VALUES(@prestamo, @fecha, @cantidad, @recargo, @formaPago, @fechaLimite, @cedulaCliente, @cedulaCobrador, @total)", conexion);
            adaptador.InsertCommand = insertar;
            adaptador.InsertCommand.Parameters.Add(new OleDbParameter("@prestamo", OleDbType.VarChar));
            adaptador.InsertCommand.Parameters.Add(new OleDbParameter("@fecha", OleDbType.VarChar));
            adaptador.InsertCommand.Parameters.Add(new OleDbParameter("@cantidad", OleDbType.VarChar));
            adaptador.InsertCommand.Parameters.Add(new OleDbParameter("@recargo", OleDbType.VarChar));
            adaptador.InsertCommand.Parameters.Add(new OleDbParameter("@formaPago", OleDbType.VarChar));
            adaptador.InsertCommand.Parameters.Add(new OleDbParameter("@fechaLimite", OleDbType.VarChar));
            adaptador.InsertCommand.Parameters.Add(new OleDbParameter("@cedulaCliente", OleDbType.VarChar));
            adaptador.InsertCommand.Parameters.Add(new OleDbParameter("@cedulaCobrador", OleDbType.VarChar));
            adaptador.InsertCommand.Parameters.Add(new OleDbParameter("@total", OleDbType.VarChar));

            datos = new DataSet();
                        
            conexion.Open();
            adaptador.Fill(datos);
            conexion.Close();
                        
            txtCedulaCliente.DataBindings.Add(new Binding("Text", datos.Tables[0], "cedulaCliente"));
            txtCedulaCobrador.DataBindings.Add(new Binding("Text", datos.Tables[0], "cedulaCobrador"));
            txtNombres.DataBindings.Add(new Binding("Text", datos.Tables[0], "nombres"));
            txtApellidos.DataBindings.Add(new Binding("Text", datos.Tables[0], "apellidos"));
            txtTelefono.DataBindings.Add(new Binding("Text", datos.Tables[0], "telefono"));
            txtCelular.DataBindings.Add(new Binding("Text", datos.Tables[0], "celular"));
            txtDireccion.DataBindings.Add(new Binding("Text", datos.Tables[0], "direccion"));
            txtPrestamo.DataBindings.Add(new Binding("Text", datos.Tables[0], "prestamo"));
            dtpFecha.DataBindings.Add(new Binding("Text", datos.Tables[0], "fecha"));
            txtCantidad.DataBindings.Add(new Binding("Text", datos.Tables[0], "cantidad"));
            txtRecargo.DataBindings.Add(new Binding("Text", datos.Tables[0], "recargo"));
            cboFormaPago.DataBindings.Add(new Binding("Text", datos.Tables[0], "formaPago"));
            dtpFechaLimite.DataBindings.Add(new Binding("Text", datos.Tables[0], "fechaLimite"));
            txtTotal.DataBindings.Add(new Binding("Text", datos.Tables[0], "total"));
            txtTotalAbono.DataBindings.Add(new Binding("Text", datos.Tables[0], "totalAbono"));
            txtSaldo.DataBindings.Add(new Binding("Text", datos.Tables[0], "saldo"));

            dgvPrestamo.DataSource = datos.Tables[0];

            bmb = BindingContext[datos.Tables[0]];

            registro();
        }

        private string registro()
        {
            txtRegistro.Text = "Registro " + (bmb.Position+1) + " de " + (datos.Tables[0].Rows.Count);
            return txtRegistro.Text;
        }

        private void btnPri_Click(object sender, EventArgs e)
        {
            bmb.Position = 0;
            registro();
        }

        private void btnAnt_Click(object sender, EventArgs e)
        {
            bmb.Position--;
            registro();
        }

        private void btnSig_Click(object sender, EventArgs e)
        {
            bmb.Position++;
            registro();
        }

        private void btnUlt_Click(object sender, EventArgs e)
        {
            bmb.Position = datos.Tables[0].Rows.Count - 1;
            registro();
        }

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            double total;
            double cantidad, recargo;

            if (txtPrestamo.Text == "" || txtCantidad.Text == "" || txtRecargo.Text == "" || txtCedulaCliente.Text == "" || txtCedulaCobrador.Text == "" || txtCantidad.Text == "0" || txtRecargo.Text == "0")
            {
                MessageBox.Show("No pueden quedar campos vacios", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    cantidad = Convert.ToDouble(txtCantidad.Text);
                    recargo = Convert.ToDouble(txtRecargo.Text);
                    total = cantidad + (cantidad * (recargo / 100));

                    txtTotal.Text = total.ToString();

                    adaptador.InsertCommand.Parameters["@prestamo"].Value = txtPrestamo.Text;
                    adaptador.InsertCommand.Parameters["@fecha"].Value = dtpFecha.Text;
                    adaptador.InsertCommand.Parameters["@cantidad"].Value = txtCantidad.Text;
                    adaptador.InsertCommand.Parameters["@recargo"].Value = txtRecargo.Text;
                    adaptador.InsertCommand.Parameters["@formaPago"].Value = cboFormaPago.Text;
                    adaptador.InsertCommand.Parameters["@fechaLimite"].Value = dtpFechaLimite.Text;
                    adaptador.InsertCommand.Parameters["@cedulaCliente"].Value = txtCedulaCliente.Text;
                    adaptador.InsertCommand.Parameters["@cedulaCobrador"].Value = txtCedulaCobrador.Text;
                    adaptador.InsertCommand.Parameters["@total"].Value = txtTotal.Text;

                    conexion.Open();
                    int i = adaptador.InsertCommand.ExecuteNonQuery();
                    conexion.Close();

                    MessageBox.Show(i + " Datos insertados", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    datos.Clear();
                    adaptador.Fill(datos);
                    dgvPrestamo.Refresh();

                    btnInsertar.Enabled = false;
                    btnNuevo.Enabled = true; 
                }
                catch (OleDbException oE)
                {
                    MessageBox.Show(oE.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            txtPrestamo.Clear();
            txtCedulaCliente.Clear();
            txtCedulaCobrador.Clear();
            txtNombres.Clear();
            txtApellidos.Clear();
            txtTelefono.Clear();
            txtCelular.Clear();
            txtCelular.Clear();
            txtDireccion.Clear();

            cboFormaPago.Text = "DIARIA";
            txtCantidad.Text = "" + 0;
            txtRecargo.Text = "" + 0; 
            txtPrestamo.Focus();

            btnInsertar.Enabled = true;
            btnNuevo.Enabled = false; 
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (TabControl1.TabPages[0].Focus())
            {
                if (dgvPrestamo.Rows.Count < 1)
                {
                    MessageBox.Show("No hay registros para actualizar", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    FrmPrestamoActualizar frmPrestamoActualizar = new FrmPrestamoActualizar();
                    frmPrestamoActualizar.Show();

                    frmPrestamoActualizar.txtPrestamo.Text = dgvPrestamo.CurrentRow.Cells["prestamo"].Value.ToString();
                    frmPrestamoActualizar.dtpFecha.Text = dgvPrestamo.CurrentRow.Cells["fecha"].Value.ToString();
                    frmPrestamoActualizar.dtpFechaLimite.Text = dgvPrestamo.CurrentRow.Cells["fechaLimite"].Value.ToString();
                    frmPrestamoActualizar.txtCantidad.Text = dgvPrestamo.CurrentRow.Cells["cantidad"].Value.ToString();
                    frmPrestamoActualizar.txtRecargo.Text = dgvPrestamo.CurrentRow.Cells["recargo"].Value.ToString();
                    frmPrestamoActualizar.cboFormaPago.Text = dgvPrestamo.CurrentRow.Cells["formaPago"].Value.ToString();
                    frmPrestamoActualizar.txtTotal.Text = dgvPrestamo.CurrentRow.Cells["total"].Value.ToString();
                    frmPrestamoActualizar.txtTotalAbono.Text = dgvPrestamo.CurrentRow.Cells["totalAbono"].Value.ToString();
                    frmPrestamoActualizar.txtSaldo.Text = dgvPrestamo.CurrentRow.Cells["saldo"].Value.ToString();
                    frmPrestamoActualizar.txtCedulaCliente.Text = dgvPrestamo.CurrentRow.Cells["cedulaCliente"].Value.ToString();
                    frmPrestamoActualizar.txtCedulaCobrador.Text = dgvPrestamo.CurrentRow.Cells["cedulaCobrador"].Value.ToString();
                }
            }
            else if (TabControl1.TabPages[1].Focus())
            {
                if (dgvPrestamoBuscar.Rows.Count < 1)
                {
                    MessageBox.Show("No hay registros para actualizar", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    FrmPrestamoActualizar frmPrestamoActualizar = new FrmPrestamoActualizar();
                    frmPrestamoActualizar.Show();

                    frmPrestamoActualizar.txtPrestamo.Text = dgvPrestamoBuscar.CurrentRow.Cells["prestamo"].Value.ToString();
                    frmPrestamoActualizar.dtpFecha.Text = dgvPrestamoBuscar.CurrentRow.Cells["fecha"].Value.ToString();
                    frmPrestamoActualizar.dtpFechaLimite.Text = dgvPrestamoBuscar.CurrentRow.Cells["fechaLimite"].Value.ToString();
                    frmPrestamoActualizar.txtCantidad.Text = dgvPrestamoBuscar.CurrentRow.Cells["cantidad"].Value.ToString();
                    frmPrestamoActualizar.txtRecargo.Text = dgvPrestamoBuscar.CurrentRow.Cells["recargo"].Value.ToString();
                    frmPrestamoActualizar.cboFormaPago.Text = dgvPrestamoBuscar.CurrentRow.Cells["formaPago"].Value.ToString();
                    frmPrestamoActualizar.txtTotal.Text = dgvPrestamoBuscar.CurrentRow.Cells["total"].Value.ToString();
                    frmPrestamoActualizar.txtTotalAbono.Text = dgvPrestamoBuscar.CurrentRow.Cells["totalAbono"].Value.ToString();
                    frmPrestamoActualizar.txtSaldo.Text = dgvPrestamoBuscar.CurrentRow.Cells["saldo"].Value.ToString();
                    frmPrestamoActualizar.txtCedulaCliente.Text = dgvPrestamoBuscar.CurrentRow.Cells["cedulaCliente"].Value.ToString();
                    frmPrestamoActualizar.txtCedulaCobrador.Text = dgvPrestamoBuscar.CurrentRow.Cells["cedulaCobrador"].Value.ToString();
                }
            }
        }

        private void btnAbonar_Click(object sender, EventArgs e)
        {
            FrmAbono1 frmAbono1 = new FrmAbono1();
            frmAbono1.Show();

            FrmAbono1.frmAbono1.txtPrestamo.Text = txtPrestamo.Text;
            FrmAbono1.frmAbono1.txtCedulaCliente.Text = txtCedulaCliente.Text;
        }

        private void btnCedulaCliente_Click(object sender, EventArgs e)
        {
            FrmClienteBuscar frmClienteBuscar = new FrmClienteBuscar();
            frmClienteBuscar.Show();
            frmClienteBuscar.btnEnviarClienteActualizar.Visible = true;
        }

        private void btnCedulaCobrador_Click(object sender, EventArgs e)
        {
            FrmCobradorBuscar frmCobradorBuscar = new FrmCobradorBuscar();
            frmCobradorBuscar.Show();
            frmCobradorBuscar.btnEnviarCobradorBuscar.Visible = true;
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string sql;
            DataTable dt;

            if (cboBuscar.Text == "Prestamo")
            {
                sql = "SELECT cedulaCobrador, cedulaCliente, nombres, apellidos, telefono, celular, direccion, P.prestamo, fecha, fechaLimite, DATEDIFF('d',fecha,fechalimite) AS tiempo, cantidad, recargo, formaPago, total / tiempo AS cuota_diaria, total, SUM(abono) AS totalAbono, (total-totalAbono) AS saldo FROM (CLIENTE AS C LEFT JOIN PRESTAMO AS P ON C.cedula=P.cedulaCliente) LEFT JOIN ABONO AS A ON P.prestamo=A.prestamo WHERE P.prestamo LIKE '%"+ txtBuscar.Text +"%' GROUP BY cedulaCobrador, cedulaCliente, nombres, apellidos, telefono, celular, direccion, P.prestamo, fecha, cantidad, recargo, formaPago, fechaLimite, total HAVING total> 0; ";
                adaptador = new OleDbDataAdapter(sql, conexion);

                dt = new DataTable();

                conexion.Open();
                adaptador.Fill(dt);
                conexion.Close();

                dgvPrestamoBuscar.DataSource = dt;
            }

            if (cboBuscar.Text == "Nombres")
            {
                sql = "SELECT cedulaCobrador, cedulaCliente, nombres, apellidos, telefono, celular, direccion, P.prestamo, fecha, fechaLimite, DATEDIFF('d',fecha,fechalimite) AS tiempo, cantidad, recargo, formaPago, total / tiempo AS cuota_diaria, total, SUM(abono) AS totalAbono, (total-totalAbono) AS saldo FROM (CLIENTE AS C LEFT JOIN PRESTAMO AS P ON C.cedula=P.cedulaCliente) LEFT JOIN ABONO AS A ON P.prestamo=A.prestamo WHERE nombres LIKE '%" + txtBuscar.Text + "%' GROUP BY cedulaCobrador, cedulaCliente, nombres, apellidos, telefono, celular, direccion, P.prestamo, fecha, cantidad, recargo, formaPago, fechaLimite, total HAVING total> 0; ";
                adaptador = new OleDbDataAdapter(sql, conexion);

                dt = new DataTable();

                conexion.Open();
                adaptador.Fill(dt);
                conexion.Close();

                dgvPrestamoBuscar.DataSource = dt;
            }

            if (cboBuscar.Text == "Apellidos")
            {
                sql = "SELECT cedulaCobrador, cedulaCliente, nombres, apellidos, telefono, celular, direccion, P.prestamo, fecha, fechaLimite, DATEDIFF('d',fecha,fechalimite) AS tiempo, cantidad, recargo, formaPago, total / tiempo AS cuota_diaria, total, SUM(abono) AS totalAbono, (total-totalAbono) AS saldo FROM (CLIENTE AS C LEFT JOIN PRESTAMO AS P ON C.cedula=P.cedulaCliente) LEFT JOIN ABONO AS A ON P.prestamo=A.prestamo WHERE apellidos LIKE '%" + txtBuscar.Text + "%' GROUP BY cedulaCobrador, cedulaCliente, nombres, apellidos, telefono, celular, direccion, P.prestamo, fecha, cantidad, recargo, formaPago, fechaLimite, total HAVING total> 0; ";
                adaptador = new OleDbDataAdapter(sql, conexion);

                dt = new DataTable();

                conexion.Open();
                adaptador.Fill(dt);
                conexion.Close();

                dgvPrestamoBuscar.DataSource = dt;
            }

            if (cboBuscar.Text == "Direccion")
            {
                sql = "SELECT cedulaCobrador, cedulaCliente, nombres, apellidos, telefono, celular, direccion, P.prestamo, fecha, fechaLimite, DATEDIFF('d',fecha,fechalimite) AS tiempo, cantidad, recargo, formaPago, total / tiempo AS cuota_diaria, total, SUM(abono) AS totalAbono, (total-totalAbono) AS saldo FROM (CLIENTE AS C LEFT JOIN PRESTAMO AS P ON C.cedula=P.cedulaCliente) LEFT JOIN ABONO AS A ON P.prestamo=A.prestamo WHERE direccion LIKE '%" + txtBuscar.Text + "%' GROUP BY cedulaCobrador, cedulaCliente, nombres, apellidos, telefono, celular, direccion, P.prestamo, fecha, cantidad, recargo, formaPago, fechaLimite, total HAVING total> 0; ";
                adaptador = new OleDbDataAdapter(sql, conexion);

                dt = new DataTable();

                conexion.Open();
                adaptador.Fill(dt);
                conexion.Close();

                dgvPrestamoBuscar.DataSource = dt;
            }

            if (cboBuscar.Text == "Cc Cliente")
            {
                sql = "SELECT cedulaCobrador, cedulaCliente, nombres, apellidos, telefono, celular, direccion, P.prestamo, fecha, fechaLimite, DATEDIFF('d',fecha,fechalimite) AS tiempo, cantidad, recargo, formaPago, total / tiempo AS cuota_diaria, total, SUM(abono) AS totalAbono, (total-totalAbono) AS saldo FROM (CLIENTE AS C LEFT JOIN PRESTAMO AS P ON C.cedula=P.cedulaCliente) LEFT JOIN ABONO AS A ON P.prestamo=A.prestamo WHERE cedulaCliente LIKE '%" + txtBuscar.Text + "%' GROUP BY cedulaCobrador, cedulaCliente, nombres, apellidos, telefono, celular, direccion, P.prestamo, fecha, cantidad, recargo, formaPago, fechaLimite, total HAVING total> 0; ";
                adaptador = new OleDbDataAdapter(sql, conexion);

                dt = new DataTable();

                conexion.Open();
                adaptador.Fill(dt);
                conexion.Close();

                dgvPrestamoBuscar.DataSource = dt;
            }

            if (cboBuscar.Text == "Cc Cobrador")
            {
                sql = "SELECT cedulaCobrador, cedulaCliente, nombres, apellidos, telefono, celular, direccion, P.prestamo, fecha, fechaLimite, DATEDIFF('d',fecha,fechalimite) AS tiempo, cantidad, recargo, formaPago, total / tiempo AS cuota_diaria, total, SUM(abono) AS totalAbono, (total-totalAbono) AS saldo FROM (CLIENTE AS C LEFT JOIN PRESTAMO AS P ON C.cedula=P.cedulaCliente) LEFT JOIN ABONO AS A ON P.prestamo=A.prestamo WHERE cedulaCobrador LIKE '%" + txtBuscar.Text + "%' GROUP BY cedulaCobrador, cedulaCliente, nombres, apellidos, telefono, celular, direccion, P.prestamo, fecha, cantidad, recargo, formaPago, fechaLimite, total HAVING total> 0; ";
                adaptador = new OleDbDataAdapter(sql, conexion);

                dt = new DataTable();

                conexion.Open();
                adaptador.Fill(dt);
                conexion.Close();

                dgvPrestamoBuscar.DataSource = dt;
            }

            if (dgvPrestamoBuscar.Rows.Count < 1)
            {
                errorProvider1.BlinkStyle = ErrorBlinkStyle.AlwaysBlink;
                errorProvider1.SetError(txtBuscar, "No hay registros");
            }
            else
            {
                errorProvider1.SetError(txtBuscar, "");
            }
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
               
        private void btnVerAbonos_Click_1(object sender, EventArgs e)
        {
            FrmAbonosPrestamo frmAbonosPrestamo = new FrmAbonosPrestamo();
            frmAbonosPrestamo.MdiParent = FrmMenu.frmMenu;
            frmAbonosPrestamo.Show();
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            ExportarExcel exportarExcel = new ExportarExcel();
            exportarExcel.Exportar(dgvPrestamo);
        }
    }//end class
}//end namespace