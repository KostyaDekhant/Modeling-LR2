namespace Modeling_LR2
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            grid_inp = new DataGridView();
            inp_panel = new Panel();
            out_panel = new Panel();
            grid_psl = new DataGridView();
            grid_orders = new DataGridView();
            start_time_lb = new Label();
            tozh_lb = new Label();
            tpr_lb = new Label();
            label1 = new Label();
            grid_out = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)grid_inp).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grid_psl).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grid_orders).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grid_out).BeginInit();
            SuspendLayout();
            // 
            // grid_inp
            // 
            grid_inp.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            grid_inp.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grid_inp.Location = new Point(12, 12);
            grid_inp.Name = "grid_inp";
            grid_inp.RowHeadersWidth = 51;
            grid_inp.Size = new Size(275, 188);
            grid_inp.TabIndex = 0;
            // 
            // inp_panel
            // 
            inp_panel.BackColor = Color.FromArgb(234, 229, 249);
            inp_panel.Location = new Point(12, 335);
            inp_panel.Name = "inp_panel";
            inp_panel.Size = new Size(1878, 340);
            inp_panel.TabIndex = 1;
            // 
            // out_panel
            // 
            out_panel.BackColor = Color.FromArgb(234, 229, 249);
            out_panel.Location = new Point(12, 681);
            out_panel.Name = "out_panel";
            out_panel.Size = new Size(1878, 340);
            out_panel.TabIndex = 2;
            // 
            // grid_psl
            // 
            grid_psl.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            grid_psl.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grid_psl.Location = new Point(293, 12);
            grid_psl.Name = "grid_psl";
            grid_psl.RowHeadersWidth = 51;
            grid_psl.Size = new Size(148, 188);
            grid_psl.TabIndex = 3;
            // 
            // grid_orders
            // 
            grid_orders.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            grid_orders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grid_orders.Location = new Point(447, 12);
            grid_orders.Name = "grid_orders";
            grid_orders.RowHeadersWidth = 51;
            grid_orders.Size = new Size(186, 188);
            grid_orders.TabIndex = 4;
            // 
            // start_time_lb
            // 
            start_time_lb.AutoSize = true;
            start_time_lb.Location = new Point(1683, 12);
            start_time_lb.Name = "start_time_lb";
            start_time_lb.Size = new Size(0, 20);
            start_time_lb.TabIndex = 5;
            // 
            // tozh_lb
            // 
            tozh_lb.AutoSize = true;
            tozh_lb.Location = new Point(1529, 12);
            tozh_lb.Name = "tozh_lb";
            tozh_lb.Size = new Size(0, 20);
            tozh_lb.TabIndex = 6;
            // 
            // tpr_lb
            // 
            tpr_lb.AutoSize = true;
            tpr_lb.Location = new Point(1368, 12);
            tpr_lb.Name = "tpr_lb";
            tpr_lb.Size = new Size(0, 20);
            tpr_lb.TabIndex = 7;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(1109, 12);
            label1.Name = "label1";
            label1.Size = new Size(230, 80);
            label1.TabIndex = 8;
            label1.Text = "Стартовая последовательность:\r\nОптимальная по Петрову:\r\nРандомная:\r\nПолный перебор:";
            label1.TextAlign = ContentAlignment.TopRight;
            // 
            // grid_out
            // 
            grid_out.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            grid_out.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grid_out.Location = new Point(694, 12);
            grid_out.Name = "grid_out";
            grid_out.RowHeadersWidth = 51;
            grid_out.Size = new Size(275, 188);
            grid_out.TabIndex = 9;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1902, 1033);
            Controls.Add(grid_out);
            Controls.Add(label1);
            Controls.Add(tpr_lb);
            Controls.Add(tozh_lb);
            Controls.Add(start_time_lb);
            Controls.Add(grid_orders);
            Controls.Add(grid_psl);
            Controls.Add(out_panel);
            Controls.Add(inp_panel);
            Controls.Add(grid_inp);
            Name = "Form1";
            Text = "Form1";
            WindowState = FormWindowState.Maximized;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)grid_inp).EndInit();
            ((System.ComponentModel.ISupportInitialize)grid_psl).EndInit();
            ((System.ComponentModel.ISupportInitialize)grid_orders).EndInit();
            ((System.ComponentModel.ISupportInitialize)grid_out).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView grid_inp;
        private Panel inp_panel;
        private Panel out_panel;
        private DataGridView grid_psl;
        private DataGridView grid_orders;
        private Label start_time_lb;
        private Label tozh_lb;
        private Label tpr_lb;
        private Label label1;
        private DataGridView grid_out;
    }
}
