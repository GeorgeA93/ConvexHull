namespace ConvexHull
{
    partial class convexHull
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
            this.compute = new System.Windows.Forms.Button();
            this.clear = new System.Windows.Forms.Button();
            this.dcCompute = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // compute
            // 
            this.compute.Location = new System.Drawing.Point(12, 384);
            this.compute.Name = "compute";
            this.compute.Size = new System.Drawing.Size(75, 23);
            this.compute.TabIndex = 0;
            this.compute.Text = "Compute";
            this.compute.UseVisualStyleBackColor = true;
            this.compute.Click += new System.EventHandler(this.compute_Click);
            // 
            // clear
            // 
            this.clear.Location = new System.Drawing.Point(594, 384);
            this.clear.Name = "clear";
            this.clear.Size = new System.Drawing.Size(75, 23);
            this.clear.TabIndex = 1;
            this.clear.Text = "Clear";
            this.clear.UseVisualStyleBackColor = true;
            this.clear.Click += new System.EventHandler(this.clear_Click);
            // 
            // dcCompute
            // 
            this.dcCompute.Location = new System.Drawing.Point(93, 384);
            this.dcCompute.Name = "dcCompute";
            this.dcCompute.Size = new System.Drawing.Size(133, 23);
            this.dcCompute.TabIndex = 2;
            this.dcCompute.Text = "Compute Using DC";
            this.dcCompute.UseVisualStyleBackColor = true;
            this.dcCompute.Click += new System.EventHandler(this.dcCompute_Click);
            // 
            // convexHull
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(681, 419);
            this.Controls.Add(this.dcCompute);
            this.Controls.Add(this.clear);
            this.Controls.Add(this.compute);
            this.Name = "convexHull";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Convex Hull";
            this.Load += new System.EventHandler(this.convexHull_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.convexHull_MouseDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button compute;
        private System.Windows.Forms.Button clear;
        private System.Windows.Forms.Button dcCompute;
    }
}

