
namespace PawnGame
{
    partial class GameVisuals
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
            this.turnLabel = new System.Windows.Forms.Label();
            this.UndoMove = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // turnLabel
            // 
            this.turnLabel.AutoSize = true;
            this.turnLabel.Location = new System.Drawing.Point(914, 65);
            this.turnLabel.Name = "turnLabel";
            this.turnLabel.Size = new System.Drawing.Size(0, 13);
            this.turnLabel.TabIndex = 0;
            // 
            // UndoMove
            // 
            this.UndoMove.Location = new System.Drawing.Point(942, 197);
            this.UndoMove.Name = "UndoMove";
            this.UndoMove.Size = new System.Drawing.Size(111, 23);
            this.UndoMove.TabIndex = 1;
            this.UndoMove.Text = "Undo Last Move";
            this.UndoMove.UseVisualStyleBackColor = true;
            this.UndoMove.Click += new System.EventHandler(this.UndoMoveVisual);
            // 
            // GameVisuals
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 961);
            this.Controls.Add(this.UndoMove);
            this.Controls.Add(this.turnLabel);
            this.Name = "GameVisuals";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label turnLabel;
        private System.Windows.Forms.Button UndoMove;
    }
}

