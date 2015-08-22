using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RevitCommentEditor
{
    /// <summary>
    /// Interaction logic for CommentEditorDialog.xaml
    /// </summary>
    public partial class CommentEditorDialog : UserControl
    {
        public CommentEditorDialog()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return CommentText.Text; }
            set { CommentText.Text = value; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).DialogResult = true;
            Window.GetWindow(this).Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).DialogResult = false;
            Window.GetWindow(this).Close();
        }
    }
}
