public class IconService
{
    private Icon mutedIcon;
    private Icon liveIcon;
    private Icon currentState;

    public IconService()
    {
        mutedIcon = CreateIcon(Color.Black);
        liveIcon = CreateIcon(Color.Red);
        currentState = liveIcon;
    }

    private Icon CreateIcon(Color color)
    {
        var bitmap = new Bitmap(16, 16);
        using (var graphics = Graphics.FromImage(bitmap))
        {
            var font = new Font("Segoe UI Symbol", 8);
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            graphics.DrawString("\u25CF", font, new SolidBrush(color), -2, -2);
        }

        var iconHandle = bitmap.GetHicon();
        return Icon.FromHandle(iconHandle);
    }

    public Icon GetCurrentState()
    {
        return currentState;
    }

    public void SetState(bool isMuted)
    {
        currentState = isMuted ? mutedIcon : liveIcon;
    }
}
