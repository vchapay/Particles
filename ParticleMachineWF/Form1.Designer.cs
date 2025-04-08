namespace ParticleMachineWF
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._mainSplitter = new System.Windows.Forms.SplitContainer();
            this._framesCounter = new System.Windows.Forms.Label();
            this._fpsLbl = new System.Windows.Forms.Label();
            this._screen = new ParticleMachineWF.Screen();
            this._timer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this._mainSplitter)).BeginInit();
            this._mainSplitter.Panel1.SuspendLayout();
            this._mainSplitter.Panel2.SuspendLayout();
            this._mainSplitter.SuspendLayout();
            this.SuspendLayout();
            // 
            // _mainSplitter
            // 
            this._mainSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this._mainSplitter.IsSplitterFixed = true;
            this._mainSplitter.Location = new System.Drawing.Point(0, 0);
            this._mainSplitter.Name = "_mainSplitter";
            this._mainSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // _mainSplitter.Panel1
            // 
            this._mainSplitter.Panel1.Controls.Add(this._framesCounter);
            this._mainSplitter.Panel1.Controls.Add(this._fpsLbl);
            // 
            // _mainSplitter.Panel2
            // 
            this._mainSplitter.Panel2.Controls.Add(this._screen);
            this._mainSplitter.Size = new System.Drawing.Size(1395, 636);
            this._mainSplitter.SplitterDistance = 31;
            this._mainSplitter.TabIndex = 2;
            // 
            // _framesCounter
            // 
            this._framesCounter.Dock = System.Windows.Forms.DockStyle.Left;
            this._framesCounter.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._framesCounter.Location = new System.Drawing.Point(262, 0);
            this._framesCounter.Name = "_framesCounter";
            this._framesCounter.Size = new System.Drawing.Size(170, 31);
            this._framesCounter.TabIndex = 1;
            this._framesCounter.Text = "Frame:";
            this._framesCounter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _fpsLbl
            // 
            this._fpsLbl.Dock = System.Windows.Forms.DockStyle.Left;
            this._fpsLbl.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._fpsLbl.Location = new System.Drawing.Point(0, 0);
            this._fpsLbl.Name = "_fpsLbl";
            this._fpsLbl.Size = new System.Drawing.Size(262, 31);
            this._fpsLbl.TabIndex = 0;
            this._fpsLbl.Text = "FPS:";
            this._fpsLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _screen
            // 
            this._screen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(13)))), ((int)(((byte)(56)))));
            this._screen.Dock = System.Windows.Forms.DockStyle.Fill;
            this._screen.Location = new System.Drawing.Point(0, 0);
            this._screen.Name = "_screen";
            this._screen.Renderer = null;
            this._screen.Size = new System.Drawing.Size(1395, 601);
            this._screen.TabIndex = 0;
            this._screen.Text = "screen2";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1395, 636);
            this.Controls.Add(this._mainSplitter);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this._mainSplitter.Panel1.ResumeLayout(false);
            this._mainSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._mainSplitter)).EndInit();
            this._mainSplitter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Screen screen1;
        private System.Windows.Forms.SplitContainer _mainSplitter;
        private System.Windows.Forms.Label _fpsLbl;
        private System.Windows.Forms.Label _framesCounter;
        private Screen _screen;
        private System.Windows.Forms.Timer _timer;
    }
}

