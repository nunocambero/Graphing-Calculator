using System;
using System.Drawing;
using System.Windows.Forms;

namespace Graph_1
{
    public partial class MainApp : System.Windows.Forms.Form
    {
        public MainApp()
        {
            InitializeComponent();
            MainBar mainAppToolBar = new MainBar();
            Controls.Add(mainAppToolBar);
            AddControls();
        }

        public class MainBar : ToolBar
        {
            public MainBar()
            {
                Dock = DockStyle.Top;
                BackColor = Color.FromArgb(32, 32, 32);
                ForeColor = Color.White;
                Height = 25; 
                ToolBarButton fileButton = new ToolBarButton("File");
                ToolBarButton editButton = new ToolBarButton("Edit");
                ToolBarButton viewButton = new ToolBarButton("View");
                ToolBarButton helpButton = new ToolBarButton("Help");

                Buttons.Add(fileButton);
                Buttons.Add(editButton);
                Buttons.Add(viewButton);
                Buttons.Add(helpButton);

                ButtonSize = new Size(50, 20); 
                ButtonClick += new ToolBarButtonClickEventHandler(ToolBar_ButtonClick);
            }

            private void ToolBar_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
            {
                MainApp mainApp = (MainApp)this.FindForm();
                switch (e.Button.Text)
                {
                    case "File":
                        mainApp.File_Click(sender, e);
                        break;
                    case "Edit":
                        mainApp.Edit_Click(sender, e);
                        break;
                    case "View":
                        mainApp.View_Click(sender, e);
                        break;
                    case "Help":
                        mainApp.Help_Click(sender, e);
                        break;
                }
            }

            public class FileMenu : ContextMenu
            {
                public FileMenu()
                {
                    MenuItems.Add("New");
                    MenuItems.Add("Open");
                    MenuItems.Add("Save");
                    MenuItems.Add("Save As");
                    MenuItems.Add("Exit");
                }
            }

            public class EditMenu : ContextMenu
            {
                public EditMenu()
                {
                    MenuItems.Add("Undo");
                    MenuItems.Add("Redo");
                    MenuItems.Add("Cut");
                    MenuItems.Add("Copy");
                    MenuItems.Add("Paste");
                }
            }

            public class ViewMenu : ContextMenu
            {
                public ViewMenu()
                {
                    MenuItems.Add("Zoom In");
                    MenuItems.Add("Zoom Out");
                    MenuItems.Add("Full Screen");
                }
            }

            public class HelpMenu : ContextMenu
            {
                public HelpMenu()
                {
                    MenuItems.Add("About");
                    MenuItems.Add("Help");
                }
            }
        }

        private void File_Click(object sender, EventArgs e)
        {
            MainBar.FileMenu fileMenu = new MainBar.FileMenu();
            fileMenu.Show(this, new Point(0, ((MainBar)Controls[0]).Height)); 
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            MainBar.EditMenu editMenu = new MainBar.EditMenu();
            editMenu.Show(this, new Point(0, ((MainBar)Controls[0]).Height));
        }

        private void View_Click(object sender, EventArgs e)
        {
            MainBar.ViewMenu viewMenu = new MainBar.ViewMenu();
            viewMenu.Show(this, new Point(0, ((MainBar)Controls[0]).Height));
        }

        private void Help_Click(object sender, EventArgs e)
        {
            MainBar.HelpMenu helpMenu = new MainBar.HelpMenu();
            helpMenu.Show(this, new Point(0, ((MainBar)Controls[0]).Height));
        }

        private void AddControls()
        {
            Label prompt = new Label();
            prompt.Text = "Enter the desired function to graph: ";
            prompt.Location = new Point(10, 90);
            prompt.Size = new Size(200, 20);
            prompt.ForeColor = Color.White;
            Controls.Add(prompt);

            TextBox inputBox = new TextBox();
            inputBox.Name = "inputBox";
            inputBox.Location = new Point(220, 90);
            inputBox.Size = new Size(200, 20);
            Controls.Add(inputBox);

            Button plotButton = new Button();
            plotButton.Text = "Plot";
            plotButton.Location = new Point(430, 80);
            plotButton.Size = new Size(80, 40);
            plotButton.Click += PlotButton_Click;
            Controls.Add(plotButton);

            FlowLayoutPanel buttonPanel = new FlowLayoutPanel();
            buttonPanel.Location = new Point(10, 150); 
            buttonPanel.Size = new Size(ClientSize.Width / 2, ClientSize.Height);   
            buttonPanel.FlowDirection = FlowDirection.LeftToRight;
            buttonPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Controls.Add(buttonPanel);

            string[] buttons = { "Sin", "Cos", "Tan", "Log", "Ln", "Sqrt", "^", "(", ")", "x", "y", "+", "-", "*", "/" };
            foreach (string text in buttons)
            {
                Button button = new Button();
                button.Text = text;
                button.Size = new Size(buttonPanel.Width / 5, 40);
                button.Click += (sender, e) => inputBox.Text += text;
                buttonPanel.Controls.Add(button);
            }

            FlowLayoutPanel numberPanel = new FlowLayoutPanel();
            numberPanel.Location = new Point(ClientSize.Width / 3 * 2 + 10, 150);
            numberPanel.Size = new Size(ClientSize.Width / 3 - 10, ClientSize.Height);
            numberPanel.FlowDirection = FlowDirection.LeftToRight;
            numberPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Controls.Add(numberPanel);

            string[] numbers = { "7", "8", "9", "4", "5", "6", "1", "2", "3", "0", ".", "="};
            foreach (string text in numbers)
            {
                Button button = new Button();
                button.Text = text;
                button.Size = new Size(numberPanel.Width / 3 - 10 , 60);
                button.Click += (sender, e) => inputBox.Text += text;
                numberPanel.Controls.Add(button);
            }

            this.Resize += MainApp_Resize;
        }

        private void MainApp_Resize(object sender, EventArgs e)
        {
            foreach (Control control in Controls)
            {
                if (control is FlowLayoutPanel panel)
                {
                    foreach (Control btn in panel.Controls)
                    {
                        btn.Width = (panel.ClientSize.Width - 20) / panel.Controls.Count;
                        btn.Height = (panel.ClientSize.Height - 20) / (panel.Controls.Count / 2);
                    }
                }
            }
        }

        private void PlotButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Plotting function...");
            string functionText = ((TextBox)Controls["inputBox"]).Text;
            GraphDisplay Graph = new GraphDisplay(functionText);
            Graph.Show();
        }

        private void Form0_Load(object sender, EventArgs e)
        {

        }
    }
}
