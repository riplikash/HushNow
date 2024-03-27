using System;
using System.Windows.Forms;

public class HushNowTrayApp : Form
{
    private NotifyIcon trayIcon;
    private MicrophoneService microphoneService;
    private IconService iconService;
    private MenuService menuService; // Added MenuService

    public HushNowTrayApp()
    {
        menuService = new MenuService(); // Initialize MenuService
        InitializeTrayIcon();
        InitializeMicrophoneService();
        RefreshDevices();
    }

    private void InitializeTrayIcon()
    {
        iconService = new IconService();

        trayIcon = new NotifyIcon()
        {
            Icon = iconService.GetCurrentState(),
            ContextMenuStrip = menuService.GetTrayMenu(), // Get the tray menu from MenuService
            Visible = true
        };

        // Handle the MouseClick event
        trayIcon.MouseClick += TrayIcon_MouseClick;
    }

    private void InitializeMicrophoneService()
    {
        microphoneService = new MicrophoneService();
    }

    private void RefreshDevices()
    {
        microphoneService.RefreshMicrophones();
        var microphones = microphoneService.GetMicrophones().ToList();
        if (microphones.Any())
        {
            PopulateDevicesMenu(microphones);
            UpdateMuteMenuItem();
        }
        else
        {
            HandleNoMicrophonesFound();
        }
    }

    private void PopulateDevicesMenu(List<MicrophoneEntity> microphones)
    {
        var devicesMenuItem = menuService.GetDevicesMenuItem(); // Get the devices menu item from MenuService
        devicesMenuItem.DropDownItems.Clear();

        // Add an option to set the microphone to the system default
        var defaultMenuItem = new ToolStripMenuItem("System Default");
        defaultMenuItem.Click += (sender, e) => SetMicrophone(microphoneService.GetDefaultMicrophone());
        devicesMenuItem.DropDownItems.Add(defaultMenuItem);

        foreach (var microphoneEntity in microphones)
        {
            var deviceMenuItem = new ToolStripMenuItem(microphoneEntity.Microphone.FriendlyName);
            deviceMenuItem.Click += (sender, e) => SetMicrophone(microphoneEntity);
            // Check if the current microphone is the one being added to the menu
            if (microphoneService.IsCurrentMicrophone(microphoneEntity))
            {
                deviceMenuItem.Checked = true; // Add a checkmark or other visual indicator
            }
            devicesMenuItem.DropDownItems.Add(deviceMenuItem);
        }
    }

    private void HandleNoMicrophonesFound()
    {
        MessageBox.Show("No microphones found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        menuService.GetMuteMenuItem().Enabled = false; // Get the mute menu item from MenuService
        menuService.GetDevicesMenuItem().Enabled = false; // Get the devices menu item from MenuService
    }

    private void SetMicrophone(MicrophoneEntity microphoneEntity)
    {
        microphoneService.SetMicrophone(microphoneEntity);
        UpdateMuteMenuItem();
        RefreshDevices();
    }

    private void ToggleMute(object sender, EventArgs e)
    {
        if (microphoneService.CurrentMicrophone.IsMuted())
        {
            OnUnmute(sender, e);
        }
        else
        {
            OnMute(sender, e);
        }
    }

    private void OnMute(object sender, EventArgs e)
    {
        microphoneService.CurrentMicrophone.Mute();
        UpdateMuteMenuItem();
    }

    private void OnUnmute(object sender, EventArgs e)
    {
        microphoneService.CurrentMicrophone.Unmute();
        UpdateMuteMenuItem();
    }

    private void UpdateMuteMenuItem()
    {
        var isMuted = microphoneService.CurrentMicrophone.IsMuted();
        menuService.GetMuteMenuItem().Checked = isMuted; // Get the mute menu item from MenuService
        iconService.SetState(isMuted);
        trayIcon.Icon = iconService.GetCurrentState();
    }

    private void OnHotKeys(object sender, EventArgs e)
    {
        // Open HotkeySetup form here
    }

    protected override void OnLoad(EventArgs e)
    {
        Visible = false;
        ShowInTaskbar = false;
        base.OnLoad(e);
    }

    private void OnExit(object sender, EventArgs e)
    {
        Application.Exit();
    }

    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            trayIcon.Dispose();
        }

        base.Dispose(isDisposing);
    }
    private void TrayIcon_MouseClick(object sender, MouseEventArgs e)
    {
        // Check if the left mouse button was clicked
        if (e.Button == MouseButtons.Left)
        {
            // Toggle the mute status
            ToggleMute(sender, e);
        }
    }
}
