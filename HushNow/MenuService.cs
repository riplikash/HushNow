public class MenuService
{
    private ContextMenuStrip trayMenu;
    private ToolStripMenuItem muteMenuItem;
    private ToolStripMenuItem devicesMenuItem;
    private ToolStripMenuItem hotKeysMenuItem;

    public MenuService()
    {
        InitializeTrayMenu();
    }

    private void InitializeTrayMenu()
    {
        trayMenu = new ContextMenuStrip();

        muteMenuItem = new ToolStripMenuItem("Mute");
        trayMenu.Items.Add(muteMenuItem);

        devicesMenuItem = new ToolStripMenuItem("Devices");
        trayMenu.Items.Add(devicesMenuItem);

        hotKeysMenuItem = new ToolStripMenuItem("HotKeys");
        trayMenu.Items.Add(hotKeysMenuItem);

        var closeMenuItem = new ToolStripMenuItem("Exit");
        closeMenuItem.Click += OnExit;
        trayMenu.Items.Add(closeMenuItem);
    }

    public ContextMenuStrip GetTrayMenu()
    {
        return trayMenu;
    }

    public ToolStripMenuItem GetMuteMenuItem()
    {
        return muteMenuItem;
    }

    public ToolStripMenuItem GetDevicesMenuItem()
    {
        return devicesMenuItem;
    }

    public ToolStripMenuItem GetHotKeysMenuItem()
    {
        return hotKeysMenuItem;
    }

    private void OnExit(object sender, EventArgs e)
    {
        Application.Exit();
    }
}