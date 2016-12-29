namespace pagadiario.NET
{
    partial class FrmAbonos
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvAbono = new System.Windows.Forms.DataGridView();
            this.btnExportar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAbono)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvAbono
            // 
            this.dgvAbono.AllowUserToAddRows = false;
            this.dgvAbono.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAbono.Location = new System.Drawing.Point(12, 12);
            this.dgvAbono.Name = "dgvAbono";
            this.dgvAbono.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAbono.Size = new System.Drawing.Size(489, 418);
            this.dgvAbono.TabIndex = 0;
            // 
            // btnExportar
            // 
            this.btnExportar.Location = new System.Drawing.Point(428, 438);
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Size = new System.Drawing.Size(75, 23);
            this.btnExportar.TabIndex = 85;
            this.btnExportar.Text = "&Excel";
            this.btnExportar.UseVisualStyleBackColor = true;
            this.btnExportar.Click += new System.EventHandler(this.btnExportar_Click);
            // 
            // FrmAbonos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 473);
            this.Controls.Add(this.btnExportar);
            this.Controls.Add(this.dgvAbono);
            this.Name = "FrmAbonos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ABONOS TODOS";
            this.Load += new System.EventHandler(this.FrmAbonos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAbono)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvAbono;
        private System.Windows.Forms.Button btnExportar;
    }
}