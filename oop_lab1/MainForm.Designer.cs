namespace todoapp_1;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;
    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components =
            new System.ComponentModel.Container();
        AutoScaleMode =
            System.Windows.Forms.AutoScaleMode.Font;
        ClientSize =
            new System.Drawing.Size(
                1200,
                700
            );

        Text = "Список дел";
    }

    #endregion
}